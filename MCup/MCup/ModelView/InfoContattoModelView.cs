using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCup.Model;
using System.Windows.Input;
using System.ComponentModel;
using MCup.Service;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using MCup.Views;

namespace MCup.ModelView
{
    public class InfoContattoModelView : INotifyPropertyChanged
    {
        private Contatto utente = new Contatto();
        private string visibile="true";
        public string nomeCognome="";

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand Elimina { protected set; get; } //Command per il tentativo di eliminare un utenza 
        public string codiceFiscale //Proprietà relativa al campo codice fiscale
        {
            get { return utente.codice_fiscale; }
            set
            {
                OnPropertyChanged();
                utente.codice_fiscale = value;
            }
        }
        public string comune_residenza //Proprietà relativa al campo codice fiscale
        {
            get { return utente.comune_residenza; }
            set
            {
                OnPropertyChanged();
                utente.comune_residenza = value;
            }
        }
        public string telefono //Proprietà relativa al campo codice fiscale
        {
            get { return utente.telefono; }
            set
            {
                OnPropertyChanged();
                utente.telefono = value;
            }
        }
        public string Visibile //Proprietà relativa al campo codice fiscale
        {
            get { return visibile; }
            set
            {
                OnPropertyChanged();
                visibile = value;
            }
        }


        public string nome //Proprietà relativa al campo nome
        {
            get { return utente.nome; }
            set
            {
                OnPropertyChanged();
                utente.nome = value;
            }
        }

        public string cognome //Proprietà relativa al campo cognome
        {
            get { return utente.cognome; }
            set
            {
                OnPropertyChanged();
                utente.cognome = value;
            }
        }

        public string statocivile
        {
            get { return utente.statocivile; }
            set
            {
                OnPropertyChanged();
                utente.statocivile = value;
            }
        }

        public string Sesso //Proprietà relativa al campo sesso
        {
            get {
                if (utente.sesso == 'M')
                    return "Maschio";
                else
                    return "Femmina";
            }
            set
            {
                OnPropertyChanged();
                utente.sesso = value[0];
            }
        }

        public string data_nascita //Proprietà relativa al campo data di nascita
        {
            get { return utente.data_nascita; }
            set
            {
                OnPropertyChanged();
                utente.data_nascita = value;
            }
        }

        public string luogo_nascita //Proprietà relativa al campo luogo di nascita
        {
            get { return utente.luogo_nascita; }
            set
            {
                OnPropertyChanged();
                utente.luogo_nascita = value;
            }
        }

        public string NomeCognome
        {
            get { return nomeCognome; }
            set
            {
                nomeCognome = value;
                OnPropertyChanged();
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public InfoContattoModelView(Contatto info)
        {
            utente = info;
            NomeCognome = info.nome + " " + info.cognome;
            if (info.AccountPrimario)
            {
                Elimina = new Command(async () =>
                {
                    REST<object,string> connessioneElimina = new REST<object, string>();
                    
                  var risposta=  await App.Current.MainPage.DisplayAlert("Eliminazione", "sei sicuro di voler eliminare l'account? Se confermi allora dovrai effettuare una nuova registrazione", "si","no");
                    if (risposta)
                    {
                        try
                        {
                            var response = await connessioneElimina.getStringHeader(URL.eliminaContattoPersonale,
                                                App.Current.Properties["tokenLogin"].ToString());
                        }
                        catch (Exception)
                        {
                            await App.Current.MainPage.DisplayAlert("Attenzione", connessioneElimina.warning, "ok");

                        }
                        await  App.Current.MainPage.DisplayAlert("Complimenti", "l'account è stato eliminato con successo", "ok");
                        App.Current.MainPage = new NavigationPage(new Login());
                    }
                    
                    
                });
            }
            else
            {
                Elimina = new Command(async () =>
                {
                    var risposta = await App.Current.MainPage.DisplayAlert("ATTENZIONE", "Sei sicuro di voler eliminare questo contatto?", "SI", "NO");
                    if (risposta == false)
                        return;
                    REST<Contatto, string> restElimina = new REST<Contatto, string>();
                    string response = await restElimina.PostJson(URL.EliminaContatto, utente, App.Current.Properties["tokenLogin"].ToString());
                    await App.Current.MainPage.DisplayAlert("Eliminazione", restElimina.warning, "OK");
                    App.Current.MainPage = new MenuPrincipale();
                });
            }
          
         }


    }
}
