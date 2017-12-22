using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCup.ModelView;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MCup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginIoS : ContentPage
    {
        public LoginIoS()
        {
            InitializeComponent();
            BindingContext = new LoginModelView();
        }
        private async void vaiRegistrazione(object sender, System.EventArgs e)
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                await Navigation.PushAsync(new RegistrazioneIOS());
            }
            else
                await Navigation.PushAsync(new Registrazione());
        }
    }
}