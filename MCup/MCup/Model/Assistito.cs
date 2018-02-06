using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Model
{
    public class Assistito
    {
        public string nome { get; set; }
        public string cognome { get; set; }
        public char sesso { get; set; }
        public string codice_fiscale { get; set; }
        public string istatComuneNascita { get; set; }
        public string luogo_nascita { get; set; }
        public string istatComuneResidenza { get; set; }
        public string comune_residenza { get; set; }
        public string telefono { get; set; }
        public string codStatoCivile { get; set; }
        public string statocivile { get; set; }
        public string indirizzores { get; set; }
        public string email { get; set; }
        public string nomeCompletoConCodiceFiscale { get; set; }
        public bool AccountPrimario { get; set; } = false;
        public string data_nascita { get; set; }


    public Assistito()
        {
            nome = "";
            cognome = "";
            codice_fiscale = "";
            this.data_nascita = "";
            this.luogo_nascita = "";
            this.sesso = ' ';
            this.comune_residenza = "";
            this.telefono = "";
            this.codStatoCivile = "";
            this.istatComuneNascita = "";
            this.istatComuneResidenza = "";
            this.statocivile = "";
            this.email = "";
            this.indirizzores = "";
       
        }

        public void Maiuscolo()
        {
            this.nome = this.nome.ToUpper();
            this.cognome = this.cognome.ToUpper();
            this.codice_fiscale = this.codice_fiscale.ToUpper();
            this.comune_residenza = this.comune_residenza.ToUpper();
            this.luogo_nascita = this.luogo_nascita.ToUpper();
            this.codStatoCivile = this.codStatoCivile.ToUpper();
            this.indirizzores = this.indirizzores.ToUpper();
        }
    }
}
