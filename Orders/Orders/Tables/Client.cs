using SQLite;

namespace Orders.Tables
{
    [Table("Clients")]
    public class Client
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }

        [Unique]
        public string Inn { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
