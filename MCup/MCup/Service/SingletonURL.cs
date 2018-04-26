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
            headers.Add(new Header("codice_struttura", "150021"));
            rotte = await connessione.PostJson("http://192.168.125.14:3000/urlserviziapp", dati, headers);
            if (rotte.codiceErrore == "codice struttura non inviato"
                || rotte.codiceErrore == "il servizio non è momentaneamente disponibile"
                || rotte.codiceErrore == "struttura non trovata")
            {
                errorePrelievoRotte = rotte.codiceErrore;
                rotte = null;
            }
            else
                errorePrelievoRotte = "";
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