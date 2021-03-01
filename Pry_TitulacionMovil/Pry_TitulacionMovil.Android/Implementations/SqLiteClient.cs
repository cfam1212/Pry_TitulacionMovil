using Pry_TitulacionMovil.Droid.Implementations;
using Pry_TitulacionMovil.Interface;
using SQLite;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(SqLiteClient))]

namespace Pry_TitulacionMovil.Droid.Implementations
{
    public class SqLiteClient : IDataBase
    {
        public SQLiteConnection GetConnection()
        {
            string bbddfile = "ordenestrabajo.db3";
            string rutadocumentos = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string path = Path.Combine(rutadocumentos, bbddfile);
            SQLiteConnection cn = new SQLiteConnection(path);
            return cn;
        }
    }
}