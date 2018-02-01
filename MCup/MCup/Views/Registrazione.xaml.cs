using MCup.Model;
using MCup.ModelView;
using MCup.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

/*
 * La corrente pagina viene visualizzata solo se il dispositivo su cui si esegue l'app è di tipo Android. 
 * Questa Pagina permette all'utente di poter registrare una nuova utenza per poter poi effettuare la login
 * per accedere ai servizi.
 */

namespace MCup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registrazione : CarouselPage
    {
        private RegistrazioneModelView modelView;
        public string dataDiNascita;
        public Registrazione()
        {
            InitializeComponent();
            modelView = new RegistrazioneModelView();
            this.Children.Clear();
            this.Children.Add(page0);
            BindingContext = modelView; //Questa pagina utilizza l'MWWM, ed effettua il binding delle informazioni con la classe RegistrazioneModelView.
            PickerComuneNascita.IsEnabled = false;
            PickerComuneResidenza.IsEnabled = false;
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {

            var options = new MobileBarcodeScanningOptions
            {
                UseFrontCameraIfAvailable = false,
                TryHarder = true,
                AutoRotate = false,
                DisableAutofocus = false
                //PossibleFormats = new List < BarcodeFormat >(){ BarcodeFormat.CODE_39 }
            };
            var overlay = new ZXingDefaultOverlay
            {
                ShowFlashButton = false,

            };
            overlay.BindingContext = overlay;
            var scanPage = new ZXingScannerPage(options, overlay);
            scanPage.OnScanResult += (result) =>
            {
                scanPage.IsScanning = false;
                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PopAsync();
                    entryCodiceFiscale.Text = result.Text;
                });
            };
            await Navigation.PushAsync(scanPage);
        }

        private void Picker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var a = sender as Picker;
            if (a.SelectedIndex > -1)
            {
                var b = a.SelectedItem as Comune;
                modelView.comuneNascitaSelezionato(b);
            }
        }

        private void Picker_OnSelectedIndexChanged1(object sender, EventArgs e)
        {
            var a = sender as Picker;
            if (a.SelectedIndex > -1)
            {
                var b = a.SelectedItem as Comune;
                modelView.comuneResidenzaSelezionato(b);
            }
            
        }

        private void Picker_OnSelectedIndexChangedProvinciaNascita(object sender, EventArgs e)
        {
            var a = sender as Picker;
            var b = a.SelectedItem as string;
            modelView.provinciaDiNascitaSelezionato(b);
            PickerComuneNascita.IsEnabled = true;
        }

        private void Picker_OnSelectedIndexChangedProvinciaResidenza(object sender, EventArgs e)
        {
            var a = sender as Picker;
            var b = a.SelectedItem as string;
            modelView.provinciaDiResidenzaSelezionato(b);
            PickerComuneResidenza.IsEnabled = true;
        }

        private void Picker_OnSelectedIndexChangedSceltaUnione(object sender, EventArgs e)
        {
            var a = sender as Picker;
            var b = a.SelectedItem as StatoCivile;
            modelView.StatoCivileScelto(b);
        }

        private async void AvantiPrimaPagina(object sender, EventArgs e)
        {
            if (await modelView.VaiAvanti())
            {
                this.Children.Clear();
                this.Children.Add(page1);
                CurrentPage = page1;
            }
        }

        private void IndietroPrimaPagina(object sender, EventArgs e)
        {
            this.Children.Clear();
            this.Children.Add(page0);
            CurrentPage = page0;
        }

        private void IndietroSecondaPagina(object sender, EventArgs e)
        {
            this.Children.Clear();
            this.Children.Add(page1);
            CurrentPage = page1;
        }

        private async void AvantiSecondaPagina(object sender, EventArgs e)
        {
            if (await modelView.AvantiSecondaPagina())
            {
                this.Children.Clear();
                this.Children.Add(page3);
                CurrentPage = page3;
            }
        }
    }
}