using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Model
{
    public class Prestazioni
    {
        public string codprest { get; set; }
        public string desbprest { get; set; }
        public string desprest { get; set; }
        public string data_inizio { get; set; }
        public Reparto reparti { get; set; }
        public bool erogabile { get; set; }


        public Prestazioni() { reparti = new Reparto(); }

        public Prestazioni(Prestazioni prestazioni)
        {
            this.codprest = prestazioni.codprest;
            this.desbprest = prestazioni.desbprest;
            this.desprest = prestazioni.desprest;
            this.reparti = prestazioni.reparti;
            this.data_inizio = prestazioni.data_inizio;
        }

        public Prestazioni(string codprest, string desbprest, string desprest, bool erogabile)
        {
            this.codprest = codprest;
            this.desbprest = desbprest;
            this.desprest = desprest;
            this.erogabile = erogabile;
            this.reparti = new Reparto();
        }
    }
}
