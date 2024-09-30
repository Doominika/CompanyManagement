namespace WindowsFormsAppMySql.Forms
{
    partial class FormAddClient
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
            this.guna2Panel3 = new Guna.UI2.WinForms.Guna2Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_add = new Guna.UI2.WinForms.Guna2Button();
            this.mail = new Guna.UI2.WinForms.Guna2TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.address = new Guna.UI2.WinForms.Guna2TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.phone = new Guna.UI2.WinForms.Guna2TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.lastname = new Guna.UI2.WinForms.Guna2TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.name = new Guna.UI2.WinForms.Guna2TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label67 = new System.Windows.Forms.Label();
            this.guna2Panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2Panel3
            // 
            this.guna2Panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(224)))), ((int)(((byte)(212)))));
            this.guna2Panel3.BorderRadius = 20;
            this.guna2Panel3.Controls.Add(this.panel2);
            this.guna2Panel3.Controls.Add(this.mail);
            this.guna2Panel3.Controls.Add(this.label10);
            this.guna2Panel3.Controls.Add(this.address);
            this.guna2Panel3.Controls.Add(this.label11);
            this.guna2Panel3.Controls.Add(this.phone);
            this.guna2Panel3.Controls.Add(this.label12);
            this.guna2Panel3.Controls.Add(this.lastname);
            this.guna2Panel3.Controls.Add(this.label13);
            this.guna2Panel3.Controls.Add(this.name);
            this.guna2Panel3.Controls.Add(this.label16);
            this.guna2Panel3.Controls.Add(this.label67);
            this.guna2Panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2Panel3.Location = new System.Drawing.Point(30, 30);
            this.guna2Panel3.Margin = new System.Windows.Forms.Padding(3, 20, 20, 3);
            this.guna2Panel3.Name = "guna2Panel3";
            this.guna2Panel3.Padding = new System.Windows.Forms.Padding(30, 0, 30, 0);
            this.guna2Panel3.Size = new System.Drawing.Size(472, 501);
            this.guna2Panel3.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btn_add);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(30, 413);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(0, 20, 0, 20);
            this.panel2.Size = new System.Drawing.Size(412, 85);
            this.panel2.TabIndex = 14;
            // 
            // btn_add
            // 
            this.btn_add.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_add.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_add.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_add.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_add.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_add.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(99)))), ((int)(((byte)(141)))));
            this.btn_add.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btn_add.ForeColor = System.Drawing.Color.White;
            this.btn_add.Location = new System.Drawing.Point(0, 20);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(412, 45);
            this.btn_add.TabIndex = 13;
            this.btn_add.Text = "Dodaj";
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // mail
            // 
            this.mail.AutoSize = true;
            this.mail.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mail.DefaultText = "";
            this.mail.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.mail.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.mail.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.mail.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.mail.Dock = System.Windows.Forms.DockStyle.Top;
            this.mail.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.mail.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.mail.ForeColor = System.Drawing.Color.Black;
            this.mail.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.mail.Location = new System.Drawing.Point(30, 373);
            this.mail.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.mail.Name = "mail";
            this.mail.PasswordChar = '\0';
            this.mail.PlaceholderText = "";
            this.mail.SelectedText = "";
            this.mail.Size = new System.Drawing.Size(412, 40);
            this.mail.TabIndex = 10;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Top;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label10.Location = new System.Drawing.Point(30, 340);
            this.label10.Name = "label10";
            this.label10.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.label10.Size = new System.Drawing.Size(58, 33);
            this.label10.TabIndex = 9;
            this.label10.Text = "E-mail";
            // 
            // address
            // 
            this.address.AutoSize = true;
            this.address.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.address.DefaultText = "";
            this.address.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.address.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.address.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.address.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.address.Dock = System.Windows.Forms.DockStyle.Top;
            this.address.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.address.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.address.ForeColor = System.Drawing.Color.Black;
            this.address.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.address.Location = new System.Drawing.Point(30, 300);
            this.address.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.address.Name = "address";
            this.address.PasswordChar = '\0';
            this.address.PlaceholderText = "";
            this.address.SelectedText = "";
            this.address.Size = new System.Drawing.Size(412, 40);
            this.address.TabIndex = 8;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Top;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label11.Location = new System.Drawing.Point(30, 267);
            this.label11.Name = "label11";
            this.label11.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.label11.Size = new System.Drawing.Size(135, 33);
            this.label11.TabIndex = 7;
            this.label11.Text = "Adres do faktury";
            // 
            // phone
            // 
            this.phone.AutoSize = true;
            this.phone.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.phone.DefaultText = "";
            this.phone.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.phone.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.phone.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.phone.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.phone.Dock = System.Windows.Forms.DockStyle.Top;
            this.phone.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.phone.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.phone.ForeColor = System.Drawing.Color.Black;
            this.phone.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.phone.Location = new System.Drawing.Point(30, 227);
            this.phone.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.phone.Name = "phone";
            this.phone.PasswordChar = '\0';
            this.phone.PlaceholderText = "";
            this.phone.SelectedText = "";
            this.phone.Size = new System.Drawing.Size(412, 40);
            this.phone.TabIndex = 6;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Top;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label12.Location = new System.Drawing.Point(30, 194);
            this.label12.Name = "label12";
            this.label12.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.label12.Size = new System.Drawing.Size(64, 33);
            this.label12.TabIndex = 5;
            this.label12.Text = "Telefon";
            // 
            // lastname
            // 
            this.lastname.AutoSize = true;
            this.lastname.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.lastname.DefaultText = "";
            this.lastname.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.lastname.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.lastname.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.lastname.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.lastname.Dock = System.Windows.Forms.DockStyle.Top;
            this.lastname.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.lastname.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lastname.ForeColor = System.Drawing.Color.Black;
            this.lastname.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.lastname.Location = new System.Drawing.Point(30, 154);
            this.lastname.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.lastname.Name = "lastname";
            this.lastname.PasswordChar = '\0';
            this.lastname.PlaceholderText = "";
            this.lastname.SelectedText = "";
            this.lastname.Size = new System.Drawing.Size(412, 40);
            this.lastname.TabIndex = 4;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Dock = System.Windows.Forms.DockStyle.Top;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label13.Location = new System.Drawing.Point(30, 121);
            this.label13.Name = "label13";
            this.label13.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.label13.Size = new System.Drawing.Size(81, 33);
            this.label13.TabIndex = 3;
            this.label13.Text = "Nazwisko";
            // 
            // name
            // 
            this.name.AutoSize = true;
            this.name.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.name.DefaultText = "";
            this.name.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.name.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.name.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.name.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.name.Dock = System.Windows.Forms.DockStyle.Top;
            this.name.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.name.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.name.ForeColor = System.Drawing.Color.Black;
            this.name.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.name.Location = new System.Drawing.Point(30, 81);
            this.name.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.name.Name = "name";
            this.name.PasswordChar = '\0';
            this.name.PlaceholderText = "";
            this.name.SelectedText = "";
            this.name.Size = new System.Drawing.Size(412, 40);
            this.name.TabIndex = 2;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Dock = System.Windows.Forms.DockStyle.Top;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label16.Location = new System.Drawing.Point(30, 48);
            this.label16.Name = "label16";
            this.label16.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.label16.Size = new System.Drawing.Size(43, 33);
            this.label16.TabIndex = 1;
            this.label16.Text = "Imię";
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.Dock = System.Windows.Forms.DockStyle.Top;
            this.label67.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label67.Location = new System.Drawing.Point(30, 0);
            this.label67.Name = "label67";
            this.label67.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.label67.Size = new System.Drawing.Size(128, 48);
            this.label67.TabIndex = 0;
            this.label67.Text = "Dodaj klienta";
            this.label67.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormAddClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 561);
            this.Controls.Add(this.guna2Panel3);
            this.Name = "FormAddClient";
            this.Padding = new System.Windows.Forms.Padding(30);
            this.Text = "FormAddClient";
            this.guna2Panel3.ResumeLayout(false);
            this.guna2Panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2Panel3;
        private System.Windows.Forms.Panel panel2;
        private Guna.UI2.WinForms.Guna2Button btn_add;
        private Guna.UI2.WinForms.Guna2TextBox mail;
        private System.Windows.Forms.Label label10;
        private Guna.UI2.WinForms.Guna2TextBox address;
        private System.Windows.Forms.Label label11;
        private Guna.UI2.WinForms.Guna2TextBox phone;
        private System.Windows.Forms.Label label12;
        private Guna.UI2.WinForms.Guna2TextBox lastname;
        private System.Windows.Forms.Label label13;
        private Guna.UI2.WinForms.Guna2TextBox name;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label67;
    }
}