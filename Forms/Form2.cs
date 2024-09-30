using Castle.Core.Internal;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsAppMySql.Database.Entities;
using WindowsFormsAppMySql.Database;
using WindowsFormsAppMySql.FileManaging;

namespace WindowsFormsAppMySql.Forms
{
    public partial class Form2 : Form
    {
        private Payment mainPayment;
        private f_Main mainForm;
        private string searchText = "";
        private int filterCompanyPayment = 0;


        List<Order> orders = new List<Order>();
        List<Product> products = new List<Product>();
        List<Payment> payments = new List<Payment>();
        List<Bill> bills = new List<Bill>();
        List<Advance> advances = new List<Advance>();

        public Form2(f_Main mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();

            /*
            ApplyStyles(this);

            load();

            this.Load += (sender, e) =>
            {
                foreach (DataGridViewRow row in PaymentsGridView.Rows)
                {
                    row.Selected = false;
                }
            };
            */

        }
    }
}

