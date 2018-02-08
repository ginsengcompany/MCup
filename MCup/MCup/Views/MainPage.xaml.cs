using MCup.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCup.Model;
using Xamarin.Forms;

namespace MCup.Views
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            tapIconaPrenotazioni.IsEnabled = true;
            tapIconaAppuntamenti.IsEnabled = true;
            tapIconaContatti.IsEnabled = true;
        }
        //Metodo per avviare la page dell'icona Prenotazioni
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            disableMultiTouch();
            Navigation.PushAsync(new FormPrenotazione());
        }

        //Metodo per avviare la page dell'icona Appuntamenti
        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            disableMultiTouch();
            Navigation.PushAsync(new PaginaAppuntamenti());
        }

        private async void logout_Clicked(object sender, EventArgs e)
        {
            List<Header> listaHeader = new List<Header>();
            listaHeader.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
            TokenNotification tokNot = new TokenNotification();
            tokNot.tokenNotification = "";
            REST<TokenNotification, bool> connessione = new REST<TokenNotification, bool>();
            bool res = await connessione.PostJson(SingletonURL.Instance.getRotte().updateTokenNotifiche, tokNot,listaHeader);
            Application.Current.Properties["flagRimaniLoggato"]= "False";
            Application.Current.MainPage=new NavigationPage(new Login());
        
        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            disableMultiTouch();
            Navigation.PushAsync(new ListaContatti());
        }
        private void disableMultiTouch()
        {
            tapIconaPrenotazioni.IsEnabled = false;
            tapIconaAppuntamenti.IsEnabled = false;
            tapIconaContatti.IsEnabled = false;
        }
        private class TokenNotification
        {
            public string tokenNotification;
        }
    }
}
