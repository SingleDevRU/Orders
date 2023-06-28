using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orders.Tables
{
    [Table("Models")]
    public class Model
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        //public string Perfomance { get; set; }
    }
}
