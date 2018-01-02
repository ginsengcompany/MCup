using MCup.Model;
using MCup.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MCup.ModelView
{
    public class PropostaRichiestaModelView : INotifyPropertyChanged
    {
        private List<PrenotazioneProposta> listPrenotazioni = new List<PrenotazioneProposta>();

        public event PropertyChangedEventHandler PropertyChanged;

        private List<Prestazioni> prestazioni;

        public List<PrenotazioneProposta> list
        {
            get { return listPrenotazioni; }
            set
            {
                OnPropertyChanged();
                listPrenotazioni = new List<PrenotazioneProposta>(value);
            }
        }

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public PropostaRichiestaModelView(List<Prestazioni> prestazioni)
        {
            this.prestazioni = prestazioni;
            recuperoInformazioni();
        }

        private async void recuperoInformazioni()
        {
            REST<List<Prestazioni>, PrenotazioneProposta> recuperoDatiLista = new REST<List<Prestazioni>, PrenotazioneProposta>();
            list = await recuperoDatiLista.PostJsonList(URL.Ricercadisponibilitaprestazioni,prestazioni);
        }
    }
}
