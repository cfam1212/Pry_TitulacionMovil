namespace Pry_TitulacionMovil.Models
{
    using SQLite;
    public class OrdenDetalles
    {
        [AutoIncrement]
        [PrimaryKey]
        public int IdOrdeDetalle { get; set; }
        public int id_orden { get; set; }
        public int id_listatrabajo { get; set; }
    }
}
