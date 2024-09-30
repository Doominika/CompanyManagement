using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsAppMySql.Database.Entities
{
    [Table("products")]
    public class Product
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public int visible { get; set; } = 1;

        public Product() { }
    }
}
