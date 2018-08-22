﻿using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCup.Model;
using MCup.Service;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MCup.CustomPopUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupInfoScan : PopupPage
    {
        private string url,nome,cognome;

        public PopupInfoScan(string imgName)
        {
            InitializeComponent();
            imgInfo.Source = imgName;
            Content = FrameContainer;
            btnInvio.IsVisible = false;
            immaginedilogo.IsVisible = false;
            //FrameContainer.HeightRequest = -1;
           CloseWhenBackgroundIsClicked = true;
        }

        public PopupInfoScan(string urlPdf, string nome, string cognome )
        {
            InitializeComponent();
            Content = FrameContainer;
            imgInfo.IsVisible = false;
            this.nome = nome;
            this.cognome = cognome;
            this.url = urlPdf;
            btnInvio.IsVisible = false;
            btnInoltra.IsVisible = true;
            entryUsername.IsVisible = true;
            immaginedilogo.IsVisible = true;
            //FrameContainer.HeightRequest = -1;
            CloseWhenBackgroundIsClicked = true;
        }

        public PopupInfoScan()
        {
            InitializeComponent();
            imgInfo.IsVisible = false;
            entryUsername.IsVisible = true;
            btnInvio.IsVisible = true;
            immaginedilogo.IsVisible = true;

            Content = FrameContainer;

            //FrameContainer.HeightRequest = -1;
            CloseWhenBackgroundIsClicked = true;
        }

        protected async override Task OnDisappearingAnimationBegin()
        {
            var taskSource = new TaskCompletionSource<bool>();
            var currentHeight = FrameContainer.Height;
            await Task.WhenAll(
                imgInfo.FadeTo(0));

            FrameContainer.Animate("HideAnimation", d =>
                {
                    FrameContainer.HeightRequest = d;
                },
                start: currentHeight,
                end: 170,
                finished: async (d, b) =>
                {
                    await Task.Delay(300);
                    taskSource.TrySetResult(true);
                });

            await taskSource.Task;
        }

        protected override bool OnBackgroundClicked()
        {
            closeAllPopup();
            return false;
        }

        private async void closeAllPopup()
        {
            await Navigation.PopAllPopupAsync();
        }


        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            closeAllPopup();
        }

        private async void ModificaPassword(object sender, EventArgs e)
        {
            Utente user = new Utente();
            user.email = entryUsername.Text;
            REST<Utente,string> connessioneModifica = new REST<Utente, string>();
            var response = await 
                connessioneModifica.PostJson(SingletonURL.Instance.getRotte().passwordSmarrita, user);
            await DisplayAlert("Attenzione", connessioneModifica.warning, "OK");
            closeAllPopup();
        }
        private async void InoltroEmail(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(entryUsername.Text))
            {
                List<Header> listaJHeaders = new List<Header>();
                listaJHeaders.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
                listaJHeaders.Add(new Header("struttura", "150907"));
                Email email = new Email(url, entryUsername.Text,nome,cognome);
                REST<Email, string> connessioneEmail = new REST<Email, string>();
                var response = await connessioneEmail.PostJson(SingletonURL.Instance.getRotte().inviaRefertoEmail,
                    email, listaJHeaders);
                await DisplayAlert("Attenzione", connessioneEmail.warning, "OK");
                closeAllPopup();
            }
        }
    }
}