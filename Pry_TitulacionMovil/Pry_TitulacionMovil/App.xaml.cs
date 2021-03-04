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
        #endregion

        #region Constructor
        public App()
        {
            InitializeComponent();

            //this.MainPage = new LoginPage();

            if (Settings.UserLogin == 0)
            {
                this.MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Orders = new OrdersViewModel();
                Application.Current.MainPage = new NavigationPage(new OrdersPage());
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
