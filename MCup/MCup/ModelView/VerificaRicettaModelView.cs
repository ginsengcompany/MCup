#region Librerie

using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using MCup.Model;
using MCup.Service;
using MCup.Views;
using Xamarin.Forms;

#endregion
namespace MCup.ModelView
{
    public class VerificaRicettaModelView : INotifyPropertyChanged
    {

        #region DichiarazioneVariabili

        private List<Prestazione> listaPrestazioni = new List<Prestazione>();//Lista utilizzata per le prestazioni
        private bool isBusy;//Variabile booleana utilizzata per l'activity indicator
        private string nomeAssistito, cognomeAssistito, codiceRicetta;//Variabili utilizzate per il nome cognome e il codice ricetta
        private Impegnativa ricetta;//Oggetto che astrae le informazioni dell'impegnativa
        //private List<Prestazione> prestazioni; //Lista delle prestazioni contenute nella ricetta
        private List<Prestazione> prestazioniErogabili;//Lista che tiene conto delle prestazioni non erogabili dalla struttura
        private bool buttonIsVisible;//Variabile utilizzata per rendere visibile o meno il button
        private VerificaRicetta verifica; //oggetto di tipo Verificaricetta che utilizziamo per richiamare i metodi nella pagina a cui si riferisce il modelview
        private List<Header> headers = new List<Header>();//lista di header
        private List<Prestazione> prestazioniDaInviare;//Prestazioni da inviare
        private bool isenabled;//Variabile Booleana per disabilitare o meno elementi nello xaml
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Proprietà

        public ICommand ContinuaPrenotazione { protected set; get; }//Command che utilizziamo per andare avanti con il procedimento di prenotazioni
        public ICommand AnnullaPrenotazione { protected set; get; }//Comando che utilizziamo  per annullare la prenotazione in sospeso

       


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //Proprietà riferita al campo Visible
        public bool ButtonIsVisible
        {
            get { return buttonIsVisible; }
            set
            {
                OnPropertyChanged();
                buttonIsVisible = value;
            }
        }

        //Proprietà riferita al campo Enabled
        public bool IsEnabled
        {
            get { return isenabled; }
            set
            {
                OnPropertyChanged();
                isenabled = value;
            }
        }

        //Proprietà riferita al campo NomeAssistito
        public string NomeAssistito
        {
            get { return nomeAssistito; }
            set
            {
                OnPropertyChanged();
                nomeAssistito = value;
            }
        }
        //Proprietà riferita al campo iSbusy

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                OnPropertyChanged();
                isBusy = value;
            }

        }
        //Proprietà riferita al campo CognomeAssistito

        public string CognomeAssistito
        {
            get { return cognomeAssistito; }
            set
            {
                OnPropertyChanged();
                cognomeAssistito = value;
            }
        }
        //Proprietà riferita al campo CodiceRicetta

        public string CodiceRicetta
        {
            get { return codiceRicetta; }
            set
            {
                OnPropertyChanged();
                codiceRicetta = value;
            }
        }

        //Proprietà riferita al campo ListaPrestazioni

        public List<Prestazione> ListaPrestazioni
        {
            get { return listaPrestazioni; }
            set
            {
                OnPropertyChanged();
                listaPrestazioni = new List<Prestazione>(value);
            }
        }

        #endregion

        #region Costruttore

        //Costruttore
        public VerificaRicettaModelView(Impegnativa impegnativa, VerificaRicetta verifica, Assistito contatto)
        {
            IsEnabled = true;
            this.verifica = verifica;
            ricetta = impegnativa;
            NomeAssistito = contatto.nome;
            CognomeAssistito = contatto.cognome + " " + contatto.nome;
            CodiceRicetta = ricetta.nre;
            ricetta.assistito = contatto;
            ButtonIsVisible = true;
            prestazioniDaInviare = new List<Prestazione>();
            headers.Add(new Header("struttura", "150907"));
            headers.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
            ingressoPagina();
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
                        await App.Current.MainPage.DisplayAlert("Attenzione " + (int)connessioneAnnullamento.responseMessage, messaggioDiAnnullamento, "OK");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Attenzione", messaggioDiAnnullamento, "ok");
                        App.Current.MainPage = new MenuPrincipale();
                    }
                }
            });
            ContinuaPrenotazione = new Command(async () =>
            {
                IsEnabled = false;
                bool verificaPrestazioni = true;
                for (int i = 0; i < ListaPrestazioni.Count; i++)
                {
                    if (!ListaPrestazioni[i].enabled)
                    {
                        prestazioniDaInviare[i].reparti[0].repartoScelto = true;
                    }

                }
                foreach (var i in prestazioniDaInviare)
                    if (i.reparti == null)
                        verificaPrestazioni = false;
                if (verificaPrestazioni == true)
                {
                    await this.verifica.Navigation.PushAsync(new PropostaRichiesta(impegnativa, prestazioniDaInviare, contatto));
                }
                else
                    await App.Current.MainPage.DisplayAlert("Attenzione", "Seleziona un reparto per ogni prestazione",
                        "OK");
                IsEnabled = true;
            });
        }
        #endregion

        #region Metodi


        //Metodo che tramite una post richiede i reparti al server ed il servizio, dopo aver controllato i dati, ritorna una lista di reparti
        private async Task ricezioneReparti()
        {
            List<Prestazione> temp = ListaPrestazioni;
            REST<Prestazione, Reparto> connessione = new REST<Prestazione, Reparto>();

            for (var i = 0; i < temp.Count; i++)
            {
                IsBusy = true;
                IsEnabled = false;
                temp[i].reparti = await connessione.PostJsonList(SingletonURL.Instance.getRotte().RicercadisponibilitaReparti, ListaPrestazioni[i], headers);
                if (connessione.responseMessage != HttpStatusCode.OK)
                {
                    await App.Current.MainPage.DisplayAlert("Attenzione " + (int)connessione.responseMessage, connessione.warning, "OK");
                }
                else
                {
                    IsEnabled = true;
                    IsBusy = false;
                    for (int p = 0; p < temp[i].reparti.Count; p++)
                    {
                        if (temp[i].reparti.Count == 1)
                            temp[i].reparti[p].defaultReparto = 0;
                        else
                            temp[i].reparti[p].defaultReparto = -1;
                    }
                    if (temp[i].reparti.Count == 1)
                    {
                        temp[i].title = temp[i].reparti[0].descrizione;
                        temp[i].enabled = false;
                    }
                    else
                    {
                        temp[i].title = "Scegli il reparto";
                        temp[i].enabled = true;
                    }

                }
                ListaPrestazioni = temp;
            }

        }

        //Metodo che parte all'ingresso della pagina ed come prima cosa crea una connessione col server e gli passa l'impegnativa cosi da ricevere le prestazioni e i reparti
        private async void ingressoPagina()
        {
            REST<Prestazione, Prestazione> connessione = new REST<Prestazione, Prestazione>();
            List<Header> headers = new List<Header>();
            headers.Add(new Header("struttura", "150907"));
            headers.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
            prestazioniErogabili = new List<Prestazione>();
            for(var i = 0; i < ricetta.prestazioni.Count; i++)
            {
                prestazioniErogabili.Add(await connessione.PostJson(SingletonURL.Instance.getRotte().StruttureErogatrici, ricetta.prestazioni[i], headers));
            }
            if (connessione.responseMessage != HttpStatusCode.OK)
            {
                await App.Current.MainPage.DisplayAlert("Attenzione " + (int)connessione.responseMessage, connessione.warning, "OK");
            }
            else
            {
                List<Prestazione> prestazioniNonErogabili = new List<Prestazione>();
                foreach (var i in prestazioniErogabili)
                {
                    if (!i.erogabile)
                        prestazioniNonErogabili.Add(i);
                }
                if (prestazioniNonErogabili.Count > 0)
                {
                    string messaggio = "";
                    for (int t = 0; t < prestazioniNonErogabili.Count; t++)
                    {
                        for (int j = 0; j < prestazioniErogabili.Count; j++)
                        {
                            if (prestazioniErogabili[j].codregionale == null)
                            {
                                if (prestazioniErogabili[j].codnazionale == prestazioniNonErogabili[t].codnazionale)
                                {
                                    messaggio = messaggio + prestazioniNonErogabili[t].desprest + "\n";
                                    prestazioniErogabili.RemoveAt(j);
                                    break;
                                }
                            }
                            else
                            {
                                if (prestazioniErogabili[j].codregionale == prestazioniNonErogabili[t].codregionale)
                                {
                                    messaggio = messaggio + prestazioniNonErogabili[t].desprest + "\n";
                                    prestazioniErogabili.RemoveAt(j);
                                    break;
                                }
                            }
                        }
                    }
                    ListaPrestazioni = prestazioniErogabili;
                    for (var i = 0; i < prestazioniErogabili.Count; i++)
                        prestazioniDaInviare.Add(prestazioniErogabili[i]);
                    if (prestazioniErogabili.Count > 0)
                    {
                        await App.Current.MainPage.DisplayAlert("Attenzione",
                            "La struttura non eroga i seguenti servizi: " + "\n" + messaggio, "OK");
                        await ricezioneReparti();
                        bool piuReparti = false;
                        foreach (var i in listaPrestazioni)
                            if (i.reparti.Count > 1)
                            {
                                piuReparti = true;
                                break;
                            }
                        if (piuReparti)
                        {
                            REST<object, string> connessioneMessaggioReparti = new REST<object, string>();
                            var risposta = await
                                connessioneMessaggioReparti.getString(SingletonURL.Instance.getRotte().piuReparti, headers);
                            await App.Current.MainPage.DisplayAlert("Attenzione", risposta, "OK");
                        }
                    }
                    else
                    {
                        ButtonIsVisible = false;
                        await App.Current.MainPage.DisplayAlert("Attenzione",
                            "La struttura non eroga nessuna prestazione contenuta nell'impegnativa, la stessa verrà resa di nuovo disponibile a breve", "OK");
                        REST<Impegnativa, string> rEST = new REST<Impegnativa, string>();
                        var response = await rEST.getString(SingletonURL.Instance.getRotte().annullaPrenotazioneSospesa,headers);
                        //await App.Current.MainPage.DisplayAlert("Elaborazione avvenuta",
                          //  rEST.warning, "OK");
                        App.Current.MainPage = new NavigationPage(new MenuPrincipale());
                    }
                }
                else
                {
                    ListaPrestazioni = prestazioniErogabili;
                    for (var i = 0; i < prestazioniErogabili.Count; i++)
                        prestazioniDaInviare.Add(prestazioniErogabili[i]);
                    await ricezioneReparti();
                    bool piuReparti = false;
                    foreach (var i in listaPrestazioni)
                        if (i.reparti.Count > 1)
                        {
                            piuReparti = true;
                            break;
                        }
                    if (piuReparti)
                    {
                        REST<object, string> connessioneMessaggioReparti = new REST<object, string>();
                        var risposta = await
                            connessioneMessaggioReparti.getString(SingletonURL.Instance.getRotte().piuReparti, headers);
                        await App.Current.MainPage.DisplayAlert("Attenzione", risposta, "OK");
                    }
                }
            }

        }

        //Metodo che legge qual è il reparto scelto dall'utente 
        public void selectedReparto(Reparto reparto)
        {
            for (var i = 0; i < prestazioniDaInviare.Count; i++)
                if (prestazioniDaInviare[i].codprest == reparto.codprest)
                    for (int j = 0; j < prestazioniDaInviare[i].reparti.Count; j++)
                    {
                        if (prestazioniDaInviare[i].reparti[j].codReparto == reparto.codReparto)
                            prestazioniDaInviare[i].reparti[j].repartoScelto = true;
                        else
                            prestazioniDaInviare[i].reparti[j].repartoScelto = false;
                    }
        }
        #endregion

    }

}

