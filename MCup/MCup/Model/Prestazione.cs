using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Model
{
    public class Prestazione: PickerProperties
    {
        public string codprest { get; set; }
        public string codregionale { get; set; }
        public string codnazionale { get; set; }
        public string desprest { get; set; }
        public string durata { get; set; }
        public char priorita { get; set; }
        public int quantita { get; set; }
        public bool erogabile { get; set; }
        public string nota { get; set; }
        public List<Reparto> reparti { get; set; }
    }
}
