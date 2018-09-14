using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
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
    class ModificaContattoModelView : INotifyPropertyChanged

    {


        #region DichiarazioneVariabili

        public event PropertyChangedEventHandler PropertyChanged; //evento che implementa l'interfaccia INotifyPropertyChanged

        private Assistito contatto; //Oggetto che astrae l'utenza del cliente

        private List<Comune> listaComuniResidenza, listaComuniNascita;//Lista di comuni di nascita e residenza

        private List<StatoCivile> listaStatoCivile = new List<StatoCivile>();//lista che contiene gli stati civili che arriveranno dal server

        private List<Provincia> province;//Lista province

        private Provincia provincia = new Provincia();


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

        public ICommand ModificaContatto { protected set; get; } //Command per il tentativo di registrazione le modifiche dell'utenza
        public ICommand AnnullaModificheContatto { protected set; get; }

        public string CodiceFiscaleContatto //Proprietà relativa al campo codice fiscale
        {
            get { return contatto.codice_fiscale; }
            set
            {
                OnPropertyChanged();
                contatto.codice_fiscale = value;
            }
        }

        public string comune_residenza
        {
            get { return contatto.comune_residenza; }
            set
            {
                OnPropertyChanged();
                contatto.comune_residenza = value;
            }
        }

        public string telefono
        {
            get { return contatto.telefono; }
            set
            {
                OnPropertyChanged();
                contatto.telefono = value;
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

        public string NomeContatto //Proprietà relativa al campo nome
        {
            get { return contatto.nome; }
            set
            {
                OnPropertyChanged();
                contatto.nome = value;
            }
        }

        public string CognomeContatto //Proprietà relativa al campo cognome
        {
            get { return contatto.cognome; }
            set
            {
                OnPropertyChanged();
                contatto.cognome = value;
            }
        }

        public string Email //Proprietà relativa al campo Email
        {
            get { return contatto.email; }
            set
            {
                OnPropertyChanged();
                contatto.email = value;
            }
        }
        public string IndirizzoRes //Proprietà relativa al campo indirizzoRes
        {
            get { return contatto.indirizzores; }
            set
            {
                OnPropertyChanged();
                contatto.indirizzores = value;
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

        public string Data_nascitaContatto //Proprietà relativa al campo data di nascita
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
                contatto.luogo_nascita = value;
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


        #endregion

        #region OnPropertyChange

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        #endregion

        #region Costruttore
        //Costruttore che inizializza un utenza vuota e definisce il metodo a cui il Command registrati fa riferimento
        public ModificaContattoModelView(Assistito utenza)
        {
            contatto = new Assistito(); //Crea un utenza vuota
            //assegno a contatto i valori dell'utentenza
            contatto = utenza;
            //assegno a le entry i valori di utenza
            CodiceFiscaleContatto = contatto.codice_fiscale;
            NomeContatto = contatto.nome;
            CognomeContatto = contatto.cognome;
            comune_residenza = contatto.comune_residenza;
            Data_nascitaContatto = contatto.data_nascita;
            Luogo_nascitaNuovoContatto = contatto.luogo_nascita;
            sceltaSesso = contatto.sesso;
            Email = contatto.email;
            IndirizzoRes = contatto.indirizzores;
            var token = App.Current.Properties["tokenLogin"].ToString();
            LeggiProvince();
            LeggiStatoCivile();
            AnnullaModificheContatto = new Command(() =>
            {
                App.Current.MainPage = new MenuPrincipale("Contatti");
            });
            ModificaContatto = new Command(async () =>
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
                /// 

                if (string.IsNullOrEmpty(contatto.comune_residenza))
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
                if (string.IsNullOrEmpty(CodiceFiscaleContatto))
                {
                    NameErrorCodFiscale = true;
                    controllPass = false;
                }

                #endregion

                //Se controllPass è rimasto True dopo il controllo errori allora effettuo la procedura di registrazione

                if (controllPass) //Controlla se l'utente ha riempito tutti i campi obbligatori
                {
                    List<Header> listaHeader = new List<Header>();
                    listaHeader.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
                    contatto.provincia = provincia.provincia;
                    contatto.codIstatProvinciaResidenza = provincia.codIstat;
                    REST<Assistito, ResponseRegistrazione> connessioneModificaContatto = new REST<Assistito, ResponseRegistrazione>();
                    try
                    {
                        contatto.Maiuscolo();
                        contatto.imgSesso = string.Empty;
                        ResponseRegistrazione response = await connessioneModificaContatto.PostJson(SingletonURL.Instance.getRotte().modAssistito, contatto, listaHeader);
                        if (connessioneModificaContatto.responseMessage != HttpStatusCode.OK)
                        {
                            await MessaggioConnessione.displayAlert((int)connessioneModificaContatto.responseMessage, connessioneModificaContatto.warning);
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Modifica Contatto", "Il contatto è stato modificato correttamente", "OK");
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

        #endregion

        #region Metodi

        //Metodo che restituisce la lista delle province proveniente dal server
        private async void LeggiProvince()
        {
            REST<object, Provincia> connessioneProvince = new REST<object, Provincia>();
            Province = await connessioneProvince.GetListJson(SingletonURL.Instance.getRotte().ListaProvince);
            if (connessioneProvince.responseMessage != HttpStatusCode.OK)
            {
                await MessaggioConnessione.displayAlert((int)connessioneProvince.responseMessage, connessioneProvince.warning);
            }
            else
            {
                REST<Provincia, Comune> connComuni = new REST<Provincia, Comune>();
                provincia.provincia = contatto.provincia;
                provincia.codIstat = contatto.codIstatProvinciaResidenza;
                ListaComuniResidenza = await connComuni.PostJsonList(SingletonURL.Instance.getRotte().ListaComuni, provincia);
                if(connComuni.responseMessage != HttpStatusCode.OK)
                    await MessaggioConnessione.displayAlert((int)connComuni.responseMessage, connComuni.warning);
            }
        }

        //Metodo che restituisce, dopo aver selezionato la provincia, la lista dei comuni
        public async void LeggiComuniResidenza(Provincia provincia)
        {
            this.provincia = provincia;
            REST<Provincia, Comune> connessioneComuni = new REST<Provincia, Comune>();
            ListaComuniResidenza = await connessioneComuni.PostJsonList(SingletonURL.Instance.getRotte().ListaComuni, provincia);
            if (connessioneComuni.responseMessage != HttpStatusCode.OK)
                await MessaggioConnessione.displayAlert((int)connessioneComuni.responseMessage, connessioneComuni.warning);
        }

        public async void LeggiComuniNascita(Provincia provincia)
        {
            Provincia provinciaSelezionata = new Provincia();
            provinciaSelezionata = provincia;
            REST<Provincia, Comune> connessioneComuni = new REST<Provincia, Comune>();
            ListaComuniNascita = await connessioneComuni.PostJsonList(SingletonURL.Instance.getRotte().ListaComuni, provinciaSelezionata);
            if (connessioneComuni.responseMessage != HttpStatusCode.OK)
            {
                await MessaggioConnessione.displayAlert((int)connessioneComuni.responseMessage, connessioneComuni.warning);
            }
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

        //Metodo che restituisce la lista degli stati civili
        private async void LeggiStatoCivile()
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
            contatto.codStatoCivile = stato.id;
            contatto.statocivile = stato.descrizione;
        }

        #endregion
    }
}
