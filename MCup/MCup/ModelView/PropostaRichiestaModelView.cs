using MCup.Model;
using MCup.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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
        private PropostaRichiesta propostaRichiesta;
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
                    IsEnabled = false;
                    propostaRichiesta.visualizzaDatePicker();
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
                    IsEnabled = false;
                    IsBusyV = true;
                    IsBusyV = false;
                    await info(appuntamentoProposto);
                    Device.StartTimer(TimeSpan.FromSeconds(3), () =>
                    {
                        IsEnabled = true;
                        return false;
                    });
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

        public PropostaRichiestaModelView(List<Prestazione> prestazioni, Assistito contatto, PropostaRichiesta proposta)
        {
            IsEnabled = true;
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
        }

        private async void recuperoInformazioni()
        {
            await info();
        }

        public async Task invioDatiPrenotazione()
        {
            REST<AppuntamentoProposto, AppuntamentiConfermati> invioDati = new REST<AppuntamentoProposto, AppuntamentiConfermati>();
            appuntamentoProposto.assistito = contatto;
            try
            {
                IsBusyV = true;
                AppuntamentiConfermati appuntamentiConfermati = await invioDati.PostJson(URL.ConfermaPrenotazione, appuntamentoProposto, headers);
                await App.Current.MainPage.DisplayAlert("Attenzione", appuntamentiConfermati.messaggio, "ok");
                IsBusyV = false;
                Visible = "false";
                VisibleHome = "true";
                if (appuntamentiConfermati.esito != 0)
                    App.Current.MainPage = new MenuPrincipale();
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert("Attenzione", "connessione non riuscita", "ok");
            }

        }

        private async Task info()
        {
            REST<List<Prestazione>, AppuntamentoProposto> recuperoDatiLista = new REST<List<Prestazione>, AppuntamentoProposto>();
            appuntamentoProposto =await recuperoDatiLista.PostJson(URL.PrimaDisponibilita, prestazioni, headers);
            ListPrenotazioni = appuntamentoProposto.appuntamenti;
            IsVisible = false;
            IsBusy = false;
            IsVisibleButton = true;
        }

        private async Task info(AppuntamentoProposto prenotazione)
        {
            REST<AppuntamentoProposto, AppuntamentoProposto> connessione = new REST<AppuntamentoProposto, AppuntamentoProposto>();
            IsBusyV = true;
            appuntamentoProposto = await connessione.PostJson(URL.PrimaDisponibilitaOra, prenotazione, headers);
            IsBusyV = false;
            ListPrenotazioni = appuntamentoProposto.appuntamenti;
        }

        public async Task infoProssimaData(string data)
        {
            REST<AppuntamentoProposto, AppuntamentoProposto> connessione = new REST<AppuntamentoProposto, AppuntamentoProposto>();
            IsBusyV = true;
            headers[1].value = data;
            appuntamentoProposto = await connessione.PostJson(URL.ricercadata, appuntamentoProposto, headers);
            IsBusyV = false;
            ListPrenotazioni = appuntamentoProposto.appuntamenti;
        }
    }
}
