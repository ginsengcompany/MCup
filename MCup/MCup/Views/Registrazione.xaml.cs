using MCup.ModelView;
using MCup.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

/*
 * La corrente pagina viene visualizzata solo se il dispositivo su cui si esegue l'app è di tipo Android. 
 * Questa Pagina permette all'utente di poter registrare una nuova utenza per poter poi effettuare la login
 * per accedere ai servizi.
 */

namespace MCup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registrazione : ContentPage
    {
        public Registrazione()
        {
            InitializeComponent();
            BindingContext = new RegistrazioneModelView(); //Questa pagina utilizza l'MWWM, ed effettua il binding delle informazioni con la classe RegistrazioneModelView.
        }
    }
}