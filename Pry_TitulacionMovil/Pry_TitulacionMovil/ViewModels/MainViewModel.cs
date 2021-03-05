namespace Pry_TitulacionMovil.ViewModels
{
    using Models;
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
        #endregion
        #region Constructor
        public MainViewModel()
        {
            instance = this;
            this.Login = new LoginViewModel();            
        } 
        #endregion

        #region ViewsModels
        public LoginViewModel Login { get; set; }
        public OrdersViewModel Orders { get; set; }
        public ClientesViewModel Cliente { get; set; }
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
    }
}
