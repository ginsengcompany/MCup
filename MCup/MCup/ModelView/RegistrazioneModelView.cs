#region Librerie

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MCup.Model;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;
using MCup.Service;

#endregion

/*
 * Questa classe gestisce le informazioni, tramite il binding con le pagine Registrazione e Registrazione IOS, relative alla fase di registrazione dell'utenza. 
 */

namespace MCup.ModelView
{
    public class RegistrazioneModelView : INotifyPropertyChanged
    {


        #region DichiarazioneVariabili

        public event PropertyChangedEventHandler PropertyChanged; //evento che implementa l'interfaccia INotifyPropertyChanged
        private List<Comune> listacomuni = new List<Comune>();
        private List<Provincia> listaprovince = new List<Provincia>();
        private List<Comune> listacomuniresidenza = new List<Comune>();
        private List<StatoCivile> listaStatoCivile = new List<StatoCivile>();
        private Regex regexPass = new Regex(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z])");
        private Utente utente; //Oggetto che astrae l'utenza del cliente
        private string confermaPassword,
            nameErrorTextUsername,
            nameErrorTextNome,
            nameErrorTextCognome,
            nameErrorTextCodice,
            nameErrorTextPassword,
            nameErrorTextConfermaPassword,
            nameerrortextdatanascita,
            nameErrorTextLuogoNascita,
            nameErrorTextProvincia,
            nameErrorTextComuneResidenza,
            nameErrorTextIndirizzo,
            nameErrorTextNumeroTelefono;

        #endregion

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
            nameErrorIndirizzo = false,
            nameErrorSesso = false,
            nameErrorCodFiscale = false,
            nameErrorUsername=false;

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


        public Boolean NameErrorUsername //proprietà per il NameErrorUsername
        {
            get { return nameErrorUsername; }
            set
            {
                OnPropertyChanged();
                nameErrorUsername = value;
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
        public Boolean NameErrorIndirizzo //proprietà per il NameErrorIndirizzo
        {
            get { return nameErrorIndirizzo; }
            set
            {
                OnPropertyChanged();
                nameErrorIndirizzo = value;
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

        #region Proprietà

        public List<StatoCivile> ListaStatoCivile
        {
            get { return listaStatoCivile; }
            set
            {
                OnPropertyChanged();
                listaStatoCivile = value;
            }
        }
        public ICommand registrati { protected set; get; } //Command per il tentativo di registrazione dell'utenza
        public ICommand Avanti { protected set; get; }
        public string Username //Proprietà relativa al campo Username
        {
            get { return utente.username; }
            set
            {
                OnPropertyChanged();
                utente.username = value;
            }
        }

        public List<Provincia> listaProvince
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
                utente.codice_fiscale = value;
            }
        }

        public string comune_residenza
        {
            get { return utente.comune_residenza; }
            set
            {
                OnPropertyChanged();
                utente.comune_residenza = value;
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
        public string NameErrorTextIndirizzo
        {
            get { return nameErrorTextIndirizzo; }
            set
            {
                OnPropertyChanged();
                nameErrorTextIndirizzo = value;
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
                utente.nome = value;
            }
        }

        public string Indirizzo //Proprietà relativa al campo nome
        {
            get { return utente.indirizzores; }
            set
            {
                OnPropertyChanged();
                utente.indirizzores = value;
            }
        }
        public string Email //Proprietà relativa al campo nome
        {
            get { return utente.email; }
            set
            {
                OnPropertyChanged();
                utente.email = value;
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

        public string ConfermaPassword //Proprietà relativa al campo password
        {
            get { return confermaPassword; }
            set
            {
                OnPropertyChanged();
                confermaPassword = value;
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


        #endregion

        #region Costruttore

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
                NameErrorProvinciaResidenza =
                NameErrorComuneResidenza =
                NameErrorTelefono =
                NameErrorSesso =
                NameErrorIndirizzo = false;

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
                if (string.IsNullOrEmpty(Indirizzo))
                {
                    NameErrorIndirizzo = true;
                    controllPass = false;
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
         
                if (string.IsNullOrEmpty(telefono))
                {
                    NameErrorTelefono = true;
                    controllPass = false;
                }
                if (sceltaSesso.Equals(' '))
                {
                    NameErrorSesso = true;
                    controllPass = false;
                }
                #endregion
                if (controllPass) //Controlla se l'utente ha riempito tutti i campi obbligatori
                {
                    utente.data_nascita = utente.data_nascita.Substring(0, 10);
                    string giorno, mese, anno;
                    giorno = utente.data_nascita.Substring(3, 2);
                    mese = utente.data_nascita.Substring(0, 2);
                    anno = utente.data_nascita.Substring(6);
                    utente.data_nascita = giorno + "/" + mese + "/" + anno;
                    REST<object, string> restTermini = new REST<object, string>(); //Crea un oggetto REST per i termini di servizio remoti
                    var termini = await restTermini.getString(SingletonURL.Instance.getRotte().TerminiServizio); //Recupera i termini di servizio attraverso una GET
                    if (restTermini.responseMessage != HttpStatusCode.OK)
                    {
                        await App.Current.MainPage.DisplayAlert("Attenzione " + (int)restTermini.responseMessage, restTermini.warning, "OK");
                    }
                    else
                    {
                        //Mostra il display alert contenente i termini di servizio recuperati dalla rest restTermini e salva la risposta dell'utente nella variabile accetaODeclina
                        var accetaODeclina = await App.Current.MainPage.DisplayAlert("Termini di servizio", termini, "ACCETTA", "DECLINA");
                        if (accetaODeclina) //Controlla che l'utente abbia accettato i termini di servizio
                        {
                            REST<Utente, ResponseRegistrazione> rest = new REST<Utente, ResponseRegistrazione>(); //Crea un oggetto rest per effettuare la registrazione da remoto
                            utente.Maiuscolo();
                            ResponseRegistrazione response = await rest.PostJson(SingletonURL.Instance.getRotte().Registrazione, utente); //Effettua una POST che restituisce nella variabile response se la registrazione ha avuto successo

                            if (rest.responseMessage != HttpStatusCode.Created)
                            {
                                await App.Current.MainPage.DisplayAlert("Attenzione " + (int)rest.responseMessage, rest.warning, "OK");
                            }
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

                }
            });
        }

        #endregion

        #region Metodi

        public async void LeggiStatoCivile()
        {
            REST<object, StatoCivile> connessioneStatoCivile = new REST<object, StatoCivile>();
            ListaStatoCivile = await connessioneStatoCivile.GetListJson(SingletonURL.Instance.getRotte().ListaStatoCivile);
            if (connessioneStatoCivile.responseMessage != HttpStatusCode.OK)
            {
                await App.Current.MainPage.DisplayAlert("Attenzione " + (int)connessioneStatoCivile.responseMessage, connessioneStatoCivile.warning, "OK");
            }
        }

        public void StatoCivileScelto(StatoCivile stato)
        {
            utente.codStatoCivile = stato.id;
            utente.statocivile = stato.descrizione;
        }




        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        public async Task<bool> AvantiSecondaPagina()
        {
            bool control = true;
            NameErrorNome = false;
            NameErrorCognome = false;
            NameErrorDataNascita = false;
            NameErrorComuneNascita = false;
            NameErrorCodFiscale = false;
            if (string.IsNullOrEmpty(nome))
            {
                NameErrorNome = true;
                control = false;
            }
            if (string.IsNullOrEmpty(cognome))
            {
                NameErrorCognome = true;
                control = false;
            }
            if (string.IsNullOrEmpty(data_nascita))
            {
                NameErrorDataNascita = true;
                control = false;
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
                control = false;
            }
            if (string.IsNullOrEmpty(codiceFiscale))
            {
                NameErrorCodFiscale = true;
                control = false;
            }
            return control;
        }



        public async Task<bool> VaiAvanti()
        {
            NameErrorTextPassword = "";
            NameErrorUsername = false;
            NameErrorTextPassword = "";
            NameErrorTextConfermaPassword = "";
            Match matchPass;
            bool control = true;
            if (string.IsNullOrEmpty(Username))
            {
                NameErrorUsername = true;
                control = false;
            }
            if (string.IsNullOrEmpty(password))
            {
                NameErrorTextPassword = "attenzione, campo obbligatorio";
                control = false;
            }
            else if (password.Length < 8)
            {
                NameErrorTextPassword = "la password deve essere almeno di 8 caratteri";
                control = false;
            }
            else
            {
                matchPass = regexPass.Match(password);
                if (!matchPass.Success)
                {
                    NameErrorTextPassword = "la password deve contenere una lettera maiuscola, minuscola ed un numero";
                    control = false;
                }
            }
            if (string.IsNullOrEmpty(ConfermaPassword))
            {
                NameErrorTextConfermaPassword = "attenzione, campo obbligatorio";
                control = false;
            }
            if (password != ConfermaPassword)
            {
                NameErrorTextPassword = "attenzione password non corrispondenti";
                NameErrorTextConfermaPassword = "attenzione password non corrispondenti";
                control = false;
            }
            return control;
        }


        private async void LeggiComuni(Provincia provincia)
        {
            Provincia provinciaSelezionata = new Provincia();
            provinciaSelezionata = provincia;
            REST<Provincia, Comune> connessioneComuni = new REST<Provincia, Comune>();
            listacomuni = await connessioneComuni.PostJsonList(SingletonURL.Instance.getRotte().ListaComuni, provinciaSelezionata);
            if (connessioneComuni.responseMessage != HttpStatusCode.OK)
            {
                await App.Current.MainPage.DisplayAlert("Attenzione " + (int)connessioneComuni.responseMessage, connessioneComuni.warning, "OK");
            }
            else
            {
                listaComuni = listacomuni;
            }

        }
        private async void LeggiComuniResidenza(Provincia provincia)
        {
            Provincia provinciaSelezionata = new Provincia();
            provinciaSelezionata = provincia;
            REST<Provincia, Comune> connessioneComuni = new REST<Provincia, Comune>();
            listaComuniResidenza = await connessioneComuni.PostJsonList(SingletonURL.Instance.getRotte().ListaComuni, provinciaSelezionata);
            if (connessioneComuni.responseMessage != HttpStatusCode.OK)
            {
                await App.Current.MainPage.DisplayAlert("Attenzione " + (int)connessioneComuni.responseMessage, connessioneComuni.warning, "OK");
            }
        }

        private async void LeggiProvince()
        {
            REST<object, Provincia> connessioneProvince = new REST<object, Provincia>();
            listaProvince = await connessioneProvince.GetListJson(SingletonURL.Instance.getRotte().ListaProvince);
            if (connessioneProvince.responseMessage != HttpStatusCode.OK)
            {
                await App.Current.MainPage.DisplayAlert("Attenzione " + (int)connessioneProvince.responseMessage, connessioneProvince.warning, "OK");
            }
        }

        public void provinciaDiNascitaSelezionato(Provincia provincia)
        {
            LeggiComuni(provincia);
        }

        public void provinciaDiResidenzaSelezionato(Provincia provincia)
        {
            LeggiComuniResidenza(provincia);
        }

        public void comuneNascitaSelezionato(Comune comune)
        {
            utente.luogo_nascita = comune.nome;
            utente.istatComuneNascita = comune.codice;
        }

        public void comuneResidenzaSelezionato(Comune comune)
        {
            utente.comune_residenza = comune.nome;
            utente.istatComuneResidenza = comune.codice;
        }


        #endregion



    }
}
