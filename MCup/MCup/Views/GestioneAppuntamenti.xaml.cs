using MCup.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCup.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MCup.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GestioneAppuntamenti : ContentPage
	{
	    private GestioneAppuntamentiModelView form;
		public GestioneAppuntamenti ()
		{
			InitializeComponent ();
		    form = new GestioneAppuntamentiModelView();
		    BindingContext = form;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

	    private void Picker_OnSelectedIndexChanged(object sender, EventArgs e)
	    {
	        var a = sender as Picker;
	        var b = a.SelectedItem as Contatto;
	        form.autoCompila(b);
        }
	}
}