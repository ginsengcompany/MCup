using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCup.ModelView;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

/*
 * Questa pagina viene visualizzata all'avvio dell'app solo se il dispositivo è IOS. Questa pagina gestisce l'autenticazione dell'utente
 * per poter accedere alla sua area privata e quindi utilizzare i servizi che l'app fornisce.
 */

namespace MCup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginIoS : ContentPage
    {
        public LoginIoS()
        {
            InitializeComponent();
            //BindingContext = new LoginModelView(true); //Questa pagina utilizza l'MWWM ed effettua il binding con la classe LoginModelView
        }
        private async void vaiRegistrazione(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Registrazione()); //Avvia la pagina di registrazione dedicata ai dispositivi IOS
        }
    }
}