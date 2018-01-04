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
    public class ListaContattiModelView : INotifyPropertyChanged
    {

        private List<Contatto> contatti;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Contatto> Contatti
        {
            get { return contatti; }
            set
            {
                OnPropertyChanged();
                contatti = new List<Contatto>(value);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ListaContattiModelView()
        {
            contatti = new List<Contatto>();
            leggiContatti();
        }

        private async void leggiContatti()
        {
            REST<object,Contacts> rest = new REST<object,Contacts>();
            Contacts contacts = await rest.GetSingleJson(URL.InfoPersonali, App.Current.Properties["tokenLogin"].ToString());
            Contatti.Add(convertiUtenteInContatto(contacts.utente));
            for (int i=0;i < contacts.contatti.Count; i++)
            {
                Contatti.Add(contacts.contatti[i]);
            }
        }

        private Contatto convertiUtenteInContatto(Utente utente)
        {
            Contatto contatto = new Contatto();
            contatto.nome = utente.nome;
            contatto.cognome = utente.cognome;
            contatto.codice_fiscale = utente.codice_fiscale;
            contatto.data_nascita = utente.data_nascita;
            contatto.luogo_nascita = utente.luogo_nascita;
            contatto.sesso = utente.sesso;
            contatto.provincia = utente.provincia;
            return contatto;
        }

        private class Contacts
        {
            public Utente utente;
            public List<Contatto> contatti;


            public Contacts()
            {
                utente = new Utente();
                contatti = new List<Contatto>();
            }
        }
    }
}
