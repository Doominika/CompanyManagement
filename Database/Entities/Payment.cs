using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WindowsFormsAppMySql.Database.Entities
{
    [Table("payments")]
    public class Payment
    {
        [Key]
        public int id { get; set; }
        public int bill_id { get; set; }
        [ForeignKey("bill_id")]
        public virtual Bill bill { get; set; }
        public int order_id { get; set; }
        [ForeignKey("order_id")]
        public virtual Order order { get; set; }
        public double net_purchase_price { get; set; } = 0;                 // cena_zakupowa_netto
        public double gross_product_price { get; set; } = 0;                // cena_towaru_umowa_brutto
        public double gross_installation_price { get; set; } = 0;           // cena_montazu_brutto
        public int var_rate { get; set; } = 0;
        public double total_gross_price { get; set; } = 0;                  // cena_calkowita_brutto
        public double material_price { get; set; } = 0;                     // cena_materialow
        public string notes { get; set; } = "";

        public Payment() { }


        
    
    }
}

