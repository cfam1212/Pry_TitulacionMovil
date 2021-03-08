namespace Pry_TitulacionMovil.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Views;

    public class MainViewModel: BaseViewModel
    {

        #region Atributos
        private UserLocal user;
        #endregion

        #region Propiedades
        public int userId { get; set; }

        public UserLocal User
        {
            get { return this.user; }
            set { SetValue(ref this.user, value); }
        }
        public ObservableCollection<MenuItemViewModel> Menus { get; set; }
        #endregion

        #region Constructor
        public MainViewModel()
        {
            instance = this;
            this.Login = new LoginViewModel();
            this.CargarMenus();
        }
        #endregion        

        #region ViewsModels
        public LoginViewModel Login { get; set; }
        public OrdersViewModel Orders { get; set; }
        public ClientesViewModel Cliente { get; set; }
        public OrdersPendienteViewModel OrdersPendientes { get; set; }
        public SincronizarViewModel Sincronizar { get; set; }
        public MyPerfilViewModel MyPerfil { get; set; }
        public CambiarPassWordViewModel CambiarPassword { get; set; }

        #endregion

        #region Singleton
        private static MainViewModel instance;
        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }
        #endregion

        #region Comandos
        public ICommand SincronizarCommand
        {
            get
            {
                return new RelayCommand(Upload);
            }
        } 
        #endregion

        #region Metodos
        private void CargarMenus()
        {
            this.Menus = new ObservableCollection<MenuItemViewModel>();

            this.Menus.Add(new MenuItemViewModel
            {
                NombrePagina = "MiPerfil",
                Icono = "ic_group",
                Titulo = "Perfil"
            });

            this.Menus.Add(new MenuItemViewModel
            {
                NombrePagina = "OrdenNueva",
                Icono = "ic_list_alt",
                Titulo = "Ordenes Nuevas"
            });

            this.Menus.Add(new MenuItemViewModel
            {
                NombrePagina = "OrdenPendiente",
                Icono = "ic_local_library",
                Titulo = "Ordenes Pendientes"
            });

            this.Menus.Add(new MenuItemViewModel
            {
                NombrePagina = "Cerrar",
                Icono = "ic_exit_to_app",
                Titulo = "Cerrar Sesión"
            });
        }
        private async void Upload()
        {
            this.Sincronizar = new SincronizarViewModel();
            await App.Navigator.PushAsync(new SincronizarOrdersPage());
        }
        #endregion
    }
}
