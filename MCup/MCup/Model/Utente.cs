using System;
namespace MCup.Model
{
    public class Utente
    {
        public string nome { get; set; }
        public string cognome { get; set; }
        public string codice_fiscale { get; set; }
        public string password { get; set; }
        public string struttura_preferita { get; set; }

        public Utente()
        {
            nome = "";
            cognome = "";
            codice_fiscale = "";
            password = "";
            struttura_preferita = "";
        }

        public Utente(Utente utente)
        {
            this.nome = utente.nome;
            this.cognome = utente.cognome;
            this.codice_fiscale = utente.codice_fiscale;
            this.password = utente.password;
            this.struttura_preferita = utente.struttura_preferita;
        }

        public Utente(string nome, string cognome, string codice_fiscale, string password)
        {
            this.nome = nome;
            this.cognome = cognome;
            this.codice_fiscale = codice_fiscale;
            this.password = password;
        }

        public Utente(string nome, string cognome, string codice_fiscale, string password, string struttura_preferita)
        {
            this.nome = nome;
            this.cognome = cognome;
            this.codice_fiscale = codice_fiscale;
            this.password = password;
            this.struttura_preferita = struttura_preferita;
        }

        public bool verificaCampiRegistrazione()
        {
            return this.nome != string.Empty && this.cognome != string.Empty && this.codice_fiscale != string.Empty && this.password != string.Empty;
        }
    }
}
