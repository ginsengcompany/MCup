using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MCup.Model;

namespace MCup.ModelView
{
    public class LoginModelView : INotifyPropertyChanged
    {
        private Utente utente;

        public event PropertyChangedEventHandler PropertyChanged;
        public LoginModelView()
        {
            utente = new Utente();
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

    }
}
