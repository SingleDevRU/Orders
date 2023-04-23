using SQLite;

namespace Orders
{
    [Table("Clients")]
    public class Client
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        
        [Unique]
        public string Inn { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
