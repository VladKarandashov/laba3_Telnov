using System;
using System.Windows.Forms;
namespace Lab3
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private int stops;
        private int number_of_buses;
        private int cap_of_buses;

        private Label[] stations;
        private Label[] queues;
        private Label[] buses;
        private Label[] pass_in_buses; 

        City city;

        private Timer timer_pass;
        private Timer timer_between;
        private Timer timer_at;
        int step = 0;
        

        private TrackBar capacity_Bar;
        private Label capacity_label;
        private TrackBar passengers_Bar;
        private Label passengers_label;
        private TrackBar buses_between_Bar;
        private Label buses_between_label;
        private TrackBar buses_at_Bar;
        private Label buses_at_label;
        private TrackBar minute_Bar;
        private Label minute_label;

        #region ForDrawing
        private int shift_X = 40;
        private int shift_Y = 180;
        private int between_X = 80;
        private int between_Y;
        private int size_of_bars = 200;
        #endregion

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
            this.Size = new System.Drawing.Size(stops * between_X + shift_X + shift_X + size_of_bars, Screen.PrimaryScreen.Bounds.Height);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Name = "Form1";
            this.Text = "Form1";
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Load += Form1_Load;
            between_Y = between_X + 60;

            CreateStations();
            CreateQueues();
            CreateTrackBars();
            CreateTimers();
            cap_of_buses = capacity_Bar.Value;
            city = new City(stops * 2, number_of_buses, cap_of_buses);
            CreateBuses();
            CreatePassInBuses();
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            timer_pass.Start();
            timer_at.Start();
            Update();
        }
        private void Timer_pass_Tick(object sender, System.EventArgs e)
        {
            city.AddPassenger();
            Update();
        }
        private void Timer_between_Tick(object sender, System.EventArgs e)
        {
            if (step < between_X)
            {
                for (int i = 0; i < number_of_buses; i++)
                {
                    if (city.Buses[i].Cur_station < stops - 1)
                    {
                        buses[i].Location = new System.Drawing.Point(buses[i].Location.X + 1, buses[i].Location.Y);
                    }
                    else if (city.Buses[i].Cur_station == stops - 1)
                    {
                        buses[i].Location = new System.Drawing.Point(buses[i].Location.X, buses[i].Location.Y - 1);
                        buses[i].BackColor = System.Drawing.Color.LightGray;
                    }
                    else if (city.Buses[i].Cur_station < 2 * stops - 1)
                    {
                        buses[i].Location = new System.Drawing.Point(buses[i].Location.X - 1, buses[i].Location.Y);
                    }
                    else
                    {
                        buses[i].Location = new System.Drawing.Point(buses[i].Location.X, buses[i].Location.Y + 1);
                        buses[i].BackColor = System.Drawing.Color.LightGray;
                    }
                }
                step++;
            }
            else
            {
                foreach (Label bus in buses) bus.BackColor = System.Drawing.Color.Purple;
                city.Move();
                city.DropOff();
                Update();
                step = 0;
                timer_at.Start();
                timer_between.Stop();
            }
        }
        private void Timer_at_Tick(object sender, System.EventArgs e)
        {
            city.PickUp();
            for (int i = 0; i < number_of_buses; i++)
            {
                city.Buses[i].To_comp = (Passenger pass1, Passenger pass2) => pass1.Finish < pass2.Finish;
                city.Buses[i].Sort();
                if (city.Buses[i].Passengers.Count < city.Cap_of_buses * 0.5)
                    buses[i].BackColor = System.Drawing.Color.LightGreen;
                else if (city.Buses[i].Passengers.Count >= city.Cap_of_buses)
                    buses[i].BackColor = System.Drawing.Color.Red;
                else buses[i].BackColor = System.Drawing.Color.Yellow;
            }
            Update();
            timer_between.Start();
            timer_at.Stop();
        }
        private void CreateTimers()
        {
            timer_pass = new Timer();
            timer_pass.Interval = minute_Bar.Value * passengers_Bar.Value;
            timer_pass.Tick += Timer_pass_Tick;

            timer_between = new Timer();
            timer_between.Interval = minute_Bar.Value * buses_between_Bar.Value / between_X;
            timer_between.Tick += Timer_between_Tick;

            timer_at = new Timer();
            timer_at.Interval = minute_Bar.Value * buses_at_Bar.Value;
            timer_at.Tick += Timer_at_Tick;
        }
        private void CreateStations()
        {
            stations = new Label[2 * stops];
            for (int i = 0, j = 2 * stops - 1; i < stops; i++, j--)
            {
                stations[i] = new Label();
                stations[j] = new Label();
                stations[i].Location = new System.Drawing.Point(i * between_X + shift_X, shift_Y + between_Y);
                stations[j].Location = new System.Drawing.Point(i * between_X + shift_X, shift_Y);
                stations[i].Text = i.ToString();
                stations[j].Text = i.ToString();
                
            }
            foreach (Label stop in stations)
            {
                stop.BorderStyle = BorderStyle.None;
                stop.BackColor = System.Drawing.Color.LightBlue;
                stop.Font = new System.Drawing.Font(stop.Font.Name, 10);
                stop.Size = new System.Drawing.Size(25, 25);
                stop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                Controls.Add(stop);
            }
        }
        private void CreateQueues()
        {
            queues = new Label[2 * stops];
            for (int i = 0; i < 2 * stops; i++) queues[i] = new Label();
            for (int i = 0; i < stops; i++)
            {
                queues[i].Location = new System.Drawing.Point(i * between_X + shift_X - 12, shift_Y + between_Y + 35);
                queues[2 * stops - i - 1].Location = new System.Drawing.Point(i * between_X + shift_X - 12, shift_Y - 160);
                queues[i].TextAlign = System.Drawing.ContentAlignment.TopCenter;
                queues[2 * stops - i - 1].TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            }
            foreach (Label text in queues)
            {
                text.BackColor = System.Drawing.Color.White;
                text.Font = new System.Drawing.Font(text.Font.Name, 9);
                text.Size = new System.Drawing.Size(49, 150);
                Controls.Add(text);
            }
            queues[stops - 1].BackColor = queues[2 * stops - 1].BackColor = this.BackColor;
        }
        private void CreateTrackBars()
        {
            capacity_Bar = new TrackBar();
            capacity_Bar.Location = new System.Drawing.Point(stops * between_X + shift_X, 50);
            capacity_Bar.Size = new System.Drawing.Size(size_of_bars, 30);
            capacity_Bar.Maximum = 100;
            capacity_Bar.Value = 15;
            capacity_Bar.Minimum = 1;
            capacity_Bar.BackColor = System.Drawing.Color.White;
            capacity_Bar.TabStop = false;
            capacity_Bar.Cursor = System.Windows.Forms.Cursors.Hand;
            capacity_Bar.TickFrequency = 5;
            capacity_label = new Label(); 
            capacity_label.Location = new System.Drawing.Point(stops * between_X + shift_X, 30);
            capacity_label.Size = new System.Drawing.Size(size_of_bars, 15);
            capacity_label.Text = String.Format("Текущая вместимость автобуса: {0}", capacity_Bar.Value);
            capacity_label.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            capacity_Bar.ValueChanged += Capacity_Bar_ValueChanged;
            Controls.Add(capacity_label);
            Controls.Add(capacity_Bar);

            passengers_Bar = new TrackBar();
            passengers_Bar.Location = new System.Drawing.Point(stops * between_X + shift_X, 130);
            passengers_Bar.Size = new System.Drawing.Size(size_of_bars, 30);
            passengers_Bar.Maximum = 20;
            passengers_Bar.Value = 3;
            passengers_Bar.Minimum = 1;
            passengers_Bar.BackColor = System.Drawing.Color.White;
            passengers_Bar.TabStop = false;
            passengers_Bar.Cursor = System.Windows.Forms.Cursors.Hand;
            passengers_label = new Label();
            passengers_label.Location = new System.Drawing.Point(stops * between_X + shift_X, 100);
            passengers_label.Size = new System.Drawing.Size(size_of_bars, 30);
            passengers_label.Text = String.Format("Время между появлениями пассажиров: {0}", passengers_Bar.Value);
            passengers_label.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            passengers_Bar.ValueChanged += Passengers_Bar_ValueChanged;
            Controls.Add(passengers_label);
            Controls.Add(passengers_Bar);

            buses_between_Bar = new TrackBar();
            buses_between_Bar.Location = new System.Drawing.Point(stops * between_X + shift_X, 200);
            buses_between_Bar.Size = new System.Drawing.Size(size_of_bars, 30);
            buses_between_Bar.Maximum = 30;
            buses_between_Bar.Value = 5;
            buses_between_Bar.Minimum = 1;
            buses_between_Bar.BackColor = System.Drawing.Color.White;
            buses_between_Bar.TabStop = false;
            buses_between_Bar.Cursor = System.Windows.Forms.Cursors.Hand;
            buses_between_label = new Label();
            buses_between_label.Location = new System.Drawing.Point(stops * between_X + shift_X, 180);
            buses_between_label.Size = new System.Drawing.Size(size_of_bars, 15);
            buses_between_label.Text = String.Format("Время движения: {0}", buses_between_Bar.Value);
            buses_between_label.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            buses_between_Bar.ValueChanged += Buses_Between_Bar_ValueChanged;
            Controls.Add(buses_between_Bar);
            Controls.Add(buses_between_label);

            buses_at_Bar = new TrackBar();
            buses_at_Bar.Location = new System.Drawing.Point(stops * between_X + shift_X, 270);
            buses_at_Bar.Size = new System.Drawing.Size(size_of_bars, 30);
            buses_at_Bar.Maximum = 5;
            buses_at_Bar.Value = 3;
            buses_at_Bar.Minimum = 1;
            buses_at_Bar.BackColor = System.Drawing.Color.White;
            buses_at_Bar.TabStop = false;
            buses_at_Bar.Cursor = System.Windows.Forms.Cursors.Hand;
            buses_at_label = new Label();
            buses_at_label.Location = new System.Drawing.Point(stops * between_X + shift_X, 250);
            buses_at_label.Size = new System.Drawing.Size(size_of_bars, 15);
            buses_at_label.Text = String.Format("Время ожидания: {0}", buses_at_Bar.Value);
            buses_at_label.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            buses_at_Bar.ValueChanged += Buses_At_Bar_ValueChanged;
            Controls.Add(buses_at_Bar);
            Controls.Add(buses_at_label);

            minute_Bar = new TrackBar();
            minute_Bar.Location = new System.Drawing.Point(stops * between_X + shift_X, 340);
            minute_Bar.Size = new System.Drawing.Size(size_of_bars, 30);
            minute_Bar.Maximum = 1000;
            minute_Bar.Value = 200;
            minute_Bar.Minimum = 100;
            minute_Bar.TickFrequency = 100;
            minute_Bar.BackColor = System.Drawing.Color.White;
            minute_Bar.TabStop = false;
            minute_Bar.Cursor = System.Windows.Forms.Cursors.Hand;
            minute_label = new Label();
            minute_label.Location = new System.Drawing.Point(stops * between_X + shift_X, 320);
            minute_label.Size = new System.Drawing.Size(size_of_bars, 15);
            minute_label.Text = String.Format("Милисекунд на \"минуту\": {0}", minute_Bar.Value);
            minute_label.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            minute_Bar.ValueChanged += Minute_Bar_ValueChanged;
            Controls.Add(minute_label);
            Controls.Add(minute_Bar);
        }
        private void CreateBuses()
        {
            buses = new Label[number_of_buses];
            for (int i = 0; i < number_of_buses; i++)
            {
                buses[i] = new Label();
                int cur_stop = city.Buses[i].Cur_station;
                if (cur_stop < stops)
                    buses[i].Location = new System.Drawing.Point(cur_stop * between_X + shift_X, shift_Y + between_Y - 30);
                else
                    buses[i].Location = new System.Drawing.Point((2 * stops - cur_stop - 1) * between_X + shift_X, shift_Y + 30);
                buses[i].Size = new System.Drawing.Size(25, 25);
                buses[i].BackColor = System.Drawing.Color.Purple;
                buses[i].Text = i.ToString();
                buses[i].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                buses[i].ForeColor = System.Drawing.Color.White;
                buses[i].Font = new System.Drawing.Font(buses[i].Font.Name, 10);
                Controls.Add(buses[i]);
            }
        }
        private void CreatePassInBuses()
        {
            int shift = 5;
            int size = (this.ClientSize.Width - shift * (number_of_buses + 1)) / number_of_buses; 
            Label[] headers = new Label[number_of_buses];
            pass_in_buses = new Label[number_of_buses];
            for (int i = 0; i < number_of_buses; i++)
            {
                headers[i] = new Label();
                headers[i].Location = new System.Drawing.Point(shift + i * (shift + size), shift_Y + between_Y + 200);
                headers[i].Size = new System.Drawing.Size(size, 15);
                headers[i].Text = String.Format("Bus-{0}", i);
                headers[i].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                headers[i].Font = new System.Drawing.Font(headers[i].Font.Name, 8);
                Controls.Add(headers[i]);
                pass_in_buses[i] = new Label();
                pass_in_buses[i].Location = new System.Drawing.Point(shift + i * (shift + size), shift_Y + between_Y + 220);
                pass_in_buses[i].Size = new System.Drawing.Size(size, 220);
                pass_in_buses[i].BackColor = System.Drawing.Color.White;
                pass_in_buses[i].Font = new System.Drawing.Font(pass_in_buses[i].Font.Name, 12);
                Controls.Add(pass_in_buses[i]);
            }
        }
        private void Capacity_Bar_ValueChanged(object sender, EventArgs e)
        {
            capacity_label.Text = String.Format("Текущая вместимость автобуса: {0}", capacity_Bar.Value);
            city.Cap_of_buses = capacity_Bar.Value;
        }
        private void Passengers_Bar_ValueChanged(object sender, EventArgs e)
        {
            passengers_label.Text = String.Format("Время между появлениями пассажиров: {0}", passengers_Bar.Value);
            timer_pass.Interval = minute_Bar.Value * passengers_Bar.Value;
        }
        private void Buses_Between_Bar_ValueChanged(object sender, EventArgs e)
        {
            buses_between_label.Text = String.Format("Время движения: {0}", buses_between_Bar.Value);
            timer_between.Interval = minute_Bar.Value * buses_between_Bar.Value / between_X;
        }
        private void Buses_At_Bar_ValueChanged(object sender, EventArgs e)
        {
            buses_at_label.Text = String.Format("Время ожидания: {0}", buses_at_Bar.Value);
            timer_at.Interval = minute_Bar.Value * buses_at_Bar.Value;
        }
        private void Minute_Bar_ValueChanged(object sender, EventArgs e)
        {
            minute_label.Text = String.Format("Милисекунд на \"минуту\": {0}", minute_Bar.Value);
            timer_pass.Interval = minute_Bar.Value * passengers_Bar.Value;
            timer_between.Interval = minute_Bar.Value * buses_between_Bar.Value / between_X;
            timer_at.Interval = minute_Bar.Value * buses_at_Bar.Value;
        }
        private void Update()
        {
            for (int i = 0; i < stops; i++)
            {
                queues[i].Text = "";
                queues[2 * stops - i - 1].Text = "";
                foreach (Passenger pass in city.Stations[i])
                {
                    queues[i].Text += pass.Start.ToString() + "->" + pass.Finish.ToString() + "\r\n";
                }
                foreach (Passenger pass in city.Stations[2 * stops - i - 1])
                {
                    if (queues[2 * stops - i - 1].Text == "") 
                        queues[2 * stops - i - 1].Text = (2 * stops - pass.Start - 1).ToString() + "->" + (2 * stops - pass.Finish - 1).ToString();
                    else 
                        queues[2 * stops - i - 1].Text = (2 * stops - pass.Start - 1).ToString() + "->" + (2 * stops - pass.Finish - 1).ToString() + "\r\n" + queues[2 * stops - i - 1].Text;
                }
            }
            for (int i = 0; i < number_of_buses; i++)
            {
                pass_in_buses[i].Text = "";
                foreach (Passenger pass in city.Buses[i].Passengers)
                {
                    if (pass.Start < stops)
                        pass_in_buses[i].Text += pass.Start.ToString() + "-> " + pass.Finish.ToString();
                    else
                        pass_in_buses[i].Text += (2 * stops - pass.Start - 1).ToString() 
                            + "-> " + (2 * stops - pass.Finish - 1).ToString();
                    if (pass.Finish == city.Buses[i].Cur_station + 1)
                        pass_in_buses[i].Text += " *\r\n";
                    else pass_in_buses[i].Text += "\r\n";
                }
            }
        }
    }
}

