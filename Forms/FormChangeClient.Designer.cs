namespace WindowsFormsAppMySql.Forms
{
    partial class FormChangeClient
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
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.client_search = new Guna.UI2.WinForms.Guna2TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cb_client_AM = new MetroFramework.Controls.MetroComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(224)))), ((int)(((byte)(212)))));
            this.guna2Panel1.BorderRadius = 20;
            this.guna2Panel1.Controls.Add(this.panel1);
            this.guna2Panel1.Controls.Add(this.panel2);
            this.guna2Panel1.Controls.Add(this.client_search);
            this.guna2Panel1.Controls.Add(this.label16);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Margin = new System.Windows.Forms.Padding(20, 20, 20, 3);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Padding = new System.Windows.Forms.Padding(30, 0, 30, 0);
            this.guna2Panel1.Size = new System.Drawing.Size(576, 218);
            this.guna2Panel1.TabIndex = 5;
            // 
            // client_search
            // 
            this.client_search.AutoSize = true;
            this.client_search.BorderRadius = 4;
            this.client_search.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.client_search.DefaultText = "";
            this.client_search.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.client_search.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.client_search.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.client_search.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.client_search.Dock = System.Windows.Forms.DockStyle.Top;
            this.client_search.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.client_search.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.client_search.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.client_search.Location = new System.Drawing.Point(30, 33);
            this.client_search.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.client_search.Name = "client_search";
            this.client_search.PasswordChar = '\0';
            this.client_search.PlaceholderText = "";
            this.client_search.SelectedText = "";
            this.client_search.Size = new System.Drawing.Size(516, 40);
            this.client_search.TabIndex = 18;
            this.client_search.TextChanged += new System.EventHandler(this.client_search_TextChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Dock = System.Windows.Forms.DockStyle.Top;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label16.Location = new System.Drawing.Point(30, 0);
            this.label16.Name = "label16";
            this.label16.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.label16.Size = new System.Drawing.Size(339, 33);
            this.label16.TabIndex = 17;
            this.label16.Text = "Zmień klienta przypisanego do zamówienia";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(30, 73);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(516, 40);
            this.panel2.TabIndex = 19;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.cb_client_AM);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(0, 3, 5, 0);
            this.panel3.Size = new System.Drawing.Size(516, 40);
            this.panel3.TabIndex = 17;
            // 
            // cb_client_AM
            // 
            this.cb_client_AM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_client_AM.DropDownHeight = 150;
            this.cb_client_AM.DropDownWidth = 100;
            this.cb_client_AM.FormattingEnabled = true;
            this.cb_client_AM.IntegralHeight = false;
            this.cb_client_AM.ItemHeight = 24;
            this.cb_client_AM.Location = new System.Drawing.Point(0, 3);
            this.cb_client_AM.Name = "cb_client_AM";
            this.cb_client_AM.Size = new System.Drawing.Size(511, 30);
            this.cb_client_AM.TabIndex = 1;
            this.cb_client_AM.UseSelectable = true;
            this.cb_client_AM.SelectedIndexChanged += new System.EventHandler(this.cb_client_AM_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.guna2Button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(30, 113);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.panel1.Size = new System.Drawing.Size(516, 84);
            this.panel1.TabIndex = 130;
            // 
            // guna2Button1
            // 
            this.guna2Button1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2Button1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(99)))), ((int)(((byte)(141)))));
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button1.ForeColor = System.Drawing.Color.White;
            this.guna2Button1.Location = new System.Drawing.Point(20, 10);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(476, 64);
            this.guna2Button1.TabIndex = 129;
            this.guna2Button1.Text = "Zapisz zmiany";
            this.guna2Button1.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // FormChangeClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 218);
            this.Controls.Add(this.guna2Panel1);
            this.Name = "FormChangeClient";
            this.Text = "FormChangeClient";
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2TextBox client_search;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private MetroFramework.Controls.MetroComboBox cb_client_AM;
        private System.Windows.Forms.Panel panel1;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
    }
}