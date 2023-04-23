﻿using SQLite;

namespace Orders
{
    [Table("Malfunctions")]
    public class Malfunction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}