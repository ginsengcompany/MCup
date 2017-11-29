using MCup.Database.Data;
using MCup.Database.Models;
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
        private UtenzaPrenotazione utenza;
        private string codiceNRE, codiceStruttura, nomeStruttura;

		public ListaDatePrestazioni (UtenzaPrenotazione utenza, string codiceNRE, string codiceStruttura, string nomeStruttura)
		{
            this.utenza = utenza;
            this.codiceNRE = codiceNRE;
            this.codiceStruttura = codiceStruttura;
            this.nomeStruttura = nomeStruttura;
			InitializeComponent ();
            BindingContext = new ListaDatePrenotazioniModelView();
        }

        private async void listDate_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ListaDatePrenotazioni elemTapped = e.Item as ListaDatePrenotazioni;
            REST<ListaDatePrenotazioni, String> conn = new REST<ListaDatePrenotazioni, String>();
            string response = await conn.PostJson(URL.Calendario, elemTapped);
            if (conn.warning == "Prenotazione effettuata")
            {
                Prenotazione prenotazione = new Prenotazione(this.utenza, elemTapped, this.codiceStruttura, this.nomeStruttura, this.codiceNRE);
                Appuntamento appuntamento = new Appuntamento(prenotazione);
                AppuntamentoData.InsertAppuntamento(appuntamento);
            }
            await DisplayAlert("Prenotazione", conn.warning, "OK");
        }
    }
}