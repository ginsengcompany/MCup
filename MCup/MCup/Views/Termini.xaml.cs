using MCup.Database.Data;
using MCup.Database.Models;
using MCup.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MCup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Termini : ContentPage
    {
        private REST<Object,String> rest;
        // NavigationPage TerminiNuovo= new NavigationPage(new Termini());
        Label labelErrore = new Label
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Start,
            HorizontalTextAlignment = TextAlignment.Center,
            FontSize = 25,
            HeightRequest = 100,
            FormattedText = "Attenzione connessione non attiva o in  errore, fra 10 secondi riproveremo a connetterci"
        };
        ActivityIndicator activityIndicator = new ActivityIndicator
        {
            IsRunning = true,
            IsVisible = true

        };


        public Termini()
        {
            InitializeComponent();
            rest = new REST<Object,String>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                Scroll.IsVisible = true;
                stackTermini.IsVisible = true;
                stackButton.IsVisible = true;
                labelErrore.IsVisible = false;
                activityIndicator.IsVisible = false;
                
                var x = await rest.getString(URL.TerminiServizio);
                labelTermini.Text = x.ToString();
            }
            catch (Exception)
            {
                Scroll.IsVisible = false;
                stackButton.IsVisible = false;
                labelErrore.IsVisible = true;
                activityIndicator.IsVisible = true;
                stackTermini.Children.Add(labelErrore);
                stackTermini.Children.Add(activityIndicator);
                
                Device.StartTimer(TimeSpan.FromSeconds(10), () =>
                {
                    OnAppearing();
                    return true;
                });

            }

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                var x = TerminiDiServizio.UpdateTermini();
                Navigation.PushModalAsync(new ListaStrutture());
            }
            catch (Exception)
            {

                DisplayAlert("Attenzione", "Errore di connessione", "riprova");
            }
        }
    }
}