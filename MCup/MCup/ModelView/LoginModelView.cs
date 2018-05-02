#region Librerie

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using MCup.Model;
using System.Windows.Input;
using Xamarin.Forms;
using MCup.Service;
using MCup.Views;
using Com.OneSignal;

#endregion


/*
 * Questa classe è il ModelView delle pagine Login e LoginIOS. La classe gestisce tutte le informazioni inserite e prelevate da remoto riguardanti l'utenza del cliente.
 * La classe concede l'accesso all'utente solo se le informazioni da lui inserite sono corrette. 
 */

namespace MCup.ModelView
{
    public class LoginModelView : INotifyPropertyChanged
    {

        #region DichiarazioneVariabili

        private Utente utente; //Oggetto che astrae l'utenza del cliente e che nel caso in cui la login vada a buon fine conterrà le informazioni relative all'utente
        private bool flagLogin = false;//Booleano che andrà a true quando l'utente avrà effettuato la login
        public event PropertyChangedEventHandler PropertyChanged; //evento che implementa l'interfaccia INotifyPropertyChanged
        private string nameErrorTextPassword;//Variabile utilizzata nel caso in cui ci sia un errore nel campo password o sia vuoto
        private bool isbusy; //variabile booleana utilizzata per gestire la proprietà IsRunning dell'activity indicator
        private string nameErrorText;//Variabile utilizzata nel caso in cui ci sia un errore nel campo nome o sia vuoto
        private bool showPassword = true;//Booleano utilizzato per mostrare o meno la password
        private bool isvisible; //variabile booleana utilizzata per gestire la proprietà IsVisible dell'activity indicator
        private bool loginisvisible;//Booleano utilizzato per mostrare o meno alcuni elementi nello xaml
        private bool signupisvisible;//Booleano utilizzato per rendere visibile o meno la label per la registrazione
        private bool isenabled;//booleano utilizzato per abilitare o meno un elemento nello xaml
        private Login loginPage;//Oggetto del tipo della pagina Login
        private List<Header> listaHeader = new List<Header>();//Lista di header
        private ImageSource showPasswordImage = "EyePasswordWhite.png";//Sorgente da cui andremo a prendere l'immagine dell'occhio per mostrare la password
        private ImageSource logoOspedale;//Sorgente per il logo della struttura 


        #endregion

        #region Proprietà

        //Proprietà per il campo ShowPassword
        public ImageSource ShowPasswordImage
        {
            get { return showPasswordImage; }
            set
            {
                OnPropertyChanged();
                showPasswordImage = value;
            }
        }

        //Proprietà relativa al logo della struttura
        public ImageSource LogoStruttura
        {
            get { return logoOspedale; }
            set
            {
                OnPropertyChanged();
                logoOspedale = value;
            }
        }
        //Command utilizzato per il tentativo di accesso ai servizi da parte dell'utente
        public ICommand effettuaLogin { protected set; get; }
        //Command utilizzato per mostrare la password
        public ICommand showPass { protected set; get; }


        //proprietà relativa al campo loginVisible
        public bool LoginIsVisible
        {
            get { return loginisvisible; }
            set
            {
                OnPropertyChanged();
                loginisvisible = value;
            }
        }

        //proprietà relativa al campo Signup

        public bool SignupIsVisible
        {
            get { return signupisvisible; }
            set
            {
                OnPropertyChanged();
                signupisvisible = value;
            }
        }

        //proprietà relativa al campo ShowPassword

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

        //proprietà relativa al campo isEnabled

        public bool IsEnabled
        {
            get { return isenabled; }
            set
            {
                OnPropertyChanged();
                isenabled = value;
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

        //proprietà relativa al campo NameErrorTextPassword
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
        public string Username
        {
            get { return utente.username; }
            set
            {
                OnPropertyChanged();
                utente.username = value;
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

        //Proprietà relativa al campo nome
        public string NameErrorText
        {
            get { return nameErrorText; }
            set
            {
                OnPropertyChanged();
                nameErrorText = value;
            }
        }


        #endregion

        #region OnPropertyChange

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region PrivateClass

        private class TokenNotification
        {
            public string tokenNotification;
        }
        private class ResponseLogin
        {
            public string token { get; set; }
            public bool auth { get; set; }
            public bool prenotazionePending { get; set; } = false;
        }

        private class ResponseStrutturaPreferita
        {
            public bool scelta;
            public string struttura;
        }

        #endregion

        #region Costruttore

        //Costruttore del ModelView che inizializza le variabili fondamentali per il corretto funzionamento della pagina di login (sia Android che IOS).
        public LoginModelView(Login loginPage)
        {


            utente = new Utente(); //Crea un oggetto Utente vuoto
            Username = utente.username = utente.recuperaUserName();
            LoginIsVisible = true;
            SignupIsVisible = true;
            this.loginPage = loginPage;
            IsEnabled = true;
            IsVisible = false; //L'activity indicator non è visibile
            IsBusy = false; //L'activity indicator non si trova nello stato IsRunning
            RicezioneLogo();
            effettuaLogin = new Command(async () => //Definisce il metodo del Command effettuaLogin che gestisce il tentativo di login da parte dell'utente
            {
                IsEnabled = false;
                if (string.IsNullOrEmpty(Username)) //Controlla che il campo codice fiscale non sia nullo o vuoto
                {
                    NameErrorText = "Attenzione Username non inserito correttamente";
                    IsBusy = false;
                }
                if (string.IsNullOrEmpty(passWord)) //Controlla che il campo password non sia nullo o vuoto
                {
                    NameErrorTextPassword = "Attenzione password non inserita correttamente";
                    IsBusy = false;
                }
                if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(passWord)) //se i campi codice fiscale e password non sono vuoti o null
                {
                    LoginIsVisible = false;
                    SignupIsVisible = false;
                    IsVisible = true; //L'activity indicator è visibile
                    IsBusy = true; //L'activity indicator è in stato IsRunning
                    REST<Utente, ResponseLogin> rest = new REST<Utente, ResponseLogin>(); //Crea l'oggetto per eseguire la chiamata REST per la login
                    ResponseLogin response = await rest.PostJson(SingletonURL.Instance.getRotte().Login, utente); //Chiamata POST per la richiesta di autenticazione delle informazioni inserite dall'utente (codice fiscale e password)
                    if (rest.responseMessage != HttpStatusCode.OK)
                    {
                        await App.Current.MainPage.DisplayAlert("Attenzione " + (int)rest.responseMessage, rest.warning, "OK");
                    }
                    else if (response.auth) //Le informazioni dell'utenza sono corrette
                    {
                        utente.cancellaEdAggiornaUsername(utente.username);
                        App.Current.Properties["tokenLogin"] = response.token; //Salva nel dictionary dell'app il token dell'utente per accedere alle sue informazioni private
                        OneSignal.Current.IdsAvailable(async (string userId, string token) =>
                        {
                            TokenNotification tokNot = new TokenNotification();
                            tokNot.tokenNotification = userId;
                            if (listaHeader.Count != 0)
                                listaHeader.Clear();
                            listaHeader.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
                            listaHeader.Add(new Header("struttura", "150021"));
                            REST<TokenNotification, bool> connessione = new REST<TokenNotification, bool>();
                            bool res = await connessione.PostJson(SingletonURL.Instance.getRotte().updateTokenNotifiche, tokNot, listaHeader);
                            if (connessione.responseMessage != HttpStatusCode.OK)
                            {
                                await App.Current.MainPage.DisplayAlert("Attenzione " + (int)connessione.responseMessage, connessione.warning, "OK");
                            }
                        });
                        if (response.prenotazionePending)
                        {
                            var responseDisplayAlert = await App.Current.MainPage.DisplayAlert("Attenzione",
                                "Gentile utente, nell'ultima sessione abbiamo constatato che ha lasciato una prenotazione in sospeso, vuoi continuare?",
                                "si", "no");
                            if (responseDisplayAlert)
                            {
                                loginPage.PendingPrenotazione(response.prenotazionePending);
                                await Application.Current.SavePropertiesAsync();

                            }
                            else
                            {
                                REST<object, string> connessioneAnnullamento = new REST<object, string>();
                                string messaggioDiAnnullamento = await connessioneAnnullamento.getString(SingletonURL.Instance.getRotte().annullaPrenotazioneSospesa,
                                    listaHeader);
                                if (connessioneAnnullamento.responseMessage != HttpStatusCode.OK)
                                {
                                    await App.Current.MainPage.DisplayAlert("Attenzione " + (int)connessioneAnnullamento.responseMessage, connessioneAnnullamento.warning, "OK");
                                }
                                else
                                {
                                    await App.Current.MainPage.DisplayAlert("Attenzione", messaggioDiAnnullamento, "ok");
                                    App.Current.MainPage = new MenuPrincipale();
                                    await Application.Current.SavePropertiesAsync();
                                }
                               
                                
                            }
                        }
                        else
                        {
                            App.Current.MainPage = new MenuPrincipale(); //Avvia la pagina MenuPrincipale
                            await Application.Current.SavePropertiesAsync();
                        }

                        //  REST<object, ResponseStrutturaPreferita> restStrutturaPreferita = new REST<object, ResponseStrutturaPreferita>(); //Crea un oggetto per la chiamata REST
                        // ResponseStrutturaPreferita responseStruttura = await restStrutturaPreferita.GetSingleJson(URL.StrutturaPreferita, response.token); //Chiamata GET che ritorna se l'utente ha già scelto la sua struttura preferita o meno
                        /*   if (responseStruttura.scelta) //Se l'utente ha già scelto la sua struttura preferita
                             App.Current.MainPage = new MenuPrincipale(); //Avvia la pagina MenuPrincipale
                             else //Se l'utente non ha ancora scelto la sua struttura preferita
                             App.Current.MainPage = new ListaStrutture("Login"); //Avvia la pagina per la scelta di essa*/


                    }


                    IsBusy = false; //L'activity indicator non è in stato IsRunning
                    IsVisible = false; //L'activity indicator non è visibile
                    LoginIsVisible = true;
                    SignupIsVisible = true;
                    IsEnabled = true;
                }
                else
                    IsEnabled = true;
            }
            );
            showPass = new Command(() =>
            {
                if (ShowPassword == true)
                {
                    ShowPasswordImage = "eye.png";
                    ShowPassword = false;
                }

                else
                {
                    ShowPassword = true;
                    ShowPasswordImage = "EyePasswordWhite.png";
                }

            });
        }

        #endregion

        #region Metodi

        //Metodo che tramite una connessione riceve il logo Della struttura
        private async void RicezioneLogo()
        {
            if (listaHeader.Count != 0)
            {
                listaHeader.Clear();
            }
            listaHeader.Add(new Header("struttura", "150021"));
            REST<object, string> connessioneLogo = new REST<object, string>();
            try
            {
                var logo = await connessioneLogo.getString("http://192.168.125.14:3000/infostruttura/logoStruttura", listaHeader);
                LogoStruttura = Xamarin.Forms.ImageSource.FromStream(
                    () => new MemoryStream(Convert.FromBase64String(logo)));
            }
            catch (Exception)
            {
               await App.Current.MainPage.DisplayAlert("Attenzione", "errore nel prelievo del logo struttura", "ok");
            }
        }

        public void modificaFlag(bool flag)
        {
            flagLogin = flag;
        }

        #endregion
       
    }
}
