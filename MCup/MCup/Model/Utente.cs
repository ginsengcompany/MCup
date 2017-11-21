using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Model
{
    public class Utente
    {
        private string codicefiscale { get; set; }
        private string nome { get; set; }
        private string cognome { get; set; }
        public string CodiceFiscale
        {
            get { return codicefiscale; }
            set { codicefiscale = value; }
        }
        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }
        public string Cognome
        {
            get { return cognome; }
            set { cognome = value; }
        }
    }
}
