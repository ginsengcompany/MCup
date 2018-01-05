using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCup.Model;
using MCup.ModelView;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MCup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaContatti : ContentPage
    {
        public ListaContatti()
        {
            InitializeComponent();
            BindingContext = new ListaContattiModelView();
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Contatto x = e.Item as Contatto;
            Navigation.PushAsync(new InfoContatto(x)); //Avvia la pagina di registrazione dedicata ai dispositivi Android
        }
    }
}