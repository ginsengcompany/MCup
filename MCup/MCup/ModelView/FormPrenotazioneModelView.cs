using MCup.Model;
using MCup.Service;
using MCup.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MCup.ModelView
{
    //ModelView della pagina FormPrenotazione, tale classe è utilizzata per implementare il binding con la relativa pagina
    public class FormPrenotazioneModelView : INotifyPropertyChanged
    {
        //Evento che prevede il cambiamento di proprietà all'interno della classe
        public event PropertyChangedEventHandler PropertyChanged;
        private Impegnativa invioImpegnativa;
        private string visible="false";
        private List<Header> headers = new List<Header>();

        private Regex regexNomeCognome = new Regex(@"^[A-Za-zèùàòé][a-zA-Z'èùàòé ]*$");

        //Oggetto che astrae l'utente che intende prenotare una o delle prestazioni
        private Assistito utenza;
        
        //Oggetto che astrae la ricetta NRE
        private InvioRicettaPrenotazione ricetta;

        private bool isenabled;


        private string nameTextErrorNome, nameTextErrorCognome, nameTextErrorCodFisc, nameTextErrorCodUno, nameTextErrorCodDue;

        public string NameTextErrorNome
        {
            get { return nameTextErrorNome; }
            set
            {
                OnPropertyChanged();
                nameTextErrorNome = value;
            }
        }

        public string NameTextErrorCognome
        {
            get { return nameTextErrorCognome; }
            set
            {
                OnPropertyChanged();
                nameTextErrorCognome = value;
            }
        }

        public string NameTextErrorCodFisc
        {
            get { return nameTextErrorCodFisc; }
            set
            {
                OnPropertyChanged();
                nameTextErrorCodFisc = value;
            }
        }

        public string NameTextErrorCodUno
        {
            get { return nameTextErrorCodUno; }
            set
            {
                OnPropertyChanged();
                nameTextErrorCodUno = value;
            }
        }

        public string NameTextErrorCodDue
        {
            get { return nameTextErrorCodDue; }
            set
            {
                OnPropertyChanged();
                nameTextErrorCodDue = value;
            }
        }

        //Oggetto che contiene tutte le informazioni della prenotazione che si vuole effettuare
        private FormPrenotazione model;

        private List<Assistito> contatti = new List<Assistito>();

        private List<Assistito> contacts;

        public List<Assistito> Contatti
        {
            get { return contatti; }
            set
            {
                OnPropertyChanged();
                contatti = new List<Assistito>(value);
            }
        }

        private class InvioRicettaPrenotazione
        {
            public string codice_uno;
            public string codice_due;
        }

        //Proprietà che definisce il nome dell'utente che sta effettuando la prenotazione
        public string nomeUtente
        {
            get { return utenza.nome; }
            set
            {
                utenza.nome = value;
                OnPropertyChanged();
            }
        }

        //Proprietà che definisce il cognome dell'utente che sta effettuando la prenotazione
        public string cognomeUtente
        {
            get { return utenza.cognome; }
            set
            {
                utenza.cognome = value;
                OnPropertyChanged();
            }
        }

        //Proprietà che definisce il codice fiscale dell'utente che sta effettuando la prenotazione
        public string codicefiscaleUtente
        {
            get { return utenza.codice_fiscale; }
            set
            {
                utenza.codice_fiscale = value;
                OnPropertyChanged();
            }
        }

        //Proprietà che definisce il primo codice della ricetta che sta effettuando la prenotazione
        public string codiceUno
        {
            get { return ricetta.codice_uno; }
            set
            {
                ricetta.codice_uno = value;
                OnPropertyChanged();
            }
        }

        //Proprietà che definisce il secondo codice della ricetta che sta effettuando la prenotazione
        public string codiceDue
        {
            get { return ricetta.codice_due; }
            set
            {
                ricetta.codice_due = value;
                OnPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get { return isenabled; }
            set
            {
                OnPropertyChanged();
                isenabled = value;
            }
        }

        //Costruttore del ModelView, viene passato come parametro il riferimento alla pagina che lo richiama per poter effettuare una Navigation.pushAsync
        public FormPrenotazioneModelView(FormPrenotazione Model, bool prenotazionePending)
        {
            IsEnabled = true;
            utenza = new Assistito();
            ricetta = new InvioRicettaPrenotazione();
            invioImpegnativa= new Impegnativa();
            headers.Add(new Header("struttura", "030001"));
            headers.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
            model = Model;
            ricetta.codice_uno = "";
            ricetta.codice_due = "";
            utenza.nome = "";
            utenza.cognome = "";
            utenza.codice_fiscale="";
            leggiContatti();
            if (prenotazionePending)
            {
             RiempiPagina();
            }
          
            InviaRichiesta = new Command(async () =>
            {
                IsEnabled = false;
                await InvioDatiAsync();
                IsEnabled = true;
            });
            
        }

        private async void leggiContatti()
        {
            REST<object, Assistito> rest = new REST<object,Assistito>();
            List<Header> listaHeader = new List<Header>();
            listaHeader.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
            contacts = await rest.GetListJson(SingletonURL.Instance.getRotte().InfoPersonali,listaHeader);
            Contatti = contacts;
        }

        public async void RiempiPagina()
        {
            REST<object, Impegnativa> connessione = new REST<object, Impegnativa>();
            List<Header> listaHeader = new List<Header>();
            listaHeader.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
            Impegnativa response = await  connessione.GetSingleJson(SingletonURL.Instance.getRotte().ricezioneDatiPrenotazione, listaHeader);
            model.selezionaElemento(response.assistito);
            codiceUno = response.nre.Substring(0, 5);
            codiceDue = response.nre.Substring(5);
        }

        //Comando che chiama la funzione asincrona InvioDatiAsync()
        public ICommand InviaRichiesta { protected set; get; }

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        //Funzione utilizzata per l'invio della richiesta di prenotazione al servizio
        public async Task InvioDatiAsync ()
        {
            bool passControl = true;
            if (string.IsNullOrEmpty(utenza.nome))
            {
                NameTextErrorNome = "Il campo nome è obbligatorio";
                passControl = false;
            }
            else if (!regexNomeCognome.IsMatch(utenza.nome))
            {
                NameTextErrorNome = "Il campo nome contiene caratteri non validi";
                passControl = false;
            }
            else
                NameTextErrorNome = "";
            if (string.IsNullOrEmpty(utenza.cognome))
            {
                NameTextErrorCognome = "Il campo cognome è obbligatorio";
                passControl = false;
            }
            else if (!regexNomeCognome.IsMatch(utenza.cognome))
            {
                NameTextErrorCognome = "Il campo cognome contiene caratteri non validi";
                passControl = false;
            }
            else
                NameTextErrorCognome = "";
            if (string.IsNullOrEmpty(utenza.codice_fiscale))
            {
                NameTextErrorCodFisc = "Il campo codice fiscale è obbligatorio";
                passControl = false;
            }
            else if (utenza.codice_fiscale.Length != 16)
            {
                NameTextErrorCodFisc = "Il campo codice fiscale non è corretto";
                passControl = false;
            }
            else
                NameTextErrorCodFisc = "";
            if (string.IsNullOrEmpty(ricetta.codice_uno))
            {
                NameTextErrorCodUno = "Il campo è obbligatorio";
                passControl = false;
            }
            else if (ricetta.codice_uno.Length != 5)
            {
                NameTextErrorCodUno = "Il campo deve contentere un codice impegnativa valido";
                passControl = false;
            }
            else
                NameTextErrorCodUno = "";
            if (string.IsNullOrEmpty(ricetta.codice_due))
            {
                NameTextErrorCodDue = "Il campo è obbligatorio";
                passControl = false;
            }
            else if (ricetta.codice_due.Length != 10)
            {
                NameTextErrorCodDue = "Il campo deve contentere un codice impegnativa valido";
                passControl = false;
            }
            else
                NameTextErrorCodDue = "";
            if (passControl)
            {
                try
                {
                    REST<Impegnativa, Impegnativa> connessione = new REST<Impegnativa, Impegnativa>();
                    invioImpegnativa.nre = codiceUno+codiceDue;
                    Impegnativa response = await connessione.PostJson(SingletonURL.Instance.getRotte().Ricetta, invioImpegnativa,headers);
                    if ((response == null) || (response == default(Impegnativa)))
                    {
                       await App.Current.MainPage.DisplayAlert("Attenzione", response.ToString(), "ok");
                    }
                    else
                    model.metodoPush(response, invioImpegnativa.assistito);
                }
                catch (Exception e)
                {
                   await App.Current.MainPage.DisplayAlert("Attenzione","connessione non riuscita o codici impegnativa errati" , "riprova");
                }
                
            }
        }

        /* Ordina la lista della combo box
        public Func<string, ICollection<string>, ICollection<string>> SortingAlgorithm { get; } = (text, values) => values
        .Where(x => x.ToLower().StartsWith(text.ToLower()))
        .OrderBy(x => x)
        .ToList(); */

        public void autoCompila(Assistito elementSelected)
        {
            nomeUtente = elementSelected.nome;
            cognomeUtente = elementSelected.cognome;
            codicefiscaleUtente = elementSelected.codice_fiscale;
            invioImpegnativa.assistito = elementSelected;
            Visible = "true";
        }

        public string Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                OnPropertyChanged();
            }
        }
    }
}
