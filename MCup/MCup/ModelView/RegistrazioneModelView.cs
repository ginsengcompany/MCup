#region Librerie

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MCup.Model;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;
using MCup.CustomPopUp;
using Xamarin.Forms;
using MCup.Service;
using Rg.Plugins.Popup.Extensions;

#endregion

/*
 * Questa classe gestisce le informazioni, tramite il binding con le pagine Registrazione e Registrazione IOS, relative alla fase di registrazione dell'utenza. 
 */

namespace MCup.ModelView
{
    public class RegistrazioneModelView : INotifyPropertyChanged
    {


        #region DichiarazioneVariabili

        private string provinciaNascita;

        private Boolean visibleCdf = false;
        public event PropertyChangedEventHandler PropertyChanged; //evento che implementa l'interfaccia INotifyPropertyChanged
        private List<Comune> listacomuni = new List<Comune>();
        private List<Provincia> listaprovince = new List<Provincia>();
        private bool nationalVisibilityForeign, nationalVisibility, nameErrorNazione = false;
        private List<Nazione> listanazioni = new List<Nazione>();
        Provincia provinciaSelezionata = new Provincia();
        private bool terminiAccettati = false;
        private List<Comune> listacomuniresidenza = new List<Comune>();
        private List<StatoCivile> listaStatoCivile = new List<StatoCivile>();
        private Regex regexPass = new Regex(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z])");
        private Regex regexMail = new Regex(@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                                            + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				                            [0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                                            + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				                            [0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                                            + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$");
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
            nameErrorTextNumeroTelefono,
            nameErrorTextEmail,
            nameErrorTextStatoCivile;
            

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
            nameErrorUsername=false,
            nameErrorEmail= false;

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

        public Boolean NameErrorEmail
        {
            get { return nameErrorEmail; }
            set
            {
                OnPropertyChanged();
                nameErrorEmail = value;
            }
        }

        public Boolean NameErrorStatoCivile
        {
            get { return nameErrorStatoCivile; }
            set
            {
                OnPropertyChanged();
                nameErrorStatoCivile = value;
            }
        }
        #endregion

        #region Proprietà
        public bool VisibleCdf
        {
            get { return visibleCdf; }
            set
            {
                OnPropertyChanged();
                visibleCdf = value;
            }
        }
        public List<Nazione> ListaNazioni
        {
            get { return listanazioni; }
            set
            {
                OnPropertyChanged();
                listanazioni = value;
            }
        }
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
        public ICommand terminiDiServizio { protected set; get; }
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

        public bool TerminiAccettati
        {
            get { return terminiAccettati; }
            set
            {
                OnPropertyChanged();
                terminiAccettati = value;
            }
        }

        public bool NationalVisibilityForeign
        {
            get { return nationalVisibilityForeign; }
            set
            {
                OnPropertyChanged();
                nationalVisibilityForeign = value;
            }
        }
        public bool NationalVisibility
        {
            get { return nationalVisibility; }
            set
            {
                OnPropertyChanged();
                nationalVisibility = value;
            }
        }
        public bool NameErrorNazione
        {
            get { return nameErrorNazione; }
            set
            {
                OnPropertyChanged();
                nameErrorNazione = value;
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

        public DateTime data_nascita //Proprietà relativa al campo data di nascita
        {
            get
            {
                if (string.IsNullOrWhiteSpace(utente.data_nascita))
                    return DateTime.Today;
                return DateTime.ParseExact(utente.data_nascita,"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            set
            {
                OnPropertyChanged();
                utente.data_nascita = value.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
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

        public string statoCivle
        {
            get { return utente.statocivile; }
            set
            {
                OnPropertyChanged();
                utente.statocivile = value;
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

        public string NameErrorTextEmail
        {
            get { return nameErrorTextEmail; }
            set
            {
                OnPropertyChanged();
                nameErrorTextEmail = value;
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

        public string Luogo_nascitaNuovoContatto //Proprietà relativa al campo luogo di nascita
        {
            get { return utente.luogo_nascita; }
            set
            {
                OnPropertyChanged();
                utente.luogo_nascita = value;
            }
        }
        public string Provincia_nascitaNuovoContatto //Proprietà relativa al campo luogo di nascita
        {
            get { return provinciaNascita; }
            set
            {
                OnPropertyChanged();
                provinciaNascita = value;
            }
        }


        #endregion

        #region Costruttore

        //Costruttore che inizializza un utenza vuota e definisce il metodo a cui il Command registrati fa riferimento
        public RegistrazioneModelView()
        {
            NationalVisibilityForeign = false;
            NationalVisibility = true;
            utente = new Utente(); //Crea un utenza vuota
            LeggiProvince();
            LeggiStatoCivile();
            RicezioneNazioni();
            registrati = new Command(async () =>
            {

                //Imposta gli errori ad una stringa vuota

                NameErrorTextPassword = String.Empty;
                NameErrorTextConfermaPassword = String.Empty;
                Match matchMail;

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
                                NameErrorIndirizzo =
                                    NameErrorStatoCivile = 
                                    NameErrorEmail = false;

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

                if (string.IsNullOrWhiteSpace(Email.Trim()))
                {
                    NameErrorTextEmail = "Attenzione, campo obbligatorio";
                    controllPass = false;
                    NameErrorEmail = true;
                }
                else 
                {
                    matchMail = regexMail.Match(Email);
                    if (!matchMail.Success)
                    {
                        NameErrorTextEmail = "La mail non è valida";
                        controllPass = false;
                        NameErrorEmail = true;
                    }
                }

                if (string.IsNullOrEmpty(statoCivle))
                {
                    NameErrorStatoCivile = true;
                    controllPass = false;
                }
                #endregion

                if (controllPass) //Controlla se l'utente ha riempito tutti i campi obbligatori
                {
                    //utente.data_nascita = utente.data_nascita.Substring(0, 10);
                   /* string giorno, mese, anno;
                    giorno = utente.data_nascita.Substring(3, 2);
                    mese = utente.data_nascita.Substring(0, 2);
                    anno = utente.data_nascita.Substring(6);
                    utente.data_nascita = giorno + "/" + mese + "/" + anno;*/
                    utente.provincia = provinciaSelezionata.provincia;
                    await App.Current.MainPage.Navigation.PushPopupAsync(new PopUpTerminiServizioRegistrazione(utente));
                }
                
            });
        }

        #endregion

        #region Metodi

        public void sceltaNazione(bool statoEstero)
        {
            if (statoEstero)
            {
                NationalVisibilityForeign = true;
                NationalVisibility = false;
                utente.luogo_nascita = String.Empty;
            }
            else
            {
                NationalVisibilityForeign = false;
                NationalVisibility = true;
                utente.luogo_nascita=String.Empty;
            }
        }

        public async void RicezioneNazioni()
        {
            REST<object, Nazione> connessioneNazioni = new REST<object, Nazione>();
            ListaNazioni = await connessioneNazioni.GetListJson(SingletonURL.Instance.getRotte().listaNazioni);
            if (connessioneNazioni.responseMessage != HttpStatusCode.OK)
                await MessaggioConnessione.displayAlert((int)connessioneNazioni.responseMessage, connessioneNazioni.warning);
            else
                ListaNazioni= listanazioni.OrderBy(o => o.descrizione).ToList();
        }

        public async void LeggiStatoCivile()
        {
            REST<object, StatoCivile> connessioneStatoCivile = new REST<object, StatoCivile>();
            ListaStatoCivile = await connessioneStatoCivile.GetListJson(SingletonURL.Instance.getRotte().ListaStatoCivile);
            if (connessioneStatoCivile.responseMessage != HttpStatusCode.OK)
            {
                await MessaggioConnessione.displayAlert((int)connessioneStatoCivile.responseMessage, connessioneStatoCivile.warning);
            }
        }

        public void StatoCivileScelto(StatoCivile stato)
        {
            utente.codStatoCivile = stato.id;
            utente.statocivile = stato.descrizione;
        }

        public void NazioneSelezionata(Nazione nation)
        {
            
            utente.luogo_nascita = nation.descrizione;
            utente.istatComuneNascita = nation.codiceCatastale;
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
            NameErrorNazione = false;
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
            if (string.IsNullOrEmpty(data_nascita.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)))
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
            if (string.IsNullOrEmpty(utente.luogo_nascita))
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
            if (string.IsNullOrEmpty(Username) || !(Username.Length >= 3 && Username.Length <=16))
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
                    NameErrorTextPassword = "Inserire una lettera maiuscola, minuscola ed un numero";
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
                await MessaggioConnessione.displayAlert((int)connessioneComuni.responseMessage, connessioneComuni.warning);
            }
            else
            {
                listaComuni = listacomuni;
            }

        }
        private async void LeggiComuniResidenza(Provincia provincia)
        {
            provinciaSelezionata = provincia;
            REST<Provincia, Comune> connessioneComuni = new REST<Provincia, Comune>();
            listaComuniResidenza = await connessioneComuni.PostJsonList(SingletonURL.Instance.getRotte().ListaComuni, provinciaSelezionata);
            if (connessioneComuni.responseMessage != HttpStatusCode.OK)
            {
                await MessaggioConnessione.displayAlert((int)connessioneComuni.responseMessage, connessioneComuni.warning);
            }
        }

        private async void LeggiProvince()
        {
            REST<object, Provincia> connessioneProvince = new REST<object, Provincia>();
            listaProvince = await connessioneProvince.GetListJson(SingletonURL.Instance.getRotte().ListaProvince);
            if (connessioneProvince.responseMessage != HttpStatusCode.OK)
            {
                await MessaggioConnessione.displayAlert((int)connessioneProvince.responseMessage, connessioneProvince.warning);
            }
        }

        public void provinciaDiNascitaSelezionato(Provincia provincia)
        {
            LeggiComuni(provincia);
        }

        public void provinciaDiResidenzaSelezionato(Provincia provincia)
        {
            utente.codIstatProvinciaResidenza = provincia.codIstat;
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

        public async void autocompila()
        {
            Comuni controlloComuneNascita = new Comuni();
            VisibleCdf = true;
            int y = 6;
            //anno
            codiceFiscale = codiceFiscale.ToUpper();
            char[] prova = new char[codiceFiscale.Length];
            prova = codiceFiscale.ToCharArray();

            while (y != 0)
            {
                switch (prova[y])
                {
                    case 'L':
                        prova[y] = '0';
                        break;
                    case 'M':
                        prova[y] = '1';
                        break;
                    case 'N':
                        prova[y] = '2';
                        break;
                    case 'P':
                        prova[y] = '3';
                        break;
                    case 'Q':
                        prova[y] = '4';
                        break;
                    case 'R':
                        prova[y] = '5';
                        break;
                    case 'S':
                        prova[y] = '6';
                        break;
                    case 'T':
                        prova[y] = '7';
                        break;
                    case 'U':
                        prova[y] = '8';
                        break;
                    case 'V':
                        prova[y] = '9';
                        break;
                    default:
                        break;
                }
                //controllo posizione e fine while
                if (y == 6)
                    y = 7;
                else if (y == 7)
                    y = 9;
                else if (y == 9)
                    y = 10;
                else if (y == 10)
                    y = 12;
                else if (y == 12)
                    y = 13;
                else if (y == 13)
                    y = 14;
                else if (y == 14)
                    y = 0;
            }
            y = 1;
            codiceFiscale = prova[0].ToString();
            while (y != prova.Length)
            {
                codiceFiscale = codiceFiscale + prova[y].ToString();
                y++;
            }
            int annoint = 2000 + Convert.ToInt16(codiceFiscale.Substring(6, 2));
            string anno;
            int annoCorrente = DateTime.Now.Year;
            if(annoint>annoCorrente)
                anno = "19" + codiceFiscale.Substring(6, 2);
            else
                anno = "20" + codiceFiscale.Substring(6, 2);

            string mese = codiceFiscale.Substring(8, 1);
            switch (mese)
            {
                case "A":
                    mese = "01";
                    break;
                case "B":
                    mese = "02";
                    break;
                case "C":
                    mese = "03";
                    break;
                case "D":
                    mese = "04";
                    break;
                case "E":
                    mese = "05";
                    break;
                case "H":
                    mese = "06";
                    break;
                case "L":
                    mese = "07";
                    break;
                case "M":
                    mese = "08";
                    break;
                case "P":
                    mese = "09";
                    break;
                case "R":
                    mese = "10";
                    break;
                case "S":
                    mese = "11";
                    break;
                case "T":
                    mese = "12";
                    break;
                default:
                    break;

            };
            char sesso;
            string giorno;
            int giornoint = Convert.ToInt16(codiceFiscale.Substring(9, 2));
            if (giornoint > 40)
            {
                sesso = 'F';
                giornoint = giornoint - 40;
            }
            else
            {
                sesso = 'M';
            }
            if (giornoint < 10)
            {
                giorno = "0" + giornoint.ToString();
            }
            else
                giorno = giornoint.ToString();

            data_nascita = DateTime.ParseExact(giorno + "/" + mese + "/" + anno, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            sceltaSesso = sesso;
            string sezCodFiscale = codiceFiscale.Substring(11, 4);
            controlloComuneNascita = await ComuneNascita(sezCodFiscale);
            Boolean flagNaziolanità = false;

            if (controlloComuneNascita.codice != null)
            {
                NationalVisibility = true;
                NationalVisibilityForeign = false;
                Provincia_nascitaNuovoContatto = controlloComuneNascita.provincia;
                provinciaSelezionata.provincia = controlloComuneNascita.provincia;
                provinciaSelezionata.codIstat = controlloComuneNascita.codIstat;
                Comune temp = new Comune();
                Luogo_nascitaNuovoContatto = controlloComuneNascita.nome;
                temp.codice = controlloComuneNascita.codice;
                temp.nome = controlloComuneNascita.nome;
                comuneNascitaSelezionato(temp);
                flagNaziolanità = true;
            }
            if (!flagNaziolanità)
            {
                foreach (var i in ListaNazioni)
                {
                    if (i.codiceCatastale == sezCodFiscale)
                    {

                        NationalVisibility = false;
                        NationalVisibilityForeign = true;
                        Luogo_nascitaNuovoContatto = i.descrizione;
                        utente.luogo_nascita = i.descrizione;
                        utente.istatComuneNascita = i.codiceCatastale;
                        break;
                    }
                }
            }
        }
        public async Task<Comuni> ComuneNascita(string temp2)
        {
            Comuni temp = new Comuni();
            REST<string, Comuni> connessioneComuni = new REST<string, Comuni>();
            temp = await connessioneComuni.GetSingleJson(SingletonURL.Instance.getRotte().comunebycodicecatastale + "?codcatastale=" + temp2);
            if (connessioneComuni.responseMessage != HttpStatusCode.OK)
            {
                //await MessaggioConnessione.displayAlert((int)connessioneComuni.responseMessage, connessioneComuni.warning);
                return new Comuni();
            }
            else
                return temp;
        }
        #endregion
    }
    public class Comuni
    {
        public string nome { get; set; }
        public string codice { get; set; }
        public string provincia { get; set; }
        public string codIstat { get; set; }
    }
}
