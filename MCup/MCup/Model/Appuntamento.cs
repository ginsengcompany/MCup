using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Model
{
    public class Appuntamento
    {
        public PrenotazioneProposta prestazione { get; set; }
        public Contatto assistito { get; set; }
    }
}
