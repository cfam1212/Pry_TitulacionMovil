namespace Pry_TitulacionMovil.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Services;
    using Models;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class CambiarPassWordViewModel: BaseViewModel
    {
        #region Services
        private ApiService apiService;
        private DataService dataService;
        #endregion

        #region Atributos
        private bool isRunning;
        private bool isEnabled;
        private string passwordActual;
        private string passwordNuevo;
        private string confirmaPassword;
        #endregion

        #region Properties
        public bool IsRunning
        {
            get { return isRunning; }
            set { SetValue(ref isRunning, value); }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { SetValue(ref isEnabled, value); }
        }
        public string PasswordActual
        {
            get { return passwordActual; }
            set { SetValue(ref passwordActual, value); }
        }
        public string PasswordNuevo
        {
            get { return passwordNuevo; }
            set { SetValue(ref passwordNuevo, value); }
        }
        public string ConfirmaPassword
        {
            get { return confirmaPassword; }
            set { SetValue(ref confirmaPassword, value); }
        }
        #endregion

        #region Constructors
        public CambiarPassWordViewModel()
        {
            this.apiService = new ApiService();
            this.dataService = new DataService();
            this.IsEnabled = true;
        }
        #endregion

        #region Command
        public ICommand GrabarCommand
        {
            get
            {
                return new RelayCommand(CambiarPassword);
            }
        }
        #endregion

        #region Metodos
        private async void CambiarPassword()
        {
            if (string.IsNullOrEmpty(this.PasswordActual))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Ingrese Password Actual",
                    "OK");
                return;
            }

            if (this.PasswordActual != MainViewModel.GetInstance().User.Password)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Password Actual Incorrecto",
                    "OK");
                return;
            }

            if (string.IsNullOrEmpty(this.PasswordNuevo))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Ingrese Nuevo Password",
                    "OK");
                return;
            }

            if (this.PasswordNuevo.Length < 6)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Al Menos debe tener 6 carácteres",
                    "OK");
                return;
            }

            if (string.IsNullOrEmpty(this.ConfirmaPassword))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Ingrese Confirmación del Password",
                    "OK");
                return;
            }

            if (this.PasswordNuevo != this.ConfirmaPassword)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Password de confirmación Incorrecto",
                    "OK");
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

            var cambiarrequest = new UserPassword
            {
                UserId = MainViewModel.GetInstance().User.UserId,
                NuevoPassword = this.PasswordNuevo
            };

            var apiService = Application.Current.Resources["APIServices"].ToString();

            var response = await this.apiService.CambiarPassWord(
                apiService,
                "api",
                "CambiarPassWord",
                cambiarrequest);

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

            MainViewModel.GetInstance().User.Password = this.PasswordNuevo;

            this.dataService.Update(MainViewModel.GetInstance().User);

            this.IsRunning = false;
            this.IsEnabled = true;

            await Application.Current.MainPage.DisplayAlert(
                        "Información",
                        "Password Modificado",
                        "OK");

            await App.Navigator.PopAsync();
            //MainViewModel.GetInstance().Orders = new OrdersViewModel();
            //Application.Current.MainPage = new MasterPage();

        }
        #endregion
    }
}
