using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Model
{
    public class AppuntamentoPrestazioneProposto: Prestazione
    {
        public string dataAppuntamento { get; set; }
        public string oraAppuntamento { get; set; }
        public string posizione { get; set; }
        public bool disponibile { get; set; }

    }
}
