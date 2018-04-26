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

		public GestioneAppuntamenti (AppuntamentoPrestazioneProposto appuntamentoSelezionato)
		{
			InitializeComponent ();
		    form = new GestioneAppuntamentiModelView(appuntamentoSelezionato,this);
		    BindingContext = form;
        }

	    public async void PopAsync()
	    {
	       await Navigation.PopAsync();
	    }
	}
}