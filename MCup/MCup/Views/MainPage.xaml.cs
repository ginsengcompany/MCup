using MCup.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MCup
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            
            var btnTermini = new Button
            {
                Text="accetta i termini",
                HorizontalOptions= LayoutOptions.Center,
                VerticalOptions= LayoutOptions.Center
            };
            stackPrincipale.Children.Add(btnTermini);
            btnTermini.Clicked += ButtonTermini_Clicked;
        }

        private void ButtonTermini_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Termini());
        }
    }
}
