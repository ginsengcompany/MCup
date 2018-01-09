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

/*
 * Questa è la pagina principale che permette all'utente di navigare tra i servizi offerti dall'app e dal sistema informatico messo a sua disposizione.
 * La pagina contiene una MasterDetailPage che visualizza all'utente, tramite un menu a tendina laterale, le possibili opzioni messe a disposizione per l'utente.
 */

namespace MCup.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPrincipale: MasterDetailPage
	{
		public MenuPrincipale ()
		{
			InitializeComponent();
            inizializzazioneMenu();
		}
        //Metodo che inizializza la MasterDetailPage e che inserisce in essa le pagine a cui è possibile accedere dal menu
        private void inizializzazioneMenu()
        {
            List<Menu> menuPrincipale = new List<Menu> //Lista contenente le pagine a cui si può accedere dalla MasterDetailPage
            {
                new Menu { MenuTitle = "Home", ImageIcon = "home.png"},
                new Menu { MenuTitle = "Scegli Struttura Preferita", ImageIcon = "modify.png"},
                new Menu { MenuTitle ="Contatti", ImageIcon = "rubrica.png"}

            };
            ListaMenu.ItemsSource = menuPrincipale; //Assegna all'oggetto ListaMenu dello xaml della pagina la lista precedentemente inizializzata
            Detail = new NavigationPage(new MainPage()); //Avvia la pagina principale
        }

        //Metodo utilizzato come event handler per il tap sul menu da parte dell'utente
        private void ListaMenu_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var menu = e.SelectedItem as Menu; //La variabile menu contiene l'elemento selezionato
            if (menu != null) //Controlla se l'elemento non è null
            {
                /*
                 * In base all'elemento che l'utente ha tappato si avvia la relativa pagina o si effettua il logout
                 */
                if (menu.MenuTitle.Equals("Home"))
                {
                    IsPresented = false;
                    Detail = new NavigationPage(new MainPage()); //Avvia la pagina principale
                }
                else if (menu.MenuTitle.Equals("Scegli Struttura Preferita"))
                {
                    IsPresented = false;
                    Detail = new NavigationPage(new ListaStrutture("Menu")); //Avvia la pagina per la scelta della struttura preferita
                }
                else if (menu.MenuTitle.Equals("Contatti"))
                {
                    IsPresented = false;
                    Detail = new NavigationPage(new ListaContatti()); //Avvia la pagina per la scelta della struttura preferita
                }
            }
        }

        public class Menu
        {
            public string MenuTitle
            {
                get;
                set;
            }

            public string ImageIcon
            {
                get;
                set;
            }
        }
    }
}