namespace Pry_TitulacionMovil.Models
{
    using SQLite;
    public class ListaTrabajo
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string DetalleTrabajo { get; set; }
        public bool CheckList { get; set; }
    }
}
