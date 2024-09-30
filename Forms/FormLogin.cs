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
    public partial class FormLogin : Form
    {

        public string login = "";
        public string password = "";
        public Employee employee;

        public FormLogin()
        {
            InitializeComponent();
            this.Font = new Font("Arial", 12, FontStyle.Regular);
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
        }


        private void btn_ok_Click(object sender, EventArgs e)
        {
            login = tb_login.Text;
            password = tb_password.Text;

            using (var context = new MyDbConnection())
            {
                employee = context.Employees.SingleOrDefault(o => o.first_name == login && o.password == password);

                if (employee != null)
                {
                    this.DialogResult = DialogResult.OK; 
                    this.Close();
                }
                else
                {
                    tb_password.Clear();
                }
            }

        }

        private void tb_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_ok_Click(this, new EventArgs());
            }
        }
    }
}
