using MCup.Model;
using MCup.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MCup.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModificaContatto : ContentPage
    {

        private ModificaContattoModelView model;

        public ModificaContatto (Assistito info)
		{
            InitializeComponent();
            PickerComuneResidenza.IsEnabled = false;
            model = new ModificaContattoModelView(info);
            riempiPicker(info);

            BindingContext = model;
        }

        private void riempiPicker(Assistito info)
        {
            StatoCivile statoCivile = new StatoCivile();
            statoCivile.id = info.codStatoCivile;
            statoCivile.descrizione = info.statocivile;
            PickerStatoCivile.SelectedItem = statoCivile;
            PickerStatoCivile.Title = statoCivile.descrizione;

            PickerProvinciaResisdenza.Title = info.provincia;
            PickerComuneResidenza.Title = info.comune_residenza;


        }
        private void Picker_SelectedIndexChangedProvinciaNascita(object sender, EventArgs e)
        {
            
            var a = sender as Picker;
            var b = a.SelectedItem as Provincia;
            model.LeggiComuniNascita(b);
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
            PickerProvinciaResisdenza.Title = "seleziona la provincia";
            var a = sender as Picker;
            var b = a.SelectedItem as Provincia;
            model.LeggiComuniResidenza(b);
            PickerComuneResidenza.Title = "inserisci nuovo comune di residenza";
            PickerComuneResidenza.IsEnabled = true;
            model.comuneResidenzaSelezionato(new Comune());
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