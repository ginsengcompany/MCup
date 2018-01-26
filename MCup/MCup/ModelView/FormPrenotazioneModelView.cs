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

        private sendRicetta invioContatto;
        private string visible="false";

        private Regex regexNomeCognome = new Regex(@"^[A-Za-zèùàòé][a-zA-Z'èùàòé ]*$");

        //Oggetto che astrae l'utente che intende prenotare una o delle prestazioni
        private UtenzaPrenotazione utenza;
        
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

        private List<Contatto> contatti = new List<Contatto>();

        private Contacts contacts;

        public List<Contatto> Contatti
        {
            get { return contatti; }
            set
            {
                OnPropertyChanged();
                contatti = new List<Contatto>(value);
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
        public FormPrenotazioneModelView(FormPrenotazione Model)
        {
            IsEnabled = true;
            utenza = new UtenzaPrenotazione();
            ricetta = new InvioRicettaPrenotazione();
            invioContatto= new sendRicetta();
            model = Model;
            ricetta.codice_uno = "";
            ricetta.codice_due = "";
            utenza.nome = "";
            utenza.cognome = "";
            utenza.setCodiceFiscale("");
            InviaRichiesta = new Command(async () =>
            {
                IsEnabled = false;
                await InvioDatiAsync();
                IsEnabled = true;
            });
            leggiContatti();
        }

        private async void leggiContatti()
        {
            REST<object, Contacts> rest = new REST<object, Contacts>();
            contacts = await rest.GetSingleJson(URL.InfoPersonali, App.Current.Properties["tokenLogin"].ToString());
            List<Contatto> temp = new List<Contatto>();
            temp.Add(new Contatto {
                nome = contacts.nome, cognome = contacts.cognome, codice_fiscale = contacts.codice_fiscale, data_nascita = contacts.data_nascita, statocivile = contacts.statocivile, codStatoCivile = contacts.codStatoCivile,
                luogo_nascita = contacts.luogo_nascita, sesso = contacts.sesso, AccountPrimario = true, istatComuneNascita = contacts.istatComuneNascita, istatComuneResidenza = contacts.istatComuneResidenza,
                nomeCompletoConCodiceFiscale = contacts.nome + " " + contacts.cognome + " " + contacts.codice_fiscale, comune_residenza = contacts.comune_residenza, telefono = contacts.telefono
            });
            for (int i = 0; i < contacts.contatti.Count; i++)
            {
                contacts.contatti[i].nomeCompletoConCodiceFiscale = contacts.contatti[i].nome + " " + contacts.contatti[i].cognome + " " + contacts.contatti[i].codice_fiscale;
                temp.Add(contacts.contatti[i]);
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
                try
                {
                    REST<sendRicetta, Ricetta> connessione = new REST<sendRicetta, Ricetta>();
                    invioContatto.codice_nre = codiceUno+codiceDue;
                    Ricetta response = await connessione.PostJson(URL.Ricetta, invioContatto);
                    if ((response == null) || (response == default(Ricetta)))
                    {
                       await App.Current.MainPage.DisplayAlert("Attenzione", response.ToString(), "ok");
                    }
                    else
                    model.metodoPush(response, invioContatto);
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

        public void autoCompila(Contatto elementSelected)
        {
            nomeUtente = elementSelected.nome;
            cognomeUtente = elementSelected.cognome;
            codicefiscaleUtente = elementSelected.codice_fiscale;
            invioContatto.contattoDaInviare = elementSelected;
            Visible = "true";
        }

        //Classe che identifica le prestazioni e se sono state erogate. Questa classe astrae dei possibili dati ricevuti da SOGEI
        private class Prestazioni
        {
            public string prestazione { get; set; }

            public bool erogato { get; set; }

        }

        //Classe utilizzata per astrarre il json da inviare al servizio per ottenere le informazioni della ricetta
        public class sendRicetta
        {
            public string codice_nre;

            public Contatto contattoDaInviare;

            public  sendRicetta()
            { }
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
