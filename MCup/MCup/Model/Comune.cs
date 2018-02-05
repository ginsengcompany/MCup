using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Model
{
   public class Comune
   {
       public string nome { get; set; }
       public string codice { get; set; }

       public Comune()
       {
           nome = "";
           codice = "";
       }
   }
}
