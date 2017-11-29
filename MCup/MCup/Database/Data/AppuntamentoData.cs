using MCup.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Database.Data
{
    public static class AppuntamentoData
    {
        public static int GetCountAppuntamenti()
        {
            int count = Database.Connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Appuntamento");
            return count;
        }

        public static List<Appuntamento> GetAppuntamenti()
        {
            return Database.Connection.Query<Appuntamento>("SELECT * FROM Appuntamento");
        }

        public static void InsertAppuntamento(Appuntamento appuntamento)
        {
            Database.Connection.Insert(appuntamento);
        }

        /*
        public static IEnumerable<Appuntamento> UpdateTermini(Appuntamento appuntamento)
        {
            return Database.Connection.Query<Appuntamento>("UPDATE Appuntamento SET accepted = ? WHERE accepted = ?", true, false);
        }
        */
    }
}
