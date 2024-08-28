using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.Shapes;

namespace BarbeariaApp.Model
{
    public class Connection
    {
        public static SQLiteAsyncConnection db;

        public static async Task Open()
        {
            if (db != null) { return; }

            string dbPath = "";
            if (Preferences.Get("CaminhoBD","") != "")
            {
                dbPath = Preferences.Get("CaminhoBD", "");
            }
            else
            {
                dbPath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "db.db");
            }
            db = new SQLiteAsyncConnection(dbPath);

            await db.CreateTableAsync<Produto>();
            await db.CreateTableAsync<Servico>();
            await db.CreateTableAsync<Agendamento>();
            await db.CreateTableAsync<Atendimento>();
        }
    }
}
