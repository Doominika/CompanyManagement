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
    public partial class FormChangeClient : Form
    {
        private Order order;
        private Measurement Measurement;
        private Client selectedClient;

        List<Client> clients = new List<Client>();


        public FormChangeClient(Order order)
        {
            this.order = order;

            InitializeComponent();
            loadClients();

            cb_client_AM.DataSource = clients;
            cb_client_AM.DisplayMember = "DisplayName";
            cb_client_AM.ValueMember = "id";
            cb_client_AM.SelectedIndex = -1;
        }

        public FormChangeClient(Measurement measurement)
        {
            this.Measurement = measurement;

            InitializeComponent();
            loadClients();

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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
           // MessageBox.Show(selectedClient.DisplayName);

            using (var context = new MyDbConnection())
            {
                if (order != null && selectedClient != null)
                {
                    context.Orders.Find(order.id).client_id = selectedClient.id;
                }
                else if (Measurement != null && selectedClient != null)
                {
                    context.Measurements.Find(Measurement.id).client_id = selectedClient.id;
                }
                context.SaveChanges();
                this.Close();
            }
        }

        private void cb_client_AM_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedClient = cb_client_AM.SelectedItem as Client;
            
        }
    }
}
