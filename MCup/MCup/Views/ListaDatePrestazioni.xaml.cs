using MCup.Model;
using MCup.ModelView;
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
	public partial class ListaDatePrestazioni : ContentPage
	{
		public ListaDatePrestazioni ()
		{
			InitializeComponent ();

            BindingContext = new ListaDatePrenotazioniModelView();
        }

        private async void listDate_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ListaDatePrenotazioni elemTapped = e.Item as ListaDatePrenotazioni;
            REST<ListaDatePrenotazioni, String> conn = new REST<ListaDatePrenotazioni, String>();
            string response = await conn.PostJson(URL.Calendario, elemTapped);
            await DisplayAlert("Prenotazione", conn.warning, "OK");
        }
    }
}