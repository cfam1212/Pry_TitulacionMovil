namespace Pry_TitulacionMovil.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using System.Windows.Input;
    using Views;
    using Xamarin.Forms;

    public class MenuItemViewModel
    {
        #region Propiedades
        public string Icono { get; set; }

        public string Titulo { get; set; }

        public string NombrePagina { get; set; }
        #endregion

        #region Commandos
        public ICommand MenuCommand
        {
            get
            {
                return new RelayCommand(NavegarMenu);
            }
        }
        #endregion

        #region Metodos
        private async void NavegarMenu()
        {
            App.Master.IsPresented = false;
            switch (this.NombrePagina)
            {
                case "MiPerfil":
                    App.Master.IsPresented = false;
                    MainViewModel.GetInstance().MyPerfil = new MyPerfilViewModel();
                    await App.Navigator.PushAsync(new MyPerfilPage());
                    break;
                case "OrdenNueva":
                    App.Master.IsPresented = false;
                    MainViewModel.GetInstance().Orders = new OrdersViewModel();
                    await App.Navigator.PushAsync(new OrdersPage());
                    break;
                case "OrdenPendiente":
                    App.Master.IsPresented = false;
                    MainViewModel.GetInstance().OrdersPendientes = new OrdersPendienteViewModel();
                    await App.Navigator.PushAsync(new OrdersPendientePage());
                    break;
                case "Cerrar":
                    Settings.UserLogin = 0;
                    var mainViewModel = MainViewModel.GetInstance();
                    mainViewModel.userId = Settings.UserLogin;
                    Application.Current.MainPage = new NavigationPage(new LoginPage());
                    break;
            }
        } 
        #endregion
    }
}
