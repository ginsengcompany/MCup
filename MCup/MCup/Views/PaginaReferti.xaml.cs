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
    public partial class PaginaReferti : ContentPage
    {
        private PaginaRefertiModelView form;

        public PaginaReferti()
        {
            InitializeComponent();
            pickerContatti.SelectedIndex = -1;
            form = new PaginaRefertiModelView(this);
            BindingContext = form;
        }

        private void SelezionaContattoPicker(object sender, EventArgs e)
        {
            if (pickerContatti.SelectedIndex == -1)
                return;
            var a = sender as Picker;
            var b = a.SelectedItem as Assistito;
            form.autoCompila(b);
        }

        public void Picker_selezionaPrimoElemento(Assistito assistito)
        {
            pickerContatti.SelectedItem = assistito;
            pickerContatti.IsEnabled = false;
            pickerContatti.Title = assistito.nomeCompletoConCodiceFiscale;
            form.autoCompila(assistito);
        }

        private async void DownloadReferto(object sender, ItemTappedEventArgs e)
        {
            ListaReferti b = e.Item as ListaReferti;
           await form.Download(b);

        }
    }
}