using Castle.Core.Internal;
using Guna.UI2.WinForms;
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WindowsFormsAppMySql.Database;
using WindowsFormsAppMySql.Database.Entities;
using WindowsFormsAppMySql.FileManaging;



namespace WindowsFormsAppMySql.Forms
{
    public partial class f_Complaints : Form
    {
        List<Complaint> complaints;
        List<Database.Entities.Order> orders;
        List<Employee> employees;
        private f_Main mainForm;
        private string searchText = "";
        private int filterCompany = 0;


        List<string> statuses = new List<string> { "Niezaczęte", "W trakcie", "Rozwiązane" };

        public f_Complaints(f_Main mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
            ApplyStyles(this);

            load();

            this.Load += (sender, e) =>
            {
                foreach (DataGridViewRow row in ComplaintsGridView.Rows)
                {
                    row.Selected = false;
                }
            };
        }

        public f_Complaints(f_Main mainForm, Complaint mainComplaint)
        {            
            this.mainForm = mainForm;
            InitializeComponent();
            ApplyStyles(this);

            load();

            this.Load += (sender, e) =>
            {

                selectComplaintsGridView(mainComplaint);
                selectComplaintssCurrentRow();
            };

        }

        private void load()
        {
            end_date_del.Image = Properties.Resources.minus;
            start_date_del.Image = Properties.Resources.minus;
            start_date_cal.Image = Properties.Resources.calendar;
            end_date_cal.Image = Properties.Resources.calendar;

            loadData();

            if (mainForm.isAdmin == 1)
            {
                filterCompany = 0;
            }
            else
            {
                filterCompany = mainForm.company + 1;
            }

            filterTable();

            btn_refresh.Image = Properties.Resources.refresh;


        }

        private void loadData()
        {
            clearClientInfo();
            clearComplaintInfo();

            using (var context = new MyDbConnection())
            {
                complaints = context.Complaints.ToList();
                orders = context.Orders.ToList();
                employees = context.Employees.ToList();

                putDataToTable(complaints);
            }

            ComplaintsGridView.ClearSelection();

        }

        private void setDataInOrder()
        {
            int lastRowIndex = ComplaintsGridView.Rows.Count - 1;
            ComplaintsGridView.ClearSelection();
            ComplaintsGridView.Rows[lastRowIndex].Selected = true;
            ComplaintsGridView.FirstDisplayedScrollingRowIndex = lastRowIndex;
        }

        private void clearComplaintInfo()
        {
            complNumber.Clear();
            supplierNumber.Clear();
            //cb_state.SelectedIndex = -1;
            start_date.Text = "";
            end_date.Text = "";
            notes.Clear();
        }

        private void clearClientInfo()
        {
            name.Clear();
            lastname.Clear();
            phone.Clear();
            mail.Clear();
            address.Clear();

            listView.Clear();
            listView.Columns.Add("Lista plików:", -2, HorizontalAlignment.Left);
        }

        private void putDataToTable(List<Complaint> complaints)
        {
            var complaintsToList = complaints.Select(c => new
            {
                Id = c.id,
                Number = c.displayCompanyNumber(),
                Client =  c.order.client.DisplayNameOnly,
                OrderNumber = c.order_id,
                Status = showStatusName(c.status),
                StartDate = dateTimeToString(c.start_date)
            }).OrderByDescending(c => c.Id).ToList();

            ComplaintsGridView.DataSource = complaintsToList.OrderBy(x => x.Id).ToList();
            ComplaintsGridView.Columns["Id"].Visible = false;

            ComplaintsGridView.ClearSelection();
        }


        private void ComplaintsGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Complaint complaint = getCurrentComplaint();

            if (complaint != null &&  e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = ComplaintsGridView.Rows[e.RowIndex];

                if (selectedRow != null)
                {
                    if (selectedRow.Cells["Id"].Value != null)
                    {
                        int id = Convert.ToInt32(selectedRow.Cells["Id"].Value);

                        using (var context = new MyDbConnection())
                        {
                            Complaint currentComplaint = context.Complaints.SingleOrDefault(c => c.id == id);
                            Database.Client currentClient = currentComplaint.order.client;
                            fillClientInfo(currentClient);
                            fillComplaintInfo(currentComplaint);
                            updateListOfFiles(currentClient);
                        }
                    }
                }
            }
        }

        private void fillComplaintInfo(Complaint complaint)
        {
            complNumber.Text = complaint.displayCompanyNumber();
            supplierNumber.Text = complaint.complaint_producer_number.ToString();
            //cb_state.SelectedIndex = complaint.status;
            start_date.Text = dateTimeToString(complaint.start_date);
            end_date.Text = dateTimeToString(complaint.end_date);
            notes.Text = complaint.notes;
        }

        private void fillClientInfo(Database.Client client)
        {
            name.Text = client.first_name;
            lastname.Text = client.last_name;
            phone.Text = client.phone_number;
            mail.Text = client.email;
            address.Text = client.address;
        }

        private string showStatusName(int status)       // 0 - niezaczęte, 1 - w trakcie, 2 - rozwiązane
        {
            if(status == 0)
            {
                return "Niezaczęte";
            }
            else if (status == 1)
            {
                return "W trakcie";
            }
            else if(status == 2)
            {
                return "Rozwiązane";
            }

            return "";
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (ComplaintsGridView.SelectedRows.Count == 1)
            {
                int id = Convert.ToInt32(ComplaintsGridView.SelectedRows[0].Cells["Id"].Value);
                using (var context = new MyDbConnection())
                {
                    Complaint complaint = context.Complaints.Find(id);
                    complaint.notes = notes.Text;
                    complaint.complaint_producer_number = supplierNumber.Text;
                    


                    if(start_date.Text.IsNullOrEmpty())
                    {
                        complaint.status = 0;
                        complaint.start_date = null;
                    }
                    else
                    {
                        complaint.status = 1;
                        complaint.start_date = stringToDateTime(start_date.Text);
                        //addInstallation(complaint);

                    }

                    if (end_date.Text.IsNullOrEmpty())
                    {
                        complaint.end_date = null;
                    }
                    else
                    {
                        complaint.status = 2; 
                        complaint.end_date = stringToDateTime(end_date.Text);
                    }


                    if (!string.IsNullOrEmpty(complNumber.Text))
                    {
                        string number = complNumber.Text.ToLower();
                        int company;

                        company = number.Contains("p") ? 1 : 0;
                        number = number.Replace("p", "").Replace(" ", "");

                        if (int.TryParse(number, out int nr))
                        {
                            complaint.complaint_number = nr;
                            complaint.company_number = company;
                        }
                    }

                    context.SaveChanges();


                    Complaint currentComplaint = getCurrentComplaint();
                    loadData();
                    selectComplaintsGridView(currentComplaint);
                    selectComplaintssCurrentRow();
                }
            }
        }

        private void addInstallation(Complaint complaint)
        {
            Installation complaintInstallation = new Installation();
            complaintInstallation.order_id = complaint.order_id;
            complaintInstallation.client_id = complaint.order.client_id;
            complaintInstallation.installation_address = complaint.order.installation_address;
            complaintInstallation.notes = "Reklamacja " + complaint.displayCompanyNumber();


            using (var context = new MyDbConnection())
            {
                context.Installations.Add(complaintInstallation);
                context.SaveChanges();
            }
        }

        private void selectComplaintsGridView(Complaint c)
        {
            if (ComplaintsGridView.Rows.Count == 0)
            {
                return; // Zakończ, jeśli tabela jest pusta
            }

            foreach (DataGridViewRow row in ComplaintsGridView.Rows)
            {
                row.Selected = false;
            }

            foreach (DataGridViewRow row in ComplaintsGridView.Rows)
            {
                if (row.Cells["Id"] != null && row.Cells["Id"].Value != null && row.Cells["Id"].Value is int && (int)row.Cells["Id"].Value == c.id)
                {
                    row.Selected = true;
                    ComplaintsGridView.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
            }
        }

        private void selectComplaintssCurrentRow()
        {
            int index = getCurrentIndex();

            if (index >= 0 && index < ComplaintsGridView.Rows.Count)
            {
                ComplaintsGridView.FirstDisplayedScrollingRowIndex = index;
                ComplaintsGridView_CellClick(ComplaintsGridView, new DataGridViewCellEventArgs(0, index));
            }
        }

        private void onlyDigitsWithP_TextChanged(object sender, EventArgs e)
        {

            Guna2TextBox textBox = sender as Guna2TextBox;
            Regex regex = new Regex("^[0-9pP]*$");

            if (!regex.IsMatch(textBox.Text))
            {
                int selectionStart = textBox.SelectionStart - 1;
                textBox.Text = regex.Replace(textBox.Text, "");
                textBox.SelectionStart = selectionStart >= 0 ? selectionStart : 0;
            }
        }

        private Complaint getCurrentComplaint()
        {
            if (ComplaintsGridView.SelectedRows.Count > 0)
            {
                var selectedRow = ComplaintsGridView.SelectedRows[0];
                var complaintId = (int)selectedRow.Cells["Id"].Value;
                var selectedComplaint = complaints.FirstOrDefault(c => c.id == complaintId);
                return selectedComplaint;
            }
            return null;
        }

        private int getCurrentIndex()
        {
            if (ComplaintsGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = ComplaintsGridView.SelectedRows[0];
                return selectedRow.Index;
            }
            else
            {
                return -1;
            }
        }

        private void listView_DoubleClick(object sender, EventArgs e)
        {
            Complaint c = getCurrentComplaint();

            DirectoryManager dm = new DirectoryManager(c.order.client_id, c.order.client.first_name, c.order.client.last_name);
            dm.openFile(listView.SelectedItems[0].Text);
        }


        private void updateListOfFiles(Client client)
        {
            int amountOffiles = 0;

            listView.Clear();
            listView.Columns.Add("Lista plików:", -2, HorizontalAlignment.Left);

            DirectoryManager dm = new DirectoryManager(client.id, client.first_name, client.last_name);
            foreach (string s in dm.loadFilesToList())
            {
                ListViewItem item = new ListViewItem(new FileInfo(s).Name);
                listView.Items.Add(item);
                amountOffiles++;
            }

            if (amountOffiles > 0)
            {
                listView.Columns[0].Width = listView.Width - 4 - SystemInformation.VerticalScrollBarWidth;
            }

        }


        private void ComplaintsGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (ComplaintsGridView.Columns[e.ColumnIndex].Name == "Status")
            {
                if (e.Value != null && e.Value is string status)
                {
                    switch (status)
                    {
                        case "Niezaczęte":
                            ComplaintsGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(211, 118, 118);
                            break;
                        case "W trakcie":
                            ComplaintsGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(235, 196, 159);
                            break;
                        case "Rozwiązane":
                            ComplaintsGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(176, 197, 164);
                            break;
                        default:
                            ComplaintsGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                            break;
                    }
                }
            }
        }



        private void ComplaintsGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Complaint c = getCurrentComplaint();
            if(c != null)
            {
                Database.Entities.Order o = c.order;
                if (o != null)
                {
                    mainForm.openOrderFromComplaint(o);

                    loadData();
                    selectComplaintsGridView(c);
                    selectComplaintssCurrentRow();
                }
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            loadData();
            search.Clear();
            filterCompany = mainForm.company + 1;
            btn_premium.Text =getCompanyName(mainForm.company);
            filterTable();
        }

        private string getCompanyName(int company)
        {
            if (company == 0)
            {
                return "Tomkar";

            }
            else if (company == 1)
            {
                return "Premium";
            }

            return "Wszystkie";
        }


        private void btn_save_C_Click(object sender, EventArgs e)
        {
            Complaint complaint = getCurrentComplaint();

            using(var context = new MyDbConnection())
            {
                Database.Client client = context.Clients.FirstOrDefault(c => c.id == complaint.order.client_id);
                client.first_name = name.Text;
                client.last_name = lastname.Text;
                client.phone_number = phone.Text;
                client.email = mail.Text;
                client.address = address.Text;

                context.SaveChanges();
            }
        }


        private void btn_openfolder_C_Click(object sender, EventArgs e)
        {
            Complaint currentComplaint = getCurrentComplaint();

            if (currentComplaint != null && currentComplaint.order != null)
            {
                Database.Client currentOrderClient = currentComplaint.order.client;

                if (currentOrderClient != null)
                {
                    DirectoryManager dm = new DirectoryManager(currentOrderClient.id, currentOrderClient.first_name, currentOrderClient.last_name);
                    dm.openDirectory();
                }
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
                    gv.RowTemplate.MinimumHeight = 25;

                    gv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;                              
                    gv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

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
                    textBox.ForeColor = Color.Black;
                }

                if (ctrl is MetroDateTime)
                {
                    MetroDateTime dt = (MetroDateTime)ctrl;
                    dt.MinimumSize = new Size(0, 0);
                    dt.Width = 0;
                }

                // Rekurencyjne wywołanie dla podkontrolek
                if (ctrl.HasChildren)
                {
                    ApplyStyles(ctrl);
                }
            }
        }

        private void add_files_Click(object sender, EventArgs e)
        {
            Complaint c = getCurrentComplaint();

            if (c != null)
            {
                List<string> files;
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

                        DirectoryManager dm = new DirectoryManager(c.order.client.id, c.order.client.first_name, c.order.client.last_name);

                        if (files != null && !files.IsNullOrEmpty())
                        {
                            foreach (string file in files)
                            {
                                dm.copyFileToFolder(file);
                            }
                        }
                    }
                }

            }
            updateListOfFiles(c.order.client);

        }

        private void filterTable()
        {
            List<Complaint> filteredComplaints = new List<Complaint>();

            if (filterCompany == 0)              // wszystkie
            {
                btn_premium.Text = "Wszystkie";
                filteredComplaints = complaints;
            }
            else if (filterCompany == 1)         // tomkar
            {
                btn_premium.Text = "Tomkar";
                filteredComplaints.AddRange(complaints.Where(c => c.order.company_number == 0).ToList());
            }
            else if (filterCompany == 2)         // premium
            {
                btn_premium.Text = "Premium";
                filteredComplaints.AddRange(complaints.Where(c => c.order.company_number == 1).ToList());
            }


            filteredComplaints = filteredComplaints.Where(c => c.id.ToString().Contains(searchText) ||
                                    c.order.client.DisplayNameOnly.ToLower().Contains(searchText) ||
                                    c.order.orderCompanyNumber().ToLower().Contains(searchText) ||
                                    statuses.ElementAt(c.status).ToLower().Contains(searchText) ||
                                    c.complaint_producer_number.ToLower().Contains(searchText)
                                    ).ToList();


            putDataToTable(filteredComplaints);

            setDataInOrder();
        }

        private void search_TextChanged(object sender, EventArgs e)
        {
            searchText = search.Text.ToLower();
            filterTable();
        }

        private void btn_premium_Click(object sender, EventArgs e)
        {
            filterCompany += 1;
            filterCompany = filterCompany % 3;

            filterTable();
        }

        public DateTime? stringToDateTime(string date)
        {
            if (string.IsNullOrEmpty(date)) return null;
            else return DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        }

        public string dateTimeToString(DateTime? date)
        {
            if (!date.HasValue)
                return "";
            else
                return date.Value.ToString("dd.MM.yyyy");
        }

        private void start_date_cal_Click(object sender, EventArgs e)
        {
            dt_start_date.Location = new Point(start_date_cal.Location.X, start_date_cal.Location.Y + start_date_cal.Height);
            dt_start_date.Visible = true;
            dt_start_date.Focus();
            SendKeys.Send("{F4}");
        }

        private void end_date_cal_Click(object sender, EventArgs e)
        {
            dt_end_date.Location = new Point(end_date_cal.Location.X, end_date_cal.Location.Y + end_date_cal.Height);
            dt_end_date.Visible = true;
            dt_end_date.Focus();
            SendKeys.Send("{F4}");
        }

        private void dt_start_date_CloseUp(object sender, EventArgs e)
        {
            DateTime selected = dt_start_date.Value;
            start_date.Text = dateTimeToString(selected);
        }

        private void dt_end_date_CloseUp(object sender, EventArgs e)
        {
            DateTime selected = dt_end_date.Value;
            end_date.Text = dateTimeToString(selected);
        }


        private void start_date_del_Click(object sender, EventArgs e)
        {
          start_date.Text = "";
        }

        private void end_date_del_Click(object sender, EventArgs e)
        {
            end_date.Text = "";
        }

        private void addToInstallations_Click(object sender, EventArgs e)
        {
            Complaint c = getCurrentComplaint();

            if (c != null)
            {
                addInstallation(c);
            }
        }


        private void ComplaintsGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Complaint c = getCurrentComplaint();

            if (c != null)
            {
                if (ComplaintsGridView.Rows[e.RowIndex].Selected)
                {
                    Rectangle rect = e.RowBounds;

                    using (Pen pen = new Pen(Color.Black, 3))
                    {
                        int penWidth = 3;
                        rect.X += penWidth / 2;
                        rect.Y += penWidth / 2;
                        rect.Width -= penWidth;
                        rect.Height -= penWidth;

                        e.Graphics.DrawRectangle(pen, rect);
                    }
                }
            }
        }
    }
}
