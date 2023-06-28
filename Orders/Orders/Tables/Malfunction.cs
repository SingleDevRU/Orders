using SQLite;

namespace Orders.Tables
{
    [Table("Malfunctions")]
    public class Malfunction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
