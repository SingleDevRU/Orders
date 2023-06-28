using SQLite;
using System;

namespace Orders.Tables
{
    [Table("Orders")]
    public class Order
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        public string Code { get; set; }
        public string ClientCode { get; set; }
        public string Date { get; set; }
        public int Number { get; set; }
        public string Client { get; set; }
        public string Executor { get; set; }
        public string Comment { get; set; }
        public bool SendMail { get; set; }
        public bool isLoaded { get; set; }
        public bool isChanged { get; set; }
    }
}
