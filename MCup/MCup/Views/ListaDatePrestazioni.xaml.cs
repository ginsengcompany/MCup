using MCup.Model;
using MCup.ModelView;
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

        private void listDate_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var x = e.Item;

        }
    }
}