using Castle.Core.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsAppMySql.Database;
using WindowsFormsAppMySql.Database.Entities;
using WindowsFormsAppMySql.FileManaging;

namespace WindowsFormsAppMySql.Forms
{
    public partial class FormAddComplaint : Form
    {
        private Order order;
        private List<string> files;

        public FormAddComplaint(Order order)
        {
            InitializeComponent();
            this.order = order;
        }

        private void FormAddComplaint_Load(object sender, EventArgs e)
        {

        }

        private void btn_addFiles_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    files = openFileDialog.FileNames.ToList();
                }
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            using (var context = new MyDbConnection())
            {
                Complaint c = new Complaint();
                c.status = 0;
                c.complaint_producer_number = supplierNumber.Text;
                c.order_id = order.id;
                c.company_number = order.company_number;
                c.notes = notes.Text;
                c.start_date = DateTime.Now;

                if(complNumber.Text.IsNullOrEmpty())
                {
                    int? maxId = context.Complaints
                    .Where(x => x.company_number == order.company_number)
                    .Select(x => (int?)x.id) 
                    .Max();

                    int finalMaxId = maxId ?? 0;

                    c.complaint_number = finalMaxId + 1;
                }
                else
                {
                    string number = complNumber.Text.ToLower();
                    int company;

                    if (number.Contains("p"))
                    {
                        company = 1;
                    }
                    else
                    {
                        company = 0;
                    }

                    number = number.Replace("p", "").Replace(" ", "");
                    if (int.TryParse(number, out int nr))
                    {
                        c.complaint_number = nr;
                        c.company_number = company;
                    }
                    else
                    {

                        int? maxId = context.Complaints
                       .Where(x => x.company_number == order.company_number)
                       .Select(x => (int?)x.id)
                       .Max();

                        int finalMaxId = maxId ?? 0;
                        c.complaint_number = finalMaxId + 1;
                        c.company_number = company;
                    }
                }

                context.Complaints.Add(c);
                context.SaveChanges();

                DirectoryManager dm = new DirectoryManager(order.client.id, order.client.first_name, order.client.last_name);

                if (files != null && !files.IsNullOrEmpty())
                {
                    foreach (string file in files)
                    {
                        dm.copyFileToFolder(file);
                    }
                }
            }

            this.Close();
        }
    }
}
