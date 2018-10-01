using MCup.CustomPopUp;
using MCup.Model;
using MCup.Service;
using MCup.Views;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MCup.Annotations;
using Xamarin.Forms;

namespace MCup.ModelView
{
    class PaginaPrivacyModelView : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged; //evento che implementa l'interfaccia INotifyPropertyChanged
        private bool isBusy = false;
        private bool isEnabled = true;
       
        #region Proprietà

        public PaginaPrivacy pagina;

        public ICommand infoPrivacy { protected set; get; } 
        public ICommand datiUtente { protected set; get; }
        public ICommand eliminaUtente { protected set; get; }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                OnPropertyChanged();
                isBusy = value;
            }
        }
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                OnPropertyChanged();
                isEnabled = value;
            }
        }

        #endregion
        #region OnPropertyChange

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
        #region Costruttore
        //Costruttore che inizializza un utenza vuota e definisce il metodo a cui il Command registrati fa riferimento
        public PaginaPrivacyModelView(PaginaPrivacy pagina)
        {
            this.pagina = pagina;
            List<Header> listaheader = new List<Header>();
            listaheader.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));

            infoPrivacy = new Command(async () =>
            {
                IsEnabled = false;
                IsBusy = true;
                await App.Current.MainPage.Navigation.PushPopupAsync(new PopUpTerminiServizio(),false);
                IsEnabled = true;
                IsBusy = false;
            });
            datiUtente = new Command(async () =>
            {
                var scelta = await App.Current.MainPage.DisplayAlert("Attenzione", "Gentile utente tutti i dati le saranno inoltrati tramite email, sei sicuro di voler procedere?", "SI", "NO");
                if (scelta)
                {
                    IsEnabled = false;
                    IsBusy = false;
                    REST<object, string> connessioneEmail = new REST<object, string>();
                    var response = await connessioneEmail.getString(SingletonURL.Instance.getRotte().infoPersonaliEmail, listaheader);
                    await MessaggioConnessione.displayAlert(connessioneEmail.warning, false);
                    IsBusy = false;
                    IsEnabled = true;
                }
            });
            eliminaUtente = new Command(async () =>
            {
                IsEnabled = false;
                await PopUp();
                IsEnabled = true;
            });
        }
        #endregion

        public async Task PopUp()
        {
            await pagina.confermaEliminaAccount();
        }
    }
}