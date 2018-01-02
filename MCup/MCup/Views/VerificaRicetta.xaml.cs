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
            REST<object, ResponseStrutturaPreferita> connessioneGetStrutturaPreferita = new REST<object, ResponseStrutturaPreferita>();
            ResponseStrutturaPreferita strutturaPreferita = await connessioneGetStrutturaPreferita.GetSingleJson(URL.StrutturaPreferita, App.Current.Properties["tokenLogin"].ToString());
            string struttura = strutturaPreferita.struttura;
            InvioDati invio = new InvioDati(prestazioni, struttura);
            REST<object, StruttureErogatrici> connessione = new REST<object, StruttureErogatrici>();
            List<StruttureErogatrici> listaStruttureErogatrici = new List<StruttureErogatrici>();
            listaStruttureErogatrici = await connessione.PostJsonList(URL.StruttureErogatrici, invio);
            //Se non ci sono strutture erogatrici
            if (listaStruttureErogatrici.Count == 0)
            {
               await DisplayAlert("Attenzione", "nessuna struttura può erogare le prestazioni contenute nella ricetta", "OK");
            }
            //Se la struttura preferita eroga la/le prestazione/prestazioni
            else if(listaStruttureErogatrici[0].esito == true)
            {
                await DisplayAlert("ora", "faccio un pushasync", "a");
            }
            //Se la struttura preferita non eroga la/le prestazione/prestazioni
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


        private class ResponseStrutturaPreferita
        {
            public bool scelta;
            public string struttura;
        }
    }
}