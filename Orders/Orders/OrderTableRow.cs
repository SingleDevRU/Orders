using SQLite;

namespace Orders
{
    [Table("OrderTableRows")]
    public class OrderTableRow
    {
        [PrimaryKey,AutoIncrement,Column("_id")]
        public int Id { get; set; }

        public int Number { get; set; }
        public int ParentNumber { get; set; }
        public bool isSaved { get; set; }
        public string Technic { get; set; }
        public string Malfunction { get; set; }
        public string Equipment { get; set; }
    }
}
