using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MCup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class prova : ContentPage
    {
        public prova()
        {
            InitializeComponent();
            View.Source = "https://drive.google.com/open?id=1dzHONPDf4LjUBr_xN0V7zypSyqwltKXv";
        }
    }
}