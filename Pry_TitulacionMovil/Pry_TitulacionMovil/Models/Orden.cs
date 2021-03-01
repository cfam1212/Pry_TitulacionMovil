namespace Pry_TitulacionMovil.Models
{
    using SQLite;
    public class Orden
    {
        [PrimaryKey]
        public int OrdenId { get; set; }
        public int EquipoId { get; set; }
        public string Estado { get; set; }
        public string Cliente { get; set; }
        public string FechaInicioOT { get; set; }
        public string FechaFinalOT { get; set; }
        public string TipoTrabajo { get; set; }
        public string ProblemaEquipo { get; set; }
        public string Notas { get; set; }
        public string MarcaId { get; set; }
        public string ModeloId { get; set; }
        public string Voltaje { get; set; }
        public string Amperaje { get; set; }
        public string Presion { get; set; }
        public string FechaInicioTR { get; set; }
        public string FechaFinalTR { get; set; }
        public string Observacion { get; set; }
        public byte[] FotoEquipo { get; set; }
        public string RutaImagen { get; set; }
    }
}
