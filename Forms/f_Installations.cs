using Castle.Core.Internal;
using Guna.UI2.WinForms;
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsAppMySql.Database.Entities;
using WindowsFormsAppMySql.Database;
using System.Globalization;
using WindowsFormsAppMySql.FileManaging;
using System.Data.Entity;
using System.Threading.Tasks;



namespace WindowsFormsAppMySql.Forms
{
    public partial class f_Installations : Form
    {
        public Database.Entities.Installation currentInstallation;
        public Database.Client currentInstallationClient;
        public int currentInstallationIndex;
        public int filterCompanyInstallation = 0;           //0 - wszystkie, 1 - tomkar, 2 - premium

        private int productTypeFilter = -1;
        private Product productFilter = null;

        private int deliveryDateFilter = 0;     // 0 - normalnnie, 1- descendong, 2 - ascending
        private int dateFilter = 0;             // 0 - normalnnie, 1- descendong, 2 - ascending
        private int deliveryDateFilter2 = 0;     // 0 - normalnnie, 1- descendong, 2 - ascending
        private int dateFilter2 = 0;             // 0 - normalnnie, 1- descendong, 2 - ascending


        List<Installation> installations1 = new List<Installation>();
        List<Installation> installations2 = new List<Installation>();
        List<Installation> filteredInstallations1 = new List<Installation>();
        List<Installation> filteredInstallations2 = new List<Installation>();
        List<Employee> employees = new List<Employee>();
        List<Product> products = new List<Product>();
        List<Order> orders = new List<Order>();
        List<Employee> installators = new List<Employee>();

        private int clickedRowIndex = -1;
        private int clickedRowIndex2 = -1;


        private f_OrderInfo f_OrderInfo;

        public f_Installations()
        {
            InitializeComponent();
            ApplyStyles(this);

            load();

            InstallationsGridView1.CellMouseEnter += new DataGridViewCellEventHandler(InstallationsGridView_CellMouseEnter);
            InstallationsGridView1.CellMouseLeave += new DataGridViewCellEventHandler(InstallationsGridView_CellMouseLeave);

            InstallationsGridView2.CellMouseEnter += new DataGridViewCellEventHandler(InstallationsGridView_CellMouseEnter2);
            InstallationsGridView2.CellMouseLeave += new DataGridViewCellEventHandler(InstallationsGridView_CellMouseLeave2);

            InstallationsGridView2.Columns["Instance2"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            InstallationsGridView2.Columns["SquaredMeters2"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            InstallationsGridView1.Columns["Instance"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            InstallationsGridView1.Columns["SquaredMeters"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            btn_create_employee_file.Location = new Point(panel5.Width / 2 - btn_create_employee_file.Width / 2, panel5.Height / 2 - btn_create_employee_file.Height / 2);
            btn_create_file.Location = new Point(panel33.Width / 2 - btn_create_file.Width / 2, panel33.Height / 2 - btn_create_file.Height / 2);


            back_panel1.Visible = false;
            back_panel2.Visible = false;

        }


        private void load()
        {
            btn_refresh.Image = Properties.Resources.refresh;
            btn_date_start_empl.Image = Properties.Resources.calendar;
            btn_date_end_empl.Image = Properties.Resources.calendar;
            btn_date_start.Image = Properties.Resources.calendar;
            btn_date_end.Image = Properties.Resources.calendar;
            btn_date1.Image = Properties.Resources.calendar;
            btn_date2.Image = Properties.Resources.calendar;

            btn_date_startrm_empl.Image = Properties.Resources.minus;
            btn_date_endrm_empl.Image = Properties.Resources.minus;
            btn_date_startrm.Image = Properties.Resources.minus;
            btn_date_endrm.Image = Properties.Resources.minus;
            btn_daterm1.Image = Properties.Resources.minus;
            btn_daterm2.Image = Properties.Resources.minus;

            cancel_back_panel1.Image = Properties.Resources.cancel;
            cancel_back_panel2.Image = Properties.Resources.cancel;

            loadDataAsync();
            inicializeFilters();

        }

        private void inicializeFilters()
        {

            // PRODUCT TYPE
            List<String> productsList = new List<String> { "Drzwi", "Okna", "Bramy", "Dodatki" };

            foreach (var pr in productsList)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Text = pr;
                checkBox.Font = new Font(checkBox.Font.FontFamily, 10);
                checkBox.CheckedChanged += new EventHandler(CheckBox_CheckedChanged1);
                productType1.Controls.Add(checkBox);
            }

            AdjustFlowLayoutPanelHeight(productType1);

            foreach (var pr in productsList)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Text = pr;
                checkBox.Font = new Font(checkBox.Font.FontFamily, 10);
                checkBox.CheckedChanged += new EventHandler(CheckBox_CheckedChanged2);
                productType2.Controls.Add(checkBox);
            }

            AdjustFlowLayoutPanelHeight(productType2);





            // PRODUCT PAYMENT

            CheckBox checkBoxPPaind1 = new CheckBox();
            checkBoxPPaind1.Text = "opłacone";
            checkBoxPPaind1.Font = new Font(checkBoxPPaind1.Font.FontFamily, 10);
            checkBoxPPaind1.CheckedChanged += new EventHandler(CheckBox_CheckedChanged1);
            productPayment1.Controls.Add(checkBoxPPaind1);

            CheckBox checkBoxPUnpaind1 = new CheckBox();
            checkBoxPUnpaind1.Text = "nieopłacone";
            checkBoxPUnpaind1.Font = new Font(checkBoxPUnpaind1.Font.FontFamily, 10);
            checkBoxPUnpaind1.CheckedChanged += new EventHandler(CheckBox_CheckedChanged1);
            productPayment1.Controls.Add(checkBoxPUnpaind1);

            AdjustFlowLayoutPanelHeight(productPayment1);


            CheckBox checkBoxPPaind2 = new CheckBox();
            checkBoxPPaind2.Text = "opłacone";
            checkBoxPPaind2.Font = new Font(checkBoxPPaind2.Font.FontFamily, 10);
            checkBoxPPaind2.CheckedChanged += new EventHandler(CheckBox_CheckedChanged2);
            productPayment2.Controls.Add(checkBoxPPaind2);

            CheckBox checkBoxPUnpaind2 = new CheckBox();
            checkBoxPUnpaind2.Text = "nieopłacone";
            checkBoxPUnpaind2.Font = new Font(checkBoxPUnpaind2.Font.FontFamily, 10);
            checkBoxPUnpaind2.CheckedChanged += new EventHandler(CheckBox_CheckedChanged2);
            productPayment2.Controls.Add(checkBoxPUnpaind2);

            AdjustFlowLayoutPanelHeight(productPayment2);




            // INSTALLATION PAYMENT

            CheckBox checkBoxIPaind1 = new CheckBox();
            checkBoxIPaind1.Text = "opłacone";
            checkBoxIPaind1.Font = new Font(checkBoxIPaind1.Font.FontFamily, 10);
            checkBoxIPaind1.CheckedChanged += new EventHandler(CheckBox_CheckedChanged1);
            installationPayment1.Controls.Add(checkBoxIPaind1);

            CheckBox checkBoxIUnpaind1 = new CheckBox();
            checkBoxIUnpaind1.Text = "nieopłacone";
            checkBoxIUnpaind1.Font = new Font(checkBoxIUnpaind1.Font.FontFamily, 10);
            checkBoxIUnpaind1.CheckedChanged += new EventHandler(CheckBox_CheckedChanged1);
            installationPayment1.Controls.Add(checkBoxIUnpaind1);

            AdjustFlowLayoutPanelHeight(installationPayment1);



            CheckBox checkBoxIPaind2 = new CheckBox();
            checkBoxIPaind2.Text = "opłacone";
            checkBoxIPaind2.Font = new Font(checkBoxIPaind2.Font.FontFamily, 10);
            checkBoxIPaind2.CheckedChanged += new EventHandler(CheckBox_CheckedChanged2);
            installationPayment2.Controls.Add(checkBoxIPaind2);

            CheckBox checkBoxIUnpaind2 = new CheckBox();
            checkBoxIUnpaind2.Text = "nieopłacone";
            checkBoxIUnpaind2.Font = new Font(checkBoxIUnpaind2.Font.FontFamily, 10);
            checkBoxIUnpaind2.CheckedChanged += new EventHandler(CheckBox_CheckedChanged2);
            installationPayment2.Controls.Add(checkBoxIUnpaind2);

            AdjustFlowLayoutPanelHeight(installationPayment2);



            //EMPLOYEES

            foreach (var e in installators)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Text = e.first_name;
                checkBox.Font = new Font(checkBox.Font.FontFamily, 10);
                // checkBox.CheckedChanged += new EventHandler(CheckBox_CheckedChange_Employees1);
                employees1.Controls.Add(checkBox);
            }

            AdjustFlowLayoutPanelHeight(employees1);

            foreach (var e in installators)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Text = e.first_name;
                checkBox.Font = new Font(checkBox.Font.FontFamily, 10);
                //checkBox.CheckedChanged += new EventHandler(CheckBox_CheckedChanged_Employees2);
                employees2.Controls.Add(checkBox);
            }

            AdjustFlowLayoutPanelHeight(employees2);

        }



        private void CheckBox_CheckedChanged1(object sender, EventArgs e)
        {
            List<Installation> listToFilter = new List<Installation>();
            bool isAnyChecked = false;
            int isBothCheckedProduct = 0;


            //PRODUCT TYPE

            CheckBox door = productType1.Controls[0] as CheckBox;
            CheckBox window = productType1.Controls[1] as CheckBox;
            CheckBox gate = productType1.Controls[2] as CheckBox;
            CheckBox other = productType1.Controls[3] as CheckBox;

            if (door.Checked && window.Checked && gate.Checked && other.Checked)
            {
                listToFilter = new List<Installation>(installations1);
            }
            else if (!door.Checked && !window.Checked && !gate.Checked && !other.Checked)
            {
                listToFilter = new List<Installation>(installations1);
            }
            else
            {
                if (door.Checked)
                {
                    listToFilter.AddRange(installations1.Where(o => o.order.product.id == products[0].id).ToList());
                }

                if (window.Checked)
                {
                    listToFilter.AddRange(installations1.Where(o => o.order.product.id == products[1].id).ToList());

                }

                if (gate.Checked)
                {
                    listToFilter.AddRange(installations1.Where(o => o.order.product.id == products[2].id).ToList());
                }


                if (other.Checked)
                {
                    for (int i = 3; i < products.Count; i++)
                    {
                        listToFilter.AddRange(installations1.Where(o => o.order.product.id == products[i].id).ToList());
                    }
                }
            }




            //PRODUCT PAYMENT

            CheckBox productPaid = productPayment1.Controls[0] as CheckBox;
            CheckBox productUnpaid = productPayment1.Controls[1] as CheckBox;

            if (productPaid.Checked && productUnpaid.Checked)
            {

            }
            else if (productPaid.Checked)
            {
                listToFilter = listToFilter.Where(o => o.order.payment_products != null).ToList();
            }
            else if (productUnpaid.Checked)
            {
                listToFilter = listToFilter.Where(o => o.order.payment_products == null).ToList();
            }



            //INSTALLATION PAYMENT

            CheckBox installationtPaid = installationPayment1.Controls[0] as CheckBox;
            CheckBox InstallationUnpaid = installationPayment1.Controls[1] as CheckBox;

            if (installationtPaid.Checked && InstallationUnpaid.Checked)
            {

            }
            else if (installationtPaid.Checked)
            {
                listToFilter = listToFilter.Where(o => o.order.payment_installation != null).ToList();
            }
            else if (InstallationUnpaid.Checked)
            {
                listToFilter = listToFilter.Where(o => o.order.payment_installation == null).ToList();
            }


            // listTofilter1 = listToFilter;
            // putDataToTableInstallations(listToFilter, "Id");
            //filteredInstallations1 = listToFilter;

            filteredInstallations1 = listToFilter.OrderBy(x => x.id).ToList();

            if (filteredInstallations1.Count > 0)
            {
                InstallationsGridView1.RowCount = filteredInstallations1.Count;
            }
            else
            {
                InstallationsGridView1.RowCount = 0;
            }

            InstallationsGridView1.Refresh();
            InstallationsGridView1.Invalidate();

            setDataInOrder1();
        }


        private void CheckBox_CheckedChanged2(object sender, EventArgs e)
        {
            List<Installation> listToFilter = new List<Installation>();
            filteredInstallations2 = new List<Installation>(installations2);
            bool isAnyChecked = false;
            int isBothCheckedProduct = 0;


            //PRODUCT TYPE

            CheckBox door = productType2.Controls[0] as CheckBox;
            CheckBox window = productType2.Controls[1] as CheckBox;
            CheckBox gate = productType2.Controls[2] as CheckBox;
            CheckBox other = productType2.Controls[3] as CheckBox;

            if (door.Checked && window.Checked && gate.Checked && other.Checked)
            {
                listToFilter = new List<Installation>(filteredInstallations2);
            }
            else if (!door.Checked && !window.Checked && !gate.Checked && !other.Checked)
            {
                listToFilter = new List<Installation>(filteredInstallations2);
            }
            else
            {
                if (door.Checked)
                {
                    listToFilter.AddRange(filteredInstallations2.Where(o => o.order.product.id == products[0].id).ToList());
                }

                if (window.Checked)
                {
                    listToFilter.AddRange(filteredInstallations2.Where(o => o.order.product.id == products[1].id).ToList());

                }

                if (gate.Checked)
                {
                    listToFilter.AddRange(filteredInstallations2.Where(o => o.order.product.id == products[2].id).ToList());
                }


                if (other.Checked)
                {
                    for (int i = 3; i < products.Count; i++)
                    {
                        listToFilter.AddRange(filteredInstallations2.Where(o => o.order.product.id == products[i].id).ToList());
                    }
                }
            }




            //PRODUCT PAYMENT

            CheckBox productPaid = productPayment2.Controls[0] as CheckBox;
            CheckBox productUnpaid = productPayment2.Controls[1] as CheckBox;

            if (productPaid.Checked && productUnpaid.Checked)
            {

            }
            else if (productPaid.Checked)
            {
                listToFilter = listToFilter.Where(o => o.order.payment_products != null).ToList();
            }
            else if (productUnpaid.Checked)
            {
                listToFilter = listToFilter.Where(o => o.order.payment_products == null).ToList();
            }



            //INSTALLATION PAYMENT

            CheckBox installationtPaid = installationPayment2.Controls[0] as CheckBox;
            CheckBox InstallationUnpaid = installationPayment2.Controls[1] as CheckBox;

            if (installationtPaid.Checked && InstallationUnpaid.Checked)
            {

            }
            else if (installationtPaid.Checked)
            {
                listToFilter = listToFilter.Where(o => o.order.payment_installation != null).ToList();
            }
            else if (InstallationUnpaid.Checked)
            {
                listToFilter = listToFilter.Where(o => o.order.payment_installation == null).ToList();
            }




            filteredInstallations2 = listToFilter.OrderBy(x => x.id).ToList();

            if (filteredInstallations2.Count > 0)
            {
                InstallationsGridView2.RowCount = filteredInstallations2.Count;
            }
            else
            {
                InstallationsGridView2.RowCount = 0;
            }

            InstallationsGridView2.Refresh();
            InstallationsGridView2.Invalidate();

            setDataInOrder2();
        }



        private void AdjustFlowLayoutPanelHeight(FlowLayoutPanel panel)
        {
            int totalHeight = 0;

            foreach (Control ctrl in panel.Controls)
            {
                totalHeight += ctrl.Height + ctrl.Margin.Top + ctrl.Margin.Bottom;
            }

            panel.Height = totalHeight;
        }



        private async Task loadDataAsync()
        {
            clearInfo1();
            clearInfo2();

            using (var context = new MyDbConnection())
            {
                try
                {
                    orders = await context.Orders.ToListAsync();
                    employees = await context.Employees.ToListAsync();
                    products = await context.Products.Where(p => p.visible == 1).ToListAsync();

                    foreach (Order order in orders)
                    {
                        if (order.is_checked_for_installation == 0 && isBeforeToday(order.delivery_date))
                        {
                            Installation newInstallation = new Installation
                            {
                                order_id = order.id,
                                client_id = order.client_id
                            };

                            context.Installations.Add(newInstallation);
                            order.is_checked_for_installation = 1;
                        }
                    }

                    await context.SaveChangesAsync(); // Asynchroniczny zapis zmian

                    installators = employees.Where(e => e.role == 1).ToList();

                    installations1 = await context.Installations
                                                   .Where(i => i.date != null && i.order.is_with_installation == 1)
                                                   .Include(i => i.client)
                                                   .Include(i => i.order)
                                                   .Include(i => i.Employees)
                                                   .OrderByDescending(o => o.id)
                                                   .ToListAsync();

                    installations2 = await context.Installations
                                                   .Where(i => i.date == null && i.order.is_with_installation == 1)
                                                   .Include(i => i.client)
                                                   .Include(i => i.order)
                                                   .Include(i => i.Employees)
                                                   .OrderByDescending(o => o.id)
                                                   .ToListAsync();

                    filteredInstallations1 = installations1.OrderBy(x => x.id).ToList();
                    filteredInstallations2 = installations2.OrderBy(x => x.id).ToList();

                    if (filteredInstallations1.Count <= 0)
                        InstallationsGridView1.RowCount = 0;
                    else
                        InstallationsGridView1.RowCount = filteredInstallations1.Count;
                    InstallationsGridView1.VirtualMode = true;

                    if (filteredInstallations2.Count <= 0)
                        InstallationsGridView2.RowCount = 0;
                    else
                        InstallationsGridView2.RowCount = filteredInstallations2.Count;
                    InstallationsGridView2.VirtualMode = true;

                    setDataInOrder1();
                    setDataInOrder2();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }




            /*
            clearInfo1();
            clearInfo2();

            using (var context = new MyDbConnection())
            {
                try
                {
                    // Asynchroniczne pobieranie danych
                    orders =  context.Orders.ToList();
                    employees =  context.Employees.ToList();
                    products =  context.Products.Where(p => p.visible == 1).ToList();

                    foreach (Order order in orders)
                    {
                        if (order.is_checked_for_installation == 0 && isBeforeToday(order.delivery_date))
                        {
                            Installation newInstallation = new Installation
                            {
                                order_id = order.id,
                                client_id = order.client_id
                            };

                            context.Installations.Add(newInstallation);
                            order.is_checked_for_installation = 1;
                        }
                    }

                    context.SaveChanges();

                    installators = employees.Where(e => e.role == 1).ToList();

                    installations1 = await context.Installations
                                                  .Where(i => i.date != null && i.order.is_with_installation == 1)
                                                  .Include(i => i.client)
                                                  .Include(i => i.order)
                                                  .Include(i => i.Employees)
                                                  .OrderByDescending(o => o.id)
                                                  .ToListAsync();
                    installations2 = await context.Installations
                                                  .Where(i => i.date == null && i.order.is_with_installation == 1)
                                                  .Include(i => i.client)
                                                  .Include(i => i.order)
                                                  .Include(i => i.Employees)
                                                  .OrderByDescending(o => o.id)
                                                  .ToListAsync();

                    filteredInstallations1 = installations1.OrderBy(x => x.id).ToList();
                    filteredInstallations2 = installations2.OrderBy(x => x.id).ToList();


                    if (filteredInstallations1.Count <= 0) InstallationsGridView1.RowCount = 0;
                    else InstallationsGridView1.RowCount = filteredInstallations1.Count;
                    InstallationsGridView1.VirtualMode = true;

                    if (filteredInstallations2.Count <= 0) InstallationsGridView2.RowCount = 0;
                    else InstallationsGridView2.RowCount = filteredInstallations2.Count;
                    InstallationsGridView2.VirtualMode = true;


                    setDataInOrder1();
                    setDataInOrder2();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            */




        }


        private void setDataInOrder1()
        {
            int lastRowIndex = InstallationsGridView1.Rows.Count - 1;
            InstallationsGridView1.ClearSelection();
            InstallationsGridView1.Rows[lastRowIndex].Selected = true;
            InstallationsGridView1.FirstDisplayedScrollingRowIndex = lastRowIndex;
        }

        private void setDataInOrder2()
        {
            int lastRowIndex = InstallationsGridView2.Rows.Count - 1;
            InstallationsGridView2.ClearSelection();
            InstallationsGridView2.Rows[lastRowIndex].Selected = true;
            InstallationsGridView2.FirstDisplayedScrollingRowIndex = lastRowIndex;
        }

        private void clearInfo1()
        {
            installationDate1.Text = "";
            installationAddress1.Text = "";
            installationNotes1.Text = "";

            foreach (Control control in employees1.Controls)
            {
                if (control is CheckBox checkBox)
                {
                    checkBox.Checked = false;
                }
            }
        }

        private void clearInfo2()
        {
            installationDate2.Text = "";
            installationAddress2.Text = "";
            installationNotes2.Text = "";

            foreach (Control control in employees2.Controls)
            {
                if (control is CheckBox checkBox)
                {
                    checkBox.Checked = false;
                }
            }
        }

        private bool isBeforeToday(DateTime? time)
        {
            if (time.HasValue)
            {
                if (time < DateTime.Now)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
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

                    gv.ColumnHeadersHeight = 35;
                    gv.RowTemplate.MinimumHeight = 30;

                    //FF6464
                    gv.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(141, 156, 99);
                    gv.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                    gv.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(141, 156, 99);
                    gv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;                              //dodac
                    gv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;


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
                    gv.GridColor = Color.DarkGray;

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

        private void InstallationsGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = InstallationsGridView1.Rows[e.RowIndex];
                DataGridViewColumn column = InstallationsGridView1.Columns[e.ColumnIndex];

                if (e.ColumnIndex == InstallationsGridView1.Columns.Count - 1)
                {
                    var id = row.Index;
                    Installation i = filteredInstallations1[id];


                    if (i != null && i.order != null)
                    {
                        if (f_OrderInfo != null)
                        {
                            f_OrderInfo.Close();
                            f_OrderInfo.Dispose();
                            f_OrderInfo = null;
                        }

                        f_OrderInfo = new f_OrderInfo(i.order);
                        f_OrderInfo.StartPosition = FormStartPosition.Manual;
                        f_OrderInfo.Location = new Point(23, 23);
                        f_OrderInfo.Show();
                    }
                }
            }
        }

        private void InstallationsGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (f_OrderInfo != null)
            {
                f_OrderInfo.Close();
                f_OrderInfo.Dispose(); // Zwolnienie zasobów
                f_OrderInfo = null;
            }
        }

        private void InstallationsGridView_CellMouseEnter2(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = InstallationsGridView2.Rows[e.RowIndex];
                DataGridViewColumn column = InstallationsGridView2.Columns[e.ColumnIndex];

                if (e.ColumnIndex == InstallationsGridView2.Columns.Count - 1)
                {
                    var id = row.Index;
                    Installation i = filteredInstallations2[id];


                    if (i != null && i.order != null)
                    {
                        if (f_OrderInfo != null)
                        {
                            f_OrderInfo.Close();
                            f_OrderInfo.Dispose();
                            f_OrderInfo = null;
                        }

                        f_OrderInfo = new f_OrderInfo(i.order);
                        f_OrderInfo.StartPosition = FormStartPosition.Manual;
                        f_OrderInfo.Location = new Point(23, 23);
                        f_OrderInfo.Show();
                    }
                }
            }
        }

        private void InstallationsGridView_CellMouseLeave2(object sender, DataGridViewCellEventArgs e)
        {
            if (f_OrderInfo != null)
            {
                f_OrderInfo.Close();
                f_OrderInfo.Dispose(); // Zwolnienie zasobów
                f_OrderInfo = null;
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

        private void InstallationsGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            back_panel2.Visible = false;
            InstallationsGridView2.ClearSelection();


            if (getCurrentInstallation() != null)
            {
                back_panel1.Visible = true;
                filter_panel1.Visible = false;
                info_panel1.Visible = true;
                btn_folder1.Visible = true;
                btn_copy1.Visible = true;

                fillInfo();
            }
        }


        private Installation getCurrentInstallation()
        {
            if (InstallationsGridView1.SelectedRows.Count > 0 && filteredInstallations1.Count > 0)
            {
                var selectedRow = InstallationsGridView1.SelectedRows[0].Index;
                var selected = filteredInstallations1[selectedRow];
                return selected;
            }
            return null;
        }


        private Installation getCurrentInstallation2()
        {
            if (InstallationsGridView2.SelectedRows.Count > 0 && filteredInstallations2.Count > 0)
            {
                var selectedRow = InstallationsGridView2.SelectedRows[0].Index;
                var selected = filteredInstallations2[selectedRow];
                return selected;
            }
            return null;
        }

        private void fillInfo()
        {
            Installation installation = getCurrentInstallation();

            if (installation != null)
            {
                using (var context = new MyDbConnection())
                {
                    if (installation.installation_address == null || installation.installation_address.IsNullOrEmpty())
                    {
                        Installation inst = context.Installations.Find(installation.id);
                        inst.installation_address = installation.order.installation_address;
                        context.SaveChanges();

                        installationAddress1.Text = installation.order.installation_address;
                    }
                    else
                    {
                        installationAddress1.Text = installation.installation_address;
                    }
                }

                installationDate1.Text = dateTimeToString(installation.date);
                installationNotes1.Text = installation.notes;


                foreach (Control control in employees1.Controls)
                {
                    if (control is CheckBox checkBox)
                    {
                        checkBox.Checked = false;
                    }
                }

                for (int i = 0; i < installators.Count; i++)
                {
                    if (installation.Employees.FirstOrDefault(em => em.id == installators[i].id) != null)
                    {
                        CheckBox cb = employees1.Controls[i] as CheckBox;
                        cb.Checked = true;
                    }
                }

            }
        }

        private void fillInfo2()
        {
            Installation installation = getCurrentInstallation2();

            if (installation != null)
            {

                using (var context = new MyDbConnection())
                {
                    if (installation.installation_address == null || installation.installation_address.IsNullOrEmpty())
                    {
                        Installation inst = context.Installations.Find(installation.id);
                        inst.installation_address = installation.order.installation_address;
                        context.SaveChanges();

                        installationAddress2.Text = installation.order.installation_address;
                    }
                    else
                    {
                        installationAddress2.Text = installation.installation_address;
                    }
                }

                installationDate2.Text = dateTimeToString(installation.date);
                installationNotes2.Text = installation.notes;

                foreach (Control control in employees2.Controls)
                {
                    if (control is CheckBox checkBox)
                    {
                        checkBox.Checked = false;
                    }
                }

                for (int i = 0; i < installators.Count; i++)
                {
                    if (installation.Employees.FirstOrDefault(em => em.id == installators[i].id) != null)
                    {
                        CheckBox cb = employees2.Controls[i] as CheckBox;
                        cb.Checked = true;
                    }
                }

            }
        }

        private void InstallationsGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            back_panel1.Visible = false;
            InstallationsGridView1.ClearSelection();

            if ((getCurrentInstallation2() != null))
            {
                back_panel2.Visible = true;
                filter_panel2.Visible = false;
                info_panel2.Visible = true;
                btn_folder2.Visible = true;

                fillInfo2();
            }
        }


        private void btn_date2_Click(object sender, EventArgs e)
        {
            Guna2Button btn = sender as Guna2Button;
            dt_date2.Location = new Point(btn.Location.X, btn.Location.Y + btn.Height);
            dt_date2.Visible = true;
            dt_date2.Focus();
            SendKeys.Send("{F4}");
        }

        private void btn_daterm2_Click(object sender, EventArgs e)
        {
            installationDate2.Text = "";
            dt_date2.Value = DateTime.Now;
        }

        private void dt_date2_CloseUp(object sender, EventArgs e)
        {
            MetroDateTime dt = sender as MetroDateTime;
            DateTime selectedDate = dt.Value;
            installationDate2.Text = dateTimeToString(selectedDate);
        }

        private void btn_date1_Click(object sender, EventArgs e)
        {
            Guna2Button btn = sender as Guna2Button;
            dt_date1.Location = new Point(btn.Location.X, btn.Location.Y + btn.Height);
            dt_date1.Visible = true;
            dt_date1.Focus();
            SendKeys.Send("{F4}");
        }

        private void btn_daterm1_Click(object sender, EventArgs e)
        {
            installationDate1.Text = "";
            dt_date1.Value = DateTime.Now;
        }

        private void dt_date1_CloseUp(object sender, EventArgs e)
        {
            MetroDateTime dt = sender as MetroDateTime;
            DateTime selectedDate = dt.Value;
            installationDate2.Text = dateTimeToString(selectedDate);
        }

        private void btn_save1_Click(object sender, EventArgs e)
        {
            Installation i = getCurrentInstallation();

            if (i != null)
            {
                using (var context = new MyDbConnection())
                {
                    i = context.Installations.FirstOrDefault(inst => inst.id == i.id);

                    if (i == null)
                    {
                        MessageBox.Show("Instalacja nie została znaleziona.");
                        return;
                    }


                    i.date = stringToDateTime(installationDate1.Text);
                    i.installation_address = installationAddress1.Text;
                    i.order.installation_address = installationAddress1.Text;
                    i.notes = installationNotes1.Text;


                    List<Employee> selectedEmployees = new List<Employee>();
                    List<Employee> employeesToRemove = new List<Employee>();
                    List<Employee> employeesToAdd = new List<Employee>();


                    foreach (Control control in employees1.Controls)
                    {
                        if (control is CheckBox checkBox && checkBox.Checked)
                        {
                            int index = employees1.Controls.IndexOf(control);
                            if (index >= 0 && index < installators.Count)
                            {
                                selectedEmployees.Add(installators[index]);
                            }
                        }
                    }


                    foreach (Employee employee in i.Employees)
                    {
                        if (selectedEmployees.FirstOrDefault(em => em.id == employee.id) == null)
                        {
                            employeesToRemove.Add(employee);
                        }
                    }

                    foreach (Employee employee in selectedEmployees)
                    {
                        if (i.Employees.ToList().FirstOrDefault(em => em.id == employee.id) == null)
                        {
                            employeesToAdd.Add(employee);
                        }
                    }

                    // Usuń pracowników, którzy już nie są zaznaczeni
                    foreach (var emp in employeesToRemove)
                    {
                        var empFromDb = context.Employees.Find(emp.id);
                        if (empFromDb != null)
                        {
                            i.Employees.Remove(empFromDb);
                        }
                    }

                    foreach (var emp in employeesToAdd)
                    {
                        var empFromDb = context.Employees.Find(emp.id);
                        if (empFromDb != null)
                        {
                            i.Employees.Add(empFromDb);
                        }
                    }

                    context.SaveChanges();

                    //loadDataInstallations();
                    CheckBox_CheckedChanged1(this, new EventArgs());
                    back_panel1.Visible = false;

                    //InstallationsGridView1.Invalidate();
                    //InstallationsGridView1.Refresh();
                    loadDataAsync();

                }
            }
        }


        private void btn_save2_Click(object sender, EventArgs e)
        {
            Installation i = getCurrentInstallation2();

            if (i != null)
            {
                using (var context = new MyDbConnection())
                {
                    i = context.Installations.FirstOrDefault(inst => inst.id == i.id);

                    if (i == null)
                    {
                        MessageBox.Show("Instalacja nie została znaleziona.");
                        return;
                    }


                    i.date = stringToDateTime(installationDate2.Text);
                    i.installation_address = installationAddress2.Text;
                    i.notes = installationNotes2.Text;


                    List<Employee> selectedEmployees = new List<Employee>();
                    List<Employee> employeesToRemove = new List<Employee>();
                    List<Employee> employeesToAdd = new List<Employee>();


                    foreach (Control control in employees2.Controls)
                    {
                        if (control is CheckBox checkBox && checkBox.Checked)
                        {
                            int index = employees2.Controls.IndexOf(control);
                            if (index >= 0 && index < installators.Count)
                            {
                                selectedEmployees.Add(installators[index]);
                            }
                        }
                    }


                    foreach (Employee employee in i.Employees)
                    {
                        if (selectedEmployees.FirstOrDefault(em => em.id == employee.id) == null)
                        {
                            employeesToRemove.Add(employee);
                        }
                    }

                    foreach (Employee employee in selectedEmployees)
                    {
                        if (i.Employees.ToList().FirstOrDefault(em => em.id == employee.id) == null)
                        {
                            employeesToAdd.Add(employee);
                        }
                    }

                    // Usuń pracowników, którzy już nie są zaznaczeni
                    foreach (var emp in employeesToRemove)
                    {
                        var empFromDb = context.Employees.Find(emp.id);
                        if (empFromDb != null)
                        {
                            i.Employees.Remove(empFromDb);
                        }
                    }

                    foreach (var emp in employeesToAdd)
                    {
                        var empFromDb = context.Employees.Find(emp.id);
                        if (empFromDb != null)
                        {
                            i.Employees.Add(empFromDb);
                        }
                    }

                    context.SaveChanges();

                    //loadDataInstallations();
                    CheckBox_CheckedChanged2(this, new EventArgs());

                    back_panel2.Visible = false;

                    //InstallationsGridView2.Invalidate();
                    //InstallationsGridView2.Refresh();

                    loadDataAsync();
                }
            }
        }


        private void InstallationsGridView2_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                /*
                DataGridViewRow row = InstallationsGridView2.Rows[e.RowIndex];
                DataGridViewColumn column = InstallationsGridView2.Columns[e.ColumnIndex];

                if (e.ColumnIndex == InstallationsGridView2.ColumnCount - 2)
                {
                   int id = Convert.ToInt32(row.Cells["Id2"].Value);
                    Installation i = installations2.FirstOrDefault(o => o.id == id);

                    if (i != null && i.order != null)
                    {
                        if (f_OrderInfo != null)
                        {
                            f_OrderInfo.Close();
                            f_OrderInfo.Dispose();
                            f_OrderInfo = null;
                        }

                        f_OrderInfo = new f_OrderInfo(i.order);
                        f_OrderInfo.StartPosition = FormStartPosition.Manual;
                        f_OrderInfo.Location = new Point(23, 23);
                        f_OrderInfo.Show();
                    }
                }
                */
            }
        }

        private void InstallationsGridView2_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (f_OrderInfo != null)
            {
                f_OrderInfo.Close();
                f_OrderInfo.Dispose(); // Zwolnienie zasobów
                f_OrderInfo = null;
            }
        }

        private void btn_filter1_Click(object sender, EventArgs e)
        {
            back_panel1.Visible = true;
            filter_panel1.Visible = true;
            info_panel1.Visible = false;
            btn_folder1.Visible = false;
            btn_copy1.Visible = false;
        }

        private void btn_filter2_Click(object sender, EventArgs e)
        {
            back_panel2.Visible = true;
            filter_panel2.Visible = true;
            info_panel2.Visible = false;
            btn_folder2.Visible = false;
            btn_copy2.Visible = false;
        }

        private void cancel_back_panel1_Click(object sender, EventArgs e)
        {
            back_panel1.Visible = false;
            InstallationsGridView1.ClearSelection();
        }

        private void cancel_back_panel2_Click(object sender, EventArgs e)
        {
            back_panel2.Visible = false;
            InstallationsGridView2.ClearSelection();
        }

        private void btn_folder1_Click(object sender, EventArgs e)
        {
            Installation i = getCurrentInstallation();
            if (i != null)
            {
                DirectoryManager dm = new DirectoryManager(i.client_id, i.client.first_name, i.client.last_name);
                dm.openDirectory();
            }
        }

        private void btn_folder2_Click(object sender, EventArgs e)
        {
            Installation i = getCurrentInstallation2();
            if (i != null)
            {
                DirectoryManager dm = new DirectoryManager(i.client_id, i.client.first_name, i.client.last_name);
                dm.openDirectory();
            }
        }

        private void btn_create_file_Click(object sender, EventArgs e)
        {
            //loadDataInstallations();

            DateTime? start = stringToDateTime(date_start.Text);
            DateTime? end = stringToDateTime(date_end.Text);

            List<Installation> listToFile = new List<Installation>();

            if (start != null && end != null)
            {
                listToFile = installations1.Where(i => i.date?.Date >= start?.Date && i.date?.Date <= end?.Date).ToList();
            }
            else if (start != null)
            {
                listToFile = installations1.Where(i => i.date?.Date >= start?.Date).ToList();
            }
            else if (end != null)
            {
                listToFile = installations1.Where(i => i.date?.Date >= DateTime.Now.Date && i.date?.Date <= end?.Date).ToList();
            }
            else
            {
                listToFile = installations1.Where(i => i.date?.Date >= DateTime.Now.Date).ToList();
            }


            WorksheetManager.createInstallationsFile(listToFile);

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            WorksheetManager.createInstallationsEmployeesFile(installations1, installators);
        }

        private void InstallationsGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string sortByValue = InstallationsGridView1.Columns[e.ColumnIndex].DataPropertyName;

            if (sortByValue != null && sortByValue.Equals("DeliveryDate"))
            {
                deliveryDateFilter += 1;
                deliveryDateFilter %= 3;

                if (deliveryDateFilter == 0) filteredInstallations1 = filteredInstallations1.OrderBy(i => i.id).ToList();
                else if (deliveryDateFilter == 1) filteredInstallations1 = filteredInstallations1.OrderBy(i => i.order.delivery_date).ToList();
                else if (deliveryDateFilter == 2) filteredInstallations1 = filteredInstallations1.OrderByDescending(i => i.order.delivery_date).ToList();

            }
            else if (sortByValue != null && sortByValue.Equals("Date"))
            {
                dateFilter += 1;
                dateFilter %= 3;

                if (dateFilter == 0) filteredInstallations1 = filteredInstallations1.OrderBy(i => i.id).ToList();
                else if (dateFilter == 1) filteredInstallations1 = filteredInstallations1.OrderBy(i => i.date).ToList();
                else if (dateFilter == 2) filteredInstallations1 = filteredInstallations1.OrderByDescending(i => i.date).ToList();

            }

            InstallationsGridView1.Refresh();
            setDataInOrder1();
        }

        private void InstallationsGridView2_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string sortByValue = InstallationsGridView2.Columns[e.ColumnIndex].DataPropertyName;

            if (sortByValue != null && sortByValue.Equals("DeliveryDate"))
            {

                deliveryDateFilter2 += 1;
                deliveryDateFilter2 %= 3;

                if (deliveryDateFilter2 == 0) filteredInstallations2 = filteredInstallations2.OrderBy(i => i.id).ToList();
                else if (deliveryDateFilter2 == 1) filteredInstallations2 = filteredInstallations2.OrderBy(i => i.order.delivery_date).ToList();
                else if (deliveryDateFilter2 == 2) filteredInstallations2 = filteredInstallations2.OrderByDescending(i => i.order.delivery_date).ToList();

            }
            else if (sortByValue != null && sortByValue.Equals("Date"))
            {
                dateFilter2 += 1;
                dateFilter2 %= 3;

                if (dateFilter2 == 0) filteredInstallations2 = filteredInstallations2.OrderBy(i => i.id).ToList();
                else if (dateFilter2 == 1) filteredInstallations2 = filteredInstallations2.OrderBy(i => i.date).ToList();
                else if (dateFilter2 == 2) filteredInstallations2 = filteredInstallations2.OrderByDescending(i => i.date).ToList();
            }

            InstallationsGridView2.Invalidate();
            InstallationsGridView2.Refresh();


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

        private void btn_reset1_Click(object sender, EventArgs e)
        {
            deliveryDateFilter = 0;
            dateFilter = 0;


            foreach (Control control in productType1.Controls)
            {
                if (control is CheckBox checkBox)
                {
                    checkBox.Checked = false;
                }
            }

            foreach (Control control in productPayment1.Controls)
            {
                if (control is CheckBox checkBox)
                {
                    checkBox.Checked = false;
                }
            }


            foreach (Control control in installationPayment1.Controls)
            {
                if (control is CheckBox checkBox)
                {
                    checkBox.Checked = false;
                }
            }

            //  loadDataInstallations();
        }

        private void btn_reset2_Click(object sender, EventArgs e)
        {
            deliveryDateFilter2 = 0;
            dateFilter2 = 0;


            foreach (Control control in productType2.Controls)
            {
                if (control is CheckBox checkBox)
                {
                    checkBox.Checked = false;
                }
            }

            foreach (Control control in productPayment2.Controls)
            {
                if (control is CheckBox checkBox)
                {
                    checkBox.Checked = false;
                }
            }


            foreach (Control control in installationPayment2.Controls)
            {
                if (control is CheckBox checkBox)
                {
                    checkBox.Checked = false;
                }
            }

            // loadDataInstallations();

        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            btn_reset1_Click(sender, e);
            btn_reset2_Click(sender, e);

            //loadDataInstallations();
        }

        private void btn_copy1_Click(object sender, EventArgs e)
        {
            Installation i = getCurrentInstallation();
            if (i != null)
            {
                using (var context = new MyDbConnection())
                {
                    Installation installationToAdd = new Installation
                    {
                        client_id = i.client_id,
                        order_id = i.order_id,
                        installation_address = i.installation_address,
                        notes = "Kontynuacja montażu z dnia " + dateTimeToString(i.date),
                        date = null
                    };

                    context.Installations.Add(installationToAdd);

                    context.SaveChanges();
                }
            }
        }

        private void InstallationsGridView1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < filteredInstallations1.Count)
            {
                var i = filteredInstallations1[e.RowIndex];

                switch (InstallationsGridView1.Columns[e.ColumnIndex].Name)
                {
                    case "DeliveryDate":
                        e.Value = dateTimeToString(i.order.delivery_date);
                        break;
                    case "Client":
                        e.Value = i.order.client.DisplayNameOnly;
                        break;
                    case "Phone":
                        e.Value = i.order.client.phone_number;
                        break;
                    case "Address":
                        e.Value = i.order.installation_address;
                        break;
                    case "State":
                        e.Value = i.order.state;
                        break;
                    case "Product":
                        e.Value = i.order.product.name;
                        break;
                    case "Instance":
                        e.Value = i.order.instance;
                        break;
                    case "SquaredMeters":
                        e.Value = i.order.squared_meters;
                        break;
                    case "Employee1":
                        e.Value = i.order.employee.first_name;
                        break;
                    case "Notes":
                        e.Value = i.notes;
                        break;
                    case "Date":
                        e.Value = dateTimeToString(i.date);
                        break;
                    case "Employee2":
                        e.Value = string.Join("\n", i.Employees.Select(empl => empl.first_name));
                        break;
                    case "Number":
                        e.Value = i.order.orderCompanyNumber();
                        break;
                }
            }
        }

        private void InstallationsGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.RowIndex < filteredInstallations1.Count)
            {
                Installation installation = filteredInstallations1[e.RowIndex];

                if (installation != null && !string.IsNullOrEmpty(installation.color_rgb))
                {
                    try
                    {
                        InstallationsGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = ColorTranslator.FromHtml(installation.color_rgb);
                    }
                    catch (Exception ex)
                    {
                        // Obsługa ewentualnych wyjątków, np. nieprawidłowy format koloru
                        // MessageBox.Show($"Błąd formatu koloru dla zamówienia {recordId}: {ex.Message}");
                    }
                }
                else if (installation != null)
                {
                    if (e.RowIndex % 2 == 0)
                    {
                        InstallationsGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
                    }
                    else
                    {
                        InstallationsGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
        }

        private void InstallationsGridView2_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < filteredInstallations2.Count)
            {
                var i = filteredInstallations2[e.RowIndex];

                switch (InstallationsGridView2.Columns[e.ColumnIndex].Name)
                {
                    case "DeliveryDate2":
                        e.Value = dateTimeToString(i.order.delivery_date);
                        break;
                    case "Client2":
                        e.Value = i.order.client.DisplayNameOnly;
                        break;
                    case "Phone2":
                        e.Value = i.order.client.phone_number;
                        break;
                    case "Address2":
                        e.Value = i.order.installation_address;
                        break;
                    case "State2":
                        e.Value = i.order.state;
                        break;
                    case "Product2":
                        e.Value = i.order.product.name;
                        break;
                    case "Instance2":
                        e.Value = i.order.instance;
                        break;
                    case "SquaredMeters2":
                        e.Value = i.order.squared_meters;
                        break;
                    case "Employee12":
                        e.Value = i.order.employee.first_name;
                        break;
                    case "Notes2":
                        e.Value = i.notes;
                        break;
                    case "Date2":
                        e.Value = dateTimeToString(i.date);
                        break;
                    case "Employee22":
                        e.Value = string.Join("\n", i.Employees.Select(empl => empl.first_name));
                        break;
                    case "Number2":
                        e.Value = i.order.orderCompanyNumber();
                        break;
                }
            }
        }

        private void InstallationsGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.RowIndex < filteredInstallations2.Count)
            {
                Installation installation = filteredInstallations2[e.RowIndex];

                if (installation != null && !string.IsNullOrEmpty(installation.color_rgb))
                {
                    try
                    {
                        InstallationsGridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor = ColorTranslator.FromHtml(installation.color_rgb);
                    }
                    catch (Exception ex)
                    {
                        // Obsługa ewentualnych wyjątków, np. nieprawidłowy format koloru
                        // MessageBox.Show($"Błąd formatu koloru dla zamówienia {recordId}: {ex.Message}");
                    }
                }
                else if (installation != null)
                {
                    if (e.RowIndex % 2 == 0)
                    {
                        InstallationsGridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
                    }
                    else
                    {
                        InstallationsGridView2.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
        }

        private void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem != null)
            {
                string selectedColor = e.ClickedItem.Name;
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

                if (clickedRowIndex != -1)           // górna tabela
                {
                    DataGridViewRow clickedRow = InstallationsGridView1.Rows[clickedRowIndex];

                    var id = filteredInstallations1[clickedRow.Index].id;

                    using (var context = new MyDbConnection())
                    {
                        Installation installation = context.Installations.Find(id);
                        if (installation != null)
                        {
                            installation.color_rgb = hexColor;
                            context.SaveChanges();
                            //if (hexColor != "") InstallationsGridView1.Rows[clickedRowIndex].DefaultCellStyle.BackColor = ColorTranslator.FromHtml(hexColor);
                            // else InstallationsGridView1.Rows[clickedRowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                            filteredInstallations1[clickedRow.Index].color_rgb = hexColor;
                            if (hexColor.Equals("")) InstallationsGridView1.Rows[clickedRowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                        }

                    }
                }
                else if (clickedRowIndex2 != -1)     // dolna tabela
                {
                    DataGridViewRow clickedRow = InstallationsGridView2.Rows[clickedRowIndex2];
                    var id = filteredInstallations2[clickedRow.Index].id;

                    using (var context = new MyDbConnection())
                    {
                        Installation installation = context.Installations.Find(id);
                        if (installation != null)
                        {
                            installation.color_rgb = hexColor;
                            context.SaveChanges();
                            //  if (hexColor != "") InstallationsGridView2.Rows[clickedRowIndex2].DefaultCellStyle.BackColor = ColorTranslator.FromHtml(hexColor);
                            //  else InstallationsGridView2.Rows[clickedRowIndex2].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                            filteredInstallations2[clickedRow.Index].color_rgb = hexColor;
                            if (hexColor.Equals("")) InstallationsGridView2.Rows[clickedRowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                        }

                    }
                }

            }
        }

        private void InstallationsGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo hit = InstallationsGridView1.HitTest(e.X, e.Y);

                clickedRowIndex = -1;
                clickedRowIndex2 = -1;

                if (hit.Type == DataGridViewHitTestType.Cell)
                {
                    clickedRowIndex = hit.RowIndex;

                    InstallationsGridView1.ClearSelection();
                    InstallationsGridView1.Rows[clickedRowIndex].Selected = true;

                    contextMenu.Show(InstallationsGridView1, e.Location);
                }
            }
        }

        private void InstallationsGridView2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo hit = InstallationsGridView2.HitTest(e.X, e.Y);

                clickedRowIndex = -1;
                clickedRowIndex2 = -1;

                if (hit.Type == DataGridViewHitTestType.Cell)
                {
                    clickedRowIndex2 = hit.RowIndex;

                    InstallationsGridView2.ClearSelection();
                    InstallationsGridView2.Rows[clickedRowIndex2].Selected = true;

                    contextMenu.Show(InstallationsGridView2, e.Location);
                }
            }
        }

        private void InstallationsGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Installation i = getCurrentInstallation();

            if (i != null)
            {
                DataGridViewRow row = InstallationsGridView1.Rows[e.RowIndex];

                if (row.Selected)
                {
                    /*
                     if (i.color_rgb != null && !string.IsNullOrEmpty(i.color_rgb))
                     {
                         Color color = ColorTranslator.FromHtml(i.color_rgb);

                         int red = Math.Min(255, (int)color.R + 30);
                         int green = Math.Min(255, (int)color.G + 30);
                         int blue = Math.Min(255, (int)color.B + 30);

                         color = Color.FromArgb(color.A, red, green, blue);

                         InstallationsGridView1.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = color;
                     }
                    */

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

        private void InstallationsGridView2_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Installation i = getCurrentInstallation2();

            if (i != null)
            {
                DataGridViewRow row = InstallationsGridView2.Rows[e.RowIndex];

                if (row.Selected)
                {
                    /*
                    if (i.color_rgb != null && !string.IsNullOrEmpty(i.color_rgb))
                    {
                        Color color = ColorTranslator.FromHtml(i.color_rgb);

                        int red = Math.Min(255, (int)color.R + 30);
                        int green = Math.Min(255, (int)color.G + 30);
                        int blue = Math.Min(255, (int)color.B + 30);

                        color = Color.FromArgb(color.A, red, green, blue);

                        InstallationsGridView2.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = color;
                    }
                    */

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

        private void btn_date_start_Click(object sender, EventArgs e)
        {
            Guna2Button btn = sender as Guna2Button;
            dt_date_start.Location = new Point(btn.Location.X, btn.Location.Y + btn.Height);
            dt_date_start.Visible = true;
            dt_date_start.Focus();
            SendKeys.Send("{F4}");
        }

        private void btn_date_end_Click(object sender, EventArgs e)
        {
            Guna2Button btn = sender as Guna2Button;
            dt_date_end.Location = new Point(btn.Location.X, btn.Location.Y + btn.Height);
            dt_date_end.Visible = true;
            dt_date_end.Focus();
            SendKeys.Send("{F4}");
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

        private void btn_date_startrm_Click(object sender, EventArgs e)
        {
            date_start.Text = "";
            dt_date_start.Value = DateTime.Now;
        }

        private void btn_date_endrm_Click(object sender, EventArgs e)
        {
            date_end.Text = "";
            dt_date_end.Value = DateTime.Now;
        }

        private void btn_date_start_empl_Click(object sender, EventArgs e)
        {
            Guna2Button btn = sender as Guna2Button;
            dt_date_start_empl.Location = new Point(btn.Location.X, btn.Location.Y + btn.Height);
            dt_date_start_empl.Visible = true;
            dt_date_start_empl.Focus();
            SendKeys.Send("{F4}");
        }

        private void btn_date_end_empl_Click(object sender, EventArgs e)
        {
            Guna2Button btn = sender as Guna2Button;
            dt_date_end_empl.Location = new Point(btn.Location.X, btn.Location.Y + btn.Height);
            dt_date_end_empl.Visible = true;
            dt_date_end_empl.Focus();
            SendKeys.Send("{F4}");
        }

        private void dt_date_start_empl_CloseUp(object sender, EventArgs e)
        {
            MetroDateTime dt = sender as MetroDateTime;
            DateTime selectedDate = dt.Value;
            date_start_empl.Text = dateTimeToString(selectedDate);
        }

        private void dt_date_end_empl_CloseUp(object sender, EventArgs e)
        {
            MetroDateTime dt = sender as MetroDateTime;
            DateTime selectedDate = dt.Value;
            date_end_empl.Text = dateTimeToString(selectedDate);
        }

        private void btn_date_startrm_empl_Click(object sender, EventArgs e)
        {
            date_start.Text = "";
            dt_date_start_empl.Value = DateTime.Now;
        }

        private void btn_date_endrm_empl_Click(object sender, EventArgs e)
        {
            date_end.Text = "";
            dt_date_end_empl.Value = DateTime.Now;
        }

        private void guna2TextBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}



