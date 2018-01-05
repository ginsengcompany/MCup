using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCup.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using MCup.Service;
using MCup.Views;

/*
 * Questa classe gestisce le informazioni, tramite il binding con le pagine Registrazione e Registrazione IOS, relative alla fase di registrazione dell'utenza. 
 */

namespace MCup.ModelView
{
    public class RegistrazioneModelView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged; //evento che implementa l'interfaccia INotifyPropertyChanged

        private Utente utente; //Oggetto che astrae l'utenza del cliente

        private string confermaPassword, nameErrorTextNome, nameErrorTextCognome, nameErrorTextCodice, nameErrorTextPassword, nameErrorTextConfermaPassword;

        public string nameerrortextdatanascita, nameErrorTextLuogoNascita, nameErrorTextProvincia;

        public ICommand registrati { protected set; get; } //Command per il tentativo di registrazione dell'utenza

        public string codiceFiscale //Proprietà relativa al campo codice fiscale
        {
            get { return utente.codice_fiscale; }
            set
            {
                OnPropertyChanged();
                utente.codice_fiscale = value;
            }
        }

        public string password //Proprietà relativa al campo password
        {
            get { return utente.password; }
            set
            {
                OnPropertyChanged();
                utente.password = value;
            }
        }

        public string nome //Proprietà relativa al campo nome
        {
            get { return utente.nome; }
            set
            {
                OnPropertyChanged();
                utente.nome = value;
            }
        }

        public string cognome //Proprietà relativa al campo cognome
        {
            get { return utente.cognome; }
            set
            {
                OnPropertyChanged();
                utente.cognome = value;
            }
        }

        public Char sceltaSesso //Proprietà relativa al campo sesso
        {
            get { return utente.sesso; }
            set
            {
                OnPropertyChanged();
                utente.sesso = value;
            }
        }

        public string data_nascita //Proprietà relativa al campo data di nascita
        {
            get { return utente.data_nascita; }
            set
            {
                OnPropertyChanged();
                utente.data_nascita = value;
            }
        }

        public string luogo_nascita //Proprietà relativa al campo luogo di nascita
        {
            get { return utente.luogo_nascita; }
            set
            {
                OnPropertyChanged();
                utente.luogo_nascita = value;
            }
        }

        public string provincia
        {
            get { return utente.provincia; }
            set
            {
                OnPropertyChanged();
                utente.provincia = value;
            }
        }

        public string ConfermaPassword //Proprietà relativa al campo password
        {
            get { return confermaPassword; }
            set
            {
                OnPropertyChanged();
                confermaPassword=value;
            }
        }

        public string NameErrorTextNome
        {
            get { return nameErrorTextNome; }
            set
            {
                OnPropertyChanged();
                nameErrorTextNome = value;
            }
        }
        public string NameErrorTextCognome
        {
            get { return nameErrorTextCognome; }
            set
            {
                OnPropertyChanged();
                nameErrorTextCognome = value;
            }
        }
        public string NameErrorTextCodice
        {
            get { return nameErrorTextCodice; }
            set
            {
                OnPropertyChanged();
                nameErrorTextCodice = value;
            }
        }
        public string NameErrorTextPassword
        {
            get { return nameErrorTextPassword; }
            set
            {
                OnPropertyChanged();
                nameErrorTextPassword = value;
            }
        }
        public string NameErrorTextConfermaPassword
        {
            get { return nameErrorTextConfermaPassword; }
            set
            {
                OnPropertyChanged();
                nameErrorTextConfermaPassword = value;
            }
        }
        public string NameErrorTextDataNascita
        {
            get { return nameerrortextdatanascita; }
            set
            {
                OnPropertyChanged();
                nameerrortextdatanascita = value;
            }
        }
        public string NameErrorTextLuogoNascita
        {
            get { return nameErrorTextLuogoNascita; }
            set
            {
                OnPropertyChanged();
                nameErrorTextLuogoNascita = value;
            }
        }
        public string NameErrorTextProvincia
        {
            get { return nameErrorTextProvincia; }
            set
            {
                OnPropertyChanged();
                nameErrorTextProvincia = value;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        //Costruttore che inizializza un utenza vuota e definisce il metodo a cui il Command registrati fa riferimento
        public RegistrazioneModelView()
        {
            utente = new Utente(); //Crea un utenza vuota

            registrati = new Command(async () =>
            {
                //Imposta gli errori ad una stringa vuota
                NameErrorTextNome=String.Empty;
                NameErrorTextCognome = String.Empty;
                NameErrorTextCodice = String.Empty;
                NameErrorTextPassword = String.Empty;
                NameErrorTextConfermaPassword = String.Empty;
                /*
                 * variabile locale utilizzata per verificare se l'utente ha inserito i campi obbligatori per effettuare il tentativo di registrazione.
                 * Questa viene inizializzata a true supponendo a priori che l'utente abbia inserito tutti i campi correttamente.
                 * Ogni qualvota che uno dei seguenti controlli non andasse a buon fine viene assegnata a questa variabile il valore false
                 */
                bool controllPass = true;
                if (string.IsNullOrEmpty(nome)) //Controlla se il campo nome è vuoto o null
                {
                    NameErrorTextNome = "Attenzione, campo obbligatorio";
                    controllPass = false;
                }
                if (string.IsNullOrEmpty(cognome)) //Controlla se il campo cognome è vuoto o null
                {
                    NameErrorTextCognome = "Attenzione, campo obbligatorio";
                    controllPass = false;
                }
                if (string.IsNullOrEmpty(codiceFiscale)) //Controlla se il campo codice fiscale è vuoto o null
                {
                    NameErrorTextCodice = "Attenzione, campo obbligatorio";
                    controllPass = false;
                }
                if (string.IsNullOrEmpty(password)) //Controlla se il campo password è vuoto o null
                {
                    NameErrorTextPassword = "Attenzione, campo obbligatorio";
                    controllPass = false;
                }
                if (string.IsNullOrEmpty(data_nascita)) //Controlla se il campo data di nascita è vuoto o null
                {
                    NameErrorTextDataNascita = "Attenzione, campo obbligatorio";
                    controllPass = false;
                }
                if (string.IsNullOrEmpty(luogo_nascita)) //Controlla se il campo luogo di nascita è vuoto o null
                {
                    NameErrorTextLuogoNascita = "Attenzione, campo obbligatorio";
                    controllPass = false;
                }
                if (utente.sesso.Equals(' ')) //Controlla se il campo sesso è vuoto
                {
                    controllPass = false;
                }
                if (string.IsNullOrEmpty(provincia) || provincia.Length != 2) //Controlla se il campo provincia è vuoto, null o length è diverso da due
                {
                    NameErrorTextProvincia = "Attenzione, campo obbligatorio";
                    controllPass = false;
                }
                if (string.IsNullOrEmpty(ConfermaPassword)) //Controlla se il campo conferma password è vuoto o null
                {
                    NameErrorTextConfermaPassword = "Attenzione, campo obbligatorio";
                    controllPass = false;
                }
                else if (password != ConfermaPassword) //Controlla se la password inserita dall'utente è uguale alla stringa inserita dall'utente nel campo conferma password
                {
                    NameErrorTextConfermaPassword = "Attenzione, la password non corrisponde";
                    controllPass = false;
                }
                if (controllPass) //Controlla se l'utente ha riempito tutti i campi obbligatori
                {
                    REST<object, string> restTermini = new REST<object, string>(); //Crea un oggetto REST per i termini di servizio remoti
                    var termini = await restTermini.getString(URL.TerminiServizio); //Recupera i termini di servizio attraverso una GET
                    //Mostra il display alert contenente i termini di servizio recuperati dalla rest restTermini e salva la risposta dell'utente nella variabile accetaODeclina
                    var accetaODeclina = await App.Current.MainPage.DisplayAlert("Termini di servizio", termini, "ACCETTA", "DECLINA");
                    if (accetaODeclina) //Controlla che l'utente abbia accettato i termini di servizio
                    {
                        REST<Utente, ResponseRegistrazione> rest = new REST<Utente, ResponseRegistrazione>(); //Crea un oggetto rest per effettuare la registrazione da remoto
                        ResponseRegistrazione response = await rest.PostJson(URL.Registrazione, utente); //Effettua una POST che restituisce nella variabile response se la registrazione ha avuto successo
                        if (response == default(ResponseRegistrazione)) //Se la variabile response contiene il valore di default della classe Response Registrazione allora la registrazione non è avvenuta
                            await App.Current.MainPage.DisplayAlert("Registrazione", rest.warning, "OK"); //Visualizza un display alert che indica all'utente che la registrazione non è stata effettuata
                        else if (response.auth) //Controlla se response contiene un oggetto e che indica che la registrazione è avvenuta con successo
                        {
                            //Visualizza un display alert che indica all'utente che la registrazione è avvenuta con successo
                            await App.Current.MainPage.DisplayAlert("Registrazione", "Registrazione effettuata con successo", "OK");
                            await App.Current.MainPage.Navigation.PopAsync(); //Ritorna alla pagina di login
                        }
                        else //Errore imprevisto durante la registrazione
                            await App.Current.MainPage.DisplayAlert("Registrazione", "Registrazione fallita", "OK");
                    }
                    else //L'utente non ha accettato i termini di servizio
                        await App.Current.MainPage.DisplayAlert("Registrazione", "Devi accettare i termini di servizio per poter proseguire", "OK");
                }
            });
        }

      
    }
}
