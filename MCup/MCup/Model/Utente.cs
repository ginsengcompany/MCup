using System;
using Xamarin.Forms;

namespace MCup.Model
{
    public class Utente
    {
        public string nome { get; set; }
        public string cognome { get; set; }
        public string codice_fiscale { get; set; }
        public string password { get; set; }
        public string data_nascita { get; set; }
        public string luogo_nascita { get; set; }
        public char sesso { get; set; }
        public string provincia { get; set; }
        public string struttura_preferita { get; set; }

        public Utente()
        {
            nome = "";
            cognome = "";
            codice_fiscale = "";
            password = "";
            struttura_preferita = "";
            this.data_nascita = "";
            this.luogo_nascita = "";
            this.sesso = ' ';
            this.provincia = "";
        }

        public Utente(Utente utente)
        {
            this.nome = utente.nome;
            this.cognome = utente.cognome;
            this.codice_fiscale = utente.codice_fiscale;
            this.password = utente.password;
            this.struttura_preferita = utente.struttura_preferita;
            this.data_nascita = utente.data_nascita;
            this.luogo_nascita = utente.luogo_nascita;
            this.sesso = utente.sesso;
            this.provincia = utente.provincia;
        }

        public Utente(string nome, string cognome, string codice_fiscale, string password, string data_nascita, string luogo_nascita, char sesso, string provincia)
        {
            this.nome = nome;
            this.cognome = cognome;
            this.codice_fiscale = codice_fiscale;
            this.password = password;
            this.data_nascita = data_nascita;
            this.luogo_nascita = luogo_nascita;
            this.sesso = sesso;
            this.provincia = provincia;
        }

        public Utente(string nome, string cognome, string codice_fiscale, string password, string data_nascita, string luogo_nascita, char sesso, string provincia, string struttura_preferita)
        {
            this.nome = nome;
            this.cognome = cognome;
            this.codice_fiscale = codice_fiscale;
            this.password = password;
            this.struttura_preferita = struttura_preferita;
            this.data_nascita = data_nascita;
            this.luogo_nascita = luogo_nascita;
            this.sesso = sesso;
            this.provincia = provincia;
        }


        /**
 * Questo metodo permette di salvare le credenziali (in particolare l'username) dell'utente
 * all'interno del dizionario gestito dal metodo statico Application.Current.Properties.Add
 * */
        public void salvaCredenzialiAccesso(string user)
        {
            if (!string.IsNullOrWhiteSpace(user))
            {
                this.codice_fiscale = user;
                Application.Current.Properties.Add("codice_fiscale", user);
                Application.Current.SavePropertiesAsync();
            }
        }

        /**
         * Questo metodo permette di recuperare l'username utente dalla memoria interna.
         * */
        public string recuperaUserName()
        {
            string u;
            /**
             * Se è già presente una chiave userName in memoria viene recuperato il campo userName 
             * dal dizionario interno dell'applicazione che è stato in precedenza settatto 
             * al valore inserito in fase di login. 
             * Tale dizionario (Application.Current.Properties) viene usato per memorizzare i dati
             * sul dispositivo.
             * */
            if (Application.Current.Properties.ContainsKey("codice_fiscale"))
            {
                u = Application.Current.Properties["codice_fiscale"].ToString();
            }
            else
            {
                u = null;
            }
            return u;
        }

        /**
         * Questo metodo agisce in caso di aggiornamento da parte dell'utente dell'username.
         * In tal modo viene salvato il nuovo username inserito in memoria del telefono, in 
         * particolare all'interno dell'Application.Current...
         * */
        public void cancellaEdAggiornaUsername(string nuovo)
        {
            this.codice_fiscale = nuovo;
            Application.Current.Properties.Clear();
            Application.Current.Properties.Add("codice_fiscale", nuovo);
            Application.Current.SavePropertiesAsync();
        }
    }
}
