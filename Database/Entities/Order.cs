using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsAppMySql.Database.Entities
{
    [Table("orders")]
    public class Order
    {
        [Key]
        public int id { get; set; }
        public int order_number { get; set; }
        public int company_number { get; set; }
        public int client_id { get; set; }
        [ForeignKey("client_id")]
        public virtual Client client { get; set; }
        public string installation_address { get; set; }
        public DateTime? order_date { get; set; }                       // data_zamowienia
        public int planned_delivery {  get; set; }                      // planowana_dostawa
        public DateTime? delivery_date { get; set; }                    // planowana_dostawa
        public DateTime? payment_products { get; set; }                 // wplata_za_towar
        public DateTime? payment_installation { get; set; }             // wplata_za_montaz
        public double product_advance { get; set; } = 0;
        public double installation_advance { get; set; } = 0;
        public virtual ICollection<Installation> Installations { get; set; }
        public int? measurement_id { get; set; }
        [ForeignKey("measurement_id")]
        public virtual Measurement measurement { get; set; } = null;
        public int product_id { get; set; }
        [ForeignKey("product_id")]
        public virtual Product product { get; set; }
        public int employee_id { get; set; } = -1;
        [ForeignKey("employee_id")]
        public virtual Employee employee { get; set; }
        public string invoice {  get; set; }
        public int instance {  get; set; }
        public double squared_meters {  get; set; }
        public int is_with_installation { get; set; } = 0;      // 0 - bez , 1 - z instalacją 
        public string state { get; set; }                       // stan budowy
        public string supplier_order_number { get; set; }
        public int is_checked_for_installation { get; set; } = 0;
        public int is_checked_for_stock { get; set; } = 0;

        public string notes { get; set; }                   // produkt
        public string notes_information { get; set; }       // uwagi  

        public string color_rgb { get; set; } = "";


        public Order() { }

        public string orderCompanyNumber()
        {
            if (company_number == 0)
            {
                return order_number.ToString();
            }
            else if (company_number == 1)
            {
                return order_number + "P";
            }
            else return order_number.ToString();
        }
    }

}

