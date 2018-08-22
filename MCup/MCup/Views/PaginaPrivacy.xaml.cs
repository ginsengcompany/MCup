using MCup.ModelView;
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
    public partial class PaginaPrivacy : ContentPage
    {

        private PaginaPrivacyModelView model;
        public PaginaPrivacy()
        {
            InitializeComponent();
            model = new PaginaPrivacyModelView();
            BindingContext = model;
        }
    }
}