using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsAppMySql.Database.Entities
{
    [Table("stock")]
    public class Stock
    {
        [Key]
        public int id { get; set; }
        public DateTime delivery_date { get; set; }
        public int order_id { get; set; }
        public string order_name { get; set; }
        public int client_id { get; set; }
        public string client_name { get; set; }
        public string notes { get; set; }
        public int is_checked { get; set; } = 0;
        public string color_rgb { get; set; } = "";
        public Stock() { }

    }
}
