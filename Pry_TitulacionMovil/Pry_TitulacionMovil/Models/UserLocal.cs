namespace Pry_TitulacionMovil.Models
{
    using SQLite;
    public class UserLocal
    {
        [PrimaryKey]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public string Password { get; set; }
        public string ImagenPath { get; set; }
        public string UserTotalName
        {
            get
            {
                return string.Format("{0} {1}", this.UserName, this.UserLastName);
            }
        }

        public override int GetHashCode()
        {
            return UserId;
        }
    }
}
