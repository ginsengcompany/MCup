using MCup.Model;
using MCup.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MCup.ModelView
{
    class ListaDatePrenotazioniModelView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<ListaDatePrenotazioni> listaDataPrenotazioni = new List<ListaDatePrenotazioni>();

        private const string url = "http://192.168.125.14:3000/calendario";

        /* Setta la lista da visualizare nel Binding*/
        public List<ListaDatePrenotazioni> ListaDataPrenotazioni
        {
            get
            {
                return listaDataPrenotazioni;
            }
            set
            {
                listaDataPrenotazioni = new List<ListaDatePrenotazioni>(value);
                OnPropertychanged();
            }
        }

        private void OnPropertychanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /*Effettua la connessione per ricevere i dati dal server*/
        public async void leggiDati()
        {
            REST<ListaDatePrenotazioni> connessione = new REST<ListaDatePrenotazioni>();
            ListaDataPrenotazioni = await connessione.GetJson(url);
        }
        /*Costruttore del metodo, avvia la connessione*/
        public  ListaDatePrenotazioniModelView()
        {
           leggiDati();
        }
    }
}
