using System;
using System.Collections.Generic;
using System.ComponentModel;
using MCup.ModelView;
using Xamarin.Forms;

/*
 * Questa pagina viene visualizzata all'avvio dell'app solo se il dispositivo è di tipo IOS. Questa pagina gestisce l'autenticazione dell'utente
 * per poter accedere alla sua area privata e quindi utilizzare i servizi che l'app fornisce.
 */

namespace MCup.Views
{
    public partial class RegistrazioneIOS : ContentPage
    {
        public RegistrazioneIOS()
        {
            InitializeComponent();
            BindingContext = new RegistrazioneModelView(); //Questa pagina utilizza l'MWWM ed effettua il binding con la classe LoginModelView
        }
    }
}
