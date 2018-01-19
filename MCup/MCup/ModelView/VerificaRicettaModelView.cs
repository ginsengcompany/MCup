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
        private List<PrestazioniTemp> listaPrestazioni = new List<PrestazioniTemp>();
        private string nomeAssistito, cognomeAssistito, codiceRicetta;
        private Ricetta ricetta;
        private List<PrestazioniTemp> prestazioni; //Lista delle prestazioni contenute nella ricetta
        private List<PrestazioniTemp> prestazioniErogabili;
        private bool buttonIsVisible;
        private List<Reparto> reparto = new List<Reparto>();
        private VerificaRicetta verifica;
        public ICommand ContinuaPrenotazione { protected set; get; }
        private List<Prestazioni> prestazioniDaInviare;

        public bool ButtonIsVisible
        {
            get { return buttonIsVisible; }
            set
            {
                OnPropertyChanged();
                buttonIsVisible = value;
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

        public VerificaRicettaModelView(Ricetta impegnativa, VerificaRicetta verifica)
        {
            this.verifica = verifica;
            ricetta = impegnativa;
            NomeAssistito = ricetta.nome_assistito;
            CognomeAssistito = ricetta.cognome_assistito;
            CodiceRicetta = ricetta.codice_nre;
            ButtonIsVisible = true;
            prestazioniDaInviare = new List<Prestazioni>();
            ingressoPagina();
            ContinuaPrenotazione = new Command(async () =>
            {
                bool verificaPrestazioni = true;
                foreach (var i in prestazioniDaInviare)
                    if (i.reparti == null)
                        verificaPrestazioni = false;
                if (verificaPrestazioni == true)
                    await this.verifica.Navigation.PushAsync(new PropostaRichiesta(prestazioniDaInviare));
                else
                    await App.Current.MainPage.DisplayAlert("Attenzione", "Seleziona un reparto per ogni prestazione",
                        "OK");
            });
        }

        public List<PrestazioniTemp> ListaPrestazioni
        {
            get { return listaPrestazioni; }
            set
            {
                OnPropertyChanged();
                listaPrestazioni = new List<PrestazioniTemp>(value);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task ricezioneReparti(DateTime data)
        {
            List<PrestazioniTemp> temp = ListaPrestazioni;
            REST<PrestazioniTemp,Reparto> connessione = new REST<PrestazioniTemp,Reparto>();
            for (var i = 0; i < temp.Count; i++)
            {
                string dataSub = data.ToString().Substring(0, 10);
                ListaPrestazioni[i].data_inizio = dataSub;
                prestazioniDaInviare[i].data_inizio = dataSub;
                temp[i].reparti = await connessione.PostJsonList(URL.RicercadisponibilitaReparti, ListaPrestazioni[i]);
            }
            ListaPrestazioni = temp;
        }

        private async void ingressoPagina()
        {
            REST<Erogazione, PrestazioniTemp> connessione = new REST<Erogazione, PrestazioniTemp>();
            Erogazione erogazioneDaInviare = new Erogazione();
            erogazioneDaInviare.prestazioni = ricetta.prestazioni;
            prestazioniErogabili = await connessione.PostJsonList(URL.StruttureErogatrici, erogazioneDaInviare);
            List<PrestazioniTemp> prestazioniNonErogabili = new List<PrestazioniTemp>();
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
                    prestazioniDaInviare.Add(prestazioniErogabili[i].estraiPrestazione());
                if (prestazioniErogabili.Count > 0)
                {
                    await App.Current.MainPage.DisplayAlert("Attenzione",
                        "La struttura non eroga i seguenti servizi: " + "\n" + messaggio, "OK");
                    await ricezioneReparti(DateTime.Today);
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
                    prestazioniDaInviare.Add(prestazioniErogabili[i].estraiPrestazione());
                await ricezioneReparti(DateTime.Today);
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
                    prestazioniDaInviare[i].reparti = reparto;
        }

        public class PrestazioniTemp
        {
            public string codprest { get; set; }
            public string desbprest { get; set; }
            public string desprest { get; set; }
            public string data_inizio { get; set; }
            public List<Reparto> reparti { get; set; }
            public bool erogabile { get; set; }
            public string struttura { get; set; } = "030001";

            public PrestazioniTemp() { }

            public PrestazioniTemp(PrestazioniTemp prestazioni)
            {
                this.codprest = prestazioni.codprest;
                this.desbprest = prestazioni.desbprest;
                this.desprest = prestazioni.desprest;
                this.reparti = prestazioni.reparti;
                this.data_inizio = prestazioni.data_inizio;
            }

            public Prestazioni estraiPrestazione()
            {
                Prestazioni prestazioni = new Prestazioni(this.codprest,this.desbprest,this.desprest,this.erogabile);
                return prestazioni;
            }

        }

    }
}
