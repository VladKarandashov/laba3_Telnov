using System;
using System.Windows.Forms;

namespace Lab3
{
    partial class StartForm
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox text;
        private System.Windows.Forms.NumericUpDown stops;
        private NumericUpDown buses; 
        private Button buttonOk;

        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 200);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Name = "StartForm";
            this.Text = "StartForm";
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ResumeLayout(false);

            //text
            text = new TextBox();
            text.Location = new System.Drawing.Point(30, 25);
            text.Size = new System.Drawing.Size(240, 100);
            text.Multiline = true;
            text.Text = "Добро пожаловать!\r\nУкажите желаемое число остановок и автобусов на маршруте соответственно";
            text.TextAlign = HorizontalAlignment.Center;
            text.TabStop = false;
            text.ReadOnly = true;
            text.Font = new System.Drawing.Font(text.Font.Name, 13);
            text.BorderStyle = BorderStyle.None;
            Controls.Add(text);

            //stops
            stops = new NumericUpDown();
            stops.Location = new System.Drawing.Point(30, 150);
            stops.Font = new System.Drawing.Font(stops.Font.Name, 16);
            stops.Maximum = new decimal(new int[] { 15, 0, 0, 0 });
            stops.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
            stops.Value = new decimal(new int[] { 5, 0, 0, 0 });
            stops.Size = new System.Drawing.Size(60, 28);
            stops.TabStop = false;
            stops.Cursor = Cursors.Hand;
            stops.ValueChanged += Stops_ValueChanged;
            Controls.Add(stops);

            //buses
            buses = new NumericUpDown();
            buses.Location = new System.Drawing.Point(120, 150);
            buses.Font = new System.Drawing.Font(stops.Font.Name, 16);
            buses.Maximum = new decimal(new int[] { 2 * (int)stops.Value, 0, 0, 0 });
            buses.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            buses.Value = new decimal(new int[] { 2, 0, 0, 0 });
            buses.Size = new System.Drawing.Size(60, 28);
            buses.Cursor = Cursors.Hand;
            buses.TabStop = false;
            Controls.Add(buses);

            //buttonOk
            buttonOk = new Button();
            buttonOk.Location = new System.Drawing.Point(210, 150);
            buttonOk.Size = new System.Drawing.Size(60, 32);
            buttonOk.Text = "Ok";
            buttonOk.Font = new System.Drawing.Font(buttonOk.Font.Name, 16);
            buttonOk.TabStop = false;
            buttonOk.Cursor = Cursors.Hand;
            buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            Controls.Add(buttonOk);
        }

        private void Stops_ValueChanged(object sender, EventArgs e)
        {
            buses.Maximum = new decimal(new int[] { 2 * (int)stops.Value, 0, 0, 0 });
        }

        private void buttonOk_Click(object sender, System.EventArgs e)
        {
            this.Visible = false;
            Form1 form = new Form1((int)stops.Value, (int)buses.Value);
            form.ShowDialog();
            this.Close();
        }
    }
}
