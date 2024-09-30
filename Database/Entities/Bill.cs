using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace WindowsFormsAppMySql.Database.Entities
{
    [Table("bills")]
    public class Bill
    {
        [Key]
        public int id { get; set; }
        public double net_purchase_price { get; set; } = 0;               // cena_zakupowa_netto
        public double gross_product_price { get; set; } = 0;              // cena_towaru_umowa_brutto
        public double gross_installation_price { get; set; } = 0;         // cena_montazu_brutto
        public double total_gross_price { get; set; } = 0;                // cena_calkowita_brutto
        public double remaining_price { get; set; }  = 0;                // cena_pozostalo
        public double material_price { get; set; } = 0;                  // cena_materialow
        public double sum_of_advances { get; set; } = 0;                  // suma_zaliczek
        public string notes { get; set; } = "";
        public string color_rgb { get; set; } = "";
        public virtual ICollection<Advance> Advances { get; set; } = new List<Advance>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

        public Bill() { }

    }
}
