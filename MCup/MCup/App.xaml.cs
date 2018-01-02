using MCup.Database.Data;
using MCup.Database.Models;
using MCup.Model;
using MCup.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MCup
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS) //controlla se il device su cui l'app viene avviata è IOS o Android
            {
                MainPage = new NavigationPage(new LoginIoS()); //Avvia la pagina di login per i dispositivi IOS
            }else
                MainPage = new NavigationPage(new Login()); //Avvia la pagina di login per i dispositivi Android
            Database.Database.Initialize(); //Inizializza il database al primo avvio dell'app, altrimenti crea i riferimenti per la sua gestione
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
