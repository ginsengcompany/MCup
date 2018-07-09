using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MCup.Annotations;
using Plugin.Geolocator;
using Xamarin.Forms;

namespace MCup.Model
{
    public class AppuntamentoPrestazioneProposto : Prestazione,INotifyPropertyChanged
    {
        private Color colore= Color.FromHex("#0971B2");
        private Color coloreTesto = Color.White;
        private ImageSource immagineBloccoData = "unlocked.png";

        public string dataAppuntamento { get; set; }
        public string oraAppuntamento { get; set; }
        public string posizione { get; set; }
        public bool disponibile { get; set; }
        public bool esitoNote { get; set; } = false;
        public bool visibleNote { get; set; } = true;
        public bool bloccoData { get; set; } = false;



        public void copiaAppuntamentoPrestazioneProposto(AppuntamentoPrestazioneProposto appuntamento)
        {
            dataAppuntamento = appuntamento.dataAppuntamento;
            oraAppuntamento = appuntamento.oraAppuntamento;
            posizione = appuntamento.posizione;
            disponibile = appuntamento.disponibile;
            esitoNote = appuntamento.esitoNote;
        }

        public Color coloreNote
        {
            get { return colore; }
            set
            {
                colore = value;
                OnPropertyChanged();
               
            }
        }
        public ImageSource ImmagineBloccoData
        {
            get { return immagineBloccoData; }
            set
            {
                immagineBloccoData = value;
                OnPropertyChanged();

            }
        }

        public Color coloreTestoNote
        {
            get { return coloreTesto; }
            set
            {
                coloreTesto = value;
                OnPropertyChanged();

            }
        }
        public ICommand AccettaNote
        {
            get
            {
                return new Command(async () =>
                {
                    esitoNote = await App.Current.MainPage.DisplayAlert("Attenzione", nota, "ho preso visione", "non ho preso visione");
                    if (esitoNote)
                    {
                         coloreNote=Color.Green;
                        coloreTestoNote=Color.White;
                    }
                    else
                    {
                        coloreNote = Color.Red;
                        coloreTestoNote = Color.White;
                    }
                });
            }
        }
        public ICommand BloccaData
        {
            get
            {
                return new Command(async () =>
                {
                    if (bloccoData == true)
                    {
                        ImmagineBloccoData = "unlocked.png";
                        bloccoData = false;
                    }
                    else
                    {

                        ImmagineBloccoData = "locked.png";
                        bloccoData = true;
                    }
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
