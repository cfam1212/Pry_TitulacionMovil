namespace Pry_TitulacionMovil.Interface
{
    public interface IDataBase
    {
        SQLite.SQLiteConnection GetConnection();
    }
}
