#region LibrerieUsate

using MCup.Model;
using MCup.Service;
using MCup.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


#endregion

namespace MCup.ModelView
{
    //ModelView della pagina FormPrenotazione, tale classe è utilizzata per implementare il binding con la relativa pagina
    public class FormPrenotazioneModelView : INotifyPropertyChanged
    {
        #region RegioneDiInizializzazioneEDichiarazione
       
        //Evento che prevede il cambiamento di proprietà all'interno della classe
        public event PropertyChangedEventHandler PropertyChanged;
        //Oggetto che astrae l'impegnativa che invieremo per prenotare
        private Impegnativa invioImpegnativa;
        //stringa che modifichera la visibilità di uno o più elementi nello xaml
        private string visible = "false";
        //Lista di header
        private List<Header> headers = new List<Header>();
        //Variabile che controlla che il nome e cognome non contenga caratteri numerici o simboli
        private Regex regexNomeCognome = new Regex(@"^[A-Za-zèùàòé][a-zA-Z'èùàòé ]*$");
        //Oggetto che astrae l'utente che intende prenotare una o delle prestazioni
        private Assistito utenza;
        private string sar;

        private string placeHolderCodiceImpegnativaSarONre;
        //Oggetto che astrae la ricetta NRE
        private InvioRicettaPrenotazione ricetta;
        //Booleano che abiliterà o non abiliterà gli elementi nello xaml
        private bool isenabled;

        private bool isEnabledCodDue;
        //Booleano di controllo, che ci permetterà di capire se la ricetta sarà sar o nre
        private bool switchSarIstrueOrFalse = false;
        //Stringhe di controllo, saranno visibili solo nel caso in cui ci sia un errore nella entry
        private string nameTextErrorNome,
            nameTextErrorCognome,
            nameTextErrorCodFisc,
            nameTextErrorCodUno,
            nameTextErrorCodDue,
            nameErrorCodiceSar;
        //Oggetto che astrae la pagina a cui punta il modelView in questione.
        private FormPrenotazione model;
        //Lista di tipo Assistito
        private List<Assistito> contatti = new List<Assistito>();
        //Lista di tipo Assistito che conterrà i contatti
        private List<Assistito> contacts;

        #endregion

        #region ProprietaGetSet

        //Proprietà che sarà usata solo se il campo sarà vuoto o non corrisponderà con il nome salvato nel database

        public string NameTextErrorNome
        {
            get { return nameTextErrorNome; }
            set
            {
                OnPropertyChanged();
                nameTextErrorNome = value;
            }
        }

        //Proprietà che eseguirà la richiesta di invio della ricetta al server
        public ICommand InviaRichiesta { protected set; get; }


        //Proprietà che sarà usata solo se l'utente avrà abilitato lo switch del sar ed il codice del sar non sarà esatto

        public string NameErrorCodiceSar
        {
            get { return nameErrorCodiceSar; }
            set
            {
                OnPropertyChanged();
                nameErrorCodiceSar = value;
            }
        }

        //Proprietà che sarà usata solo se il cognome avrà errori ortografici o non corrisponderà con il cognome salvato nel database

        public string NameTextErrorCognome
        {
            get { return nameTextErrorCognome; }
            set
            {
                OnPropertyChanged();
                nameTextErrorCognome = value;
            }
        }

        //Proprietà che sarà usata solo se il codice fiscale avrà errori ortografici o non corrisponderà con il codice fiscale salvato nel database

        public string NameTextErrorCodFisc
        {
            get { return nameTextErrorCodFisc; }
            set
            {
                OnPropertyChanged();
                nameTextErrorCodFisc = value;
            }
        }
        //Proprietà che sarà usata solo se il primo codice dell'impegnativa sarà errato

        public string NameTextErrorCodUno
        {
            get { return nameTextErrorCodUno; }
            set
            {
                OnPropertyChanged();
                nameTextErrorCodUno = value;
            }
        }

        //Proprietà che sarà usata solo se il primo codice dell'impegnativa sarà errato

        public string NameTextErrorCodDue
        {
            get { return nameTextErrorCodDue; }
            set
            {
                OnPropertyChanged();
                nameTextErrorCodDue = value;
            }
        }

        //Proprietà che contiene tutte le informazioni della prenotazione che si vuole effettuare


        public List<Assistito> Contatti
        {
            get { return contatti; }
            set
            {
                OnPropertyChanged();
                contatti = new List<Assistito>(value);
                if (value.Count == 1)
                {
                    model.Picker_SelezionaPrimoElemento(value[0]);
                }
            }
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

        //Proprietà che definisce il codice Sar di 15 caratteri

        public string codiceSar
        {
            get { return sar; }
            set
            {
                sar = value;
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

        public string PlaceHolderCodiceImpegnativaSarONre
        {
            get { return placeHolderCodiceImpegnativaSarONre; }
            set
            {
               OnPropertyChanged();
                placeHolderCodiceImpegnativaSarONre = value;
            }
        }
        //Proprietà che definisce il primo codice della ricetta che sta effettuando la prenotazione
        public string codiceUno
        {
            get { return ricetta.codice_uno; }
            set
            {
                ricetta.codice_uno = value;
                if (ricetta.codice_uno.Length != 5)
                {
                    PlaceHolderCodiceImpegnativaSarONre = "Inserisci il codice Impegnativa";
                    IsEnabledCodiceDue = false;
                }
                else
                {
                    IsEnabledCodiceDue = true;
                }

                if (ricetta.codice_uno.Length > 5)
                {
                    PlaceHolderCodiceImpegnativaSarONre = "Inserisci codice SAR";
                }
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

        public bool IsEnabledCodiceDue
        {
            get { return isEnabledCodDue; }
            set
            {
                OnPropertyChanged();
                isEnabledCodDue = value;
            }
        }

        //Proprietà che definisce la possibilità di abilitare o disabilitare gli elementi all'interno della pagina
        public bool IsEnabled
        {
            get { return isenabled; }
            set
            {
                OnPropertyChanged();
                isenabled = value;
            }
        }
   

        //Proprietà che definisce la possibilità di rendere visibili gli elementi all'interno della pagina
        public string Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Costruttore

        //Costruttore del ModelView, viene passato come parametro il riferimento alla pagina che lo richiama per poter effettuare una Navigation.pushAsync
        public FormPrenotazioneModelView(FormPrenotazione Model, bool prenotazionePending)
        {
            IsEnabled = true;
            utenza = new Assistito();
            ricetta = new InvioRicettaPrenotazione();
            invioImpegnativa = new Impegnativa();
            headers.Add(new Header("struttura", "150907"));
            headers.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
            model = Model;
            ricetta.codice_uno = "";
            ricetta.codice_due = "";
            utenza.nome = "";
            utenza.cognome = "";
            utenza.codice_fiscale = "";
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


        #endregion

        #region Metodi

        //Metodo che tramite una connessione restituisce i contatti collegati al token dell'applicazione
        private async void leggiContatti()
        {
            REST<object, Assistito> rest = new REST<object, Assistito>();
            List<Header> listaHeader = new List<Header>();
            listaHeader.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
            contacts = await rest.GetListJson(SingletonURL.Instance.getRotte().InfoPersonali, listaHeader);
            if (rest.responseMessage != HttpStatusCode.OK)
            {
                await App.Current.MainPage.DisplayAlert("Attenzione " + (int)rest.responseMessage, rest.warning, "OK");
            }
            else
            {
                Contatti = contacts.OrderBy(o => o.nomeCompletoConCodiceFiscale).ToList();
            }
        }

        //Metodo che richimiamo solo nel caso in cui l'applicativo è stato chiuso o crashato, ed restituisce, tramite connessione, tutti i campi riempiti della pagina
        public async void RiempiPagina()
        {
            REST<object, Impegnativa> connessione = new REST<object, Impegnativa>();
            List<Header> listaHeader = new List<Header>();
            listaHeader.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
            Impegnativa response = await connessione.GetSingleJson(SingletonURL.Instance.getRotte().ricezioneDatiPrenotazione, listaHeader);
            if (connessione.responseMessage == HttpStatusCode.OK)
            {
                model.selezionaElemento(response.assistito);
                if (response.sar)
                {
                    codiceUno = response.nre;
                }
                else
                {
                    codiceUno = response.nre.Substring(0, 5);
                    codiceDue = response.nre.Substring(5);
                }
            }

        }

        //Metodo che viene richiamato quando l'utente sceglie il contatto per cui prenotare dal picker
        public void autoCompila(Assistito elementSelected)
        {
            nomeUtente = elementSelected.nome;
            cognomeUtente = elementSelected.cognome;
            codicefiscaleUtente = elementSelected.codice_fiscale;
            invioImpegnativa.assistito = elementSelected;
            Visible = "true";
        }

        //Funzione utilizzata per l'invio della richiesta di prenotazione al servizio
        public async Task InvioDatiAsync()
        {
            #region RegioneDiControllo

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
                else if (ricetta.codice_uno.Length != 5 && ricetta.codice_uno.Length != 15)
                {
                    NameTextErrorCodUno = "Il campo deve contentere un codice impegnativa valido";
                    passControl = false;
                }
                else
                    NameTextErrorCodUno = "";
            if (IsEnabledCodiceDue)
            {
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
            }
                
            


            #endregion

            #region RegioneControlloRiuscito

            if (passControl)
            {
                try
                {
                   
                    REST<Impegnativa, Impegnativa> connessione = new REST<Impegnativa, Impegnativa>();
                    if (codiceUno.Length==15)
                    {
                        codiceSar = codiceUno;
                        invioImpegnativa.nre = codiceSar;
                        invioImpegnativa.sar = true;
                    }
                    else
                    {
                        invioImpegnativa.nre = codiceUno + codiceDue;
                        invioImpegnativa.sar = false;

                    }

                    Impegnativa response = await connessione.PostJson(SingletonURL.Instance.getRotte().Ricetta, invioImpegnativa, headers);
                    if (connessione.responseMessage == HttpStatusCode.OK)
                        model.metodoPush(response, invioImpegnativa.assistito);
                    else if (connessione.responseMessage == HttpStatusCode.BadRequest)
                        await App.Current.MainPage.DisplayAlert("Attenzione 400", connessione.warning, "OK");
                    else
                        await App.Current.MainPage.DisplayAlert("Attenzione " + (int)connessione.responseMessage, connessione.warning, "OK");
                }
                catch (Exception e)
                {
                    await App.Current.MainPage.DisplayAlert("Attenzione", "connessione non riuscita o codici impegnativa errati", "riprova");
                }
            }

            #endregion

        }
        #endregion

        #region PropertyChange

        //Comando che chiama la funzione asincrona InvioDatiAsync()

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        #endregion

        #region ClassiPrivate

        // classe privata che richiamiamo per inviare i dati della ricetta al server
        private class InvioRicettaPrenotazione
        {
            public string codice_uno;
            public string codice_due;
        }

        #endregion

    }
}
