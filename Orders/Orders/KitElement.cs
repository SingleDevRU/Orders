using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Orders
{
    [Table ("KitElements")]
    public class KitElement
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string Code { get; set; }        
        public string Name { get; set; }
    }
}
