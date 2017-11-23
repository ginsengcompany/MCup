using System.Collections.Generic;
using MCup.Database.Models;

namespace MCup.Database.Data
{
    public static class StrutturePreferite
    {
        public static int GetCountStrutturePreferite()
        {
            int count = Database.Connection.ExecuteScalar<int>("SELECT COUNT(*) FROM TbStrutturePreferite");
            return count;
        }

        public static void InserisciStrutturaPreferita(TbStrutturePreferite struttura)
        {
            Database.Connection.Insert(struttura);
        }

        public static List<TbStrutturePreferite> VisualizzaStrutturePreferite()
        {
            return Database.Connection.Query<TbStrutturePreferite>("SELECT * FROM TbStrutturePreferite");
        }

        public static List<TbStrutturePreferite> PrelevaIdStruttura(string id)
        {
            return Database.Connection.Query<TbStrutturePreferite>("SELECT * FROM TbStrutturePreferite WHERE id = ?", id);
        }

        public static IEnumerable<TbStrutturePreferite> UpdateStrutturaPreferita(string nomeStruttura, string id)
        {
            return Database.Connection.Query<TbStrutturePreferite>("UPDATE TbStrutturePreferite SET NomeStruttura = ?, id = ? WHERE id = ?; ", nomeStruttura, id, id);
        }
    }
}




