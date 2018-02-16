using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
            if (Device.RuntimePlatform == Device.iOS)
            {
                url = string.Format("http://maps.apple.com/maps?q={0},{1}", reparto.latitudine, reparto.longitudine);

            }
            else
            {
                url = string.Format("http://maps.google.com/maps?q={0},{1}", reparto.latitudine, reparto.longitudine);
            }
            Device.OpenUri(new Uri(url));
        }

    }
}
