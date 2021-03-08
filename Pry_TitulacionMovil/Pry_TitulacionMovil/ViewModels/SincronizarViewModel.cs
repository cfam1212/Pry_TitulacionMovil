namespace Pry_TitulacionMovil.ViewModels
{
    using Helpers;
    using Services;
    using System.Linq;
    using Xamarin.Forms;
    using Views;
    using System.Threading.Tasks;

    public class SincronizarViewModel : BaseViewModel
    {
        #region Atributos
        private double progressBar;
        private int totalsubido;
        private bool isRunning;
        #endregion

        #region Services
        private ApiService apiService;
        private DataService dataService;
        private DataAccess dataAccess;
        #endregion

        #region Properties
        public double ProgressBar
        {
            get { return this.progressBar; }
            set { SetValue(ref this.progressBar, value); }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }
        #endregion

        #region Constructor
        public SincronizarViewModel()
        {
            this.apiService = new ApiService();
            this.dataService = new DataService();
            this.dataAccess = new DataAccess();
            this.SubirOrdenes();
        }
        #endregion

        #region Metodos
        private async void SubirOrdenes()
        {
            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                IsRunning = false;

                await Application.Current.MainPage.DisplayAlert(
                    "Mensaje",
                    "Sin Conexión, Grabada Localmente",
                    "OK");
            }
            else
            {
                var listOrdersPendiente = this.dataAccess.GetOrdersPendientes();
                var total = listOrdersPendiente.Count();

                if (total > 0)
                {
                    foreach (var item in listOrdersPendiente)
                    {
                        this.ProgressBar = this.ProgressBar + (1.0 / total);
                        //this.SyncUp(itemlist.OtcaCodigo);

                        var ordencabecera = this.dataAccess.GetOrdenesLocal(item.IdOrden);
                        var ordendetalle = this.dataAccess.GetOrdenDetalleLocal(item.IdOrden);

                        var ordencabedeta = Converter.ToOrdenCabeDeta(ordencabecera, ordendetalle);
                        var apiService = Application.Current.Resources["APIServices"].ToString();

                        var response = await this.apiService.GrabarOrden(
                            apiService,
                            "api",
                            "ordencabedeta",
                            ordencabedeta);

                        if (response.IsSuccess)
                        {
                            this.dataService.Delete(ordencabecera);

                            foreach (var itemorder in ordendetalle)
                            {
                                this.dataService.Delete(itemorder);
                            }

                            this.totalsubido = this.totalsubido + 1;
                        }

                        await Task.Delay(5000);
                    }

                    if (this.totalsubido == total)
                    {
                        await Application.Current.MainPage.DisplayAlert(
                            "Información",
                            "Sincronizado Total",
                            "OK");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(
                            "Error",
                            "Sin Conexión, Grabada Localmente",
                            "OK");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Información",
                        "No Existen Ordenes Pendientes",
                        "OK");
                }
            }

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Orders = new OrdersViewModel();
            Application.Current.MainPage = new MasterPage();
        } 
        #endregion
    }
}
