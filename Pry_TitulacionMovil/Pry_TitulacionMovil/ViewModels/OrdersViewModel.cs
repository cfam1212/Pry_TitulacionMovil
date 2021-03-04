namespace Pry_TitulacionMovil.ViewModels
{
    using Models;
    using Services;
    using System;
    using System.Linq;
    using Helpers;
    using System.Collections.Generic;
    using Xamarin.Forms;
    using System.Collections.ObjectModel;

    public class OrdersViewModel: BaseViewModel
    {
        #region Services
        private ApiService apiService;
        private DataService dataService;
        #endregion

        #region Attributes
        private ObservableCollection<OrderItemViewModel> neworders;
        private List<Orden> listorders;
        private bool isRefreshing;
        private string filter;
        private bool isContinued;
        #endregion

        #region Properties
        public ObservableCollection<OrderItemViewModel> NewOrders
        {
            get { return neworders; }
            set { this.SetValue(ref neworders, value); }
        }

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { this.SetValue(ref isRefreshing, value); }
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

        public bool IsContinued
        {
            get { return isContinued; }
            set
            {
                this.SetValue(ref isContinued, value);
            }
        }

        #endregion

        #region Constructor
        public OrdersViewModel()
        {
            this.apiService = new ApiService();
            this.dataService = new DataService();
            this.IsRefreshing = false;
            this.CargarOrdenes();
        }
        #endregion

        #region Metodos
        private async void CargarOrdenes()
        {
            this.IsRefreshing = true;
            this.IsContinued = false;

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsContinued = false;
                return;
            }
            else
            {
                this.IsContinued = true;
            }

            var mainViewModel = MainViewModel.GetInstance();
            var tecnico = mainViewModel.userId;


            if (this.IsContinued)
            {
                var apiService = Application.Current.Resources["APIServices"].ToString();

                var orders = await this.apiService.GetListPorTecnico<Orden>(
                    apiService,
                    "api",
                    "ordenes",
                    tecnico);

                if (orders.Result != null)
                {
                    this.listorders = (List<Orden>)(orders.Result);

                    if (listorders.Count > 0)
                    {
                        foreach (var itemorder in listorders)
                        {
                            var neworder = new Orden()
                            {
                                IdOrden = itemorder.IdOrden,
                                IdEquipo = itemorder.IdEquipo,
                                OrdenEstado = itemorder.OrdenEstado,
                                Cliente = itemorder.Cliente,
                                FechaInicioOT = itemorder.FechaInicioOT,
                                FechaFinalOT = itemorder.FechaFinalOT,
                                TipoTrabajo = itemorder.TipoTrabajo,
                                ProblemaEquipo = itemorder.ProblemaEquipo,
                                Notas = itemorder.Notas,
                                Equipo = itemorder.Equipo,
                                MarcaId = itemorder.MarcaId,
                                ModeloId = itemorder.ModeloId,
                                Voltaje = itemorder.Voltaje,
                                Amperaje = itemorder.Amperaje,
                                Presion = itemorder.Presion,
                                FechaInicioTR = itemorder.FechaInicioTR,
                                FechaFinalTR = itemorder.FechaFinalTR,
                                Observacion = itemorder.Observacion,
                                ImagenTR = itemorder.ImagenTR,
                                RutaImagen = itemorder.RutaImagen,
                                Latitud = itemorder.Latitud,
                                Logintud = itemorder.Logintud,
                            };
                            this.dataService.Insert(neworder);
                        }
                    }
                }
            }

            this.IsRefreshing = false;
            this.listorders = new DataAccess().GetOrdenesTrabajo();
            this.NewOrders = new ObservableCollection<OrderItemViewModel>(this.ToOrderItemViewModel().Where(l => l.OrdenEstado == "INI"));
        }

        private IEnumerable<OrderItemViewModel> ToOrderItemViewModel()
        {
            return this.listorders.Select(lista => new OrderItemViewModel
            {
                IdOrden = lista.IdOrden,
                IdEquipo = lista.IdEquipo,
                OrdenEstado = lista.OrdenEstado,
                Cliente = lista.Cliente,
                FechaInicioOT = lista.FechaInicioOT,
                FechaFinalOT = lista.FechaFinalOT,
                TipoTrabajo = lista.TipoTrabajo,
                ProblemaEquipo = lista.ProblemaEquipo,
                Notas = lista.Notas,
                Equipo = lista.Equipo,
                MarcaId = lista.MarcaId,
                ModeloId = lista.ModeloId,
                Voltaje = lista.Voltaje,
                Amperaje = lista.Amperaje,
                Presion = lista.Presion,
                FechaInicioTR = lista.FechaInicioTR,
                FechaFinalTR = lista.FechaFinalTR,
                Observacion = lista.Observacion,
                ImagenTR = lista.ImagenTR,
                RutaImagen = lista.RutaImagen,
                Latitud = lista.Latitud,
                Logintud = lista.Logintud,
            });
        }

        private void Buscar()
        {
            if (string.IsNullOrEmpty(Filter))
            {
                this.NewOrders = new ObservableCollection<OrderItemViewModel>(
                    this.ToOrderItemViewModel());
            }
            else
            {
                this.NewOrders = new ObservableCollection<OrderItemViewModel>(
                    this.ToOrderItemViewModel().Where(l => l.Cliente.ToLower().Contains(
                        this.Filter.ToLower()) || l.IdOrden.ToString().Contains(
                            this.Filter)));
            }
        } 
        #endregion
    }
}
