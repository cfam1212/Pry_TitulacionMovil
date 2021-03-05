namespace Pry_TitulacionMovil.ViewModels
{
    using Models;
    using System;
    using System.Collections.ObjectModel;
    using Services;
    using Helpers;
    using System.Collections.Generic;

    public class ClientesViewModel : BaseViewModel
    {
        #region Atributos
        public Orden Cliente { get; set; }
        private List<Marca> listamarcas;
        private List<Modelo> listamodelos;
        private ObservableCollection<Marca> marcas;
        private ObservableCollection<Modelo> modelos;
        private DataService dataService;
        private DataAccess dataAccess;
        private int marcaindex;
        private int modeloindex;
        #endregion

        #region Propiedades
        public ObservableCollection<Marca> Marcas
        {
            get { return marcas; }
            set { this.SetValue(ref marcas, value); }
        }

        public ObservableCollection<Modelo> Modelos
        {
            get { return modelos; }
            set { this.SetValue(ref modelos, value); }
        }

        public int MarcaIndex
        {
            get { return marcaindex; }
            set { this.SetValue(ref marcaindex, value); }
        }
        public int ModeloIndex
        {
            get { return modeloindex; }
            set { this.SetValue(ref modeloindex, value); }
        } 
        #endregion

        #region Constructor
        public ClientesViewModel(Orden cliente)
        {
            this.dataService = new DataService();
            this.dataAccess = new DataAccess();
            this.CargarMarca();
            this.CargarModelo();
            this.MarcaIndex = this.GetMarcaIndex();
            this.ModeloIndex = this.GetModeloIndex();
            this.Cliente = cliente;
        } 
        #endregion

        #region Metodos
        private int GetModeloIndex()
        {
            return this.listamodelos.FindIndex(m => m.CodigoParametro == Cliente.ModeloId);
        }

        private int GetMarcaIndex()
        {
            //var index = this.listamarcas.FindIndex(m => m.CodigoParametro == Cliente.MarcaId);
            //return index;
            return this.listamarcas.FindIndex(m => m.CodigoParametro == Cliente.MarcaId);
        }

        private void CargarMarca()
        {
            this.listamarcas = this.dataAccess.GetMarcas();
            this.Marcas = new ObservableCollection<Marca>(this.listamarcas);
        }
        private void CargarModelo()
        {
            this.listamodelos = this.dataAccess.GetModelos();
            this.Modelos = new ObservableCollection<Modelo>(this.listamodelos);
        } 
        #endregion

    }
}
