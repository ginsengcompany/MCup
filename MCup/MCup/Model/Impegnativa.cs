using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Model
{
    public class Impegnativa
    {
        public string nre { get; set; }
        public string sar { get; set; }
        public List<Prestazione> prestazioni { get; set; }
        public Assistito assistito { get; set; }
        public string dataEmissioneRicetta { get; set; }
        public string classePriorita { get; set; }

    }
}
