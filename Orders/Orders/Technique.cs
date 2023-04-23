using SQLite;

namespace Orders
{
    [Table("Techniques")]
    public class Technique
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Parent { get; set; }
        public string SerialKey { get; set; }
        public string Name { get; set; }

    }
}
