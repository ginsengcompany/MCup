using MCup.Model;
using MCup.Service;
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
	public partial class ListaStrutture : ContentPage
	{   // Le cose commentate le ho messe perchè ora non sono implementabili dato che non abbiamo i servizi
        // List<Struttura> listaStrutture = new List<Struttura>();
        List<Struttura> listaDiProva = new List<Struttura>();
        

        public ListaStrutture ()
		{
			InitializeComponent ();
            riempimentoStruttura();
            
		}
        public async void riempimentoStruttura()
        {
            CreazioneGrigliaStrutture grigliaStruttura1 = new CreazioneGrigliaStrutture();
             grigliaStruttura1.CreazioneGriglia(grigliaStrutture);
        }

        
  
    }
}