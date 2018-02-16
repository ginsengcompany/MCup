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
    public class AppuntamentoPrestazioneProposto: Prestazione
    {
        public string dataAppuntamento { get; set; }
        public string oraAppuntamento { get; set; }
        public string posizione { get; set; }
        public bool disponibile { get; set; }
        public ICommand LuogoUbicazioneReparto
        {
            get
            {
                return new Command(async () =>
                {
                    RiceviLuogo(reparti[0]);
                });
            }
        }

        private async Task RiceviLuogo(Reparto reparto)
        {
            string url;
            var locator = CrossGeolocator.Current;
            var position = await locator.GetLastKnownLocationAsync();
            if (position != null)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    url = string.Format("https://www.google.com/maps/dir/?api=1&origin={0},{1}&destination={2},{3}&travelmode=car", position.Latitude.ToString().Replace(',','.'),position.Longitude.ToString().Replace(',', '.'), reparto.latitudine, reparto.longitudine);

                }
                else
                {
                    url = string.Format("https://www.google.com/maps/dir/?api=1&origin={0},{1}&destination={2},{3}&travelmode=car", position.Latitude.ToString().Replace(',', '.'), position.Longitude.ToString().Replace(',', '.'), reparto.latitudine, reparto.longitudine);
                }
                Device.OpenUri(new Uri(url));
            }

            
        }

    }
}
