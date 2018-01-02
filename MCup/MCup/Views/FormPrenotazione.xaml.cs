using Lamp.Plugin;
using MCup.Model;
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
using MCup.CustomPopUp;
using Rg.Plugins.Popup.Extensions;

//Questa pagina esegue il Binding con la ModelView FormPrenotazioneModelView.cs

namespace MCup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormPrenotazione : ContentPage
    {
        public string imgCodFiscale = "coduno.png";
        public string imgCodUno = "coddue.png";
        public string imgCodDue = "codtre.png";

        private async void Handle_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushPopupAsync(new PopupInfoScan("coduno.png"));
            await Navigation.PushPopupAsync(new PopupInfoScan(imgCodFiscale));
            //DisplayAlert("Info", "L'icona qui di fianco serve a fare una scansione del codice a barre situato sul retro della Tessera Sanitaria", "OK");
        }

        private async void Handle_Tapped_1(object sender, System.EventArgs e)
        {
            await Navigation.PushPopupAsync(new PopupInfoScan("coddue.png"));

            await Navigation.PushPopupAsync(new PopupInfoScan(imgCodUno));

            //DisplayAlert("Info", "L'icona qui di fianco serve a fare una scansione del primo codice a barre situato sul fronte della Ricetta Medica", "OK");
        }

        private async void Handle_Tapped_2(object sender, System.EventArgs e)
        {
            await Navigation.PushPopupAsync(new PopupInfoScan("codtre.png"));
            await Navigation.PushPopupAsync(new PopupInfoScan(imgCodDue));

           // DisplayAlert("Info","L'icona qui di fianco serve a fare una scansione del secondo codice a barre presente sul fronte della Ricetta Medica","OK");
        }

        public FormPrenotazione()
        {
            InitializeComponent();
            BindingContext = new FormPrenotazioneModelView(this);
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
                    entryCodiceRicettaDue.Text = result.Text;
                });
            };
            await Navigation.PushAsync(scanPage);
        }

        //Funzione chiamata per passare alla pagina ListaDatePrestazioni per visualizzare la lista dei giorni e degli orari in cui effettuare la richiesta di prenotazione
        public void metodoPush(Ricetta ricetta)
        {
            Navigation.PushAsync(new VerificaRicetta(ricetta));
        }
    }
}