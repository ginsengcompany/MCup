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

        private PropostaRichiestaModelView modelView;

        public PropostaRichiesta(List<Prestazioni> prestazioni, Contatto contatto)
        {
            InitializeComponent();
            modelView = new PropostaRichiestaModelView(prestazioni, contatto);
            BindingContext = modelView;
        }
    }
}
