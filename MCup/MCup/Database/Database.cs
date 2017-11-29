using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using PCLStorage;
using System.Diagnostics;
using MCup.Database.Models;

namespace MCup.Database
{
    public static class Database
    {
        private static string dbname = "dbCup.db";
        private static string dbPath;
        private static ExistenceCheckResult exist;
        private static SQLiteConnection connection {get;set;}
        public static SQLiteConnection Connection
        {
            get
            {
                return connection;
            }
        }

        public static void Initialize()
        {
            //recupero il path del fileSystem del dispositivo
            IFolder rootfolder = FileSystem.Current.LocalStorage;
            dbPath = Path.Combine(rootfolder.Path, dbname);
            connection = new SQLiteConnection(dbPath);
            connection.CreateTable<TerminiServizio>();
            connection.CreateTable<TbStrutturePreferite>();
            connection.CreateTable<Appuntamento>();
            //await CreateDatabase(rootfolder);
        }

        private static async Task CreateDatabase(IFolder folder)
        {
            exist = await folder.CheckExistsAsync(dbname);
            if (exist == ExistenceCheckResult.NotFound)
                using (connection)
                {
                    connection.CreateTable<TerminiServizio>();
                }
        }
    }
}
