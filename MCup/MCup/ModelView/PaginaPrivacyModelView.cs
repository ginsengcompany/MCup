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
using Xamarin.Forms;

namespace MCup.ModelView
{
    class PaginaPrivacyModelView : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged; //evento che implementa l'interfaccia INotifyPropertyChanged
       
        #region Proprietà

        public ICommand infoPrivacy { protected set; get; } 
        public ICommand datiUtente { protected set; get; }
        public ICommand eliminaUtente { protected set; get; }

        #endregion
        #region OnPropertyChange

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
        #region Costruttore
        //Costruttore che inizializza un utenza vuota e definisce il metodo a cui il Command registrati fa riferimento
        public PaginaPrivacyModelView()
        {
            List<Header> listaheader = new List<Header>();
            listaheader.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));

            infoPrivacy = new Command(async () =>
            {
                await App.Current.MainPage.Navigation.PushPopupAsync(new PopUpTerminiServizio());

            });
            datiUtente = new Command(async () =>
            {
                var scelta = await App.Current.MainPage.DisplayAlert("Attenzione", "Gentile utente tutti i dati le saranno inoltrati tramite email, sei sicuro di voler procedere?", "SI", "NO");
                if (scelta)
                {
                    REST<object, string> connessioneEmail = new REST<object, string>();
                    var response = await connessioneEmail.getString(SingletonURL.Instance.getRotte().infoPersonaliEmail, listaheader);
                    await MessaggioConnessione.displayAlert(connessioneEmail.warning, false);
                }
                
            });
            eliminaUtente = new Command(async () =>
            {
                REST<object, string> connessioneElimina = new REST<object, string>();

                var risposta = await App.Current.MainPage.DisplayAlert("Eliminazione", "sei sicuro di voler eliminare l'account? In accordo al rgpd agli articoli 17, 21 e 22 tutti i tuoi dati saranno rimossi", "si", "no");
                if (risposta)
                {
                    try
                    {
                        var response = await connessioneElimina.getString(SingletonURL.Instance.getRotte().eliminaContattoPersonale, listaheader);
                        if (connessioneElimina.responseMessage != HttpStatusCode.OK)
                        {
                            await MessaggioConnessione.displayAlert((int)connessioneElimina.responseMessage, connessioneElimina.warning);
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Complimenti", "l'account è stato eliminato con successo", "ok");
                            App.Current.MainPage = new NavigationPage(new Login());
                        }
                    }
                    catch (Exception)
                    {
                        await App.Current.MainPage.DisplayAlert("Attenzione", connessioneElimina.warning, "ok");

                    }

                }


            });
        }
        #endregion
    }
}