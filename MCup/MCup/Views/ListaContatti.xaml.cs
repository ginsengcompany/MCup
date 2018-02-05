using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCup.Model;
using MCup.ModelView;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;
using Platform = Xamarin.Forms.PlatformConfiguration;
namespace MCup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaContatti : ContentPage
    {
        private Assistito contattoPrimo;
        public ListaContatti()
        {
            InitializeComponent();
            BindingContext = new ListaContattiModelView(this);
            
        }
    

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Assistito x = e.Item as Assistito;
            Navigation.PushAsync(new InfoContatto(x)); //Avvia la pagina di registrazione dedicata ai dispositivi Android
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            App.Current.MainPage = new MenuPrincipale();
            return true;
        }
    }
}