using Guna.UI2.WinForms;
using System;
using System.Windows.Forms;
using WindowsFormsAppMySql.Database;
using WindowsFormsAppMySql.Database.Entities;

namespace WindowsFormsAppMySql.Forms
{
    public partial class f_Main : Form
    {
        //private readonly MyDbConnection _context;
        public int isAdmin = 0;
        public int company = 0;
        private Employee mainEmployee;

        Guna2Button recentBtn;

        public f_Main(Employee mainEmployee)
        {
            this.mainEmployee = mainEmployee;
            this.isAdmin = mainEmployee.admin;
            this.company = mainEmployee.company;

            InitializeComponent();

            if (isAdmin == 0)
            {
                btn_admin.Visible = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            openForm(new f_Offers());
            //btn_offers.FillColor = System.Drawing.Color.White;
            //btn_offers.ForeColor = System.Drawing.Color.FromArgb(167, 146, 119);
            changeNewBtn(btn_offers);
            recentBtn = btn_offers;

        }

        private void btn_offers_Click(object sender, EventArgs e)
        {
            openForm(new f_Offers());
            changeOldBtn(recentBtn);
            changeNewBtn(btn_offers);
            recentBtn = btn_offers;
        }

        private void btn_measurements_Click(object sender, EventArgs e)
        {
            openForm(new f_Measurements(mainEmployee, this));
            changeOldBtn(recentBtn);
            changeNewBtn(btn_measurements);
            recentBtn = btn_measurements;
        }

        private void btn_orders_Click(object sender, EventArgs e)
        {
            openForm(new f_Orders(mainEmployee, this));
            changeOldBtn(recentBtn);
            changeNewBtn(btn_orders);
            recentBtn = btn_orders;
        }

        private void btn_installations_Click(object sender, EventArgs e)
        {
            openForm(new f_Installations());
            changeOldBtn(recentBtn);
            changeNewBtn(btn_installations);
            recentBtn = btn_installations;
        }

        private void btn_payments_Click(object sender, EventArgs e)
        {
            openForm(new f_Payments(this));
            changeOldBtn(recentBtn);
            changeNewBtn(btn_payments);
            recentBtn = btn_payments;
        }


        public void openOrderFromComplaint(Order order)
        {
            openForm(new f_Orders(mainEmployee, order, this));
            changeOldBtn(recentBtn);
            changeNewBtn(btn_orders);
            recentBtn = btn_orders;
        }

        public void openOrderFromPayment(Order order)
        {
            openForm(new f_Orders(mainEmployee, order, this));
            changeOldBtn(recentBtn);
            changeNewBtn(btn_orders);
            recentBtn = btn_orders;
        }

        public void openPaymentFromOrder(Payment payment)
        {
            openForm(new f_Payments(this, payment));
            changeOldBtn(recentBtn);
            changeNewBtn(btn_payments);
            recentBtn = btn_payments;
        }

        public void openComplaintFromOrder(Complaint complaint)
        {
            openForm(new f_Complaints(this, complaint));
            changeOldBtn(recentBtn);
            changeNewBtn(btn_complaints);
            recentBtn = btn_complaints;
        }

        private void btn_complaints_Click(object sender, EventArgs e)
        {
            openForm(new f_Complaints(this));
            changeOldBtn(recentBtn);
            changeNewBtn(btn_complaints);
            recentBtn = btn_complaints;
        }
        private void btn_admin_Click(object sender, EventArgs e)
        {
            openForm(new f_AddToDatabase(this));
            changeOldBtn(recentBtn);
            changeNewBtn(btn_admin);
            recentBtn = btn_admin;
        }

        private void btn_stock_Click(object sender, EventArgs e)
        {
            openForm(new f_StockStatus(this));
            changeOldBtn(recentBtn);
            changeNewBtn(btn_stock);
            recentBtn = btn_stock;
        }

        private void openForm(Form form)
        {
            foreach (Control control in panel1.Controls)
            {
                if (control is Form oldForm)
                {
                    oldForm.Dispose(); 
                }
            }

            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            panel1.Controls.Clear();
            panel1.Controls.Add(form);

            form.Show();
        }


        public void changeOldBtn(Guna2Button btn)
        {
            btn.FillColor = System.Drawing.Color.FromArgb(209, 187, 158);
            btn.ForeColor = System.Drawing.Color.White;
        }

        public void changeNewBtn(Guna2Button btn)
        {
            btn.FillColor = System.Drawing.Color.White;
            btn.ForeColor = System.Drawing.Color.FromArgb(167, 146, 119);
        }


    }
}
