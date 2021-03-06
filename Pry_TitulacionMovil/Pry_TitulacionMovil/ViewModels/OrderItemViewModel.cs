namespace Pry_TitulacionMovil.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using System.Windows.Input;
    using Views;
    using Xamarin.Forms;

    public class OrderItemViewModel: Orden
    {
        #region Comandos
        public ICommand SelectOrderCommand
        {
            get { return new RelayCommand(SelectOrder); }
        }
        #endregion

        #region Metodos
        private async void SelectOrder()
        {

            MainViewModel.GetInstance().Cliente = new ClientesViewModel(this);
            await App.Navigator.PushAsync(new OrderTabbedPage());
            //await Application.Current.MainPage.Navigation.PushAsync(new OrderTabbedPage());

        } 
        #endregion
    }
}
