namespace Pry_TitulacionMovil.Models
{
    using SQLite;
    public class OrdenDetalles
    {
        [AutoIncrement]
        [PrimaryKey]
        public int IdOrdeDetalle { get; set; }
        public int IdOrden { get; set; }
        public int IdListaTrabajo { get; set; }
    }
}
