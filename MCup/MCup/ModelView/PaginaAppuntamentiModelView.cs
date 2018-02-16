#region Librerie

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MCup.Model;
using MCup.Service;
using MCup.Views;
using Xamarin.Forms;

#endregion


namespace MCup.ModelView
{
    public class PaginaAppuntamentiModelView: INotifyPropertyChanged
    {


        #region DichiarazioneVariabili

        public event PropertyChangedEventHandler PropertyChanged;//Evento che tiene traccia dei cambiamenti di stato delle proprietà
        private Assistito contatto;//Oggetto che astrae la classe assistito
        private bool switched;
        private PaginaAppuntamenti paginaAppuntamenti;//Oggetto che astrae la pagina a cui si riferisce il model view
        private List<Assistito> contatti = new List<Assistito>();// lista di contattai di tipo assistito
        private List<AppuntamentoProposto> appuntamenti = new List<AppuntamentoProposto>();//Lista di appuntamenti utilizzato per creare la listview di appuntamenti da far selezionare all'utente
        private AppuntamentoProposto date = new AppuntamentoProposto();//Oggetto di tipo appuntamento proposto contiene i dati dell'appuntamento
        private Boolean visibileLabel = false;//variabile booleana che setta la visibilità o meno di un elemento nello xaml
        List<AppuntamentoPrestazioneProposto> appunt = new List<AppuntamentoPrestazioneProposto>();//Lista che servirà per il binding nello xaml
        private Boolean visibile = true;//variabile booleana che setta la visibilità o meno di un elemento nello xaml
        private string visi;//variabile  che setta la visibilità o meno di un elemento nello xaml
        private bool visibleSwitch = false;

        #endregion

        #region Proprietà

        public List<AppuntamentoPrestazioneProposto> Appunt//Proprietà riferita al campo Appunt
        {
            get { return appunt; }
            set
            {
                OnPropertyChanged();
                appunt = value;
            }
        }

        public bool VisibleSwitch
        {
            get { return visibleSwitch; }
            set
            {
                OnPropertyChanged();
                visibleSwitch = value;
            }
        }
        public string VisibileL//Proprietà riferita al campo VisibleL
        {
            get { return visi; }
            set
            {
                OnPropertyChanged();
                visi = value;
            }
        }
        public Boolean Visibile//Proprietà riferita al campo Visible
        {
            get { return visibile; }
            set
            {
                OnPropertyChanged();
                visibile = value;
            }
        }
        public Boolean VisibileLabel//Proprietà riferita al campo VisibileLabel
        {
            get { return visibileLabel; }
            set
            {
                OnPropertyChanged();
                visibileLabel = value;
            }
        }

        public List<AppuntamentoProposto> Appuntamenti //Proprietà riferita al campo List Appuntamenti
        {
            get { return appuntamenti; }
            set
            {
                OnPropertyChanged();
                appuntamenti = value;
            }
        }

        public List<Assistito> Contatti //Proprietà riferita al campo Contatti
        {
            get { return contatti; }
            set
            {
                OnPropertyChanged();
                contatti = new List<Assistito>(value);
            }
        }



        #endregion

        #region OnPropertyChange

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region Metodi


        public async void recuperaDatiAppuntamentiPassati()
        {
            await invioDatiAssistitoConSwitch();

        }

        //Metodo che implementa l'autocompilazione nel picker della scelta del contatto di cui visualizzare gli appuntamenti
        public async void autoCompila(Assistito elementSelected)
        {
            date.assistito = elementSelected;
            await invioDatiAssistito();
        }

        //Metodo che invia i dati dell'utente per cui si è scelto di visualizzare gli appuntamenti
        public async Task invioDatiAssistito()
        {
            List<Header> listaJHeaders = new List<Header>();
            listaJHeaders.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
            try
            {
                Assistito invioContatto = date.assistito;
                REST<Assistito, AppuntamentoProposto> connessione = new REST<Assistito, AppuntamentoProposto>();
                Appuntamenti = await connessione.PostJsonList(SingletonURL.Instance.getRotte().appuntamenti, invioContatto, listaJHeaders);
                if (connessione.responseMessage != HttpStatusCode.OK)
                {
                    await App.Current.MainPage.DisplayAlert("Attenzione " + (int)connessione.responseMessage, connessione.warning, "OK");
                }
                if (Appuntamenti.Count == 0)
                {
                    Visibile = false;
                    VisibileLabel = true;
                }
                else
                {
                    VisibleSwitch = true;
                    Visibile = true;
                    VisibileL = "true";
                    VisibileLabel = false;
                    foreach (var i in Appuntamenti)
                    {
                        Appunt = i.appuntamenti;
                    }

                }
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Attenzione",
                    "connessione non riuscita o codici impegnativa errati", "riprova");
            }
        }

        public async Task invioDatiAssistitoConSwitch()
        {
            List<Header> listaJHeaders = new List<Header>();
            listaJHeaders.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
            try
            {
                Assistito invioContatto = date.assistito;
                REST<Assistito, AppuntamentoProposto> connessione = new REST<Assistito, AppuntamentoProposto>();
                Appuntamenti = await connessione.PostJsonList(SingletonURL.Instance.getRotte().appuntamentiFuturiEPassati, invioContatto, listaJHeaders);
                if (connessione.responseMessage != HttpStatusCode.OK)
                {
                    await App.Current.MainPage.DisplayAlert("Attenzione " + (int)connessione.responseMessage, connessione.warning, "OK");
                }
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
                    foreach (var i in Appuntamenti)
                    {
                        Appunt = i.appuntamenti;
                    }

                }
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Attenzione",
                    "connessione non riuscita o codici impegnativa errati", "riprova");
            }
        }


        //Metodo che implementa il passaggio da una pagina all'altra
        public async void push(AppuntamentoProposto elementoSelezionato)
        {
            await paginaAppuntamenti.Navigation.PushAsync(new GestioneAppuntamenti(elementoSelezionato));
            Contatti.Clear();
            VisibileLabel = false;
        }

        //Metodo che tramite una connessione riceve i contatti relativi all'utentenza
        private async void leggiContatti()
        {
            List<Header> listaHeader = new List<Header>();
            listaHeader.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
            REST<object, List<Assistito>> rest = new REST<object, List<Assistito>>();
            contatti = await rest.GetSingleJson(SingletonURL.Instance.getRotte().InfoPersonali, listaHeader);
            if (rest.responseMessage != HttpStatusCode.OK)
            {
                await App.Current.MainPage.DisplayAlert("Attenzione " + (int)rest.responseMessage, rest.warning, "OK");
            }
            else
            {
                Contatti = contatti.OrderBy(o => o.nomeCompletoConCodiceFiscale).ToList();
            }
        }
        #endregion

        #region Costruttore

        public PaginaAppuntamentiModelView(PaginaAppuntamenti pagina)
        {
            VisibileL = "false";
            leggiContatti();
            this.paginaAppuntamenti = pagina;
        }

        #endregion

    }
}
