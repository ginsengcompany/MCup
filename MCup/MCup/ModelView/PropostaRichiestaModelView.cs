using MCup.Model;
using MCup.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MCup.ModelView
{
    public class PropostaRichiestaModelView : INotifyPropertyChanged
    {
        private List<PrenotazioneProposta> listPrenotazioni;
        private bool isvisible, isbusy,isvisibleButton;
        public ICommand cambiaData
        {
            get
            {
                return new Command(async (e) =>
                {
                    var item = (e as PrenotazioneProposta);
                    await info(item);
                });
            }
        }

        public bool IsVisible
        {
            get { return isvisible; }
            set
            {
                OnPropertyChanged();
                isvisible = value;
            }
        }

        public bool IsBusy
        {
            get { return isbusy; }
            set
            {
                OnPropertyChanged();
                isbusy = value;
            }
        }

        public bool IsVisibleButton
        {
            get { return isvisibleButton; }
            set
            {
                OnPropertyChanged();
                isvisibleButton = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private List<Prestazioni> prestazioni;

        public List<PrenotazioneProposta> ListPrenotazioni
        {
            get { return listPrenotazioni; }
            set
            {
                OnPropertyChanged();
                listPrenotazioni = value;
            }
        }

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public PropostaRichiestaModelView(List<Prestazioni> prestazioni)
        {
            listPrenotazioni = new List<PrenotazioneProposta>();
            IsVisibleButton = false;
            IsVisible = true;
            IsBusy = true;
            this.prestazioni = prestazioni;
            recuperoInformazioni();
        }

        private async void recuperoInformazioni()
        {
            await info();
        }

        private async Task info()
        {
            REST<Prestazioni, PrenotazioneProposta> recuperoDatiLista = new REST<Prestazioni, PrenotazioneProposta>();
            List<PrenotazioneProposta> temp = new List<PrenotazioneProposta>();
            for (int i = 0; i < prestazioni.Count; i++)
            {
                temp.Add(await recuperoDatiLista.PostJson(URL.PrimaDisponibilita, prestazioni[i]));
            }
            ListPrenotazioni = temp;
            IsVisible = false;
            IsBusy = false;
            IsVisibleButton = true;
        }

        private async Task info(PrenotazioneProposta prenotazione)
        {
            REST<Prestazioni, PrenotazioneProposta> connessione = new REST<Prestazioni, PrenotazioneProposta>();
            PrenotazioneProposta nuovaproposta = new PrenotazioneProposta();
            Prestazioni prestazione = new Prestazioni();
            prestazione.codprest = prenotazione.codPrestazione;
            prestazione.data_inizio = prenotazione.dataAppuntamento;
            prestazione.reparti.codReparto = prenotazione.codReparto;
            prestazione.reparti.unitaOperativa = prenotazione.unitaOperativa;
            nuovaproposta = await connessione.PostJson(URL.PrimaDisponibilita, prestazione);
            List<PrenotazioneProposta> temp = new List<PrenotazioneProposta>(ListPrenotazioni);
            for (int i=0; i < temp.Count; i++)
            {
                if (temp[i].codPrestazione == nuovaproposta.codPrestazione)
                {
                    temp[i] = nuovaproposta;
                    break;
                }
            }
            ListPrenotazioni = temp;
        }

    }
}
