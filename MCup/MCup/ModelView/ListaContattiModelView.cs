#region Librerie

using MCup.Model;
using MCup.Service;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MCup.Views;
using Xamarin.Forms;
using System.Linq;

#endregion

namespace MCup.ModelView
{
    public class ListaContattiModelView : INotifyPropertyChanged
    {


        #region DichiarazioneVariabili

        public event PropertyChangedEventHandler PropertyChanged;

        //Lista usata per contenere i contatti
        private List<Assistito> contatti = new List<Assistito>();
        //Variabile che contraddistingue il contatto dell'utente utilizzatore dell'applicativo dai contatti "figli"
        public string primoNome;

       //Variabile che contraddistingue il contatto dell'utente utilizzatore dell'applicativo dai contatti "figli"
        public Assistito contattoPrimo = new Assistito();
        //Comando che richiama il metodo per aggiungere un contatto
        public ICommand AggiungereContatto { protected set; get; }

        //Comando che richiama il metodo per andare alle info personali del contatto utente
        public ICommand MioContattoPersonale { protected set; get; }

        //comando utilizzato per ricercare all'interno della rubrica un contatto
        public ICommand searchContacts { protected set; get; }

        //oggetto utilizzato per richiamare metodi dalla pagina
        private ListaContatti paginaListaContatti;

        //Variabile utilizzata per ricercare i contatti
        private string textSearch = "";

        //Variabile usata per raggruppare la lista dei contatti
        private ObservableCollection<Rubrica> grouped { get; set; }

        //Variabile utilizzate per raggruppare i contatti visualizzati
        private ObservableCollection<Rubrica> collectionView;

        #endregion

        #region Proprietà

        //Proprietà relativa al campo textSearch
        public string TextSearch
        {
            get { return textSearch; }
            set
            {
                OnPropertyChanged();
                textSearch = value;
                aggiornaRubrica();
            }
        }

        //Proprietà relativa al collectionView
        public ObservableCollection<Rubrica> CollectionView
        {
            get { return collectionView; }
            set
            {
                OnPropertyChanged();
                collectionView = value;
            }
        }

        //Proprietà relativa al campo Contatti
        public List<Assistito> Contatti
        {
            get { return contatti; }
            set
            {
                OnPropertyChanged();
                contatti = new List<Assistito>(value);
            }
        }

        //Proprietà relativa al campo del primo nome visualizzato
        public string PrimoNome
        {
            get { return primoNome; }
            set
            {
                OnPropertyChanged();
                primoNome = value;
            }
        }
        #endregion

        #region OnPropertyChange

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region Costruttore

        public ListaContattiModelView(ListaContatti pagina)
        {
            paginaListaContatti = pagina;
            collectionView = new ObservableCollection<Rubrica>();
            grouped = new ObservableCollection<Rubrica>();
            leggiContatti();
            AggiungereContatto = new Command(() =>
            {
                App.Current.MainPage = new NavigationPage(new AutoCompilazionePage());
            });
            MioContattoPersonale = new Command(async () =>
            {
                await paginaListaContatti.Navigation.PushAsync(new InfoContatto(contattoPrimo));
            });
            searchContacts = new Command(() =>
            {
                aggiornaRubrica();
            });
        }

        #endregion

        #region Metodi

        //Metodo che implementa l'aggiornamento della rubrica
        private void aggiornaRubrica()
        {
            string keyword = textSearch;
            if (string.IsNullOrWhiteSpace(textSearch))
                CollectionView = grouped;
            else
            {
                ObservableCollection<Rubrica> temp = new ObservableCollection<Rubrica>();
                for (int i = 0; i < grouped.Count; i++)
                {
                    ObservableCollection<Assistito> tempContatto = new ObservableCollection<Assistito>();
                    for (int j = 0; j < grouped[i].Count; j++)
                    {
                        if (grouped[i][j].nomeCompletoConCodiceFiscale.ToLower().Contains(keyword.ToLower()))
                            tempContatto.Add(grouped[i][j]);
                    }
                    if (tempContatto.Count != 0)
                    {
                        Rubrica rubrica = new Rubrica(tempContatto[0].nome.ToUpper()[0].ToString(), tempContatto[0].nome.ToUpper()[0].ToString());
                        temp.Add(rubrica);
                        for (int k = 0; k < tempContatto.Count; k++)
                            temp[temp.IndexOf(rubrica)].Add(tempContatto[k]);
                    }
                }
                CollectionView = temp;
            }
        }

        //Metodo che tramite una connessione legge i contatti salvati
        private async void leggiContatti()
        {
            List<Header> listaHeader = new List<Header>();
            listaHeader.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
            REST<object, List<Assistito>> rest = new REST<object, List<Assistito>>();
            List<Assistito> contacts =
                await rest.GetSingleJson(SingletonURL.Instance.getRotte().InfoPersonali, listaHeader);
            if (rest.responseMessage != HttpStatusCode.OK)
            {
                await MessaggioConnessione.displayAlert((int)rest.responseMessage, rest.warning);
            }
            else
            {
                List<Assistito> temp = new List<Assistito>();
                char a = 'a';
                for (int i = 0; i < contacts.Count; i++)
                {
                    contacts[i].nomeCompletoConCodiceFiscale = contacts[i].nome + " " + contacts[i].cognome + " " + contacts[i].codice_fiscale;
                    temp.Add(contacts[i]);
                }
                PrimoNome = temp[0].nome + " " + temp[0].cognome;
                contattoPrimo.nome = temp[0].nome;
                contattoPrimo.cognome = temp[0].cognome;
                contattoPrimo.codice_fiscale = temp[0].codice_fiscale;
                contattoPrimo.data_nascita = temp[0].data_nascita;
                contattoPrimo.luogo_nascita = temp[0].luogo_nascita;
                contattoPrimo.sesso = temp[0].sesso;
                contattoPrimo.AccountPrimario = true;
                contattoPrimo.comune_residenza = temp[0].comune_residenza;
                contattoPrimo.telefono = temp[0].telefono;
                contattoPrimo.statocivile = temp[0].statocivile;
                contattoPrimo.codStatoCivile = temp[0].codStatoCivile;
                contattoPrimo.indirizzores = temp[0].indirizzores;
                contattoPrimo.email = temp[0].email;
                temp.Remove(temp[0]);
                Contatti = temp;
                ImplementaRubrica();
            }

        }

        //Metodo che implementa la visualizzazione della rubrica
        private void ImplementaRubrica()
        {
            List<Rubrica> listGroup = new List<Rubrica>();
            bool x;
            for (int i = 0; i < contatti.Count; i++)
            {
                x = false;
                if (listGroup.Count > 0)
                {
                    int j;
                    for (j = 0; j < listGroup.Count; j++)
                    {
                        if (listGroup[j].ShortName.ToLower()[0] == contatti[i].cognome.ToLower()[0])
                        {
                            x = true;
                            listGroup[j].Add(contatti[i]);
                            break;
                        }
                    }
                    if (!x)
                    {
                        listGroup.Add(new Rubrica(contatti[i].cognome.ToUpper()[0].ToString(), contatti[i].cognome.ToUpper()[0].ToString()));
                        listGroup[j].Add(contatti[i]);
                    }
                }
                else
                {
                    listGroup.Add(new Rubrica(contatti[i].cognome.ToUpper()[0].ToString(), contatti[i].cognome.ToUpper()[0].ToString()));
                    listGroup[0].Add(contatti[i]);
                }
            }
            for (int i = 0; i < listGroup.Count; i++)
            {
                grouped.Add(listGroup[i]);
            }
            CollectionView = new ObservableCollection<Rubrica>(grouped.OrderBy(o => o.ShortName).ToList());
            //CollectionView = grouped;
        }

        #endregion
    }

    #region PublicOrPrivateClass
    public class Rubrica : ObservableCollection<Assistito>
    {
        public string LongName { get; set; }
        public string ShortName { get; set; }

        public Rubrica(string longN, string shortN)
        {
            this.LongName = longN;
            this.ShortName = shortN;
        }
    }
    #endregion


}
