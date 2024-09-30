using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsAppMySql.Database;
using WindowsFormsAppMySql.Database.Entities;

namespace WindowsFormsAppMySql.Classes
{
    public class BillToReview
    {
        public Bill Bill { get; set; }
        public Client Client { get; set; }
        public String Products { get; set; }

        private List<Payment> paymentsList = new List<Payment>();

        public BillToReview(Bill bill, Client client)
        {
            this.Bill = bill;
            this.Client = client;

            using (var context = new MyDbConnection())
            {
                this.paymentsList = context.Payments.Where(p => p.bill_id == bill.id).ToList();
                MessageBox.Show(paymentsList.Count + "");
                // string.Join(", ", paymentsList.Select(p => p.order.orderCompanyNumber()));
            }
        }

        public BillToReview() { }

        public string DisplayBill
        {
    
            get
            {
                using (var context = new MyDbConnection())
                { 
                    return $"[{Bill.id}] {Client.first_name} {Client.last_name} :  {string.Join(", ", context.Payments.Where(p => p.bill_id == Bill.id).ToList().Select(p => p.order.orderCompanyNumber()))}";
                }
            }
        }

        public List<BillToReview> DisplayBillPayments(List<Bill> bills, List<Payment> payments, List<Order> orders)
        {
            List<BillToReview> result = new List<BillToReview>();

            for (int i = 0; i < bills.Count; i++)
            {
                BillToReview btr = new BillToReview();
                List<Product> productsTmp = new List<Product>();

                btr.Bill = bills[i];
                List<Payment> p = payments.Where(paym => paym.bill_id == bills[i].id).ToList();
                foreach (Payment payment in p)
                {
                    Order o = orders.First(ord => ord.id == payment.order_id);
                    btr.Client = o.client;
                    productsTmp.Add(o.product);
                }

                btr.Products = string.Join(", ", productsTmp);

            }

            return result;
        }


    }

}
