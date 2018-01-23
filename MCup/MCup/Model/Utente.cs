using System;
using Xamarin.Forms;

namespace MCup.Model
{
    public class Utente
    {
        public string username { get; set; }
        public string nome { get; set; }
        public string cognome { get; set; }
        public string codice_fiscale { get; set; }
        public string password { get; set; }
        public string data_nascita { get; set; }
        public string istatComuneNascita { get; set; }
        public string istatComuneResidenza { get; set; }
        public string luogo_nascita { get; set; }
        public char sesso { get; set; }
        public string codStatoCivile { get; set; }
        public string statocivile { get; set; }
        public string struttura_preferita { get; set; }
        public string comune_residenza { get; set; }
        public string telefono { get; set; }

        public Utente()
        {
            username = "";
            nome = "";
            cognome = "";
            codice_fiscale = "";
            password = "";
            struttura_preferita = "";
            this.data_nascita = "";
            this.luogo_nascita = "";
            this.sesso = ' ';
            this.comune_residenza = "";
            this.telefono = "";
            this.codStatoCivile = "";
            this.istatComuneNascita = "";
            this.istatComuneResidenza = "";
            this.statocivile = "";
        }

        public Utente(Utente utente)
        {
            this.username = utente.username;
            this.nome = utente.nome;
            this.cognome = utente.cognome;
            this.codice_fiscale = utente.codice_fiscale;
            this.password = utente.password;
            this.struttura_preferita = utente.struttura_preferita;
            this.data_nascita = utente.data_nascita;
            this.luogo_nascita = utente.luogo_nascita;
            this.sesso = utente.sesso;
            this.comune_residenza = utente.comune_residenza;
            this.telefono = utente.telefono;
            this.istatComuneResidenza = utente.istatComuneResidenza;
            this.istatComuneNascita = utente.istatComuneNascita;
            this.codStatoCivile = utente.codStatoCivile;
            this.statocivile = utente.statocivile;
        }


        /**
 * Questo metodo permette di salvare le credenziali (in particolare l'username) dell'utente
 * all'interno del dizionario gestito dal metodo statico Application.Current.Properties.Add
 * */
        public void salvaCredenzialiAccesso(string user)
        {
            if (!string.IsNullOrWhiteSpace(user))
            {
                this.username = user;
                Application.Current.Properties.Add("username", user);
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
            if (Application.Current.Properties.ContainsKey("username"))
            {
                u = Application.Current.Properties["username"].ToString();
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
            this.username = nuovo;
            Application.Current.Properties.Clear();
            Application.Current.Properties.Add("username", nuovo);
            Application.Current.SavePropertiesAsync();
        }
    }
}
