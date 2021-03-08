namespace Pry_TitulacionMovil.ViewModels
{
    using Helpers;
    using Models;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class OrdersPendienteViewModel : BaseViewModel
    {
        #region Services
        private DataAccess dataAccess;
        #endregion

        #region Attributes
        private ObservableCollection<Orden> pendienteorders;
        private List<Orden> listaorders;
        private string filter;
        #endregion

        #region Properties
        public ObservableCollection<Orden> PendienteOrders
        {
            get { return pendienteorders; }
            set { this.SetValue(ref pendienteorders, value); }
        }

        public string Filter
        {
            get { return filter; }
            set
            {
                this.SetValue(ref filter, value);
                this.Buscar();
            }
        }
        #endregion

        #region Constructor
        public OrdersPendienteViewModel()
        {
            this.dataAccess = new DataAccess();
            this.CargarOrdenes();
        }
        #endregion

        #region Metodos
        private void CargarOrdenes()
        {
            this.listaorders = dataAccess.GetOrdenesTrabajo();
            this.PendienteOrders = new ObservableCollection<Orden>(this.listaorders.Where(l => l.OrdenEstado == "PRO"));

        }
        private void Buscar()
        {
            if (string.IsNullOrEmpty(Filter))
            {
                this.PendienteOrders = new ObservableCollection<Orden>(
                    this.listaorders.Where(l => l.OrdenEstado == "PRO"));
            }
            else
            {
                this.PendienteOrders = new ObservableCollection<Orden>(
                    this.listaorders.Where(l => l.Cliente.ToLower().Contains(
                        this.Filter.ToLower()) || l.IdOrden.ToString().Contains(
                            this.Filter)));
            }
        } 
        #endregion
    }
}
