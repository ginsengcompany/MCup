using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using MCup.Model;
using MCup.Service;
using MCup.ModelView;

namespace MCup.Views
{
    public partial class PropostaRichiesta : ContentPage
    {


        public PropostaRichiesta(List<Prestazioni> prestazioni)
        {
            InitializeComponent();
            BindingContext = new PropostaRichiestaModelView(prestazioni);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            
        }
    }
}
