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
using Plugin.Geolocator;
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

        private AppuntamentoPrestazioneProposto appuntamentoSelezionatoProposto;

        //Booleano che renderà visibile o non la label di informazione sulla presenza o meno degli appuntamenti
        private Boolean visibileLabel = false;


        //Lista di header
        private List<Header> headers = new List<Header>();

        //Lista di tipo Appuntamento proposto che conterrà gli appuntamenti dell'utente selezionato
        // *ci serve per il Binding*
        List<AppuntamentoPrestazioneProposto> appunt = new List<AppuntamentoPrestazioneProposto>();

        //Booleano di controllo per la visibilità degli elementi nello xaml
        private Boolean visibile = true;

        private Boolean visibileB = true;
        //Variabile usata per la visibilità degli elementi nello xaml
        private string visi, desprest, nomeStruttura, dataAppuntamento, oraAppuntamento, nomeMedico, ubicazioneReparto;

        #endregion

        #region ProprietaGetSet

        //Comando che richiama il metodo dell'eliminazione di un appuntamento


        public string Titolo
        {
            get { return desprest; }
            set
            {
                OnPropertyChanged();
                desprest = value;
            }
        }

        public string NomeStruttura
        {
            get { return nomeStruttura; }
            set
            {
                OnPropertyChanged();
                nomeStruttura = value;
            }
        }

        public string DataAppuntamento
        {
            get { return dataAppuntamento; }
            set
            {
                OnPropertyChanged();
                dataAppuntamento = value;
            }
        }

        public string OraAppuntamento
        {
            get { return oraAppuntamento; }
            set
            {
                OnPropertyChanged();
                oraAppuntamento = value;
            }
        }

        public string NomeMedico
        {
            get { return nomeMedico; }
            set
            {
                OnPropertyChanged();
                nomeMedico = value;
            }
        }

        public string UbicazioneReparto
        {
            get { return ubicazioneReparto; }
            set
            {
                OnPropertyChanged();
                ubicazioneReparto = value;
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

        public Boolean VisibileB
        {
            get { return visibileB; }
            set
            {
                OnPropertyChanged();
                visibileB = value;
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



        #endregion

        #region Metodi

        //Metodo che implementa l'eliminazione di un appuntamento
     

        //Metodo che tramite una connessione invia al server i dati dell'Utente
      /*  public async Task invioDatiAssistito()
        {
            try
            {
                
                    if (string.IsNullOrEmpty(appuntamentoSelezionatoProposto.reparti[0].latitudine) ||
                        string.IsNullOrEmpty(appuntamentoSelezionatoProposto.reparti[0].longitudine))
                        appuntamentoSelezionatoProposto.reparti[0].visibile = false;

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
        */


        public void IngressoPagina()
        {
            Titolo = appuntamentoSelezionatoProposto.desprest;
            UbicazioneReparto = appuntamentoSelezionatoProposto.reparti[0].ubicazioneReparto;
            NomeStruttura = appuntamentoSelezionatoProposto.reparti[0].nomeStruttura;
            DataAppuntamento = appuntamentoSelezionatoProposto.dataAppuntamento;
            OraAppuntamento = appuntamentoSelezionatoProposto.oraAppuntamento;
            NomeMedico = appuntamentoSelezionatoProposto.reparti[0].nomeMedico;
            if (string.IsNullOrEmpty(appuntamentoSelezionatoProposto.reparti[0].latitudine) || string.IsNullOrEmpty(appuntamentoSelezionatoProposto.reparti[0].longitudine))
                VisibileB = false;
        }

        public ICommand LuogoUbicazioneReparto
        {
            get
            {
                return new Command(async () =>
                {
                   await RiceviLuogo(appuntamentoSelezionatoProposto.reparti[0]);
                });
            }
        }

        private async Task RiceviLuogo(Reparto reparto)
        {
            string url;
            var locator = CrossGeolocator.Current;
            var position = await locator.GetLastKnownLocationAsync();
            if (position != null)
            {

                url = string.Format(
                    "https://www.google.com/maps/dir/?api=1&origin={0},{1}&destination={2},{3}&travelmode=car",
                    position.Latitude.ToString().Replace(',', '.'), position.Longitude.ToString().Replace(',', '.'),
                    reparto.latitudine, reparto.longitudine);
                Device.OpenUri(new Uri(url));
            }
            else
            {
                url = string.Format("https://www.google.com/maps/?q={0},{1}", reparto.latitudine, reparto.longitudine);
                Device.OpenUri(new Uri(url));
            }

        }
        #endregion

        #region Costruttore

        //Costruttore
        public GestioneAppuntamentiModelView(AppuntamentoPrestazioneProposto appuntamentoSelezionato, GestioneAppuntamenti page)
        {
            VisibileL = "false";
            headers.Add(new Header("struttura", "030001"));
            this.pagina = page;
            
            this.appuntamentoSelezionatoProposto = appuntamentoSelezionato;
          //  invioDatiAssistito();
          IngressoPagina();
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
