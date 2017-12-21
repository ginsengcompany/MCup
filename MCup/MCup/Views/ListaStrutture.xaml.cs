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
        List<Struttura> listaStrutture = new List<Struttura>();
        REST<Object,Struttura> connessioneListaStrutture = new REST<Object,Struttura>();
        ResponseStrutturaPreferita StrutturaPreferita;
        private string provenienza;

        public ListaStrutture (string provenienza)
		{
			InitializeComponent ();
            this.provenienza = provenienza;
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
            try
            {
                listaStrutture = await connessioneListaStrutture.GetJson(URL.Strutture);
                ImageSource imgSrc = "";
                foreach (var i in listaStrutture)
                {
                    imgSrc = Xamarin.Forms.ImageSource.FromStream(
               () => new MemoryStream(Convert.FromBase64String(i.Logo_struttura)));
                    i.imgStruttura = imgSrc;
                }
                ListaStruttura.SeparatorColor = Color.Black;
                caricamentoPagina.IsRunning = false;
                caricamentoPagina.IsVisible = false;
                ListaStruttura.ItemsSource = listaStrutture;
                REST<object, ResponseStrutturaPreferita> connessioneStrutturaPreferita = new REST<object, ResponseStrutturaPreferita>();
                StrutturaPreferita = await connessioneStrutturaPreferita.GetSingleJson(URL.StrutturaPreferita, App.Current.Properties["tokenLogin"].ToString());
                if (StrutturaPreferita.scelta)
                    StrutturaPreferitaScelta();
            }
            catch (Exception)
            {
                await DisplayAlert("Attenzione", "connessione non riuscita", "riprova");
                riempimentoStruttura();
            }
        }

        private void StrutturaPreferitaScelta()
        {
            string idStruttura = StrutturaPreferita.struttura;
            foreach(var i in listaStrutture)
            {
                if (idStruttura == i.Codice_struttura)
                    ListaStruttura.SelectedItem = i;
            }
        }

        /**
         * Metodo tapped, recupera l'informazione dal tap dell'utente nella nostra list view. Salva in locale l'elemento tappato.
         */
        private async void ListaStruttura_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                Struttura elemTapped = e.Item as Struttura;
                REST <ResponseStrutturaPreferita, Utente> connessioneSceltaStruttura = new REST<ResponseStrutturaPreferita, Utente>();
                ResponseStrutturaPreferita responseStrutturaPreferita = new ResponseStrutturaPreferita();
                responseStrutturaPreferita.struttura = elemTapped.Codice_struttura;
                Utente response = await connessioneSceltaStruttura.PostJson(URL.StrutturaPreferita, responseStrutturaPreferita,App.Current.Properties["tokenLogin"].ToString());
                await DisplayAlert("Struttura preferita", connessioneSceltaStruttura.warning, "OK");
                StrutturaPreferita.struttura = elemTapped.Codice_struttura;
                ListaStruttura.SelectedItem = null;
                StrutturaPreferitaScelta();
                if (provenienza == "Login")
                    App.Current.MainPage = new NavigationPage(new MenuPrincipale());
            }
            catch (Exception)
            {
                await DisplayAlert("Attenzione", "connessione non riuscita", "riprova");
            }
        }

        private class ResponseStrutturaPreferita
        {
            public bool scelta;
            public string struttura;
        }
    }
}