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
        public bool repartoScelto { get; set; }
        public string orarioApertura { get; set; }
        public string orarioChiusura { get; set; }
        public int defaultReparto { get; set; }
        public string nomeMedico { get; set; }
        public string ubicazioneReparto { get; set; }
        public string ubicazioneUnita { get; set; }
    }
}
