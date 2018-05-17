using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MCup.CustomPopUp;
using MCup.ModelView;
using MCup.Service;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

/*
 * Questa pagina viene visualizzata all'avvio dell'app solo se il dispositivo è Android. Questa pagina gestisce l'autenticazione dell'utente
 * per poter accedere alla sua area privata e quindi utilizzare i servizi che l'app fornisce.
 */

namespace MCup.Views
{
    public partial class Login : ContentPage
    {
        private Boolean flaglogin = false;
        private LoginModelView z;
        public Login()
        {
            InitializeComponent();
            z = new LoginModelView(this);
            BindingContext = z; //Questa pagina utilizza l'MWWM ed effettua il binding con la classe LoginModelView

        }

        public async void PendingPrenotazione(bool pending)
        {
            bool controllo = await ControlloAsl();
            if (controllo)
              await  Navigation.PushAsync(new FormPrenotazioneAsl(pending));
            else
               await Navigation.PushAsync(new FormPrenotazione(pending));
        }
        private async Task<bool> ControlloAsl()
        {
            REST<object, bool> connessioneControlloAsl = new REST<object, bool>();
            List<Header> listaheaders = new List<Header>();
            listaheaders.Add(new Header("struttura", "150907"));
            bool controllo = await connessioneControlloAsl.GetSingleJson(SingletonURL.Instance.getRotte().isAsl, listaheaders);
            return controllo;
        }
        /*
         * Questo metodo viene chiamato quando l'utente clicca sulla label che fa riferimento alla fase di registrazione
         */
        private async void vaiRegistrazione(object sender, System.EventArgs e)
        {
            if (SingletonURL.Instance.error)
                await DisplayAlert("Attenzione", "server momentaneamente non disponibile", "OK");
            else
                await Navigation.PushAsync(new Registrazione()); //Avvia la pagina di registrazione dedicata ai dispositivi Android
        }

        private async void richiestaDimenticaPassw(object sender, EventArgs e)
        {
            if (SingletonURL.Instance.error)
                await DisplayAlert("Attenzione", "server momentaneamente non disponibile", "OK");
            else
                await Navigation.PushPopupAsync(new PopupInfoScan());
        }
    }
}
