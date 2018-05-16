using MCup.Model;
using System.Collections.Generic;

namespace MCup.Service
{
    sealed class SingletonURL
    {
        private static SingletonURL instance = null;
        public static string errorePrelievoRotte = "";
        public static ListaURL rotte = new ListaURL();

        private SingletonURL()
        {
        }

        public ListaURL getRotte()
        {
            return rotte;
        }

        public async void prelevaRotte()
        {
            ListaURL dati = new ListaURL();
            REST<ListaURL, ListaURL> connessione = new REST<ListaURL, ListaURL>();
            List<Header> headers = new List<Header>();
            headers.Add(new Header("codice_struttura", "150907"));
            var response = await connessione.PostJson("http://192.168.125.24:3001/urlserviziapp", dati, headers);
            if (connessione.responseMessage != System.Net.HttpStatusCode.OK)
            {
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