﻿using MCup.Model;
using MCup.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MCup.Views;
using Xamarin.Forms;

namespace MCup.ModelView
{
    public class PropostaRichiestaModelView : INotifyPropertyChanged
    {
        private bool isvisible, isbusy, isvisibleButton, isenabled, isbusyV;
        private string esito;
        private Assistito contatto;
        private AppuntamentiConfermati appuntamentiConfermati = new AppuntamentiConfermati();
        private string visible = "true";
        private string visibleHome = "false";
        private Impegnativa impegnativa;
        private PropostaRichiesta propostaRichiesta;
        public ICommand AnnullaPrenotazione { protected set; get; }

        private AppuntamentoProposto appuntamentoProposto=new AppuntamentoProposto();
        private List<Header> headers = new List<Header>();

        public event PropertyChangedEventHandler PropertyChanged;

        private List<Prestazione> prestazioni;

        public List<AppuntamentoPrestazioneProposto> ListPrenotazioni
        {
            get { return appuntamentoProposto.appuntamenti; }
            set
            {
                OnPropertyChanged();
                appuntamentoProposto.appuntamenti = value;
            }
        }

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        public string Visible
        {
            get { return visible; }
            set
            {
                OnPropertyChanged();
                visible = value;
            }
        }
        public string VisibleHome
        {
            get { return visibleHome; }
            set
            {
                OnPropertyChanged();
                visibleHome = value;
            }
        }

        public ICommand InvioDatiPerPrenotazione
        {
            get
            {
                return new Command(async () =>
               {
                   IsEnabled = false;
                   await invioDatiPrenotazione();
                   IsEnabled = true;
               });
            }
        }

        public ICommand TornaAllaHome
        {
            get
            {
                return new Command(async () =>
               {
                   IsEnabled = false;
                   App.Current.MainPage = new NavigationPage(new MenuPrincipale());
                   IsEnabled = true;
               });
            }
        }

        public ICommand cambiaData
        {
            get
            {
                return new Command(async (e) =>
                {
                    if (impegnativa.classePriorita == "P")
                    {
                        IsEnabled = false;
                        propostaRichiesta.visualizzaDatePicker();
                    }
                    else
                    {
                        var responseDisplayAlert = await App.Current.MainPage.DisplayAlert("Attenzione",
                            "Sei sicuro di voler cambiare? Se confermi perderai la priorità assegnata dal tuo medico curante",
                            "SI", "NO");
                        if (responseDisplayAlert)
                        {
                            IsEnabled = false;
                            propostaRichiesta.visualizzaDatePicker();
                        }
                    }
                    
                });
            }
        }

        public bool IsEnabled
        {
            get { return isenabled; }
            set
            {
                OnPropertyChanged();
                isenabled = value;
            }
        }

        public ICommand cambiaOra
        {
            get
            {
                return new Command(async () =>
                {
                    if (impegnativa.classePriorita == "P")
                    {
                        IsEnabled = false;
                        IsBusyV = true;
                        IsBusyV = false;
                        await info(appuntamentoProposto);
                        Device.StartTimer(TimeSpan.FromSeconds(3), () =>
                        {
                            IsEnabled = true;
                            return false;
                        });
                    }
                    else
                    {
                        var responseDisplayAlert = await App.Current.MainPage.DisplayAlert("Attenzione",
                            "Sei sicuro di voler cambiare? Se confermi perderai la priorità assegnata dal tuo medico curante",
                            "SI", "NO");
                        if (responseDisplayAlert)
                        {
                            IsEnabled = false;
                            IsBusyV = true;
                            IsBusyV = false;
                            await info(appuntamentoProposto);
                            Device.StartTimer(TimeSpan.FromSeconds(3), () =>
                            {
                                IsEnabled = true;
                                return false;
                            });
                        }
                    }
                   
                });
            }
        }

        public bool IsVisible
        {
            get { return isvisible; }
            set
            {
                OnPropertyChanged();
                isvisible = value;
            }
        }

        public bool IsBusy
        {
            get { return isbusy; }
            set
            {
                OnPropertyChanged();
                isbusy = value;
            }
        }

        public bool IsBusyV
        {
            get { return isbusyV; }
            set
            {
                OnPropertyChanged();
                isbusyV = value;
            }
        }

        public bool IsVisibleButton
        {
            get { return isvisibleButton; }
            set
            {
                OnPropertyChanged();
                isvisibleButton = value;
            }
        }

        public PropostaRichiestaModelView(Impegnativa ricetta,List<Prestazione> prestazioni, Assistito contatto, PropostaRichiesta proposta)
        {
            IsEnabled = true;
            this.impegnativa = ricetta;
            propostaRichiesta = proposta;
            this.contatto = contatto;
            IsVisibleButton = false;
            IsVisible = true;
            IsBusy = true;
            this.prestazioni = prestazioni;
            headers.Add(new Header("struttura","030001"));
            headers.Add(new Header("dataRicerca",""));
            headers.Add(new Header("x-access-token",App.Current.Properties["tokenLogin"].ToString()));
            recuperoInformazioni();
            AnnullaPrenotazione = new Command(async () =>
            {

                var responseDisplayAlert = await App.Current.MainPage.DisplayAlert("Attenzione", "Sei sicuro di voler annullare la prenotazione?", "si",
                    "no");
                if (responseDisplayAlert)
                {
                    REST<object, string> connessioneAnnullamento = new REST<object, string>();
                    string messaggioDiAnnullamento = await connessioneAnnullamento.getString(SingletonURL.Instance.getRotte().annullaPrenotazioneSospesa,
                        headers);
                    if (connessioneAnnullamento.responseMessage != HttpStatusCode.OK)
                    {
                        await App.Current.MainPage.DisplayAlert("Attenzione " + (int)connessioneAnnullamento.responseMessage, connessioneAnnullamento.warning, "OK");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Attenzione", messaggioDiAnnullamento, "ok");
                        App.Current.MainPage = new MenuPrincipale();
                    }
                }
            });
        }

        private async void recuperoInformazioni()
        {
            await info();
        }

        public async Task invioDatiPrenotazione()
        {
            var noteAccettate = false;
            string note = "";
            for(int i = 0; i < appuntamentoProposto.appuntamenti.Count; i++)
                if (!(string.IsNullOrEmpty(appuntamentoProposto.appuntamenti[i].nota)))
                    note += appuntamentoProposto.appuntamenti[i].nota + '\n';
            if (string.IsNullOrEmpty(note))
                noteAccettate = await App.Current.MainPage.DisplayAlert("Conferma Prenotazione", "Sicuro di voler confermare la prenotazione?", "SI", "NO");
            else
                noteAccettate = await App.Current.MainPage.DisplayAlert("Conferma Prenotazione", note, "SI", "NO");
            if (noteAccettate)
            {
                REST<AppuntamentoProposto, AppuntamentiConfermati> invioDati = new REST<AppuntamentoProposto, AppuntamentiConfermati>();
                appuntamentoProposto.assistito = contatto;
                appuntamentoProposto.codiceImpegnativa = impegnativa.nre;
                appuntamentoProposto.classePriorita = impegnativa.classePriorita;
                appuntamentoProposto.dataEmissioneRicetta = impegnativa.dataEmissioneRicetta;
                try
                {
                    IsBusyV = true;
                    AppuntamentiConfermati appuntamentiConfermati = await invioDati.PostJson(SingletonURL.Instance.getRotte().ConfermaPrenotazione, appuntamentoProposto, headers);
                    if ((invioDati.responseMessage != HttpStatusCode.Created) || (invioDati.responseMessage != HttpStatusCode.InternalServerError)||(invioDati.responseMessage != HttpStatusCode.NotFound))
                    {
                        await App.Current.MainPage.DisplayAlert("Attenzione " + (int)invioDati.responseMessage, invioDati.warning, "OK");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Appuntamento Confermato", appuntamentiConfermati.messaggio, "ok");
                        IsBusyV = false;
                        Visible = "false";
                        VisibleHome = "true";
                        if (appuntamentiConfermati.esito != 0)
                            App.Current.MainPage = new MenuPrincipale();
                    }
                 
                }
                catch (Exception)
                {
                    await App.Current.MainPage.DisplayAlert("Attenzione", "connessione non riuscita", "ok");
                }
            }
        }

        private async Task info()
        {
            REST<List<Prestazione>, AppuntamentoProposto> recuperoDatiLista = new REST<List<Prestazione>, AppuntamentoProposto>();
            appuntamentoProposto =await recuperoDatiLista.PostJson(SingletonURL.Instance.getRotte().PrimaDisponibilita, prestazioni, headers);
            if (recuperoDatiLista.responseMessage != HttpStatusCode.OK)
            {
                await App.Current.MainPage.DisplayAlert("Attenzione " + (int)recuperoDatiLista.responseMessage, recuperoDatiLista.warning, "OK");
            }
            else
            {
                ListPrenotazioni = appuntamentoProposto.appuntamenti;
                IsVisible = false;
                IsBusy = false;
                IsVisibleButton = true;
            }
           
        }

        private async Task info(AppuntamentoProposto prenotazione)
        {
            REST<AppuntamentoProposto, AppuntamentoProposto> connessione = new REST<AppuntamentoProposto, AppuntamentoProposto>();
            IsBusyV = true;
            appuntamentoProposto = await connessione.PostJson(SingletonURL.Instance.getRotte().PrimaDisponibilitaOra, prenotazione, headers);
            if (connessione.responseMessage != HttpStatusCode.OK)
            {
                await App.Current.MainPage.DisplayAlert("Attenzione " + (int)connessione.responseMessage, connessione.warning, "OK");
            }
            else
            {
                IsBusyV = false;
                ListPrenotazioni = appuntamentoProposto.appuntamenti;
            }
           
        }

        public async Task infoProssimaData(string data)
        {
            REST<AppuntamentoProposto, AppuntamentoProposto> connessione = new REST<AppuntamentoProposto, AppuntamentoProposto>();
            IsBusyV = true;
            headers[1].value = data;
            appuntamentoProposto = await connessione.PostJson(SingletonURL.Instance.getRotte().ricercadata, appuntamentoProposto, headers);
            if (connessione.responseMessage != HttpStatusCode.OK)
            {
                await App.Current.MainPage.DisplayAlert("Attenzione " + (int)connessione.responseMessage, connessione.warning, "OK");
            }
            else
            {
                IsBusyV = false;
                ListPrenotazioni = appuntamentoProposto.appuntamenti;
            }
           
        }
    }
}
