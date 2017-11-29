using MCup.Database.Data;
using MCup.Database.Models;
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

        private List<Appuntamento> listAppuntamenti = new List<Appuntamento>();

        public List<Appuntamento> ListAppuntamenti
        {
            get
            {
                return listAppuntamenti;
            }
            set
            {
                listAppuntamenti = value;
                OnPropertyChanged();
            }
        }

        public GestioneAppuntamentiModelView()
        {
            ListAppuntamenti = AppuntamentoData.GetAppuntamenti();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
