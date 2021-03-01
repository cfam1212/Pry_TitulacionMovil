namespace Pry_TitulacionMovil.Helpers
{
    using Models;
    using Interface;
    using SQLite;
    using System.Linq;
    using System;
    using Xamarin.Forms;
    using System.Collections.Generic;

    public class DataAccess : IDisposable
    {
        #region Propiedades
        private SQLiteConnection connection;
        #endregion

        #region Constructor
        public DataAccess()
        {
            this.connection = DependencyService.Get<IDataBase>().GetConnection();

            connection.CreateTable<UserLocal>();
            connection.CreateTable<Orden>();
        }
        #endregion

        #region Methods
        public void Insert<T>(T model)
        {
            this.connection.Insert(model);
        }

        public void Update<T>(T model)
        {
            this.connection.Update(model);
        }

        public void Delete<T>(T model)
        {
            this.connection.Delete(model);
        } 
        #endregion


        #region Metodos
        public List<Orden> GetOrdenesTrabajo()
        {
            var consulta = from datos in connection.Table<Orden>()
                           select datos;

            return consulta.ToList();
        }
        #endregion

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
