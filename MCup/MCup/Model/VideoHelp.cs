using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Model
{
    public class VideoHelp
    {
        public String nome {
            get
            {
                return Nome;
            }
            set
            {
                Nome = value;
            }
        }
        public String link
        {
            get
            {
                return Link;
            }
            set
            {
                Link = value;
            }
        }
        public String immagine
        {
            get
            {
                return Immagine;
            }
            set
            {
                Immagine = value;
            }
        }

        private String Nome { get; set; }
        private String Link { get; set; }
        private String Immagine { get; set; }
    }
}
