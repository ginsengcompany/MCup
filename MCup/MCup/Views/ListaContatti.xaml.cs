﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
       
}
}