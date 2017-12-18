using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Model
{
    public class Ricetta
    {
        public string codice_nre { get; set; }
        public string cognome_assistito { get; set; }
        public string nome_assistito { get; set; }
        public string codice_fiscale_medico { get; set; }
        public List<Prestazioni> prestazioni { get; set; }
    }
}
