using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MCup.Model;
using MCup.Service;
using MCup.Views;
using Xamarin.Forms;

namespace MCup.ModelView
{
    public class PaginaAppuntamentiModelView: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Assistito contatto;
        private Color colore;
        private PaginaAppuntamenti paginaAppuntamenti;
        private List<Assistito> contatti = new List<Assistito>();
        private List<AppuntamentoProposto> appuntamenti = new List<AppuntamentoProposto>();
        private AppuntamentoProposto date = new AppuntamentoProposto();
        private Boolean visibileLabel = false;
        List<AppuntamentoPrestazioneProposto> appunt = new List<AppuntamentoPrestazioneProposto>();
        private Boolean visibile = true;
        private string visi;


        public List<AppuntamentoPrestazioneProposto> Appunt
        {
            get { return appunt; }
            set
            {
                OnPropertyChanged();
                appunt = value;
            }
        }
        public string VisibileL
        {
            get { return visi; }
            set
            {
                OnPropertyChanged();
                visi = value;
            }
        }
        public Boolean Visibile
        {
            get { return visibile; }
            set
            {
                OnPropertyChanged();
                visibile = value;
            }
        }
        public Boolean VisibileLabel
        {
            get { return visibileLabel; }
            set
            {
                OnPropertyChanged();
                visibileLabel = value;
            }
        }

        public List<AppuntamentoProposto> Appuntamenti
        {
            get { return appuntamenti; }
            set
            {
                OnPropertyChanged();
                appuntamenti = value;
            }
        }

        public List<Assistito> Contatti
        {
            get { return contatti; }
            set
            {
                OnPropertyChanged();
                contatti = new List<Assistito>(value);
            }
        }


        private async void leggiContatti()
        {
            List<Header> listaHeader = new List<Header>();
            listaHeader.Add(new Header("x-access-token",App.Current.Properties["tokenLogin"].ToString() ));
            REST<object, List<Assistito>> rest = new REST<object, List<Assistito>>();
            contatti = await rest.GetSingleJson(SingletonURL.Instance.getRotte().InfoPersonali,listaHeader );
            Contatti = contatti;
        }

        public PaginaAppuntamentiModelView( PaginaAppuntamenti pagina)
        {
            VisibileL = "false";
            
            leggiContatti();
            this.paginaAppuntamenti = pagina;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public async void autoCompila(Assistito elementSelected)
        {
            date.assistito = elementSelected;
            await invioDatiAssistito();
        }

        public async Task invioDatiAssistito()
        {
            List<Header> listaJHeaders= new List<Header>();
            listaJHeaders.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
            try
            {
                Assistito invioContatto = date.assistito;
                REST<Assistito, AppuntamentoProposto> connessione = new REST<Assistito, AppuntamentoProposto>();
                Appuntamenti = await connessione.PostJsonList(SingletonURL.Instance.getRotte().appuntamenti, invioContatto,listaJHeaders);

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

        public async void push(AppuntamentoProposto elementoSelezionato)
        {
            await paginaAppuntamenti.Navigation.PushAsync(new GestioneAppuntamenti(elementoSelezionato));
            Contatti.Clear();
            VisibileLabel = false;
        }
    }
}
