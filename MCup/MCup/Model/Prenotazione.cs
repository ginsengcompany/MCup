using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Model
{
    public class Prenotazione
    {
        public UtenzaPrenotazione utente { get; set; }
        public string data { get; set; }
        public string ora { get; set; }
        public string codiceStruttura { get; set; }
        public string nomeStruttura { get; set; }
        public string codiceNRE { get; set; }
        public Prenotazione() { }
        public Prenotazione(UtenzaPrenotazione utente, ListaDatePrenotazioni data_prenotazione,string codiceStruttura, string nomeStruttura, string codiceNRE)
        {
            this.utente = utente;
            this.data = data_prenotazione.data;
            this.ora = data_prenotazione.ora;
            this.codiceStruttura = codiceStruttura;
            this.nomeStruttura = nomeStruttura;
            this.codiceNRE = codiceNRE;
        }
    }
}
