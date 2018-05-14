using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Net.Http;
using MCup;
using MCup.Service;

/*
 * Questa è la pagina principale che permette all'utente di navigare tra i servizi offerti dall'app e dal sistema informatico messo a sua disposizione.
 * La pagina contiene una MasterDetailPage che visualizza all'utente, tramite un menu a tendina laterale, le possibili opzioni messe a disposizione per l'utente.
 */

namespace MCup.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPrincipale: MasterDetailPage
	{
	    private List<Header> listaHeader;

		public MenuPrincipale ()
		{
			InitializeComponent();
		    getLogo();
            inizializzazioneMenu();
		}

	    private async void getLogo()
	    {
	        listaHeader = new List<Header>();
            if (listaHeader.Count != 0)
	        {
	            listaHeader.Clear();
	        }
	        listaHeader.Add(new Header("struttura", "150907"));
	        REST<object, string> connessioneLogo = new REST<object, string>();
	        var logo = await connessioneLogo.getString("http://192.168.125.24:3002/infostruttura/logoStruttura", listaHeader);
	        Image.Source = Xamarin.Forms.ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(logo)));
        }

        public MenuPrincipale(string scelta)
        {
            InitializeComponent();
            inizializzazioneMenu();
            if(scelta.Equals("Contatti"))
                Detail = new NavigationPage(new ListaContatti());
            if(scelta.Equals("Appuntamenti"))
                Detail= new NavigationPage(new PaginaAppuntamenti());
            getLogo();
        }
        //Metodo che inizializza la MasterDetailPage e che inserisce in essa le pagine a cui è possibile accedere dal menu
        private void inizializzazioneMenu()
        {
            List<Menu> menuPrincipale = new List<Menu> //Lista contenente le pagine a cui si può accedere dalla MasterDetailPage
            {
                new Menu { MenuTitle = "Home", ImageIcon = "home.png"},
              //  new Menu { MenuTitle = "Scegli Struttura Preferita", ImageIcon = "modify.png"},
                new Menu { MenuTitle ="Contatti", ImageIcon = "contact.png"},
                new Menu{MenuTitle ="Pagamento", ImageIcon = "soldi.png"}

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
                else if (menu.MenuTitle.Equals("Pagamento"))
                {
                    IsPresented = false;
                   // Detail = new NavigationPage(new Pagamento()); //Avvia la pagina principale
                }
                /*  else if (menu.MenuTitle.Equals("Scegli Struttura Preferita"))
                  {
                      IsPresented = false;
                      Detail = new NavigationPage(new ListaStrutture("Menu")); //Avvia la pagina per la scelta della struttura preferita
                  }*/
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