using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Model
{
   public class Erogazione
    {
        public List<Prestazioni> prestazioni { get; set; }
        public string struttura { get; set; } = "030001";

        public Erogazione(Erogazione eroga)
        {
            this.prestazioni = eroga.prestazioni;
            this.struttura = eroga.struttura;
        }

        public Erogazione()
        {
            this.prestazioni= new List<Prestazioni>();

        }
    }
}
