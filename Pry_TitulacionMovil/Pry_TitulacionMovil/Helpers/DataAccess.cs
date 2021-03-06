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
            connection.CreateTable<Marca>();
            connection.CreateTable<Modelo>();
            connection.CreateTable<ListaTrabajo>();
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

        public List<Marca> GetMarcas()
        {
            var consulta = from datos in connection.Table<Marca>()
                           select datos;

            return consulta.ToList();
        }

        public List<Modelo> GetModelos()
        {
            var consulta = from datos in connection.Table<Modelo>()
                           select datos;

            return consulta.ToList();
        }

        public List<ListaTrabajo> GetListaTrabajo()
        {
            var consulta = from datos in connection.Table<ListaTrabajo>()
                           select datos;

            return consulta.ToList();
        }
        public UserLocal BuscarUsuario(int iduser)
        {
            var consulta = from datos in connection.Table<UserLocal>()
                           where datos.UserId == iduser
                           select datos;

            return consulta.FirstOrDefault();
        }
        #endregion

        #region CloseBase
        public void Dispose()
        {
            connection.Dispose();
        } 
        #endregion
    }
}
