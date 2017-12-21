using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCup.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using MCup.Service;
using MCup.Views;

namespace MCup.ModelView
{
    public class RegistrazioneModelView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Utente utente;

        public ICommand registrati { protected set; get; }

        public string codiceFiscale
        {
            get { return utente.codice_fiscale; }
            set
            {
                OnPropertyChanged();
                utente.codice_fiscale = value;
            }
        }

        public string password
        {
            get { return utente.password; }
            set
            {
                OnPropertyChanged();
                utente.password = value;
            }
        }

        public string nome
        {
            get { return utente.nome; }
            set
            {
                OnPropertyChanged();
                utente.nome = value;
            }
        }

        public string cognome
        {
            get { return utente.cognome; }
            set
            {
                OnPropertyChanged();
                utente.cognome = value;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public RegistrazioneModelView()
        {
            utente = new Utente();
            registrati = new Command(async () =>
            {
                if (utente.verificaCampiRegistrazione())
                {
                    REST<object, string> restTermini = new REST<object, string>();
                    var termini = await restTermini.getString(URL.TerminiServizio);
                    var accetaODeclina = await App.Current.MainPage.DisplayAlert("Termini di servizio", termini, "ACCETTA","DECLINA");
                    if (accetaODeclina)
                    {
                        REST<Utente, ResponseRegistrazione> rest = new REST<Utente, ResponseRegistrazione>();
                        ResponseRegistrazione response = await rest.PostJson(URL.Registrazione, utente);
                        if (response == default(ResponseRegistrazione))
                            await App.Current.MainPage.DisplayAlert("Registrazione", rest.warning, "OK");
                        else if (response.auth)
                        {
                            await App.Current.MainPage.DisplayAlert("Registrazione", "Registrazione effettuata con successo", "OK");
                            await App.Current.MainPage.Navigation.PopAsync();
                        }
                        else
                            await App.Current.MainPage.DisplayAlert("Registrazione", "Registrazione fallita", "OK");
                    }
                    else
                        await App.Current.MainPage.DisplayAlert("Registrazione", "Devi accettare i termini di servizio per poter proseguire", "OK");
                }
                else
                    await App.Current.MainPage.DisplayAlert("Registrazione", "Compilare tutti i campi", "OK");
            });
        }

        public class ResponseRegistrazione
        {
            public bool auth;
            public string token;
        }
    }
}
