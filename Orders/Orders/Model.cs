using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orders
{
    [Table("Models")]
    public class Model
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string GroupCode { get; set; }
        public string Perfomance { get; set; }
    }
}
