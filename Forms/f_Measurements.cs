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
using WindowsFormsAppMySql.FileManaging;
using System.Globalization;
using Client = WindowsFormsAppMySql.Database.Client;
using Guna.UI2.WinForms;
using System.Text.RegularExpressions;
using MetroFramework.Controls;
using System.Data.Entity;

namespace WindowsFormsAppMySql.Forms
{
    public partial class f_Measurements : Form
    {
        private Employee mainEmployee;
        private f_Main mainForm;

        private string searchText = "";
        public int filterCompanyMeasurements = 0;       //0 - wszystkie, 1 - tomkar, 2 - premium

        private int registration_click = 0;
        private int installation_click = 0;

        private int clickedRowIndex = -1;

        List<Measurement> measurements = new List<Measurement>();
        List<Measurement> filteredMeasurements = new List<Measurement>();
        List<Employee> employees = new List<Employee>();

        public f_Measurements(Employee employee, f_Main mainForm)
        {
            this.mainEmployee = employee;
            this.mainForm = mainForm;

            InitializeComponent();
            ApplyStyles(this);

            load();
        }

        private void load()
        {
            loadDataAsync();

            //eventy
            cb_employee.MouseWheel += new MouseEventHandler(preventMouseWheel);


            //obrazy
            btn_cal_M.Image = Properties.Resources.calendar;
            btn_calrm_M.Image = Properties.Resources.minus;
            btn_add_M.Image = Properties.Resources.plus;
            btn_refresh_M.Image = Properties.Resources.refresh;
            registrationdaterm.Image = Properties.Resources.minus;
            btn_date_startrm.Image = Properties.Resources.minus;
            btn_date_endrm.Image = Properties.Resources.minus;
            btn_registrationdate.Image = Properties.Resources.calendar;
            btn_date_start.Image = Properties.Resources.calendar;
            btn_date_end.Image = Properties.Resources.calendar;


            cb_employee.DataSource = employees.Where(e => e.role == 0).ToList();
            cb_employee.DisplayMember = "first_name";
            cb_employee.ValueMember = "id";
            cb_employee.SelectedIndex = -1;

            MeasurementsGridView.Columns["Employee"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            MeasurementsGridView.Columns["PriceToPay"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            MeasurementsGridView.Columns["Number"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            MeasurementsGridView.Columns["Number"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            MeasurementsGridView.Columns["Done"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void preventMouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private async Task loadDataAsync()
        {
            clearMeasurementClientInfo();
            clearMeasurementInfo();

            using (var context = new MyDbConnection())
            {

                var uncheckedMeasurements = await context.Measurements
                    .Where(m => m.is_checked_if_ordered == 0)
                    .ToListAsync();

                foreach (Measurement m in uncheckedMeasurements)
                {
                    Order order = await context.Orders
                        .FirstOrDefaultAsync(o => o.measurement_id == m.id && o.order_date != null);

                    if (order != null)
                    {
                        m.is_checked_if_ordered = 1;
                        m.color_rgb = "#9bb0c1";
                    }
                }

                await context.SaveChangesAsync();

                employees = await context.Employees.ToListAsync();

                measurements = await context.Measurements
                    .Include(m => m.client)
                    .Include(m => m.employee)
                    .ToListAsync();



                if (mainForm.isAdmin == 1)
                {
                    filterCompanyMeasurements = 0;
                    filteredMeasurements = measurements.OrderBy(m => m.id).ToList();
                }
                else
                {
                    filterCompanyMeasurements = mainForm.company + 1;
                    filteredMeasurements = measurements.Where(o => o.employee.company == mainForm.company).OrderBy(m => m.id).ToList();
                }

                btn_premium_M.Text = getCompanyName(filterCompanyMeasurements);

                if (filteredMeasurements.Count <= 0) MeasurementsGridView.RowCount = 0;
                else MeasurementsGridView.RowCount = filteredMeasurements.Count;

                MeasurementsGridView.VirtualMode = true;

                setDataInOrder();
            }




        }

        private async void loadFromOtherWindow(Measurement measurement)
        {
            await loadDataAsync();
            selectMeasurementsGridView(measurement);
            selectMeasurementsCurrentRow();
        }

        private void setDataInOrder()
        {
            MeasurementsGridView.Refresh();
            int lastRowIndex = MeasurementsGridView.Rows.Count - 1;
            MeasurementsGridView.ClearSelection();
            MeasurementsGridView.FirstDisplayedScrollingRowIndex = lastRowIndex;
            MeasurementsGridView.Rows[lastRowIndex].Selected = true;
        }

        private void clearMeasurementInfo()
        {
            address_M.Clear();
            date_M.Text = "";
            status_M.Clear();
            products.Clear();
            information.Clear();
            is_paid.Text = "☐";
            installation_price.Clear();
        }

        private void clearMeasurementClientInfo()
        {
            name_MC.Clear();
            lastname_MC.Clear();
            phone_MC.Clear();
            address_MC.Clear();
            mail_MC.Clear();
        }

        private void MeasurementsGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Measurement m = getCurrentMeasurement();

            if (e.RowIndex > -1 && e.ColumnIndex > -1 && m != null)
            {
                int rowIndex = e.RowIndex;
                Measurement measurement = filteredMeasurements[rowIndex];
                int id = measurement.id;

                using (var context = new MyDbConnection())
                {
                    var currentMeasurement = context.Measurements.SingleOrDefault(o => o.id == id);
                    if (currentMeasurement != null)
                    {
                        var currentMeasurementClient = currentMeasurement.client;
                        fillMeasurementClientInfo(currentMeasurementClient);
                        fillMeasurementInfo(currentMeasurement);
                    }
                }
            }
        }

        public string priceToString(double price)
        {
            return price.ToString("F2");
        }

        public double stringToPrice(string price)
        {
            if (double.TryParse(price, out double result))
            {
                return result;
            }

            return 0.00;
        }

        private void fillMeasurementInfo(Measurement measurement)
        {
            clearMeasurementInfo();

            if (measurement != null)
            {
                address_M.Text = measurement.measurement_address;
                if (measurement.measurement_date != null)
                {
                    date_M.Text = dateTimeToString(measurement.measurement_date.Value);
                }
                status_M.Text = measurement.status;
                products.Text = measurement.notes;
                information.Text = measurement.notes_information;
                cb_employee.SelectedValue = measurement.employee_id;
                installation_price.Text = priceToString(measurement.price);
                if (measurement.registration_date != null)
                {
                    registration_date.Text = dateTimeToString(measurement.registration_date.Value);
                }

                // ☐  ☑   ☒
                if (measurement.is_paid == 1)
                {
                    is_paid.Text = "☒";
                }
                else
                {
                    is_paid.Text = "☐";
                }
            }


        }

        private void fillMeasurementClientInfo(Database.Client client)
        {
            clearMeasurementClientInfo();

            if (client != null)
            {
                name_MC.Text = client.first_name;
                lastname_MC.Text = client.last_name;
                phone_MC.Text = client.phone_number;
                address_MC.Text = client.address;
                mail_MC.Text = client.email;
            }
        }

        private void openCalendarM(object sender, EventArgs e)
        {
            dateTime_M.Location = new Point(btn_cal_M.Location.X, btn_cal_M.Location.Y + btn_cal_M.Height);
            dateTime_M.Visible = true;
            dateTime_M.Focus();
            SendKeys.Send("{F4}");
        }

        private void closeCalendarM(object sender, EventArgs e)
        {
            DateTime selectedDate = dateTime_M.Value;
            date_M.Text = dateTimeToString(selectedDate);
        }

        private void btn_daterm_M_Click(object sender, EventArgs e)
        {
            date_M.Text = "";
            dateTime_M.Value = DateTime.Now;
        }


        private void openCalendarRegistration(object sender, EventArgs e)
        {
            dt_registrationdate.Location = new Point(btn_registrationdate.Location.X, btn_registrationdate.Location.Y + btn_registrationdate.Height);
            dt_registrationdate.Visible = true;
            dt_registrationdate.Focus();
            SendKeys.Send("{F4}");
        }

        private void closeCalendarRegistration(object sender, EventArgs e)
        {
            DateTime selectedDate = dt_registrationdate.Value;
            registration_date.Text = dateTimeToString(selectedDate);
        }

        private void registrationdaterm_Click(object sender, EventArgs e)
        {
            btn_registrationdate.Text = "";
            dt_registrationdate.Value = DateTime.Now;
        }


        private void btn_add_M_Click(object sender, EventArgs e)
        {
            //open new window
            FormAddMeasurement newForm = new FormAddMeasurement(mainEmployee);

            newForm.FormClosing += OtherForm_FormClosing;
            newForm.Show();
        }

        private void OtherForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            loadDataAsync();
        }

        private void btn_premium_M_Click(object sender, EventArgs e)
        {
            // 0 - wszystkie, 1 - tomkar, 2 - premium
            filterCompanyMeasurements += 1;
            filterCompanyMeasurements = filterCompanyMeasurements % 3;

            filterTable();
        }

        private void filterTable()
        {
            List<Measurement> filtered = new List<Measurement>(measurements);


            // pasek wyszukiwania
            if (!string.IsNullOrEmpty(searchText))
            {
                filtered = filtered
                   .Where(m => m.id.ToString().ToLower().Contains(searchText) ||
                    (m.client?.first_name ?? "").ToLower().Contains(searchText) ||
                    (m.client?.last_name ?? "").ToLower().Contains(searchText) ||
                    (m.client?.phone_number ?? "").ToLower().Contains(searchText) ||
                    (m.measurement_address ?? "").ToLower().Contains(searchText) ||
                    (m.status ?? "").ToLower().Contains(searchText) ||
                    (dateTimeToString(m.measurement_date) ?? "").ToLower().Contains(searchText)
                ).ToList();
            }


            // przycisk 
            switch (filterCompanyMeasurements)
            {
                case 0:
                    btn_premium_M.Text = "Wszystkie";
                    break;
                case 1:
                    filtered = filtered.Where(m => m.employee.company == 0).ToList();
                    btn_premium_M.Text = "Tomkar";
                    break;
                case 2:
                    filtered = filtered.Where(m => m.employee.company == 1).ToList();
                    btn_premium_M.Text = "Premium";
                    break;
            }


            filteredMeasurements = filtered.OrderBy(m => m.id).ToList();

            if (filteredMeasurements.Count > 0)
            {
                MeasurementsGridView.RowCount = filteredMeasurements.Count;
            }
            else
            {
                MeasurementsGridView.RowCount = 0;
            }

            MeasurementsGridView.Refresh();
            MeasurementsGridView.Invalidate();

            setDataInOrder();
        }

        private void btn_refresh_M_Click(object sender, EventArgs e)
        {
            //loadDataMeasurements();
            loadDataAsync();
            //filterCompanyMeasurements = mainForm.company + 1;
            //btn_premium_M.Text = getCompanyName(mainForm.company);

            if (mainForm.isAdmin == 1) filterCompanyMeasurements = 0;
            else filterCompanyMeasurements = mainForm.company + 1;

            btn_premium_M.Text = getCompanyName(filterCompanyMeasurements);
            search.Clear();
        }

        private string getCompanyName(int company)
        {
            if (company == 1)
            {
                return "Tomkar";

            }
            else if (company == 2)
            {
                return "Premium";
            }

            return "Wszystkie";
        }

        private void btn_save_M_Click(object sender, EventArgs e)
        {
            //clearMeasurementInfo();
            Measurement currentMeasurement = getCurrentMeasurement();

            if (currentMeasurement != null)
            {
                using (var context = new MyDbConnection())
                {

                    if (currentMeasurement != null)
                    {
                        Measurement measurementToSave = context.Measurements.Find(currentMeasurement.id);
                        measurementToSave.measurement_address = address_M.Text;
                        measurementToSave.status = status_M.Text;
                        measurementToSave.measurement_date = stringToDateTime(date_M.Text);
                        measurementToSave.notes = products.Text;

                        if (cb_employee.SelectedValue != null)
                        {
                            int id = (int)cb_employee.SelectedValue;
                            measurementToSave.employee_id = id;
                        }

                        measurementToSave.registration_date = stringToDateTime(registration_date.Text);
                        measurementToSave.notes_information = information.Text;

                        if (double.TryParse(installation_price.Text, out double price))
                        {
                            measurementToSave.price = price;
                        }

                        if (is_paid.Text == "☒")                // ☐  ☑   ☒
                        {
                            measurementToSave.is_paid = 1;
                        }
                        else
                        {
                            measurementToSave.is_paid = 0;
                        }

                        context.SaveChanges();
                    }
                }

                //loadDataMeasurements();
                //selectMeasurementsGridView(currentMeasurement);
                //selectMeasurementsCurrentRow();
                loadFromOtherWindow(currentMeasurement);

            }
        }

        private void btn_save_MC_Click(object sender, EventArgs e)
        {
            Measurement currentMeasurement = getCurrentMeasurement();
            if (currentMeasurement != null)
            {
                Client currentMeasurementClient = currentMeasurement.client;

                DirectoryManager dm = new DirectoryManager(currentMeasurementClient.id, currentMeasurementClient.first_name, currentMeasurementClient.last_name);


                if (currentMeasurementClient != null)
                {
                    using (var context = new MyDbConnection())
                    {
                        int clientId = currentMeasurementClient.id;
                        Database.Client clientToSave = context.Clients.Find(clientId);

                        if (clientToSave != null)
                        {
                            clientToSave.first_name = name_MC.Text;
                            clientToSave.last_name = lastname_MC.Text;
                            clientToSave.phone_number = phone_MC.Text;
                            clientToSave.email = mail_MC.Text;
                            clientToSave.address = address_MC.Text;

                            dm.updateDirectory(clientToSave.first_name, clientToSave.last_name);
                        }

                        context.SaveChanges();
                    }

                    loadFromOtherWindow(currentMeasurement);

                }
            }
        }


        private void selectMeasurementsGridView(Database.Entities.Measurement m)
        {
            MeasurementsGridView.ClearSelection();

            for (int i = 0; i < filteredMeasurements.Count(); i++)
            {
                if (filteredMeasurements[i].id == m.id)
                {
                    MeasurementsGridView.Rows[i].Selected = true;
                    MeasurementsGridView.FirstDisplayedScrollingRowIndex = i;
                    break;
                }
            }
        }

        private void selectMeasurementsCurrentRow()
        {
            int currentMeasurementIndex = getCurrentIndex();

            if (currentMeasurementIndex >= 0 && currentMeasurementIndex < MeasurementsGridView.Rows.Count)
            {
                MeasurementsGridView.FirstDisplayedScrollingRowIndex = currentMeasurementIndex;
                MeasurementsGridView_CellClick(MeasurementsGridView, new DataGridViewCellEventArgs(0, currentMeasurementIndex));
            }
        }

        private void btn_opendir_MC_Click(object sender, EventArgs e)
        {
            Measurement currentMeasurement = getCurrentMeasurement();
            if (currentMeasurement != null)
            {
                Client currentMeasurementClient = currentMeasurement.client;
                if (currentMeasurementClient != null)
                {
                    DirectoryManager dm = new DirectoryManager(currentMeasurementClient.id, currentMeasurementClient.first_name, currentMeasurementClient.last_name);
                    dm.openDirectory();
                }
            }
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

        private void search_TextChanged(object sender, EventArgs e)
        {
            searchText = search.Text.ToLower();

            filterTable();
        }

        private Measurement getCurrentMeasurement()
        {
            if (MeasurementsGridView.SelectedRows.Count > 0 && filteredMeasurements.Count > 0)
            {
                var id = MeasurementsGridView.SelectedRows[0].Index;
                var selected = filteredMeasurements[id];
                return selected;
            }
            return null;
        }


        private int getCurrentIndex()
        {
            if (MeasurementsGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = MeasurementsGridView.SelectedRows[0];
                return selectedRow.Index;
            }
            else
            {
                return -1;
            }
        }


        private void btn_order_Click(object sender, EventArgs e)
        {
            Measurement currentMeasurement = getCurrentMeasurement();
            if (currentMeasurement != null)
            {
                Client currentMeasurementClient = currentMeasurement.client;


                FormAddOrder newForm = new FormAddOrder(mainEmployee, currentMeasurement, currentMeasurementClient);
                newForm.Show();
            }
        }

        private void MeasurementsGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            /*
            Measurement currentMeasurement = getCurrentMeasurement();

            if (currentMeasurement != null && MeasurementsGridView.SelectedRows != null)
            {
                int id = currentMeasurement.id;
                int status = currentMeasurement.execution_status;

                using (var context = new MyDbConnection())
                {
                    currentMeasurement = context.Measurements.Find(id);
                    currentMeasurement.execution_status = Math.Abs(status - 1);

                    context.SaveChanges();
                }

                //loadDataMeasurements();
                //selectMeasurementsGridView(currentMeasurement);
                //selectMeasurementsCurrentRow();
                loadFromOtherWindow(currentMeasurement);

            }
            */
        }

        private void MeasurementsGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.RowIndex < filteredMeasurements.Count)
            {
                Measurement measurement = filteredMeasurements[e.RowIndex];

                if (measurement != null && !string.IsNullOrEmpty(measurement.color_rgb))
                {
                    try
                    {
                        MeasurementsGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = ColorTranslator.FromHtml(measurement.color_rgb);
                    }
                    catch (Exception ex) { }
                }
                else
                {
                    if (e.RowIndex % 2 == 0)
                    {
                        MeasurementsGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
                    }
                    else
                    {
                        MeasurementsGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Guna2TextBox textbox = sender as Guna2TextBox;

            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            if (!char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',' && textbox.Text.Contains(','))
            {
                e.Handled = true;
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            Guna2TextBox textBox = (Guna2TextBox)sender;

            int cursorPosition = textBox.SelectionStart;

            if (!IsTextValid(textBox.Text))
            {
                int dotIndex = textBox.Text.IndexOf(',');
                if (dotIndex >= 0 && textBox.Text.Length > dotIndex + 3)
                {
                    textBox.Text = textBox.Text.Substring(0, dotIndex + 3);
                    cursorPosition = Math.Min(cursorPosition, textBox.Text.Length);
                }

                textBox.SelectionStart = cursorPosition;

            }

        }

        private bool IsTextValid(string text)
        {
            Regex regex = new Regex(@"^\d{0,15}(\.\d{0,2})?$");
            return regex.IsMatch(text);
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
                    gv.AlternatingRowsDefaultCellStyle.Font = new Font("Calibri", 10);
                    gv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                    gv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                    gv.ColumnHeadersHeight = 30;
                    gv.RowTemplate.MinimumHeight = 50;

                    //FF6464
                    gv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(141, 156, 99);
                    gv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    gv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(141, 156, 99);

                    // Zmiana koloru wierszy
                    gv.DefaultCellStyle.BackColor = Color.FromArgb(249, 234, 225);
                    gv.DefaultCellStyle.ForeColor = Color.Black;

                    // Opcjonalnie zmiana kolorów wierszy przy najechaniu kursorem
                    gv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 234, 225);
                    gv.RowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);


                    gv.MultiSelect = false;
                    gv.AllowUserToResizeRows = false;

                    gv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);

                    gv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 219, 204);
                    gv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(0, 0, 0);


                    gv.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 219, 204);
                    gv.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(0, 0, 0);

                    gv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                    gv.GridColor = Color.LightGray;
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

        private void MeasurementsGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string sortByValue = MeasurementsGridView.Columns[e.ColumnIndex].DataPropertyName;


            if (sortByValue != null && sortByValue.Equals("ApplicationDate"))
            {
                registration_click += 1;
                registration_click %= 3;

                if (registration_click == 0) filterTable();
                else if (registration_click == 1) filteredMeasurements = filteredMeasurements.OrderBy(m => m.registration_date).ToList();
                else if (registration_click == 2) filteredMeasurements = filteredMeasurements.OrderByDescending(m => m.registration_date).ToList();

            }
            else if (sortByValue != null && sortByValue.Equals("DateMeasurement"))
            {
                installation_click += 1;
                installation_click %= 3;

                if (installation_click == 0) filterTable();
                else if (installation_click == 1) filteredMeasurements = filteredMeasurements.OrderBy(m => m.measurement_date).ToList();
                else if (installation_click == 2) filteredMeasurements = filteredMeasurements.OrderByDescending(m => m.measurement_date).ToList();

            }

            setDataInOrder();

        }

        private void is_paid_Click(object sender, EventArgs e)
        {
            // ☐  ☑   ☒
            if (is_paid.Text.Equals("☒"))
            {
                is_paid.Text = "☐";
                //iswithinstallation.Text = "Bez montażu";
            }
            else if (is_paid.Text.Equals("☐"))
            {
                is_paid.Text = "☒";
                //iswithinstallation.Text = "Z montażem";
            }
        }

        private void MeasurementsGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            using (var context = new MyDbConnection())
            {
                if (e.RowIndex >= 0 && e.RowIndex < filteredMeasurements.Count)
                {
                    var measurement = filteredMeasurements[e.RowIndex];

                    switch (MeasurementsGridView.Columns[e.ColumnIndex].Name)
                    {
                        case "Number":
                            e.Value = measurement.id;
                            break;
                        case "ApplicationDate":
                            e.Value = dateTimeToString(measurement.registration_date);
                            break;
                        case "PriceToPay":
                            e.Value = priceToString(measurement.price);
                            break;
                        case "Client":
                            e.Value = measurement.client.first_name;
                            break;
                        case "Phone":
                            e.Value = measurement.client.phone_number;
                            break;
                        case "Address":
                            e.Value = measurement.measurement_address;
                            break;
                        case "Status":
                            e.Value = measurement.status;
                            break;
                        case "Date":
                            e.Value = dateTimeToString(measurement.measurement_date);
                            break;
                        case "Product":
                            e.Value = measurement.notes;
                            break;
                        case "Employee":
                            e.Value = measurement.employee.first_name;
                            break;
                        case "Notes":
                            e.Value = measurement.notes_information;
                            break;
                        case "Done":
                            e.Value = (measurement.execution_status == 1) ? true : false;
                            break;
                    }
                }
            }
        }

        private void MeasurementsGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Measurement m = getCurrentMeasurement();

            if (m != null)
            {
                if (MeasurementsGridView.Rows[e.RowIndex].Selected)
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

        private void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem != null)
            {
                string selectedColor = e.ClickedItem.Name;
                DataGridViewRow clickedRow = MeasurementsGridView.Rows[clickedRowIndex];

                var id = filteredMeasurements[clickedRow.Index].id;
                string hexColor = "";

                switch (selectedColor)
                {
                    case "Red":
                        {
                            hexColor = "#ff5757";
                            break;
                        }
                    case "Pink":
                        {
                            hexColor = "#c74375";
                            break;
                        }
                    case "Pink2":
                        {
                            hexColor = "#f7a3b1";
                            break;
                        }
                    case "Orange":
                        {
                            hexColor = "#ff914d";
                            break;
                        }
                    case "Yellow":
                        {
                            hexColor = "#ffdb4f";
                            break;
                        }
                    case "Green":
                        {
                            hexColor = "#4c9045";
                            break;
                        }
                    case "Blue":
                        {
                            hexColor = "#9bb0c1";
                            break;
                        }
                    case "Purple":
                        {
                            hexColor = "#ac7ba6";
                            break;
                        }
                    case "None":
                        {
                            hexColor = "";
                            break;
                        }
                }

                using (var context = new MyDbConnection())
                {
                    //Order order = context.Orders.Find(id);
                    Measurement measurement = context.Measurements.Find(id);
                    if (measurement != null)
                    {
                        measurement.color_rgb = hexColor;
                        context.SaveChanges();
                        filteredMeasurements[clickedRow.Index].color_rgb = hexColor;
                        if (hexColor.Equals("")) MeasurementsGridView.Rows[clickedRowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);

                    }
                }
            }
        }

        private void MeasurementsGridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo hit = MeasurementsGridView.HitTest(e.X, e.Y);

                if (hit.Type == DataGridViewHitTestType.Cell)
                {
                    clickedRowIndex = hit.RowIndex;

                    MeasurementsGridView.ClearSelection();
                    MeasurementsGridView.Rows[clickedRowIndex].Selected = true;

                    contextMenu.Show(MeasurementsGridView, e.Location);
                }
            }
        }

        private void label50_DoubleClick(object sender, EventArgs e)
        {
            Measurement current = getCurrentMeasurement();

            if (current != null)
            {
                FormChangeClient newForm = new FormChangeClient(current);

                newForm.StartPosition = FormStartPosition.Manual;
                newForm.Location = new Point(guna2Panel7.Location.X, guna2Panel7.Location.Y);

                newForm.FormClosing += OtherForm_FormClosing_ChangeClient;
                newForm.Show();
            }
        }

        private void OtherForm_FormClosing_ChangeClient(object sender, FormClosingEventArgs e)
        {
            loadFromOtherWindow(getCurrentMeasurement());
        }

        private void btn_create_file_Click(object sender, EventArgs e)
        {
            DateTime? start = stringToDateTime(date_start.Text);
            DateTime? end = stringToDateTime(date_end.Text);

            List<Measurement> listToFile = new List<Measurement>();

            if (start != null && end != null)
            {
                listToFile = measurements.Where(m => m.measurement_date != null && !string.IsNullOrEmpty(m.measurement_date.ToString()) && m.measurement_date?.Date >= start?.Date && m.measurement_date?.Date <= end?.Date).ToList();
            }
            else if (start != null)
            {
                listToFile = measurements.Where(m => m.measurement_date != null && !string.IsNullOrEmpty(m.measurement_date.ToString()) && m.measurement_date?.Date >= start?.Date).ToList();
            }
            else if (end != null)
            {
                listToFile = measurements.Where(m => m.measurement_date != null && !string.IsNullOrEmpty(m.measurement_date.ToString()) && m.measurement_date?.Date >= DateTime.Now.Date && m.measurement_date?.Date <= end?.Date).ToList();
            }
            else
            {
                listToFile = measurements.Where(m => m.measurement_date != null && !string.IsNullOrEmpty(m.measurement_date.ToString()) && m.measurement_date?.Date >= DateTime.Now.Date).ToList();
            }


            WorksheetManager.createMeasurementsFile(listToFile);
        }

        private void btn_date_start_Click(object sender, EventArgs e)
        {
            Guna2Button btn = sender as Guna2Button;
            dt_date_start.Location = new Point(btn.Location.X, btn.Location.Y + btn.Height);
            dt_date_start.Visible = true;
            dt_date_start.Focus();
            SendKeys.Send("{F4}");
        }

        private void btn_date_startrm_Click(object sender, EventArgs e)
        {
            date_start.Text = "";
            dt_date_start.Value = DateTime.Now;
        }

        private void btn_date_end_Click(object sender, EventArgs e)
        {
            Guna2Button btn = sender as Guna2Button;
            dt_date_end.Location = new Point(btn.Location.X, btn.Location.Y + btn.Height);
            dt_date_end.Visible = true;
            dt_date_end.Focus();
            SendKeys.Send("{F4}");
        }

        private void btn_date_endrm_Click(object sender, EventArgs e)
        {
            date_end.Text = "";
            dt_date_end.Value = DateTime.Now;
        }

        private void dt_date_start_CloseUp(object sender, EventArgs e)
        {
            MetroDateTime dt = sender as MetroDateTime;
            DateTime selectedDate = dt.Value;
            date_start.Text = dateTimeToString(selectedDate);
        }

        private void dt_date_end_CloseUp(object sender, EventArgs e)
        {
            MetroDateTime dt = sender as MetroDateTime;
            DateTime selectedDate = dt.Value;
            date_end.Text = dateTimeToString(selectedDate);
        }

        private void MeasurementsGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int rowIndex = e.RowIndex;
                int columnIndex = e.ColumnIndex;

               // int check = 0;
               // string color_rgb = "";

                if (columnIndex == MeasurementsGridView.ColumnCount -1)
                {
                    using (var context = new MyDbConnection())
                    {
                        if (filteredMeasurements[rowIndex].execution_status == 1)
                        {
                            filteredMeasurements[rowIndex].execution_status = 0;
                            //filteredMeasurements[rowIndex].color_rgb = "";

                            //check = 0;
                            //color_rgb = "";
                        }
                        else
                        {
                            filteredMeasurements[rowIndex].execution_status = 1;
                            //filteredMeasurements[rowIndex].color_rgb = "#babd42";
                            //check = 1;
                            //color_rgb = "#babd42";
                        }

                        Measurement measurementToUpdate = context.Measurements.Find(filteredMeasurements[rowIndex].id);
                        measurementToUpdate.execution_status = filteredMeasurements[rowIndex].execution_status;
                        //if (check == 1) stoctToUpdate.color_rgb = color_rgb;

                        context.SaveChanges();
                        MeasurementsGridView.Invalidate();
                        MeasurementsGridView.Refresh();
                    }
                }
            }
        }
    }
}


