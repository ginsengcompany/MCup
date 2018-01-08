using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MCup.ModelView
{
    public class GestioneAppuntamentiModelView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        

        public GestioneAppuntamentiModelView()
        {
            
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
