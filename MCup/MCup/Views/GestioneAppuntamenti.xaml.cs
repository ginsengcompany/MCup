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
	public partial class GestioneAppuntamenti : ContentPage
	{
		public GestioneAppuntamenti ()
		{
			InitializeComponent ();
            BindingContext = new GestioneAppuntamentiModelView();
		}

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }
}