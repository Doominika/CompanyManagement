using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsAppMySql.Database;
using WindowsFormsAppMySql.Database.Entities;

namespace WindowsFormsAppMySql.Forms
{
    public partial class FormAddStockNotes : Form
    {
        private Stock stock;
        public FormAddStockNotes(Stock stock)
        {
            this.stock = stock;
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            using (var context = new MyDbConnection())
            {
                Stock s = context.Stocks.Find(stock.id);
                if (s != null)
                {
                    s.notes = note.Text;
                    context.SaveChanges();
                }
            }

            this.Close();
        }
    }
}
