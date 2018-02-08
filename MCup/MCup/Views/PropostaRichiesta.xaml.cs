using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Xamarin.Forms;
using MCup.Model;
using MCup.Service;
using MCup.ModelView;
using System.Threading.Tasks;

namespace MCup.Views
{
    public partial class PropostaRichiesta : ContentPage
    {

        private PropostaRichiestaModelView modelView;
        private string itemDatePicker;
        private bool dataCambiata = false;

        public PropostaRichiesta(Impegnativa ricetta,List<Prestazione> prestazioni, Assistito contatto)
        {
            InitializeComponent();
            data.MinimumDate = DateTime.Now;
            itemDatePicker = "";
            modelView = new PropostaRichiestaModelView(ricetta,prestazioni, contatto, this);
            BindingContext = modelView;
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        public void visualizzaDatePicker()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                data.Date = Convert.ToDateTime(DateTime.Today, new CultureInfo("it-IT"));
            }
            else
            {
                data.Date = DateTime.Today;
            }
            data.Focus();
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            var a = sender as DatePicker;
            var x = a.Date;
            dataCambiata = true;
            itemDatePicker = string.Format("{0:dd/MM/yyyy}", x);
        }

        private async void data_Unfocused(object sender, FocusEventArgs e)
        {
            if (dataCambiata)
            {
                await modelView.infoProssimaData(itemDatePicker);
                modelView.IsEnabled = true;
                dataCambiata = false;
            }
            else
            {
                modelView.IsEnabled = true;
            }
        }
    }
}
