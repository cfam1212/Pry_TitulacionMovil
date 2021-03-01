using Pry_TitulacionMovil.Interface;
using Pry_TitulacionMovil.iOS.Implementations;
using SQLite;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(SqLiteClient))]

namespace Pry_TitulacionMovil.iOS.Implementations
{
    public class SqLiteClient : IDataBase
    {
        public SQLiteConnection GetConnection()
        {
            string bbddfile = "ordenestrabajo.db3";
            string rutadocumentos = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string librarypath = Path.Combine(rutadocumentos, "..", "Library", "Databases");
            if (!Directory.Exists(librarypath))
            {
                Directory.CreateDirectory(librarypath);
            }
            string path = Path.Combine(librarypath, bbddfile);
            SQLiteConnection cn = new SQLiteConnection(path);
            return cn;
        }
    }
}