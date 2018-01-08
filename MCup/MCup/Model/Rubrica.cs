using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Model
{
    public class Rubrica : ObservableCollection<Contatto>
    {
        public string LongName { get; set; }
        public string ShortName { get; set; }
        public Rubrica(string LongName, string ShortName)
        {
            this.LongName = LongName;
            this.ShortName = ShortName;
        }
    }
}
