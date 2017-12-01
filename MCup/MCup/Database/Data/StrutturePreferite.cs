using System.Collections.Generic;
using MCup.Database.Models;

namespace MCup.Database.Data
{
    /*
     * La classe StrutturePreferite è utilizzata per interagire con la classe TbStrutturePreferite che astrae a sua volta la tabella TbStrutturePreferite del database locale
     */
    public static class StrutturePreferite
    {
        //Il metodo restituisce il numero di righe nella tabella TbStrutturePreferite
        public static int GetCountStrutturePreferite()
        {
            int count = Database.Connection.ExecuteScalar<int>("SELECT COUNT(*) FROM TbStrutturePreferite");
            return count;
        }

        //Il metodo inserisce una riga all'interno della tabella TbStrutturePreferite
        public static void InserisciStrutturaPreferita(TbStrutturePreferite struttura)
        {
            Database.Connection.Insert(struttura);
        }

        //Il metodo restituisce le strutture preferite scelte dall'utente
        public static List<TbStrutturePreferite> VisualizzaStrutturePreferite()
        {
            return Database.Connection.Query<TbStrutturePreferite>("SELECT * FROM TbStrutturePreferite");
        }

        //Il metodo restituisce tutte le righe con l'id passato come parametro
        public static List<TbStrutturePreferite> PrelevaIdStruttura(string id)
        {
            return Database.Connection.Query<TbStrutturePreferite>("SELECT * FROM TbStrutturePreferite WHERE id = ?", id);
        }

        public static List<TbStrutturePreferite> PrelevaIdStruttura()
        {
            return Database.Connection.Query<TbStrutturePreferite>("SELECT * FROM TbStrutturePreferite");
        }

        //Il metodo aggiorna le informazioni nella tabella TbStrutturePreferite con i parametri passati per valore
        public static IEnumerable<TbStrutturePreferite> UpdateStrutturaPreferita(string nomeStruttura, string id)
        {
            var x = getIdStruttura();
            return Database.Connection.Query<TbStrutturePreferite>("UPDATE TbStrutturePreferite SET NomeStruttura = ?, id = ? WHERE id = ?", nomeStruttura, id, x);
        }

        public static string getIdStruttura()
        {
            return Database.Connection.ExecuteScalar<string>("SELECT id FROM TbStrutturePreferite");
        }
    }
}




