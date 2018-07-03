using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MCup.Model
{
    public class PickerProperties
    {
        public string title { get; set; }
        public bool enabled { get; set; }
        public ICommand SetFocusPicker {
            get
            {
                return new Command<Picker>((Picker picker) =>
                {
                    if(enabled)
                        picker.Focus();
                });
            }
        }
    }
}
