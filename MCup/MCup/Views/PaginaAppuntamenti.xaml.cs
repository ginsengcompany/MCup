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

        public PaginaAppuntamenti()
        {
            InitializeComponent();
            
        }
        private void Picker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var a = sender as Picker;
            var b = a.SelectedItem as Assistito;
            form.autoCompila(b);
        }

        private void VaiPaginaAppuntamentiIndettaglio(object sender, ItemTappedEventArgs e)
        {
            var b = e.Item as AppuntamentoProposto;
             form.push(b);
        }

        protected override void OnAppearing()
        {
            pickerContatti.SelectedIndex = -1;
            form = new PaginaAppuntamentiModelView(this);
            BindingContext = form;
        }
    }
}