using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Net;
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
	        var logo = await connessioneLogo.getString("http://ecuptservice.ak12srl.it/infostruttura/logoStruttura", listaHeader);
            //Image.Source = Xamarin.Forms.ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(logo)));
            Image.Source = "logo.png";
        }

        public MenuPrincipale(string scelta)
        {
            InitializeComponent();
            inizializzazioneMenu();
            if(scelta.Equals("Contatti"))
                Detail = new NavigationPage(new ListaContatti());
            if(scelta.Equals("Appuntamenti"))
                Detail= new NavigationPage(new PaginaAppuntamenti());
            if (scelta.Equals("Prenota"))
                Detail = new NavigationPage(new FormPrenotazione(false));
            getLogo();
        }
        //Metodo che inizializza la MasterDetailPage e che inserisce in essa le pagine a cui è possibile accedere dal menu
        private void inizializzazioneMenu()
        {
            List<Menu> menuPrincipale = new List<Menu> //Lista contenente le pagine a cui si può accedere dalla MasterDetailPage
            {
                new Menu { MenuTitle = "Home", ImageIcon = "home.png"},
                new Menu{MenuTitle ="Prenota", ImageIcon = "prenotaMenu.png"},
                new Menu{MenuTitle ="Lista appuntamenti", ImageIcon = "appuntamentiMenu.png"},
                new Menu{ MenuTitle ="Rubrica", ImageIcon = "rubrica.png"},
                new Menu{MenuTitle ="Pagamento", ImageIcon = "soldi.png"},
                new Menu{MenuTitle ="Lista Referti", ImageIcon = "refertiMenu.png"},
                new Menu{MenuTitle = "Gestione Account", ImageIcon = "contact.png"},
                new Menu{MenuTitle = "Video tutorial", ImageIcon = "videotutorial.png"},
                new Menu{MenuTitle = "Info e contatti", ImageIcon = "help.png"},
                new Menu{MenuTitle = "Logout", ImageIcon = "logout.png"}
            };
            ListaMenu.ItemsSource = menuPrincipale; //Assegna all'oggetto ListaMenu dello xaml della pagina la lista precedentemente inizializzata
            Detail = new NavigationPage(new MainPage()); //Avvia la pagina principale
        }

        //Metodo utilizzato come event handler per il tap sul menu da parte dell'utente
        private async void ListaMenu_ItemSelected(object sender, SelectedItemChangedEventArgs e)
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
                   Detail = new NavigationPage(new PaginaLavoriInCorso()); //Avvia la pagina principale
                }
                else if (menu.MenuTitle.Equals("Video tutorial"))
                {
                      IsPresented = false;
                      Detail = new NavigationPage(new Help()); //Avvia la pagina per la scelta della struttura preferita
                }
                else if (menu.MenuTitle.Equals("Rubrica"))
                {
                    IsPresented = false;
                    Detail = new NavigationPage(new ListaContatti()); //Avvia la pagina per la scelta della struttura preferita
                }
                else if (menu.MenuTitle.Equals("Lista appuntamenti"))
                {
                    IsPresented = false;
                    Detail = new NavigationPage(new PaginaAppuntamenti()); //Avvia la pagina per la scelta della struttura preferita
                }
                else if (menu.MenuTitle.Equals("Prenota"))
                {
                    IsPresented = false;
                    Detail = new NavigationPage(new FormPrenotazione(false)); //Avvia la pagina per la scelta della struttura preferita
                }
                else if (menu.MenuTitle.Equals("Lista Referti"))
                {
                    IsPresented = false;
                    Detail = new NavigationPage(new PaginaReferti()); //Avvia la pagina per la scelta della struttura preferita
                }
                else if (menu.MenuTitle.Equals("Gestione Account"))
                {
                    IsPresented = false;
                    Detail = new NavigationPage(new PaginaPrivacy());
                }
                else if (menu.MenuTitle.Equals("Info e contatti"))
                {
                    IsPresented = false;
                    Detail = new NavigationPage(new InfoContattiApp());
                }
                else if(menu.MenuTitle.Equals("Logout"))
                {
                    var risposta = await DisplayAlert("Logout", "Sei sicuro di voler effettuare il logout?", "SI", "NO");
                    if (!risposta)
                        return;
                    List<Header> listaHeader = new List<Header>();
                    listaHeader.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
                    TokenNotification tokNot = new TokenNotification();
                    tokNot.tokenNotification = "";
                    REST<TokenNotification, bool> connessione = new REST<TokenNotification, bool>();
                    bool res = await connessione.PostJson(SingletonURL.Instance.getRotte().updateTokenNotifiche, tokNot, listaHeader);
                    if (connessione.responseMessage != HttpStatusCode.OK)
                    {
                        await App.Current.MainPage.DisplayAlert("Attenzione " + (int)connessione.responseMessage, connessione.warning, "OK");
                    }
                    Application.Current.Properties["flagRimaniLoggato"] = "False";
                    Application.Current.MainPage = new NavigationPage(new Login());
                }
            }
        }

        private class TokenNotification
        {
            public string tokenNotification;
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