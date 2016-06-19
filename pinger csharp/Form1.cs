﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.NetworkInformation;

namespace pinger_csharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private double fontsize;
        private void Form1_Load(object sender, EventArgs e)
        {
            string filepath = Environment.GetEnvironmentVariable("APPDATA") + "\\Pinger\\settings.cfg";
            label1.ForeColor = Color.FromArgb(64, 64, 64); 
            label2.ForeColor = Color.FromArgb(64, 64, 64);
            if (System.IO.File.Exists(filepath))  //if cfg file exists
            {
                System.IO.FileInfo file = new System.IO.FileInfo(filepath);
                file.Directory.Create(); // if the directory already exists, this method does nothing, just a failsafe
                string[] settings = System.IO.File.ReadAllLines(file.FullName, Encoding.UTF8);
                Rectangle resolution = Screen.PrimaryScreen.Bounds;

                h = resolution.Height;
                w = resolution.Width;
                int x = 0, y = 0;
                if (System.Convert.ToInt32(settings[0]) < w && System.Convert.ToInt32(settings[0]) < h) //if out of bounds of the main screen
                {
                    OwnX.Text = settings[0];
                    OwnY.Text = settings[1];
                    Location = new Point(System.Convert.ToInt32(settings[0]), System.Convert.ToInt32(settings[1]));
                }
                else {
                    if (System.Convert.ToInt32(settings[0]) > w)
                    {
                        OwnX.Text = "" + 0;
                        x = 0;
                        y = System.Convert.ToInt32(settings[1]);
                    }
                    if (System.Convert.ToInt32(settings[1]) > h)
                    {
                        OwnY.Text = "" + 0;
                        x = System.Convert.ToInt32(settings[0]);
                        y = 0;
                    }
                    Location = new Point(x, y);
                }
                fontsize = System.Convert.ToDouble(settings[2]);
                if (fontsize <= 0)  //cant be negative
                {
                    fontsize = 9.75;
                }
                var cvt = new FontConverter();
                Font f = cvt.ConvertFromString(settings[3]) as Font;
                label1.Font = f;
                label2.Font = f;
                pingadress1.Text = settings[5];
                pingadress2.Text = settings[6];
                int r, g, b;
                r = Convert.ToInt16(settings[7]);
                g = Convert.ToInt16(settings[8]);
                b = Convert.ToInt16(settings[9]);
                label1.BackColor = Color.FromArgb(r, g, b);
                label2.BackColor = Color.FromArgb(r, g, b);
            }
            else
            {
                fontsize = 9.75;
            }
            checkipadress();
            refreshsize();
        }
        private double Parsestring(String strings)
        {
            System.Int32 number;

            if (strings != "")
            {
                for (int i = 0; i < strings.Length; i++)
                {
                    if (char.IsLetter(strings[i]) || char.IsPunctuation(strings[i]) || char.IsWhiteSpace(strings[i]))
                    {
                        strings = "";
                        return 0;
                    }
                }
                number = System.Int32.Parse(strings);
                if (strings.Length > 4)
                {
                    strings = "";
                    return 0;
                }
                if (System.Int32.Parse(strings) < 1)
                    return 0;
                if (System.Int32.Parse(strings) > 5000)
                    return 0;
            }
            else {
                strings = "Empty";
                return 0;
            }
            return number;
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by Michał Osowski (Osa__PL)",
            "About",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
        }
        private int w;
        private int h;
        private void leftLowerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rectangle resolution = Screen.PrimaryScreen.Bounds;
            h = resolution.Height;
            OwnX.Text = "" + 0;
            OwnY.Text = "" + (h - Size.Height);
            Location = new Point(0, h - Size.Height);
        }

        private void leftHigherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OwnX.Text = "" + 0;
            OwnY.Text = "" + 0;
            Location = new Point(0, 0);
        }

        private void rightLowerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rectangle resolution = Screen.PrimaryScreen.Bounds;
            h = resolution.Height;
            w = resolution.Width;
            OwnX.Text = "" + (h - Size.Height);
            OwnY.Text = "" + (w - Size.Width);
            Location = new Point(w - Size.Width, h - Size.Height);
        }

        private void rightHigherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rectangle resolution = Screen.PrimaryScreen.Bounds;
            w = resolution.Width;
            OwnX.Text = "" + (w - Size.Width);
            OwnY.Text = "" + 0;
            Location = new Point(w - Size.Width, 0);
        }

        private void OwnX_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)//on enter press, change location
            {
                Location = new Point((int)Parsestring(OwnX.Text), (int)Parsestring(OwnY.Text));
            }
        }
        private IPAddress validatedaddress1;
        private IPAddress validatedaddress2;
        private void toolStripTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)//on enter press, check if ip adresses are valid, and then ping
            {
                checkipadress();
            }
        }
        private Thread th1;
        private Thread th2;
        private void timer1_Tick(object sender, EventArgs e) 
        {
            if (pingadress1.Text != "Wrong address") 
            {
                th1 = new Thread(pingthread1);
                th1.Start();
            }
            else
            {
                label1.Text = "Address!";
                label1.ForeColor = Color.White;
            }
            if (pingadress2.Text != "Wrong address")
            {
                th2 = new Thread(pingthread2);
                th2.Start();
            }
            else
            {
                label2.Text = "Address!";
                label2.ForeColor = Color.White;
            }
            if (!IsOnScreen()) //check if button is on screen
            {
                //Location = new Point(0, 0);
                Rectangle resolution = Screen.PrimaryScreen.Bounds;

                int h = resolution.Height;
                int w = resolution.Width;
                int y = 0, x = 0;
                if (Location.X > w)
                {
                    x = w;
                }
                if (Location.Y > h)
                {
                    y = h;
                }
                Location = new Point(x, y);
                mouseDown = false;
                this.Focus();
            }
        }
        private void pingthread1()
        {
            try
            {
                Ping pingClass = new Ping();
                PingReply pingReply = pingClass.Send(validatedaddress1.ToString());
                long ping = pingReply.RoundtripTime;
                if (ping == 0)
                {
                    label1.Text = "Timeout!";
                    label1.ForeColor = Color.White;
                }
                else {
                    label1.Text = "(1)" + ping + "ms";
                    //using 2 diffrent functions to create green to yellow to red spectrum for the ranges 25 to 230 ms.
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

                    label1.ForeColor = Color.FromArgb(r, g, 0);
                }
            }
            catch (Exception)
            {

            }
        }
        private void pingthread2()
        {
            try
            {
                Ping pingClass = new Ping();
                PingReply pingReply = pingClass.Send(validatedaddress2.ToString());
                long ping = pingReply.RoundtripTime;
                if (ping == 0)
                {
                    label1.Text = "Timeout!";
                    label1.ForeColor = Color.White;
                }
                else {
                    label2.Text = "(2)" + ping + "ms";
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

                    label2.ForeColor = Color.FromArgb(r, g, 0);
                }
            }
            catch (Exception)
            {

            }
        }

        private void timeset_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                double time = Parsestring(timeset.Text);
                if (time < 200)
                {
                    timeset.Text = "";
                }
                else
                {
                    timer1.Interval = (int)time;
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //on close save everything
            string[] settings = { "", "", "", "", "", "", "", "", "", "", "" };
            settings[0] = Location.X.ToString();
            settings[1] = Location.Y.ToString();
            settings[2] = fontsize.ToString();
            var cvt = new FontConverter();
            settings[3] = cvt.ConvertToString(label1.Font);
            settings[4] = "nothing";
            if (validatedaddress1 == null)
                validatedaddress1 = IPAddress.Parse("8.8.8.8");
            if (validatedaddress2 == null)
                validatedaddress2 = IPAddress.Parse("8.8.8.8");

            settings[5] = validatedaddress1.ToString();
            settings[6] = validatedaddress2.ToString();
            settings[7] = label1.BackColor.R.ToString();
            settings[8] = label1.BackColor.G.ToString();
            settings[9] = label1.BackColor.B.ToString();

            string filepath = Environment.GetEnvironmentVariable("APPDATA") + "\\Pinger\\settings.cfg";
            System.IO.FileInfo file = new System.IO.FileInfo(filepath);
            file.Directory.Create();
            System.IO.File.WriteAllLines(file.FullName, settings, Encoding.UTF8);

        }

        private void biggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontsize++;
            setfontsize();
            refreshsize();
        }

        private void smallerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontsize--;
            if (fontsize < 0)//cant be negative
                fontsize = 9.25;
            setfontsize();
            refreshsize();
            
        }

        private void backgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = true;
            MyDialog.Color = label1.BackColor;

            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                label1.BackColor = MyDialog.Color;
                label2.BackColor = MyDialog.Color;
            }
        }

        private void boldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog1 = new FontDialog();
            fontDialog1.Font = label1.Font;

            if (fontDialog1.ShowDialog() != DialogResult.Cancel)
            {
                label1.Font = fontDialog1.Font;
                label2.Font = fontDialog1.Font;
            }
            fontsize = label1.Font.Size;
            refreshsize();
        }

        private void resetlocation_Tick(object sender, EventArgs e)//one time size refresh to make sure labels are alligned properly
        {
            refreshsize();
            resetlocation.Enabled = false;
        }

        // mouse dragging by button
        private bool mouseDown;
        private Point lastPos;
        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                mouseDown = true;
                lastPos = MousePosition;
            }
        }

        private void button1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                int xoffset = MousePosition.X - lastPos.X;
                int yoffset = MousePosition.Y - lastPos.Y;
                Left += xoffset;
                Top += yoffset;
                lastPos = MousePosition;
                OwnX.Text = "" + Location.X;
                OwnY.Text = "" + Location.Y;
            }
        }
        private bool IsOnScreen()
        {
            Screen[] screens = Screen.AllScreens;
            foreach (Screen screen in screens)
            {
                Rectangle controlRectangle = new Rectangle(this.Left, this.Top, this.button1.Width, this.button1.Height);

                if (screen.WorkingArea.Contains(controlRectangle))
                {
                    return true;
                }
            }
            return false;
        }

        private void refreshsize() //recalculates form size and label placement
        {
            label2.Location = new Point(label1.Location.X + label1.Size.Width, 1);
            if (button1.Height < this.Height)
            {
                Size = new Size(14 + label1.Size.Width + label2.Size.Width, this.Height);
            }
            else
            {
                Size = new Size(14 + label1.Size.Width + label2.Size.Width, label1.Height);
            }
        }
        private void setfontsize()//updates font size and style
        {
                label1.Font = new System.Drawing.Font(label1.Font.Name, (float)fontsize, label1.Font.Style);
                label2.Font = new System.Drawing.Font(label1.Font.Name, (float)fontsize, label1.Font.Style);
        }
        private void checkipadress() //checks if address is valid, without pinging!
        {
            if (IPAddress.TryParse(pingadress1.Text, out validatedaddress1))
            {
                pingadress1.Text = validatedaddress1.ToString();

            }
            else {
                try
                {
                    validatedaddress1 = Dns.GetHostAddresses(pingadress1.Text)[0];
                }
                catch (Exception)
                {
                    pingadress1.Text = "Wrong address";
                }
            }

            if (IPAddress.TryParse(pingadress2.Text, out validatedaddress2))
            {
                pingadress2.Text = validatedaddress2.ToString();

            }
            else {
                try
                {
                    validatedaddress2 = Dns.GetHostAddresses(pingadress2.Text)[0];
                }
                catch (Exception)
                {
                    pingadress2.Text = "Wrong address";
                }
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }

}
