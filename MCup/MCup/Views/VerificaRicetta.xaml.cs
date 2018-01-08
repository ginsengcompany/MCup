using MCup.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private List<Prestazioni> prestazioni; //Lista delle prestazioni contenute nella ricetta

        //Costruttore della pagina che inizializza e visualizza le informazioni descritte nel commento della pagina
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

        /*
         * Questo metodo invia i dati della ricetta al servizio competente che elabora tali informazioni per controllare se la struttura preferita, scelta dall'utente 
         * durante il suo primo accesso o nella relativa pagina di scelta della struttura preferita in un secondo momento, o le strutture che utilizzano il sistema informatico
         * su cui l'app si basa erogano le prestazioni contenute nella ricetta. Il metodo quindi, gestisce tre casi:
         * 1)La struttura preferita eroga le prestazioni;
         * 2)La struttura preferita non eroga le prestazioni a cui si fa riferimento mentre le altre strutture erogano tali prestazioni;
         * 3)Nessuna struttura eroga le prestazioni a cui si fa riferimento.
         */
        private async void Button_Clicked(object sender, EventArgs e)
        {
            REST<object, ResponseStrutturaPreferita> connessioneGetStrutturaPreferita = new REST<object, ResponseStrutturaPreferita>(); //Crea un oggetto REST
            //La comunicazione GET restituisce la struttura preferita scelta dall'utente
            ResponseStrutturaPreferita strutturaPreferita = await connessioneGetStrutturaPreferita.GetSingleJson(URL.StrutturaPreferita, App.Current.Properties["tokenLogin"].ToString());
            //Salva il codice della struttura preferita nella variabile, di tipo string, struttura
            string struttura = strutturaPreferita.struttura;
            InvioDati invio = new InvioDati(prestazioni, struttura); //Crea un oggetto di tipo InvioDati che contiene le informazioni da inviare come json alla rotta controlloPrestazioni 
            REST<object, StruttureErogatrici> connessione = new REST<object, StruttureErogatrici>(); //Crea un oggetto REST
            //Crea un oggetto di tipo StruttureErogatrici che conterrà la lista delle strutture che erogano o meno le prestazioni a cui si fa riferimento
            List<StruttureErogatrici> listaStruttureErogatrici = new List<StruttureErogatrici>();
            listaStruttureErogatrici = await connessione.PostJsonList(URL.StruttureErogatrici, invio); //Invia i dati della ricetta e la struttura preferita scelta dall'utente
            //Se non ci sono strutture erogatrici
            if (listaStruttureErogatrici.Count == 0)
            {
               await DisplayAlert("Attenzione", "nessuna struttura può erogare le prestazioni contenute nella ricetta", "OK");
            }
            //Se la struttura preferita eroga la/le prestazione/prestazioni
            else if(listaStruttureErogatrici[0].esito == true)
            {
                await Navigation.PushAsync(new PropostaRichiesta(prestazioni));
            }
            //Se la struttura preferita non eroga la/le prestazione/prestazioni
            else
            {
                await DisplayAlert("aldo", "ci sono piu strutture", "aaaaaaaa");
            }
            Debug.WriteLine(listaStruttureErogatrici);
            
        }

        //La classe InvioDati astrae il json da inviare alla rotta controlloPrestazioni per conoscere quali strutture erogano le prestazioni della ricetta
        private class InvioDati
        {
            public List<Prestazioni> prestazioni { get; set; }
            public string strutturaPreferita { get; set; }

            public InvioDati(List<Prestazioni> prestazioni, string strutturaPreferita)
            {
                this.prestazioni = prestazioni;
                this.strutturaPreferita = strutturaPreferita;
            }
        }

        //La classe ResponseStrutturaPreferita contiene se la struttura preferita è stata scelta e in tal caso il codice della struttura
        private class ResponseStrutturaPreferita
        {
            public bool scelta;
            public string struttura;
        }
    }
}