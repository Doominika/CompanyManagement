using Castle.Core.Internal;
using Guna.UI2.WinForms;
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsAppMySql.Database.Entities;
using WindowsFormsAppMySql.Database;
using WindowsFormsAppMySql.FileManaging;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Data.Entity;

namespace WindowsFormsAppMySql.Forms
{
    public partial class f_Orders : Form
    {
        private Employee mainEmployee;
        private f_Main mainForm;
        private int filterCompanyOrder = 0;       //0 - wszystkie, 1 - tomkar, 2 - premium
        private string searchText = "";

        private int filter_invoice = 0;
        private int filter_number = 0;
        private int filter_orderdate = 0;
        private int filter_delivarydate = 0;


        private List<Order> orders = new List<Order>();
        private List<Employee> employees = new List<Employee>();
        private List<Product> products = new List<Product>();
        private List<string> statuses = new List<string> { "Niezaczęte", "W trakcie", "Rozwiązane" };
        private List<Database.Entities.Order> filteredOrders = new List<Database.Entities.Order>();

        private int clickedRowIndex = -1;


        public f_Orders(Employee mainEmployee, f_Main mainForm)
        {
            this.mainEmployee = mainEmployee;
            this.mainForm = mainForm;
            InitializeComponent();
            ApplyStyles(this);

            load();
        }

        public f_Orders(Employee mainEmployee, Order order, f_Main mainForm)
        {
            this.mainEmployee = mainEmployee;
            this.mainForm = mainForm;
            InitializeComponent();
            ApplyStyles(this);

            loadFromOtherWindow(order);
        }

        private async void loadFromOtherWindow(Order order)
        {
            await loadDataOrdersAsync();
            selectOrdersGridView(order);
            selectOrdersCurrentRow();
        }

        private async void load()
        {
            await loadDataOrdersAsync();

            using (var context = new MyDbConnection())
            {
                employees = context.Employees.ToList();
                products = context.Products.Where(p => p.visible == 1).ToList();
            }

            //eventy
            cb_employee.MouseWheel += new MouseEventHandler(preventMouseWheel);
            cb_product_O.MouseWheel += new MouseEventHandler(preventMouseWheel);


            //obrazy
            btn_add_O.Image = Properties.Resources.plus;
            btn_refresh_O.Image = Properties.Resources.refresh;

            btn_orderdate_O.Image = Properties.Resources.calendar;
            btn_orderdaterm_O.Image = Properties.Resources.minus;
            btn_deliverydate_O.Image = Properties.Resources.calendar;
            deliverydaterm_O.Image = Properties.Resources.minus;
            btn_product_O.Image = Properties.Resources.calendar;
            btn_productrm_O.Image = Properties.Resources.minus;
            btn_installation_O.Image = Properties.Resources.calendar;
            btn_installationrm_O.Image = Properties.Resources.minus;

            cb_product_O.DataSource = products;
            cb_product_O.DisplayMember = "name";
            cb_product_O.ValueMember = "id";
            cb_product_O.SelectedIndex = -1;

            cb_employee.DataSource = employees.Where(e => e.role == 0).ToList();
            cb_employee.DisplayMember = "first_name";
            cb_employee.ValueMember = "id";
            cb_employee.SelectedIndex = -1;
        }

        private void preventMouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private async Task loadDataOrdersAsync()
        {
            clearOrderClientInfo();
            clearOrderInfo();

            using (var context = new MyDbConnection())
            {
                employees = context.Employees.ToList();
                products = context.Products.Where(p => p.visible == 1).ToList();

                orders = await context.Orders
                          .Include(o => o.client)
                          .Include(o => o.product)
                          .Include(o => o.employee)
                          .OrderByDescending(o => o.id)
                          .ToListAsync();

                if (mainForm.isAdmin == 1)
                {
                    filteredOrders = orders.OrderBy(o => o.id).ToList();
                    filterCompanyOrder = 0;
                }
                else
                {
                    filteredOrders = orders.Where(o => o.employee.company == mainForm.company).OrderBy(o => o.id).ToList();
                    filterCompanyOrder = mainForm.company + 1;
                }

                btn_premium_O.Text = getCompanyName(filterCompanyOrder);

                if (filteredOrders.Count <= 0) OrdersGridView.RowCount = 0;
                else OrdersGridView.RowCount = filteredOrders.Count;

                OrdersGridView.VirtualMode = true;
            }

            setDataInOrder();


        }

        private void setDataInOrder()
        {
            int lastRowIndex = OrdersGridView.Rows.Count - 1;
            OrdersGridView.ClearSelection();
            OrdersGridView.Rows[lastRowIndex].Selected = true;
            OrdersGridView.FirstDisplayedScrollingRowIndex = lastRowIndex;
        }

        private void clearOrderInfo()
        {
            ordernumber_O.Clear();
            orderdate_O.Text = "";
            delivery_O.Clear();
            deliverydate_O.Text = "";
            product_O.Text = "";
            installation_O.Text = "";
            iswithinstallation.Clear();
            squared_meters.Clear();
            instances.Clear();
            state.Clear();
            fv_O.Clear();
            deliverynumber_O.Clear();
            cb_product_O.SelectedIndex = -1;
            cb_employee.SelectedIndex = -1;
            notes_O.Clear();
            notes_information.Clear();
            complaint.FillColor = System.Drawing.Color.White;
            complaint.Text = "";
        }

        private void clearOrderClientInfo()
        {
            name_OC.Clear();
            lastname_OC.Clear();
            phone_OC.Clear();
            address_OC.Clear();
            mail_OC.Clear();
        }


        public string OrderNumberWithCompany(int orderNr, int companyNr)
        {
            if (companyNr > 0)
            {
                return orderNr + "P";
            }

            return orderNr.ToString();
        }

        private void AddOrderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            loadDataOrdersAsync();
        }

        private void btn_add_O_Click(object sender, EventArgs e)
        {
            if (mainEmployee != null)
            {
                FormAddOrder newForm = new FormAddOrder(mainEmployee);

                newForm.StartPosition = FormStartPosition.Manual;
                newForm.Top = this.Top + 50;
                newForm.Left = this.Left + 100;

                newForm.FormClosing += AddOrderForm_FormClosing;
                newForm.Show();
            }

        }

        private void OrdersGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            clearOrderClientInfo();
            clearOrderInfo();

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int rowIndex = e.RowIndex;

                var order = filteredOrders[rowIndex];

                int id = order.id;

                using (var context = new MyDbConnection())
                {
                    Order currentOrder = context.Orders.SingleOrDefault(o => o.id == id);
                    Client currentOrderClient = currentOrder.client;
                    fillOrdersClientInfo(currentOrderClient);
                    fillOrdersInfo(currentOrder);

                    Complaint compl = context.Complaints.OrderByDescending(c => c.id).FirstOrDefault(c => c.order_id == order.id);
                    if (compl != null)
                    {
                        complaint.Text = statuses.ElementAt(compl.status);

                        switch (compl.status)
                        {
                            case 0:
                                complaint.FillColor = System.Drawing.Color.FromArgb(195, 60, 84);   //(211, 118, 118);
                                complaint.ForeColor = System.Drawing.Color.Black;
                                break;
                            case 1:
                                complaint.FillColor = System.Drawing.Color.FromArgb(255, 231, 76);     //(255,231,76);
                                complaint.ForeColor = System.Drawing.Color.Black;
                                break;
                            case 2:
                                complaint.FillColor = System.Drawing.Color.FromArgb(124, 181, 24);    //(176, 197, 164);
                                complaint.ForeColor = System.Drawing.Color.Black;
                                break;
                            default:
                                complaint.FillColor = System.Drawing.Color.White;
                                break;
                        }
                    }
                }

            }

        }


        public void fillOrdersInfo(Database.Entities.Order order)
        {
            if (order == null)
            {
                return;
            }

            ordernumber_O.Text = OrderNumberWithCompany(order.order_number, order.company_number);
            orderdate_O.Text = dateTimeToString(order.order_date);
            delivery_O.Text = order.planned_delivery.ToString() ?? "";
            deliverydate_O.Text = dateTimeToString(order.delivery_date);
            product_O.Text = dateTimeToString(order.payment_products);
            installation_O.Text = dateTimeToString(order.payment_installation);
            fv_O.Text = order.invoice;
            deliverynumber_O.Text = order.supplier_order_number;
            instances.Text = order.instance.ToString();
            squared_meters.Text = priceToString(order.squared_meters);
            state.Text = order.state;

            // ☐  ☑   ☒

            if (order.is_with_installation == 1)
            {
                iswithinstallation.Text = order.installation_address;
                btn_is_with_installation.Text = "☒";
                iswithinstallation.ReadOnly = false;
            }
            else if (order.is_with_installation == 0)
            {
                iswithinstallation.Text = "Bez montażu";
                btn_is_with_installation.Text = "☐";
                iswithinstallation.ReadOnly = true;
            }


            if (order.product != null)
            {
                for (int i = 0; i < cb_product_O.Items.Count; i++)
                {
                    Product product = cb_product_O.Items[i] as Product;

                    if (product != null && product.id == order.product_id)
                    {
                        cb_product_O.SelectedIndex = i;
                        break;
                    }
                }
            }

            notes_O.Text = order.notes ?? "";
            notes_information.Text = order.notes_information ?? "";
            cb_employee.SelectedValue = order.employee_id;

        }
        public void fillOrdersClientInfo(Database.Client c)
        {
            if (c != null)
            {
                name_OC.Text = c.first_name;
                lastname_OC.Text = c.last_name;
                phone_OC.Text = c.phone_number;
                address_OC.Text = c.address;
                mail_OC.Text = c.email;
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

        private void TextBox_KeyPress_OnlyDigits(object sender, KeyPressEventArgs e)
        {
            Guna2TextBox textbox = sender as Guna2TextBox;

            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void TextBox_TextChanged_OnlyDigits(object sender, EventArgs e)
        {
            Guna2TextBox textBox = (Guna2TextBox)sender;

            int cursorPosition = textBox.SelectionStart;

            string newText = new string(textBox.Text.Where(char.IsDigit).ToArray());

            if (textBox.Text != newText)
            {
                textBox.Text = newText;
                textBox.SelectionStart = Math.Min(cursorPosition, textBox.Text.Length);
            }
        }

        private bool IsTextValid(string text)
        {
            Regex regex = new Regex(@"^\d{0,15}(\.\d{0,2})?$");
            return regex.IsMatch(text);
        }

        private void openCalendarO(object sender, MetroDateTime dt)
        {
            Guna2Button btn = sender as Guna2Button;
            dt.Location = new Point(btn.Location.X, btn.Location.Y + btn.Height);
            dt.Visible = true;
            dt.Focus();
            SendKeys.Send("{F4}");
        }

        private void closeCalendarO(object sender, Guna2TextBox textbox)
        {
            MetroDateTime dt = sender as MetroDateTime;
            DateTime selectedDate = dt.Value;
            textbox.Text = dateTimeToString(selectedDate);
        }

        private void btn_orderdate_O_Click(object sender, EventArgs e)
        {
            openCalendarO(sender, dt_orderdate_O);
        }

        private void btn_deliverydate_O_Click(object sender, EventArgs e)
        {
            openCalendarO(sender, dt_deliverydate_O);
        }

        private void btn_product_O_Click(object sender, EventArgs e)
        {
            openCalendarO(sender, dt_product_O);
        }

        private void btn_installation_O_Click(object sender, EventArgs e)
        {
            openCalendarO(sender, dt_installation_O);
        }

        private void dt_orderdate_O_CloseUp(object sender, EventArgs e)
        {
            closeCalendarO(sender, orderdate_O);
        }

        private void dt_deliverydate_O_CloseUp(object sender, EventArgs e)
        {
            closeCalendarO(sender, deliverydate_O);
        }

        private void dt_product_O_CloseUp(object sender, EventArgs e)
        {
            closeCalendarO(sender, product_O);
        }

        private void dt_installation_O_CloseUp(object sender, EventArgs e)
        {
            closeCalendarO(sender, installation_O);
        }

        private void btn_orderdaterm_O_Click(object sender, EventArgs e)
        {
            orderdate_O.Text = "";
            dt_orderdate_O.Value = DateTime.Now;
        }

        private void deliverydaterm_O_Click(object sender, EventArgs e)
        {
            deliverydate_O.Text = "";
            dt_deliverydate_O.Value = DateTime.Now;
        }

        private void btn_productrm_O_Click(object sender, EventArgs e)
        {
            product_O.Text = "";
            dt_product_O.Value = DateTime.Now;
        }

        private void btn_installationrm_O_Click(object sender, EventArgs e)
        {
            installation_O.Text = "";
            dt_installation_O.Value = DateTime.Now;
        }

        private void btn_save_O_Click(object sender, EventArgs e)
        {
            Order currentOrder = getCurrentOrder();

            if (currentOrder != null)
            {
                Client currentOrderClient = currentOrder.client;

                using (var context = new MyDbConnection())
                {
                    if (currentOrder != null)
                    {
                        Database.Entities.Order existingOrder = context.Orders.Find(currentOrder.id);
                        if (existingOrder != null)
                        {
                            //numer 
                            if (!ordernumber_O.Text.IsNullOrEmpty())
                            {
                                string number = ordernumber_O.Text.ToLower();
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
                                    existingOrder.order_number = nr;
                                    existingOrder.company_number = company;
                                }
                            }

                            existingOrder.order_date = stringToDateTime(orderdate_O.Text);

                            if (!string.IsNullOrWhiteSpace(delivery_O.Text))
                            {
                                existingOrder.planned_delivery = int.Parse(delivery_O.Text);
                            }

                            if (!string.IsNullOrWhiteSpace(instances.Text))
                            {
                                existingOrder.instance = int.Parse(instances.Text);
                            }

                            if (!string.IsNullOrWhiteSpace(squared_meters.Text))
                            {
                                existingOrder.squared_meters = double.Parse(squared_meters.Text);
                            }

                            existingOrder.delivery_date = stringToDateTime(deliverydate_O.Text);
                            existingOrder.payment_products = stringToDateTime(product_O.Text);
                            existingOrder.payment_installation = stringToDateTime(installation_O.Text);
                            existingOrder.invoice = fv_O.Text;
                            existingOrder.supplier_order_number = deliverynumber_O.Text;
                            existingOrder.notes = notes_O.Text;
                            existingOrder.notes_information = notes_information.Text;

                            if (cb_employee.SelectedValue != null)
                            {
                                int id = (int)cb_employee.SelectedValue;
                                existingOrder.employee_id = id;
                            }
                            existingOrder.state = state.Text;

                            // ☐  ☑   ☒
                            if (btn_is_with_installation.Text.Equals("☐")) existingOrder.is_with_installation = 0;
                            else if (btn_is_with_installation.Text.Equals("☒"))
                            {
                                existingOrder.is_with_installation = 1;
                                existingOrder.installation_address = iswithinstallation.Text;
                            }

                            int prodId = Convert.ToInt32(cb_product_O.SelectedValue);
                            existingOrder.product_id = prodId;
                        }
                    }
                    context.SaveChanges();
                }


                loadFromOtherWindow(currentOrder);
            }
        }


        private void selectOrdersGridView(Order o)
        {
            OrdersGridView.ClearSelection();

            //foreach(Order order in filteredOrders)
            for (int i = 0; i < filteredOrders.Count(); i++)
            {
                if (filteredOrders[i].id == o.id)
                {
                    OrdersGridView.Rows[i].Selected = true;
                    OrdersGridView.FirstDisplayedScrollingRowIndex = i;
                    break;
                }
            }
            /*
            foreach (DataGridViewRow row in OrdersGridView.Rows)
            {
                if (row.Index >= 0 && filteredOrders[row.Index].id == o.id)
                {
                    MessageBox.Show("linia: " + row.Index);
                    row.Selected = true;
                    OrdersGridView.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
            }
            */
        }

        private void selectOrdersCurrentRow()
        {
            int currentOrderIndex = getCurrentIndex();

            if (currentOrderIndex >= 0 && currentOrderIndex < OrdersGridView.Rows.Count)
            {
                OrdersGridView.FirstDisplayedScrollingRowIndex = currentOrderIndex;
                OrdersGridView_CellClick(OrdersGridView, new DataGridViewCellEventArgs(0, currentOrderIndex));
            }
        }

        private void btn_premium_O_Click(object sender, EventArgs e)
        {
            filterCompanyOrder += 1;
            filterCompanyOrder = filterCompanyOrder % 3;

            filterTable();

        }

        private void btn_refresh_O_Click(object sender, EventArgs e)
        {
            loadDataOrdersAsync();
            //filterCompanyOrder = (mainForm.company + 1) % 3;
            if (mainForm.isAdmin == 1)
            {
                filterCompanyOrder = 0;
            }
            else
            {
                filterCompanyOrder = (mainForm.company + 1) % 3;
            }
            btn_premium_O.Text = getCompanyName(filterCompanyOrder);
            search.Clear();

            filter_invoice = 0;
            filter_number = 0;
            filter_orderdate = 0;
            filter_delivarydate = 0;
        }

        private string getCompanyName(int company)      // 0 - wszystkie, 1 - tomkar, 2 - premium
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

        private void btn_save_OC_Click(object sender, EventArgs e)
        {
            Order currentOrder = getCurrentOrder();

            if (currentOrder != null)
            {
                Client currentOrderClient = currentOrder.client;

                DirectoryManager dm = new DirectoryManager(currentOrderClient.id, currentOrderClient.first_name, currentOrderClient.last_name);

                if (currentOrderClient != null)
                {
                    using (var context = new MyDbConnection())
                    {
                        Database.Client existingClient = context.Clients.Find(currentOrderClient.id);
                        if (existingClient != null)
                        {
                            existingClient.first_name = name_OC.Text;
                            existingClient.last_name = lastname_OC.Text;
                            existingClient.phone_number = phone_OC.Text;
                            existingClient.address = address_OC.Text;
                            existingClient.email = mail_OC.Text;
                        }

                        context.SaveChanges();

                        dm.updateDirectory(existingClient.first_name, existingClient.last_name);

                        loadDataOrdersAsync();

                        if (currentOrder != null && currentOrderClient != null)
                        {
                            selectOrdersGridView(currentOrder);
                            selectOrdersCurrentRow();

                        }
                    }
                }
            }
        }

        private void btn_opendir_OC_Click(object sender, EventArgs e)
        {
            Order currentOrder = getCurrentOrder();

            if (currentOrder != null)
            {
                Client currentOrderClient = currentOrder.client;

                if (currentOrderClient != null)
                {
                    DirectoryManager dm = new DirectoryManager(currentOrderClient.id, currentOrderClient.first_name, currentOrderClient.last_name);
                    dm.openDirectory();
                }
            }
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

        private void btn_complaint_Click(object sender, EventArgs e)
        {
            Order selected = getCurrentOrder();
            int index = getCurrentIndex();

            if (OrdersGridView.SelectedRows.Count > 0 && selected != null)
            {
                //var selectedRow = OrdersGridView.SelectedRows[0];
                // int selectedOrderId = Convert.ToInt32(selectedRow.Cells["IdOrder"].Value);

                //Order currentOrder = getCurrentOrder();

                //if (selected != null)
                {
                    FormAddComplaint newForm = new FormAddComplaint(selected);
                    newForm.StartPosition = FormStartPosition.Manual;
                    newForm.Top = this.Top + 50;
                    newForm.Left = this.Left + 100;

                    newForm.FormClosing += AddOrderForm_FormClosing;
                    newForm.Show();
                }
            }
        }


        private Order getCurrentOrder()
        {
            if (OrdersGridView.SelectedRows.Count > 0 && filteredOrders.Count > 0)
            {
                var selectedId = OrdersGridView.SelectedRows[0].Index;
                var selected = filteredOrders[selectedId];
                return selected;
            }

            return null;
        }


        private int getCurrentIndex()
        {
            if (OrdersGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = OrdersGridView.SelectedRows[0];
                return selectedRow.Index;
            }
            else
            {
                return -1;
            }
        }

        private void btn_refresh_O_Click_1(object sender, EventArgs e)
        {
            loadDataOrdersAsync();

            //filterCompanyOrder = (mainForm.company + 1) % 3;
            if (mainForm.isAdmin == 1)
            {
                filterCompanyOrder = 0;
            }
            else
            {
                filterCompanyOrder = (mainForm.company + 1) % 3;
            }
            btn_premium_O.Text = getCompanyName(filterCompanyOrder);
            search.Clear();
            complaint.FillColor = System.Drawing.Color.White;
            complaint.Text = "";
        }

        private void search_TextChanged(object sender, EventArgs e)
        {
            searchText = search.Text.ToLower();
            filterTable();
        }

        private void filterTable()
        {

            List<Order> filtered = new List<Order>(orders);

            if (!string.IsNullOrEmpty(searchText))
            {
                filtered = filtered
                    .Where(o => o != null &&
                                (o.id.ToString().Contains(searchText) ||
                                 OrderNumberWithCompany(o.order_number, o.company_number)?.ToString().ToLower().Contains(searchText) == true ||
                                 (o.product?.name?.ToLower().Contains(searchText) == true) ||
                                 (o.client?.first_name?.ToLower().Contains(searchText) == true) ||
                                 (o.client?.last_name?.ToLower().Contains(searchText) == true) ||
                                 (o.employee?.first_name?.ToLower().Contains(searchText) == true) ||
                                 (o.invoice?.ToLower().Contains(searchText) == true))
                    ).ToList();
            }


            switch (filterCompanyOrder)
            {
                case 0:
                    btn_premium_O.Text = "Wszystkie";
                    break;
                case 1:
                    //filtered = filtered.Where(o => o.employee?.company == 0).ToList();
                    filtered = filtered.Where(o => o.company_number == 0).ToList();
                    btn_premium_O.Text = "Tomkar";

                    break;
                case 2:
                    //filtered = filtered.Where(o => o.employee?.company == 1).ToList();
                    filtered = filtered.Where(o => o.company_number == 1).ToList();
                    btn_premium_O.Text = "Premium";
                    break;
            }

            //OrdersGridView.VirtualMode = true;
            filteredOrders = filtered.OrderBy(o => o.id).ToList();
            setDataInOrder();

            if (filteredOrders.Count > 0)
            {
                OrdersGridView.RowCount = filteredOrders.Count;
            }
            else
            {
                OrdersGridView.RowCount = 0;
            }

            OrdersGridView.Refresh();
            OrdersGridView.Invalidate();

        }

        private void complaint_Click(object sender, EventArgs e)
        {
            Order currentOrder = getCurrentOrder();

            if (currentOrder != null)
            {
                using (var context = new MyDbConnection())
                {
                    Complaint c = context.Complaints.FirstOrDefault(compl => compl.order_id == currentOrder.id);


                    if (c != null)
                    {
                        mainForm.openComplaintFromOrder(c);
                    }
                }
            }
        }

        private void btn_payments_Click(object sender, EventArgs e)
        {
            Order currentOrder = getCurrentOrder();

            if (currentOrder != null)
            {
                using (var context = new MyDbConnection())
                {
                    Payment p = context.Payments.FirstOrDefault(paym => paym.order_id == currentOrder.id);

                    if (p != null)
                    {
                        mainForm.openPaymentFromOrder(p);
                    }
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
                    gv.AlternatingRowsDefaultCellStyle.Font = new Font("Calibri", 10);

                    gv.ColumnHeadersHeight = 30;
                    gv.RowTemplate.MinimumHeight = 40;

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
                    gv.RowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);


                    gv.MultiSelect = false;
                    gv.AllowUserToResizeRows = false;

                    gv.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);


                    gv.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(245, 219, 204);
                    gv.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(0, 0, 0);


                    gv.AlternatingRowsDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(245, 219, 204);
                    gv.AlternatingRowsDefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(0, 0, 0);

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

                    if (btn.Name == "complaint")
                    {
                        btn.BorderThickness = 3;
                        btn.TextAlign = HorizontalAlignment.Center;
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

        private void OrdersGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            string sortByValue = OrdersGridView.Columns[e.ColumnIndex].DataPropertyName;


            if (sortByValue != null && sortByValue.Equals("Number"))
            {
                filter_number += 1;
                filter_number %= 3;

                if (filter_number == 0) filterTable();
                else if (filter_number == 1) filteredOrders = filteredOrders.OrderBy(o => o.order_number).ToList();
                else if (filter_number == 2) filteredOrders = filteredOrders.OrderByDescending(o => o.order_number).ToList();
            }
            else if (sortByValue != null && sortByValue.Equals("OrderDate"))
            {
                filter_orderdate += 1;
                filter_orderdate %= 3;

                if (filter_orderdate == 0) filterTable();
                else if (filter_orderdate == 1) filteredOrders = filteredOrders.OrderBy(o => dateTimeToString(o.order_date)).ToList();
                else if (filter_orderdate == 2) filteredOrders = filteredOrders.OrderByDescending(o => dateTimeToString(o.order_date)).ToList();
            }
            else if (sortByValue != null && sortByValue.Equals("DeliveryDate"))
            {
                filter_delivarydate += 1;
                filter_delivarydate %= 3;

                if (filter_delivarydate == 0) filterTable();
                else if (filter_delivarydate == 1) filteredOrders = filteredOrders.OrderBy(o => dateTimeToString(o.delivery_date)).ToList();
                else if (filter_delivarydate == 2) filteredOrders = filteredOrders.OrderByDescending(o => dateTimeToString(o.delivery_date)).ToList();
            }
            else if (sortByValue != null && sortByValue.Equals("Invoice"))
            {
                filter_invoice += 1;
                filter_invoice %= 3;

                if (filter_invoice == 0) filterTable();
                else if (filter_invoice == 1) filteredOrders = filteredOrders.OrderBy(o => o.invoice).ToList();
                else if (filter_invoice == 2) filteredOrders = filteredOrders.OrderByDescending(o => o.invoice).ToList();
            }

            OrdersGridView.Refresh();
            setDataInOrder();


        }

        private void btn_is_with_installation_Click(object sender, EventArgs e)
        {
            Order o = getCurrentOrder();
            if (o != null)
            {
                // ☐  ☑   ☒
                if (btn_is_with_installation.Text.Equals("☒"))
                {
                    btn_is_with_installation.Text = "☐";
                    iswithinstallation.Text = "Bez montażu";
                    iswithinstallation.ReadOnly = true;
                }
                else if (btn_is_with_installation.Text.Equals("☐"))
                {

                    btn_is_with_installation.Text = "☒";
                    if (o.installation_address != null)
                    {
                        iswithinstallation.Text = o.installation_address;
                    }
                    else
                    {
                        iswithinstallation.Text = "";
                    }
                    iswithinstallation.ReadOnly = false;
                }
            }
        }



        private void OrdersGridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo hit = OrdersGridView.HitTest(e.X, e.Y);

                if (hit.Type == DataGridViewHitTestType.Cell)
                {
                    clickedRowIndex = hit.RowIndex;

                    OrdersGridView.ClearSelection();
                    OrdersGridView.Rows[clickedRowIndex].Selected = true;

                    contextMenu.Show(OrdersGridView, e.Location);
                }
            }
        }

        private void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem != null)
            {
                string selectedColor = e.ClickedItem.Name;
                DataGridViewRow clickedRow = OrdersGridView.Rows[clickedRowIndex];

                var id = filteredOrders[clickedRow.Index].id;        // (int)clickedRow.Cells["IdOrder"].Value;
                string hexColor = "";

                switch (selectedColor)
                {
                    case "Red":
                        {
                            //Color color = ColorTranslator.FromHtml("199; 67; 117");   
                            //clickedRow.DefaultCellStyle.BackColor = color;
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
                    Order order = context.Orders.Find(id);
                    if (order != null)
                    {
                        order.color_rgb = hexColor;
                        context.SaveChanges();
                        filteredOrders[clickedRow.Index].color_rgb = hexColor;
                        if (hexColor.Equals("")) OrdersGridView.Rows[clickedRowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);

                        //if (hexColor != "") OrdersGridView.Rows[clickedRowIndex].DefaultCellStyle.BackColor = ColorTranslator.FromHtml(hexColor);
                        //else OrdersGridView.Rows[clickedRowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                    }
                }
            }
        }

        private void OrdersGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.RowIndex < filteredOrders.Count)
            {
                Order order = filteredOrders[e.RowIndex];

                if (order != null && !string.IsNullOrEmpty(order.color_rgb))
                {
                    try
                    {
                        OrdersGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = ColorTranslator.FromHtml(order.color_rgb);
                    }
                    catch (Exception ex)
                    {
                        // Obsługa ewentualnych wyjątków, np. nieprawidłowy format koloru
                        // MessageBox.Show($"Błąd formatu koloru dla zamówienia {recordId}: {ex.Message}");
                    }
                }
                else
                {
                    if (e.RowIndex % 2 == 0)
                    {
                        OrdersGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
                    }
                    else
                    {
                        OrdersGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }

        }

        private void OrdersGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Order o = getCurrentOrder();

            if (o != null)
            {
                if (OrdersGridView.Rows[e.RowIndex].Selected)
                {
                    Rectangle rect = e.RowBounds;

                    /*
                    if (o.color_rgb != null && !string.IsNullOrEmpty(o.color_rgb))
                    {
                        Color color = ColorTranslator.FromHtml(o.color_rgb);
                        color = Color.FromArgb(color.A, (int)color.R + 50, (int)color.G + 50, (int)color.B + 50);
                        OrdersGridView.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = color;//ColorTranslator.FromHtml(o.color_rgb); //System.Drawing.Color.FromArgb(245, 219, 204);
                        OrdersGridView.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Black; //System.Drawing.Color.FromArgb(0, 0, 0);
                    }
                    else
                    {
                        OrdersGridView.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.White;//ColorTranslator.FromHtml(o.color_rgb); //System.Drawing.Color.FromArgb(245, 219, 204);
                        OrdersGridView.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Black; //Syst
                    }
                    */

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

        private void OrdersGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            // Sprawdź, czy indeks wiersza jest poprawny
            if (e.RowIndex >= 0 && e.RowIndex < filteredOrders.Count)
            {
                var order = filteredOrders[e.RowIndex];

                // Przypisz odpowiednie wartości do komórek na podstawie indeksu kolumny
                switch (OrdersGridView.Columns[e.ColumnIndex].Name)
                {
                    case "Number":
                        e.Value = OrderNumberWithCompany(order.order_number, order.company_number);
                        break;
                    case "Client":
                        e.Value = FullName(order.client.first_name, order.client.last_name);
                        break;
                    case "OrderDate":
                        e.Value = dateTimeToString(order.order_date);
                        break;
                    case "PlannedDelivery":
                        if (order.planned_delivery > 0) e.Value = order.planned_delivery;
                        break;
                    case "DeliveryDate":
                        e.Value = dateTimeToString(order.delivery_date);
                        break;
                    case "ProductPay":
                        e.Value = dateTimeToString(order.payment_products);
                        break;
                    case "InstallationPay":
                        e.Value = dateTimeToString(order.payment_installation);
                        break;
                    case "Invoice":
                        e.Value = order.invoice;
                        break;
                    case "Employee":
                        e.Value = order.employee.first_name;
                        break;
                    case "Product":
                        e.Value = order.notes;
                        break;
                }
            }
        }

        private void change_client_DoubleClick(object sender, EventArgs e)
        {
            Order current = getCurrentOrder();

            if (current != null)
            {
                FormChangeClient newForm = new FormChangeClient(current);

                newForm.StartPosition = FormStartPosition.Manual;
                newForm.Location = new Point(guna2Panel15.Location.X, guna2Panel15.Location.Y);

                newForm.FormClosing += OtherForm_FormClosing;
                newForm.Show();
            }
        }

        private void OtherForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            loadFromOtherWindow(getCurrentOrder());
        }
    }

}
