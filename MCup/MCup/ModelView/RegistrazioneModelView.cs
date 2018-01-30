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
        private List<Comune> listacomuni = new List<Comune>();
        private List<string> listaprovince = new List<string>();
        private List<Comune> listacomuniresidenza = new List<Comune>();
        private List<StatoCivile> listaStatoCivile = new List<StatoCivile>();

        private Utente utente; //Oggetto che astrae l'utenza del cliente


        #region Boolean_di_Controllo

        ///<summary>Boolean per controllo errore</summary> 
        //variabili private
        private Boolean nameErrorNome = false,
            nameErrorCognome = false,
            nameErrorDataNascita = false,
            nameErrorProvinciaNascita = false,
            nameErrorComuneNascita = false,
            nameErrorProvinciaResidenza = false,
            nameErrorComuneResidenza = false,
            nameErrorStatoCivile = false,
            nameErrorTelefono = false,
            nameErrorSesso = false,
            nameErrorCodFiscale = false,
            nameErrorUsername=false;

        public Boolean NameErrorTextUsername
        {
            get { return nameErrorUsername; }
            set
            {
                OnPropertyChanged();
                nameErrorUsername = value;
            }
        }

        //variabili publiche per Binding
        public Boolean NameErrorNome //proprietà per il NameErrorNome
        {
            get { return nameErrorNome; }
            set
            {
                OnPropertyChanged();
                nameErrorNome = value;
            }
        }
        public Boolean NameErrorCognome //proprietà per il NameErrorCognome
        {
            get { return nameErrorCognome; }
            set
            {
                OnPropertyChanged();
                nameErrorCognome = value;
            }
        }
        public Boolean NameErrorDataNascita //proprietà per il NameErrorDataNascita
        {
            get { return nameErrorDataNascita; }
            set
            {
                OnPropertyChanged();
                nameErrorDataNascita = value;
            }
        }
        public Boolean NameErrorProvinciaNascita //proprietà per il NameErrorProvinciaNascita
        {
            get { return nameErrorProvinciaNascita; }
            set
            {
                OnPropertyChanged();
                nameErrorProvinciaNascita = value;
            }
        }
        public Boolean NameErrorComuneNascita //proprietà per il NameErrorComuneNascita
        {
            get { return nameErrorComuneNascita; }
            set
            {
                OnPropertyChanged();
                nameErrorComuneNascita = value;
            }
        }
        public Boolean NameErrorProvinciaResidenza //proprietà per il NameErrorProvinciaResidenza
        {
            get { return nameErrorProvinciaResidenza; }
            set
            {
                OnPropertyChanged();
                nameErrorProvinciaResidenza = value;
            }
        }
        public Boolean NameErrorComuneResidenza //proprietà per il NameErrorComuneResidenza
        {
            get { return nameErrorComuneResidenza; }
            set
            {
                OnPropertyChanged();
                nameErrorComuneResidenza = value;
            }
        }
        public Boolean NameErrorTelefono //proprietà per il NameErrorTelefono
        {
            get { return nameErrorTelefono; }
            set
            {
                OnPropertyChanged();
                nameErrorTelefono = value;
            }
        }
        public Boolean NameErrorSesso //proprietà per il NameErrorSesso
        {
            get { return nameErrorSesso; }
            set
            {
                OnPropertyChanged();
                nameErrorSesso = value;
            }
        }
        public Boolean NameErrorStatoCivile //proprietà per il NameErrorStatoCivile
        {
            get { return nameErrorStatoCivile; }
            set
            {
                OnPropertyChanged();
                nameErrorStatoCivile = value;
            }
        }
        public Boolean NameErrorCodFiscale //proprietà per il NameErrorCodFiscale
        {
            get { return nameErrorCodFiscale; }
            set
            {
                OnPropertyChanged();
                nameErrorCodFiscale = value;
            }
        }
        #endregion



        public List<StatoCivile> ListaStatoCivile
        {
            get { return listaStatoCivile; }
            set
            {
                OnPropertyChanged();
                listaStatoCivile = value;
            }
        }

        public async void LeggiStatoCivile()
        {
            REST<object, StatoCivile> connessioneStatoCivile = new REST<object, StatoCivile>();
            ListaStatoCivile = await connessioneStatoCivile.GetJson(URL.ListaStatoCivile);
        }

        public void StatoCivileScelto(StatoCivile stato)
        {
            utente.codStatoCivile = stato.id;
            utente.statocivile = stato.descrizione;
        }

        private string confermaPassword,
            nameErrorTextUsername,
            nameErrorTextNome,
            nameErrorTextCognome,
            nameErrorTextCodice,
            nameErrorTextPassword,
            nameErrorTextConfermaPassword;

        public string nameerrortextdatanascita, 
            nameErrorTextLuogoNascita, 
            nameErrorTextProvincia, 
            nameErrorTextComuneResidenza, 
            nameErrorTextNumeroTelefono;

        public ICommand registrati { protected set; get; } //Command per il tentativo di registrazione dell'utenza
        public string Username //Proprietà relativa al campo Username
        {
            get { return utente.username; }
            set
            {
                OnPropertyChanged();
                utente.username = value;
            }
        }

        public List<string> listaProvince
        {
            get { return listaprovince; }
            set
            {
                OnPropertyChanged();
                listaprovince = value;
            }
        }

        public List<Comune> listaComuniResidenza
        {
            get { return listacomuniresidenza; }
            set
            {
                OnPropertyChanged();
                listacomuniresidenza = value;
            }
        }

        public List<Comune> listaComuni
        {
            get { return listacomuni; }
            set
            {
                OnPropertyChanged();
                listacomuni = value;
            }
        }
        public string codiceFiscale //Proprietà relativa al campo codice fiscale
        {
            get { return utente.codice_fiscale; }
            set
            {
                OnPropertyChanged();
                utente.codice_fiscale = value.ToUpper();
            }
        }

        public string comune_residenza
        {
            get { return utente.comune_residenza; }
            set
            {
                OnPropertyChanged();
                utente.comune_residenza = value.ToUpper();
            }
        }

        public string telefono
        {
            get { return utente.telefono; }
            set
            {
                OnPropertyChanged();
                utente.telefono = value;
            }
        }

        public string NameErrorTextNumeroTelefono
        {
            get { return nameErrorTextNumeroTelefono; }
            set
            {
                OnPropertyChanged();
                nameErrorTextNumeroTelefono = value;
            }
        }

        public string NameErrorTextComuneResidenza
        {
            get { return nameErrorTextComuneResidenza; }
            set
            {
                OnPropertyChanged();
                nameErrorTextComuneResidenza = value;
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
                utente.nome = value.ToUpper();
            }
        }

        public string cognome //Proprietà relativa al campo cognome
        {
            get { return utente.cognome; }
            set
            {
                OnPropertyChanged();
                utente.cognome = value.ToUpper();
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
                
                utente.data_nascita= value;
            }
        }

        public string luogo_nascita //Proprietà relativa al campo luogo di nascita
        {
            get { return utente.luogo_nascita; }
            set
            {
                OnPropertyChanged();
                utente.luogo_nascita = value.ToUpper();
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
     

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private async void LeggiComuni(string provincia)
        {
            Provincia provinciaSelezionata = new Provincia();
            provinciaSelezionata.provincia = provincia;
            REST<Provincia, Comune> connessioneComuni = new REST<Provincia, Comune>();
            listaComuni = await connessioneComuni.PostJsonList(URL.ListaComuni, provinciaSelezionata);
        }
        private async void LeggiComuniResidenza(string provincia)
        {
            Provincia provinciaSelezionata = new Provincia();
            provinciaSelezionata.provincia = provincia;
            REST<Provincia, Comune> connessioneComuni = new REST<Provincia, Comune>();
            listaComuniResidenza = await connessioneComuni.PostJsonList(URL.ListaComuni, provinciaSelezionata);
        }

        private async void LeggiProvince()
        {
            REST<object, string> connessioneProvince = new REST<object, string>();
            listaProvince = await connessioneProvince.GetJson(URL.ListaProvince);

        }

        public void provinciaDiNascitaSelezionato(string provincia)
        {
            LeggiComuni(provincia);
        }

        public void provinciaDiResidenzaSelezionato(string provincia)
        {
            LeggiComuniResidenza(provincia);
        }

        public void comuneNascitaSelezionato( Comune comune)
        {
            utente.luogo_nascita = comune.nome;
            utente.istatComuneNascita = comune.codice;
        }

        public void comuneResidenzaSelezionato(Comune comune)
        {
            utente.comune_residenza = comune.nome;
            utente.istatComuneResidenza = comune.codice;
        }
        //Costruttore che inizializza un utenza vuota e definisce il metodo a cui il Command registrati fa riferimento
        public RegistrazioneModelView()
        {

            utente = new Utente(); //Crea un utenza vuota
            LeggiProvince();
            LeggiStatoCivile();
            registrati = new Command(async () =>
            {

                //Imposta gli errori ad una stringa vuota
            
                NameErrorTextPassword = String.Empty;
                NameErrorTextConfermaPassword = String.Empty;
               
                /*
                 * variabile locale utilizzata per verificare se l'utente ha inserito i campi obbligatori per effettuare il tentativo di registrazione.
                 * Questa viene inizializzata a true supponendo a priori che l'utente abbia inserito tutti i campi correttamente.
                 * Ogni qualvota che uno dei seguenti controlli non andasse a buon fine viene assegnata a questa variabile il valore false
                 */

                #region controlloErrori
                //Imposta gli errori ad una stringa vuota
                NameErrorNome =
                NameErrorCognome =
                NameErrorDataNascita =
                NameErrorProvinciaNascita =
                NameErrorComuneNascita =
                NameErrorProvinciaResidenza =
                NameErrorComuneResidenza =
                NameErrorStatoCivile =
                NameErrorTelefono =
                NameErrorSesso =
                NameErrorCodFiscale = false;

                /*
                * variabile locale utilizzata per verificare se l'utente ha inserito i campi obbligatori per effettuare il tentativo di registrazione.
                * Questa viene inizializzata a true supponendo a priori che l'utente abbia inserito tutti i campi correttamente.
                * Ogni qualvota che uno dei seguenti controlli non andasse a buon fine viene assegnata a questa variabile il valore false
                */
                ///<summary>
                ///Controllo se i capi sono nulli, se uno è nulla la registrazione non verrà effettuata e il campo o i campi vuoi verragno segnalati
                ///grazie alla label di errore.
                /// </summary>
                bool controllPass = true;

                if (string.IsNullOrEmpty(Username))
                {
                    NameErrorTextUsername = true;
                    controllPass = true;
                }
                else
                {
                    NameErrorTextUsername = false;
                }

                if (string.IsNullOrEmpty(nome))
                {
                    NameErrorNome = true;
                    controllPass = false;
                }
                else
                {
                    NameErrorNome = false;
                }
                if (string.IsNullOrEmpty(password))
                {
                    NameErrorTextPassword = "attenzione, campo obbligatorio";
                    controllPass = false;
                }
                else
                {
                    controllPass = true;
                }
                if (string.IsNullOrEmpty(ConfermaPassword))
                {
                    NameErrorTextConfermaPassword = "attenzione, campo obbligatorio";
                    controllPass = false;
                }
                else
                {
                    controllPass = true;
                }
                if (string.IsNullOrEmpty(cognome))
                {
                    NameErrorCognome = true;
                    controllPass = false;
                }
                else
                {
                    NameErrorCognome = false;
                }
                if (string.IsNullOrEmpty(data_nascita))
                {
                    NameErrorDataNascita = true;
                    controllPass = false;
                }
                else
                {
                    NameErrorDataNascita = false;
                }
                //non so dove trovare la provincia
                /* if (string.IsNullOrEmpty())
                 {
                     NameErrorProvinciaNascita = true;
                     controllPass = false;
                 }
                 else
                 {
                     NameErrorProvinciaNascita = false;
                 }*/
                if (string.IsNullOrEmpty(luogo_nascita))
                {
                    NameErrorComuneNascita = true;
                    controllPass = false;
                }
                else
                {
                    NameErrorComuneNascita = false;
                }
                /*
                if (string.IsNullOrEmpty(provinciaSelezionata.provincia))
                {
                    NameErrorProvinciaResidenza = true;
                    controllPass = false;
                }
                else
                {
                    NameErrorProvinciaResidenza = false;
                }*/
                if (string.IsNullOrEmpty(comune_residenza))
                {
                    NameErrorComuneResidenza = true;
                    controllPass = false;
                }
                else
                {
                    NameErrorComuneResidenza = false;
                }
                if (string.IsNullOrEmpty(utente.statocivile))
                {
                    NameErrorStatoCivile = true;
                    controllPass = false;
                }
                else
                {
                    NameErrorStatoCivile = false;
                }
                if (string.IsNullOrEmpty(telefono))
                {
                    NameErrorTelefono = true;
                    controllPass = false;
                }
                else
                {
                    NameErrorTelefono = false;
                }
                if (sceltaSesso.Equals(' '))
                {
                    NameErrorSesso = true;
                    controllPass = false;
                }
                else
                {
                    NameErrorSesso = false;
                }
                if (string.IsNullOrEmpty(codiceFiscale))
                {
                    NameErrorCodFiscale = true;
                    controllPass = false;
                }
                else
                {
                    NameErrorCodFiscale = false;
                }
                if (password != ConfermaPassword)
                {
                    NameErrorTextPassword = "attenzione password non corrispondenti";
                    controllPass = false;
                }
                #endregion


              
                if (controllPass) //Controlla se l'utente ha riempito tutti i campi obbligatori
                {
                    utente.data_nascita = utente.data_nascita.Substring(0, 10);
                    string giorno, mese, anno;
                    giorno = utente.data_nascita.Substring(3, 2);
                    mese = utente.data_nascita.Substring(0, 2);
                    anno= utente.data_nascita.Substring(6);
                    utente.data_nascita = giorno + "/" + mese + "/" + anno;
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
