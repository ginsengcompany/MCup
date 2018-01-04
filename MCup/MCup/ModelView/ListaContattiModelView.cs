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

        private List<Contatto> contatti = new List<Contatto>();

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
            leggiContatti();
        }

        private async void leggiContatti()
        {
            REST<object,Contacts> rest = new REST<object,Contacts>();
            Contacts contacts = await rest.GetSingleJson(URL.InfoPersonali, App.Current.Properties["tokenLogin"].ToString());
            List<Contatto> temp = new List<Contatto>();
            temp.Add(new Contatto {
                nome = contacts.nome,
                cognome = contacts.cognome,
                codice_fiscale = contacts.codice_fiscale,
                data_nascita = contacts.data_nascita,
                luogo_nascita = contacts.luogo_nascita,
                sesso = contacts.sesso,
                provincia = contacts.provincia
            });
            for (int i=0;i < contacts.contatti.Count; i++)
            {
                temp.Add(contacts.contatti[i]);
            }
            Contatti = temp;
        }

        private class Contacts
        {
            public List<Contatto> contatti;
            public string nome { get; set; }
            public string cognome { get; set; }
            public string codice_fiscale { get; set; }
            public string data_nascita { get; set; }
            public string luogo_nascita { get; set; }
            public char sesso { get; set; }
            public string provincia { get; set; }

            public Contacts()
            {
                this.nome = "";
                this.cognome = "";
                this.codice_fiscale = "";
                this.data_nascita = "";
                this.luogo_nascita = "";
                this.sesso = ' ';
                this.provincia = "";
                contatti = new List<Contatto>();
            }
        }
    }
}
