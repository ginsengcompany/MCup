using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Model
{
    public class Contatto
    {
        public string nome { get; set; }
        public string cognome { get; set; }
        public string codice_fiscale{ get; set;}
        public string data_nascita { get; set; }
        public string luogo_nascita { get; set; }
        public string provincia { get; set; }
        public char sesso { get; set; }
        public bool AccountPrimario { get; set; }


        public Contatto()
        {
            this.nome = "";
            this.cognome = "";
            this.codice_fiscale = "";
            this.data_nascita = "";
            this.luogo_nascita = "";
            this.provincia = "";
            this.sesso = ' ';
            this.AccountPrimario =false;
        }

        public Contatto(Contatto contatto)
        {
            this.nome = contatto.nome;
            this.cognome = contatto.cognome;
            this.codice_fiscale = contatto.codice_fiscale;
            this.data_nascita = contatto.data_nascita;
            this.luogo_nascita = contatto.luogo_nascita;
            this.provincia = contatto.provincia;
            this.sesso = contatto.sesso;
            this.AccountPrimario = contatto.AccountPrimario;
        }

        public string longName()
        {
            return this.nome + " " + cognome;
        }
    }
}
