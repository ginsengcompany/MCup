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
using Xamarin.Forms;

namespace MCup.ModelView
{
    public class ListaContattiModelView : INotifyPropertyChanged
    {
      
        private List<Contatto> contatti = new List<Contatto>();
        public string primoNome;
        public ICommand AggiungereContatto { protected set; get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Rubrica> Grouped
        {
            get { return grouped; }
            set
            {
                OnPropertyChanged();
                grouped = value;
            }
        }

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

        private ObservableCollection<Rubrica> grouped { get; set; }

        public ListaContattiModelView()
        {
            Grouped = new ObservableCollection<Rubrica>();
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
            ImplementaRubrica();
        }

        private void ImplementaRubrica()
        {
            List<Rubrica> listGroup = new List<Rubrica>();
            listGroup.Add(new Rubrica("A", "A"));
            listGroup.Add(new Rubrica("B", "B"));
            listGroup.Add(new Rubrica("C", "C"));
            listGroup.Add(new Rubrica("D", "D"));
            listGroup.Add(new Rubrica("E", "E"));
            listGroup.Add(new Rubrica("F", "F"));
            listGroup.Add(new Rubrica("G", "G"));
            listGroup.Add(new Rubrica("H", "H"));
            listGroup.Add(new Rubrica("I", "I"));
            listGroup.Add(new Rubrica("J", "J"));
            listGroup.Add(new Rubrica("K", "K"));
            listGroup.Add(new Rubrica("L", "L"));
            listGroup.Add(new Rubrica("M", "M"));
            listGroup.Add(new Rubrica("N", "N"));
            listGroup.Add(new Rubrica("O", "O"));
            listGroup.Add(new Rubrica("P", "P"));
            listGroup.Add(new Rubrica("Q", "Q"));
            listGroup.Add(new Rubrica("R", "R"));
            listGroup.Add(new Rubrica("S", "S"));
            listGroup.Add(new Rubrica("T", "T"));
            listGroup.Add(new Rubrica("U", "U"));
            listGroup.Add(new Rubrica("V", "V"));
            listGroup.Add(new Rubrica("W", "W"));
            listGroup.Add(new Rubrica("X", "X"));
            listGroup.Add(new Rubrica("Y", "Y"));
            listGroup.Add(new Rubrica("Z", "Z"));
            for(int i = 0; i < contatti.Count; i++)
            {
                for(int j = 0; j < listGroup.Count; j++)
                {
                    if(listGroup[j].ShortName.ToLower()[0] == contatti[i].nome.ToLower()[0])
                    {
                        listGroup[j].Add(contatti[i]);
                        break;
                    }
                }
            }
            for(int i = 0; i < listGroup.Count; i++)
            {
                Grouped.Add(listGroup[i]);
            }
        }
    }
}
