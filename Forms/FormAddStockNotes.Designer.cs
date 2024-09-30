namespace WindowsFormsAppMySql.Forms
{
    partial class FormAddStockNotes
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
            this.note = new System.Windows.Forms.RichTextBox();
            this.label42 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // note
            // 
            this.note.Dock = System.Windows.Forms.DockStyle.Top;
            this.note.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.note.Location = new System.Drawing.Point(20, 53);
            this.note.Name = "note";
            this.note.Size = new System.Drawing.Size(488, 186);
            this.note.TabIndex = 127;
            this.note.Text = "";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Dock = System.Windows.Forms.DockStyle.Top;
            this.label42.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label42.Location = new System.Drawing.Point(20, 20);
            this.label42.Name = "label42";
            this.label42.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.label42.Size = new System.Drawing.Size(102, 33);
            this.label42.TabIndex = 126;
            this.label42.Text = "Opis towaru";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.guna2Button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(20, 239);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.panel1.Size = new System.Drawing.Size(488, 84);
            this.panel1.TabIndex = 129;
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
            this.guna2Button1.Size = new System.Drawing.Size(448, 64);
            this.guna2Button1.TabIndex = 129;
            this.guna2Button1.Text = "Dodaj notatkę";
            this.guna2Button1.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // FormAddStockNotes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(224)))), ((int)(((byte)(212)))));
            this.ClientSize = new System.Drawing.Size(528, 351);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.note);
            this.Controls.Add(this.label42);
            this.Name = "FormAddStockNotes";
            this.Padding = new System.Windows.Forms.Padding(20);
            this.Text = "FormAddStockNotes";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox note;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Panel panel1;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
    }
}