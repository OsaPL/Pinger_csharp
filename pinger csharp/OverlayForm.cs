using System;
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
using System.Net.NetworkInformation;
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
        public bool BytesActivated;
        public bool MoveButton;

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
                settings[14] = BytesActivated.ToString();
                settings[15] = MoveButton.ToString();

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
                    PingInterval = Convert.ToInt32(settings[5]);
                    LabelsNr = Convert.ToInt32(settings[6]);
                    BackColor = Color.FromArgb(Convert.ToInt16(settings[7]), Convert.ToInt16(settings[8]), Convert.ToInt16(settings[9]));
                    GraphActivated = Convert.ToBoolean(settings[10]);
                    BarsWidth = Convert.ToInt32(settings[11]);
                    DotHeight = Convert.ToInt32(settings[12]);
                    BarsSpacing = Convert.ToInt32(settings[13]);
                    BytesActivated = Convert.ToBoolean(settings[14]);
                    MoveButton = Convert.ToBoolean(settings[15]);
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
            BytesActivated = false;
            MoveButton = false;
        }
        public void PrintValues()
        {
            string text = ""
            + Location.ToString() + "\n"
            + LabelsNr.ToString() + "\n"
            + "hit";
            MessageBox.Show(text,
            "About",

            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
        }
    }
    public partial class OverlayForm : Form
    {
        public OverlayForm()
        {
            InitializeComponent();
        }

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
        private List<List<int>> graphPings = new List<List<int>>();
        private List<IPAddress> validatedAdresses = new List<IPAddress>();
        private List<int> maxValue = new List<int>();
        private int GraphLimit = 5;
        private DragButton dragbutton;

        private void OverlayForm_Load(object sender, EventArgs e)
        {
            BackColor = Color.FromArgb(64, 64, 64);
            TransparencyKey = Color.Black;
            TopMost = true;
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            Opacity = 0.6;
            //prepare dragbutton
            dragbutton = new DragButton();
            dragbutton.Show();
            dragbutton.ContextMenuStrip = contextMenuStrip;
            //make overlay not clickable
            int initialStyle = GetWindowLong(Handle, -20);
            SetWindowLong(Handle, -20, initialStyle | 0x80000 | 0x20);

            if (!UsedSettings.LoadSettings())
                UsedSettings.DefaultValues();

            dragbutton.Location = new Point(UsedSettings.Location.X, UsedSettings.Location.Y);
            //UsedSettings.PrintValues();

            LoadValidatedAdresses();
            if (UsedSettings.LabelsNr > 0)
            {
                int number = UsedSettings.LabelsNr;
                UsedSettings.LabelsNr = 0;
                for (int i = 0; number > i; i++)
                {
                    AddNewLabel();
                }
            }
            this.SetStyle(
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint |
            ControlStyles.DoubleBuffer,
            true);           //to avoid flickering (check if it works for lots of graphs!!
            RefreshOverlay();
        }
        public static int RandNumber(int Low, int High)
        {
            Random rndNum = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), System.Globalization.NumberStyles.HexNumber));

            int rnd = rndNum.Next(Low, High);

            return rnd;
        }
        public void RemoveLastLabel()
        {
            if (UsedSettings.LabelsNr < 1)
                return;
            else
            {
                List<Label> lToRemove = new List<Label>();
                foreach (Label label in Controls.OfType<Label>())
                {
                    if (!(label.Name == "bytesRLabel" || label.Name == "bytesSLabel"))
                    {
                        string name = "" + UsedSettings.LabelsNr;

                        if (label.Name == name)
                        {
                            lToRemove.Add(label);
                        }
                    }
                }
                foreach (Label label in lToRemove)
                {
                    Controls.Remove(label);
                    label.Dispose();
                }
                adressesToolStripMenuItem.DropDownItems.RemoveByKey("T" + UsedSettings.LabelsNr);
                UsedSettings.LabelsNr--;
            }
            RefreshOverlay();
        }
        public System.Windows.Forms.Label AddNewLabel()
        {
            System.Windows.Forms.Label label = new System.Windows.Forms.Label();
            this.Controls.Add(label);
            UsedSettings.LabelsNr++;
            ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem()
            {
                Name = "T" + UsedSettings.LabelsNr,
                Text = "(" + UsedSettings.LabelsNr + ")"
            };
            adressesToolStripMenuItem.DropDownItems.Add(item);
            ToolStripTextBox bar = new System.Windows.Forms.ToolStripTextBox()
            {
                Name = "B" + UsedSettings.LabelsNr,
                Text = ""
            };
            bar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.bar_KeyPress);
            if (validatedAdresses.Count < UsedSettings.LabelsNr)
            {
                validatedAdresses.Add(IPAddress.Parse("127.0.0.1"));
                bar.Text = "127.0.0.1";
            }
            else
            {
                bar.Text = validatedAdresses[UsedSettings.LabelsNr - 1].ToString();
            }
            item.DropDownItems.Add(bar);
            label.ForeColor = Color.White;
            label.BackColor = Color.FromArgb(64, 64, 64);
            label.Name = "" + UsedSettings.LabelsNr;
            label.Text = "Ping " + UsedSettings.LabelsNr;
            label.Font = UsedSettings.Font;
            graphPings.Add(new List<int> { });
            maxValue.Add(1);
            // if (graphsActivated)
            //     DrawGraphs();
            RefreshOverlay();
            return label;
        }
        private double widthscale = 8.5;
        private double heightscale = 1.7;
        private void bar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)//on enter press
            {
                ToolStripTextBox textbox = sender as ToolStripTextBox;
                if (textbox != null)
                {
                    int number = Int32.Parse(textbox.Name[1].ToString()) - 1;
                    Thread t = new Thread(() => Checkipadress(number));
                    t.Start();
                }

            }
        }
        private void Checkipadress(int id) //checks if address is valid, without pingin, if yes, convert to IP4/6
        {
            string name = "B" + (id + 1);
            ToolStripItem[] menu = adressesToolStripMenuItem.DropDownItems.Find(name, true);
            name = menu[0].Text;
            menu[0].Text = "Validating!";
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
                    validated = Dns.GetHostAddresses(name)[0];
                    validatedAdresses[id] = validated;
                    menu[0].Text = name;
                }
                catch (Exception)
                {
                    menu[0].Text = "Can't reach!";
                }
            }
        }
        private void throwPing_Tick(object sender, EventArgs e)
        {
            try
            {
                if (UsedSettings.LabelsNr >= 1)
                {
                    foreach (Label label in Controls.OfType<Label>())
                    {
                        if (!(label.Name == "bytesRLabel" || label.Name == "bytesSLabel"))
                        {
                            int number;
                            number = Int32.Parse(label.Name) - 1;
                            Thread t = new Thread(() => pingthread(number));
                            t.Start();
                        }
                    }
                    if (UsedSettings.BytesActivated)
                    {
                        int avgping = averagePing();
                        bytesSLabel.ForeColor = pingColor(avgping);
                        bytesRLabel.ForeColor = bytesSLabel.ForeColor;
                        if (avgping < 25)
                        {
                            netQualityBar.Value = 230;
                        }
                        if (avgping > 230)
                        {
                            netQualityBar.Value = 30;
                        }
                        if (avgping > 25 && avgping < 230)
                        {
                            netQualityBar.Value = 255 - avgping;
                        }
                        netQualityBar.ForeColor = bytesRLabel.ForeColor;
                    }
                }
            }

            catch (Exception er)
            {
            }
        }

        private int averagePing()
        {
            long PingsSum = 0;
            int avgpings;
            int realpings = 0;
            for (int j = 0; j < graphPings.Count; j++)
            {
                for (int k = 0; k < graphPings[j].Count; k++)
                {
                    if (graphPings[j][k] != 0)
                    {
                        PingsSum += graphPings[j][k];
                        realpings++;
                    }
                }
            }
            if (realpings == 0)
                return 0;
            avgpings = (int)(PingsSum / realpings);
            return avgpings;
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
                    label.Text = "(" + (id + 1) + ")" + ping + "ms";

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
                    maxValue[id] = 1; //to make sure it wont try to use old maxvalue that is not in graphpings atm
                    //also this line ^ is dangerous, data race! should be ok tho
                    for (int k = 0; k < graphPings[id].Count; k++)
                    {
                        if (graphPings[id][k] > maxValue[id])
                            maxValue[id] = graphPings[id][k];
                    }
                }
                else
                {
                    graphPings[id].Clear();
                    graphPings[id].Add((int)ping);
                }
            }
            catch (Exception e)
            {
                Label label = this.Controls.Find((id + 1).ToString(), true).FirstOrDefault() as Label;
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
            foreach (Label label in Controls.OfType<Label>())
            {
                if (!(label.Name == "bytesRLabel" || label.Name == "bytesSLabel"))
                {
                    int number = Int32.Parse(label.Name) - 1;
                    Rectangle Canvas = new Rectangle(label.Left + 1, label.Bottom, label.Width - (int)(widthscale / 2), Height - label.Height - 1);
                    DrawGraph(number, e, Canvas); ;
                }
            }
        }
        private void DrawGraph(int number, PaintEventArgs e, Rectangle Canvas)
        {
            try //try catch IS NECESSARY, otherwise for  first paint event graphlimit is 0;
            {
                Graphics g = e.Graphics;
                Pen p = new Pen(Color.FromArgb(RandNumber(0, 255), RandNumber(0, 255), RandNumber(0, 255)));
                float scale = (float)Canvas.Height / (float)maxValue[number];
                Color c;
                //g.DrawRectangle(p, Canvas); //debug canvas
                int k;
                for (k = 0; k < GraphLimit; k++)
                {
                    c = pingColor(graphPings[number][k]);

                    Pen pPen = new Pen(c, UsedSettings.BarsWidth);

                    if (graphPings[number][k] == 1)
                    {
                        g.DrawLine(pPen, Canvas.Left + (UsedSettings.BarsWidth + UsedSettings.BarsSpacing) * k, Canvas.Bottom - Canvas.Height,
                                Canvas.Left + (UsedSettings.BarsWidth + UsedSettings.BarsSpacing) * k, Canvas.Bottom);
                    }
                    else
                    {
                        float pixelPerV = graphPings[number][k] * scale;
                        g.DrawLine(pPen, Canvas.Left + (UsedSettings.BarsWidth + UsedSettings.BarsSpacing) * k, Canvas.Bottom - pixelPerV,
                                Canvas.Left + (UsedSettings.BarsWidth + UsedSettings.BarsSpacing) * k, Canvas.Bottom);
                        pPen.Color = Color.White;
                        if(UsedSettings.DotHeight == 1) //drawline cant draw single pixels
                        {
                            SolidBrush brush = new SolidBrush(pPen.Color);
                            e.Graphics.FillRectangle(brush, Canvas.Left + (UsedSettings.BarsWidth + UsedSettings.BarsSpacing) * k, Canvas.Bottom - pixelPerV -1, 1, 1);
                        }
                        else
                        {
                            g.DrawLine(pPen, Canvas.Left + (UsedSettings.BarsWidth + UsedSettings.BarsSpacing) * k, Canvas.Bottom - pixelPerV,
                                Canvas.Left + (UsedSettings.BarsWidth + UsedSettings.BarsSpacing) * k, Canvas.Bottom - pixelPerV - UsedSettings.DotHeight);
                        }
                    }
                    //g.DrawString("(" + (number + 1) + ")",
                    //new Font("Arial", (int)(Font.SizeInPoints / 1.5)), System.Drawing.Brushes.DarkGray, new Point(Canvas.Left, Canvas.Top));
                }
            }
            catch (Exception er)
            {
            }
        }

        private void refresh_Tick(object sender, EventArgs e)
        {
            Refresh();
            Location = new Point(dragbutton.Location.X - 1, dragbutton.Location.Y);

            int h = Screen.PrimaryScreen.WorkingArea.Bottom;
            int w = Screen.PrimaryScreen.WorkingArea.Right;
            Point p = PointToClient(Control.MousePosition);
            p.X -= w / 64;
            p.Y -= h / 64;
            Rectangle r = new Rectangle(p, new Size(w / 32, h / 32));
            if (Opacity != UsedSettings.Opacity && (!ClientRectangle.IntersectsWith(r) || dragbutton.ContainsFocus))
            {
                if (Opacity <= UsedSettings.Opacity)
                {
                    Opacity += 0.03;
                    if(Opacity > UsedSettings.Opacity)
                        Opacity = UsedSettings.Opacity;
                }
            }
            if (!dragbutton.ContainsFocus)
            {
                if (ClientRectangle.IntersectsWith(r))
                {
                    if(Opacity >= UsedSettings.Opacity * 0.3)
                        Opacity -= 0.03;
                }
            }
            dragbutton.AnchorToCorners(h,w,p,Size);
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
            UsedSettings.Location = dragbutton.Location;
            SaveValidatedAdresses();
            if (!UsedSettings.SaveSettings())
                MessageBox.Show("Cant save",
                "About",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        private bool SaveValidatedAdresses()
        {
            try
            {
                //on close save everything
                string[] settings = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
                //cause Im dumb and also lazy ^ DONT LOOK AT THAT LINE ^
                int i;
                for (i = 0; i < UsedSettings.LabelsNr; i++)
                {
                    settings[i] = validatedAdresses[i].ToString();
                }

                string filepath = Environment.GetEnvironmentVariable("APPDATA") + "\\Pinger\\validatedadresses.dat";
                System.IO.FileInfo file = new System.IO.FileInfo(filepath);
                file.Directory.Create();
                System.IO.File.WriteAllLines(file.FullName, settings, Encoding.UTF8);
                return true;
            }
            catch (Exception e)
            {
                //message zong, cant save
                return false;
            }
        }
        private bool LoadValidatedAdresses()
        {
            try
            {
                string filepath = Environment.GetEnvironmentVariable("APPDATA") + "\\Pinger\\validatedadresses.dat";
                if (System.IO.File.Exists(filepath))  //if cfg file exists
                {
                    System.IO.FileInfo file = new System.IO.FileInfo(filepath);
                    file.Directory.Create(); // if the directory already exists, this method does nothing, just a failsafe
                    string[] settings = System.IO.File.ReadAllLines(file.FullName, Encoding.UTF8);
                    int i = 0;
                    while (settings[i] != "")
                    {
                        validatedAdresses.Add(IPAddress.Parse(settings[i]));
                        i++;
                    }
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

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog1 = new FontDialog();
            fontDialog1.Font = UsedSettings.Font;

            if (fontDialog1.ShowDialog() != DialogResult.Cancel)
            {
                UsedSettings.Font = fontDialog1.Font;
                RefreshOverlay();
            }
        }

        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = true;
            MyDialog.Color = UsedSettings.BackColor;

            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                UsedSettings.BackColor = MyDialog.Color;
                RefreshOverlay();
            }
        }
        private void RefreshOverlay()
        {
            try
            {
                Label last = null;
                foreach (Label label in Controls.OfType<Label>())
                {
                    if (!(label.Name == "bytesRLabel" || label.Name == "bytesSLabel"))
                    {
                        label.Font = UsedSettings.Font;
                        label.Size = new Size((int)(label.Font.SizeInPoints * widthscale), (int)(label.Font.SizeInPoints * heightscale));
                        label.BackColor = UsedSettings.BackColor;
                        label.Size = new Size((int)(label.Font.SizeInPoints * widthscale), (int)(label.Font.SizeInPoints * heightscale));
                        if (last == null)
                        {
                            label.Size = new Size((int)(label.Font.SizeInPoints * widthscale), (int)(label.Font.SizeInPoints * heightscale));
                            label.Location = new Point((Int32.Parse(label.Name) - 1) * label.Width - 1, 0);
                        }
                        else
                        {
                            label.Size = last.Size;
                            label.Location = new Point(last.Right, 0);
                        }
                        last = label;
                    }
                }
                last = this.Controls.Find((UsedSettings.LabelsNr).ToString(), true).FirstOrDefault() as Label;
                if (!UsedSettings.GraphActivated)
                    Size = new Size(last.Right, last.Height);
                else
                    Size = new Size(last.Right, last.Height * 3);
                throwPing.Interval = UsedSettings.PingInterval;
            if (UsedSettings.BackColor == Color.FromArgb(255,0,0,0))
                dragbutton.SetButtonColor(Color.FromArgb(64, 64, 64));
            else
                dragbutton.SetButtonColor(UsedSettings.BackColor);
            BackColor = UsedSettings.BackColor;
            GraphLimit = ((last.Width - (int)(widthscale / 2)) - 1) / (UsedSettings.BarsWidth + UsedSettings.BarsSpacing);
            barsWidthTextBox.Text = "" + UsedSettings.BarsWidth;
            barsSpacingTextBox.Text = "" + UsedSettings.BarsSpacing;
            dotsHeightTextBox.Text = "" + UsedSettings.DotHeight;
            if (UsedSettings.GraphActivated)
            {
                graphsToggleToolStripMenuItem.Text = "Graphs ON";
                graphsToggleToolStripMenuItem.BackColor = Color.FromArgb(150, 210, 150);
            }
            else
            {
                graphsToggleToolStripMenuItem.Text = "Graphs OFF";
                graphsToggleToolStripMenuItem.BackColor = Color.White;
            }


            if (UsedSettings.BytesActivated)
            {
                bytesSLabel.BackColor = UsedSettings.BackColor;
                bytesSLabel.Location = new Point(last.Right, 0);
                bytesSLabel.Text = "Sent ";
                bytesSLabel.Font = UsedSettings.Font;
                bytesSLabel.Size = new Size((int)(bytesSLabel.Font.SizeInPoints * widthscale), (int)(bytesSLabel.Font.SizeInPoints * heightscale));

                bytesRLabel.BackColor = UsedSettings.BackColor;
                bytesRLabel.Size = bytesSLabel.Size;
                bytesRLabel.Text = "Received ";
                bytesRLabel.Font = UsedSettings.Font;
                if (UsedSettings.GraphActivated)
                {
                    bytesRLabel.Location = new Point(last.Right, bytesSLabel.Bottom);
                    Size = new Size((int)(Size.Width + bytesSLabel.Size.Width * widthscale / 3.5), Size.Height);
                }
                else
                {
                    bytesRLabel.Location = new Point(bytesRLabel.Right, 0);
                    Size = new Size((int)(Size.Width + (bytesSLabel.Size.Width * widthscale / 3.5) * 2), Size.Height);
                }
            }
            if (UsedSettings.BytesActivated)
            {
                netQualityBar.Location = new Point(last.Right, bytesRLabel.Bottom);
                netQualityBar.Size = new Size(Size.Width - netQualityBar.Location.X - 1, Size.Height - bytesRLabel.Bottom - 1);
                netQualityBar.Show();
                netQualityBar.ForeColor = UsedSettings.BackColor;
                transferToolStripMenuItem.Text = "Transfer ON";
                transferToolStripMenuItem.BackColor = Color.FromArgb(150, 210, 150);
            }
            else
            {
                netQualityBar.Hide();
                transferToolStripMenuItem.Text = "Transfer OFF";
                transferToolStripMenuItem.BackColor = Color.White;
            }
            throwPing.Interval = UsedSettings.PingInterval;
            Opacity = UsedSettings.Opacity;
            dragbutton.Opacity = 0.1;
            dragbutton.SetButtonSize(last.Width,last.Height);
            opacityTextBox.Text = "" + Opacity*100 + "%";
            intervalStripTextBox.Text = "" + UsedSettings.PingInterval + "ms";
                //Size = new Size (Size.Width*UsedSettings.SizeMlt,Size.Height*UsedSettings.SizeMlt); //need things other than just this to scale overlay
                //UsedSettings.SizeMlt = 1;
                if (UsedSettings.MoveButton)
                {
                    dragbutton.Show();
                    moveToolStripMenuItem.Text = "Lock OFF";
                    moveToolStripMenuItem.BackColor = Color.White;
                }
                else
                {
                    dragbutton.Hide();
                    moveToolStripMenuItem.Text = "Lock ON";
                    moveToolStripMenuItem.BackColor = Color.FromArgb(150, 210, 150);
                }
            }
            catch(Exception er)
            {
                UsedSettings.DefaultValues();
            }
        }

        private void dotsHeightTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)//on enter press
            {
                try
                {
                    UsedSettings.DotHeight = Int32.Parse(dotsHeightTextBox.Text);
                    if (UsedSettings.DotHeight > 10)
                    {
                        UsedSettings.DotHeight = 10;
                    }
                    if (UsedSettings.DotHeight <= 0)
                    {
                        UsedSettings.DotHeight = 0;
                    }
                    RefreshOverlay();
                }
                catch (Exception er)
                {
                    dotsHeightTextBox.Text = "Wrong value!";
                }
            }
        }
        private void barsWidthTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)//on enter press
            {
                try
                {
                    UsedSettings.BarsWidth = Int32.Parse(barsWidthTextBox.Text);
                    if (UsedSettings.BarsWidth > 10)
                    {
                        UsedSettings.BarsWidth = 10;
                    }
                    if (UsedSettings.BarsWidth <= 1)
                    {
                        UsedSettings.BarsWidth = 1;
                    }
                    RefreshOverlay();
                }
                catch (Exception er)
                {
                    barsWidthTextBox.Text = "Wrong value!";
                }
            }
        }
        private void barsSpacingTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)//on enter press
            {
                try
                {
                    UsedSettings.BarsSpacing = Int32.Parse(barsSpacingTextBox.Text);
                    if (UsedSettings.BarsSpacing > 10)
                    {
                        UsedSettings.BarsSpacing = 10;
                    }
                    if (UsedSettings.BarsSpacing <= 0)
                    {
                        UsedSettings.BarsSpacing = 0;
                    }
                }
                catch (Exception er)
                {
                    barsSpacingTextBox.Text = "Wrong value!";
                }
                RefreshOverlay();
            }
        }

        private void graphsToggleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UsedSettings.GraphActivated)
            {
                System.Media.SystemSounds.Beep.Play();
                UsedSettings.GraphActivated = false;
            }
            else
            {
                UsedSettings.GraphActivated = true;
                System.Media.SystemSounds.Beep.Play();
            }
            RefreshOverlay();
        }

        private long startBytesReceived;
        private long startBytesSent;
        private void netBytes()
        {
            if (startBytesSent == 0 && startBytesReceived == 0)
            {
                if (!NetworkInterface.GetIsNetworkAvailable())
                    return;

                NetworkInterface[] interfaces
                    = NetworkInterface.GetAllNetworkInterfaces();
                startBytesReceived = 0;
                startBytesSent = 0;
                foreach (NetworkInterface ni in interfaces)
                {
                    startBytesSent += ni.GetIPv4Statistics().BytesSent;
                    startBytesReceived += ni.GetIPv4Statistics().BytesReceived;
                }
            }
            else
            {
                double valueR = 0, valueS = 0;
                long tempR = 0, tempS = 0;
                if (!NetworkInterface.GetIsNetworkAvailable())
                    return;

                NetworkInterface[] interfaces
                    = NetworkInterface.GetAllNetworkInterfaces();

                foreach (NetworkInterface ni in interfaces)
                {
                    tempR += ni.GetIPv4Statistics().BytesReceived;
                    tempS += ni.GetIPv4Statistics().BytesSent;
                }
                valueR = tempR - startBytesReceived;
                valueS = tempS - startBytesSent;
                startBytesReceived = tempR;
                startBytesSent = tempS;
                if (valueR > 1024)
                {
                    valueR /= 1024;
                    if (valueR > 1024)
                    {
                        valueR /= 1024;
                        bytesRLabel.Text = "↓ " + Math.Round(valueR, 2) + " MB/s";
                    }
                    else
                    {
                        bytesRLabel.Text = "↓ " + Math.Round(valueR, 2) + " KB/s";
                    }

                }
                else
                {
                    bytesRLabel.Text = "↓ " + valueR + " B/s";
                }

                if (valueS > 1024)
                {
                    valueS /= 1024;
                    if (valueS > 1024)
                    {
                        valueS /= 1024;
                        bytesSLabel.Text = "↑ " + Math.Round(valueS, 2) + " MB/s";
                    }
                    else
                    {
                        bytesSLabel.Text = "↑ " + Math.Round(valueS, 2) + " KB/s";
                    }
                }
                else
                {
                    bytesSLabel.Text = "↑ " + valueS + " B/s";
                }

            }
        }

        private void bytesTimer_Tick(object sender, EventArgs e)
        {
            if (UsedSettings.BytesActivated)
            {
                netBytes();
                //temporary workaround to the placing of the rlabel when graphs off, fix it!
                if (!UsedSettings.GraphActivated)
                {
                    bytesRLabel.Location = new Point(bytesSLabel.Right, 0);
                    Width = bytesRLabel.Right;
                }
            }

        }

        private void transferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UsedSettings.BytesActivated)
            {
                System.Media.SystemSounds.Beep.Play();
                UsedSettings.BytesActivated = false;
            }
            else
            {
                UsedSettings.BytesActivated = true;
                System.Media.SystemSounds.Beep.Play();
            }
            RefreshOverlay();
        }

        private void intervalStripTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)//on enter press
            {
                if (intervalStripTextBox.Text.Contains("ms"))
                {
                    intervalStripTextBox.Text = intervalStripTextBox.Text.Replace("ms", "");
                }
                int i = 0;
                if (Int32.TryParse(intervalStripTextBox.Text, out i))
                {
                    if (i < 50)
                    {
                        intervalStripTextBox.Text = "Wrong value!";
                    }
                    else
                    {
                        UsedSettings.PingInterval = i;
                        RefreshOverlay();
                    }
                }
                else
                {
                    intervalStripTextBox.Text = "Wrong value!";
                }
            }
            else
            {
                wrongValue_Click(sender, null);
            }
            if (!intervalStripTextBox.Text.Contains("ms") && intervalStripTextBox.Text != "Wrong value!")
            {
                int t = intervalStripTextBox.SelectionStart;
                intervalStripTextBox.Text += "ms";
                intervalStripTextBox.SelectionStart = t;
            }
        }

        private void opacityTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)//on enter press
            {
                if (opacityTextBox.Text.Contains("%"))
                {
                    opacityTextBox.Text = opacityTextBox.Text.Replace("%", "");
                }
                double i = 0;
                if (Double.TryParse(opacityTextBox.Text, out i) )
                {
                    i /= 100;
                    if (i < 0.2 || i > 1)
                    {
                        opacityTextBox.Text = "Wrong value!";
                    }
                    else
                    {
                        UsedSettings.Opacity = i;
                        RefreshOverlay();
                    }
                }
                else
                {
                    opacityTextBox.Text = "Wrong value!";
                }
            }
            else
            {
                wrongValue_Click(sender, null);
            }
            if (!opacityTextBox.Text.Contains("%") && opacityTextBox.Text != "Wrong value!")
            {
                int t = opacityTextBox.SelectionStart;
                opacityTextBox.Text += "%";
                opacityTextBox.SelectionStart = t;
            }
        }
        private void wrongValue_Click(object sender, EventArgs e)
        {
            if ((sender as ToolStripTextBox).Text == "Wrong value!")
            {
                (sender as ToolStripTextBox).Text = "";
            }

        }

        private void moveToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (UsedSettings.MoveButton)
            {
                UsedSettings.MoveButton = false;
            }
            else
            {
                UsedSettings.MoveButton = true;
            }
            RefreshOverlay();
        }
    }
}


