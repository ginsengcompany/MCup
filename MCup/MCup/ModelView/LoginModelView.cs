using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MCup.Model;
using System.Windows.Input;
using Xamarin.Forms;
using MCup.Service;
using MCup.Views;

/*
 * Questa classe è il ModelView delle pagine Login e LoginIOS. La classe gestisce tutte le informazioni inserite e prelevate da remoto riguardanti l'utenza del cliente.
 * La classe concede l'accesso all'utente solo se le informazioni da lui inserite sono corrette. 
 */

namespace MCup.ModelView
{
    public class LoginModelView : INotifyPropertyChanged
    {
        private Utente utente; //Oggetto che astrae l'utenza del cliente e che nel caso in cui la login vada a buon fine conterrà le informazioni relative all'utente
        
        private string nameErrorTextPassword; 
        private bool isbusy; //variabile booleana utilizzata per gestire la proprietà IsRunning dell'activity indicator
        private string nameErrorText;
        private bool showPassword = true;
        private bool isvisible; //variabile booleana utilizzata per gestire la proprietà IsVisible dell'activity indicator
        private bool loginisvisible;
        private bool signupisvisible;

        //Command utilizzato per il tentativo di accesso ai servizi da parte dell'utente
        public ICommand effettuaLogin { protected set; get; }
        public ICommand showPass { protected set; get; }

        public event PropertyChangedEventHandler PropertyChanged; //evento che implementa l'interfaccia INotifyPropertyChanged

        public bool LoginIsVisible
        {
            get { return loginisvisible; }
            set
            {
                OnPropertyChanged();
                loginisvisible = value;
            }
        }

        public bool SignupIsVisible
        {
            get { return signupisvisible; }
            set
            {
                OnPropertyChanged();
                signupisvisible = value;
            }
        }

        public bool ShowPassword
        {
            get { return showPassword; }
            set
            {
                OnPropertyChanged();
                showPassword = value;
            }
        }
        //Proprietà relativa alla variabile isvisible
        public bool IsVisible
        {
            get { return isvisible; }
            set
            {
                OnPropertyChanged();
                isvisible = value;
            }
        }

        //Proprietà relativa alla variabile isbusy
        public bool IsBusy
        {
            get { return isbusy; }
            set
            {
                OnPropertyChanged();
                isbusy = value;
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

        public string NameErrorTextPassword
        {
            get { return nameErrorTextPassword; }
            set
            {
                OnPropertyChanged();
                nameErrorTextPassword = value;
            }
        }

        //Costruttore del ModelView che inizializza le variabili fondamentali per il corretto funzionamento della pagina di login (sia Android che IOS).
        public LoginModelView()
        {
            utente = new Utente(); //Crea un oggetto Utente vuoto
            codiceFiscale = utente.codice_fiscale = utente.recuperaUserName();
            LoginIsVisible = true;
            SignupIsVisible = true;
            IsVisible = false; //L'activity indicator non è visibile
            IsBusy = false; //L'activity indicator non si trova nello stato IsRunning
            effettuaLogin = new Command(async () => //Definisce il metodo del Command effettuaLogin che gestisce il tentativo di login da parte dell'utente
            {
                if (string.IsNullOrEmpty(codiceFiscale)) //Controlla che il campo codice fiscale non sia nullo o vuoto
                {
                    NameErrorText = "Attenzione codice fiscale non inserito correttamente";
                    IsBusy = false;
                }
                if (string.IsNullOrEmpty(passWord)) //Controlla che il campo password non sia nullo o vuoto
                {
                    NameErrorTextPassword = "Attenzione password non inserita correttamente";
                    IsBusy = false;
                }
                if (!string.IsNullOrEmpty(codiceFiscale) && !string.IsNullOrEmpty(passWord)) //se i campi codice fiscale e password non sono vuoti o null
                {
                    LoginIsVisible = false;
                    SignupIsVisible = false;
                    IsVisible = true; //L'activity indicator è visibile
                    IsBusy = true; //L'activity indicator è in stato IsRunning
                    REST<Utente, ResponseLogin> rest = new REST<Utente, ResponseLogin>(); //Crea l'oggetto per eseguire la chiamata REST per la login
                    ResponseLogin response = await rest.PostJson(URL.Login, utente); //Chiamata POST per la richiesta di autenticazione delle informazioni inserite dall'utente (codice fiscale e password)
                    if (response == null) //Controlla se si è verificato un errore di connessione
                    {
                       await App.Current.MainPage.DisplayAlert("Attenzione", rest.warning, "riprova");
                    }
                    else if (response == default(ResponseLogin)) //Controlla se la login non ha avuto successo
                    {
                        await App.Current.MainPage.DisplayAlert("Login", rest.warning, "OK");

                    }
                    else if (!response.auth) //Controlla se la login ha dato unn response negativo alle informazioni inserite dall'utente
                        await App.Current.MainPage.DisplayAlert("Login", "Login non riuscita", "OK");
                    else //Le informazioni dell'utenza sono corrette
                    {
                        utente.cancellaEdAggiornaUsername(utente.codice_fiscale);
                        App.Current.Properties["tokenLogin"] = response.token; //Salva nel dictionary dell'app il token dell'utente per accedere alle sue informazioni private
                      //  REST<object, ResponseStrutturaPreferita> restStrutturaPreferita = new REST<object, ResponseStrutturaPreferita>(); //Crea un oggetto per la chiamata REST
                       // ResponseStrutturaPreferita responseStruttura = await restStrutturaPreferita.GetSingleJson(URL.StrutturaPreferita, response.token); //Chiamata GET che ritorna se l'utente ha già scelto la sua struttura preferita o meno
                         /*   if (responseStruttura.scelta) //Se l'utente ha già scelto la sua struttura preferita
                              App.Current.MainPage = new MenuPrincipale(); //Avvia la pagina MenuPrincipale
                              else //Se l'utente non ha ancora scelto la sua struttura preferita
                              App.Current.MainPage = new ListaStrutture("Login"); //Avvia la pagina per la scelta di essa*/
                        App.Current.MainPage = new MenuPrincipale(); //Avvia la pagina MenuPrincipale
                    }
                    IsBusy = false; //L'activity indicator non è in stato IsRunning
                    IsVisible = false; //L'activity indicator non è visibile
                    LoginIsVisible = true;
                    SignupIsVisible = true;
                }
            });
            showPass = new Command(() =>
            {
                if (ShowPassword == true)
                    ShowPassword = false;
                else
                    ShowPassword = true;
            });
        }

    }
}
