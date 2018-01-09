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
        public string NomeCognome="";

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

        public string provincia
        {
            get { return utente.provincia; }
            set
            {
                OnPropertyChanged();
                utente.provincia = value;
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
                Visibile = "false";
            }
            Elimina = new Command(async () =>
            {
                REST<Contatto, string> restElimina = new REST<Contatto, string>();
                string response = await restElimina.PostJson(URL.EliminaContatto, utente, App.Current.Properties["tokenLogin"].ToString());
                await App.Current.MainPage.DisplayAlert("Eliminazione", restElimina.warning, "OK");
                App.Current.MainPage = new MenuPrincipale();
                // await App.Current.MainPage.Navigation.PopAsync();
            });
         }


    }
}
