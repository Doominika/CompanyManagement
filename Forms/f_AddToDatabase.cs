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
using WindowsFormsAppMySql.Database;
using Guna.UI2.WinForms;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace WindowsFormsAppMySql.Forms
{
    public partial class f_AddToDatabase : Form
    {
        private f_Main mainForm;

        List<Employee> employees;
        List<Product> products;


        public f_AddToDatabase(f_Main mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
            ApplyStyles(this);

            loadData();
        }

        private void loadData()
        {
            clearEmployeeInfo();
            

            using (var context = new MyDbConnection())
            {
                employees = context.Employees.Where(e => e.role == 0 || e.role == 1).ToList();
                products = context.Products.Where(p => p.visible == 1).ToList();

                putDataToTable(employees);
                putDataToTableP(products);
            }
        }

        private void putDataToTableP(List<Product> products)
        {
            var complaintsToList = products.Select(p => new
            {
                Id = p.id,
                Product = p.name
            }).OrderByDescending(p => p.Id).ToList();

            ProductGridView.DataSource = complaintsToList;
            ProductGridView.Columns["Id"].Visible = false;
        }

        private void clearEmployeeInfo()
        {
            empl_name.Clear();
            empl_pass.Clear();
            role0.Checked = false;  
            role1.Checked = false; 
            admin0.Checked = false;
            admin1.Checked = false; 
            company0.Checked = false;
            company1.Checked = false;

            btn_delete.Visible = false;
            btn_save.Dock = DockStyle.Fill;

            btn_delete_product.Visible = false;
            btn_save_product.Dock = DockStyle.Fill;
        }

        private void putDataToTable(List<Employee> employees)
        {
            var complaintsToList = employees.Select(e => new
            {
                Id = e.id,
                Name = e.first_name,
                Role = e.role == 0 ? "Biuro" : "Montaż",
                Admin = e.admin == 0 ? "Nie" : "Tak",
                Company = e.company == 0 ? "Tomkar" : "Premium",
                Password = e.password

            }).OrderByDescending(e => e.Id).ToList();

            EmployeeGridView.DataSource = complaintsToList;
            EmployeeGridView.Columns["Id"].Visible = false;
        }

        private void role0_CheckedChanged(object sender, EventArgs e)
        {
            role1.Checked = false;
        }

        private void role1_CheckedChanged(object sender, EventArgs e)
        {
            role0.Checked = false;
        }

        private void admin0_CheckedChanged(object sender, EventArgs e)
        {
            admin1.Checked = false;
        }

        private void admin1_CheckedChanged(object sender, EventArgs e)
        {
            admin0.Checked = false;
        }

        private void company0_CheckedChanged(object sender, EventArgs e)
        {
            company1.Checked = false;
        }

        private void company1_CheckedChanged(object sender, EventArgs e)
        {
            company0.Checked = false;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if(btn_delete.Visible == false && getCurrentEmployee() != null)
            { 
                Employee employee = new Employee();

                if (role0.Checked)
                {
                    employee.role = 0;
                }
                else if (role1.Checked)
                {
                    employee.role = 1;
                }

                if (admin0.Checked)
                {
                    employee.admin = 0;
                }
                else if (admin1.Checked)
                {
                    employee.admin = 1;
                }

                if (company0.Checked)
                {
                    employee.company = 0;
                }
                else if (company1.Checked)
                {
                    employee.company = 1;
                }

                employee.first_name = empl_name.Text;
                employee.password = empl_pass.Text;

                using (var context = new MyDbConnection())
                {
                    context.Employees.Add(employee);
                    context.SaveChanges();
                }
            }
            else      // update
            {
                if(getCurrentEmployee() != null)
                {
                    using (var context = new MyDbConnection())
                    {
                        Employee employee = context.Employees.Find(getCurrentEmployee().id);

                        if (role0.Checked)
                        {
                            employee.role = 0;
                        }
                        else if (role1.Checked)
                        {
                            employee.role = 1;
                        }

                        if (admin0.Checked)
                        {
                            employee.admin = 0;
                        }
                        else if (admin1.Checked)
                        {
                            employee.admin = 1;
                        }

                        if (company0.Checked)
                        {
                            employee.company = 0;
                        }
                        else if (company1.Checked)
                        {
                            employee.company = 1;
                        }

                        employee.first_name = empl_name.Text;
                        employee.password = empl_pass.Text;

                        context.SaveChanges();
                    }
                }
            }
            clearEmployeeInfo();
            loadData();

        }

        private Employee getCurrentEmployee()
        {
            if (EmployeeGridView.SelectedRows.Count > 0)
            {
                var selectedRow = EmployeeGridView.SelectedRows[0];
                var id = (int)selectedRow.Cells["Id"].Value;
                var selected = employees.FirstOrDefault(b => b.id == id);
                return selected;
            }
            return null;
        }

        private Product getCurrentProduct()
        {
            if (ProductGridView.SelectedRows.Count > 0)
            {
                var selectedRow = ProductGridView.SelectedRows[0];
                var id = (int)selectedRow.Cells["Id"].Value;
                var selected = products.FirstOrDefault(b => b.id == id);
                return selected;
            }
            return null;
        }

        private void btn_save_product_Click(object sender, EventArgs e)
        {

            
            if (btn_delete_product.Visible == false)  //dodawanie
            {

                Product product = new Product();
                product.name = produkt_name.Text;

                using (var context = new MyDbConnection())
                {
                    context.Products.Add(product);
                    context.SaveChanges();
                }

            }
            else if(getCurrentProduct() != null)    // edycja
            {
                using (var context = new MyDbConnection())
                {
                    Product product = context.Products.Find(getCurrentProduct().id);
                    product.name = produkt_name.Text;
                    context.SaveChanges();
                }
               
            }

            produkt_name.Clear();
            loadData();

        }

        private void ProductGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (ProductGridView.SelectedRows.Count > 0)
            {
                var selectedRow = ProductGridView.SelectedRows[0];
                var Id = (int)selectedRow.Cells["Id"].Value;
                var selected = products.FirstOrDefault(p => p.id == Id);

                if (selected != null)
                {
                    btn_delete_product.Visible = true;
                    btn_save_product.Dock = DockStyle.Right;

                    produkt_name.Text = selected.name;

                }
            }

            /*
            if (ProductGridView.SelectedRows.Count > 0)
            {
                var selectedRow = ProductGridView.SelectedRows[0];
                var Id = (int)selectedRow.Cells["Id"].Value;
                var selected = products.FirstOrDefault(p => p.id == Id);

                if(selected != null)
                {
                    using (var context = new MyDbConnection())
                    {
                        context.Products.Remove(context.Products.FirstOrDefault(p => p.id == Id));
                        context.SaveChanges();
                    }
                }

                loadData();
            }
            */
        }

        private void EmployeeGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (EmployeeGridView.SelectedRows.Count > 0)
            {
                var selectedRow = EmployeeGridView.SelectedRows[0];
                var Id = (int)selectedRow.Cells["Id"].Value;
                var selected = employees.FirstOrDefault(p => p.id == Id);

                if (selected != null)
                {
                    btn_delete.Visible = true;
                    btn_save.Dock = DockStyle.Right;

                    if (selected.admin == 0) admin0.Select();
                    else if(selected.admin == 1) admin1.Select();

                    if (selected.role == 0) role0.Select();
                    else if (selected.role == 1) role1.Select();

                    if (selected.company == 0) company0.Select();
                    else if (selected.company == 1) company1.Select();

                    empl_name.Text = selected.first_name;
                    empl_pass.Text = selected.password;

                }

               // loadData();
            }
        }

        private void ApplyStyles(Control control)
        {
            foreach (Control ctrl in control.Controls)
            {
                if (ctrl is Guna2DataGridView)
                {
                    Guna2DataGridView gv = (Guna2DataGridView)ctrl;
                    gv.ColumnHeadersDefaultCellStyle.Font = new Font("Calibri", 10, FontStyle.Bold);
                    gv.DefaultCellStyle.Font = new Font("Calibri", 10);
                    gv.ColumnHeadersHeight = 30;

                    //FF6464
                    gv.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(141, 156, 99);
                    gv.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                    gv.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(141, 156, 99);

                    // Zmiana koloru wierszy
                    gv.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(249, 234, 225);
                    gv.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;

                    // Opcjonalnie zmiana kolorów wierszy przy najechaniu kursorem
                    gv.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(249, 234, 225);
                    gv.AlternatingRowsDefaultCellStyle.Font = new Font("Calibri", 10);
                    gv.RowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);

                    gv.MultiSelect = false;
                    gv.AllowUserToResizeRows = false;


                    gv.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);

                    gv.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(245, 219, 204);
                    gv.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(0, 0, 0);


                    gv.AlternatingRowsDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(245, 219, 204);
                    gv.AlternatingRowsDefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(0, 0, 0);

                }

                if (ctrl is Guna2Button)
                {
                    Guna2Button btn = (Guna2Button)ctrl;

                    btn.BorderRadius = 0;
                    btn.BorderThickness = 0;

                    if (btn.Text.Length <= 0)
                    {
                        btn.TextAlign = HorizontalAlignment.Right;
                    }

                }

                if (ctrl is Guna2TextBox)
                {
                    Guna2TextBox textBox = (Guna2TextBox)ctrl;

                    textBox.Font = new Font("Calibri", 12);
                    textBox.ForeColor = System.Drawing.Color.Black;
                }

                // Rekurencyjne wywołanie dla podkontrolek
                if (ctrl.HasChildren)
                {
                    ApplyStyles(ctrl);
                }
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (EmployeeGridView.SelectedRows.Count > 0)
            {
                var selectedRow = EmployeeGridView.SelectedRows[0];
                var Id = (int)selectedRow.Cells["Id"].Value;
                Employee selected = employees.FirstOrDefault(p => p.id == Id);

                if (selected != null)
                {
                    //btn_delete.Visible = true;
                    //btn_save.Dock = DockStyle.Right;
                    
                    using (var context = new MyDbConnection())
                    {

                        context.Employees.Find(selected.id).role = 3;
                        context.SaveChanges();
                    }
                    
                }

                 loadData();
            }
        }

        private void btn_delete_product_Click(object sender, EventArgs e)
        {
            if (ProductGridView.SelectedRows.Count > 0)
            {
                var selectedRow = ProductGridView.SelectedRows[0];
                var Id = (int)selectedRow.Cells["Id"].Value;
                var selected = products.FirstOrDefault(p => p.id == Id);

                if (selected != null)
                {
                    using (var context = new MyDbConnection())
                    {
                        context.Products.Find(selected.id).visible = 0;  
                        context.SaveChanges();
                    }
                }
                produkt_name.Clear();
                loadData();
            }
        }
    }
}
