using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WindowsFormsAppMySql.Database.Entities
{
    [Table("advances")]
    public class Advance
    {
        [Key]
        public int id { get; set; }
        public int bill_id { get; set; }
        [ForeignKey("bill_id")]
        public virtual Bill bill { get; set; }
        public double amount { get; set; }
        public DateTime? date { get; set; }

    }
}
