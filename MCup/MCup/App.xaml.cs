using MCup.Database.Data;
using MCup.Database.Models;
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

        private bool strutturaScelta;

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new Login());
            Database.Database.Initialize();
        }

        private bool checkStrutturaScelta()
        {
            int count = StrutturePreferite.GetCountStrutturePreferite();
            if (count > 0)
                return true;
            else
                return false;
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
