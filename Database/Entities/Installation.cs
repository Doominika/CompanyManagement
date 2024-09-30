using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WindowsFormsAppMySql.Database.Entities;

namespace WindowsFormsAppMySql.Database.Entities
{
    [Table("installations")]

    public class Installation
    {
        [Key]
        public int id { get; set; }
        public int order_id { get; set; }
        [ForeignKey("order_id")]
        public virtual Order order { get; set; }
        public int client_id { get; set; }
        [ForeignKey("client_id")]
        public virtual Client client { get; set; }
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public DateTime? date {  get; set; }
        public string installation_address { get; set; }      //?????
        public string notes { get; set; }
        public string color_rgb { get; set; } = "";

        public Installation() { }

    }
}
