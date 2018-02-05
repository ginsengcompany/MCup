using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MCup.Model;
using MCup.Service;
using Xamarin.Forms;

namespace MCup.ModelView
{
    public class GestioneAppuntamentiModelView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Assistito contatto;
        private Color colore;
        private List<Assistito> contatti = new List<Assistito>();
        private List<AppuntamentoPrestazioneProposto> appuntamenti = new List<AppuntamentoPrestazioneProposto>();
        private AppuntamentoProposto date = new AppuntamentoProposto();
        private Boolean visibileLabel = false;
        private Boolean visibile = true;
        private string visi;



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

        public List<AppuntamentoPrestazioneProposto> Appuntamenti
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
            REST<object, List<Assistito>> rest = new REST<object, List<Assistito>>();
            contatti = await rest.GetSingleJson(URL.InfoPersonali, App.Current.Properties["tokenLogin"].ToString());
            Contatti = contatti;
        }

        public GestioneAppuntamentiModelView()
        {
            VisibileL = "false";
            leggiContatti();
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
            try
            {
                Assistito invioContatto = date.assistito;
                REST<Assistito, AppuntamentoPrestazioneProposto> connessione = new REST<Assistito, AppuntamentoPrestazioneProposto>();
                Appuntamenti = await connessione.PostJsonList(URL.appuntamenti, invioContatto,App.Current.Properties["tokenLogin"].ToString());
                if (Appuntamenti.Count==0)
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
    }
}
