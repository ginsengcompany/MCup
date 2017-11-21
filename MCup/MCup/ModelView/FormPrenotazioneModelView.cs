using MCup.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MCup.ModelView
{
    public class FormPrenotazioneModelView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private UtenzaPrenotazione utenza;
        public string nomeUtente
        {
            get { return utenza.nome; }
            set
            {
                utenza.nome = value;
                OnPropertyChanged();
            }
        }
        public string cognomeUtente
        {
            get { return utenza.cognome; }
            set
            {
                utenza.cognome = value;
                OnPropertyChanged();
            }
        }
        public string codicefiscale
        {
            get { return utenza.getCodiceFiscale(); }
            set
            {
                utenza.setCodiceFiscale(value);
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
