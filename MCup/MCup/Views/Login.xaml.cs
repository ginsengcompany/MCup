using System;
using System.Collections.Generic;
using MCup.ModelView;
using Xamarin.Forms;

namespace MCup.Views
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            BindingContext = new LoginModelView();
                  }
    }
}
