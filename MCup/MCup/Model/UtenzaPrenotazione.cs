using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Model
{
    //La seguente classe astra l'utenze per effettuare la prenotazione
    public class UtenzaPrenotazione
    {
        public string nome { get; set; } //nome dell'utente
        public string cognome { get; set; } //cognome dell'utente
        private string codice_fiscale { get; set; } //codice fiscale dell'utente

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
    }
}
