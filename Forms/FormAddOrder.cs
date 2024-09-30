using Castle.Core.Internal;
using DocumentFormat.OpenXml.ExtendedProperties;
using Guna.UI2.WinForms;
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WindowsFormsAppMySql.Classes;
using WindowsFormsAppMySql.Database;
using WindowsFormsAppMySql.Database.Entities;

namespace WindowsFormsAppMySql.Forms
{

    public partial class FormAddOrder : Form
    {
        Employee mainEmployee;

        List<Product> products = new List<Product>();
        List<Client> clients = new List<Client>();
        List<Bill> bills = new List<Bill>();
        List<Measurement> measurements = new List<Measurement>();
        List<Payment> payments = new List<Payment>();
        List<BillToReview> billsToReview = new List<BillToReview>();
        List<BillToReview> billsToReviewList = new List<BillToReview>();


        List<Order> ordersToAdd = new List<Order>();    //lista stworzonych zamowien
        List<Payment> paymentsToAdd = new List<Payment>();  //lista stworzonych płatnosci


        double netPurchasePrice = 0;         // cena_zakupowa_netto
        double grossProductPrice = 0;        // cena_towaru_umowa_brutto
        double grossInstallationPrice = 0;   // cena_montazu_brutto
        int vatRate = 0;
        double totalGrossPrice = 0;          // cena_calkowita_brutto
        double materialPrice = 0;            // cena_materialow

        private int orderFromMeasurement = 0;
        private Measurement measurementToOrder;
        private int is_with_installation = 0;


        public FormAddOrder(Employee mainEmployee)
        {
            this.mainEmployee = mainEmployee;

            InitializeComponent();
            ApplyStyles(this);
            loadData();

            //eventy
            cb_bill_AO.MouseWheel += new MouseEventHandler(preventMouseWheel);
            cb_product_AO.MouseWheel += new MouseEventHandler(preventMouseWheel);
            cb_client_AO.MouseWheel += new MouseEventHandler(preventMouseWheel);


            //obrazy
            btn_orderdate_AO.Image = Properties.Resources.calendar;
            btn_orderdaterm_AO.Image = Properties.Resources.minus;

            btn_deliverydate_AO.Image = Properties.Resources.calendar;
            deliverydaterm_AO.Image = Properties.Resources.minus;

            btn_updatestate.Image = Properties.Resources.plus;
            btn_addclient_AO.Image = Properties.Resources.plus;
            btn_updateaddress_CAO.Image = Properties.Resources.plus;
            btn_updateaddress2_CAO.Image = Properties.Resources.plus;
            btn_fillremainingprice_AO.Image = Properties.Resources.equal;

            cb_billrm_AO.Image = Properties.Resources.minus;

            cb_product_AO.DataSource = products;
            cb_product_AO.DisplayMember = "name";
            cb_product_AO.ValueMember = "id";
            cb_product_AO.SelectedIndex = -1;

            cb_client_AO.DataSource = clients;
            cb_client_AO.DisplayMember = "DisplayName";
            cb_client_AO.ValueMember = "id";
            cb_client_AO.SelectedIndex = -1;

            vat_AO.Text = "8";
        }

        private void preventMouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        public FormAddOrder(Employee mainEmployee, Measurement measurement, Client client)
        {
            this.mainEmployee = mainEmployee;
            this.measurementToOrder = measurement;

            InitializeComponent();
            ApplyStyles(this);

            loadData();

            cb_product_AO.DataSource = products;
            cb_product_AO.DisplayMember = "name";
            cb_product_AO.ValueMember = "id";
            cb_product_AO.SelectedIndex = -1;

            cb_client_AO.DataSource = clients;
            cb_client_AO.DisplayMember = "DisplayName";
            cb_client_AO.ValueMember = "id";
            cb_client_AO.SelectedValue = client.id;

            address2_CAO.Text = measurement.measurement_address.ToString();
            state_CAO.Text = measurement.status.ToString();

            vat_AO.Text = "8";

            orderFromMeasurement = 1;
        }


        private void loadData()
        {
            using (var context = new MyDbConnection())
            {
                products = context.Products.Where(p => p.visible == 1).ToList();
                clients = context.Clients.ToList();
                payments = context.Payments.ToList();
                payments = GetUniqueElements(payments);


                foreach (var payment in payments)
                {
                    BillToReview newBill = new BillToReview();

                    var order = context.Orders.FirstOrDefault(x => x.id == payment.order_id);
                    var bill = context.Bills.FirstOrDefault(x => x.id == payment.bill_id);

                    if (order != null && bill != null)
                    {
                        newBill.Client = order.client;
                        newBill.Bill = bill;

                        billsToReview.Add(newBill);
                    }

                }

                measurements = context.Measurements.ToList();
            }
        }

        public static List<Payment> GetUniqueElements(List<Payment> paymentsList)
        {
            HashSet<int> seenVariables = new HashSet<int>();
            List<Payment> uniqueElements = new List<Payment>();

            foreach (var p in paymentsList)
            {
                if (!seenVariables.Contains(p.bill_id))
                {
                    seenVariables.Add(p.bill_id);
                    uniqueElements.Add(p);
                }
            }

            return uniqueElements;
        }

        private void cb_client_AO_SelectedIndexChanged(object sender, EventArgs e)
        {
            clearClientInfo();

            Client selectedClient = cb_client_AO.SelectedItem as Client;
            if (selectedClient != null)
            {
                name_CAO.Text = selectedClient.first_name + " " + selectedClient.last_name;
                phone_CAO.Text = selectedClient.phone_number;
                mail_CAO.Text = selectedClient.email;
                address_CAO.Text = selectedClient.address;
                Measurement m = measurements.Where(x => x.client_id == selectedClient.id)
                                            .OrderByDescending(x => x.id)
                                            .FirstOrDefault() as Measurement;
                if (m != null)
                {
                    address2_CAO.Text = m.measurement_address;
                    state_CAO.Text = m.status;
                }
            }


            Client c = cb_client_AO.SelectedItem as Client;


            billsToReviewList.Clear();

            foreach (var bill in billsToReview)
            {
                if (c != null && bill.Client != null && c.id == bill.Client.id)
                {
                    billsToReviewList.Add(bill);
                }
            }

            cb_bill_AO.DataSource = null;
            cb_bill_AO.DataSource = billsToReviewList;
            cb_bill_AO.DisplayMember = "DisplayBill";
            cb_bill_AO.SelectedIndex = -1;

        }

        private void clearClientInfo()
        {
            name_CAO.Clear();
            phone_CAO.Clear();
            mail_CAO.Clear();
            address_CAO.Clear();
            address2_CAO.Clear();
            state_CAO.Clear();
        }

        private void btn_updateaddress_CAO_Click(object sender, EventArgs e)
        {
            if (!address_CAO.Text.IsNullOrEmpty())
            {
                installationaddress_AO.Text = address_CAO.Text;
            }
        }

        private void btn_updateaddress2_CAO_Click(object sender, EventArgs e)
        {
            if (!address2_CAO.Text.IsNullOrEmpty())
            {
                installationaddress_AO.Text = address2_CAO.Text;
            }
        }

        private void btn_orderdate_AO_Click(object sender, EventArgs e)
        {
            openCalendar(sender as Guna2Button, dt_orderdate_AO);
        }

        private void btn_orderdaterm_AO_Click(object sender, EventArgs e)
        {
            orderdate_AO.Text = "";
            dt_orderdate_AO.Value = System.DateTime.Now;
        }

        private void btn_deliverydate_AO_Click(object sender, EventArgs e)
        {
            openCalendar(sender as Guna2Button, dt_deliverydate_AO);
        }

        private void deliverydaterm_AO_Click(object sender, EventArgs e)
        {
            deliverydate_AO.Text = "";
            dt_deliverydate_AO.Value = System.DateTime.Now;
        }

        private void dt_orderdate_AO_CloseUp(object sender, EventArgs e)
        {
            closeCalendar(sender as MetroDateTime, orderdate_AO);
        }

        private void dt_deliverydate_AO_CloseUp(object sender, EventArgs e)
        {
            closeCalendar(sender as MetroDateTime, deliverydate_AO);
        }

        public void openCalendar(Guna2Button btn, MetroDateTime cal)
        {
            cal.Location = new System.Drawing.Point(btn.Location.X, btn.Location.Y + btn.Height);
            cal.Visible = true;
            cal.Focus();
            SendKeys.Send("{F4}");
        }
        public void closeCalendar(MetroDateTime cal, Guna2TextBox textbox)
        {
            System.DateTime selectedDate = cal.Value;
            textbox.Text = dateTimeToString(selectedDate);
        }


        private bool IsTextValid(string text)
        {
            Regex regex = new Regex(@"^\d{0,15}(\.\d{0,2})?$");
            return regex.IsMatch(text);
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


        private void btn_fillremainingprice_AO_Click(object sender, EventArgs e)
        {

            //przecinek czy kropka       ????????

            if (double.TryParse(productprice_AO.Text, out double productPrice))
            {
                grossProductPrice = productPrice;
            }

            if (double.TryParse(installationprice_AO.Text, out double installationPrice))
            {
                grossInstallationPrice = installationPrice;
            }


            totalGrossPrice = grossProductPrice + grossInstallationPrice;
            //remainingprice_AO.Text = totalGrossPrice.ToString().Replace(',', '.');
            remainingprice_AO.Text = totalGrossPrice.ToString();
        }






        private void btn_addtolist_AO_Click(object sender, EventArgs e)
        {
            if (cb_client_AO.SelectedIndex > -1 && cb_product_AO.SelectedIndex > -1 && !string.IsNullOrEmpty(number_AO.Text))
            {
                //order 
                Order orderToList = new Order();

                orderToList.installation_address = installationaddress_AO.Text;
                orderToList.notes = notes_AO.Text;
                orderToList.notes_information = notes_information.Text;
                orderToList.order_date = stringToDateTime(orderdate_AO.Text);
                orderToList.delivery_date = stringToDateTime(deliverydate_AO.Text);
                orderToList.supplier_order_number = deliverynumber_AO.Text;
                orderToList.employee_id = mainEmployee.id;
                orderToList.state = state.Text;
                orderToList.is_with_installation = is_with_installation;
                //orderToList.instance = instances.Text;


                if (int.TryParse(instances.Text, out int instance))
                {
                    orderToList.instance = instance;
                }

                if (double.TryParse(squared_meters.Text, out double meters))
                {
                    orderToList.squared_meters = meters;
                }

                if (int.TryParse(delivery_AO.Text, out int plannedDelivery))
                {
                    orderToList.planned_delivery = plannedDelivery;
                }

                string number = number_AO.Text.ToLower();
                //int company;

                if (number.Contains("p"))
                {
                    orderToList.company_number = 1;
                }
                else
                {
                    orderToList.company_number = 0;
                }

                number = number.Replace("p", "").Replace(" ", "");

                if (int.TryParse(number, out int nr))
                {
                    orderToList.order_number = nr;
                }



                Client c = cb_client_AO.SelectedItem as Client;
                if (c != null)
                {
                    //orderToList.Client = clients.First(o => o.Id == c.Id);
                    orderToList.client_id = c.id;
                }

                Product p = cb_product_AO.SelectedItem as Product;
                if (p != null)
                {
                    orderToList.product = products.First(o => o.name == p.name);
                    orderToList.product_id = p.id;
                }


                //payment
                Payment paymentToList = new Payment();

                paymentToList.net_purchase_price = stringToPrice(purchaseprice_AO.Text);
                paymentToList.gross_product_price = stringToPrice(productprice_AO.Text);
                paymentToList.gross_installation_price = stringToPrice(installationprice_AO.Text);
                paymentToList.material_price = stringToPrice(materialprice_AO.Text);

                if (int.TryParse(vat_AO.Text, out int result))
                {
                    paymentToList.var_rate = result;
                }
                else
                {
                    if (paymentToList.gross_installation_price == 0.00)
                    {
                        paymentToList.var_rate = 8;
                    }
                    else
                    {
                        paymentToList.var_rate = 23;
                    }
                }

                paymentToList.total_gross_price = stringToPrice(remainingprice_AO.Text);


                ordersToAdd.Add(orderToList);
                paymentsToAdd.Add(paymentToList);

                var orders = ordersToAdd.Select(o => new { o.product.name, o.notes });
                OrdersGridView.DataSource = orders.ToList();


                //wyczyszczenie pól
                purchaseprice_AO.Clear();
                productprice_AO.Clear();
                installationprice_AO.Clear();
                materialprice_AO.Clear();
                remainingprice_AO.Clear();

                number_AO.Clear();
                cb_product_AO.SelectedIndex = -1;
                notes_AO.Clear();
                orderdate_AO.Clear();
                dt_orderdate_AO.Value = System.DateTime.Now;
                delivery_AO.Clear();
                deliverydate_AO.Clear();
                dt_deliverydate_AO.Value = System.DateTime.Now;
                deliverynumber_AO.Clear();

            }

            else
            {
                MessageBox.Show("Brak numeru zamówienia, wybranego klienta lub produktu");
            }
        }

        private void btn_save_AO_Click(object sender, EventArgs e)
        {
            if (ordersToAdd.Count > 0)
            {
                Bill billToAdd = null;
                BillToReview billToReviewTmp = cb_bill_AO.SelectedItem as BillToReview;


                netPurchasePrice = 0;
                grossProductPrice = 0;
                grossInstallationPrice = 0;
                totalGrossPrice = 0;
                materialPrice = 0;

                for (int i = 0; i < paymentsToAdd.Count; i++)
                {
                    netPurchasePrice += paymentsToAdd[i].net_purchase_price;
                    grossProductPrice += paymentsToAdd[i].gross_product_price;
                    grossInstallationPrice += paymentsToAdd[i].gross_installation_price;
                    totalGrossPrice += paymentsToAdd[i].total_gross_price;
                    materialPrice += paymentsToAdd[i].material_price;
                }


                if (billToReviewTmp != null)
                {
                    using (var context = new MyDbConnection())
                    {
                        billToAdd = context.Bills.FirstOrDefault(b => b.id == billToReviewTmp.Bill.id);

                        billToAdd.net_purchase_price += netPurchasePrice;
                        billToAdd.gross_product_price += grossProductPrice;
                        billToAdd.gross_installation_price += grossInstallationPrice;
                        billToAdd.total_gross_price += totalGrossPrice;
                        billToAdd.material_price += materialPrice;

                        context.SaveChanges();
                    }
                }
                else
                {
                    billToAdd = new Bill();

                    billToAdd.net_purchase_price = netPurchasePrice;
                    billToAdd.gross_product_price = grossProductPrice;
                    billToAdd.gross_installation_price = grossInstallationPrice;
                    billToAdd.total_gross_price = totalGrossPrice;
                    billToAdd.material_price = materialPrice;
                    billToAdd.sum_of_advances = 0.00;

                    using (var context = new MyDbConnection())
                    {
                        context.Bills.Add(billToAdd);
                        context.SaveChanges();
                    }
                }

                using (var context = new MyDbConnection())
                {

                    for (int i = 0; i < ordersToAdd.Count; i++)
                    {
                        Order o = ordersToAdd[i];
                        Payment p = paymentsToAdd[i];

                        //generowanie numeru 
                        /*
                        if (o.company_number == -1)
                        {
                            int nextOrderNumber = 1;

                            var maxOrder = context.Orders
                                .Where(x => x.company_number == mainEmployee.company)
                                .OrderByDescending(x => x.order_number)
                                .FirstOrDefault();

                            if (maxOrder != null)
                            {
                                nextOrderNumber = maxOrder.order_number + 1;
                            }

                            o.order_number = nextOrderNumber;
                            o.company_number = mainEmployee.company;
                        }
                        */



                        var existingClient = context.Clients.First(x => x.id == o.client_id);
                        var existingProduct = context.Products.First(x => x.id == o.product_id);

                        o.product = existingProduct;
                        o.client = existingClient;

                        if (orderFromMeasurement == 1)
                        {
                            o.measurement_id = measurementToOrder.id;
                        }

                        context.Orders.Add(o);
                        context.SaveChanges();


                        p.order = o;
                        p.order_id = o.id;
                        p.bill_id = billToAdd.id;

                        context.Payments.Add(p);
                        context.SaveChanges();
                    }
                }

                this.Close();
            }
        }




        public System.DateTime? stringToDateTime(string date)
        {
            if (string.IsNullOrEmpty(date)) return null;
            else return System.DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        }

        public string dateTimeToString(System.DateTime date)
        {
            if (date == null) return "";
            else return date.ToString("dd.MM.yyyy");
        }

        public double stringToPrice(string price)
        {
            //if (double.TryParse(price.Replace('.', ','), out double result))
            if (double.TryParse(price, out double result))
            {
                return result;
            }

            return 0.00;
        }


        private void cb_billrm_AO_Click(object sender, EventArgs e)
        {
            cb_bill_AO.SelectedIndex = -1;
        }

        private void OrdersGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                ordersToAdd.RemoveAt(e.RowIndex);
                paymentsToAdd.RemoveAt(e.RowIndex);

                OrdersGridView.DataSource = null;
                //OrdersGridView.DataSource = ordersToAdd;
                var orders = ordersToAdd.Select(o => new { o.product.name, o.notes });
                OrdersGridView.DataSource = orders.ToList();
            }

        }

        private void btn_addclient_AO_Click(object sender, EventArgs e)
        {
            FormAddClient newForm = new FormAddClient(mainEmployee);
            newForm.FormClosing += OtherForm_FormClosingClient;
            newForm.ShowDialog();
        }

        private void OtherForm_FormClosingClient(object sender, FormClosingEventArgs e)
        {
            int before = cb_client_AO.Items.Count;
            loadData();

            cb_client_AO.DataSource = null;
            cb_client_AO.DataSource = clients;
            cb_client_AO.DisplayMember = "DisplayName";
            cb_client_AO.ValueMember = "id";
            cb_client_AO.SelectedIndex = -1;

            int after = cb_client_AO.Items.Count;

            if (after != before)
            {
                cb_client_AO.SelectedIndex = cb_client_AO.Items.Count - 1;
            }
        }

        private void ApplyStyles(Control control)
        {
            foreach (Control ctrl in control.Controls)
            {
                if (ctrl is Guna2DataGridView)
                {
                    Guna2DataGridView gv = (Guna2DataGridView)ctrl;
                    gv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Calibri", 10, FontStyle.Bold);
                    gv.DefaultCellStyle.Font = new System.Drawing.Font("Calibri", 10);
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
                    gv.AlternatingRowsDefaultCellStyle.Font = new System.Drawing.Font("Calibri", 10);
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

                    textBox.Font = new System.Drawing.Font("Calibri", 12);
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

        private void client_search_TextChanged(object sender, EventArgs e)
        {
            string text = client_search.Text.ToLower();
            var filteredClients = clients
                .Where(c => (c.first_name?.ToLower().Contains(text) ?? false) || (c.last_name?.ToLower().Contains(text) ?? false) || (c.id.ToString().Contains(text)))
                .ToList();

            if (filteredClients.Count > 0)
            {

                cb_client_AO.DropDownStyle = ComboBoxStyle.DropDown;
                cb_client_AO.DataSource = null;
                cb_client_AO.DataSource = filteredClients;
                cb_client_AO.DisplayMember = "DisplayName";
                cb_client_AO.ValueMember = "id";
            }
            else
            {
                cb_client_AO.DroppedDown = false;
                cb_client_AO.DataSource = null;
                cb_client_AO.DisplayMember = "DisplayName";
                cb_client_AO.ValueMember = "id";
            }
        }

        private void btn_updatestate_Click(object sender, EventArgs e)
        {
            state.Text = state_CAO.Text;
        }


        // zieloiny - #8d9c63  fiolet - #E9DEEC
        private void without_installation_Click(object sender, EventArgs e)
        {
            is_with_installation = 0;
            with_installation.FillColor = ColorTranslator.FromHtml("#E9DEEC");
            without_installation.FillColor = ColorTranslator.FromHtml("#8d9c63");
        }

        private void with_installation_Click(object sender, EventArgs e)
        {
            is_with_installation = 1;
            without_installation.FillColor = ColorTranslator.FromHtml("#E9DEEC");
            with_installation.FillColor = ColorTranslator.FromHtml("#8d9c63");
        }

    }

}
