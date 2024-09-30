using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsAppMySql.Database.Entities
{
    [Table("measurements")]

    public class Measurement
    {
        [Key]
        public int id { get; set; }
        public int client_id { get; set; }
        [ForeignKey("client_id")]
        public virtual Client client { get; set; }
        public string measurement_address { get; set; }
        public string status { get; set; }
        public double price { get; set; }
        public int is_paid { get; set; } = 0;
        public int execution_status { get; set; } = 0;       //0 - nie zaczęte, 1 - zakończone
        public DateTime? measurement_date { get; set; }
        public DateTime? registration_date { get; set; }
        public int is_checked_if_ordered { get; set; } = 0;
        public int employee_id { get; set; } = -1;
        [ForeignKey("employee_id")]
        public virtual Employee employee { get; set; }
        public string notes { get; set; }                   // produkty
        public string notes_information { get; set; }       // uwagi  
        public string color_rgb { get; set; } = "";


        public Measurement ()
        {

        }
    }
}
