#region Librerie

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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
    public class PaginaAppuntamentiModelView : INotifyPropertyChanged
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
        private ObservableCollection<VisualizzaAppuntamenti> appunt = new ObservableCollection<VisualizzaAppuntamenti>();//Lista che servirà per il binding nello xaml
        private Boolean visibile = true;//variabile booleana che setta la visibilità o meno di un elemento nello xaml
        private string visi;//variabile  che setta la visibilità o meno di un elemento nello xaml
        private bool visibleSwitch = false;

        #endregion

        #region Proprietà

        public ObservableCollection<VisualizzaAppuntamenti> Appunt//Proprietà riferita al campo Appunt
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
            VisibleSwitch = true;
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
                    if (Appunt.Count != 0)
                    {
                        Appunt.Clear();
                    }
                }
                else
                {
                    VisibleSwitch = true;
                    Visibile = true;
                    VisibileL = "true";
                    VisibileLabel = false;
                    raggruppaLista();
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
                    raggruppaLista();
                }
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Attenzione",
                    "connessione non riuscita o codici impegnativa errati", "riprova");
            }
        }


        //Metodo che implementa il passaggio da una pagina all'altra
        public async void push(AppuntamentoPrestazioneProposto elementoSelezionato)
        {
            await paginaAppuntamenti.Navigation.PushAsync(new GestioneAppuntamenti(elementoSelezionato));
           // Contatti.Clear();
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

        private void raggruppaLista()
        {
            if (Appunt.Count != 0)
            {
                Appunt.Clear();
            }
            for (int i = 0; i < Appuntamenti.Count; i++)
            {/*
                for (int t = 0; t < Appuntamenti.Count - 1; t++)
                {
                    int posmin = t;
                    for (int k = t + 1; k < Appuntamenti.Count; k++)
                    {
                        DateTime date1 = new DateTime(Convert.ToInt32(Appuntamenti[posmin].appuntamenti[0].dataAppuntamento.Substring(6, 4), 10), Convert.ToInt32(Appuntamenti[posmin].appuntamenti[0].dataAppuntamento.Substring(3, 2), 10), Convert.ToInt32(Appuntamenti[posmin].appuntamenti[0].dataAppuntamento.Substring(0, 2), 10));
                        DateTime date2 = new DateTime(Convert.ToInt32(Appuntamenti[k].appuntamenti[0].dataAppuntamento.Substring(6, 4), 10), Convert.ToInt32(Appuntamenti[k].appuntamenti[0].dataAppuntamento.Substring(3, 2), 10), Convert.ToInt32(Appuntamenti[k].appuntamenti[0].dataAppuntamento.Substring(0, 2), 10));
                        if (date1.CompareTo(date2) > 0)
                        {
                            posmin = k;
                        }
                    }
                    if (posmin != t)
                    {
                        AppuntamentoProposto temp = Appuntamenti[t];
                        Appuntamenti[t] = Appuntamenti[posmin];
                        Appuntamenti[posmin] = temp;
                    }
                }*/
                VisualizzaAppuntamenti grouped = new VisualizzaAppuntamenti()
                {
                    LongName = Appuntamenti[i].codiceImpegnativa,
                    ShortName = Appuntamenti[i].codiceImpegnativa,
                    dataEmissioneRicetta = Appuntamenti[i].dataEmissioneRicetta,
                    contatto = Appuntamenti[i].assistito
                };
                for (int j = 0; j < Appuntamenti[i].appuntamenti.Count; j++)
                {
                    grouped.Add(Appuntamenti[i].appuntamenti[j]);
                }
                grouped.Scaduto();
                Appunt.Add(grouped);
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
