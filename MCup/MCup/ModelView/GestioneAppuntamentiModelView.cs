using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MCup.Model;
using MCup.Service;
using MCup.Views;
using Xamarin.Forms;

namespace MCup.ModelView
{
    public class GestioneAppuntamentiModelView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Assistito contatto;
        private Color colore;
        private GestioneAppuntamenti pagina;
        private List<Assistito> contatti = new List<Assistito>();
        private List<AppuntamentoProposto> appuntamenti = new List<AppuntamentoProposto>();
        private AppuntamentoProposto date = new AppuntamentoProposto();
        private AppuntamentoProposto appuntamentoSelezionato = new AppuntamentoProposto();
        private Boolean visibileLabel = false;
        List<AppuntamentoPrestazioneProposto> appunt= new List<AppuntamentoPrestazioneProposto>();
        private Boolean visibile = true;
        private string visi;
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
                   ResponseAnnullaImpegnativa response =  await connessioneAnnullamentoImpegnativa.PostJson(SingletonURL.Instance.getRotte().annullaImpegnativa,
                                appuntamentoSelezionato, headers);
                     await App.Current.MainPage.DisplayAlert("Mcup",response.messaggio , "ok");
                    if (response.esito == true)
                    {
                      await  pagina.Navigation.PushAsync(new NavigationPage(new PaginaAppuntamenti()));
                    }
                }
                catch (Exception)
                {

                    await App.Current.MainPage.DisplayAlert("Mcup", "Connessione non riuscita", "ok");

                }

            }
           
        }
        public GestioneAppuntamentiModelView( AppuntamentoProposto appuntamentoSelezionato)
        {
            VisibileL = "false";
            this.appuntamentoSelezionato = appuntamentoSelezionato;
            invioDatiAssistito();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public async Task invioDatiAssistito()
        {
            try
            {
               

                Appunt = appuntamentoSelezionato.appuntamenti;
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
                  /*  foreach (var i in Appuntamenti)
                    {
                        Appunt = i.appuntamenti;
                    }*/
                    
                }
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Attenzione",
                    "connessione non riuscita o codici impegnativa errati", "riprova");
            }
        }
    }

    public class ResponseAnnullaImpegnativa
    {
         public string messaggio { get; set; }
         public bool esito { get; set; }
    }
}
