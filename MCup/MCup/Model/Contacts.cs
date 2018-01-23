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
        public string comune_residenza { get; set; }
        public string telefono { get; set; }
        public string codStatoCivile { get; set; }
        public string istatComuneNascita { get; set; }
        public string istatComuneResidenza { get; set; }
        public string statocivile { get; set; }

        public Contacts()
        {
            this.nome = "";
            this.cognome = "";
            this.codice_fiscale = "";
            this.data_nascita = "";
            this.luogo_nascita = "";
            this.sesso = ' ';
            this.provincia = "";
            this.comune_residenza = "";
            this.telefono = "";
            this.statocivile = "";
            contatti = new List<Contatto>();
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
