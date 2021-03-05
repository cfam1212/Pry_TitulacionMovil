namespace Pry_TitulacionMovil.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using System;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Views;
    using Models;
    using Services;
    using System.Collections.Generic;

    public class LoginViewModel: BaseViewModel
    {
        #region Services
        private ApiService apiService;
        private DataService dataService;
        #endregion

        #region Atributos
        private string username;
        private string password;
        private bool isRemember;
        private bool isRunning;
        private bool isEnabled;
        private List<Marca> listmarcas;
        private List<Modelo> listmodelos;
        #endregion

        #region Propiedades
        public string UserName 
        {
            get { return this.username; }
            set { SetValue(ref this.username, value); } 
        }
        public string Password 
        {
            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }
        public bool IsRemember 
        {
            get { return this.isRemember; }
            set { SetValue(ref this.isRemember, value); }
        }
        public bool IsRunning 
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }
        public bool IsEnabled 
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }
        #endregion

        #region Constructor
        public LoginViewModel()
        {
            this.apiService = new ApiService();
            this.dataService = new DataService();
            this.IsRemember = true;
            this.IsEnabled = true;
        }
        #endregion

        #region Commandos
        public ICommand LoginCommand 
        { 
            get
            {
                return new RelayCommand(Login);
            }
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(this.UserName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error", 
                    "Ingrese Usuario..!", 
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Ingrese Password..!",
                    "Aceptar");

                this.Password = string.Empty;

                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error de Conección",
                    connection.Message,
                    "Aceptar");

                return;
            }


            var _urlbase = Application.Current.Resources["APIServices"].ToString();

            var _login = await this.apiService.GetLogin(
                _urlbase,
                "api",
                "login",
                this.UserName,
                this.Password);

            if (_login.UserId == 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Login Incorrecto",
                    connection.Message,
                    "Aceptar");

                this.IsRunning = false;
                this.IsEnabled = true;
                this.Password = string.Empty;

                return;
            }

            var userlocal = Converter.ToUserLocal(_login);
            var mainViewModel = MainViewModel.GetInstance();

            mainViewModel.userId = _login.UserId;
            mainViewModel.User = userlocal;

            if (this.IsRemember)
            {
                Settings.UserLogin = _login.UserId;
                this.dataService.Delete(userlocal);
                this.dataService.Insert(userlocal);
            }

            this.IsRunning = false;
            this.IsEnabled = true;
            this.UserName = string.Empty;
            this.Password = string.Empty;

            var marcas = await this.apiService.GetParametrosGenerales<Marca>(
                _urlbase,
                "api",
                "parametro",
                "MARCA");

            this.listmarcas = (List<Marca>)marcas.Result;

            if (listmarcas.Count > 0)
            {
                foreach (var _item in this.listmarcas)
                {
                    var newmarcas = new Marca()
                    {
                        Id = _item.Id,
                        CodigoParametro = _item.CodigoParametro,
                        NombreParametro = _item.NombreParametro
                    };

                    this.dataService.Delete(newmarcas);
                    this.dataService.Insert(newmarcas);
                }
            }

            var modelos = await this.apiService.GetParametrosGenerales<Modelo>(
                _urlbase,
                "api",
                "parametro",
                "MODELO");

            this.listmodelos = (List<Modelo>)modelos.Result;

            if (listmodelos.Count > 0)
            {
                foreach (var _item in this.listmodelos)
                {
                    var newmodelos = new Modelo()
                    {
                        Id = _item.Id,
                        CodigoParametro = _item.CodigoParametro,
                        NombreParametro = _item.NombreParametro
                    };

                    this.dataService.Delete(newmodelos);
                    this.dataService.Insert(newmodelos);
                }
            }

            mainViewModel.Orders = new OrdersViewModel();
            //MainViewModel.GetInstance().Orders = new OrdersViewModel();

            await Application.Current.MainPage.Navigation.PushAsync(new OrdersPage());

        }
        #endregion

    }
}
