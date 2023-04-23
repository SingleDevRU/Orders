using SQLite;
using System;

namespace Orders
{
    [Table("Orders")]
    public class Order
    {
        [PrimaryKey,AutoIncrement,Column("_id")]
        public int Id { get; set; }

        public string Date { get; set; }
        public int Number { get; set; }
        public string Client { get; set; }
        public string Executor { get; set; }
        public bool SendMail { get; set; }
    }
}
