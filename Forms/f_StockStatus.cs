using Guna.UI2.WinForms;
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsAppMySql.Database;
using WindowsFormsAppMySql.Database.Entities;

namespace WindowsFormsAppMySql.Forms
{
    public partial class f_StockStatus : Form
    {
        private f_Main mainForm;
        //private Stock editedStock;
        private int editedIndex = -1;
        private int filter_date_number = 0;


        List<Client> clients = new List<Client>();
        List<Order> orders = new List<Order>();
        List<Order> filteredOrders = new List<Order>();
        List<Stock> stocks = new List<Stock>();
        List<Stock> filteredStocks = new List<Stock>();

        public f_StockStatus(f_Main mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
            ApplyStyles(this);

            load();
        }

        private async void load()
        {
            await loadDataAsync();
            setDataInOrder();
        }

        private async Task loadDataAsync()
        {
            using (var context = new MyDbConnection())
            {
                clients = context.Clients.ToList();
                orders = await context.Orders
                    .Include(o => o.client)
                    .Include(o => o.product)
                    .Include(o => o.employee)
                    .ToListAsync();

                var newStocks = new List<Stock>();

                foreach (var o in orders)
                {
                    if (o.is_checked_for_stock == 0)
                    {
                        if (o.delivery_date != null && o.delivery_date <= DateTime.Today)
                        {
                            var s = new Stock
                            {
                                client_id = o.client_id,
                                order_id = o.id,
                                delivery_date = (DateTime)o.delivery_date,
                                order_name = o.orderCompanyNumber(),
                                client_name = o.client.DisplayNameOnly
                            };

                            newStocks.Add(s);

                            o.is_checked_for_stock = 1;
                        }
                    }
                }

                if (newStocks.Count > 0)
                {
                    context.Stocks.AddRange(newStocks);
                }

                await context.SaveChangesAsync();

                stocks = await context.Stocks.ToListAsync();

                filteredStocks = stocks.OrderBy(x => x.delivery_date).ToList();

                if (filteredStocks.Count <= 0) StocksGridView.RowCount = 0;
                else StocksGridView.RowCount = filteredStocks.Count;

                StocksGridView.VirtualMode = true;

               // StocksGridView.ClearSelection();
            }
        }


        private void MeasurementsGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < filteredStocks.Count)
            {
                Stock stock = filteredStocks[e.RowIndex];
                Order order = orders.FirstOrDefault(o => o.id == stock.order_id);

                if (order != null)
                {
                    Client client = order.client;

                    var columnName = StocksGridView.Columns[e.ColumnIndex]?.Name;

                    switch (columnName)
                    {
                        case "Number":
                            e.Value = order.orderCompanyNumber();
                            break;
                        case "Client":
                            e.Value = stock.client_name;//client.DisplayNameOnly;
                            break;
                        case "Product":
                            e.Value = order.product.name;
                            break;
                        case "ProductInfo":
                            e.Value = order.notes;
                            break;
                        case "DeliveryDate":
                            e.Value = dateTimeToString(stock.delivery_date);
                            break;
                        case "Notes":
                            e.Value = stock.notes;
                            break;
                        case "Done":
                            e.Value = (stock.is_checked == 0) ? false : true;
                            break;
                    }
                }
            }
        }

        public static string dateTimeToString(DateTime? date)
        {
            if (!date.HasValue)
                return "";
            else
                return date.Value.ToString("dd.MM.yyyy");
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
                    gv.RowTemplate.MinimumHeight = 30;

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


        private void StocksGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int rowIndex = e.RowIndex;
                int columnIndex = e.ColumnIndex;

                int check = 0;
                string color_rgb = "";

                if (columnIndex == 6)
                {
                    using (var context = new MyDbConnection())
                    {
                        if (filteredStocks[rowIndex].is_checked == 1)
                        {
                            filteredStocks[rowIndex].is_checked = 0;
                            filteredStocks[rowIndex].color_rgb = "";

                            check = 0;
                            color_rgb = "";
                        }
                        else
                        {
                            filteredStocks[rowIndex].is_checked = 1;
                            filteredStocks[rowIndex].color_rgb = "#babd42";
                            check = 1;
                            color_rgb = "#babd42";
                        }

                        Stock stoctToUpdate = context.Stocks.Find(filteredStocks[rowIndex].id);
                        stoctToUpdate.is_checked = filteredStocks[rowIndex].is_checked;
                        if (check == 1) stoctToUpdate.color_rgb = color_rgb;

                        context.SaveChanges();
                        StocksGridView.Invalidate();
                        StocksGridView.Refresh();
                    }
                }
            }
        }

        private void StocksGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.RowIndex < filteredStocks.Count)
            {
                Stock stock = filteredStocks[e.RowIndex];

                if (stock != null && stock.is_checked == 1 && !string.IsNullOrEmpty(stock.color_rgb))
                {
                    try
                    {
                        StocksGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = ColorTranslator.FromHtml(stock.color_rgb);

                    }
                    catch (Exception ex)
                    {
                        // Obsługa ewentualnych wyjątków, np. nieprawidłowy format koloru
                        // MessageBox.Show($"Błąd formatu koloru dla zamówienia {recordId}: {ex.Message}");
                    }
                }
                else
                {
                    StocksGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#ffffff");

                }
            }
        }


        private void OtherForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (editedIndex > -1)
            {
                using (var context = new MyDbConnection())
                {
                    Stock s = context.Stocks.Find(filteredStocks[editedIndex].id);
                    filteredStocks[editedIndex].notes = s.notes;

                    editedIndex = -1;
                }
            }

            StocksGridView.Invalidate();
            StocksGridView.Refresh();

        }

        private void StocksGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int rowIndex = e.RowIndex;
                int columnIndex = e.ColumnIndex;

                if (columnIndex == 5)
                {
                    editedIndex = rowIndex;
                    FormAddStockNotes newForm = new FormAddStockNotes(filteredStocks[editedIndex]);

                    Point clickLocation = StocksGridView.PointToScreen(StocksGridView.GetCellDisplayRectangle(columnIndex, rowIndex, true).Location);

                    // Ustaw położenie formularza na lewo od kliknięcia
                    newForm.StartPosition = FormStartPosition.Manual;
                    newForm.Location = new Point(clickLocation.X - newForm.Width, clickLocation.Y);

                    newForm.FormClosing += OtherForm_FormClosing;
                    newForm.Show();
                }
            }
        }

        private void StocksGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            string sortByValue = StocksGridView.Columns[e.ColumnIndex].DataPropertyName;

            if (sortByValue != null && sortByValue.Equals("DeliveryDate"))
            {
                filter_date_number += 1;
                filter_date_number %= 3;

                if (filter_date_number == 0) filteredStocks = filteredStocks.OrderBy(x => x.id).ToList();
                else if (filter_date_number == 1) filteredStocks = filteredStocks.OrderBy(x => x.delivery_date).ToList();
                else if (filter_date_number == 2) filteredStocks = filteredStocks.OrderByDescending(x => x.delivery_date).ToList();
            }


            StocksGridView.Refresh();
            setDataInOrder();
        }

        private void setDataInOrder()
        {
            int lastRowIndex = StocksGridView.Rows.Count - 1;
            StocksGridView.ClearSelection();
            StocksGridView.Rows[lastRowIndex].Selected = true;
            StocksGridView.FirstDisplayedScrollingRowIndex = lastRowIndex;
        }


        private void searchRows()
        {
            String searchText = search.Text.ToLower();

            filteredStocks = stocks;

            if (!string.IsNullOrEmpty(searchText))
            {
                filteredStocks = filteredStocks
                    .Where(s => s != null &&
                        (s.order_name.ToLower().Contains(searchText) == true) ||
                        (s.client_name.ToLower().Contains(searchText) == true)
                    ).ToList();
            }

            filteredStocks = filteredStocks.OrderBy(x => x.delivery_date).ToList();

            //StocksGridView.VirtualMode = true;

            if (filteredStocks.Count > 0)
            {
                StocksGridView.RowCount = filteredStocks.Count;
            }
            else
            {
                StocksGridView.RowCount = 0;
            }



            StocksGridView.Refresh();
            StocksGridView.Invalidate();
            setDataInOrder();
        }

        private void search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                searchRows();
               e.Handled = true;
            }
        }
    }

}
