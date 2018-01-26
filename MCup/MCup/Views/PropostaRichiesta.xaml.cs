using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            data.Date = DateTime.Parse(item.dataAppuntamento);
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
