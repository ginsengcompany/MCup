﻿using MCup.Model;
using MCup.Service;
using MCup.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MCup.ModelView
{

    public class AutoCompilazioneModelView : INotifyPropertyChanged
    {
        #region DichiarazioneVariabili
        private List<Nazione> listanazioni = new List<Nazione>();
        private bool visibleCdf = false;
        private bool nationalVisibilityForeign, nationalVisibility, nameErrorNazione = false;
        public event PropertyChangedEventHandler PropertyChanged; //evento che implementa l'interfaccia INotifyPropertyChanged
        private Assistito contatto; //Oggetto che astrae l'utenza del cliente
        Provincia provinciaSelezionata = new Provincia();

        private List<Comune> listaComuniResidenza, listaComuniNascita;//Lista di comuni di nascita e residenza

        private List<StatoCivile> listaStatoCivile = new List<StatoCivile>();//lista che contiene gli stati civili che arriveranno dal server

        private List<Provincia> province;//Lista province

        private string provinciaNascita; //Oggetto che astrae l'utenza del cliente


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


        public ICommand registraNuovoContatto { protected set; get; } //Command per il tentativo di registrazione dell'utenza
        public ICommand annullaRegistrazioneNuovoContatto { protected set; get; }

        public string CodiceFiscaleNuovoContatto //Proprietà relativa al campo codice fiscale
        {
            get { return contatto.codice_fiscale; }
            set
            {
                OnPropertyChanged();
                contatto.codice_fiscale = value;
                if (contatto.codice_fiscale.Length == 16)
                {
                    AutoCompilazioneConnessione();
                }
                else
                {
                    visibleCdf = false;
                }
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

        public string NomeNuovoContatto //Proprietà relativa al campo nome
        {
            get { return contatto.nome; }
            set
            {
                OnPropertyChanged();
                contatto.nome = value;
            }
        }

        public string Indirizzo //Proprietà relativa al campo nome
        {
            get { return contatto.indirizzores; }
            set
            {
                OnPropertyChanged();
                contatto.indirizzores = value;
            }
        }

        public string Email //Proprietà relativa al campo nome
        {
            get { return contatto.email; }
            set
            {
                OnPropertyChanged();
                contatto.email = value;
            }
        }

        public string CognomeNuovoContatto //Proprietà relativa al campo cognome
        {
            get { return contatto.cognome; }
            set
            {
                OnPropertyChanged();
                contatto.cognome = value;
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

        public DateTime Data_nascitaNuovoContatto //Proprietà relativa al campo data di nascita
        {
            get {
                if (string.IsNullOrWhiteSpace(contatto.data_nascita))
                    return DateTime.Today;
                return DateTime.ParseExact(contatto.data_nascita, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            set
            {
                OnPropertyChanged();
                contatto.data_nascita = value.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
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
        public string Provincia_nascitaNuovoContatto //Proprietà relativa al campo luogo di nascita
        {
            get { return provinciaNascita; }
            set
            {
                OnPropertyChanged();
                provinciaNascita = value;
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
        public AutoCompilazioneModelView()
        {
            NationalVisibilityForeign = false;
            NationalVisibility = true;

            RicezioneNazioni();
            LeggiProvince();
            LeggiStatoCivile();

            contatto = new Assistito(); //Crea un utenza vuota
            var token = App.Current.Properties["tokenLogin"].ToString();

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

                if (string.IsNullOrEmpty(CognomeNuovoContatto))
                {
                    NameErrorCognome = true;
                    controllPass = false;
                }
                if (string.IsNullOrEmpty(contatto.data_nascita))
                {
                    NameErrorDataNascita = true;
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
                if (string.IsNullOrEmpty(contatto.luogo_nascita))
                {
                    NameErrorComuneNascita = true;
                    controllPass = false;
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
                if (string.IsNullOrEmpty(CodiceFiscaleNuovoContatto))
                {
                    NameErrorCodFiscale = true;
                    controllPass = false;
                }

                #endregion

                //Se controllPass è rimasto True dopo il controllo errori allora effettuo la procedura di registrazione

                if (controllPass) //Controlla se l'utente ha riempito tutti i campi obbligatori
                {
                    string giorno, mese, anno;
                    List<Header> listaHeader = new List<Header>();
                    listaHeader.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
                    contatto.provincia = provinciaSelezionata.provincia;
                    contatto.codIstatProvinciaResidenza = provinciaSelezionata.codIstat;
                    REST<Assistito, ResponseRegistrazione> connessioneNuovoContatto = new REST<Assistito, ResponseRegistrazione>();
                    try
                    {
                        contatto.Maiuscolo();
                        ResponseRegistrazione response = await connessioneNuovoContatto.PostJson(SingletonURL.Instance.getRotte().AggiungiNuovoContatto, contatto, listaHeader);
                        if (connessioneNuovoContatto.responseMessage == HttpStatusCode.BadRequest)
                            await MessaggioConnessione.displayAlert((int)connessioneNuovoContatto.responseMessage, "Controllare tutti i dati");
                        else if (connessioneNuovoContatto.responseMessage != HttpStatusCode.Created)
                            await MessaggioConnessione.displayAlert((int)connessioneNuovoContatto.responseMessage, connessioneNuovoContatto.warning);
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
        }

        //Metodo che restituisce, dopo aver selezionato la provincia, la lista dei comuni
        public async void LeggiComuniResidenza(Provincia provincia)
        {
            provinciaSelezionata = provincia;
            REST<Provincia, Comune> connessioneComuni = new REST<Provincia, Comune>();
            ListaComuniResidenza = await connessioneComuni.PostJsonList(SingletonURL.Instance.getRotte().ListaComuni, provinciaSelezionata);
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
        public async void RicezioneNazioni()
        {
            REST<object, Nazione> connessioneNazioni = new REST<object, Nazione>();
            ListaNazioni = await connessioneNazioni.GetListJson(SingletonURL.Instance.getRotte().listaNazioni);
            if (connessioneNazioni.responseMessage != HttpStatusCode.OK)
            {
                await MessaggioConnessione.displayAlert((int)connessioneNazioni.responseMessage, connessioneNazioni.warning);
            }
            else
            {
                ListaNazioni = listanazioni.OrderBy(o => o.descrizione).ToList();
            }
        }
        public void NazioneSelezionata(Nazione nation)
        {

            contatto.luogo_nascita = nation.descrizione;
            contatto.istatComuneNascita = nation.codiceCatastale;
        }
        public void sceltaNazione(bool statoEstero)
        {
            if (statoEstero)
            {

                NationalVisibilityForeign = true;
                NationalVisibility = false;
                contatto.luogo_nascita = String.Empty;
            }
            else
            {
                NationalVisibilityForeign = false;
                NationalVisibility = true;
                contatto.luogo_nascita = String.Empty;
            }
        }

        #endregion
        
     

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


        public async void AutoCompilazioneConnessione()
        {
            
            REST<object,AutoCompilazione> connessione = new REST<object, AutoCompilazione>();
            List<Header> header = new List<Header>();
            header.Add(new Header("codfisc",CodiceFiscaleNuovoContatto.ToUpper()));
            if (CodiceFiscaleNuovoContatto.Length ==16 )
            {
                var response = await connessione.GetSingleJson(SingletonURL.Instance.getRotte().converticodicefiscale, header);
                if (connessione.responseMessage != HttpStatusCode.OK)
                {
                    await App.Current.MainPage.DisplayAlert("Attenzione", connessione.warning, "ok");
                }
                else
                {
                    VisibleCdf = true;
                    Comuni controlloComuneNascita = new Comuni();
                    sceltaSesso = response.sesso[0];
                    controlloComuneNascita = await ComuneNascita(response.codcatastale);
                    Data_nascitaNuovoContatto = DateTime.ParseExact(response.datanascita, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
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
                            if (i.codiceCatastale == response.codcatastale)
                            {

                                NationalVisibility = false;
                                NationalVisibilityForeign = true;
                                Luogo_nascitaNuovoContatto = i.descrizione;
                                contatto.luogo_nascita = i.descrizione;
                                contatto.istatComuneNascita = i.codiceCatastale;
                                break;
                            }
                        }
                    }
                }
            }
         
        }
    }

}


public class Comuni
{
    public string nome { get; set; }
    public string codice { get; set; }
    public string provincia { get; set; }
    public string codIstat { get; set; }
}