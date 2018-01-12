using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Model
{
    public class Province
    {
        public string Capoluogo { get; set; }
        public string Provincia { get; set; }

        public Province(string capo, string prov)
        {
            this.Capoluogo = capo;
            this.Provincia = prov;
        }

    }

}

