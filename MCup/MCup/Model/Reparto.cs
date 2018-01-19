using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Model
{
    public class Reparto
    {
        public string unitaOperativa { get; set; }
        public string codReparto { get; set; }
        public string descrizione { get; set; }
        public string nomeStruttura { get; set; }
        public string dataDisponibile { get; set; }
        public string codprest { get; set; }
        public int defaultReparto { get; set; }
        

        public Reparto(Reparto reparto)

        {
            this.codReparto = reparto.codReparto;
            this.dataDisponibile = reparto.dataDisponibile;
            this.descrizione = reparto.descrizione;
            this.nomeStruttura = reparto.nomeStruttura;
            this.unitaOperativa = reparto.unitaOperativa;
            this.codprest = reparto.codprest;
        }

        public Reparto()
        {

        }
    }
}
