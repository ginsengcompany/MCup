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
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListaStrutture : ContentPage
	{   // Le cose commentate le ho messe perchè ora non sono implementabili dato che non abbiamo i servizi
        // List<Struttura> listaStrutture = new List<Struttura>();
        List<Struttura> listaDiProva = new List<Struttura>();
        REST<Struttura> connessione = new REST<Struttura>();
        string url =" http://192.168.125.39:3000/strutture";
        public ListaStrutture ()
		{
			InitializeComponent ();
            riempimentoStruttura();
            
		}
        public async void riempimentoStruttura()
        {
          //  CreazioneGrigliaStrutture grigliaStruttura1 = new CreazioneGrigliaStrutture();
          //   grigliaStruttura1.CreazioneGriglia(grigliaStrutture);
                listaDiProva = await connessione.GetJson(url);
                Struttura ciao = new Struttura();
            ImageSource imgSrc="";

            foreach (var i in listaDiProva)
            {
                imgSrc = Xamarin.Forms.ImageSource.FromStream(
           () => new MemoryStream(Convert.FromBase64String(i.Logo_struttura)));
             
                
                i.imgStruttura = imgSrc;

            }
            ListaStruttura.SeparatorColor = Color.Black;

            ListaStruttura.ItemsSource = listaDiProva;



        }



    }
}