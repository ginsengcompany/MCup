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
        public string oraAppuntamento { get; set; }
        public string posizione { get; set; }
        public string orarioApertura { get; set; }
        public string orarioChiusura { get; set; }

        public PrenotazioneProposta()
        {
            this.codPrestazione = "";
            this.dataAppuntamento = "";
            this.descPrestazione = "";
            this.descReparto = "";
            this.durataPrestazione = "";
            this.oraAppuntamento = "";
            this.orarioApertura = "";
            this.orarioChiusura = "";
            this.posizione = "";
        }
    }
}
