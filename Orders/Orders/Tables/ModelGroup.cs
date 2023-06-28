using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orders.Tables
{
    [Table("ModelGroups")]
    public class ModelGroup
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Code { get; set; }
       // public string Perfomance { get; set; }
        public string Name { get; set; }
    }
}
