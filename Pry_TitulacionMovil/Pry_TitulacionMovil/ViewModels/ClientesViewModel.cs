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
        private string filter;
        private ImageSource imagenFuente;
        private MediaFile file;
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
        #endregion

    }
}
