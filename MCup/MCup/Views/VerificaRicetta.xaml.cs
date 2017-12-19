using MCup.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCup.Database.Data;
using MCup.Service;
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

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var listStruttura = StrutturePreferite.VisualizzaStrutturePreferite();
            string struttura = listStruttura[0].id.ToString();

            InvioDati invio = new InvioDati(prestazioni, struttura);
            ;
            REST<object, StruttureErogatrici> connessione = new REST<object, StruttureErogatrici>();
            List<StruttureErogatrici> listaStruttureErogatrici = new List<StruttureErogatrici>();
            listaStruttureErogatrici = await connessione.PostJsonList(URL.StruttureErogatrici, invio);
            if (listaStruttureErogatrici.Count == 0)
            {
               await DisplayAlert("Attenzione", "nessuna struttura può erogare il servizio", "ritenta");
            }
            else if(listaStruttureErogatrici[0].codice_struttura == listStruttura[0].id)
            {
                await DisplayAlert("ora", "faccio un pushasync", "ricchio");
            }
            else
            {
                await DisplayAlert("aldo", "ci sono piu strutture", "aaaaaaaa");
            }
            Debug.WriteLine(listaStruttureErogatrici);
            
        }

        class InvioDati
        {
            public List<Prestazioni> prestazioni { get; set; }
            public string strutturaPreferita { get; set; }

            public InvioDati(List<Prestazioni> prestazioni, string strutturaPreferita)
            {
                this.prestazioni = prestazioni;
                this.strutturaPreferita = strutturaPreferita;
                 
            }
        }
    }
}