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

        private bool isbusy;

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
                IsVisible = true;
                IsBusy = true;
                REST<Utente, ResponseLogin> rest = new REST<Utente, ResponseLogin>();
                ResponseLogin response = await rest.PostJson(URL.Login, utente);
                IsBusy = false;
                IsVisible = false;
                if (response == default(ResponseLogin))
                    await App.Current.MainPage.DisplayAlert("Login", rest.warning, "OK");
                else if (!response.auth)
                    await App.Current.MainPage.DisplayAlert("Login", "Login non riuscita", "OK");
                else
                    App.Current.MainPage = new MenuPrincipale();
            });
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

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private class ResponseLogin
        {
            public string token;
            public bool auth;
        }

    }
}
