using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppTCC2
{
    public static class Constants
    {
        public const string DatabaseFilename = "PetVaccineDB.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // abrir o banco para leitura e escrita
            SQLite.SQLiteOpenFlags.ReadWrite |
            // criar o banco se não existir
            SQLite.SQLiteOpenFlags.Create |
            // habilitar multi-thread
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    }
}