using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MCup.Annotations;
using MCup.Model;
using MCup.Service;

namespace MCup.ModelView
{
  public  class VerificaRicettaModelView : INotifyPropertyChanged
    {
        private List<Prestazioni> listaPrestazioni = new List<Prestazioni>();
        private string nomeAssistito, cognomeAssistito, codiceRicetta;
        private Ricetta ricetta;
        private List<Prestazioni> prestazioni; //Lista delle prestazioni contenute nella ricetta
        private Erogazione eroga;
        private List<Prestazioni> prestazioniErogabili;
        private bool buttonIsVisible;

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

        public VerificaRicettaModelView(Ricetta impegnativa)
        {
            ricetta = impegnativa;
            NomeAssistito = ricetta.nome_assistito;
            CognomeAssistito = ricetta.cognome_assistito;
            CodiceRicetta = ricetta.codice_nre;
            ButtonIsVisible = true;
            ingressoPagina();
        }

        public List<Prestazioni> ListaPrestazioni
        {
            get { return listaPrestazioni; }
            set
            {
                OnPropertyChanged();
                listaPrestazioni = new List<Prestazioni>(value);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void ingressoPagina()
        {
            REST<Erogazione, Prestazioni> connessione = new REST<Erogazione, Prestazioni>();
            Erogazione erogazioneDaInviare = new Erogazione();
            erogazioneDaInviare.prestazioni = ricetta.prestazioni;
            prestazioniErogabili = await connessione.PostJsonList(URL.StruttureErogatrici, erogazioneDaInviare);
            List<Prestazioni> prestazioniNonErogabili = new List<Prestazioni>();
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
                if (prestazioniErogabili.Count > 0)
                    await App.Current.MainPage.DisplayAlert("Attenzione",
                        "La struttura non eroga i seguenti servizi: " + "\n" + messaggio, "OK");
                else
                {
                    ButtonIsVisible = false;
                    await App.Current.MainPage.DisplayAlert("Attenzione",
                        "La struttura non eroga nessuna prestazione contenuta nella ricetta", "OK");
                }
            }
            else
                ListaPrestazioni = ricetta.prestazioni;
        }
    }
}
