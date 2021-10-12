
namespace Scheduler
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btNextDate = new System.Windows.Forms.Button();
            this.dtCurrent = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dtEnd = new System.Windows.Forms.DateTimePicker();
            this.dtStart = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txFrequency = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbOccurs = new System.Windows.Forms.ComboBox();
            this.dtDateTime = new System.Windows.Forms.DateTimePicker();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txDescription = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txNextExec = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btNextDate);
            this.panel1.Controls.Add(this.dtCurrent);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(567, 122);
            this.panel1.TabIndex = 0;
            // 
            // btNextDate
            // 
            this.btNextDate.Location = new System.Drawing.Point(380, 36);
            this.btNextDate.Name = "btNextDate";
            this.btNextDate.Size = new System.Drawing.Size(154, 23);
            this.btNextDate.TabIndex = 2;
            this.btNextDate.Text = "Calculate next date";
            this.btNextDate.UseVisualStyleBackColor = true;
            this.btNextDate.Click += new System.EventHandler(this.btNextDate_Click);
            // 
            // dtCurrent
            // 
            this.dtCurrent.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtCurrent.Location = new System.Drawing.Point(115, 38);
            this.dtCurrent.Name = "dtCurrent";
            this.dtCurrent.Size = new System.Drawing.Size(200, 20);
            this.dtCurrent.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Current date";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.dtEnd);
            this.panel2.Controls.Add(this.dtStart);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.txFrequency);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.cmbOccurs);
            this.panel2.Controls.Add(this.dtDateTime);
            this.panel2.Controls.Add(this.cmbType);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 122);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(567, 182);
            this.panel2.TabIndex = 1;
            // 
            // dtEnd
            // 
            this.dtEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtEnd.Location = new System.Drawing.Point(380, 143);
            this.dtEnd.Name = "dtEnd";
            this.dtEnd.Size = new System.Drawing.Size(154, 20);
            this.dtEnd.TabIndex = 11;
            this.dtEnd.ValueChanged += new System.EventHandler(this.dtEnd_ValueChanged);
            // 
            // dtStart
            // 
            this.dtStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtStart.Location = new System.Drawing.Point(115, 141);
            this.dtStart.Name = "dtStart";
            this.dtStart.Size = new System.Drawing.Size(154, 20);
            this.dtStart.TabIndex = 10;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(315, 147);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 13);
            this.label9.TabIndex = 9;
            this.label9.Text = "End date";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(28, 149);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Start date";
            // 
            // txFrequency
            // 
            this.txFrequency.Location = new System.Drawing.Point(494, 104);
            this.txFrequency.Name = "txFrequency";
            this.txFrequency.Size = new System.Drawing.Size(40, 20);
            this.txFrequency.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(395, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Frequency";
            // 
            // cmbOccurs
            // 
            this.cmbOccurs.FormattingEnabled = true;
            this.cmbOccurs.Location = new System.Drawing.Point(115, 104);
            this.cmbOccurs.Name = "cmbOccurs";
            this.cmbOccurs.Size = new System.Drawing.Size(121, 21);
            this.cmbOccurs.TabIndex = 5;
            // 
            // dtDateTime
            // 
            this.dtDateTime.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtDateTime.Location = new System.Drawing.Point(115, 58);
            this.dtDateTime.Name = "dtDateTime";
            this.dtDateTime.Size = new System.Drawing.Size(419, 20);
            this.dtDateTime.TabIndex = 4;
            this.dtDateTime.ValueChanged += new System.EventHandler(this.dtDateTime_ValueChanged);
            // 
            // cmbType
            // 
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(115, 22);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(121, 21);
            this.cmbType.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Occurs";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "DateTime";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Type";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.txDescription);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.txNextExec);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 304);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(567, 144);
            this.panel3.TabIndex = 2;
            // 
            // txDescription
            // 
            this.txDescription.Location = new System.Drawing.Point(136, 60);
            this.txDescription.Multiline = true;
            this.txDescription.Name = "txDescription";
            this.txDescription.Size = new System.Drawing.Size(398, 55);
            this.txDescription.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(31, 60);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Description";
            // 
            // txNextExec
            // 
            this.txNextExec.Location = new System.Drawing.Point(136, 24);
            this.txNextExec.Name = "txNextExec";
            this.txNextExec.ReadOnly = true;
            this.txNextExec.Size = new System.Drawing.Size(398, 20);
            this.txNextExec.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Next execution time";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 448);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btNextDate;
        private System.Windows.Forms.DateTimePicker dtCurrent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cmbOccurs;
        private System.Windows.Forms.DateTimePicker dtDateTime;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txFrequency;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txDescription;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txNextExec;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtStart;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dtEnd;
    }
}

