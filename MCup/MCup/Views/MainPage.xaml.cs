using MCup.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MCup.Model;
using Xamarin.Forms;
using MCup.CustomPopUp;
using Rg.Plugins.Popup.Extensions;

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
            tapIconaPagamenti.IsEnabled = true;
        }
        //Metodo per avviare la page dell'icona Prenotazioni
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            disableMultiTouch();
            bool controllo = await ControlloAsl();
            if(controllo)
           await Navigation.PushAsync(new FormPrenotazioneAsl(false));
            else
               await Navigation.PushAsync(new FormPrenotazione(false));
        }

       private async Task<bool> ControlloAsl()
        {
            REST<object, bool> connessioneControlloAsl = new REST<object, bool>();
            List<Header> listaheaders = new List<Header>();
            listaheaders.Add(new Header("struttura", "150907"));
            bool controllo = await connessioneControlloAsl.GetSingleJson(SingletonURL.Instance.getRotte().isAsl,listaheaders);
            return controllo;
        }

        //Metodo per avviare la page dell'icona Appuntamenti
        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            disableMultiTouch();
            Navigation.PushAsync(new PaginaAppuntamenti());
        }

        private async void logout_Clicked(object sender, EventArgs e)
        {
            var risposta = await DisplayAlert("Logout","Sei sicuro di voler effettuare il logout?","SI","NO");
            if (!risposta)
                return;
            List<Header> listaHeader = new List<Header>();
            listaHeader.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
            TokenNotification tokNot = new TokenNotification();
            tokNot.tokenNotification = "";
            REST<TokenNotification, bool> connessione = new REST<TokenNotification, bool>();
            bool res = await connessione.PostJson(SingletonURL.Instance.getRotte().updateTokenNotifiche, tokNot,listaHeader);
            if (connessione.responseMessage != HttpStatusCode.OK)
            {
                await App.Current.MainPage.DisplayAlert("Attenzione " + (int)connessione.responseMessage, connessione.warning, "OK");
            }
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

        private async void vaiPaginaManutenzione(object sender, EventArgs e)
        {
           await Navigation.PushAsync(new  PaginaLavoriInCorso());
        }

        private async void termini_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PaginaPrivacy());
        }

        private async void vaiPaginaReferti(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PaginaReferti());

        }
    }
}
