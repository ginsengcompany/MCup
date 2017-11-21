using MCup.Model;
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
    public class FormPrenotazioneModelView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private UtenzaPrenotazione utenza;

        private Ricetta ricetta;

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
        public string codicefiscaleUtente
        {
            get { return utenza.getCodiceFiscale(); }
            set
            {
                utenza.setCodiceFiscale(value);
                OnPropertyChanged();
            }
        }

        public string codiceUno
        {
            get { return ricetta.codice_uno; }
            set
            {
                ricetta.codice_uno = value;
                OnPropertyChanged();
            }
        }

        public string codiceDue
        {
            get { return ricetta.codice_due; }
            set
            {
                ricetta.codice_due = value;
                OnPropertyChanged();
            }
        }

        public FormPrenotazioneModelView()
        {
            utenza = new UtenzaPrenotazione();
            ricetta = new Ricetta();
            ricetta.codice_uno = "";
            ricetta.codice_due = "";
            utenza.nome = "";
            utenza.cognome = "";
            utenza.setCodiceFiscale("");
            InviaRichiesta = new Command(() =>
            {
                Debug.WriteLine(utenza.nome);
                Debug.WriteLine(utenza.cognome);
                Debug.WriteLine(utenza.getCodiceFiscale());
            });
        }

        public ICommand InviaRichiesta { protected set; get; }

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
