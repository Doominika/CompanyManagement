using Guna.UI2.WinForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsAppMySql.Classes;
using WindowsFormsAppMySql.Database;
using WindowsFormsAppMySql.Database.Entities;

namespace WindowsFormsAppMySql.Forms
{
    public partial class f_AddAdvance2 : Form
    {
        private double advance;
        private double sumOfAdvances;
        private double sumOfCurrentAdvances = 0;
        private int billId;

        private List<Order> orders;
        private List<Payment> payments;
        private List<OrdersAdvances> listOfAdvances = new List<OrdersAdvances>();

        public f_AddAdvance2()
        {
            InitializeComponent();
        }

        public f_AddAdvance2(double advance, List<Order> orders, List<Payment> payments, double sumOfAdvances, int billId)
        {
            this.advance = advance;
            this.orders = orders;
            this.payments = payments;
            this.sumOfAdvances = sumOfAdvances;
            this.billId = billId;
            InitializeComponent();

            OrdersGridView.RowTemplate.Height = 40;

            ApplyStyles(this);

            foreach (var order in orders)
            {
                if (order != null)
                {
                    OrdersAdvances oa = new OrdersAdvances();
                    var payment = payments.FirstOrDefault(p => p.order_id == order.id);
                    if (payment != null)
                    {
                        oa.orderId = order.id;

                        oa.pprice = payment.gross_product_price;
                        oa.psumOfAdvances = order.product_advance;
                        oa.pchangedAdvances = order.product_advance;

                        oa.iprice = payment.gross_installation_price;
                        oa.isumOfAdvances = order.installation_advance;
                        oa.ichangedAdvances = order.installation_advance;

                        if(oa != null) listOfAdvances.Add(oa);
                    }
                }
            }

            putDataToTable();
            calculateMainAdvance();

        }

        private void calculateMainAdvance()
        {
            double paidAdvances = 0;


            foreach(OrdersAdvances oa in listOfAdvances)
            {
                paidAdvances = paidAdvances + (oa.pchangedAdvances + oa.ichangedAdvances + oa.pnewAdvance + oa.inewAdvance);
            }

            //advanceValue.Text = priceToString(sumOfAdvances - paidAdvances + advance - sumOfCurrentAdvances);
            advanceValue.Text = priceToString(sumOfAdvances - paidAdvances + advance);

        }

        private void putDataToTable()
        {
            var list = orders.Select(o => new
            {
                Id = o.id,
                Product = o.product.name,
                Notes = o.notes,
                CostProduct = priceToString(payments.FirstOrDefault(p => p.order_id == o.id).gross_product_price),
                RestProduct = priceToString(listOfAdvances.FirstOrDefault(l => l.orderId == o.id).pprice - listOfAdvances.FirstOrDefault(l => l.orderId == o.id).pchangedAdvances - listOfAdvances.FirstOrDefault(l => l.orderId == o.id).pnewAdvance),       //priceToString(payments.FirstOrDefault(p => p.order_id == o.id).gross_product_price - o.product_advance),
                CostInstallation = priceToString(payments.FirstOrDefault(p => p.order_id == o.id).gross_installation_price),
                RestInstallation = priceToString(listOfAdvances.FirstOrDefault(l => l.orderId == o.id).iprice - listOfAdvances.FirstOrDefault(l => l.orderId == o.id).ichangedAdvances - listOfAdvances.FirstOrDefault(l => l.orderId == o.id).inewAdvance)      //priceToString(payments.FirstOrDefault(p => p.order_id == o.id).gross_installation_price - o.installation_advance)
            }).ToList();

            OrdersGridView.DataSource = list;
            OrdersGridView.Columns["Id"].Visible = false;


        }

        private void f_AddAdvance2_Load(object sender, EventArgs e)
        {
           
        }

        private void OrdersGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Order o = getCurrentOrder();

            if (o != null)
            {
                fillInfo(o);
            }
        }

        private void fillInfo(Order o)
        {
            costP.Clear();
            costI.Clear();
            sumP.Clear();
            sumI.Clear();
            restI.Clear();
            restP.Clear();

            OrdersAdvances oa = listOfAdvances.Where(l => l.orderId == o.id).FirstOrDefault();

            if (oa != null)
            {
                //int selectedIndex = OrdersGridView.SelectedRows[0].Index;
                //listOfAdvances[selectedIndex].pnewAdvance = stringToPrice(advanceP.Text);

                costP.Text = priceToString(payments.FirstOrDefault(p => p.order_id == o.id).gross_product_price);
                costI.Text = priceToString(payments.FirstOrDefault(p => p.order_id == o.id).gross_installation_price);
                sumP.Text = priceToString(oa.pchangedAdvances + oa.pnewAdvance);
                sumI.Text = priceToString(oa.ichangedAdvances + oa.inewAdvance);
                restP.Text = priceToString(payments.FirstOrDefault(p => p.order_id == o.id).gross_product_price - oa.pchangedAdvances - oa.pnewAdvance);
                restI.Text = priceToString(payments.FirstOrDefault(p => p.order_id == o.id).gross_installation_price - oa.ichangedAdvances - oa.inewAdvance);
                advanceP.Text = priceToString(oa.pnewAdvance);
                advanceI.Text = priceToString(oa.inewAdvance);
            }


            /*
            costP.Text = priceToString(payments.FirstOrDefault(p => p.order_id == o.id).gross_product_price);
            costI.Text = priceToString(payments.FirstOrDefault(p => p.order_id == o.id).gross_installation_price);
            sumP.Text = priceToString(o.product_advance);
            sumI.Text = priceToString(o.installation_advance);
            restP.Text = priceToString(payments.FirstOrDefault(p => p.order_id == o.id).gross_product_price - o.product_advance);
            restI.Text = priceToString(payments.FirstOrDefault(p => p.order_id == o.id).gross_installation_price - o.installation_advance);
            */
          
        }

        private Order getCurrentOrder()
        {
            if (OrdersGridView.SelectedRows.Count > 0)
            {
                var selectedRow = OrdersGridView.SelectedRows[0];
                var id = (int)selectedRow.Cells["Id"].Value;
                var selected = orders.FirstOrDefault(b => b.id == id);
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

        private void advanceP_KeyDown(object sender, KeyEventArgs e)
        {
            Order o = getCurrentOrder();
            if (e.KeyCode == Keys.Enter && o != null)
            {
                e.SuppressKeyPress = true;

                if (stringToPrice(advanceP.Text) <= stringToPrice(restP.Text))
                //if (stringToPrice(advanceP.Text) <= listOfAdvances.FirstOrDefault(p => p.orderId == o.id).pprice - )
                {

                    for(int i = 0; i<listOfAdvances.Count; i++)
                    {
                        if (listOfAdvances[i].orderId == o.id)
                        {
                            listOfAdvances[i].pnewAdvance = stringToPrice(advanceP.Text);
                            //listOfAdvances[i].pchangedAdvances = listOfAdvances[i].psumOfAdvances + stringToPrice(advanceP.Text);
                        }
                    }

                   // int index = getCurrentIndex();  
                    calculateMainAdvance();
                    putDataToTable();
                    selectPaymentGridView(o);
                    selectPaymentCurrentRow();
                }
                else
                {

                }
            }
        }

        private void advanceI_KeyDown(object sender, KeyEventArgs e)
        {
            Order o = getCurrentOrder();
            if (e.KeyCode == Keys.Enter && o != null)
            {
                e.SuppressKeyPress = true;

                if (stringToPrice(advanceI.Text) <= stringToPrice(restI.Text))
                {
                    for (int i = 0; i < listOfAdvances.Count; i++)
                    {
                        if (listOfAdvances[i].orderId == o.id)
                        {
                            listOfAdvances[i].inewAdvance = stringToPrice(advanceI.Text);
                            //listOfAdvances[i].ichangedAdvances = listOfAdvances[i].isumOfAdvances + stringToPrice(advanceI.Text);
                        }
                    }

                    calculateMainAdvance();
                    putDataToTable();
                    selectPaymentGridView(o);
                    selectPaymentCurrentRow();
                }
                else
                {

                }
            }
        }

        private void fillI_Click(object sender, EventArgs e)
        {
            Order o = getCurrentOrder();

            if (o != null)
            {
                for (int i = 0; i < listOfAdvances.Count; i++)
                {
                    if (listOfAdvances[i].orderId == o.id)
                    {
                        //if (listOfAdvances[i].iprice - listOfAdvances[i].ichangedAdvances <= stringToPrice(advanceValue.Text))
                        if (listOfAdvances[i].iprice - o.installation_advance <= stringToPrice(advanceValue.Text))
                        {
                            listOfAdvances[i].inewAdvance = listOfAdvances[i].iprice - o.installation_advance;     // kwota  - suma zaliczek
                            //listOfAdvances[i].ichangedAdvances = listOfAdvances[i].iprice;
                        }
                        else
                        {
                            listOfAdvances[i].inewAdvance = stringToPrice(advanceValue.Text);     
                            //listOfAdvances[i].ichangedAdvances = listOfAdvances[i].ichangedAdvances + stringToPrice(advanceValue.Text);
                        }
                    }
                }

                calculateMainAdvance();
                putDataToTable();
                selectPaymentGridView(o);
                selectPaymentCurrentRow();
                //fillInfo(getCurrentOrder());
            }
        }


        // TODO sprawdzic czy w ogole sie da uzupelnic i jesli nie, to po prostu dac co jest 
        private void fillP_Click(object sender, EventArgs e)
        {
            Order o = getCurrentOrder();

            if (o != null)
            {
                for (int i = 0; i < listOfAdvances.Count; i++)
                {
                    if (listOfAdvances[i].orderId == o.id)
                    {
                        //if (listOfAdvances[i].pprice - listOfAdvances[i].pchangedAdvances <= stringToPrice(advanceValue.Text))
                        if (listOfAdvances[i].pprice - listOfAdvances[i].pchangedAdvances <= stringToPrice(advanceValue.Text))
                        { 
                            listOfAdvances[i].pnewAdvance = listOfAdvances[i].pprice - listOfAdvances[i].pchangedAdvances;     // kwota  - suma zaliczek
                            //listOfAdvances[i].pchangedAdvances = listOfAdvances[i].pprice;
                        }
                        else
                        {
                            listOfAdvances[i].pnewAdvance = stringToPrice(advanceValue.Text);     // kwota  - suma zaliczek
                            //listOfAdvances[i].pchangedAdvances = listOfAdvances[i].pchangedAdvances + stringToPrice(advanceValue.Text);
                        }
                    }
                }

                calculateMainAdvance();
                putDataToTable();
                selectPaymentGridView(o);
                selectPaymentCurrentRow();
                //fillInfo(getCurrentOrder());
            }
        }

        private void sumP_KeyDown(object sender, KeyEventArgs e)
        {
            Order o = getCurrentOrder();
            if (e.KeyCode == Keys.Enter && o != null)
            {
                e.SuppressKeyPress = true;
                
                for (int i = 0; i < listOfAdvances.Count; i++)
                {
                    if (listOfAdvances[i].orderId == o.id)
                    {
                        double calc = stringToPrice(advanceValue.Text) + listOfAdvances[i].psumOfAdvances - stringToPrice(sumP.Text);

                        if (calc >= 0 && listOfAdvances[i].pprice - stringToPrice(sumP.Text) >= 0)
                        {
                            listOfAdvances[i].pchangedAdvances = stringToPrice(sumP.Text);
                            listOfAdvances[i].pnewAdvance = 0;
                        }
                        else
                        {

                        }
                    }
                }

                calculateMainAdvance();
                putDataToTable();
                selectPaymentGridView(o);
                selectPaymentCurrentRow();
                //fillInfo(getCurrentOrder());
            }
        }

        private void sumI_KeyDown(object sender, KeyEventArgs e)
        {
            Order o = getCurrentOrder();
            if (e.KeyCode == Keys.Enter && o != null)
            {
                e.SuppressKeyPress = true;

                for (int i = 0; i < listOfAdvances.Count; i++)
                {
                    if (listOfAdvances[i].orderId == o.id)
                    {
                        double calc = stringToPrice(advanceValue.Text) + listOfAdvances[i].isumOfAdvances - stringToPrice(sumI.Text);

                        if (calc >= 0 && listOfAdvances[i].iprice - stringToPrice(sumI.Text) >= 0)
                        {
                            listOfAdvances[i].ichangedAdvances = stringToPrice(sumI.Text);
                            listOfAdvances[i].inewAdvance = 0;
                        }
                        else
                        {

                        }
                    }
                }


                calculateMainAdvance();
                putDataToTable();
                selectPaymentGridView(o);
                selectPaymentCurrentRow();
            }
        }

        private void selectPaymentGridView(Order o)
        {
            foreach (DataGridViewRow row in OrdersGridView.Rows)
            {
                row.Selected = false;
            }

            foreach (DataGridViewRow row in OrdersGridView.Rows)
            {
                if (row.Cells["Id"].Value != null && (int)row.Cells["Id"].Value == o.id)
                {
                    row.Selected = true;
                    OrdersGridView.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
            }
        }

        private void selectPaymentCurrentRow()
        {
            int currentIndex = getCurrentIndex();

            if (currentIndex >= 0 && currentIndex < OrdersGridView.Rows.Count)
            {
                OrdersGridView.FirstDisplayedScrollingRowIndex = currentIndex;
                OrdersGridView_CellClick(OrdersGridView, new DataGridViewCellEventArgs(0, currentIndex));
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            if (stringToPrice(advanceValue.Text) == 0)
            {
                using (var context = new MyDbConnection())
                {
                    foreach (Order o in orders)
                    {
                        Order ord = context.Orders.Find(o.id);
                        if (ord != null)
                        {
                            double product = listOfAdvances.Find(p => p.orderId == o.id).pchangedAdvances + listOfAdvances.Find(p => p.orderId == o.id).pnewAdvance;
                            double installation = listOfAdvances.Find(p => p.orderId == o.id).ichangedAdvances + listOfAdvances.Find(p => p.orderId == o.id).inewAdvance;

                            ord.product_advance = product;
                            if(listOfAdvances.Find(p => p.orderId == o.id).pprice == product)
                            {
                                ord.payment_products = DateTime.Now;
                            }
                            else
                            {
                                ord.payment_products = null;
                            }

                            ord.installation_advance = installation;
                            if (listOfAdvances.Find(p => p.orderId == o.id).iprice == installation)
                            {
                                ord.payment_installation = DateTime.Now;
                            }
                            else
                            {
                                ord.payment_installation = null;
                            }

                        }
                    }

                    Advance ad = new Advance();
                    ad.date = DateTime.Now;
                    ad.bill_id = billId;
                    ad.amount = advance;

                    context.Advances.Add(ad);

                    context.SaveChanges();

                }

                this.Close();
            }
        }
    }

  

}
