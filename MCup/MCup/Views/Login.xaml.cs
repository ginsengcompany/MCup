using System;
using System.Collections.Generic;
using MCup.ModelView;
using Xamarin.Forms;

namespace MCup.Views
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            BindingContext = new LoginModelView();

        }

        private async void vaiRegistrazione(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Registrazione());
        }
    }
}
