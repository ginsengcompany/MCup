using MCup.Model;
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
    public partial class VerificaRicetta : ContentPage
    {
        public VerificaRicetta(Ricetta ricetta)
        {
            InitializeComponent();
            List<prest> esempio = new List<prest>();
            esempio.Add(new prest(){prestazione="ALTRA IRRIGAZIONE DI FERITA"});
            desprest.ItemsSource = esempio;
        }
        private class prest{
            public string prestazione { get; set; }
        }
    }
}