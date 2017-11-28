using MCup.Database.Data;
using MCup.Database.Models;
using MCup.Model;
using MCup.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MCup.Views
{
    /**
     * Questa page implementa una list view di strutture prenotabili dall'utente, la lista di strutture è riempita dal risultato di una GET. 
     */
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListaStrutture : ContentPage
	{   
        /**
         * @param: listaDiProva = lista che verrà riempita dal risultato della connessione
         * @param connessione= oggetto di tipo REST
         * @param url= stringa che contiene l'url a cui ci riferiremo nella get
         */
        List<Struttura> listaDiProva = new List<Struttura>();
        REST<Struttura> connessione = new REST<Struttura>();
        public ListaStrutture ()
		{
			InitializeComponent ();
            riempimentoStruttura();
            
		}

        /**
         * Metodo riempimentoStruttura(), crea la connessione col server, 
         * converte le immagini in 64 bit e le inserisce nella nostra listview
         */
        public async void riempimentoStruttura()
        {
            caricamentoPagina.IsRunning = true;
            caricamentoPagina.IsVisible = true;
            listaDiProva = await connessione.GetJson(URL.Strutture);
            ImageSource imgSrc="";

            foreach (var i in listaDiProva)
            {
                imgSrc = Xamarin.Forms.ImageSource.FromStream(
           () => new MemoryStream(Convert.FromBase64String(i.Logo_struttura)));
                i.imgStruttura = imgSrc;
            }
            ListaStruttura.SeparatorColor = Color.Black;
            caricamentoPagina.IsRunning = false;
            caricamentoPagina.IsVisible = false;
            ListaStruttura.ItemsSource = listaDiProva;

        }
        /**
         * Metodo tapped, recupera l'informazione dal tap dell'utente nella nostra list view. Salva in locale l'elemento tappato. 
         */

        private void ListaStruttura_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            int count = StrutturePreferite.GetCountStrutturePreferite();
            Struttura elemTapped = e.Item as Struttura;
            if (count > 0)
            {
                TbStrutturePreferite struttura = new TbStrutturePreferite(elemTapped.Codice_struttura, elemTapped.Nome_struttura);
                StrutturePreferite.UpdateStrutturaPreferita(elemTapped.Nome_struttura, elemTapped.Codice_struttura);
                DisplayAlert("STRUTTURA PREFERITA", "Hai selezionato: " + struttura.NomeStruttura, "OK");
            }
            else
            {
                TbStrutturePreferite struttura = new TbStrutturePreferite(elemTapped.Codice_struttura, elemTapped.Nome_struttura);
                StrutturePreferite.InserisciStrutturaPreferita(struttura);
                Navigation.PushModalAsync(new MenuPrincipale());
            }  
        }
    }
}