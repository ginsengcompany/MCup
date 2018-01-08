using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MCup.Model
{
    //La seguente classe astra l'utenze per effettuare la prenotazione
    public class UtenzaPrenotazione
    {
        public string nome { get; set; } //nome dell'utente
        public string cognome { get; set; } //cognome dell'utente
        private string codice_fiscale { get; set; } //codice fiscale dell'utente
        private Regex regexCodFisc = new Regex(@"^[A-Za-z]{6}[0-9]{2}[A-Za-z]{1}[0-9]{2}[A-Za-z]{1}[0-9]{3}[A-Za-z]{1}$");//Espressione regolare utilizzata per controllare che il codice fiscale sia corretto
        //Costruttore con parametri
        public UtenzaPrenotazione(string name, string surname, string cod)
        {
            nome = name;
            cognome = surname;
            codice_fiscale = cod;
        }

        //Costruttore di default
        public UtenzaPrenotazione()
        {
        }

        public string getCodiceFiscale()
        {
            return this.codice_fiscale;
        }

        public void setCodiceFiscale(string codicefiscale)
        {
            this.codice_fiscale = codicefiscale;
        }

        public bool checkCodiceFiscale()
        {
            return regexCodFisc.IsMatch(this.codice_fiscale);
        }
    }
}
