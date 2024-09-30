using Castle.Core.Internal;
using Guna.UI2.WinForms;
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsAppMySql.Database;
using WindowsFormsAppMySql.Database.Entities;
using WindowsFormsAppMySql.FileManaging;
using System.Data.Entity;


namespace WindowsFormsAppMySql.Forms
{
    public partial class f_Payments : Form
    {
        private Payment mainPayment;
        private f_Main mainForm;
        private string searchText = "";
        private int filterCompanyPayment = 0;


        List<Order> orders = new List<Order>();
        List<Product> products = new List<Product>();
        List<Payment> payments = new List<Payment>();
        List<Payment> filteredPayments = new List<Payment>();
        List<Bill> bills = new List<Bill>();
        List<Bill> filteredBills = new List<Bill>();
        List<Advance> advances = new List<Advance>();

        private int clickedRowIndex = -1;

        public f_Payments(f_Main mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();

            ApplyStyles(this);

            load();
        }

        public f_Payments(f_Main mainForm, Payment payment)
        {
            this.mainPayment = payment;
            this.mainForm = mainForm;
            InitializeComponent();
            ApplyStyles(this);


            loadFromOtherWindow(payment.bill);
        }

        private void load()
        {
            loadDataAsync();

            //obrazy
            btn_refresh.Image = Properties.Resources.refresh;
            fillReminingPrice.Image = Properties.Resources.equal;
        }

        private async void loadFromOtherWindow(Bill bill)
        {
            await loadDataAsync();
            selectPaymentGridView(bill);
            selectPaymentCurrentRow();
        }

        private async Task loadDataAsync()
        {
            clearBillInfo();
            clearPaymentsInfo();

            using (var context = new MyDbConnection())
            {
                products = context.Products.Where(p => p.visible == 1).ToList();
                bills = context.Bills.OrderByDescending(b => b.id).ToList();
                advances = context.Advances.ToList();

                payments = await context.Payments
                          .Include(o => o.order)
                          .Include(o => o.bill)
                          .ToListAsync();

                orders = await context.Orders
                          .Include(o => o.client)
                          .Include(o => o.product)
                          .Include(o => o.employee)
                          .ToListAsync();

                filteredBills = bills;


                if (mainForm.isAdmin == 1)
                {
                    filteredPayments = payments;
                    filteredBills = bills.OrderBy(b => b.id).ToList();
                }
                else
                {
                    filteredPayments = payments.Where(p => p.order.company_number == mainForm.company).ToList();
                    filteredBills = filteredBills.Where(b => filteredPayments.FirstOrDefault(p => p.bill_id == b.id) != null).OrderBy(b => b.id).ToList();
                }


                if (filteredBills.Count <= 0) PaymentsGridView.RowCount = 0;
                else PaymentsGridView.RowCount = filteredBills.Count;

                PaymentsGridView.VirtualMode = true;
            }

            setDataInOrder();
        }


        private void setDataInOrder()
        {
            int lastRowIndex = PaymentsGridView.Rows.Count - 1;
            PaymentsGridView.ClearSelection();
            PaymentsGridView.Rows[lastRowIndex].Selected = true;
            PaymentsGridView.FirstDisplayedScrollingRowIndex = lastRowIndex;
        }


        private void PaymentsGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < filteredBills.Count)
            {
                Bill bill = filteredBills[e.RowIndex];
                List<Payment> payment = payments.Where(p => p.bill_id == bill.id).ToList();

                if (bill != null && payment.Count > 0)
                {
                    switch (PaymentsGridView.Columns[e.ColumnIndex].Name)
                    {
                        case "Number":
                            e.Value = getPremiumPaymentNumber(payment[0].id, payment[0].order.company_number);
                            break;
                        case "Product":
                            e.Value = string.Join(", ", orders
                                    .Where(o => payment.Select(p => p.order_id).Contains(o.id))
                                    .Select(o => o.product.name ?? "").Distinct().ToList());

                            break;
                        case "Client":
                            e.Value = FullName(payment[0].order.client.first_name, payment[0].order.client.last_name);
                            break;
                    }
                }
            }
        }


        private void clearPaymentsInfo()
        {
            netPurchasePriceO.Clear();
            grossProductPriceO.Clear();
            grossInstallationPriceO.Clear();
            materialPriceO.Clear();
            totalGrossPriceO.Clear();
            vatO.Clear();
            btn_product.Text = "☐";
            btn_installation.Text = "☐";
            productAdvance.Clear();
            installationAdvance.Clear();
        }

        private void clearBillInfo()
        {
            netPurchasePrice.Clear();
            grossProductPrice.Clear();
            grossInstallationPrice.Clear();
            materialPrice.Clear();
            totalGrossPrice.Clear();
            reminingPrice.Clear();
            notes.Clear();
        }

        private string getPremiumPaymentNumber(int id, int? company)
        {
            if (company == 0)
            {
                return id.ToString();
            }
            else if (company == 1)
            {
                return id + "P";
            }
            else
            {
                return id.ToString();
            }
        }

        private void PaymentsGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            clearPaymentsInfo();
            clearBillInfo();

            Bill bill = getCurrentBill();

            if (e.RowIndex > -1 && bill != null)
            {
                filteredPayments = payments.Where(p => p.bill_id == bill.id).ToList();

                fillBillInfo(bill);
                fillPaymentsOrdersInfo(bill);
                fillPaymentsAdvancesInfo(bill);

                if (filteredPayments.Count > 0)
                {
                    Client client = filteredPayments[0].order.client;                //context.Payments.FirstOrDefault(p => p.bill_id == currentBill.id).order.client;
                    if (client != null)
                    {
                        fillClientInfo(client);
                    }
                }
            }
        }

        private void fillPaymentsAdvancesInfo(Bill bill)
        {
            var advancesList = advances.Where(a => a.bill_id == bill.id);

            var advancesTable = advancesList.Select(a => new
            {
                Id = a.id,
                Date = dateTimeToString((DateTime)a.date),
                Amount = priceToString(a.amount),
            }).ToList();

            AdvancesGridView.DataSource = advancesTable;
            AdvancesGridView.Columns["Id"].Visible = false;
        }

        private void fillPaymentsOrdersInfo(Bill bill)
        {
            var orderIds = payments.Where(p => p.bill_id == bill.id).Select(p => p.order_id).ToList();
            var resultOrders = orders.Where(o => orderIds.Contains(o.id)).ToList();

            var ordersList = resultOrders.Select(o => new
            {
                PId = o.id,
                Id = o.orderCompanyNumber(),
                Product = o.product.name,
                Notes = o.notes
            }).ToList();

            OrderGridView.DataSource = ordersList;
            OrderGridView.Columns["PId"].Visible = false;
            OrderGridView.ClearSelection();

        }

        private void fillBillInfo(Bill bill)
        {
            netPurchasePrice.Text = priceToString(bill.net_purchase_price);
            grossProductPrice.Text = priceToString(bill.gross_product_price);
            grossInstallationPrice.Text = priceToString(bill.gross_installation_price);
            materialPrice.Text = priceToString(bill.material_price);
            totalGrossPrice.Text = priceToString(bill.total_gross_price);
            reminingPrice.Text = priceToString(bill.total_gross_price - bill.sum_of_advances);
            notes.Text = bill.notes;
        }

        private void fillClientInfo(Client c)
        {
            updateListOfFiles(c);
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

        private void OrderGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridViewRow selectedRow = OrderGridView.Rows[e.RowIndex];

                if (selectedRow != null)
                {
                    if (selectedRow.Cells["PId"].Value != null)
                    {
                        int id = Convert.ToInt32(selectedRow.Cells["Pid"].Value);

                        using (var context = new MyDbConnection())
                        {
                            Payment currentPayment = context.Payments.First(p => p.order_id == id);
                            fillPaymentsInfo(currentPayment);
                        }
                    }
                }
            }
        }

        private void fillPaymentsInfo(Payment payment)
        {
            netPurchasePriceO.Text = priceToString(payment.net_purchase_price);
            double product = payment.gross_product_price;
            grossProductPriceO.Text = priceToString(product);
            double installation = payment.gross_installation_price;
            grossInstallationPriceO.Text = priceToString(installation);

            materialPriceO.Text = priceToString(payment.material_price);
            totalGrossPriceO.Text = priceToString(payment.total_gross_price);

            Order ord = getCurrentOrder();
            if (ord != null)
            {
                productAdvance.Text = priceToString(product - ord.product_advance);
                installationAdvance.Text = priceToString(installation - ord.installation_advance);
            }

            vatO.Text = payment.var_rate.ToString();
            if (payment.order.payment_products.HasValue)
            {
                btn_product.Text = "☒";
            }
            else
            {
                btn_product.Text = "☐";
            }

            if (payment.order.payment_installation.HasValue)
            {
                btn_installation.Text = "☒";
            }
            else
            {
                btn_installation.Text = "☐";
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
            Regex regex = new Regex(@"^\d{0,15}(\,\d{0,2})?$");
            return regex.IsMatch(text);
        }

        public string dateTimeToString(DateTime date)
        {
            if (date == null) return "";
            else return date.ToString("dd.MM.yyyy");
        }

        private void btn_advance_Click(object sender, EventArgs e)
        {
            Bill bill = getCurrentBill();

            if (bill == null) return;
            else
            {
                var orderIds = payments.Where(p => p.bill_id == bill.id).Select(p => p.order_id).ToList();
                var resultOrders = orders.Where(o => orderIds.Contains(o.id)).ToList();

                double sumOfAdvances = advances.Where(a => a.bill_id == bill.id).Sum(o => o.amount);


                f_AddAdvance2 fa = new f_AddAdvance2(stringToPrice(advance.Text), resultOrders, payments, sumOfAdvances, bill.id);
                fa.Top = this.Top + 10;
                fa.Left = this.Left + 20;

                fa.FormClosed += new FormClosedEventHandler(AddAdvanceForm_FormClosed);

                fa.Show();
            }
        }

        private void AddAdvanceForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Bill b = getCurrentBill();
            //loadData();
            //loadDataAsync();
            calculateBillAdvances(b.id);
            advance.Clear();

            loadFromOtherWindow(b);
            //selectPaymentGridView(b);
            // selectPaymentCurrentRow();
        }


        private void btn_refresh_Click(object sender, EventArgs e)
        {
            //loadData();
            loadDataAsync();
            search.Clear();

            if (mainForm.isAdmin == 1)
            {
                filterCompanyPayment = 0;
            }
            else
            {
                filterCompanyPayment = (mainForm.company + 1) % 3;
            }

            btn_premium.Text = getCompanyName(filterCompanyPayment);
            filterTable();
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

        private void btn_saveChangesO_Click(object sender, EventArgs e)
        {
            Bill currentBill = getCurrentBill();
            Order currentOrder = getCurrentOrder();

            if (OrderGridView.SelectedRows.Count > 0 && currentBill != null && currentOrder != null)
            {
                var selectedRow = OrderGridView.SelectedRows[0];

                using (var context = new MyDbConnection())
                {
                    Payment p = context.Payments.FirstOrDefault(paym => paym.order_id == currentOrder.id);

                    if (p != null)
                    {
                        p.net_purchase_price = stringToPrice(netPurchasePriceO.Text);
                        p.gross_product_price = stringToPrice(grossProductPriceO.Text);
                        p.gross_installation_price = stringToPrice(grossInstallationPriceO.Text);
                        p.material_price = stringToPrice(materialPriceO.Text);
                        p.total_gross_price = stringToPrice(totalGrossPriceO.Text);
                        if (!vatO.Text.IsNullOrEmpty())
                        {
                            p.var_rate = int.Parse(vatO.Text);
                        }
                        else
                        {
                            p.var_rate = 8;
                        }
                        p.var_rate = 8;
                    }

                    context.SaveChanges();

                    calculateBillOrders(currentBill.id);
                }

                //loadData();
                /*
                loadDataAsync();
                selectPaymentGridView(currentBill);
                selectPaymentCurrentRow();
                */
                loadFromOtherWindow(currentBill);
            }

        }


        private void selectPaymentGridView(Bill b)
        {
            PaymentsGridView.ClearSelection();

            for (int i = 0; i < filteredBills.Count(); i++)
            {
                if (filteredBills[i].id == b.id)
                {
                    PaymentsGridView.Rows[i].Selected = true;
                    PaymentsGridView.FirstDisplayedScrollingRowIndex = i;
                    break;
                }
            }
        }

        private void selectPaymentCurrentRow()
        {
            int currentBillIndex = getCurrentIndex();

            if (currentBillIndex >= 0 && currentBillIndex < PaymentsGridView.Rows.Count)
            {
                PaymentsGridView.FirstDisplayedScrollingRowIndex = currentBillIndex;
                PaymentsGridView_CellClick(PaymentsGridView, new DataGridViewCellEventArgs(0, currentBillIndex));
            }
        }

        private void calculateBillOrders(int billId)
        {
            double netPurchasePrice = 0;         // cena_zakupowa_netto
            double grossProductPrice = 0;        // cena_towaru_umowa_brutto
            double grossInstallationPrice = 0;   // cena_montazu_brutto
            double totalGrossPrice = 0;          // cena_calkowita_brutto
            double materialPrice = 0;            // cena_materialow


            using (var context = new MyDbConnection())
            {
                List<Payment> listToCalculate = context.Payments.Where(p => p.bill_id == billId).ToList();
                Bill billToAdd = context.Bills.Find(billId);

                if (billToAdd != null)
                {
                    foreach (Payment p in listToCalculate)
                    {
                        netPurchasePrice += p.net_purchase_price;
                        grossProductPrice += p.gross_product_price;
                        grossInstallationPrice += p.gross_installation_price;
                        totalGrossPrice += p.total_gross_price;
                        materialPrice += p.material_price;
                    }

                    billToAdd.net_purchase_price = netPurchasePrice;
                    billToAdd.gross_product_price = grossProductPrice;
                    billToAdd.gross_installation_price = grossInstallationPrice;
                    billToAdd.total_gross_price = totalGrossPrice;
                    billToAdd.material_price = materialPrice;

                    context.SaveChanges();
                }
            }
        }

        private void calculateBillAdvances(int billId)
        {
            double sumOfAdvances = 0;

            using (var context = new MyDbConnection())
            {
                List<Advance> listToCalculate = context.Advances.Where(a => a.bill_id == billId).ToList();
                Bill billToAdd = context.Bills.Find(billId);

                foreach (Advance a in listToCalculate)
                {
                    sumOfAdvances += a.amount;
                }

                billToAdd.sum_of_advances = sumOfAdvances;
                context.SaveChanges();

            }
        }

        private void fillReminingPrice_Click(object sender, EventArgs e)
        {
            double price = 0;

            if (!grossProductPriceO.Text.IsNullOrEmpty())
            {
                price += double.Parse(grossProductPriceO.Text);
            }

            if (!grossInstallationPriceO.Text.IsNullOrEmpty())
            {
                price += double.Parse(grossInstallationPriceO.Text);
            }

            totalGrossPriceO.Text = priceToString(price);
        }

        private Bill getCurrentBill()
        {
            if (PaymentsGridView.SelectedRows.Count > 0 && filteredBills.Count > 0)
            {
                var selectedId = PaymentsGridView.SelectedRows[0].Index;
                var selected = filteredBills[selectedId];
                return selected;
            }

            return null;
        }

        private Order getCurrentOrder()
        {
            if (getCurrentBill() != null)
            {
                if (OrderGridView.SelectedRows.Count > 0)
                {
                    var selectedRow = OrderGridView.SelectedRows[0].Index;
                    //var id = (int)selectedRow.Cells["PId"].Value;
                    //var selected = orders.FirstOrDefault(o => o.id == filteredPayments[selectedRow.Index].id);
                    var selected = orders.FirstOrDefault(o => o.id == filteredPayments[selectedRow].order_id);

                    return selected;
                }
            }
            return null;
        }

        private int getCurrentIndex()
        {
            if (PaymentsGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = PaymentsGridView.SelectedRows[0];
                return selectedRow.Index;
            }
            else
            {
                return -1;
            }
        }

        private void filterTable()
        {
            List<Bill> filtered = new List<Bill>(bills);


            if (!string.IsNullOrEmpty(searchText.Replace(" ", "")))
            {
                filtered = filtered.Where(b => b.id.ToString().ToLower().Contains(searchText) ||
                                payments.Any(p => p.bill_id == b.id &&
                                ((p.order?.client?.first_name ?? "").ToLower().Contains(searchText) ||
                                (p.order?.client?.last_name ?? "").ToLower().Contains(searchText))
                )).ToList();
            }

            switch (filterCompanyPayment)
            {
                case 0:
                    btn_premium.Text = "Wszystkie";
                    break;
                case 1:
                    filtered = filtered.Where(b => payments.FirstOrDefault(p => p.bill_id == b.id && p.order.company_number == 0) != null).ToList();
                    btn_premium.Text = "Tomkar";
                    break;
                case 2:
                    filtered = filtered.Where(b => payments.FirstOrDefault(p => p.bill_id == b.id && p.order.company_number == 1) != null).ToList();
                    btn_premium.Text = "Premium";
                    break;
            }

            filteredBills = filtered.OrderBy(b => b.id).ToList();

            if (filteredBills.Count > 0)
            {
                PaymentsGridView.RowCount = filteredBills.Count;
            }
            else
            {
                PaymentsGridView.RowCount = 0;
            }

            PaymentsGridView.Refresh();
            PaymentsGridView.Invalidate();

            setDataInOrder();

        }

        private void search_TextChanged(object sender, EventArgs e)
        {
            searchText = search.Text.ToLower();
            filterTable();
        }

        private void btn_premium_Click(object sender, EventArgs e)
        {

            filterCompanyPayment += 1;
            filterCompanyPayment = filterCompanyPayment % 3;

            filterTable();
        }

        private void OrderGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // TODO 
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < OrderGridView.Rows.Count)
                {
                    DataGridViewRow row = OrderGridView.Rows[e.RowIndex];

                    if (row != null && OrderGridView.Columns.Contains("PId") && row.Cells["PId"] != null && row.Cells["PId"].Value != null)
                    {
                        int id = Convert.ToInt32(row.Cells["PId"].Value);


                        using (var context = new MyDbConnection())
                        {
                            Order o = context.Orders.FirstOrDefault(ord => ord.id == id);

                            if (o != null && mainForm != null)
                            {
                                mainForm.openOrderFromPayment(o);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_product_Click(object sender, EventArgs e)
        {
            // ☐  ☑   ☒
            Order current = getCurrentOrder();
            if (current != null)
            {
                using (var context = new MyDbConnection())
                {
                    Order order = context.Orders.FirstOrDefault(o => o.id == current.id);
                    if (order.payment_products.HasValue)
                    {
                        order.payment_products = null;
                        btn_product.Text = "☐";
                    }
                    else
                    {
                        order.payment_products = DateTime.Now;
                        btn_product.Text = "☒";
                    }

                    context.SaveChanges();
                }
            }

        }

        private void btn_installation_Click(object sender, EventArgs e)
        {
            Order current = getCurrentOrder();
            if (current != null)
            {
                using (var context = new MyDbConnection())
                {
                    Order order = context.Orders.FirstOrDefault(o => o.id == current.id);
                    if (order.payment_installation.HasValue)
                    {
                        order.payment_installation = null;
                        btn_installation.Text = "☐";
                    }
                    else
                    {
                        order.payment_installation = DateTime.Now;
                        btn_installation.Text = "☒";
                    }

                    context.SaveChanges();
                }
            }
        }

        private void btn_debtors_Click(object sender, EventArgs e)
        {
            List<Bill> debtors = bills.Where(b => (b.total_gross_price - b.sum_of_advances) != 0).ToList();
            WorksheetManager.createDebtorsList(debtors, payments, orders);
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
                    gv.RowTemplate.MinimumHeight = 25;

                    gv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                    gv.GridColor = Color.LightGray;

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
            Bill currentBill = getCurrentBill();

            if (currentBill != null)
            {
                Payment currentPayment = payments.FirstOrDefault(p => p.bill_id == currentBill.id);

                if (currentPayment != null && currentPayment.order != null && currentPayment.order.client != null)
                {
                    Client currentClient = currentPayment.order.client;
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

                            DirectoryManager dm = new DirectoryManager(currentClient.id, currentClient.first_name, currentClient.last_name);

                            if (files != null && !files.IsNullOrEmpty())
                            {
                                foreach (string file in files)
                                {
                                    dm.copyFileToFolder(file);
                                }
                            }
                        }
                    }
                    updateListOfFiles(currentClient);

                }
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

        private void PaymentsGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Bill b = getCurrentBill();

            if (b != null)
            {
                if (PaymentsGridView.Rows[e.RowIndex].Selected)
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
                DataGridViewRow clickedRow = PaymentsGridView.Rows[clickedRowIndex];

                var id = filteredBills[clickedRow.Index].id;        // (int)clickedRow.Cells["IdOrder"].Value;
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
                    //Order order = context.Orders.Find(id);
                    Bill bill = context.Bills.Find(id);
                    if (bill != null)
                    {
                        bill.color_rgb = hexColor;
                        context.SaveChanges();
                        filteredBills[clickedRow.Index].color_rgb = hexColor;
                        if (hexColor.Equals("")) PaymentsGridView.Rows[clickedRowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                    }
                }

            }
        }

        private void PaymentsGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.RowIndex < filteredBills.Count)
            {
                Bill bill = filteredBills[e.RowIndex];

                if (bill != null && !string.IsNullOrEmpty(bill.color_rgb))
                {
                    try
                    {
                        PaymentsGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = ColorTranslator.FromHtml(bill.color_rgb);

                    }
                    catch (Exception ex)
                    {
                        // Obsługa ewentualnych wyjątków, np. nieprawidłowy format koloru
                        // MessageBox.Show($"Błąd formatu koloru dla zamówienia {recordId}: {ex.Message}");
                    }
                }
            }
        }

        private void PaymentsGridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo hit = PaymentsGridView.HitTest(e.X, e.Y);

                if (hit.Type == DataGridViewHitTestType.Cell)
                {
                    clickedRowIndex = hit.RowIndex;

                    PaymentsGridView.ClearSelection();
                    PaymentsGridView.Rows[clickedRowIndex].Selected = true;

                    contextMenu.Show(PaymentsGridView, e.Location);
                }
            }
        }

        private void btn_save_notes_Click(object sender, EventArgs e)
        {
            Bill b = getCurrentBill();

            if (b != null)
            {
                using (var context = new MyDbConnection())
                {
                    b.notes = notes.Text;
                    context.Bills.Find(b.id).notes = notes.Text; 
                    context.SaveChanges();
                }

                PaymentsGridView.Invalidate();
                PaymentsGridView.Refresh();
            }
        }
    }
}
