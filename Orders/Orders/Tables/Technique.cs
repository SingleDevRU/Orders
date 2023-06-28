using SQLite;

namespace Orders.Tables
{
    [Table("Techniques")]
    public class Technique
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string ParentCode { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string ModelCode { get; set; }
        public string Parent { get; set; }
        public string SerialKey { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        //public string GroupModel { get; set; }

    }
}
