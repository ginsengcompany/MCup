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
        private List<PrenotazioneProposta> listPrenotazioni;
        private bool isvisible, isbusy, isvisibleButton, isenabled, isbusyV;
        private string esito;
        private Contatto contatto;
        private List<ResponsePrenotazione> listaResponsePrenotazioni = new List<ResponsePrenotazione>();
        private string visible = "true";
        private string visibleHome = "false";
        private PropostaRichiesta propostaRichiesta;
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
                    var item = (e as PrenotazioneProposta);
                    propostaRichiesta.visualizzaDatePicker(item);
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
                return new Command(async (e) =>
                {

                    IsEnabled = false;
                    IsBusyV = true;
                    var item = (e as PrenotazioneProposta);
                    IsBusyV = false;
                    await info(item);
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

        public event PropertyChangedEventHandler PropertyChanged;

        private List<Prestazioni> prestazioni;

        public List<PrenotazioneProposta> ListPrenotazioni
        {
            get { return listPrenotazioni; }
            set
            {
                OnPropertyChanged();
                listPrenotazioni = value;
            }
        }

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public PropostaRichiestaModelView(List<Prestazioni> prestazioni, Contatto contatto, PropostaRichiesta proposta)
        {
            IsEnabled = true;
            propostaRichiesta = proposta;
            this.contatto = contatto;
            listPrenotazioni = new List<PrenotazioneProposta>();
            IsVisibleButton = false;
            IsVisible = true;
            IsBusy = true;
            this.prestazioni = prestazioni;
            recuperoInformazioni();
        }

        private async void recuperoInformazioni()
        {
            await info();

        }

        public async Task invioDatiPrenotazione()
        {
            REST<PrenotazioniContatto, ResponsePrenotazione> invioDati = new REST<PrenotazioniContatto, ResponsePrenotazione>();
            PrenotazioniContatto prenotazioni;
            List<ResponsePrenotazione> listaPrenotazioniNonAndateABuonFine = new List<ResponsePrenotazione>();
            List<ResponsePrenotazione> listaPrenotazioniAndateABuonFine = new List<ResponsePrenotazione>();
            List<PrenotazioneProposta> temp = new List<PrenotazioneProposta>();
            PrenotazioneProposta pren = new PrenotazioneProposta();

            try
            {
                for (int i = 0; i < ListPrenotazioni.Count; i++)
                {
                    prenotazioni = new PrenotazioniContatto();
                    prenotazioni.contatto = contatto;
                    prenotazioni.prestazione = ListPrenotazioni[i];
                    IsBusyV = true;
                    listaResponsePrenotazioni.Add(await invioDati.PostJson(URL.ConfermaPrenotazione, prenotazioni, App.Current.Properties["tokenLogin"].ToString()));
                    IsBusyV = false;
                }
                for (int j = 0; j < listaResponsePrenotazioni.Count; j++)
                {
                    if (listaResponsePrenotazioni[j].esito == 0)
                    {
                        listaPrenotazioniNonAndateABuonFine.Add(listaResponsePrenotazioni[j]);
                        pren = listPrenotazioni[j];
                        pren.VisibleEsito = "true";
                        pren.VisibleButton = "false";
                        pren.EsitoPrenotazione = listaResponsePrenotazioni[j].messaggio;
                        pren.Color = Color.Red;
                    }
                    else if (listaResponsePrenotazioni[j].esito == 1)
                    {
                        listaPrenotazioniAndateABuonFine.Add(listaResponsePrenotazioni[j]);
                        pren = listPrenotazioni[j];
                        pren.VisibleEsito = "true";
                        pren.VisibleButton = "false";
                        pren.EsitoPrenotazione = listaResponsePrenotazioni[j].messaggio;
                        pren.Color = Color.Green;
                    }
                    else if (listaResponsePrenotazioni[j].esito == 2)
                    {
                        listaPrenotazioniAndateABuonFine.Add(listaResponsePrenotazioni[j]);
                        pren = listPrenotazioni[j];
                        pren.VisibleEsito = "true";
                        pren.VisibleButton = "false";
                        pren.EsitoPrenotazione = listaResponsePrenotazioni[j].messaggio;
                        pren.Color = Color.Orange;
                    }
                    temp.Add(pren);

                }
                Visible = "false";
                VisibleHome = "true";
                ListPrenotazioni = temp;
            }
            catch (Exception)
            {

                await App.Current.MainPage.DisplayAlert("Attenzione", "connessione non riuscita", "ok");
            }

        }

        private async Task info()
        {
            REST<Prestazioni, PrenotazioneProposta> recuperoDatiLista = new REST<Prestazioni, PrenotazioneProposta>();
            List<PrenotazioneProposta> temp = new List<PrenotazioneProposta>();
            for (int i = 0; i < prestazioni.Count; i++)
            {
                temp.Add(await recuperoDatiLista.PostJson(URL.PrimaDisponibilita, prestazioni[i]));
            }
            ListPrenotazioni = temp;
            IsVisible = false;
            IsBusy = false;
            IsVisibleButton = true;
        }

        private async Task info(PrenotazioneProposta prenotazione)
        {
            REST<Prestazioni, PrenotazioneProposta> connessione = new REST<Prestazioni, PrenotazioneProposta>();
            PrenotazioneProposta nuovaproposta = new PrenotazioneProposta();
            Prestazioni prestazione = new Prestazioni();
            prestazione.codprest = prenotazione.codPrestazione;
            prestazione.data_inizio = prenotazione.dataAppuntamento;
            prestazione.reparti.codReparto = prenotazione.codReparto;
            prestazione.reparti.unitaOperativa = prenotazione.unitaOperativa;
            prestazione.reparti.nomeStruttura = prenotazione.nomeStruttura;
            IsBusyV = true;
            nuovaproposta = await connessione.PostJson(URL.PrimaDisponibilita, prestazione);
            IsBusyV = false;
            List<PrenotazioneProposta> temp = new List<PrenotazioneProposta>(ListPrenotazioni);
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].codPrestazione == nuovaproposta.codPrestazione)
                {
                    temp[i] = nuovaproposta;
                    break;
                }
            }
            ListPrenotazioni = temp;
        }

        public async Task infoProssimaData(PrenotazioneProposta prenotazione)
        {
            REST<Prestazioni, PrenotazioneProposta> connessione = new REST<Prestazioni, PrenotazioneProposta>();
            PrenotazioneProposta nuovaproposta = new PrenotazioneProposta();
            Prestazioni prestazione = new Prestazioni();
            prestazione.codprest = prenotazione.codPrestazione;
            prestazione.data_inizio = prenotazione.dataAppuntamento;
            prestazione.reparti.codReparto = prenotazione.codReparto;
            prestazione.reparti.unitaOperativa = prenotazione.unitaOperativa;
            prestazione.reparti.nomeStruttura = prenotazione.nomeStruttura;
            IsBusyV = true;
            nuovaproposta = await connessione.PostJson(URL.PrimaDisponibilita, prestazione);
            IsBusyV = false;
            List<PrenotazioneProposta> temp = new List<PrenotazioneProposta>(ListPrenotazioni);
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].codPrestazione == nuovaproposta.codPrestazione)
                {
                    temp[i] = nuovaproposta;
                    break;
                }
            }
            ListPrenotazioni = temp;
        }

        private class PrenotazioniContatto
        {
            public Contatto contatto { get; set; }
            public PrenotazioneProposta prestazione { get; set; }
            public string struttura { get; set; } = "030001";
        }

        private class ResponsePrenotazione
        {
            public string messaggio { get; set; }
            public int esito { get; set; }

        }

    }
}
