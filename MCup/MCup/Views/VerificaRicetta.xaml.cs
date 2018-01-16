using MCup.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCup.ModelView;
using MCup.Service;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

/*
 * Questa pagina Visualizza all'utente il contenuto della ricetta a cui fa riferimento in base ai dati inseriti nella pagina precedente (FormPrenotazione).
 * Le informazioni che vengono visualizzate sono: codice della ricetta, cognome e nome dell'assistito, codice fiscale del medico e la lista delle prestazioni
 * contenute nella ricetta. La pagina non utilizza l'MWWM.
 */

namespace MCup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerificaRicetta : ContentPage
    {

        private VerificaRicettaModelView ModelViewVerifica;

        //Costruttore della pagina che inizializza e visualizza le informazioni descritte nel commento della pagina
        public VerificaRicetta(Ricetta ricetta)
        {
            InitializeComponent();
            ModelViewVerifica = new VerificaRicettaModelView(ricetta, this);
            BindingContext = ModelViewVerifica;
        }

        private void Picker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var a = sender as Picker;
            var b = a.SelectedItem as Reparto;
            ModelViewVerifica.selectedReparto(b);
        }

    }
}