using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WindowsFormsAppMySql.Database.Entities;
using System.Collections.Generic;


namespace WindowsFormsAppMySql.Database.Entities
{
    [Table("employees")]
    public class Employee
    {
        [Key]
        public int id { get; set; }
        public string first_name {  get; set; }
        public int role { get; set; }       // 0 - biuro, 1 - montaz, 3 - do usuniecia 
        public int admin {  get; set; }
        public string password { get; set; }
        public int company { get; set; } // 0 - tomkar, 1 - premium
        public virtual ICollection<Installation> Installations { get; set; } = new List <Installation>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<Measurement> Measurements { get; set; } = new List<Measurement>();

        public Employee() { }
    }
}
