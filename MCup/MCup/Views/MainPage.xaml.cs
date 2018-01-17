﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MCup.Views
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }
        
        //Metodo per avviare la page dell'icona Prenotazioni
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FormPrenotazione());
        }

        //Metodo per avviare la page dell'icona Appuntamenti
        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GestioneAppuntamenti());
        }

        private void logout_Clicked(object sender, EventArgs e)
        {
            if (Device.RuntimePlatform == Device.Android)
                App.Current.MainPage = new NavigationPage(new Login());
            else
                App.Current.MainPage = new NavigationPage(new LoginIoS());
        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ListaContatti());
        }
    }
}
