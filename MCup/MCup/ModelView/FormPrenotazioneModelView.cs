using MCup.Model;
using MCup.Service;
using MCup.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls.Maps;
using Xamarin.Forms;

namespace MCup.ModelView
{
    //ModelView della pagina FormPrenotazione, tale classe è utilizzata per implementare il binding con la relativa pagina
    public class FormPrenotazioneModelView : INotifyPropertyChanged
    {
        //Evento che prevede il cambiamento di proprietà all'interno della classe
        public event PropertyChangedEventHandler PropertyChanged;

        private string visible="false";

        private Regex regexNomeCognome = new Regex(@"^[A-Za-zèùàòé][a-zA-Z'èùàòé ]*$");

        //Oggetto che astrae l'utente che intende prenotare una o delle prestazioni
        private UtenzaPrenotazione utenza;
        
        //Oggetto che astrae la ricetta NRE
        private InvioRicettaPrenotazione ricetta;


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

        private ObservableCollection<string> contatti = new ObservableCollection<string>();

        private Contacts contacts;

        public ObservableCollection<string> Contatti
        {
            get { return contatti; }
            set
            {
                OnPropertyChanged();
                contatti = new ObservableCollection<string>(value);
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
            get { return utenza.getCodiceFiscale(); }
            set
            {
                utenza.setCodiceFiscale(value);
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

        //Costruttore del ModelView, viene passato come parametro il riferimento alla pagina che lo richiama per poter effettuare una Navigation.pushAsync
        public FormPrenotazioneModelView(FormPrenotazione Model)
        {
            utenza = new UtenzaPrenotazione();
            ricetta = new InvioRicettaPrenotazione();
            model = Model;
            ricetta.codice_uno = "";
            ricetta.codice_due = "";
            utenza.nome = "";
            utenza.cognome = "";
            utenza.setCodiceFiscale("");
            InviaRichiesta = new Command(async () =>
            {
                await InvioDatiAsync();
            });
            leggiContatti();
        }

        private async void leggiContatti()
        {
            REST<object, Contacts> rest = new REST<object, Contacts>();
            contacts = await rest.GetSingleJson(URL.InfoPersonali, App.Current.Properties["tokenLogin"].ToString());
            ObservableCollection<string> temp = new ObservableCollection<string>();
            temp.Add(contacts.nome + ", " + contacts.cognome + ", " + contacts.codice_fiscale);
            for (int i = 0; i < contacts.contatti.Count; i++)
            {
                temp.Add(contacts.contatti[i].nome + ", " + contacts.contatti[i].cognome + ", " + contacts.contatti[i].codice_fiscale);
            }
            Contatti = temp;
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
            if (string.IsNullOrEmpty(utenza.getCodiceFiscale()))
            {
                NameTextErrorCodFisc = "Il campo codice fiscale è obbligatorio";
                passControl = false;
            }
            else if (utenza.getCodiceFiscale().Length != 16)
            {
                NameTextErrorCodFisc = "Il campo codice fiscale non è corretto";
                passControl = false;
            }
            else
                NameTextErrorCodFisc = "";
            if (string.IsNullOrEmpty(ricetta.codice_uno) || ricetta.codice_uno.Length != 5)
            {
                NameTextErrorCodUno = "Il campo è obbligatorio";
                passControl = false;
            }
            else
                NameTextErrorCodUno = "";
            if (string.IsNullOrEmpty(ricetta.codice_due) || ricetta.codice_due.Length != 10)
            {
                NameTextErrorCodDue = "Il campo è obbligatorio";
                passControl = false;
            }
            else
                NameTextErrorCodDue = "";
            if (passControl)
            {
                REST<sendRicetta, Ricetta> connessione = new REST<sendRicetta,Ricetta>();
                sendRicetta nre = new sendRicetta(ricetta.codice_uno.ToString(),ricetta.codice_due.ToString());
                Ricetta response = await connessione.PostJson(URL.Ricetta,nre);
                model.metodoPush(response);
            }
        }

        //Ordina la lista della combo box
        public Func<string, ICollection<string>, ICollection<string>> SortingAlgorithm { get; } = (text, values) => values
        .Where(x => x.ToLower().StartsWith(text.ToLower()))
        .OrderBy(x => x)
        .ToList();

        public void autoCompila(string elementSelected)
        {
            int indexEndName = elementSelected.IndexOf(',');
            int indexEndSurname = elementSelected.IndexOf(',', indexEndName + 1);
            string name = elementSelected.Substring(0, indexEndName);
            string surname = elementSelected.Substring(indexEndName + 2, elementSelected.Length - (indexEndName + 2) - 18);
            string codFisc = elementSelected.Substring(indexEndSurname + 2);
            int inContacts = contacts.searchContact(name, surname, codFisc);
            if(inContacts == -1) //L'utente ha cliccato su se stesso nella lista della checkbox
            {
                nomeUtente = contacts.nome;
                cognomeUtente = contacts.cognome;
                codicefiscaleUtente = contacts.codice_fiscale;
                Visible = "true";
            }
            else if(inContacts >= 0)
            {
                nomeUtente = contacts.contatti[inContacts].nome;
                cognomeUtente = contacts.contatti[inContacts].cognome;
                codicefiscaleUtente = contacts.contatti[inContacts].codice_fiscale;
                Visible = "true";
            }
        }

        //Classe che identifica le prestazioni e se sono state erogate. Questa classe astrae dei possibili dati ricevuti da SOGEI
        private class Prestazioni
        {
            public string prestazione { get; set; }

            public bool erogato { get; set; }

        }

        //Classe utilizzata per astrarre il json da inviare al servizio per ottenere le informazioni della ricetta
        private class sendRicetta
        {
            public string codice_nre;

            public sendRicetta(string codice_uno, string codice_due)
            {
                this.codice_nre = codice_uno + codice_due;
            }
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
