#region LibrerieUsate
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using MCup.Model;
using MCup.Service;
using MCup.Views;
using Xamarin.Forms;
#endregion


namespace MCup.ModelView
{
    public class GestioneAppuntamentiModelView : INotifyPropertyChanged
    {



        #region DichiarazionieInizializzazione

        //Evento che prevede il cambiamento di proprietà all'interno della classe
        public event PropertyChangedEventHandler PropertyChanged;

        //Oggetto che astrae l'utente che intende prenotare una o delle prestazioni        
        private Assistito contatto;

        //Oggetto che astrae la pagina a cui punta il modelView in questione.
        private GestioneAppuntamenti pagina;

        //Lista di tipo Appuntamento proposto che conterrà gli appuntamenti dell'utente selezionato
        private List<AppuntamentoProposto> appuntamenti = new List<AppuntamentoProposto>();

        //Oggetto che conterrà le informazioni dell'appuntamento selezionato dall'utente
        private AppuntamentoProposto appuntamentoSelezionato = new AppuntamentoProposto();

        //Booleano che renderà visibile o non la label di informazione sulla presenza o meno degli appuntamenti
        private Boolean visibileLabel = false;

        //Lista di header
        private List<Header> headers = new List<Header>();

        //Lista di tipo Appuntamento proposto che conterrà gli appuntamenti dell'utente selezionato
        // *ci serve per il Binding*
        List<AppuntamentoPrestazioneProposto> appunt = new List<AppuntamentoPrestazioneProposto>();

        //Booleano di controllo per la visibilità degli elementi nello xaml
        private Boolean visibile = true;

        //Variabile usata per la visibilità degli elementi nello xaml
        private string visi;

        #endregion

        #region ProprietaGetSet

        //Comando che richiama il metodo dell'eliminazione di un appuntamento
        public ICommand EliminaAppuntamento
        {
            get
            {
                return new Command(async () =>
                {
                    await EliminazioneAppuntamento();
                });
            }
        }

        //Comando che richiama il metodo dello spostamento di un appuntamento
        public ICommand SpostaAppuntamento
        {
            get
            {
                return new Command(async () =>
                {
                    await SpostaAppuntamentoMethod();
                });
            }
        }

        //Proprietà che conterrà gli appuntamenti dell'utente selezionato
        public List<AppuntamentoPrestazioneProposto> Appunt
        {
            get { return appunt; }
            set
            {
                OnPropertyChanged();
                appunt = value;
            }
        }

        //Proprietà che verrà richiamata nel momento in cui abbiamo bisogno di rendere visibile o meno un'elemento nello xaml
        public string VisibileL
        {
            get { return visi; }
            set
            {
                OnPropertyChanged();
                visi = value;
            }
        }
        //Proprietà che verrà richiamata nel momento in cui abbiamo bisogno di rendere visibile o meno un'elemento nello xaml

        public Boolean Visibile
        {
            get { return visibile; }
            set
            {
                OnPropertyChanged();
                visibile = value;
            }
        }

        //Proprietà che verrà richiamata nel momento in cui abbiamo bisogno di rendere visibile o meno un'elemento nello xaml

        public Boolean VisibileLabel
        {
            get { return visibileLabel; }
            set
            {
                OnPropertyChanged();
                visibileLabel = value;
            }
        }

        //Proprietà che verrà usata per il risultato della connessione al server e verrà riempita degli appuntamenti dell'utente

        public List<AppuntamentoProposto> Appuntamenti
        {
            get { return appuntamenti; }
            set
            {
                OnPropertyChanged();
                appuntamenti = value;
            }
        }

        #endregion

        #region Metodi

        //Metodo che implementa l'eliminazione di un appuntamento
        public async Task EliminazioneAppuntamento()
        {
            var messDisplay = "";
            DateTime dataEmissione = Convert.ToDateTime(appuntamentoSelezionato.dataEmissioneRicetta);
            DateTime dataOdierna = DateTime.Today;
            if ((dataEmissione - dataOdierna).TotalDays < 30)
            {
                messDisplay = "Sei sicuro di voler annullare la prenotazione?\nse confermi non sarà più possibile prenotare con questa impegnativa, inquanto la data di emissione dell'impegnativa ha superato i 30 giorni utili per utilizzarla";
            }
            else
                messDisplay = "Sei sicuro di voler annullare la prenotazione?";
            var esitoDisplayAlert = await App.Current.MainPage.DisplayAlert("Attenzione", messDisplay
                , "si", "no");
            REST<AppuntamentoProposto, ResponseAnnullaImpegnativa> connessioneAnnullamentoImpegnativa = new REST<AppuntamentoProposto, ResponseAnnullaImpegnativa>();
            List<Header> headers = new List<Header>();
            headers.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
            if (esitoDisplayAlert)
            {
                try
                {
                    ResponseAnnullaImpegnativa response = await connessioneAnnullamentoImpegnativa.PostJson(SingletonURL.Instance.getRotte().annullaImpegnativa,
                                 appuntamentoSelezionato, headers);
                    if (connessioneAnnullamentoImpegnativa.responseMessage != HttpStatusCode.OK)
                    {
                        await App.Current.MainPage.DisplayAlert("Attenzione " + (int)connessioneAnnullamentoImpegnativa.responseMessage, connessioneAnnullamentoImpegnativa.warning, "OK");
                    }
                    else
                    {
                        pagina.PopAsync();
                    }
                }
                catch (Exception)
                {

                    await App.Current.MainPage.DisplayAlert("Mcup", "Connessione non riuscita", "ok");

                }

            }

        }

        //Metodo che implementa lo spostamento di un appuntamento
        public async Task SpostaAppuntamentoMethod()
        {
            try
            {
                REST<object, string> connessioneSpostamento = new REST<object, string>();
                string messaggioDalServer = await connessioneSpostamento.getString(SingletonURL.Instance.getRotte().spostamentoPrenotazione,
                        headers);
                if (connessioneSpostamento.responseMessage != HttpStatusCode.OK)
                {
                    await App.Current.MainPage.DisplayAlert("Attenzione " + (int)connessioneSpostamento.responseMessage, connessioneSpostamento.warning, "OK");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Attenzione", messaggioDalServer, "ok");
                }

            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert("Attenzione", "connessione non riuscita", "ok");
            }

        }

        //Metodo che tramite una connessione invia al server i dati dell'Utente
        public async Task invioDatiAssistito()
        {
            try
            {
                Appunt = appuntamentoSelezionato.appuntamenti;
                if (Appuntamenti.Count == 0)
                {
                    Visibile = false;
                    VisibileLabel = true;
                }
                else
                {
                    Visibile = true;
                    VisibileL = "true";
                    VisibileLabel = false;
                }
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Attenzione",
                    "connessione non riuscita o codici impegnativa errati", "riprova");
            }
        }
        #endregion

        #region Costruttore

        //Costruttore
        public GestioneAppuntamentiModelView(AppuntamentoProposto appuntamentoSelezionato, GestioneAppuntamenti page)
        {
            VisibileL = "false";
            headers.Add(new Header("struttura", "030001"));
            this.pagina = page;
            this.appuntamentoSelezionato = appuntamentoSelezionato;
            invioDatiAssistito();
        }


        #endregion

        #region OnPropertyChange

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        #endregion


    }

    #region PublicOrPrivateClass

    public class ResponseAnnullaImpegnativa
    {
        public string messaggio { get; set; }
        public bool esito { get; set; }
    }

    #endregion


}
