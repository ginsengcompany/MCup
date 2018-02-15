#region Librerie

using System;
using System.Collections.Generic;
using MCup.Model;
using System.Windows.Input;
using System.ComponentModel;
using System.Net;
using MCup.Service;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using MCup.Views;

#endregion


namespace MCup.ModelView
{
    public class InfoContattoModelView : INotifyPropertyChanged
    {


        #region DichiarazioneVaribili
        //Oggetto che astrae le proprietà dell'utente utilizzatore
        private Assistito utente = new Assistito();
        //Variabile che implementata rende visibile o meno un elemento nello xaml
        private string visibile = "true";
        //Variabile che mostra il nome e il cognome dell'utente
        public string nomeCognome = "";
        private InfoContatto pagina;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Proprietà

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

        public string statocivile//Proprietà relativa al campo stato civile
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
            get
            {
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

        #endregion

        #region OnPropertyChange

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        #endregion

        #region Costruttore

        //Costruttore
        public InfoContattoModelView(Assistito info, InfoContatto paginaInfoContatto)
        {
            utente = info;
            this.pagina = paginaInfoContatto;
            NomeCognome = info.nome + " " + info.cognome;
            List<Header> listaheader = new List<Header>();
            listaheader.Add(new Header("x-access-token", App.Current.Properties["tokenLogin"].ToString()));
            if (info.AccountPrimario)
            {

                Elimina = new Command(async () =>
                {
                    REST<object, string> connessioneElimina = new REST<object, string>();

                    var risposta = await App.Current.MainPage.DisplayAlert("Eliminazione", "sei sicuro di voler eliminare l'account? Se confermi allora dovrai effettuare una nuova registrazione", "si", "no");
                    if (risposta)
                    {
                        try
                        {
                            var response = await connessioneElimina.getString(SingletonURL.Instance.getRotte().eliminaContattoPersonale, listaheader);
                            if (connessioneElimina.responseMessage != HttpStatusCode.OK)
                            {
                                await App.Current.MainPage.DisplayAlert("Attenzione " + (int)connessioneElimina.responseMessage, connessioneElimina.warning, "OK");
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert("Complimenti", "l'account è stato eliminato con successo", "ok");
                                App.Current.MainPage = new NavigationPage(new Login());
                            }
                        }
                        catch (Exception)
                        {
                            await App.Current.MainPage.DisplayAlert("Attenzione", connessioneElimina.warning, "ok");

                        }

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
                    REST<Assistito, string> restElimina = new REST<Assistito, string>();
                    string response = await restElimina.PostJson(SingletonURL.Instance.getRotte().EliminaContatto, utente, listaheader);
                    if (restElimina.responseMessage != HttpStatusCode.OK)
                    {
                        await App.Current.MainPage.DisplayAlert("Attenzione " + (int)restElimina.responseMessage, restElimina.warning, "OK");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Eliminazione", restElimina.warning, "OK");
                        App.Current.MainPage = new MenuPrincipale();
                    }

                });
            }

        }

        #endregion



    }
}
