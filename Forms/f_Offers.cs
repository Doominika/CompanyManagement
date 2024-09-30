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
using WindowsFormsAppMySql.Database.Entities;
using WindowsFormsAppMySql.Database;
using WindowsFormsAppMySql.FileManaging;
using Guna.UI2.WinForms;
using Castle.Core.Internal;
using MetroFramework.Controls;
using System.Globalization;

namespace WindowsFormsAppMySql.Forms
{
    public partial class f_Offers : Form
    {
        //private Database.Client currentClient;
        //private int currentClientIndex = -1;
        private f_Main mainForm;

        List<Employee> employees = new List<Employee>();
        List<Product> products = new List<Product>();
        List<Client> clients = new List<Client>();



        public f_Offers()
        {
            InitializeComponent();
            load();

            ApplyStyles(this);

            OffersGridView.RowPostPaint += OfferssGridView_RowPostPaint;


            this.Load += (sender, e) =>
            {
                foreach (DataGridViewRow row in OffersGridView.Rows)
                {
                    row.Selected = false;
                }
            };
        }


        private void load()
        {
            loadDataOffers();
            btn_refresh.Image = Properties.Resources.refresh;
            OffersGridView.ContextMenuStrip = contextMenu;

        }


        private void btn_add_Click(object sender, EventArgs e)
        {
            using (var context = new MyDbConnection())
            {

                var employee = context.Employees.SingleOrDefault(empl => empl.id == 4);
                if (employee == null)
                {
                    return;
                }

                Database.Client client = new Database.Client(tb_name.Text, tb_lastname.Text, tb_phone.Text, tb_address.Text, tb_mail.Text);

                context.Clients.Add(client);
                context.SaveChanges();
                loadDataOffers();

                DirectoryManager dm = new DirectoryManager(client.id, client.first_name, client.last_name);
                dm.createDirectory();

            }
            clearClientAdding();

        }

        public void loadDataOffers()
        {
            clearClientInfo();

            using (var context = new MyDbConnection())
            {
                //if(offerClients != null) offerClients.Clear();
                clients = context.Clients.ToList();
                employees = context.Employees.ToList();

                var clientsList = context.Clients.Select(c => new { c.id, c.first_name, c.last_name, c.phone_number, c.address, c.email }).ToList();
                var clientsWithFullName = clientsList.Select(c => new
                {
                    c.id,
                    FullName = FullName(c.first_name, c.last_name),
                    PhoneNumber = c.phone_number,
                    c.address,
                }).OrderByDescending(c => c.id).ToList();


                OffersGridView.DataSource = clientsWithFullName;
                //OffersGridView.Columns["Id"].Visible = false;
            }

            foreach (DataGridViewRow row in OffersGridView.Rows)
            {
                row.Selected = false;
            }
        }



        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Client client = getCurrentClient();

            if (client != null)
            {
                fillClientInfo();
                updateListOfFiles(client);
            }
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

        public void fillClientInfo()
        {
            clearClientInfo();

            Client currentClient = getCurrentClient();

            if (currentClient != null)
            {
                tf_name_info.Text = currentClient.first_name;
                tf_lastname_info.Text = currentClient.last_name;
                tf_address_info.Text = currentClient.address;
                tf_mail_info.Text = currentClient.email;
                tf_phone_info.Text = currentClient.phone_number;

            }
        }

        private void clearClientAdding()
        {
            tb_name.Clear();
            tb_lastname.Clear();
            tb_address.Clear();
            tb_mail.Clear();
            tb_phone.Clear();
        }

        private void clearClientInfo()
        {
            tf_name_info.Clear();
            tf_lastname_info.Clear();
            tf_address_info.Clear();
            tf_mail_info.Clear();
            tf_phone_info.Clear();

            listView.Clear();
            listView.Columns.Add("Lista plików:", -2, HorizontalAlignment.Left);
        }

        private void listView_DoubleClick(object sender, EventArgs e)
        {
            Client currentClient = getCurrentClient();
            if (currentClient != null)
            {
                DirectoryManager dm = new DirectoryManager(currentClient.id, currentClient.first_name, currentClient.last_name);
                dm.openFile(listView.SelectedItems[0].Text);
            }
        }


        private void btn_save_info_Click(object sender, EventArgs e)
        {
            Client currentClient = getCurrentClient();

            if (currentClient != null)
            {
                using (var context = new MyDbConnection())
                {
                    var existingClient = context.Clients.Find(currentClient.id);

                    if (existingClient != null)
                    {
                        existingClient.first_name = tf_name_info.Text;
                        existingClient.last_name = tf_lastname_info.Text;
                        existingClient.address = tf_address_info.Text;
                        existingClient.email = tf_mail_info.Text;
                        existingClient.phone_number = tf_phone_info.Text;

                        context.SaveChanges();
                    }
                }

                loadDataOffers();
                if (currentClient != null)
                {
                    selectCurrentRow();
                    selectOffersGridView(currentClient);
                }
            }
        }

        private void selectOffersGridView(Database.Client c)
        {
            foreach (DataGridViewRow row in OffersGridView.Rows)
            {
                row.Selected = false;
            }

            foreach (DataGridViewRow row in OffersGridView.Rows)
            {
                if (row.Cells["Id"].Value != null && (int)row.Cells["Id"].Value == c.id)
                {
                    row.Selected = true;
                    OffersGridView.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
            }
        }

        private void selectCurrentRow()
        {
            int currentClientIndex = getCurrentIndex();

            if (currentClientIndex >= 0 && currentClientIndex < OffersGridView.Rows.Count)
            {
                OffersGridView.FirstDisplayedScrollingRowIndex = currentClientIndex;
                DataGridView_CellClick(OffersGridView, new DataGridViewCellEventArgs(0, currentClientIndex));
            }
        }

        private void openDirectory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Client currentClient = getCurrentClient();
            if (currentClient != null)
            {
                DirectoryManager dm = new DirectoryManager(currentClient.id, currentClient.first_name, currentClient.last_name);
                dm.openDirectory();
            }
        }

        private void guna2TextBox42_TextChanged(object sender, EventArgs e)
        {
            string searchText = guna2TextBox42.Text.ToLower();

            // Filtrowanie klientów na podstawie wprowadzonego tekstu
            var filteredClients = clients
             .Where(c => (c.first_name?.ToLower().Contains(searchText) ?? false) ||
                         (c.last_name?.ToLower().Contains(searchText) ?? false) ||
                         (c.phone_number?.ToLower().Contains(searchText) ?? false) ||
                         (c.address?.ToLower().Contains(searchText) ?? false) ||
                         (c.email?.ToLower().Contains(searchText) ?? false))
             .ToList();

            var clientsWithFullName = filteredClients.Select(c => new
            {
                Id = c.id,
                FullName = FullName(c.first_name, c.last_name),
                c.phone_number,
                c.address,
                c.email
            }).ToList();

            OffersGridView.DataSource = clientsWithFullName;
            //OffersGridView.Columns["Id"].Visible = false;
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            loadDataOffers();

        }

        public string FullName(string firstName, string lastName)
        {
            if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName))
            {
                return firstName + " " + lastName;
            }
            else if (!string.IsNullOrWhiteSpace(firstName))
            {
                return firstName;
            }
            else if (!string.IsNullOrWhiteSpace(lastName))
            {
                return lastName;
            }
            else
            {
                return "klient";
            }
        }

        private Client getCurrentClient()
        {
            if (OffersGridView.SelectedRows.Count > 0)
            {
                var selectedRow = OffersGridView.SelectedRows[0];
                var id = (int)selectedRow.Cells["Id"].Value;
                var selected = clients.FirstOrDefault(c => c.id == id);
                return selected;
            }
            return null;
        }

        private int getCurrentIndex()
        {
            if (OffersGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = OffersGridView.SelectedRows[0];
                return selectedRow.Index;
            }
            else
            {
                return -1;
            }
        }

        private void btn_open_folder_Click(object sender, EventArgs e)
        {
            Client currentClient = getCurrentClient();
            DirectoryManager dm = new DirectoryManager(currentClient.id, currentClient.first_name, currentClient.last_name);
            dm.openDirectory();
        }

        private void add_files_Click(object sender, EventArgs e)
        {
            Client c = getCurrentClient();

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

                        DirectoryManager dm = new DirectoryManager(c.id, c.first_name, c.last_name);

                        if (files != null && !files.IsNullOrEmpty())
                        {
                            foreach (string file in files)
                            {
                                dm.copyFileToFolder(file);
                            }
                        }
                    }
                }

                updateListOfFiles(c);

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
                    gv.AlternatingRowsDefaultCellStyle.Font = new Font("Calibri", 10);

                    gv.ColumnHeadersHeight = 30;
                    gv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                    gv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

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

        private int clickedRowIndex = -1;

        private void OffersGridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo hit = OffersGridView.HitTest(e.X, e.Y);

                if (hit.Type == DataGridViewHitTestType.Cell)
                {
                    clickedRowIndex = hit.RowIndex;

                    OffersGridView.ClearSelection();
                    OffersGridView.Rows[clickedRowIndex].Selected = true;

                    contextMenu.Show(OffersGridView, e.Location);
                }
            }
        }

        private void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem != null)
            {
                string selectedColor = e.ClickedItem.Name;
                DataGridViewRow clickedRow = OffersGridView.Rows[clickedRowIndex];

                switch (selectedColor)
                 {
                     case "Red":
                        {
                            Color color = ColorTranslator.FromHtml("199; 67; 117");   //("172; 123; 166");
                            clickedRow.DefaultCellStyle.BackColor = color;
                            break;
                        }
                     case "Pink":
                        {
                            Color color = ColorTranslator.FromHtml("198; 132; 132");   //("172; 123; 166");
                            clickedRow.DefaultCellStyle.BackColor = color;
                            break;
                        }
                    case "Pink2":
                        {
                            Color color = ColorTranslator.FromHtml("247; 163; 177");   //("172; 123; 166");
                            clickedRow.DefaultCellStyle.BackColor = color;
                            break;
                        }
                    case "Orange":
                        {
                            Color color = ColorTranslator.FromHtml("252; 199; 97");   //("172; 123; 166");
                            clickedRow.DefaultCellStyle.BackColor = color;
                            break;
                        }
                    case "Green":
                        {
                            Color color = ColorTranslator.FromHtml("210; 222; 50");   //("172; 123; 166");
                            clickedRow.DefaultCellStyle.BackColor = color;
                            break;
                        }
                    case "Green2":
                        {
                            Color color = ColorTranslator.FromHtml("186; 189; 66");   //("172; 123; 166");
                            clickedRow.DefaultCellStyle.BackColor = color;
                            break;
                        }
                    case "Blue":
                        {
                            Color color = ColorTranslator.FromHtml("155; 176; 193");   //("172; 123; 166");
                            clickedRow.DefaultCellStyle.BackColor = color;
                            break;
                        }
                    case "Purple":
                        {
                            Color color = ColorTranslator.FromHtml("172; 123; 166");   //("172; 123; 166");
                            clickedRow.DefaultCellStyle.BackColor = color;
                            break;
                        }
                }
             }
        }

        private void OffersGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (OffersGridView.Rows[e.RowIndex].Selected)
            {
                e.CellStyle.Font = new Font(OffersGridView.DefaultCellStyle.Font, FontStyle.Bold);
            }
            else
            {
                e.CellStyle.Font = OffersGridView.DefaultCellStyle.Font;
            }

        }

        private void OfferssGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (OffersGridView.Rows[e.RowIndex].Selected)
            {

                Rectangle rect = e.RowBounds;

    
                // Ustawienie koloru tła dla zaznaczonego wiersza
                using (Brush brush = new SolidBrush(Color.FromArgb(150, Color.Gray))) // Przezroczysty kolor tła
                {
                    e.Graphics.FillRectangle(brush, rect);
                }


                //Rectangle rect = e.RowBounds;

                // Ustawienie grubości i koloru obramowania
                using (Pen pen = new Pen(Color.Red, 2))
                {
                    int penWidth = 2;
                    // Obliczenie prostokąta otaczającego cały wiersz
                    rect.X += penWidth / 2;
                    rect.Y += penWidth / 2;
                    rect.Width -= penWidth;
                    rect.Height -= penWidth;

                    // Rysowanie prostokąta wokół całego wiersza
                    e.Graphics.DrawRectangle(pen, rect);
                }
            }
        }

    }
}
