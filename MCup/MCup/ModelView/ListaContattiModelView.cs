using MCup.Model;
using MCup.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MCup.Views;
using MvvmHelpers;
using Xamarin.Forms;

namespace MCup.ModelView
{
    public class ListaContattiModelView : INotifyPropertyChanged
    {
      
        private List<Contatto> contatti = new List<Contatto>();
        public string primoNome;
        public ICommand AggiungereContatto { protected set; get; }
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

        public string PrimoNome
        {
            get { return primoNome; }
            set
            {
                OnPropertyChanged();
                primoNome = value;
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ListaContattiModelView()
        {
            leggiContatti();
            AggiungereContatto = new Command(() =>
            {
                App.Current.MainPage = new NuovoContatto();
            });
        }

        private async void leggiContatti()
        {
            REST<object, Contacts> rest = new REST<object, Contacts>();
            Contacts contacts =
                await rest.GetSingleJson(URL.InfoPersonali, App.Current.Properties["tokenLogin"].ToString());
            List<Contatto> temp = new List<Contatto>();
            char a = 'a';
            temp.Add(new Contatto
            {
                nome = contacts.nome,
                cognome = contacts.cognome,
                codice_fiscale = contacts.codice_fiscale,
                data_nascita = contacts.data_nascita,
                luogo_nascita = contacts.luogo_nascita,
                sesso = contacts.sesso,
                provincia = contacts.provincia
            });
            for (int i = 0; i < contacts.contatti.Count; i++)
            {
                
                temp.Add(contacts.contatti[i]);
            }

            PrimoNome = temp[0].nome + " " + temp[0].cognome;

            Contatti = temp;
        }
    }
}
