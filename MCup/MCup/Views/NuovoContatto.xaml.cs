using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCup.Model;
using MCup.ModelView;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace MCup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NuovoContatto : ContentPage
    {

        private NuovoContattoModelView model;

        public NuovoContatto()
        {
            InitializeComponent();
            PickerComuneNascita.IsEnabled = false;
            PickerComuneResidenza.IsEnabled = false;
            model = new NuovoContattoModelView();
            BindingContext = model;
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

        private void Picker_SelectedIndexChangedProvinciaNascita(object sender, EventArgs e)
        {
            var a = sender as Picker;
            var b = a.SelectedItem as string;
            model.LeggiComuniNascita(b);
            PickerComuneNascita.IsEnabled = true;
        }

        private void Picker_SelectedIndexChangedComuneNascita(object sender, EventArgs e)
        {
            var a = sender as Picker;
            if (a.SelectedIndex > -1)
            {
                var b = a.SelectedItem as Comune;
                model.comuneNascitaSelezionato(b);
            }
        }

        private void Picker_SelectedIndexChangedProvinciaResidenza(object sender, EventArgs e)
        {
            var a = sender as Picker;
            var b = a.SelectedItem as string;
            model.LeggiComuniResidenza(b);
            PickerComuneResidenza.IsEnabled = true;
        }

        private void Picker_SelectedIndexChangedComuneResidenza(object sender, EventArgs e)
        {
            var a = sender as Picker;
            if (a.SelectedIndex > -1)
            {
                var b = a.SelectedItem as Comune;
                model.comuneResidenzaSelezionato(b);
            }
        }

        private void Picker_OnSelectedIndexChangedSceltaUnione(object sender, EventArgs e)
        {
            var a = sender as Picker;
            var b = a.SelectedItem as StatoCivile;
            model.StatoCivileScelto(b);
        }

    }
}