using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MCup.Annotations;
using MCup.Model;
using MCup.Service;
using MCup.Views;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
using Xamarin.Forms;

namespace MCup.ModelView
{
    public class PaginaRefertiModelView: INotifyPropertyChanged
    {
        private Assistito contatto;//Oggetto che astrae la classe assistito
        private bool switched;
        private bool isBusy = false;
        private PaginaReferti paginaReferti;//Oggetto che astrae la pagina a cui si riferisce il model view
        private List<Assistito> contatti = new List<Assistito>();// lista di contattai di tipo assistito
        private List<ListaReferti> referti = new List<ListaReferti>();//Lista di appuntamenti utilizzato per creare la listview di appuntamenti da far selezionare all'utente
        private Boolean visibileLabel = false;//variabile booleana che setta la visibilità o meno di un elemento nello xaml
        private Boolean visibile = true;//variabile booleana che setta la visibilità o meno di un elemento nello xaml
        private string visi;//variabile  che setta la visibilità o meno di un elemento nello xaml
        private Boolean isbusyannulla;
        private string refertodata = "";


        #region Proprietà

        public Boolean IsBusy
        {
            get { return isBusy; }
            set
            {
                OnPropertyChanged();
                isBusy = value;
            }
        }

        public string RefertoPiuData
        {
            get { return refertodata; }
            set
            {
                OnPropertyChanged();
                refertodata = value;
            }
        }

        public Boolean IsBusyAnnulla
        {
            get { return isbusyannulla; }
            set
            {
                OnPropertyChanged();
                isbusyannulla = value;
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

        public List<ListaReferti> Referti //Proprietà riferita al campo List Appuntamenti
        {
            get { return referti; }
            set
            {
                OnPropertyChanged();
                referti = value;
            }
        }

        public List<Assistito> Contatti //Proprietà riferita al campo Contatti
        {
            get { return contatti; }
            set
            {
                OnPropertyChanged();
                contatti = new List<Assistito>(value);
                if (value.Count == 1)
                {
                    paginaReferti.Picker_selezionaPrimoElemento(value[0]);
                }
            }
        }


        #endregion



        public PaginaRefertiModelView(PaginaReferti pagina)
        {
            leggiContatti();
            this.paginaReferti = pagina;
        }

        private async void leggiContatti()
        {
            List<Header> listaHeader = new List<Header>();
            listaHeader.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
            REST<object, List<Assistito>> rest = new REST<object, List<Assistito>>();
            contatti = await rest.GetSingleJson(SingletonURL.Instance.getRotte().InfoPersonali, listaHeader);
            if (rest.responseMessage != HttpStatusCode.OK)
            {
                await MessaggioConnessione.displayAlert((int)rest.responseMessage, rest.warning);
            }
            else
            {
                Contatti = contatti.OrderBy(o => o.cognome).ToList();
            }
        }


        public async void autoCompila(Assistito elementSelected)
        {
            contatto = elementSelected;
            IsBusy = true;
            await invioDatiAssistito();
            IsBusy = false;
        }

        //Metodo che invia i dati dell'utente per cui si è scelto di visualizzare gli appuntamenti
        public async Task invioDatiAssistito()
        {
            List<Header> listaJHeaders = new List<Header>();
            listaJHeaders.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
            listaJHeaders.Add(new Header("struttura", "150907"));
            listaJHeaders.Add(new Header("cf",contatto.codice_fiscale));
            try
            {

                REST<object, Referto> connessione = new REST<object, Referto>();
               var Referto = await connessione.PostJson(SingletonURL.Instance.getRotte().listaRefertiUtente, listaJHeaders);
               
                if (connessione.responseMessage != HttpStatusCode.OK)
                {
                    await MessaggioConnessione.displayAlert((int)connessione.responseMessage, connessione.warning);
                }
                else
                    Referti = Referto.listaReferti;

                for (int i = 0; i < Referti.Count; i++)
                {
                    if (string.IsNullOrEmpty(Referti[i].metadati.autoreDocumento))
                    {
                        Referti[i].metadati.autoreDocumento = "N/D";
                    }
                    if (string.IsNullOrEmpty(Referti[i].metadati.desEvento))
                    {
                        Referti[i].metadati.desEvento = "N/D";
                    }
                    if (string.IsNullOrEmpty(Referti[i].metadati.desDocumento))
                    {
                        Referti[i].metadati.desDocumento = "N/D";
                    }
                    if (string.IsNullOrEmpty(Referti[i].metadati.dataDocumento))
                    {
                        Referti[i].metadati.dataDocumento = "N/D";
                    }
                }

                if (Referti.Count == 0)
                    VisibileLabel = true;
                else
                    VisibileLabel = false;



            }
            catch (FormatException e)
            {
                await App.Current.MainPage.DisplayAlert("Attenzione",
                    "connessione non riuscita o codici impegnativa errati", "riprova");
            }
        }

        public async Task Download(ListaReferti refertoSelezionato)
        {
            List<Header> listaJHeaders = new List<Header>();
            listaJHeaders.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
            listaJHeaders.Add(new Header("struttura", "150907"));
            listaJHeaders.Add(new Header("id", refertoSelezionato.id));
            REST<object, string> connessioneDownload = new REST<object, string>();
            var response = await connessioneDownload.PostJson(SingletonURL.Instance.getRotte().scaricaReferto, listaJHeaders);
            if (connessioneDownload.responseMessage != HttpStatusCode.OK)
            {
                await MessaggioConnessione.displayAlert((int)connessioneDownload.responseMessage, connessioneDownload.warning);
            }
            else
            {
                Device.OpenUri(new Uri(connessioneDownload.warning + refertoSelezionato.id));
            }
           /* var downloadManager = CrossDownloadManager.Current;
              var file = downloadManager.CreateDownloadFile(connessioneDownload.warning + refertoSelezionato.id);
              downloadManager.Start(file);*/
        }
       



        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
