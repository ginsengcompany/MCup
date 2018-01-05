using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Model
{
    public class Contacts
    {
        public List<Contatto> contatti;
        public string nome { get; set; }
        public string cognome { get; set; }
        public string codice_fiscale { get; set; }
        public string data_nascita { get; set; }
        public string luogo_nascita { get; set; }
        public char sesso { get; set; }
        public string provincia { get; set; }

        public Contacts()
        {
            this.nome = "";
            this.cognome = "";
            this.codice_fiscale = "";
            this.data_nascita = "";
            this.luogo_nascita = "";
            this.sesso = ' ';
            this.provincia = "";
            contatti = new List<Contatto>();
        }

        public Contacts(string nome, string cognome, string codice_fiscale, string data_nascita, string luogo_nascita, char sesso, string provincia)
        {
            this.nome = nome;
            this.cognome = cognome;
            this.codice_fiscale = codice_fiscale;
            this.data_nascita = data_nascita;
            this.luogo_nascita = luogo_nascita;
            this.sesso = sesso;
            this.provincia = provincia;
        }

        public Contacts(string nome, string cognome, string codice_fiscale, string data_nascita, string luogo_nascita, char sesso, string provincia, List<Contatto> contatti)
        {
            this.nome = nome;
            this.cognome = cognome;
            this.codice_fiscale = codice_fiscale;
            this.data_nascita = data_nascita;
            this.luogo_nascita = luogo_nascita;
            this.sesso = sesso;
            this.provincia = provincia;
            this.contatti = contatti;
        }

        public int searchContact(string nome, string cognome, string codice_fiscale)
        {
            if (this.nome == nome && this.cognome == cognome && this.codice_fiscale == codice_fiscale)
                return -1;
            else
            {
                for (int i = 0; i < this.contatti.Count; i++)
                {
                    if (this.contatti[i].nome == nome && this.contatti[i].cognome == cognome && this.contatti[i].codice_fiscale == codice_fiscale)
                    {
                        return i;
                    }
                }
                return -2;
            }
        }
    }
}
