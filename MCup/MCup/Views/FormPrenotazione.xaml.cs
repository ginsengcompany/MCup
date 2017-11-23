using Lamp.Plugin;
using MCup.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

//Questa pagina esegue il Binding con la ModelView FormPrenotazioneModelView.cs

namespace MCup.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FormPrenotazione : ContentPage
	{
		public FormPrenotazione ()
		{
			InitializeComponent ();
            BindingContext = new FormPrenotazioneModelView();
		}

        //Funzione chiamata per scannerizzare il codice fiscale dell'utente
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
                ShowFlashButton = true,
                
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

        private void ScanPage_OnScanResult(ZXing.Result result)
        {
            throw new NotImplementedException();
        }

        //Funzione chiamata per scannerizzare il primo codice della ricetta
        private async void Button_Clicked_1(object sender, EventArgs e)
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
                ShowFlashButton = true
            };
            overlay.BindingContext = overlay;
            var scanPage = new ZXingScannerPage(options, overlay);
            scanPage.OnScanResult += (result) =>
            {
                scanPage.IsScanning = false;
                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PopAsync();
                    entryCodiceRicettaUno.Text = result.Text;
                });
            };
            await Navigation.PushAsync(scanPage);
        }

        //Funzione chiamata per scannerizzare il secondo codice della ricetta
        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            var options = new MobileBarcodeScanningOptions
            {
                UseFrontCameraIfAvailable = false,
                TryHarder = true,
                AutoRotate = false,
                DisableAutofocus = false
                //PossibleFormats = new List < BarcodeFormat >(){ BarcodeFormat.CODE_39 }
            };
            var overlay = new ZXingDefaultOverlay {
                ShowFlashButton = true
            };
            overlay.BindingContext = overlay;
            var scanPage = new ZXingScannerPage(options, overlay);
            scanPage.OnScanResult += (result) =>
            {
                scanPage.IsScanning = false;
                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PopAsync();
                    entryCodiceRicettaDue.Text = result.Text;
                });
            };
            await Navigation.PushAsync(scanPage);
        }
    }
}