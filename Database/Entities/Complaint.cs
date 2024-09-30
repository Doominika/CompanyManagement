using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsAppMySql.Database.Entities
{
    [Table("complaints")]
    public class Complaint
    {
        [Key]
        public int id { get; set; }
        public int order_id {  get; set; }
        [ForeignKey("order_id")]
        public virtual Order order { get; set; }
        public int complaint_number { get; set; } = 0;
        public DateTime? start_date { get; set; }           
        public DateTime? end_date { get; set; }           
        public int company_number { get; set; }
        public string complaint_producer_number { get; set; }
        public int status { get; set; } = 0;            // 0 - niezaczęte, 1 - w trakcie, 2 - rozwiązane
        public string notes {  get; set; }
        public string color_rgb { get; set; } = "";

        public Complaint () { }


        public string displayCompanyNumber()
        {
            if (company_number == 0)
            {
                return complaint_number.ToString();
            }
            else if (company_number == 1)
            {
                return complaint_number + "P";
            }
            else return "";
        }
    }
}

