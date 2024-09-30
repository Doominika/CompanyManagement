using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsAppMySql.Database.Entities;

namespace WindowsFormsAppMySql.Forms
{
    public partial class f_OrderInfo : Form
    {
        private Order order;
        public f_OrderInfo(Order order)
        {
            InitializeComponent();
            this.order = order;


            number.Text = order.orderCompanyNumber();
            deliverydate.Text = dateTimeToString(order.delivery_date);
            product_payment.Text = dateTimeToString(order.payment_products);
            installation_payment.Text = dateTimeToString(order.payment_installation);
            product.Text = order.product.name;
            notes.Text = order.notes;


        }

        public static string dateTimeToString(DateTime? date)
        {
            if (!date.HasValue)
                return "";
            else
                return date.Value.ToString("dd.MM.yyyy");
        }

        private void f_OrderInfo_Load(object sender, EventArgs e)
        {
            this.ActiveControl = label58;
        }
    }
}
