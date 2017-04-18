﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pinger_csharp
{
    public struct Settings
    {
        public Point Location;
        public double SizeMlt;
        public Font Font;
        public double Opacity;
        public int PingInterval;
        public int LabelsNr;
        public Color BackColor;
        public bool GraphActivated;
        public int BarsWidth;
        public int DotHeight;
        public int BarsSpacing;
        public bool WideAlignment;

        public bool SaveSettings()
        {
            try
            {
                //on close save everything
                string[] settings = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
                settings[0] = Location.X.ToString();
                settings[1] = Location.Y.ToString();
                settings[2] = SizeMlt.ToString();
                var cvt = new FontConverter();
                settings[3] = cvt.ConvertToString(Font);
                settings[4] = Opacity.ToString();
                settings[5] = PingInterval.ToString();
                settings[6] = LabelsNr.ToString();
                settings[7] = BackColor.R.ToString();
                settings[8] = BackColor.G.ToString();
                settings[9] = BackColor.B.ToString();
                settings[10] = GraphActivated.ToString();
                settings[11] = BarsWidth.ToString();
                settings[12] = DotHeight.ToString();
                settings[13] = BarsSpacing.ToString();
                settings[14] = WideAlignment.ToString();

                string filepath = Environment.GetEnvironmentVariable("APPDATA") + "\\Pinger\\settings.cfg";
                System.IO.FileInfo file = new System.IO.FileInfo(filepath);
                file.Directory.Create();
                System.IO.File.WriteAllLines(file.FullName, settings, Encoding.UTF8);
                return true;
            }
            catch (Exception e)
            {
                //message zonk, cant save
                return false;
            }

        }
        public bool LoadSettings()
        {
            try
            {
                string filepath = Environment.GetEnvironmentVariable("APPDATA") + "\\Pinger\\settings.cfg";
                if (System.IO.File.Exists(filepath))  //if cfg file exists
                {
                    System.IO.FileInfo file = new System.IO.FileInfo(filepath);
                    file.Directory.Create(); // if the directory already exists, this method does nothing, just a failsafe
                    string[] settings = System.IO.File.ReadAllLines(file.FullName, Encoding.UTF8);
                    Location = new Point(System.Convert.ToInt32(settings[0]), System.Convert.ToInt32(settings[1]));
                    SizeMlt = System.Convert.ToDouble(settings[2]);
                    var cvt = new FontConverter();
                    Font = cvt.ConvertFromString(settings[3]) as Font;
                    Opacity = Convert.ToDouble(settings[4]);
                    LabelsNr = Convert.ToInt32(settings[6]);
                    PingInterval = Convert.ToInt32(settings[5]);
                    BackColor = Color.FromArgb(Convert.ToInt16(settings[7]), Convert.ToInt16(settings[8]), Convert.ToInt16(settings[9]));
                    GraphActivated = Convert.ToBoolean(settings[10]);
                    BarsWidth = Convert.ToInt32(settings[11]);
                    DotHeight = Convert.ToInt32(settings[12]);
                    BarsSpacing = Convert.ToInt32(settings[13]);
                    WideAlignment = Convert.ToBoolean(settings[14]);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }
        public void DefaultValues()
        {
            Location = new Point(0, 0);
            SizeMlt = 0;
            Font = new Font(FontFamily.GenericSansSerif, (float)9.75);
            Opacity = 0.8;
            PingInterval = 300;
            LabelsNr = 1;
            BackColor = Color.FromArgb(64, 64, 64);
            GraphActivated = false;
            BarsWidth = 1;
            DotHeight = 1;
            BarsSpacing = 0;
            WideAlignment = true;
        }
    }
    public partial class OverlayForm : Form
    {
        public OverlayForm()
        {
            InitializeComponent();
        }
        string filepath = Environment.GetEnvironmentVariable("APPDATA") + "\\Pinger\\settings.cfg"; //cfg filepath
        
        protected override CreateParams CreateParams //to make it alt tab invisible
        {
            get
            {
                CreateParams pm = base.CreateParams;
                pm.ExStyle |= 0x80;
                return pm;
            }
        }
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        private Settings UsedSettings;
        private int labelsNr = 0;
        private List<List<int>> graphPings = new List<List<int>>();
        private List<IPAddress> validatedAdresses = new List<IPAddress>();
        private List<int> maxValue = new List<int>();
        private int GraphLimit=1;
        private DragButton dragbutton;
        private void OverlayForm_Load(object sender, EventArgs e)
        {
            BackColor = Color.Black;
            TransparencyKey = Color.Black;
            TopMost = true;
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            Opacity = 60;
            //prepare dragbutton
            dragbutton = new DragButton();
            dragbutton.Opacity = 60;
            dragbutton.Show();
            dragbutton.ContextMenuStrip = contextMenuStrip;
            //make overlay not clickable
            int initialStyle = GetWindowLong(Handle, -20);
            SetWindowLong(Handle, -20, initialStyle | 0x80000 | 0x20);

            if (!UsedSettings.LoadSettings())
                UsedSettings.DefaultValues();
            dragbutton.Location = new Point (UsedSettings.Location.X, UsedSettings.Location.Y);
        }
        public static int RandNumber(int Low, int High)
        {
            Random rndNum = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), System.Globalization.NumberStyles.HexNumber));

            int rnd = rndNum.Next(Low, High);

            return rnd;
        }
        public void RemoveLastLabel()
        {
            if (labelsNr < 1)
                return;
            else
            {
                List<Label> lToRemove = new List<Label>();
                foreach (Label label in Controls.OfType<Label>())
                {
                    string name = "" + labelsNr;

                    if (label.Name == name)
                    {
                        lToRemove.Add(label);
                    }
                }
                foreach (Label label in lToRemove)
                {
                    Controls.Remove(label);
                    label.Dispose();
                }
                adressesToolStripMenuItem.DropDownItems.RemoveByKey("T"+labelsNr);
                labelsNr--;
                validatedAdresses.RemoveAt(labelsNr);
            }
        }
        public System.Windows.Forms.Label AddNewLabel()
        {
            System.Windows.Forms.Label label = new System.Windows.Forms.Label();
            this.Controls.Add(label);
            labelsNr++;
            ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem()
            {
                Name = "T"+labelsNr,
                Text = "(" + labelsNr + ")"
            };
            adressesToolStripMenuItem.DropDownItems.Add(item);
            ToolStripTextBox bar = new System.Windows.Forms.ToolStripTextBox()
            {
                Name = "B" + labelsNr,
                Text = ""
            };
            bar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.bar_KeyPress);
            bar.Text = "127.0.0.1";
            validatedAdresses.Add(IPAddress.Parse("127.0.0.1"));
            item.DropDownItems.Add(bar);
            label.ForeColor = Color.White;
            label.BackColor = Color.FromArgb(64, 64, 64);
            label.Name = "" + labelsNr;
            label.Size = new Size((int)(label.Font.SizeInPoints * widthscale), (int)(label.Font.SizeInPoints * heightscale));
            label.Location = new Point((labelsNr - 1) * label.Width+1, 0);
            label.Text = "Ping " + labelsNr;
            Size = new Size(label.Right, label.Height);
            graphPings.Add(new List<int> { 0 });
            // if (graphsActivated)
            //     DrawGraphs();
            recalculateSize();
            return label;
        }
        private double widthscale = 8.5;
        private double heightscale = 1.5;
        private void recalculateSize()
        {

        }
        private void bar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)//on enter press
            {
                ToolStripTextBox textbox = sender as ToolStripTextBox;
                if (textbox != null)
                {
                    int number = Int32.Parse(textbox.Name[1].ToString()) - 1;
                    Checkipadress(number);
                }

            }
        }
        private void Checkipadress(int id) //checks if address is valid, without pingin, if yes, convert to IP4/6
        {
            string name = "B" + (id + 1);
            ToolStripItem[] menu = adressesToolStripMenuItem.DropDownItems.Find(name, true);

            name = menu[0].Text;
            IPAddress validated;
            if (IPAddress.TryParse(name, out validated))
            {
                validatedAdresses[id] = validated;
                menu[0].Text = validatedAdresses[id].ToString();
            }
            else
            {
                try
                {
                    name = Dns.GetHostAddresses(name)[0].ToString();
                }
                catch (Exception)
                {
                    name = "Can't reach!";
                }
            }
        }
        private void throwPing_Tick(object sender, EventArgs e)
        {
            try
            {
                if (labelsNr >= 1)
                {
                    foreach (Label label in Controls.OfType<Label>())
                    {
                        int number;
                        number = Int32.Parse(label.Name) - 1;
                        Thread t = new Thread(() => pingthread(number));
                        t.Start();
                    }
                }
            }
            catch(Exception er)
            {
            }
        }
        private void pingthread(int id)
        {
            try
            {
                Label label = this.Controls.Find((id + 1).ToString(), true).FirstOrDefault() as Label;
                Ping pingClass = new Ping();
                PingReply pingReply = pingClass.Send(validatedAdresses[id].ToString());
                long ping = pingReply.RoundtripTime;
                if (ping == 0)
                {
                    label.Text = "Timeout!";
                    label.ForeColor = Color.White;
                }
                else
                {
                    label.Text = "(" + (id+1) + ")" + ping + "ms";

                    label.ForeColor = pingColor(ping);
                }
                if (UsedSettings.GraphActivated == true)
                {
                    if (ping == 0)
                        ping = 1;
                    if (graphPings[id].Count > GraphLimit - 1)
                    {
                        graphPings[id].Insert(GraphLimit, (int)ping);
                        graphPings[id].RemoveAt(0);
                    }
                    else
                    {
                        int temp = (int)ping;
                        graphPings[id].Add(temp);
                    } 
                }
            }
            catch (Exception e)
            {
                Label label = this.Controls.Find((id+1).ToString(), true).FirstOrDefault() as Label;
                label.Text = e.ToString();//"Unreachable!";
                label.ForeColor = Color.White;
            }
        }
        private Color pingColor(long ping) //using 2 diffrent functions to create green to yellow to red spectrum for the ranges 25 to 230 ms.
        {
            if (ping <= 1)
            {
                return Color.FromArgb(255, 255, 255);
            }
            else
            {
                if (ping > 230)
                    ping = 230;

                int r = 0, g = 0;
                if (ping < 25)
                {
                    r = 0;
                    g = 255;
                }
                if (ping < 120)
                {
                    r = (int)(2.55 * ping - 51);
                    if (r > 255)
                        r = 255;
                    if (r < 0)
                        r = 0;
                    g = 255;
                }
                if (ping >= 120)
                {
                    r = 255;
                    g = (int)(-2.125 * ping + 510);
                    if (g > 255)
                        g = 255;
                    if (g < 0)
                        g = 0;
                }

                return Color.FromArgb(r, g, 0);
            }
        }
        private void OverlayForm_Paint(object sender, PaintEventArgs e)
        {
            /*Graphics g = e.Graphics;
            Pen p = new Pen(Color.Red);
            int h = Height - 1;
            int w = Width - 1;
            p.Width = 20;
            g.DrawRectangle(p, 0, 0, w, h);
            Brush b = (Brush)Brushes.Green;
            e.Graphics.FillRectangle(b, RandNumber(0, w), RandNumber(0, h), 1, 1);
            b = (Brush)Brushes.Yellow;
            e.Graphics.FillRectangle(b, RandNumber(0, w), RandNumber(0, h), 1, 1);
            b = (Brush)Brushes.Blue;
            e.Graphics.FillRectangle(b, RandNumber(0, w), RandNumber(0, h), 1, 1);
            */
        }

        private void refresh_Tick(object sender, EventArgs e)
        {
            Refresh();
            Location = new Point(dragbutton.Location.X-1 + dragbutton.Width, dragbutton.Location.Y+1);
            //Update();
            //Invalidate();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

       
        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewLabel();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveLastLabel();
        }

        private void OverlayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UsedSettings.Location = dragbutton.Location ;
            if(!UsedSettings.SaveSettings())
                MessageBox.Show("Cant save",
                "About",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}


