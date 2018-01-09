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
        private NavigationPage pagina = new  NavigationPage();
        private InfoContatto paginaInfoContatto;
        public  Contatto contattoPrimo = new Contatto();
        public ICommand AggiungereContatto { protected set; get; }
        public ICommand MioContattoPersonale { protected set; get; }
        private ListaContatti paginaListaContatti;

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

        public ListaContattiModelView(ListaContatti pagina)
        {
            paginaListaContatti = pagina;
            Grouped = new ObservableCollection<Rubrica>();
            leggiContatti();
            AggiungereContatto = new Command(() =>
            {
                App.Current.MainPage = new NuovoContatto();
            });
            MioContattoPersonale = new Command(async () =>
            {
                await paginaListaContatti.Navigation.PushAsync(new InfoContatto(contattoPrimo));
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
            contattoPrimo.nome = temp[0].nome;
            contattoPrimo.cognome= temp[0].cognome;
            contattoPrimo.codice_fiscale= temp[0].codice_fiscale;
            contattoPrimo.data_nascita = temp[0].data_nascita;
            contattoPrimo.luogo_nascita = temp[0].luogo_nascita;
            contattoPrimo.provincia = temp[0].provincia;
            contattoPrimo.sesso = temp[0].sesso;
            contattoPrimo.AccountPrimario = true;
            temp.Remove(temp[0]);
            Contatti = temp;
            ImplementaRubrica();
        }

        private void ImplementaRubrica()
        {
            List<Rubrica> listGroup = new List<Rubrica>();
            bool x;
            for(int i = 0; i < contatti.Count; i++)
            {
                x = false;
                if (listGroup.Count > 0)
                {
                    int j;
                    for (j = 0; j < listGroup.Count; j++)
                    {
                        if(listGroup[j].ShortName.ToLower()[0] == contatti[i].nome.ToLower()[0])
                        {
                            x = true;
                            listGroup[j].Add(contatti[i]);
                            break;
                        }
                    }
                    if (!x)
                    {
                        listGroup.Add(new Rubrica(contatti[i].nome.ToUpper()[0].ToString(), contatti[i].nome.ToUpper()[0].ToString()));
                        listGroup[j].Add(contatti[i]);
                    }
                }
                else
                {
                    listGroup.Add(new Rubrica(contatti[i].nome.ToUpper()[0].ToString(), contatti[i].nome.ToUpper()[0].ToString()));
                    listGroup[0].Add(contatti[i]);
                }
            }
            for(int i = 0; i < listGroup.Count; i++)
            {
                Grouped.Add(listGroup[i]);
            }
        }

      
    }
}
