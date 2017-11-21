using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using MCup.Database.Models;
using System;

namespace MCup.Database.Data
{
    public static class StrutturaPreferitaQuery
    {
        // TO DO

        public static int GetStrutturaPreferita()
        {
            int count = Database.Connection.ExecuteScalar<int>("SELECT COUNT(*) FROM TerminiServizio");
            return count;
        }

        public static bool getStrutturaPreferita()
        {
            return Database.Connection.ExecuteScalar<bool>("SELECT accepted FROM TerminiServizio");
        }

        public static void InsertStrutturaPreferita(TerminiServizio termini)
        {
            Database.Connection.Insert(termini);
        }

        public static IEnumerable<StrutturaPreferita> UpdateTermini()
        {
            return Database.Connection.Query<StrutturaPreferita>("UPDATE TerminiServizio SET accepted = ? WHERE accepted = ?", true, false);
        }
    }
}
