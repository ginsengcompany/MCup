using MCup.Model;
using MCup.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using MCup.Views;
using Xamarin.Forms;

namespace MCup.ModelView
{
    public class PropostaRichiestaModelView : INotifyPropertyChanged
    {

        #region DichiarazioneVariabili
        private char[] alfabeto = { 'ì', 'a', 'b', 'c', 'd', 'D', 'e', 'f', 'g', 'G', 'h', 'i', 'l', 'L', 'm', 'M', 'n', 'o', 'p', 'q', 'r', 's', 'S', 't', 'u', 'v', 'V', 'z', ' ' };
        private bool isvisible, isbusy, isvisibleButton, isenabled, isbusyV;//Variabili booleane utilizzate per rendere visibili e abilitati elementi nello xaml
        private Assistito contatto;//Oggetto di tipo assistito, da cui raccoglieremo le informazioni del contatto
        private string visible = "true";//Variabile utilizzata per rendere visibile o meno oggetti nello xaml
        private string messaggio = "";//Variabile utilizzata per dire all'utente quali esami sono disponibili o meno
        private string visibleHome = "false";//Variabile utilizzata per rendere visibile o meno oggetti nello xaml
        private int count = 0;//Variabile che tiene conto di quanti esami non sono disponibili
        private Impegnativa impegnativa;//Oggetto di tipo impegnativa da cui traiamo le informazioni dell'impegnativa
        private PropostaRichiesta propostaRichiesta;//Oggetto di tipo della pagina da cui il model view si riferisce
        private AppuntamentoProposto appuntamentoProposto = new AppuntamentoProposto();//Oggetto di tipo Appuntamentoproposto che utilizzeremo per far vedere all'utente l'appuntamento proposto dal sistema.
        private List<Header> headers = new List<Header>();//lista di header
        private List<Prestazione> prestazioni;//Lista di prestazioni
        public event PropertyChangedEventHandler PropertyChanged;//evento on property change

        #endregion

        #region OnPropertyChange

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
       
        #region Proprietà

        //Comando che richiama un metodo che annulla la prenotazione in sospeso
        public ICommand AnnullaPrenotazione { protected set; get; }
 
        //proprietà riferita al campo ListPrestazioni
        public List<AppuntamentoPrestazioneProposto> ListPrenotazioni
        {
            get { return appuntamentoProposto.appuntamenti; }
            set
            {
                OnPropertyChanged();
                appuntamentoProposto.appuntamenti = value;
            }
        }
        
        //Proprietà riferita al campo Visible
        public string Visible
        {
            get { return visible; }
            set
            {
                OnPropertyChanged();
                visible = value;
            }
        }

        //Proprietà riferita al campo VisibleHome
        public string VisibleHome
        {
            get { return visibleHome; }
            set
            {
                OnPropertyChanged();
                visibleHome = value;
            }
        }

        //Comando usato per inviare i dati per la conferma della prenotazione
        public ICommand InvioDatiPerPrenotazione
        {
            get
            {
                return new Command(async () =>
                {
                    IsEnabled = false;
                    await invioDatiPrenotazione();
                    IsEnabled = true;
                });
            }
        }
     
        //Comando usato per tornare alla home page dell'applicativo
        public ICommand TornaAllaHome
        {
            get
            {
                return new Command(async () =>
                {
                    IsEnabled = false;
                    App.Current.MainPage = new NavigationPage(new MenuPrincipale());
                    IsEnabled = true;
                });
            }
        }

        //proprietà riferita al campo IsEnabled

        public bool IsEnabled
        {
            get { return isenabled; }
            set
            {
                OnPropertyChanged();
                isenabled = value;
            }
        }

        //Comando usato per richiamare la funzione per cambiare data
        public ICommand cambiaData
        {
            get
            {
                return new Command(async (e) =>
                {
                    if ((impegnativa.classePriorita ==  "P") || (string.IsNullOrEmpty(impegnativa.classePriorita)))
                    {
                        IsEnabled = false;
                        propostaRichiesta.visualizzaDatePicker();
                    }
                    else
                    {
                        var responseDisplayAlert = await App.Current.MainPage.DisplayAlert("Attenzione",
                            "Sei sicuro di voler cambiare? Se confermi perderai la priorità assegnata dal tuo medico curante",
                            "SI", "NO");
                        if (responseDisplayAlert)
                        {
                            IsEnabled = false;
                            propostaRichiesta.visualizzaDatePicker();
                        }
                    }

                });
            }
        }


        //Comando usato per richiamare la funzione per cambiare ora
        public ICommand cambiaOra
        {
            get
            {
                return new Command(async () =>
                {
                    if ((impegnativa.classePriorita == "P") || (string.IsNullOrEmpty(impegnativa.classePriorita)))
                    {
                        IsEnabled = false;
                        IsBusyV = true;
                        IsBusyV = false;
                        await info(appuntamentoProposto);
                        Device.StartTimer(TimeSpan.FromSeconds(3), () =>
                        {
                            IsEnabled = true;
                            return false;
                        });
                    }
                    else
                    {

                        var responseDisplayAlert = await App.Current.MainPage.DisplayAlert("Attenzione",
                            "Sei sicuro di voler cambiare? Se confermi perderai la priorità assegnata dal tuo medico curante",
                            "SI", "NO");
                        if (responseDisplayAlert)
                        {
                            IsEnabled = false;
                            IsBusyV = true;
                            IsBusyV = false;
                            await info(appuntamentoProposto);
                            Device.StartTimer(TimeSpan.FromSeconds(3), () =>
                            {
                                IsEnabled = true;
                                return false;
                            });
                        }
                    }

                });
            }
        }



        //Proprietà riferita al campo IsVisible
        public bool IsVisible
        {
            get { return isvisible; }
            set
            {
                OnPropertyChanged();
                isvisible = value;
            }
        }
        //Proprietà riferita al campo IsBusy
        public bool IsBusy
        {
            get { return isbusy; }
            set
            {
                OnPropertyChanged();
                isbusy = value;
            }
        }

        //Proprietà riferita al campo IsbusyV
        public bool IsBusyV
        {
            get { return isbusyV; }
            set
            {
                OnPropertyChanged();
                isbusyV = value;
            }
        }
        //Proprietà riferita al campo IsVisibleButton
        public bool IsVisibleButton
        {
            get { return isvisibleButton; }
            set
            {
                OnPropertyChanged();
                isvisibleButton = value;
            }
        }

        #endregion

        #region Costruttore

        public PropostaRichiestaModelView(Impegnativa ricetta, List<Prestazione> prestazioni, Assistito contatto, PropostaRichiesta proposta)
        {
           
            this.impegnativa = ricetta;
            ricetta.assistito = contatto;
            propostaRichiesta = proposta;
            this.contatto = contatto;
            IsVisibleButton = false;
            IsVisible = true;
            IsBusy = true;
            this.prestazioni = prestazioni;
            headers.Add(new Header("struttura", "150907"));
            headers.Add(new Header("dataRicerca", ""));
            headers.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
            recuperoInformazioni();
            AnnullaPrenotazione = new Command(async () =>
            {

                var responseDisplayAlert = await App.Current.MainPage.DisplayAlert("Attenzione", "Sei sicuro di voler annullare la prenotazione?", "si",
                    "no");
                if (responseDisplayAlert)
                {
                    REST<object, string> connessioneAnnullamento = new REST<object, string>();
                    string messaggioDiAnnullamento = await connessioneAnnullamento.getString(SingletonURL.Instance.getRotte().annullaPrenotazioneSospesa,
                        headers);
                    if (connessioneAnnullamento.responseMessage != HttpStatusCode.OK)
                    {
                        await MessaggioConnessione.displayAlert((int)connessioneAnnullamento.responseMessage, messaggioDiAnnullamento);
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Attenzione", messaggioDiAnnullamento, "ok");
                        App.Current.MainPage = new MenuPrincipale();
                    }
                
                }
            });
        }


        #endregion

        #region Metodi

        //Metodo usato per richiamare un altro metodo di nome info, nel quale, tramite una connessione e l'invio di una lista di prestazioni, il servizio ci restituirà gli appuntamenti proposti
        private async void recuperoInformazioni()
        {
            IsEnabled = false;
            await info();
            IsEnabled = true;
        }

        //Metodo che utilizzeremo per inviare i dati dell'assistito, della ricetta al server 
        public async Task invioDatiPrenotazione()
        {
            var noteAccettate = true;
            string note = "Prima di continuare prendi visione di tutte le note: " + "\n\n";
            bool flag = false;


            for (int i = 0; i < appuntamentoProposto.appuntamenti.Count; i++)
            {
                if (appuntamentoProposto.appuntamenti[i].esitoNote != true && !string.IsNullOrEmpty(appuntamentoProposto.appuntamenti[i].nota))
                {
                    noteAccettate = false;
                    note += appuntamentoProposto.appuntamenti[i].desprest + "\n\n";
                    appuntamentoProposto.appuntamenti[i].coloreNote=Color.Red;
                    appuntamentoProposto.appuntamenti[i].coloreTestoNote = Color.White;
                    flag = true;
                }
            }
            if(flag)
                await App.Current.MainPage.DisplayAlert("Attenzione", note, "ok");

            for (int i = 0; i < appuntamentoProposto.appuntamenti.Count; i++)
            {
                if (!string.IsNullOrEmpty(appuntamentoProposto.appuntamenti[i].nota))
                {
                    note = "Note presenti";
                }

            }
            if (string.IsNullOrEmpty(note)||noteAccettate)
                noteAccettate = await App.Current.MainPage.DisplayAlert("Conferma Prenotazione", "Sicuro di voler confermare la prenotazione?", "SI", "NO");
          
            if (noteAccettate)
            {
                REST<AppuntamentoProposto, AppuntamentiConfermati> invioDati = new REST<AppuntamentoProposto, AppuntamentiConfermati>();
                appuntamentoProposto.assistito = contatto;
                appuntamentoProposto.codiceImpegnativa = impegnativa.nre;
                appuntamentoProposto.classePriorita = impegnativa.classePriorita;
                appuntamentoProposto.dataEmissioneRicetta = impegnativa.dataEmissioneRicetta;
                try
                {
                    IsBusyV = true;
                    AppuntamentiConfermati appuntamentiConfermati = await invioDati.PostJson(SingletonURL.Instance.getRotte().ConfermaPrenotazione, appuntamentoProposto, headers);
                    if ((invioDati.responseMessage != HttpStatusCode.Created) && (invioDati.responseMessage != HttpStatusCode.InternalServerError) && (invioDati.responseMessage != HttpStatusCode.NotFound))
                    {
                        await App.Current.MainPage.DisplayAlert("Attenzione " + (int)invioDati.responseMessage, invioDati.warning, "OK");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Appuntamento Confermato", appuntamentiConfermati.messaggio, "ok");
                        IsBusyV = false;
                        Visible = "false";
                        VisibleHome = "true";
                        if (appuntamentiConfermati.esito != 0)
                            App.Current.MainPage = new MenuPrincipale();
                    }

                }
                catch (Exception)
                {
                    await App.Current.MainPage.DisplayAlert("Attenzione", "connessione non riuscita", "ok");
                }
            }
        }

        private async Task info()
        {
            string messaggio = "";
            REST<List<Prestazione>, AppuntamentoProposto> recuperoDatiLista = new REST<List<Prestazione>, AppuntamentoProposto>();
            appuntamentoProposto = await recuperoDatiLista.PostJson(SingletonURL.Instance.getRotte().PrimaDisponibilita, prestazioni, headers);
            if (recuperoDatiLista.responseMessage != HttpStatusCode.OK)
            {
                await MessaggioConnessione.displayAlert((int)recuperoDatiLista.responseMessage, recuperoDatiLista.warning);
            }
            else
            {
                for (int i = 0; i < appuntamentoProposto.appuntamenti.Count; i++)
                {

                   
                    if (appuntamentoProposto.appuntamenti[i].disponibile == false)
                    {
                        messaggio = messaggio + appuntamentoProposto.appuntamenti[i].desprest + '\n';
                        appuntamentoProposto.appuntamenti.RemoveAt(i);
                        prestazioni.RemoveAt(i);
                        i -= 1;
                        count++;
                    }
                }
                if (count > 0)
                    await App.Current.MainPage.DisplayAlert("Attenzione",
                        "Le seguenti prestazioni non sono momentaneamente disponibili: " + "\n" + messaggio, "OK");
                IsBusy = false;
                if (appuntamentoProposto.appuntamenti.Count > 0)
                {

                    for (int j = 0; j < appuntamentoProposto.appuntamenti.Count; j++)
                    {
                        DateTime data_appuntamento = DateTime.ParseExact(appuntamentoProposto.appuntamenti[j].dataAppuntamento, "dd/MM/yyyy", null);

                        var culture = new System.Globalization.CultureInfo("it-IT");
                        var day = culture.DateTimeFormat.GetDayName(data_appuntamento.DayOfWeek);
                        appuntamentoProposto.appuntamenti[j].dataAppuntamento = day.ToString() + " " + appuntamentoProposto.appuntamenti[j].dataAppuntamento;

                        if (appuntamentoProposto.appuntamenti[j].nota.Trim() == string.Empty)
                        {
                            appuntamentoProposto.appuntamenti[j].esitoNote = true;
                            appuntamentoProposto.appuntamenti[j].visibleNote = false;
                        }
                    }
                    for (int k = 0; k < appuntamentoProposto.appuntamenti.Count; k++)
                    {
                        appuntamentoProposto.appuntamenti[k].desprest = prestazioni[k].reparti[0].desprest;
                        if (appuntamentoProposto.appuntamenti[k].dataAppuntamento.Length < 10)
                        {
                            DateTime dataTemp;
                            if (Regex.IsMatch(appuntamentoProposto.appuntamenti[k].dataAppuntamento, @"^\d{1}/\d{1}/\d{4}"))
                            {
                                dataTemp = new DateTime(int.Parse(appuntamentoProposto.appuntamenti[k].dataAppuntamento.Substring(4)), int.Parse(appuntamentoProposto.appuntamenti[k].dataAppuntamento.Substring(2, 1)), int.Parse(appuntamentoProposto.appuntamenti[k].dataAppuntamento.Substring(0, 1)));
                            }
                            else if (Regex.IsMatch(appuntamentoProposto.appuntamenti[k].dataAppuntamento, @"^\d{2}/\d{1}/\d{4}"))
                            {
                                dataTemp = new DateTime(int.Parse(appuntamentoProposto.appuntamenti[k].dataAppuntamento.Substring(5)), int.Parse(appuntamentoProposto.appuntamenti[k].dataAppuntamento.Substring(3, 1)), int.Parse(appuntamentoProposto.appuntamenti[k].dataAppuntamento.Substring(0, 2)));
                            }
                            else
                            {
                                dataTemp = new DateTime(int.Parse(appuntamentoProposto.appuntamenti[k].dataAppuntamento.Substring(5)), int.Parse(appuntamentoProposto.appuntamenti[k].dataAppuntamento.Substring(2, 2)), int.Parse(appuntamentoProposto.appuntamenti[k].dataAppuntamento.Substring(0, 1)));
                            }
                            var dataAppoggio = String.Format("{0:dd/MM/yyyy}", dataTemp);
                            appuntamentoProposto.appuntamenti[k].dataAppuntamento = dataAppoggio + dataTemp.DayOfWeek.ToString();
                            
                        }
                        
                        if (string.IsNullOrEmpty(appuntamentoProposto.appuntamenti[k].reparti[0].nomeMedico))
                            appuntamentoProposto.appuntamenti[k].reparti[0].nomeMedico = "N/D";
                    }
                    ListPrenotazioni = appuntamentoProposto.appuntamenti;
                  /*  if ((appuntamentoProposto.classePriorita == "B") || (appuntamentoProposto.classePriorita == "U"))
                    {
                        messaggio = "Le seguenti prestazioni superano la priorità assegnata dal medico curante:\n";
                        for (int j = 0; j < appuntamentoProposto.appuntamenti.Count; j++)
                        {
                            var dataAppProposto = DateTime.ParseExact(appuntamentoProposto.appuntamenti[j].dataAppuntamento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            var dataEmissione = DateTime.ParseExact(appuntamentoProposto.dataEmissioneRicetta, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            if (((appuntamentoProposto.classePriorita == "U") && ((dataAppProposto - dataEmissione).TotalDays > 3)) || ((appuntamentoProposto.classePriorita == "B") && ((dataAppProposto - dataEmissione).TotalDays > 10)))
                            {
                                messaggio += appuntamentoProposto.appuntamenti[j].desprest+"\n";
                            }
                        }
                        var displayAlertUrgente = await App.Current.MainPage.DisplayAlert("Attenzione",messaggio,"SI", "NO");
                        if (!displayAlertUrgente)
                        {
                            REST<object, string> connessioneAnnullamento = new REST<object, string>();
                            string messaggioDiAnnullamento = await connessioneAnnullamento.getString(SingletonURL.Instance.getRotte().annullaPrenotazioneSospesa,
                                headers);
                            if (connessioneAnnullamento.responseMessage != HttpStatusCode.OK)
                            {
                                await App.Current.MainPage.DisplayAlert("Attenzione " + (int)connessioneAnnullamento.responseMessage, messaggioDiAnnullamento, "OK");
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert("Attenzione", messaggioDiAnnullamento, "ok");
                                App.Current.MainPage = new MenuPrincipale();
                            }
                        }
                    }*/
                }
                else
                {
                    ListPrenotazioni = new List<AppuntamentoPrestazioneProposto>();
                    await App.Current.MainPage.DisplayAlert("Attenzione","Nessuna prestazione è disponibile presso la struttura","OK");
                    App.Current.MainPage = new NavigationPage(new MenuPrincipale());
                }
                IsVisible = false;
                IsVisibleButton = true;
            }

        }

        private async Task info(AppuntamentoProposto prenotazione)
        {
            REST<AppuntamentoProposto, AppuntamentoProposto> connessione = new REST<AppuntamentoProposto, AppuntamentoProposto>();
            IsBusyV = true;

            foreach(var i in prenotazione.appuntamenti)
            {
                i.dataAppuntamento = i.dataAppuntamento.Trim(alfabeto);
            }
            
            appuntamentoProposto = await connessione.PostJson(SingletonURL.Instance.getRotte().PrimaDisponibilitaOra, prenotazione, headers);
            if (connessione.responseMessage != HttpStatusCode.OK)
            {
                await MessaggioConnessione.displayAlert((int)connessione.responseMessage, connessione.warning);
            }
            else
            {
                count = 0;
                messaggio = "";


                for (int i = 0; i < appuntamentoProposto.appuntamenti.Count; i++)
                {

                    if (appuntamentoProposto.appuntamenti[i].disponibile == false)
                    {
                        messaggio = messaggio + appuntamentoProposto.appuntamenti[i].desprest + '\n';
                        appuntamentoProposto.appuntamenti.RemoveAt(i);
                        prestazioni.RemoveAt(i);
                        i -= 1;
                        count++;
                    }
                }
                if (count > 0)
                    await App.Current.MainPage.DisplayAlert("Attenzione",
                        "Le seguenti prestazioni non sono momentaneamente disponibili: " + "\n" + messaggio, "OK");
                IsBusyV = false;
                if (appuntamentoProposto.appuntamenti.Count > 0)
                {

                    for (int j = 0; j < appuntamentoProposto.appuntamenti.Count; j++)
                    {

                        DateTime data_appuntamento = DateTime.ParseExact(appuntamentoProposto.appuntamenti[j].dataAppuntamento, "dd/MM/yyyy", null);

                        var culture = new System.Globalization.CultureInfo("it-IT");
                        var day = culture.DateTimeFormat.GetDayName(data_appuntamento.DayOfWeek);
                        appuntamentoProposto.appuntamenti[j].dataAppuntamento = day.ToString() + " " + appuntamentoProposto.appuntamenti[j].dataAppuntamento;
                        appuntamentoProposto.appuntamenti[j].desprest = prestazioni[j].reparti[0].desprest;
                        if (appuntamentoProposto.appuntamenti[j].nota.Trim() == string.Empty)
                        {
                            appuntamentoProposto.appuntamenti[j].esitoNote = true;
                            appuntamentoProposto.appuntamenti[j].visibleNote = false;
                        }
                    }
                    for (int k = 0; k < appuntamentoProposto.appuntamenti.Count; k++)
                    {
                        if (appuntamentoProposto.appuntamenti[k].dataAppuntamento.Length < 10)
                        {
                            DateTime dataTemp;
                            if (Regex.IsMatch(appuntamentoProposto.appuntamenti[k].dataAppuntamento, @"^\d{1}/\d{1}/\d{4}"))
                            {
                                dataTemp = new DateTime(int.Parse(appuntamentoProposto.appuntamenti[k].dataAppuntamento.Substring(4)), int.Parse(appuntamentoProposto.appuntamenti[k].dataAppuntamento.Substring(2, 1)), int.Parse(appuntamentoProposto.appuntamenti[k].dataAppuntamento.Substring(0, 1)));
                            }
                            else if (Regex.IsMatch(appuntamentoProposto.appuntamenti[k].dataAppuntamento, @"^\d{2}/\d{1}/\d{4}"))
                            {
                                dataTemp = new DateTime(int.Parse(appuntamentoProposto.appuntamenti[k].dataAppuntamento.Substring(5)), int.Parse(appuntamentoProposto.appuntamenti[k].dataAppuntamento.Substring(3, 1)), int.Parse(appuntamentoProposto.appuntamenti[k].dataAppuntamento.Substring(0, 2)));
                            }
                            else
                            {
                                dataTemp = new DateTime(int.Parse(appuntamentoProposto.appuntamenti[k].dataAppuntamento.Substring(5)), int.Parse(appuntamentoProposto.appuntamenti[k].dataAppuntamento.Substring(2, 2)), int.Parse(appuntamentoProposto.appuntamenti[k].dataAppuntamento.Substring(0, 1)));
                            }
                            appuntamentoProposto.appuntamenti[k].dataAppuntamento = String.Format("{0:dd/MM/yyyy}", dataTemp) + dataTemp.DayOfWeek;
                        }
                        if (string.IsNullOrEmpty(appuntamentoProposto.appuntamenti[k].reparti[0].nomeMedico))
                            appuntamentoProposto.appuntamenti[k].reparti[0].nomeMedico = "N/D";
                        ListPrenotazioni = appuntamentoProposto.appuntamenti;
                        /*   if ((appuntamentoProposto.classePriorita == "B") || (appuntamentoProposto.classePriorita == "U"))
                           {
                               messaggio = "Le seguenti prestazioni superano la priorità assegnata dal medico curante:\n";
                               for (int j = 0; j < appuntamentoProposto.appuntamenti.Count; j++)
                               {
                                   var dataAppProposto = DateTime.ParseExact(appuntamentoProposto.appuntamenti[j].dataAppuntamento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                   var dataEmissione = DateTime.ParseExact(appuntamentoProposto.dataEmissioneRicetta, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                   if (((appuntamentoProposto.classePriorita == "U") && ((dataAppProposto - dataEmissione).TotalDays > 3)) || ((appuntamentoProposto.classePriorita == "B") && ((dataAppProposto - dataEmissione).TotalDays > 10)))
                                   {
                                       messaggio += appuntamentoProposto.appuntamenti[j].desprest + "\n";
                                   }
                               }
                               var displayAlertUrgente = await App.Current.MainPage.DisplayAlert("Attenzione", messaggio, "SI", "NO");
                               if (!displayAlertUrgente)
                               {
                                   REST<object, string> connessioneAnnullamento = new REST<object, string>();
                                   string messaggioDiAnnullamento = await connessioneAnnullamento.getString(SingletonURL.Instance.getRotte().annullaPrenotazioneSospesa,
                                       headers);
                                   if (connessioneAnnullamento.responseMessage != HttpStatusCode.OK)
                                   {
                                       await App.Current.MainPage.DisplayAlert("Attenzione " + (int)connessioneAnnullamento.responseMessage, messaggioDiAnnullamento, "OK");
                                   }
                                   else
                                   {
                                       await App.Current.MainPage.DisplayAlert("Attenzione", messaggioDiAnnullamento, "ok");
                                       App.Current.MainPage = new MenuPrincipale();
                                   }
                               }
                           }*/
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Attenzione","L'agenda dell'azienda ospedaliera non presenta più date per le prestazioni","OK");
                }
            }

        }

        public async Task infoProssimaData(string data)
        {
            REST<AppuntamentoProposto, AppuntamentoProposto> connessione = new REST<AppuntamentoProposto, AppuntamentoProposto>();
            IsBusyV = true;
            headers[1].value = data;
            foreach (var i in appuntamentoProposto.appuntamenti)
            {
                i.dataAppuntamento = i.dataAppuntamento.Trim(alfabeto);
            }
            appuntamentoProposto = await connessione.PostJson(SingletonURL.Instance.getRotte().ricercadata, appuntamentoProposto, headers);
            if (connessione.responseMessage != HttpStatusCode.OK)
            {
                await MessaggioConnessione.displayAlert((int)connessione.responseMessage, connessione.warning);
            }
            else
            {
                count = 0;
                messaggio = "";
                for (int i = 0; i < appuntamentoProposto.appuntamenti.Count; i++)
                {
                    if (appuntamentoProposto.appuntamenti[i].disponibile == false)
                    {
                        messaggio = messaggio + appuntamentoProposto.appuntamenti[i].desprest + '\n';
                        appuntamentoProposto.appuntamenti.RemoveAt(i);
                        prestazioni.RemoveAt(i);
                        i -= 1;
                        count++;
                    }
                }
                if (count > 0)
                    await App.Current.MainPage.DisplayAlert("Attenzione",
                        "Le seguenti prestazioni non sono momentaneamente disponibili: " + "\n" + messaggio, "OK");
                IsBusyV = false;
                if (appuntamentoProposto.appuntamenti.Count > 0)
                { 

                    for (int k = 0; k < appuntamentoProposto.appuntamenti.Count;k++)
                    {

                        DateTime data_appuntamento = DateTime.ParseExact(appuntamentoProposto.appuntamenti[k].dataAppuntamento, "dd/MM/yyyy", null);

                        var culture = new System.Globalization.CultureInfo("it-IT");
                        var day = culture.DateTimeFormat.GetDayName(data_appuntamento.DayOfWeek);
                        appuntamentoProposto.appuntamenti[k].dataAppuntamento = day.ToString() + " " + appuntamentoProposto.appuntamenti[k].dataAppuntamento;
                        appuntamentoProposto.appuntamenti[k].desprest = prestazioni[k].reparti[0].desprest;
                        if (appuntamentoProposto.appuntamenti[k].nota.Trim() == string.Empty)
                        {
                            appuntamentoProposto.appuntamenti[k].esitoNote = true;
                            appuntamentoProposto.appuntamenti[k].visibleNote = false;
                        }
                    }
                    for (int j = 0; j < appuntamentoProposto.appuntamenti.Count; j++)
                    {
                        if (appuntamentoProposto.appuntamenti[j].dataAppuntamento.Length < 10)
                        {
                            DateTime dataTemp;
                            if (Regex.IsMatch(appuntamentoProposto.appuntamenti[j].dataAppuntamento, @"^\d{1}/\d{1}/\d{4}"))
                            {
                                dataTemp = new DateTime(int.Parse(appuntamentoProposto.appuntamenti[j].dataAppuntamento.Substring(4)), int.Parse(appuntamentoProposto.appuntamenti[j].dataAppuntamento.Substring(2, 1)), int.Parse(appuntamentoProposto.appuntamenti[j].dataAppuntamento.Substring(0, 1)));
                            }
                            else if (Regex.IsMatch(appuntamentoProposto.appuntamenti[j].dataAppuntamento, @"^\d{2}/\d{1}/\d{4}"))
                            {
                                dataTemp = new DateTime(int.Parse(appuntamentoProposto.appuntamenti[j].dataAppuntamento.Substring(5)), int.Parse(appuntamentoProposto.appuntamenti[j].dataAppuntamento.Substring(3, 1)), int.Parse(appuntamentoProposto.appuntamenti[j].dataAppuntamento.Substring(0, 2)));
                            }
                            else
                            {
                                dataTemp = new DateTime(int.Parse(appuntamentoProposto.appuntamenti[j].dataAppuntamento.Substring(5)), int.Parse(appuntamentoProposto.appuntamenti[j].dataAppuntamento.Substring(2, 2)), int.Parse(appuntamentoProposto.appuntamenti[j].dataAppuntamento.Substring(0, 1)));
                            }
                            appuntamentoProposto.appuntamenti[j].dataAppuntamento = String.Format("{0:dd/MM/yyyy}", dataTemp) + dataTemp.DayOfWeek;
                        }
                        if (string.IsNullOrEmpty(appuntamentoProposto.appuntamenti[j].reparti[0].nomeMedico))
                            appuntamentoProposto.appuntamenti[j].reparti[0].nomeMedico = "N/D";
                    }
                    ListPrenotazioni = appuntamentoProposto.appuntamenti;
                    /*if ((appuntamentoProposto.classePriorita == "B") || (appuntamentoProposto.classePriorita == "U"))
                    {
                        messaggio = "Le seguenti prestazioni superano la priorità assegnata dal medico curante:\n";
                        for (int j = 0; j < appuntamentoProposto.appuntamenti.Count; j++)
                        {
                            var dataAppProposto = DateTime.ParseExact(appuntamentoProposto.appuntamenti[j].dataAppuntamento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            var dataEmissione = DateTime.ParseExact(appuntamentoProposto.dataEmissioneRicetta, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            if (((appuntamentoProposto.classePriorita == "U") && ((dataAppProposto - dataEmissione).TotalDays > 3)) || ((appuntamentoProposto.classePriorita == "B") && ((dataAppProposto - dataEmissione).TotalDays > 10)))
                            {
                                messaggio += appuntamentoProposto.appuntamenti[j].desprest + "\n";
                            }
                        }
                        var displayAlertUrgente = await App.Current.MainPage.DisplayAlert("Attenzione", messaggio, "SI", "NO");
                        if (!displayAlertUrgente)
                        {
                            REST<object, string> connessioneAnnullamento = new REST<object, string>();
                            string messaggioDiAnnullamento = await connessioneAnnullamento.getString(SingletonURL.Instance.getRotte().annullaPrenotazioneSospesa,
                                headers);
                            if (connessioneAnnullamento.responseMessage != HttpStatusCode.OK)
                            {
                                await App.Current.MainPage.DisplayAlert("Attenzione " + (int)connessioneAnnullamento.responseMessage, messaggioDiAnnullamento, "OK");
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert("Attenzione", messaggioDiAnnullamento, "ok");
                                App.Current.MainPage = new MenuPrincipale();
                            }
                        }
                    }*/
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Attenzione", "L'agenda dell'azienda ospedaliera non presenta più date per le prestazioni", "OK");
                }
            }
        }

        #endregion

    }
}
