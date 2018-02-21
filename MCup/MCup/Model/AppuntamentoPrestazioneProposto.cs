using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Geolocator;
using Xamarin.Forms;

namespace MCup.Model
{
    public class AppuntamentoPrestazioneProposto : Prestazione
    {
        public string dataAppuntamento { get; set; }
        public string oraAppuntamento { get; set; }
        public string posizione { get; set; }
        public bool disponibile { get; set; }
        public bool esitoNote { get; set; } = false;
        public ICommand AccettaNote
        {
            get
            {
                return new Command(async () =>
                {
                    esitoNote = await App.Current.MainPage.DisplayAlert("Attenzione", nota, "accetto", "declino");
                    
                });
            }
        }
        
    }
}
