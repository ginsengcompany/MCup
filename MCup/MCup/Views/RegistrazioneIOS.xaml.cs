using System;
using System.Collections.Generic;
using System.ComponentModel;
using MCup.ModelView;
using Xamarin.Forms;

namespace MCup.Views
{
    public partial class RegistrazioneIOS : ContentPage
    {
        public RegistrazioneIOS()
        {
            InitializeComponent();
            BindingContext = new RegistrazioneModelView();

        }
   
    }
}
