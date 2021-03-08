namespace Pry_TitulacionMovil.ViewModels
{
    using Helpers;
    using Plugin.Media.Abstractions;
    using Services;
    using Xamarin.Forms;
    using Models;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using System;
    using Plugin.Media;
    using Views;

    public class MyPerfilViewModel: BaseViewModel
    {
        #region Services
        private ApiService apiService;
        private DataService dataService;
        #endregion

        #region Attributes
        private bool isRunning;
        private bool isEnabled;
        private ImageSource imagenFuente;
        private MediaFile file;
        private bool isEnabledImage;
        private bool isVisible;
        #endregion

        #region Properties
        public UserLocal User { get; set; }

        public ImageSource ImagenFuente
        {
            get { return this.imagenFuente; }
            set { SetValue(ref this.imagenFuente, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public bool IsEnabledImage
        {
            get { return this.isEnabledImage; }
            set { SetValue(ref this.isEnabledImage, value); }
        }

        public bool IsVisible
        {
            get { return this.isVisible; }
            set { SetValue(ref this.isVisible, value); }
        }
        #endregion

        #region Constructor
        public MyPerfilViewModel()
        {
            this.apiService = new ApiService();
            this.dataService = new DataService();
            this.User = MainViewModel.GetInstance().User;
            this.ImagenFuente = User.ImagenPath;
            this.IsEnabled = true;
            this.IsVisible = true;
            this.IsEnabledImage = true;
            //if (User.ImagenPath != "ic_nouser")
            //{
            //    this.IsEnabledImage = false;
            //    this.IsVisible = false;
            //}
        } 
        #endregion

        #region Commandos
        public ICommand CambiarImagenCommand
        {
            get
            {
                return new RelayCommand(CambiarImagen);
            }
        }

        public ICommand CambiarPasswordCommand
        {
            get
            {
                return new RelayCommand(CambiarPassword);
            }
        }

        public ICommand GrabarCommand
        {
            get
            {
                return new RelayCommand(Grabar);
            }
        }

        #endregion

        #region Metodos
        private async void CambiarImagen()
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

            if (this.file != null)
            {
                this.ImagenFuente = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
        }
        private async void CambiarPassword()
        {
            MainViewModel.GetInstance().CambiarPassword = new CambiarPassWordViewModel();
            await App.Navigator.PushAsync(new CambiarPassWordPage());
        }
        private async void Grabar()
        {
            if (string.IsNullOrEmpty(this.User.UserName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Ingrese el Nombre",
                    "OK");
                return;
            }

            if (string.IsNullOrEmpty(this.User.UserLastName))
            {
                await Application.Current.MainPage.DisplayAlert(
                   "Error",
                   "Ingrese el Apellido",
                   "OK");
                return;

            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var checkConnetion = await this.apiService.CheckConnection();

            if (!checkConnetion.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    checkConnetion.Message,
                    "OK");
                return;
            }

            byte[] imageArray = null;

            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
            }

            var userApi = Converter.ToUserApi(this.User, imageArray);

            var apiService = Application.Current.Resources["APIServices"].ToString();

            var response = await this.apiService.UpdateUser(
                apiService,
                "api",
                "Usuarios",
                userApi);

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "OK");
                return;
            }

            var user = await this.apiService.GetUser(
                    apiService,
                    "api",
                    "GetUsuario",
                    this.User.UserId);

            var userLocal = Converter.ToUserLocal(user);

            MainViewModel.GetInstance().User = userLocal;
            this.dataService.Update(userLocal);

            this.IsRunning = false;
            this.IsEnabled = true;

            await App.Navigator.PopAsync();
        } 
        #endregion

    }
}
