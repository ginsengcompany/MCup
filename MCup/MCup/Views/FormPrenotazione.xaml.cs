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

/*
 * Questa pagina presenta all'utente una form da compilare con le informazioni riguardanti la ricetta e con quale nominativo utilizzarla.
 * La pagina prevede l'inserimento dei dati anagrafici relativi alla persona che intende utilizzare la ricetta e i codici relativi ad essa.
 */

namespace MCup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormPrenotazione : ContentPage
    {
        public string imgCodFiscale = "coduno.png"; //Variabile contenente il nome dell'immagine di esempio del codice fiscale
        public string imgCodUno = "coddue.png"; //Variabile contenente il nome dell'immagine di esempio del codice 1 della ricetta
        public string imgCodDue = "codtre.png"; //Variabile contenente il nome dell'immagine di esempio del codice 2 della ricetta
        private FormPrenotazioneModelView form;
        private bool sar;

        //Metodo che visualizza il popup contenente l'immagine di esempio del codice fiscale
        private async void Handle_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushPopupAsync(new PopupInfoScan(imgCodFiscale));
        }

        //Metodo che visualizza il popup contenente l'immagine di esempio del codice 1 della ricetta
        private async void Handle_Tapped_1(object sender, System.EventArgs e)
        {
            await Navigation.PushPopupAsync(new PopupInfoScan(imgCodUno));
        }

        //Metodo che visualizza il popup contenente l'immagine di esempio del codice 2 della ricetta
        private async void Handle_Tapped_2(object sender, System.EventArgs e)
        {
            await Navigation.PushPopupAsync(new PopupInfoScan(imgCodDue));
        }
      
       
        public FormPrenotazione(bool prenotazioPending)
        {
            InitializeComponent();
            stackSar.IsVisible = false;
            form = new FormPrenotazioneModelView(this, prenotazioPending);
            BindingContext = form; //Questa pagina esegue il Binding con la classe FormPrenotazioneModelView
        }

        //Funzione chiamata per scannerizzare il codice fiscale dell'utente
     

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
                ShowFlashButton = false
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
                ShowFlashButton = false
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

        //Funzione chiamata per passare alla pagina VerificaRicetta per visualizzare il contenuto della ricetta all'utente
        public void metodoPush(Impegnativa ricetta, Assistito contatto)
        {
            Navigation.PushAsync(new VerificaRicetta(ricetta, contatto));
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var a = sender as Picker;
            var b = a.SelectedItem as Assistito;
            form.autoCompila(b);
        }

        public void selezionaElemento(Assistito contatto)
        {
            pickerContatti.Title = contatto.nomeCompletoConCodiceFiscale;
            form.autoCompila(contatto);
        }
        /*
private void XfxComboBox_ItemSelected(object sender, SelectedItemChangedEventArgs e)
{
   string x = e.SelectedItem as string;
   form.autoCompila(x);
   ComboNome.Unfocus();
} */
        public void Switch(object sender, ToggledEventArgs e)
        {
            if (switchSar.IsToggled)
            {
                stackSar.IsVisible = true;
                sar = true;
                entryCodiceRicettaSar.IsVisible = true;
                stackNRE.IsVisible = false;
            }
            else
            {
                entryCodiceRicettaSar.IsVisible = false;
                stackSar.IsVisible = false;
                stackNRE.IsVisible = true;
                sar = false;
            }
        }

        public bool isSwitch()
        {
            return sar;
        }

        private async void ScanSarMethod(object sender, EventArgs e)
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
                ShowFlashButton = false
            };
            overlay.BindingContext = overlay;
            var scanPage = new ZXingScannerPage(options, overlay);
            scanPage.OnScanResult += (result) =>
            {
                scanPage.IsScanning = false;
                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PopAsync();
                    entryCodiceRicettaSar.Text = result.Text;
                });
            };
            await Navigation.PushAsync(scanPage);
        }
    }
}