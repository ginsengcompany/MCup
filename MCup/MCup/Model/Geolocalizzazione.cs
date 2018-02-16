using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Model
{
    public class Geolocalizzazione
    {
        public string latitudine { get; set; }

        public string longitudine { get; set; }
        public bool visibile { get; set; } = true;
    }
}
