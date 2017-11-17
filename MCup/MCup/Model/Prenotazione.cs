using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Model
{
    public class Prenotazione
    {
        private UtenzaPrenotazione utente { get; set; }
        private string data_prenotazione { get; set; }
        private Struttura struttura { get; set; }
        private Ricetta ricetta { get; set; }
    }
}
