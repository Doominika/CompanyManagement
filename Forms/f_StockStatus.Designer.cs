namespace WindowsFormsAppMySql.Forms
{
    partial class f_StockStatus
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.StocksGridView = new Guna.UI2.WinForms.Guna2DataGridView();
            this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeliveryDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Client = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Notes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Done = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panel9 = new System.Windows.Forms.Panel();
            this.btn_order = new Guna.UI2.WinForms.Guna2Button();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.search = new Guna.UI2.WinForms.Guna2TextBox();
            this.btn_refresh_M = new Guna.UI2.WinForms.Guna2Button();
            this.tableLayoutPanel5.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StocksGridView)).BeginInit();
            this.panel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.AutoScroll = true;
            this.tableLayoutPanel5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33332F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel5.Controls.Add(this.panel8, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 980F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Size = new System.Drawing.Size(1702, 1055);
            this.tableLayoutPanel5.TabIndex = 7;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(224)))), ((int)(((byte)(212)))));
            this.tableLayoutPanel5.SetColumnSpan(this.panel8, 3);
            this.panel8.Controls.Add(this.StocksGridView);
            this.panel8.Controls.Add(this.panel9);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(23, 10);
            this.panel8.Margin = new System.Windows.Forms.Padding(3, 10, 3, 20);
            this.panel8.Name = "panel8";
            this.panel8.Padding = new System.Windows.Forms.Padding(20);
            this.panel8.Size = new System.Drawing.Size(1656, 950);
            this.panel8.TabIndex = 6;
            // 
            // StocksGridView
            // 
            this.StocksGridView.AllowUserToAddRows = false;
            this.StocksGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.StocksGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.StocksGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.StocksGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(225)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.StocksGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.StocksGridView.ColumnHeadersHeight = 50;
            this.StocksGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.StocksGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Number,
            this.DeliveryDate,
            this.Client,
            this.Product,
            this.ProductInfo,
            this.Notes,
            this.Done});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.StocksGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.StocksGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StocksGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.StocksGridView.Location = new System.Drawing.Point(20, 70);
            this.StocksGridView.Name = "StocksGridView";
            this.StocksGridView.ReadOnly = true;
            this.StocksGridView.RowHeadersVisible = false;
            this.StocksGridView.RowHeadersWidth = 51;
            this.StocksGridView.RowTemplate.Height = 50;
            this.StocksGridView.Size = new System.Drawing.Size(1616, 860);
            this.StocksGridView.TabIndex = 3;
            this.StocksGridView.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.StocksGridView.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.StocksGridView.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.StocksGridView.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.StocksGridView.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.StocksGridView.ThemeStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(225)))));
            this.StocksGridView.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.StocksGridView.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.StocksGridView.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.StocksGridView.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.StocksGridView.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.StocksGridView.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.StocksGridView.ThemeStyle.HeaderStyle.Height = 50;
            this.StocksGridView.ThemeStyle.ReadOnly = true;
            this.StocksGridView.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.StocksGridView.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.StocksGridView.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.StocksGridView.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.StocksGridView.ThemeStyle.RowsStyle.Height = 50;
            this.StocksGridView.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.StocksGridView.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.StocksGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.StocksGridView_CellContentClick);
            this.StocksGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.StocksGridView_CellDoubleClick);
            this.StocksGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.StocksGridView_CellFormatting);
            this.StocksGridView.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.MeasurementsGridView_CellValueNeeded);
            this.StocksGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.StocksGridView_ColumnHeaderMouseClick);
            // 
            // Number
            // 
            this.Number.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Number.DataPropertyName = "Number";
            this.Number.FillWeight = 1F;
            this.Number.HeaderText = "Numer zamówienia";
            this.Number.MinimumWidth = 6;
            this.Number.Name = "Number";
            this.Number.ReadOnly = true;
            this.Number.Width = 101;
            // 
            // DeliveryDate
            // 
            this.DeliveryDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.DeliveryDate.DataPropertyName = "DeliveryDate";
            this.DeliveryDate.FillWeight = 1F;
            this.DeliveryDate.HeaderText = "Data dostawy";
            this.DeliveryDate.MinimumWidth = 6;
            this.DeliveryDate.Name = "DeliveryDate";
            this.DeliveryDate.ReadOnly = true;
            this.DeliveryDate.Width = 140;
            // 
            // Client
            // 
            this.Client.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Client.DataPropertyName = "Client";
            this.Client.FillWeight = 1F;
            this.Client.HeaderText = "Klient";
            this.Client.MinimumWidth = 6;
            this.Client.Name = "Client";
            this.Client.ReadOnly = true;
            this.Client.Width = 160;
            // 
            // Product
            // 
            this.Product.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Product.DataPropertyName = "Product";
            this.Product.FillWeight = 1F;
            this.Product.HeaderText = "Towar";
            this.Product.MinimumWidth = 6;
            this.Product.Name = "Product";
            this.Product.ReadOnly = true;
            this.Product.Width = 101;
            // 
            // ProductInfo
            // 
            this.ProductInfo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ProductInfo.DataPropertyName = "ProductInfo";
            this.ProductInfo.FillWeight = 1F;
            this.ProductInfo.HeaderText = "Opis towaru";
            this.ProductInfo.MinimumWidth = 6;
            this.ProductInfo.Name = "ProductInfo";
            this.ProductInfo.ReadOnly = true;
            // 
            // Notes
            // 
            this.Notes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Notes.DataPropertyName = "Notes";
            this.Notes.FillWeight = 1F;
            this.Notes.HeaderText = "Uwagi";
            this.Notes.MinimumWidth = 6;
            this.Notes.Name = "Notes";
            this.Notes.ReadOnly = true;
            // 
            // Done
            // 
            this.Done.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Done.DataPropertyName = "Done";
            this.Done.FillWeight = 1F;
            this.Done.HeaderText = "Odhaczone";
            this.Done.MinimumWidth = 6;
            this.Done.Name = "Done";
            this.Done.ReadOnly = true;
            this.Done.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Done.Width = 101;
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(234)))), ((int)(((byte)(225)))));
            this.panel9.Controls.Add(this.btn_order);
            this.panel9.Controls.Add(this.tableLayoutPanel13);
            this.panel9.Controls.Add(this.search);
            this.panel9.Controls.Add(this.btn_refresh_M);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(20, 20);
            this.panel9.Name = "panel9";
            this.panel9.Padding = new System.Windows.Forms.Padding(1, 5, 0, 5);
            this.panel9.Size = new System.Drawing.Size(1616, 50);
            this.panel9.TabIndex = 0;
            // 
            // btn_order
            // 
            this.btn_order.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_order.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_order.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_order.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_order.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_order.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(156)))), ((int)(((byte)(99)))));
            this.btn_order.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btn_order.ForeColor = System.Drawing.Color.White;
            this.btn_order.Location = new System.Drawing.Point(1387, 5);
            this.btn_order.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btn_order.Name = "btn_order";
            this.btn_order.Size = new System.Drawing.Size(147, 40);
            this.btn_order.TabIndex = 21;
            this.btn_order.Text = "Zamówienie";
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.ColumnCount = 1;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel13.Location = new System.Drawing.Point(1534, 5);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 1;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel13.Size = new System.Drawing.Size(40, 40);
            this.tableLayoutPanel13.TabIndex = 19;
            // 
            // search
            // 
            this.search.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.search.DefaultText = "";
            this.search.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.search.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.search.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.search.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.search.Dock = System.Windows.Forms.DockStyle.Left;
            this.search.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.search.Font = new System.Drawing.Font("Segoe UI", 10.8F);
            this.search.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.search.Location = new System.Drawing.Point(1, 5);
            this.search.Margin = new System.Windows.Forms.Padding(8, 5, 4, 5);
            this.search.Name = "search";
            this.search.PasswordChar = '\0';
            this.search.PlaceholderText = "Szukaj";
            this.search.SelectedText = "";
            this.search.Size = new System.Drawing.Size(306, 40);
            this.search.TabIndex = 15;
            this.search.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.search_KeyPress);
            // 
            // btn_refresh_M
            // 
            this.btn_refresh_M.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_refresh_M.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_refresh_M.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_refresh_M.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_refresh_M.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_refresh_M.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(156)))), ((int)(((byte)(99)))));
            this.btn_refresh_M.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btn_refresh_M.ForeColor = System.Drawing.Color.White;
            this.btn_refresh_M.Location = new System.Drawing.Point(1574, 5);
            this.btn_refresh_M.Name = "btn_refresh_M";
            this.btn_refresh_M.Size = new System.Drawing.Size(42, 40);
            this.btn_refresh_M.TabIndex = 14;
            // 
            // f_StockStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1702, 1055);
            this.Controls.Add(this.tableLayoutPanel5);
            this.Name = "f_StockStatus";
            this.Text = "StockStatus";
            this.tableLayoutPanel5.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.StocksGridView)).EndInit();
            this.panel9.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Panel panel8;
        private Guna.UI2.WinForms.Guna2DataGridView StocksGridView;
        private System.Windows.Forms.Panel panel9;
        private Guna.UI2.WinForms.Guna2Button btn_order;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
        private Guna.UI2.WinForms.Guna2TextBox search;
        private Guna.UI2.WinForms.Guna2Button btn_refresh_M;
        private System.Windows.Forms.DataGridViewTextBoxColumn Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeliveryDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Client;
        private System.Windows.Forms.DataGridViewTextBoxColumn Product;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Notes;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Done;
    }
}