using System;
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
            if (System.IO.File.Exists(filepath))
            {
                System.IO.FileInfo file = new System.IO.FileInfo(filepath);
                file.Directory.Create(); // If the directory already exists, this method does nothing.
                string[] settings = System.IO.File.ReadAllLines(file.FullName, Encoding.UTF8);
                Rectangle resolution = Screen.PrimaryScreen.Bounds;

                h = resolution.Height;
                w = resolution.Width;
                int x = 0, y = 0;
                if (System.Convert.ToInt32(settings[0]) < w && System.Convert.ToInt32(settings[0]) < h)
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
                if (fontsize <= 0)
                {
                    fontsize = 9.75;
                }
                if (System.Convert.ToBoolean(settings[3]))
                {
                    label1.BackColor = Color.FromArgb(64, 64, 64);
                    label2.BackColor = Color.FromArgb(64, 64, 64);
                }
                else
                {
                    label1.BackColor = Color.Black;
                    label2.BackColor = Color.Black;
                }
                if (System.Convert.ToBoolean(settings[4]))
                {
                    label1.Font = new System.Drawing.Font("Arial", (float)fontsize, FontStyle.Bold);
                    label2.Font = new System.Drawing.Font("Arial", (float)fontsize, FontStyle.Bold);
                }
                else
                {
                    label1.Font = new System.Drawing.Font("Arial", (float)fontsize, FontStyle.Regular);
                    label2.Font = new System.Drawing.Font("Arial", (float)fontsize, FontStyle.Regular);
                }
                pingadress1.Text = settings[5];
                pingadress2.Text = settings[6];
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
                Size = new Size(14 + label1.Size.Width + label2.Size.Width + 10, label1.Height);
                label2.Location = new Point(label1.Location.X + label1.Size.Width, 1);
            }
            else
            {
                fontsize = 9.75;
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
                Size = new Size(14 + label1.Size.Width + label2.Size.Width + 10, label1.Height);
                label2.Location = new Point(label1.Location.X + label1.Size.Width, 1);
            }
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
        private String ping1;
        private String ping2;
        private double lastping1;
        private double lastping2;

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
            if(e.KeyChar == (char)13)
            {
                Location = new Point((int)Parsestring(OwnX.Text), (int)Parsestring(OwnY.Text));
            }
        }
        private IPAddress validatedaddress1;
        private IPAddress validatedaddress2;
        private void toolStripTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)
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
                label1.ForeColor = Color.BlueViolet;
            }
            if (pingadress2.Text != "Wrong address") { 
                th2 = new Thread(pingthread2);
            th2.Start();
            }
            else
            {
                label2.Text = "Address!";
                label2.ForeColor = Color.BlueViolet;
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
                    label1.ForeColor = Color.BlueViolet;
                }
                else {
                    label1.Text = "(1)" + ping + "ms";
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
                    label1.ForeColor = Color.BlueViolet;
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
                    timeset.Text = "To small interval!";
                }
                else
                {
                    timer1.Interval = (int)time;
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            string[] settings= { "", "", "", "", "", "", "", "" };
            settings[0] = Location.X.ToString();
            settings[1] = Location.Y.ToString();
            settings[2] = fontsize.ToString();
            if (label1.BackColor != Color.FromArgb(64, 64, 64))
                settings[3] = false.ToString();
            else
                settings[3] = true.ToString();
            if (label1.Font.Style != FontStyle.Bold)
                settings[4] = false.ToString();
            else
                settings[4] = true.ToString();
            if(validatedaddress1 == null)
              validatedaddress1 = IPAddress.Parse("8.8.8.8");
            if(validatedaddress2 == null)
              validatedaddress2 = IPAddress.Parse("8.8.8.8");

            settings[5] = validatedaddress1.ToString();
            settings[6] = validatedaddress2.ToString();
            

            string filepath = Environment.GetEnvironmentVariable("APPDATA") + "\\Pinger\\settings.cfg";
            System.IO.FileInfo file = new System.IO.FileInfo(filepath);
            file.Directory.Create();
            System.IO.File.WriteAllLines(file.FullName, settings, Encoding.UTF8);
           
        }

        private void biggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontsize++;
            label1.Font = new System.Drawing.Font("Arial", (float)fontsize, FontStyle.Regular);
            label2.Font = new System.Drawing.Font("Arial", (float)fontsize, FontStyle.Regular);
            Size = new Size(14 + label1.Size.Width + label2.Size.Width + 10, label1.Height);
            label2.Location = new Point(label1.Location.X + label1.Size.Width, 1);
        }

        private void smallerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontsize--;
            label1.Font = new System.Drawing.Font("Arial", (float)fontsize, FontStyle.Regular);
            label2.Font = new System.Drawing.Font("Arial", (float)fontsize, FontStyle.Regular);
            Size = new Size(14 + label1.Size.Width + label2.Size.Width + 10, label1.Height);
            label2.Location = new Point(label1.Location.X + label1.Size.Width, 1);
        }

        private void backgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(label1.BackColor == Color.FromArgb(64, 64, 64))
            {
                label1.BackColor = Color.Black;
                label2.BackColor = Color.Black;
            }
            else
            {
                label1.BackColor = Color.FromArgb(64,64,64);
                label2.BackColor = Color.FromArgb(64,64,64);
            }
        }

        private void boldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (label1.Font.Style != FontStyle.Bold)
            {
                label1.Font = new System.Drawing.Font("Arial", (float)fontsize, FontStyle.Bold);
                label2.Font = new System.Drawing.Font("Arial", (float)fontsize, FontStyle.Bold);
            }
            else
            {
                label1.Font = new System.Drawing.Font("Arial", (float)fontsize, FontStyle.Regular);
                label2.Font = new System.Drawing.Font("Arial", (float)fontsize, FontStyle.Regular);
            }
        }

        private void resetlocation_Tick(object sender, EventArgs e)
        {
            Size = new Size(14 + label1.Size.Width + label2.Size.Width + 10, label1.Height);
            label2.Location = new Point(label1.Location.X + label1.Size.Width, 1);
            resetlocation.Enabled = false;
        }
    }
}
