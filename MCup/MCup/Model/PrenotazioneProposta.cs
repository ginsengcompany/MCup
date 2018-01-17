using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Model
{
    public class PrenotazioneProposta
    {
        public string codPrestazione { get; set; }
        public string descPrestazione { get; set; }
        public string durataPrestazione { get; set; }
        public string descReparto { get; set; }
        public string dataAppuntamento { get; set; }
        public bool disponibile { get; set; }
        public string oraAppuntamento { get; set; }
        public string posizione { get; set; }
        public string orarioApertura { get; set; }
        public string orarioChiusura { get; set; }
        public string codReparto { get; set; }
        public string unitaOperativa { get; set; }

        public PrenotazioneProposta(){ }
    }
}
