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
        private PrenotazioneProposta itemDatePicker;
        private bool dataCambiata = false;

        public PropostaRichiesta(List<Prestazioni> prestazioni, Contatto contatto)
        {
            InitializeComponent();
            data.MinimumDate = DateTime.Now;
            itemDatePicker = new PrenotazioneProposta();
            modelView = new PropostaRichiestaModelView(prestazioni, contatto, this);
            BindingContext = modelView;
        }

        public void visualizzaDatePicker(PrenotazioneProposta item)
        {
            itemDatePicker = item;
            if (Device.RuntimePlatform == Device.iOS)
            {
                /*
                string temp = item.dataAppuntamento.Substring(0, 10);
                string giorno = temp.Substring(0, 2);
                string mese = temp.Substring(3, 2);
                string anno = temp.Substring(6);
                temp = mese + "/" + giorno + "/" + anno;*/

                data.Date = Convert.ToDateTime(item.dataAppuntamento, new CultureInfo("it-IT"));
            }
            else
            {
            data.Date = DateTime.Parse(item.dataAppuntamento);
            }
            data.Focus();
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            var a = sender as DatePicker;
            var x = a.Date;
            dataCambiata = true;
            itemDatePicker.dataAppuntamento = string.Format("{0:dd/MM/yyyy}", x);
        }

        private async void data_Unfocused(object sender, FocusEventArgs e)
        {
            if (dataCambiata)
            {
                await modelView.infoProssimaData(itemDatePicker);
                dataCambiata = false;
            }
        }
    }
}
