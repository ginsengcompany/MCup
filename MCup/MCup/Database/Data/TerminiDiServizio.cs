using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using MCup.Database.Models;
using System;

namespace MCup.Database.Data
{
    public static class TerminiDiServizio
    {
        public static int GetCountTermini()
        {
            int count = Database.Connection.ExecuteScalar<int>("SELECT COUNT(*) FROM TerminiServizio");
            return count;
        }

        public static bool GetTermini()
        {
            return Database.Connection.ExecuteScalar<bool>("SELECT accepted FROM TerminiServizio");
        }

        public static void InsertTermini(TerminiServizio termini)
        {
            Database.Connection.Insert(termini);
        }

        public static IEnumerable<TerminiServizio> UpdateTermini()
        {
            return Database.Connection.Query<TerminiServizio>("UPDATE TerminiServizio SET accepted = ? WHERE accepted = ?",true,false);
        }
    }
}
