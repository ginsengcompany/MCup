using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Net.Http;
using MCup;
namespace MCup.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPrincipale: MasterDetailPage
	{
		public MenuPrincipale ()
		{
			InitializeComponent ();
            inizializzazioneMenu();

		}
        private void inizializzazioneMenu()
        {
            List<Menu> menuPrincipale = new List<Menu>
            {
                new Menu {MenuTitle = "Prenotazioni"},
                new Menu {MenuTitle = "Appuntamenti"}
            };
            ListaMenu.ItemsSource = menuPrincipale;
            Detail = new NavigationPage(new MainPage());
        }

        private void ListaMenu_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
        public class Menu
        {
            public string MenuTitle
            {
                get;
                set;
            }
        }
    }
}