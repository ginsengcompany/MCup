using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Model
{
    public class AppuntamentoProposto
    {
        public List<AppuntamentoPrestazioneProposto> appuntamenti { get; set; }
        public Assistito assistito { get; set; }
        public string termid { get; set; }

    }
}
