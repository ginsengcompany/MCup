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
        private REST<String> rest;
        

        public Termini()
        {
            InitializeComponent();
            rest = new REST<String>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var x = await rest.getString(URL.TerminoServizio);
            labelTermini.Text = x.ToString();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var x =TerminiDiServizio.UpdateTermini();
            Navigation.PushModalAsync(new ListaStrutture());
        }
    }
}