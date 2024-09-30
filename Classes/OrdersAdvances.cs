using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsAppMySql.Classes
{
    public class OrdersAdvances
    {

        public int orderId { get; set; }
        public double pprice {  get; set; }                     // koszt
        public double psumOfAdvances { get; set; }              // suma zaliczek
        public double pchangedAdvances { get; set; } = 0;       // jesli zostala zmieniona suma zaliczek
        public double pnewAdvance { get; set; } = 0;            // nowo przydzielona suma zaliczek


        public double iprice { get; set; }
        public double isumOfAdvances { get; set; }
        public double ichangedAdvances { get; set; } = 0;
        public double inewAdvance { get; set; } = 0;

        public OrdersAdvances()
        {

        }

    }
}
