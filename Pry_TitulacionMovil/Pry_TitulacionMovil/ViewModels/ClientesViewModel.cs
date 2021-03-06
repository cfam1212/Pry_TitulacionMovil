namespace Pry_TitulacionMovil.ViewModels
{
    using Models;
    using System.Linq;
    using System;
    using System.Collections.ObjectModel;
    using Services;
    using Helpers;
    using Plugin.Media;
    using System.Collections.Generic;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;
    using Plugin.Media.Abstractions;
    using Xamarin.Essentials;

    public class ClientesViewModel : BaseViewModel
    {
        #region Atributos
        public Orden Cliente { get; set; }
        private List<Marca> listamarcas;
        private List<Modelo> listamodelos;
        private List<ListaTrabajo> listatrabajos;
        private ObservableCollection<Marca> marcas;
        private ObservableCollection<Modelo> modelos;
        private ObservableCollection<ListaTrabajo> trabajo;
        private DataService dataService;
        private DataAccess dataAccess;
        private int marcaindex;
        private int modeloindex;
        private bool isRefreshing;
        private bool isChecked;
        private bool isEneabled;
        private int itemseleccionado;
        private string filter;
        private ImageSource imagenFuente;
        private MediaFile file;
        private string latitud;
        private string longitud;
        private string fechainiciotr;
        #endregion

        #region Propiedades
        public ObservableCollection<Marca> Marcas
        {
            get { return marcas; }
            set { this.SetValue(ref marcas, value); }
        }

        public ObservableCollection<Modelo> Modelos
        {
            get { return modelos; }
            set { this.SetValue(ref modelos, value); }
        }

        public ObservableCollection<ListaTrabajo> Trabajo
        {
            get { return trabajo; }
            set { this.SetValue(ref trabajo, value); }
        }

        public int MarcaIndex
        {
            get { return marcaindex; }
            set { this.SetValue(ref marcaindex, value); }
        }
        public int ModeloIndex
        {
            get { return modeloindex; }
            set { this.SetValue(ref modeloindex, value); }
        }
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { this.SetValue(ref isRefreshing, value); }
        }
        public bool IsChecked
        {
            get { return isChecked; }
            set { this.SetValue(ref isChecked, value); }
        }
        public bool IsEneabled
        {
            get { return isEneabled; }
            set { this.SetValue(ref isEneabled, value); }
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
        public ImageSource ImagenFuente
        {
            get { return this.imagenFuente; }
            set { SetValue(ref this.imagenFuente, value); }
        }
        public string Latitud
        {
            get { return latitud; }
            set { this.SetValue(ref latitud, value); }
        }
        public string Longitud
        {
            get { return longitud; }
            set { this.SetValue(ref longitud, value); }
        }
        #endregion

        #region Constructor
        public ClientesViewModel(Orden cliente)
        {
            this.dataService = new DataService();
            this.dataAccess = new DataAccess();
            this.Cliente = cliente;
            this.CargarMarca();
            this.CargarModelo();
            this.CargarListaTrabajo();
            this.MarcaIndex = this.GetMarcaIndex();
            this.ModeloIndex = this.GetModeloIndex();
            this.ImagenFuente = "camara";
            this.IsEneabled = true;
            this.fechainiciotr = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }
        #endregion

        #region Commandos
        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(this.Buscar);
            }
        }
        public ICommand TomarFotoCommand
        {
            get
            {
                return new RelayCommand(this.TomarFoto);
            }
        }

        public ICommand GrabarCommand
        {
            get
            {
                return new RelayCommand(this.Grabar);
            }
        }
        #endregion

        #region Metodos
        private int GetModeloIndex()
        {
            return this.listamodelos.FindIndex(m => m.CodigoParametro == Cliente.ModeloId);
        }

        private int GetMarcaIndex()
        {
            //var index = this.listamarcas.FindIndex(m => m.CodigoParametro == Cliente.MarcaId);
            //return index;
            return this.listamarcas.FindIndex(m => m.CodigoParametro == Cliente.MarcaId);
        }

        private void CargarMarca()
        {
            this.listamarcas = this.dataAccess.GetMarcas();
            this.Marcas = new ObservableCollection<Marca>(this.listamarcas);
        }
        private void CargarModelo()
        {
            this.listamodelos = this.dataAccess.GetModelos();
            this.Modelos = new ObservableCollection<Modelo>(this.listamodelos);
        }
        private void CargarListaTrabajo()
        {
            this.listatrabajos = this.dataAccess.GetListaTrabajo();
            this.Trabajo = new ObservableCollection<ListaTrabajo>(this.listatrabajos);
            this.IsRefreshing = false;
        }
        private void Buscar()
        {
            if (string.IsNullOrEmpty(Filter))
            {
                this.Trabajo = new ObservableCollection<ListaTrabajo>(this.listatrabajos);
            }
            else
            {
                this.Trabajo = new ObservableCollection<ListaTrabajo>(
                    this.listatrabajos.Where(l => l.DetalleTrabajo.ToLower().Contains(
                        this.Filter.ToLower())));
            }
        }
        private async void TomarFoto()
        {
            await CrossMedia.Current.Initialize();

            if (CrossMedia.Current.IsCameraAvailable &&
                CrossMedia.Current.IsTakePhotoSupported)
            {
                var fuente = await Application.Current.MainPage.DisplayActionSheet(
                    "Seleccione Fuente",
                    "Cancelar",
                    null,
                    "Galeria",
                    "Camara");

                if (fuente == "Cancelar")
                {
                    this.file = null;
                    return;
                }

                if (fuente == "Camara")
                {
                    this.file = await CrossMedia.Current.TakePhotoAsync(
                        new StoreCameraMediaOptions
                        {
                            Directory = "Sample",
                            Name = "test.jpg",
                            PhotoSize = PhotoSize.Small,
                        }
                    );
                }
                else
                {
                    this.file = await CrossMedia.Current.PickPhotoAsync();
                }
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            //if (CrossMedia.Current.IsCameraAvailable)
            //{
            //    this.file = await CrossMedia.Current.TakePhotoAsync(
            //        new StoreCameraMediaOptions
            //        {
            //            Directory = "Sample",
            //            Name = "test.jpg",
            //            PhotoSize = PhotoSize.Small,
            //        }
            //    );
            //}

            if (this.file != null)
            {
                this.ImagenFuente = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
        }

        private async void Grabar()
        {
            if (this.file == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Tome la Foto",
                    "Aceptar");
                return;
            }

            foreach (var item in this.Trabajo)
            {
                if (item.CheckList)
                {
                    this.itemseleccionado = 1;
                    break;
                }
                else
                {
                    this.itemseleccionado = 0;
                }
            }

            if (this.itemseleccionado == 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Mensaje",
                    "Seleccione un Item de Trabajo",
                    "Aceptar");
                return;
            }

            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                    if (location != null)
                    {
                        this.Latitud = location.Latitude.ToString();
                        this.Longitud = location.Longitude.ToString();
                    }
                }
                else
                {
                    this.Latitud = location.Latitude.ToString();
                    this.Longitud = location.Longitude.ToString();
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                this.Latitud = string.Empty;
                this.Longitud = string.Empty;
            }
            catch (FeatureNotEnabledException fneEx)
            {
                this.Latitud = string.Empty;
                this.Longitud = string.Empty;
            }
            catch (PermissionException pEx)
            {
                this.Latitud = string.Empty;
                this.Longitud = string.Empty;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    ex.ToString(),
                    "Aceptar");
            }

            var codigomarca = this.listamarcas.ElementAt(this.MarcaIndex).CodigoParametro;
            var codigomodelo = this.listamodelos.ElementAt(this.ModeloIndex).CodigoParametro;

            var ordernew = new Orden()
            {
                IdOrden = Cliente.IdOrden,
                IdEquipo = Cliente.IdEquipo,
                OrdenEstado = "PRO",
                Cliente = Cliente.Cliente,
                Direccion = Cliente.Direccion,
                Contacto = Cliente.Contacto,
                Celular = Cliente.Celular,
                FechaInicioOT = Cliente.FechaInicioOT,
                FechaFinalOT = Cliente.FechaFinalOT,
                TipoTrabajo = Cliente.TipoTrabajo,
                ProblemaEquipo = Cliente.ProblemaEquipo,
                Notas = Cliente.Notas,
                Equipo = Cliente.Equipo,
                MarcaId = codigomarca,
                ModeloId = codigomodelo,
                Voltaje = Cliente.Voltaje,
                Amperaje = Cliente.Amperaje,
                Presion = Cliente.Presion,
                FechaInicioTR = fechainiciotr,
                FechaFinalTR = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                Observacion = Cliente.Observacion,
                ImagenTR = FilesHelper.ReadFully(this.file.GetStream()),
                RutaImagen = Cliente.RutaImagen,
                Latitud = Cliente.Latitud,
                Logintud = Cliente.Logintud,
            };

            this.dataService.Update(ordernew);

            foreach (var item in this.listatrabajos)
            {
                if (item.CheckList)
                {
                    var neworderdet = new OrdenDetalles
                    {
                        IdOrden = Cliente.IdOrden,
                        IdListaTrabajo = item.Id
                    };
                    this.dataService.Insert(neworderdet);
                }
            }

            var orderupdate = this.dataAccess.GetOrdenesLocal(Cliente.IdOrden);
            var orderdetail = this.dataAccess.GetOrdenDetalleLocal(Cliente.IdOrden);

            IsRunning = true;
            IsEnable = false;

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                IsRunning = false;
                IsEnable = true;

                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.ConfirmValidation3,
                    Languages.Accept);
            }
            else
            {
                var apiService = Application.Current.Resources["APIServices"].ToString();
                var orderapi = Converter.ToOrderApi(orderupdate, orderdetail);

                var response = await this.apiService.Post(
                    apiService,
                    "/api",
                    "/orderdetail",
                    orderapi);

                if (response.IsSuccess)
                {
                    this.dataService.Delete(orderupdate);
                    foreach (var itemorder in orderdetail)
                    {
                        this.dataService.Delete(itemorder);
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        response.Message,
                        Languages.ConfirmValidation3,
                        Languages.Accept);
                }
            }

            IsRunning = false;
            IsEnable = true;

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Orders = new OrdersViewModel();
            Application.Current.MainPage = new MasterPage();
        }
        #endregion

    }
}
