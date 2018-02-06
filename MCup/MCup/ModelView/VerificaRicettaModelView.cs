using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MCup.Annotations;
using MCup.Model;
using MCup.Service;
using MCup.Views;
using Xamarin.Forms;

namespace MCup.ModelView
{
    public class VerificaRicettaModelView : INotifyPropertyChanged
    {
        private List<Prestazione> listaPrestazioni = new List<Prestazione>();
        private bool isBusy;
        private string nomeAssistito, cognomeAssistito, codiceRicetta;
        private Impegnativa ricetta;
        private List<Prestazione> prestazioni; //Lista delle prestazioni contenute nella ricetta
        private List<Prestazione> prestazioniErogabili;
        private bool buttonIsVisible;
        private List<Reparto> reparto = new List<Reparto>();
        private VerificaRicetta verifica;
        public ICommand ContinuaPrenotazione { protected set; get; }
        private List<Prestazione> prestazioniDaInviare;
        private bool isenabled;

        public bool ButtonIsVisible
        {
            get { return buttonIsVisible; }
            set
            {
                OnPropertyChanged();
                buttonIsVisible = value;
            }
        }

        public bool IsEnabled
        {
            get { return isenabled; }
            set
            {
                OnPropertyChanged();
                isenabled = value;
            }
        }

        public string NomeAssistito
        {
            get { return nomeAssistito; }
            set
            {
                OnPropertyChanged();
                nomeAssistito = value;
            }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                OnPropertyChanged();
                isBusy = value;
            }

        }
        public string CognomeAssistito
        {
            get { return cognomeAssistito; }
            set
            {
                OnPropertyChanged();
                cognomeAssistito = value;
            }
        }

        public string CodiceRicetta
        {
            get { return codiceRicetta; }
            set
            {
                OnPropertyChanged();
                codiceRicetta = value;
            }
        }

        public VerificaRicettaModelView(Impegnativa impegnativa, VerificaRicetta verifica, Assistito contatto)
        {
            IsEnabled = true;
            this.verifica = verifica;
            ricetta = impegnativa;
            NomeAssistito = contatto.nome;
            CognomeAssistito = contatto.cognome;
            CodiceRicetta = ricetta.nre;
            ButtonIsVisible = true;
            prestazioniDaInviare = new List<Prestazione>();
            ingressoPagina();
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
                    await this.verifica.Navigation.PushAsync(new PropostaRichiesta(prestazioniDaInviare, contatto));
                else
                    await App.Current.MainPage.DisplayAlert("Attenzione", "Seleziona un reparto per ogni prestazione",
                        "OK");
                IsEnabled = true;
            });
        }

        public List<Prestazione> ListaPrestazioni
        {
            get { return listaPrestazioni; }
            set
            {
                OnPropertyChanged();
                listaPrestazioni = new List<Prestazione>(value);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task ricezioneReparti()
        {
            List<Prestazione> temp = ListaPrestazioni;
            REST<Prestazione, Reparto> connessione = new REST<Prestazione, Reparto>();
            List<Header> headers = new List<Header>();
            headers.Add(new Header("struttura", "030001"));
            for (var i = 0; i < temp.Count; i++)
            {
                IsBusy = true;
                IsEnabled = false;
                temp[i].reparti = await connessione.PostJsonList(URL.RicercadisponibilitaReparti, ListaPrestazioni[i], headers);
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

        private async void ingressoPagina()
        {
            REST<Impegnativa, List<Prestazione>> connessione = new REST<Impegnativa, List<Prestazione>>();
            List<Header> headers = new List<Header>();
            headers.Add(new Header("struttura", "030001"));
            prestazioniErogabili = await connessione.PostJson(URL.StruttureErogatrici, ricetta, headers);
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
                        if (prestazioniErogabili[j].codprest == prestazioniNonErogabili[t].codprest)
                        {
                            messaggio = messaggio + prestazioniNonErogabili[t].desprest + "\n";
                            prestazioniErogabili.RemoveAt(j);
                            break;
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
                        await App.Current.MainPage.DisplayAlert("Attenzione", "Alcune prestazioni vengono erogate da più reparti, se non si conosce il reparto per cui prenotare chiamare il Call Center", "OK");
                }
                else
                {
                    ButtonIsVisible = false;
                    await App.Current.MainPage.DisplayAlert("Attenzione",
                        "La struttura non eroga nessuna prestazione contenuta nella ricetta", "OK");
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
                    await App.Current.MainPage.DisplayAlert("Attenzione", "Alcune prestazioni vengono erogate da più reparti, se non si conosce il reparto per cui prenotare chiamare il Call Center", "OK");
            }
        }

        public void selectedReparto(Reparto reparto)
        {
            for (var i = 0; i < prestazioniDaInviare.Count; i++)
                if (prestazioniDaInviare[i].codprest == reparto.codprest)
                    for (int j = 0; j < prestazioniDaInviare[i].reparti.Count; j++)
                    {
                        if(prestazioniDaInviare[i].reparti[j].codReparto==reparto.codReparto)
                            prestazioniDaInviare[i].reparti[j].repartoScelto = true;
                        else
                            prestazioniDaInviare[i].reparti[j].repartoScelto = false;
                    }
        }
    }

}

