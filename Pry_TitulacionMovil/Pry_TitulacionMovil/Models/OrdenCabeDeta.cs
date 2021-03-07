namespace Pry_TitulacionMovil.Models
{
    using System.Collections.Generic;
    public class OrdenCabeDeta
    {
        public int IdOrden { get; set; }
        public int IdEquipo { get; set; }
        public string Equipo { get; set; }
        public string MarcaId { get; set; }
        public string ModeloId { get; set; }
        public string Voltaje { get; set; }
        public string Amperaje { get; set; }
        public string Presion { get; set; }
        public string FechaInicioTR { get; set; }
        public string FechaFinalTR { get; set; }
        public string Observacion { get; set; }
        public byte[] ImagenTR { get; set; }
        public string RutaImagen { get; set; }
        public string Latitud { get; set; }
        public string Logintud { get; set; }
        public List<OrdenDetalles> OrdenDetalles { get; set; }
    }
}
