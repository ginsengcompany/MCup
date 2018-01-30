﻿using System;
using System.Collections.Generic;
using MCup.ModelView;
using Xamarin.Forms;

/*
 * Questa pagina viene visualizzata all'avvio dell'app solo se il dispositivo è Android. Questa pagina gestisce l'autenticazione dell'utente
 * per poter accedere alla sua area privata e quindi utilizzare i servizi che l'app fornisce.
 */

namespace MCup.Views
{
    public partial class Login : ContentPage
    {
        private Boolean flaglogin = false;
        private LoginModelView z = new LoginModelView();
        public Login()
        {
            InitializeComponent();
            
            BindingContext = z; //Questa pagina utilizza l'MWWM ed effettua il binding con la classe LoginModelView

        }

        /*
         * Questo metodo viene chiamato quando l'utente clicca sulla label che fa riferimento alla fase di registrazione
         */
        private async void vaiRegistrazione(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Registrazione()); //Avvia la pagina di registrazione dedicata ai dispositivi Android
        }

        private async void Switch_OnToggled(object sender, ToggledEventArgs e)
        {
            if (e.Value)
            {
                flaglogin = true;
               
            }
            else
            {
                flaglogin = false;
            }
            z.modificaFlag(flaglogin);
        }
    }
}
