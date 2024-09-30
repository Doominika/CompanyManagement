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
using WindowsFormsAppMySql.FileManaging;

namespace WindowsFormsAppMySql.Forms
{
    public partial class FormAddClient : Form
    {
        private Employee employee;
        public FormAddClient(Employee employee)
        {
            InitializeComponent();
            this.employee = employee;
        }

        public FormAddClient()
        {
            InitializeComponent();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            Client client = new Client();
            client.first_name = name.Text;
            client.last_name = lastname.Text;
            client.email = mail.Text;
            client.address = address.Text;
            client.phone_number = phone.Text;

            using (var context = new MyDbConnection())
            {
                context.Clients.Add(client);
                context.SaveChanges();

                DirectoryManager dm = new DirectoryManager(client.id, client.first_name, client.last_name);
                dm.createDirectory();
            }

           

            this.Close();
        }
    }
}
