using MCup.Database.Data;
using MCup.Database.Models;
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
        public Termini()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var x =TerminiDiServizio.UpdateTermini();
            Navigation.PushModalAsync(new MainPage());
        }
    }
}