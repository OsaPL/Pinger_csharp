using System;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

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
            this.ShowInTaskbar = false;
            pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            pictureBox2.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox2_Paint);
            graphPings1 = new List<Int32>();
            graphPings2 = new List<Int32>();
            barsWidth = 1;
            dotHeight = 1;
            maxValue1 = 1;
            maxValue2 = 1;


            if (System.IO.File.Exists(filepath))  //if cfg file exists
            {
                try
                {
                    System.IO.FileInfo file = new System.IO.FileInfo(filepath);
                    file.Directory.Create(); // if the directory already exists, this method does nothing, just a failsafe
                    string[] settings = System.IO.File.ReadAllLines(file.FullName, Encoding.UTF8);
                    int x = 0, y = 0;
                            y = System.Convert.ToInt32(settings[1]);
                            OwnY.Text = "" + y;
                            x = System.Convert.ToInt32(settings[0]);
                            OwnX.Text = "" + x;
                            Location = new Point(x, y);
                    if (!IsOnScreen())
                        {
                            OwnY.Text = "" + 0;
                            OwnX.Text = "" + 0;
                        y = 0;
                            x = 0;
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
                    this.Opacity = Convert.ToDouble(settings[4]);
                    if (this.Opacity < 15 / 100)
                        this.Opacity = 1;
                    if (this.Opacity > 1)
                        this.Opacity = 1;
                    toolStripTextBox1.Text = "" + this.Opacity*100;
                    lastOpacity = Convert.ToDouble(settings[4]);
                    pingadress1.Text = settings[5];
                    pingadress2.Text = settings[6];
                    int r, g, b;
                    r = Convert.ToInt16(settings[7]);
                    g = Convert.ToInt16(settings[8]);
                    b = Convert.ToInt16(settings[9]);
                    label1.BackColor = Color.FromArgb(r, g, b);
                    label2.BackColor = Color.FromArgb(r, g, b);
                    pictureBox1.BackColor = Color.FromArgb(r, g, b);
                    pictureBox2.BackColor = Color.FromArgb(r, g, b);
                    this.BackColor = Color.FromArgb(r, g, b);
                    graphActivated = Convert.ToBoolean(settings[10]);
                    rightNotBottom = Convert.ToBoolean(settings[11]);
                    timer1.Interval = Convert.ToInt32(settings[12]);
                    rightBottomToolStripMenuItem.PerformClick();
                    rightBottomToolStripMenuItem.PerformClick();
                    //this should be on form load!!
                    refreshsize();
                }

                catch (Exception)
                {
                    defaultValues();
                }
            }
            else
            {
                defaultValues();
            }
            locked = false;
            
            checkipadress();
            refreshsize();

            if (graphActivated) // dirty AF
            {
                graphCheck.PerformClick();
                graphCheck.PerformClick();
            }

        }
        private double Parsestring(String strings)
        {
            try { 
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
            catch (Exception)
            {
                return 0;
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Created by Michał Osowski (Osa__PL)",
            //"About",
            //MessageBoxButtons.OK,
            //MessageBoxIcon.Information);
            debugWindow(sender, e);
        }
        private int w;
        private int h;
        private void leftLowerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            h = Screen.PrimaryScreen.WorkingArea.Bottom;
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
            h = Screen.PrimaryScreen.WorkingArea.Bottom;
            w = Screen.PrimaryScreen.WorkingArea.Right;
            OwnX.Text = "" + (h - Size.Height);
            OwnY.Text = "" + (w - Size.Width);
            Location = new Point(w - Size.Width, h - Size.Height);
        }

        private void rightHigherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            w = Screen.PrimaryScreen.WorkingArea.Right;
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
                    
                    label1.ForeColor = pingColor(ping);
                }
                if (graphActivated == true)
                {
                    if (ping == 0)
                        ping = 1;
                    if (graphPings1.Count > graphLimit - 1)
                    {
                        graphPings1.Insert(graphLimit, (int)ping);
                        graphPings1.RemoveAt(0);
                    }
                    else {
                        int temp = (int)ping;
                        graphPings1.Add(temp);
                    }
                    //change the graphping1 array from {first, ... , last2nd, last1st} to {..., last2nd,last1st, NEWping }
                    //PING1 and PING2 Should use DIFFERENT TABLES!!! otherwise access violation can be caused!  
                }
            }
            catch (Exception e)
            {
                label1.Text = "Unreachable!";
                label1.ForeColor = Color.White;
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


                    label2.ForeColor = pingColor(ping);
                }
                if (graphActivated == true)
                {
                    if (ping == 0)
                        ping = 1;
                    if (graphPings2.Count > graphLimit - 1)
                    {
                        graphPings2.Insert(graphLimit, (int)ping);
                        graphPings2.RemoveAt(0);
                    }
                    else {
                        int temp = (int)ping;
                        graphPings2.Add(temp);
                    }
                    //change the graphping1 array from {first, ... , last2nd, last1st} to {..., last2nd,last1st, NEWping }
                    //PING1 and PING2 Should use DIFFERENT TABLES!!! otherwise access violation can be caused!  
                }
            }
            catch (Exception e)
            { 
                label2.Text = "Unreachable!";
                label2.ForeColor = Color.White;
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
            string[] settings = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            settings[0] = Location.X.ToString();
            settings[1] = Location.Y.ToString();
            settings[2] = fontsize.ToString();
            var cvt = new FontConverter();
            settings[3] = cvt.ConvertToString(label1.Font);
            settings[4] = this.Opacity.ToString();
            if (validatedaddress1 == null)
                validatedaddress1 = IPAddress.Parse("8.8.8.8");
            if (validatedaddress2 == null)
                validatedaddress2 = IPAddress.Parse("8.8.8.8");

            settings[5] = validatedaddress1.ToString();
            settings[6] = validatedaddress2.ToString();
            settings[7] = label1.BackColor.R.ToString();
            settings[8] = label1.BackColor.G.ToString();
            settings[9] = label1.BackColor.B.ToString();
            settings[10] = graphActivated.ToString();
            settings[11] = rightNotBottom.ToString();
            settings[12] = timer1.Interval.ToString();

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
                pictureBox1.BackColor = MyDialog.Color;
                pictureBox2.BackColor = MyDialog.Color;
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
            graphCheck.PerformClick();
            graphCheck.PerformClick();
            refreshsize();
        }
        double lastOpacity;
        private void resetlocation_Tick(object sender, EventArgs e)//one time size refresh to make sure labels are alligned properly
        {
            refreshsize();
            if (IsOnScreen())
            {
                lastFormPos = Location;
            }
            if (!this.ContainsFocus)
            {
                Point p = PointToClient(Control.MousePosition);
                h = Screen.PrimaryScreen.WorkingArea.Bottom;
                w = Screen.PrimaryScreen.WorkingArea.Right;
                p.X -= w/64;
                p.Y -= h/64;
                Rectangle r = new Rectangle (p, new Size(w/32, h/32));
                if (ClientRectangle.IntersectsWith(r) && !mouseDown)
                {
                    if(!locked)
                        this.Opacity = lastOpacity * 0.3;
                }
                else
                {
                    this.Opacity = lastOpacity;
                }
                if (ClientRectangle.IntersectsWith(r) && locked)
                {
                    this.Opacity = 0;
                }
            }
            else
            {
                this.Opacity = lastOpacity;
            }

        }

        // mouse dragging by button
        private bool mouseDown;
        private Point lastPos;
        private Point lastFormPos;
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
                lastFormPos = this.Location;
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
                if (!IsOnScreen())
                {
                    Location = lastFormPos;
                    mouseDown = false;
                    this.Focus();
                }
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
        private bool graphActivated;
        private List<Int32> graphPings1;
        private List<Int32> graphPings2;
        private int graphLimit;
        private void refreshsize() //recalculates form size and label placement
        {
            //button1.Size =new Size (label1.Height,label1.Height);
            
            if (!graphActivated) {
                label2.Location = new Point(label1.Right+1, label1.Location.Y);
                this.BackColor = label1.BackColor; //temporary!

                if (button1.Height > label1.Height)
                    {
                        Size = new Size(126, button1.Height);
                    }
                else
                    {
                        Size = new Size(126, label1.Height);
                    }
                graphCheck.Text = "Graph OFF";
            }
            else
            {
                
                if (rightNotBottom)
                    label2.Location = new Point(pictureBox2.Location.X+1, label1.Location.Y);
                else
                    label2.Location = new Point(label1.Right+1, label1.Location.Y);
                this.BackColor = label1.BackColor;
                graphCheck.Text = "Graph ON";
                maxValue1 = 1;
                maxValue2 = 1;
                for (int k = 0; k < graphPings1.Count; k++)
                {
                    if (graphPings1[k] > maxValue1)
                        maxValue1 = graphPings1[k];
                }
                for (int k = 0; k < graphPings2.Count; k++)
                {
                    if (graphPings2[k] > maxValue2)
                        maxValue2 = graphPings2[k];
                }

                pictureBox1.Invalidate();
                pictureBox2.Invalidate();
            }
        }
        private int maxValue1;
        private int maxValue2;
        private int barsWidth;
        private int dotHeight;
        private bool rightNotBottom;
        private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            // Create a local version of the graphics object for the PictureBox.
            
            Graphics g = e.Graphics;
            Point tempp = pictureBox1.Location;
            tempp = new Point(0, 0);
            SolidBrush sPen = new SolidBrush(Color.FromArgb(64,64,64));
            //g.FillRectangle(sPen, 0, 0, pictureBox1.Width, pictureBox1.Height);
            Color c1 = Color.FromArgb(100, 0, 100);

            float scale = (float)pictureBox1.Height / (float)maxValue1;

            int k;
            for (k = 0; k < graphPings1.Count; k++)
            {
                c1 = pingColor(graphPings1[k]);

                Pen pPen = new Pen(c1);
                pPen.Width = 2.0F;
                float pixelPerV = graphPings1[k] * scale;
                //if (pixelPerV <= 0)
                //    pixelPerV = 1;
                g.DrawLine(pPen, tempp.X + barsWidth * k, pictureBox1.Height - pixelPerV,
                    tempp.X + barsWidth * k, pictureBox1.Height);
                pPen.Color = Color.White;
                g.DrawLine(pPen, tempp.X + barsWidth * k, pictureBox1.Height - pixelPerV - dotHeight,
                    tempp.X + barsWidth * k, pictureBox1.Height - pixelPerV);
            }
            g.DrawString("(1)",
                new Font("Arial", (int)(fontsize / 1.5)), System.Drawing.Brushes.DarkGray, new Point(0, 0));
        }
        private void pictureBox2_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            // Create a local version of the graphics object for the PictureBox.

            Graphics g = e.Graphics;
            Point tempp = pictureBox2.Location;
            tempp = new Point(0, 0);
            SolidBrush sPen = new SolidBrush(Color.FromArgb(64, 64, 64));
            //g.FillRectangle(sPen, 0, 0, pictureBox2.Width, pictureBox2.Height);
            Color c1 = Color.FromArgb(100, 0, 100);

            float scale = (float)pictureBox2.Height / (float)maxValue2;

            int k;
            for (k = 0; k < graphPings2.Count; k++)
            {
                c1 = pingColor(graphPings2[k]);

                Pen pPen = new Pen(c1);
                pPen.Width = 2.0F;
                float pixelPerV = graphPings2[k] * scale;
                //if (pixelPerV <= 0)
                //    pixelPerV = 1;
                g.DrawLine(pPen, tempp.X + barsWidth * k, pictureBox2.Height - pixelPerV,
                    tempp.X + barsWidth * k, pictureBox2.Height);
                pPen.Color = Color.White;
                g.DrawLine(pPen, tempp.X + barsWidth * k, pictureBox2.Height - pixelPerV - dotHeight,
                    tempp.X + barsWidth * k, pictureBox2.Height - pixelPerV);
            }
            g.DrawString("(2)",
                new Font("Arial", (int)(fontsize / 1.5)), System.Drawing.Brushes.DarkGray, new Point(0, 0));
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

        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (toolStripTextBox1.Text == "Too high!" || toolStripTextBox1.Text == "Too low!")
            {
                toolStripTextBox1.Text = "";
            }

            if (e.KeyChar == (char)13)
            {
                String temp = toolStripTextBox1.Text;
                double op = (int)Parsestring(temp);
                if (op <= 100)
                {
                    if (op >= 15)
                    {
                        this.Opacity = op / 100;
                        lastOpacity = op / 100;
                    }
                    else
                    {
                        toolStripTextBox1.Text = "Too low!";
                    }
                }
                else
                {
                    toolStripTextBox1.Text = "Too high!";
                }
            }

        }
        private void toolStripTextBox1_Click(object sender, KeyPressEventArgs e)
        {
            if (toolStripTextBox1.Text == "Too high!" || toolStripTextBox1.Text == "Too low!")
            {
                toolStripTextBox1.Text = "";
            }
        }
        private bool locked;
        private void lockWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (locked)
            {
                System.Media.SystemSounds.Beep.Play();
                this.Opacity = lastOpacity;
                locked = false;
                lockWindowToolStripMenuItem.Text = "Lock";
                lockWindowToolStripMenuItem.Image = null;
                button1.BackColor = Color.FromArgb(64, 64, 64);
            }
            else
            {
                locked = true;
                lockWindowToolStripMenuItem.Text = "Locked";
                Icon i = new Icon(SystemIcons.Warning, 20, 20);
                lockWindowToolStripMenuItem.Image = i.ToBitmap();
                button1.BackColor = Color.FromArgb(210, 0, 0);
            }
        } 
        private void defaultValues()
        {
            OwnX.Text = "" + 0;
            OwnY.Text = "" + 0;
            Location = new Point(0, 0);
            fontsize = 9.75;
            label1.Font = new Font("Arial", (float)fontsize, FontStyle.Regular);
            label2.Font = new Font("Arial", (float)fontsize, FontStyle.Regular);
            this.Opacity = 0.8;
            lastOpacity = 0.8;
            toolStripTextBox1.Text = "" + 80;
            pingadress1.Text = "wp.pl";
            pingadress2.Text = "8.8.8.8";
            label1.BackColor = Color.FromArgb(64, 64, 64);
            label2.BackColor = Color.FromArgb(64, 64, 64);
            graphActivated = false;
            rightNotBottom = true;
            rightBottomToolStripMenuItem.PerformClick();
            rightBottomToolStripMenuItem.PerformClick();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
                this.Hide();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            //this.Focus();
        }

        private void graphCheck_Click(object sender, EventArgs e)
        {
            if (graphActivated)
            {
                graphActivated = false;
                pictureBox1.Hide();
                pictureBox2.Hide();
                graphCheck.Text = "Graph OFF";
            }
            else
            {
                
                pictureBox1.Location = new Point(0, label1.Bottom);
                if (rightNotBottom)
                {
                    pictureBox1.Size = new Size(this.Width/2, pictureBox1.Height);
                    pictureBox2.Size = pictureBox1.Size;
                    pictureBox2.Location = new Point(pictureBox1.Location.X+pictureBox1.Width+2, pictureBox1.Top);
                    Size = new Size(14 + label1.Size.Width + label2.Size.Width + pictureBox2.Size.Width, pictureBox1.Bottom);
                }
                else
                {
                    pictureBox1.Size = new Size(this.Width, pictureBox1.Height);
                    pictureBox2.Size = pictureBox1.Size;
                    pictureBox2.Location = new Point(pictureBox1.Location.X, pictureBox1.Bottom+2);
                    Size = new Size(14 + label1.Size.Width + label2.Size.Width, pictureBox2.Bottom);
                }
                graphActivated = true;
                graphLimit = pictureBox1.Width / barsWidth + 1;
                pictureBox1.Show();
                pictureBox2.Show();
                graphCheck.Text = "Graph ON";
            }
            
        }
        private void debugWindow(object sender, EventArgs e)
        {
            string text = ""
                + sender.ToString() + "\n"
                + e.ToString() + "\n";
            MessageBox.Show(text,
            "About",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        }
        private Color pingColor(long ping)
        {
            //using 2 diffrent functions to create green to yellow to red spectrum for the ranges 25 to 230 ms.
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

        private void rightBottomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightNotBottom) { 
                rightNotBottom = false;
                rightBottomToolStripMenuItem.Text = "Under";
            }
            else { 
                rightNotBottom = true;
                rightBottomToolStripMenuItem.Text = "Side by side";
            }
            graphCheck.PerformClick();
            graphCheck.PerformClick();
        }

        private void barsWidthToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}


