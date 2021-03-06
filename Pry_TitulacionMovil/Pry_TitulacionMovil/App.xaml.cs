namespace Pry_TitulacionMovil
{
    using ViewModels;
    using Helpers;
    using Xamarin.Forms;
    using Views;
    public partial class App : Application
    {
        #region Propiedades
        public static NavigationPage Navigator { get; internal set; }
        public static MasterPage Master { get; internal set; }

        #endregion

        #region Constructor
        public App()
        {
            InitializeComponent();

            if (Settings.UserLogin == 0)
            {
                this.MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                var dataAcces = new DataAccess();
                var user = dataAcces.BuscarUsuario(Settings.UserLogin);
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.userId = user.UserId;
                mainViewModel.User = user;
                mainViewModel.Orders = new OrdersViewModel();
                Application.Current.MainPage = new MasterPage();
            }

        }
        #endregion

        #region Methods
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        } 
        #endregion
    }
}
