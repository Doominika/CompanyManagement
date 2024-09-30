using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WindowsFormsAppMySql.Database.Entities;

namespace WindowsFormsAppMySql.Database
{
    [Table("clients")]
    public class Client
    {
        [Key]
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone_number { get; set; }
        public string address { get; set; } 
        public string email { get; set; } 
        public virtual ICollection <Measurement> Measurements { get; set; } = new List<Measurement>();
        public virtual ICollection <Installation> Installations { get; set; } = new List <Installation>();
        public virtual ICollection <Order> Orders { get; set; } = new List<Order>();

        public Client (string firstName, string lastName, string phoneNumber,
              string address, string email)
        {
            this.first_name = firstName;
            this.last_name = lastName;
            this.phone_number = phoneNumber;
            this.address = address;
            this.email = email;
        }


        public Client() { }

        public string DisplayName
        {
            get { return $"[{id}] {first_name} {last_name}"; }
        }

        public string DisplayNameOnly
        {
            get { return $"{first_name} {last_name}"; }
        }
    }
}
