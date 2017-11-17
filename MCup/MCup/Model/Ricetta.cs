using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Model
{
    public class Ricetta
    {
        private string regione { get; set; }
        private string codice_uno { get; set; }
        private string codice_due { get; set; }
        private string indirizzo { get; set; }
        private string cap { get; set; }
        private string comune { get; set; }
        private string provincia { get; set; }
        private string esenzione { get; set; }
        private string codice_asl { get; set; }
        private string tipologia_prescrizione { get; set; }
        private string altro { get; set; }
        private string priorita_prescrizione { get; set; }
        private string disposizioni_regionali { get; set; }
        private List<string> prestazione { get; set; }
        private string quesito_diagnostico { get; set; }
        private int numero_prestazioni { get; set; }
        private string tipo { get; set; }
        private string data { get; set; }
        private string cod_fisc_medico { get; set; }
        private string codice_autenticazione { get; set; }
        private string nome_medico { get; set; }
        private string cognome_medico { get; set; }
    }
}
