using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MCup.Model;
using System.Windows.Input;
using Xamarin.Forms;
using MCup.Service;
using MCup.Views;

namespace MCup.ModelView
{
    public class LoginModelView : INotifyPropertyChanged
    {
        private Utente utente;
        private string nameErrorTextPassword;
        private bool isbusy;
        private string nameErrorText;

        private bool isvisible;

        public ICommand effettuaLogin { protected set; get; }

        public bool IsVisible
        {
            get { return isvisible; }
            set
            {
                OnPropertyChanged();
                isvisible = value;
            }
        }

        public bool IsBusy
        {
            get { return isbusy; }
            set
            {
                OnPropertyChanged();
                isbusy = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public LoginModelView()
        {
            utente = new Utente();
            IsVisible = false;
            IsBusy = false;
            effettuaLogin = new Command(async () =>
            {
                if (string.IsNullOrEmpty(codiceFiscale))
                {
                    NameErrorText = "Attenzione codice fiscale non inserito correttamente";
                    IsBusy = false;
                }
                if (string.IsNullOrEmpty(passWord))
                {
                    NameErrorTextPassword = "Attenzione password non inserita correttamente";
                    IsBusy = false;
                }
                if (!string.IsNullOrEmpty(codiceFiscale) && !string.IsNullOrEmpty(passWord))
                {
                    IsVisible = true;
                    IsBusy = true;
                    REST<Utente, ResponseLogin> rest = new REST<Utente, ResponseLogin>();
                    ResponseLogin response = await rest.PostJson(URL.Login, utente);
                    IsBusy = false;
                    IsVisible = false;
                    if (response == null)
                    {
                       await App.Current.MainPage.DisplayAlert("Attenzione", "connessione non riuscita", "riprova");
                    }

                    if (response == default(ResponseLogin))
                    {
                        await App.Current.MainPage.DisplayAlert("Login", rest.warning, "OK");

                    }
                    else if (!response.auth)
                        await App.Current.MainPage.DisplayAlert("Login", "Login non riuscita", "OK");
                    else
                    {
                        App.Current.Properties["tokenLogin"] = response.token;
                        REST<object, ResponseStrutturaPreferita> restStrutturaPreferita =
                            new REST<object, ResponseStrutturaPreferita>();
                        ResponseStrutturaPreferita responseStruttura =
                            await restStrutturaPreferita.GetSingleJson(URL.StrutturaPreferita, response.token);
                        if (responseStruttura.scelta)
                            App.Current.MainPage = new MenuPrincipale();
                        else
                            App.Current.MainPage = new ListaStrutture("Login");
                    }
                }
            });
        }

        public string NameErrorTextPassword
        {
            get { return nameErrorTextPassword; }
            set
            {
                OnPropertyChanged();
                nameErrorTextPassword = value;
            }
        }
        //Proprietà che definisce l' Username di chi effettua l'accesso
        public string codiceFiscale
        { 
            get { return utente.codice_fiscale; }
            set 
            { 
                OnPropertyChanged();
                utente.codice_fiscale = value;
            }
        }
        //Proprietà che definisce la password di chi effettua il login
        public string passWord
        {
            get { return utente.password; }
            set
            {
                OnPropertyChanged();
                utente.password = value;
            }
        }

        public string NameErrorText
        {
            get { return nameErrorText; }
            set
            {
                OnPropertyChanged();
                nameErrorText = value;
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private class ResponseLogin
        {
            public string token;
            public bool auth;
        }

        private class ResponseStrutturaPreferita
        {
            public bool scelta;
            public string struttura;
        }

    }
}
