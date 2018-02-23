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
    public partial class PaginaAppuntamenti : ContentPage
    {
        private PaginaAppuntamentiModelView form;
        private bool switchApp=false;
        public PaginaAppuntamenti()
        {
            InitializeComponent();
            pickerContatti.SelectedIndex = -1;
            form = new PaginaAppuntamentiModelView(this);
            BindingContext = form;
           
        }
        private void Picker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if(pickerContatti.SelectedIndex==-1)
                return;
            var a = sender as Picker;
            var b = a.SelectedItem as Assistito;
            form.autoCompila(b);
        }

        private void VaiPaginaAppuntamentiIndettaglio(object sender, ItemTappedEventArgs e)
        {
            var b = e.Item as AppuntamentoPrestazioneProposto;
             form.push(b);
        }

       /* protected override void OnAppearing()
        {
            pickerContatti.SelectedIndex = -1;
            form = new PaginaAppuntamentiModelView(this);
            BindingContext = form;
        }*/

        private async void SwitchVisibleAppuntamentiScaduti(object sender, ToggledEventArgs e)
        {

            if (SwitchAppuntamentiPassati.IsToggled)
                form.recuperaDatiAppuntamentiPassati();
            else
            {
              await form.invioDatiAssistito();
            }
        }
    }
}