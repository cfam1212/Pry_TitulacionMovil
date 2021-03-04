namespace Pry_TitulacionMovil.ViewModels
{
    using Models;
    public class ClientesViewModel : BaseViewModel
    {
        public Orden CliOrder { get; set; }

        public ClientesViewModel(Orden cliorder)
        {
            this.CliOrder = cliorder;
        }
    }
}
