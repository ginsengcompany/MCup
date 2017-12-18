using MCup.Model;
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
    public partial class VerificaRicetta : ContentPage
    {

        private List<Prestazioni> prestazioni;

        public VerificaRicetta(Ricetta ricetta)
        {
            InitializeComponent();
            codice_ricetta.Text = ricetta.codice_nre;
            cognome_assistito.Text = ricetta.cognome_assistito;
            nome_assistito.Text = ricetta.nome_assistito;
            codice_fiscale_medico.Text = ricetta.codice_fiscale_medico;
            prestazioni = new List<Prestazioni>();
            foreach (var prest in ricetta.prestazioni){
                prestazioni.Add(new Prestazioni(prest));
            }
            lista_prestazioni.ItemsSource = prestazioni;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}