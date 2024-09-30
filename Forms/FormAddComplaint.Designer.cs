namespace WindowsFormsAppMySql.Forms
{
    partial class FormAddComplaint
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_save = new Guna.UI2.WinForms.Guna2Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btn_addFiles = new Guna.UI2.WinForms.Guna2Button();
            this.notes = new System.Windows.Forms.RichTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.supplierNumber = new Guna.UI2.WinForms.Guna2TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.complNumber = new Guna.UI2.WinForms.Guna2TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label67 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(224)))), ((int)(((byte)(212)))));
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.notes);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.supplierNumber);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.complNumber);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.label67);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(30, 30);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 20, 20, 3);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(30, 0, 30, 0);
            this.panel1.Size = new System.Drawing.Size(472, 517);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btn_save);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(30, 425);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(0, 20, 0, 20);
            this.panel2.Size = new System.Drawing.Size(412, 85);
            this.panel2.TabIndex = 63;
            // 
            // btn_save
            // 
            this.btn_save.BorderRadius = 5;
            this.btn_save.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_save.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_save.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_save.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_save.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_save.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(99)))), ((int)(((byte)(141)))));
            this.btn_save.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btn_save.ForeColor = System.Drawing.Color.White;
            this.btn_save.Location = new System.Drawing.Point(0, 20);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(412, 45);
            this.btn_save.TabIndex = 13;
            this.btn_save.Text = "Dodaj reklamację";
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btn_addFiles);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(30, 340);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new System.Windows.Forms.Padding(0, 20, 0, 20);
            this.panel5.Size = new System.Drawing.Size(412, 85);
            this.panel5.TabIndex = 62;
            // 
            // btn_addFiles
            // 
            this.btn_addFiles.BorderRadius = 5;
            this.btn_addFiles.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_addFiles.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_addFiles.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_addFiles.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_addFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_addFiles.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(99)))), ((int)(((byte)(141)))));
            this.btn_addFiles.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btn_addFiles.ForeColor = System.Drawing.Color.White;
            this.btn_addFiles.Location = new System.Drawing.Point(0, 20);
            this.btn_addFiles.Name = "btn_addFiles";
            this.btn_addFiles.Size = new System.Drawing.Size(412, 45);
            this.btn_addFiles.TabIndex = 13;
            this.btn_addFiles.Text = "Dodaj pliki";
            this.btn_addFiles.Click += new System.EventHandler(this.btn_addFiles_Click);
            // 
            // notes
            // 
            this.notes.Dock = System.Windows.Forms.DockStyle.Top;
            this.notes.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.notes.Location = new System.Drawing.Point(30, 227);
            this.notes.Name = "notes";
            this.notes.Size = new System.Drawing.Size(412, 113);
            this.notes.TabIndex = 61;
            this.notes.Text = "";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Top;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label9.Location = new System.Drawing.Point(30, 194);
            this.label9.Name = "label9";
            this.label9.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.label9.Size = new System.Drawing.Size(253, 33);
            this.label9.TabIndex = 60;
            this.label9.Text = "Informacje dotyczące reklamacji";
            // 
            // supplierNumber
            // 
            this.supplierNumber.AutoSize = true;
            this.supplierNumber.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.supplierNumber.DefaultText = "";
            this.supplierNumber.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.supplierNumber.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.supplierNumber.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.supplierNumber.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.supplierNumber.Dock = System.Windows.Forms.DockStyle.Top;
            this.supplierNumber.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.supplierNumber.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.supplierNumber.ForeColor = System.Drawing.Color.Black;
            this.supplierNumber.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.supplierNumber.Location = new System.Drawing.Point(30, 154);
            this.supplierNumber.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.supplierNumber.Name = "supplierNumber";
            this.supplierNumber.PasswordChar = '\0';
            this.supplierNumber.PlaceholderText = "";
            this.supplierNumber.SelectedText = "";
            this.supplierNumber.Size = new System.Drawing.Size(412, 40);
            this.supplierNumber.TabIndex = 59;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Top;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label11.Location = new System.Drawing.Point(30, 121);
            this.label11.Name = "label11";
            this.label11.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.label11.Size = new System.Drawing.Size(237, 33);
            this.label11.TabIndex = 58;
            this.label11.Text = "Numer reklamacji producenta";
            // 
            // complNumber
            // 
            this.complNumber.AutoSize = true;
            this.complNumber.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.complNumber.DefaultText = "";
            this.complNumber.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.complNumber.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.complNumber.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.complNumber.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.complNumber.Dock = System.Windows.Forms.DockStyle.Top;
            this.complNumber.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.complNumber.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.complNumber.ForeColor = System.Drawing.Color.Black;
            this.complNumber.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.complNumber.Location = new System.Drawing.Point(30, 81);
            this.complNumber.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.complNumber.Name = "complNumber";
            this.complNumber.PasswordChar = '\0';
            this.complNumber.PlaceholderText = "";
            this.complNumber.SelectedText = "";
            this.complNumber.Size = new System.Drawing.Size(412, 40);
            this.complNumber.TabIndex = 57;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Top;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label12.Location = new System.Drawing.Point(30, 48);
            this.label12.Name = "label12";
            this.label12.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.label12.Size = new System.Drawing.Size(144, 33);
            this.label12.TabIndex = 56;
            this.label12.Text = "Numer reklamacji";
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.Dock = System.Windows.Forms.DockStyle.Top;
            this.label67.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label67.Location = new System.Drawing.Point(30, 0);
            this.label67.Name = "label67";
            this.label67.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.label67.Size = new System.Drawing.Size(133, 48);
            this.label67.TabIndex = 55;
            this.label67.Text = "Dodaj pomiar";
            this.label67.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormAddComplaint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(532, 577);
            this.Controls.Add(this.panel1);
            this.Name = "FormAddComplaint";
            this.Padding = new System.Windows.Forms.Padding(30);
            this.Text = "FormAddComplaint";
            this.Load += new System.EventHandler(this.FormAddComplaint_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Guna.UI2.WinForms.Guna2Button btn_save;
        private System.Windows.Forms.Panel panel5;
        private Guna.UI2.WinForms.Guna2Button btn_addFiles;
        private System.Windows.Forms.RichTextBox notes;
        private System.Windows.Forms.Label label9;
        private Guna.UI2.WinForms.Guna2TextBox supplierNumber;
        private System.Windows.Forms.Label label11;
        private Guna.UI2.WinForms.Guna2TextBox complNumber;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label67;
    }
}