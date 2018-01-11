using System;
using System.Collections.Generic;
using System.ComponentModel;
using MCup.ModelView;
using Xamarin.Forms;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

/*
 * Questa pagina viene visualizzata all'avvio dell'app solo se il dispositivo è di tipo IOS. Questa pagina gestisce l'autenticazione dell'utente
 * per poter accedere alla sua area privata e quindi utilizzare i servizi che l'app fornisce.
 */

namespace MCup.Views
{
    public partial class RegistrazioneIOS : ContentPage
    {
        public RegistrazioneIOS()
        {
            InitializeComponent();
            BindingContext = new RegistrazioneModelView(); //Questa pagina utilizza l'MWWM ed effettua il binding con la classe LoginModelView
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
    }
}
