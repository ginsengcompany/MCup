using MCup.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MCup.Model
{
    public static class MessaggioConnessione
    {
      static string messaggio = "Connessione Assente o servizio momentaneamente non disponibile";
        public static async Task displayAlert() {
            await App.Current.MainPage.DisplayAlert("ATTENZIONE", messaggio, "OK");
            App.Current.MainPage = new Login();
        }
        public static async Task displayAlert(string messaggioNuovo)
        {
            await App.Current.MainPage.DisplayAlert("ATTENZIONE", messaggioNuovo, "OK");
            App.Current.MainPage = new Login();
        }
        public static async Task displayAlert(int i, string mes)
        {
            if((i!=0)&&(i!=408))
             await App.Current.MainPage.DisplayAlert("ATTENZIONE" + i, mes, "OK");
            else
            {
                await App.Current.MainPage.DisplayAlert("ATTENZIONE" + i, messaggio, "OK");
                 App.Current.MainPage = new NavigationPage( new Login());
            }
        }
    }

}
