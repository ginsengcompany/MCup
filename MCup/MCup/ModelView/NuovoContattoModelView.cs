using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MCup.Model;
using MCup.Service;
using MCup.Views;
using Xamarin.Forms;
using XLabs;

namespace MCup.ModelView
{
    public class NuovoContattoModelView : INotifyPropertyChanged

    {
        public event PropertyChangedEventHandler PropertyChanged; //evento che implementa l'interfaccia INotifyPropertyChanged

        private Assistito contatto; //Oggetto che astrae l'utenza del cliente

        private List<Comune> listaComuniResidenza, listaComuniNascita;

        private List<StatoCivile> listaStatoCivile = new List<StatoCivile>();

        private List<Provincia> province;

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
                        nameErrorStatoCivile=false,
                        nameErrorTelefono = false,
                        nameErrorSesso = false,
                        nameErrorCodFiscale = false;
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

        public ICommand registraNuovoContatto { protected set; get; } //Command per il tentativo di registrazione dell'utenza
        public ICommand annullaRegistrazioneNuovoContatto { protected set; get; }

        public string CodiceFiscaleNuovoContatto //Proprietà relativa al campo codice fiscale
        {
            get { return contatto.codice_fiscale; }
            set
            {
                OnPropertyChanged();
                contatto.codice_fiscale = value.ToUpper();
            }
        }

        public string comune_residenza
        {
            get { return contatto.comune_residenza; }
            set
            {
                OnPropertyChanged();
                contatto.comune_residenza = value.ToUpper();
            }
        }

        public string telefono
        {
            get { return contatto.telefono; }
            set
            {
                OnPropertyChanged();
                contatto.telefono = value.ToUpper();
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

        public string NomeNuovoContatto //Proprietà relativa al campo nome
        {
            get { return contatto.nome; }
            set
            {
                OnPropertyChanged();
                contatto.nome = value.ToUpper();
            }
        }

        public string CognomeNuovoContatto //Proprietà relativa al campo cognome
        {
            get { return contatto.cognome; }
            set
            {
                OnPropertyChanged();
                contatto.cognome = value.ToUpper();
            }
        }

        public Char sceltaSesso //Proprietà relativa al campo sesso
        {
            get { return contatto.sesso; }
            set
            {
                OnPropertyChanged();
                contatto.sesso = value;
            }
        }

        public string Data_nascitaNuovoContatto //Proprietà relativa al campo data di nascita
        {
            get { return contatto.data_nascita; }
            set
            {
                OnPropertyChanged();
                contatto.data_nascita = value;
            }
        }

        public string Luogo_nascitaNuovoContatto //Proprietà relativa al campo luogo di nascita
        {
            get { return contatto.luogo_nascita; }
            set
            {
                OnPropertyChanged();
                contatto.luogo_nascita = value.ToUpper();
            }
        }

        public List<Provincia> Province
        {
            get { return province; }
            set
            {
                OnPropertyChanged();
                province = value;
            }
        }
        
        public List<Comune> ListaComuniResidenza
        {
            get { return listaComuniResidenza; }
            set
            {
                OnPropertyChanged();
                listaComuniResidenza = value;
            }
        }

        public List<Comune> ListaComuniNascita
        {
            get { return listaComuniNascita; }
            set
            {
                OnPropertyChanged();
                listaComuniNascita = value;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        //Costruttore che inizializza un utenza vuota e definisce il metodo a cui il Command registrati fa riferimento
        public NuovoContattoModelView()
        {
            contatto = new Assistito(); //Crea un utenza vuota
            var token = App.Current.Properties["tokenLogin"].ToString();
            LeggiProvince();
            LeggiStatoCivile();
            annullaRegistrazioneNuovoContatto = new Command(() =>
            {
                App.Current.MainPage = new MenuPrincipale("Contatti");

            });
            registraNuovoContatto = new Command(async () =>
            {
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
                bool controllPass = true;
                ///<summary>
                ///Controllo se i capi sono nulli, se uno è nulla la registrazione non verrà effettuata e il campo o i campi vuoi verragno segnalati
                ///grazie alla label di errore.
                /// </summary>
                
                if (string.IsNullOrEmpty(NomeNuovoContatto))
                {
                    NameErrorNome = true;
                    controllPass = false;
                }
                else
                {
                    NameErrorNome = false;
                }
                if (string.IsNullOrEmpty(CognomeNuovoContatto))
                {
                    NameErrorCognome = true;
                    controllPass = false;
                }
                else
                {
                    NameErrorCognome = false;
                }
                if (string.IsNullOrEmpty(contatto.data_nascita))
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
                if (string.IsNullOrEmpty(contatto.luogo_nascita))
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
                if (string.IsNullOrEmpty(contatto.comune_residenza))
                {
                    NameErrorComuneResidenza = true;
                    controllPass = false;
                }
                else
                {
                    NameErrorComuneResidenza = false;
                }
                if (string.IsNullOrEmpty(contatto.statocivile))
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
                if (string.IsNullOrEmpty(CodiceFiscaleNuovoContatto))
                {
                    NameErrorCodFiscale = true;
                    controllPass = false;
                }
                else
                {
                    NameErrorCodFiscale = false;
                }

                #endregion

                //Se controllPass è rimasto True dopo il controllo errori allora effettuo la procedura di registrazione

                if (controllPass) //Controlla se l'utente ha riempito tutti i campi obbligatori
                {
                    string giorno, mese, anno;
                    giorno = contatto.data_nascita.Substring(3, 2);
                    mese = contatto.data_nascita.Substring(0, 2);
                    anno = contatto.data_nascita.Substring(6, 4);
                    contatto.data_nascita = giorno + "/" + mese + "/" + anno;
                    REST<Assistito, ResponseRegistrazione> connessioneNuovoContatto = new REST<Assistito, ResponseRegistrazione>();
                    try
                    {
                        contatto.Maiuscolo();
                        ResponseRegistrazione response = await connessioneNuovoContatto.PostJson(URL.AggiungiNuovoContatto, contatto, token);
                        if ((response == null) || (response == default(ResponseRegistrazione)))
                        {
                            await App.Current.MainPage.DisplayAlert("Registrazione contatto", connessioneNuovoContatto.warning, "ok");
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Nuovo contatto", "Il contatto è stato aggiunto correttamente", "OK");
                            App.Current.MainPage = new MenuPrincipale("Contatti");
                        }

                    }
                    catch (Exception)
                    {
                        await App.Current.MainPage.DisplayAlert("Attenzione", "connessione non riuscita", "riprova");
                    }
                }
            });
        }

        private async void LeggiProvince()
        {
            REST<object, Provincia> connessioneProvince = new REST<object, Provincia>();
            Province = await connessioneProvince.GetJson(URL.ListaProvince);
        }

        public async void LeggiComuniResidenza(Provincia provincia)
        {
            Provincia provinciaSelezionata = new Provincia();
            provinciaSelezionata = provincia;
            REST<Provincia, Comune> connessioneComuni = new REST<Provincia, Comune>();
            ListaComuniResidenza = await connessioneComuni.PostJsonList(URL.ListaComuni, provinciaSelezionata);
        }

        public async void LeggiComuniNascita(Provincia provincia)
        {
            Provincia provinciaSelezionata = new Provincia();
            provinciaSelezionata = provincia;
            REST<Provincia, Comune> connessioneComuni = new REST<Provincia, Comune>();
            ListaComuniNascita = await connessioneComuni.PostJsonList(URL.ListaComuni, provinciaSelezionata);
        }

        public void comuneResidenzaSelezionato(Comune comune)
        {
            contatto.comune_residenza = comune.nome;
            contatto.istatComuneResidenza = comune.codice;
        }

        public void comuneNascitaSelezionato(Comune comune)
        {
            contatto.luogo_nascita = comune.nome;
            contatto.istatComuneNascita = comune.codice;
        }
        private async void LeggiStatoCivile()
        {
            REST<object, StatoCivile> connessioneStatoCivile = new REST<object, StatoCivile>();
            ListaStatoCivile = await connessioneStatoCivile.GetJson(URL.ListaStatoCivile);
        }

        public void StatoCivileScelto(StatoCivile stato)
        {
            contatto.codStatoCivile = stato.id;
            contatto.statocivile = stato.descrizione;
        }

    }
}
