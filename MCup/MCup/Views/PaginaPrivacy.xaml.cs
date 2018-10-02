using MCup.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCup.CustomPopUp;
using MCup.Service;
using Rg.Plugins.Popup.Extensions;
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
            model = new PaginaPrivacyModelView(this);
            BindingContext = model;
        }

        public async Task confermaEliminaAccount()
        {
            if (SingletonURL.Instance.error)
                await DisplayAlert("Attenzione", "server momentaneamente non disponibile", "OK");
            else
                await Navigation.PushPopupAsync(new PopupInfoScan(true));
        }
    }
}