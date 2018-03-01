using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCup.CustomPopUp;
using MCup.Model;
using MCup.ModelView;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace MCup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormPrenotazioneAsl : ContentPage
    {
        public string imgCodFiscale = "coduno.png"; //Variabile contenente il nome dell'immagine di esempio del codice fiscale
        public string imgCodUno = "coddue.png"; //Variabile contenente il nome dell'immagine di esempio del codice 1 della ricetta
        public string imgCodDue = "codtre.png"; //Variabile contenente il nome dell'immagine di esempio del codice 2 della ricetta

        private FormPrenotazioneAslModelView form;
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
        public FormPrenotazioneAsl(bool prenotazioPending)
        {
            InitializeComponent();
            form = new FormPrenotazioneAslModelView(this,prenotazioPending);
            BindingContext = form; //Questa pagina esegue il Binding con la classe FormPrenotazioneModelView
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


        private void InfoNomeMedico(object sender, EventArgs e)
        {
            DisplayAlert("Attenzione","Questo campo è facoltativo, servirà per una ricerca più specifica tra le strutture","ok");
        }
    }
}