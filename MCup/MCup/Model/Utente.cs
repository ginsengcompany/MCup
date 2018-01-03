using System;
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
    }
}
