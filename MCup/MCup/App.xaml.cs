using MCup.Views;
using Xamarin.Forms;
using Com.OneSignal;
using MCup.Service;

namespace MCup
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new Login());
            //MainPage = new NavigationPage(new MenuPrincipale());
          /*  try
            {
                if (Application.Current.Properties["flagRimaniLoggato"].ToString().Equals("True")) //controlla se il device su cui l'app viene avviata è IOS o Android
                {
                    MainPage = new NavigationPage(new MenuPrincipale()); //Avvia la pagina di login per i dispositivi IOS
                }
                else
                    MainPage = new NavigationPage(new Login()); //Avvia la pagina di login per i dispositivi Android
            }
            catch (Exception)
            {
                Application.Current.Properties.Add("flagRimaniLoggato", "False");
                MainPage = new NavigationPage(new Login());
            }*/
            OneSignal.Current.StartInit("821d395a-09ed-48a4-81b8-4a79971452eb").EndInit();
        }

        protected override void OnStart()
        {
            SingletonURL.Instance.prelevaRotte();
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
