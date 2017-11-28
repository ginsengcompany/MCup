using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using MCup.Database.Models;
using System;

namespace MCup.Database.Data
{
    /*
     * La classe TerminiDiServizio è utilizzata per poter interagire con la classe termini che a sua volta definisce la tabella termini nel database locale
     */
    public static class TerminiDiServizio
    {

        //Il metodo ritorna il numero di elementi nella tabella termini del database
        public static int GetCountTermini()
        {
            int count = Database.Connection.ExecuteScalar<int>("SELECT COUNT(*) FROM TerminiServizio");
            return count;
        }

        //Restituisce il valore del campo accepted nella tabella termini
        public static bool GetTermini()
        {
            return Database.Connection.ExecuteScalar<bool>("SELECT accepted FROM TerminiServizio");
        }

        //Inserisce i termini di servizio nella tabella termini
        public static void InsertTermini(TerminiServizio termini)
        {
            Database.Connection.Insert(termini);
        }

        //Aggiorna la riga della tabella termini
        public static IEnumerable<TerminiServizio> UpdateTermini()
        {
            return Database.Connection.Query<TerminiServizio>("UPDATE TerminiServizio SET accepted = ? WHERE accepted = ?",true,false);
        }
    }
}
