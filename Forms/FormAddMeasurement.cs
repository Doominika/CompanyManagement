using Castle.Core.Internal;
using DocumentFormat.OpenXml.ExtendedProperties;
using Guna.UI2.WinForms;
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WindowsFormsAppMySql.Database;
using WindowsFormsAppMySql.Database.Entities;

namespace WindowsFormsAppMySql.Forms
{
    public partial class FormAddMeasurement : Form
    {
        List<Client> clients = new List<Client>();
        Database.Client selectedClient;

        private Employee mainEmployee;

        public FormAddMeasurement(Employee employee)
        {
            this.mainEmployee = employee;
            InitializeComponent();
            ApplyStyles(this);

            loadClients();

            btn_addclient_AM.Image = Properties.Resources.plus;
            btn_cal_AM.Image = Properties.Resources.calendar;
            btn_calrm_AM.Image = Properties.Resources.minus;
            btn_updateaddress.Image = Properties.Resources.plus;


            cb_client_AM.DataSource = clients;
            cb_client_AM.DisplayMember = "DisplayName";
            cb_client_AM.ValueMember = "id";
            cb_client_AM.SelectedIndex = -1;

        }

        private void loadClients()
        {
            using (var context = new MyDbConnection())
            {
                clients = context.Clients.ToList();
            }
        }

        private void btn_addclient_AM_Click(object sender, EventArgs e)
        {
            FormAddClient newForm = new FormAddClient(mainEmployee);
            newForm.FormClosing += OtherForm_FormClosingClient;
            newForm.ShowDialog();
        }

        private void OtherForm_FormClosingClient(object sender, FormClosingEventArgs e)
        {
            int before = cb_client_AM.Items.Count;
            loadClients();

            cb_client_AM.DataSource = null;
            cb_client_AM.DataSource = clients;
            cb_client_AM.DisplayMember = "DisplayName";
            cb_client_AM.ValueMember = "id";
            cb_client_AM.SelectedIndex = - 1;

            int after = cb_client_AM.Items.Count;

            if (after != before)
            {
                cb_client_AM.SelectedIndex = cb_client_AM.Items.Count - 1;
            }
        }

        private void OtherForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            loadClients();
        }

        private void btn_calrm_AM_Click(object sender, EventArgs e)
        {
            date_AM.Text = "";
        }

        private void btn_cal_AM_Click(object sender, EventArgs e)
        {
            dateTime_AM.Location = new Point(btn_cal_AM.Location.X, btn_cal_AM.Location.Y + btn_cal_AM.Height);
            dateTime_AM.Visible = true;
            dateTime_AM.Focus();
            SendKeys.Send("{F4}");
        }

        private void dateTime_AM_CloseUp(object sender, EventArgs e)
        {
            DateTime selectedDate = dateTime_AM.Value;
            date_AM.Text = dateTimeToString(selectedDate);
        }

        private void cb_client_AM_SelectedIndexChanged(object sender, EventArgs e)
        {
            name_CAM.Clear();   
            lastname_CAM.Clear();
            phone_CAM.Clear();
            address_CAM.Clear();
            mail_CAM.Clear();

            selectedClient = cb_client_AM.SelectedItem as Client;
            if(selectedClient != null)
            {
                name_CAM.Text = selectedClient.first_name;
                lastname_CAM.Text = selectedClient.last_name;
                phone_CAM.Text = selectedClient.phone_number;
                address_CAM.Text = selectedClient.address;
                mail_CAM.Text = selectedClient.email;
            }

        }

        private void btn_add_AM_Click(object sender, EventArgs e)
        {
            using (var context = new MyDbConnection())
            {
                
                Database.Entities.Measurement measurementToSave = new Database.Entities.Measurement();
                measurementToSave.client = context.Clients.Find(selectedClient.id);
                measurementToSave.client_id = selectedClient.id;
                measurementToSave.measurement_address = address_AM.Text;
                measurementToSave.status = state_AM.Text;
                measurementToSave.execution_status = 0;
                measurementToSave.employee_id = mainEmployee.id;
                measurementToSave.registration_date = DateTime.Now;

                if (double.TryParse(installation_price.Text, out double price))
                {
                    measurementToSave.price = price;
                }

                if (!date_AM.Text.IsNullOrEmpty())
                {
                    measurementToSave.measurement_date = stringToDateTime(date_AM.Text);
                }
                else
                {
                    measurementToSave.measurement_date = null;

                }
                measurementToSave.notes = notes_AM.Text;
                measurementToSave.notes_information = information.Text;

                context.Measurements.Add(measurementToSave);

                var client = context.Clients.Find(selectedClient.id);
                client.Measurements.Add(measurementToSave);

                context.SaveChanges();
            }

            this.Close();
       
        }


        public DateTime stringToDateTime(string date)
        {
            return DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        }

        public string dateTimeToString(DateTime date)
        {
            return date.ToString("dd.MM.yyyy");
        }

        private void btn_updateaddress_Click(object sender, EventArgs e)
        {
            address_AM.Text = address_CAM.Text;
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
        private void client_search_TextChanged(object sender, EventArgs e)
        {
            string text = client_search.Text.ToLower();
            var filteredClients = clients
                .Where(c => (c.first_name?.ToLower().Contains(text) ?? false) || (c.last_name?.ToLower().Contains(text) ?? false) || (c.id.ToString().Contains(text)))
                .ToList();

            if (filteredClients.Count > 0)
            {
                
                cb_client_AM.DropDownStyle = ComboBoxStyle.DropDown;
                cb_client_AM.DataSource = null;
                cb_client_AM.DataSource = filteredClients;
                cb_client_AM.DisplayMember = "DisplayName";
                cb_client_AM.ValueMember = "id";
                

            }
            else
            {
                cb_client_AM.DroppedDown = false;
                cb_client_AM.DataSource = null;
                cb_client_AM.DisplayMember = "DisplayName";
                cb_client_AM.ValueMember = "id";
            }
        }


        private void ApplyStyles(Control control)
        {
            foreach (Control ctrl in control.Controls)
            {
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
    }
}
