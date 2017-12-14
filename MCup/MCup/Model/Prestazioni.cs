using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Model
{
    public class Prestazioni
    {
        public string codnazionale { get; set; }
        public string codprest { get; set; }
        public string codregionale { get; set; }
        public string codstruttura { get; set; }
        public string desbprest { get; set; }
        public string desprest { get; set; }
        public string mnemo { get; set; }

        public Prestazioni() { }
        public Prestazioni(Prestazioni prestazioni)
        {
            this.codnazionale = prestazioni.codnazionale;
            this.codprest = prestazioni.codprest;
            this.codregionale = prestazioni.codregionale;
            this.codstruttura = prestazioni.codstruttura;
            this.desbprest = prestazioni.desbprest;
            this.desprest = prestazioni.desprest;
            this.mnemo = prestazioni.mnemo;
        }
    }
}
