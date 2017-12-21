﻿using MCup.ModelView;
using MCup.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MCup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registrazione : ContentPage
    {
        public Registrazione()
        {
            InitializeComponent();
            BindingContext = new RegistrazioneModelView();
        }
    }
}