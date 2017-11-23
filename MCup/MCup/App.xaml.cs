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
        private bool acceptTermini;

        private bool strutturaScelta;

        public App()
        {
            InitializeComponent();
            Database.Database.Initialize();
            checkTermini();
        }

        private void checkTermini()
        {
            acceptTermini = checkTerminiServizio();
            if (acceptTermini == false)
                MainPage = new Termini();
            else
            {
                strutturaScelta = checkStrutturaScelta();
                if (strutturaScelta)
                    MainPage = new NavigationPage(new MenuPrincipale());
                else
                    MainPage = new NavigationPage(new ListaStrutture());
            }
                
        }

        private bool checkTerminiServizio()
        {
            int count = TerminiDiServizio.GetCountTermini();
            if (count > 0)
            {
                return TerminiDiServizio.GetTermini();
            }
            else
            {
                TerminiServizio termini = new TerminiServizio();
                TerminiDiServizio.InsertTermini(termini);
                return false;
            }
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
