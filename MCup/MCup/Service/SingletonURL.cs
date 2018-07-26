using MCup.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MCup.Service
{
    sealed class SingletonURL
    {
        private static SingletonURL instance = null;
        public static string errorePrelievoRotte = "";
        public  bool error = false;
        public static ListaURL rotte = new ListaURL();

        private SingletonURL()
        {
        }

        public ListaURL getRotte()
        {
            return rotte;
        }

        public async Task prelevaRotte()
        {
            REST<ListaURL, ListaURL> connessione = new REST<ListaURL, ListaURL>();
            List<Header> headers = new List<Header>();
            headers.Add(new Header("codice_struttura", "150907"));
            var response = await connessione.GetSingleJson("http://ecuptservice.ak12srl.it/urlserviziapp", headers);
            if (connessione.responseMessage != System.Net.HttpStatusCode.OK)
            {
                error = true;
                await App.Current.MainPage.DisplayAlert("Attenzione", connessione.warning, "OK");
                rotte = null;
            }
            else
                rotte = response;
        } 

        static internal SingletonURL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SingletonURL();
                }
                return instance;
            }
        }
    }
}