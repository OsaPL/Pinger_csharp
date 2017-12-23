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
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Net.Sockets;
using System.Security.Principal;
using System.Linq.Expressions;
using System.Reflection;

namespace pinger_csharp
{
    public struct Settings
    {
        public Point Location;
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
        public bool AutoPing;

        public bool SaveSettings()
        {
            try
            {
                //on close save everything
                List<string> settings = new List<string>();
                settings.Add("[X]=" + Location.X.ToString());
                settings.Add("[Y]=" + Location.Y.ToString());
                var cvt = new FontConverter();
                settings.Add("[Font]=" + cvt.ConvertToString(Font));
                settings.Add("[Opacity]=" + Opacity.ToString());
                settings.Add("[Interval]=" + PingInterval.ToString());
                settings.Add("[Labels]=" + LabelsNr.ToString());
                settings.Add("[BackgroundR]=" + BackColor.R.ToString());
                settings.Add("[BackgroundG]=" + BackColor.G.ToString());
                settings.Add("[BackgroundB]=" + BackColor.B.ToString());
                settings.Add("[Graphs]=" + GraphActivated.ToString());
                settings.Add("[BarsWidth]=" + BarsWidth.ToString());
                settings.Add("[DotHeight]=" + DotHeight.ToString());
                settings.Add("[BarsSpacing]=" + BarsSpacing.ToString());
                settings.Add("[DataTraffic]=" + BytesActivated.ToString());
                settings.Add("[Movable]=" + MoveButton.ToString());
                settings.Add("[AutoPing]=" + AutoPing.ToString());

                string filepath = Environment.GetEnvironmentVariable("APPDATA") + "\\Pinger\\settings.cfg";
                System.IO.FileInfo file = new System.IO.FileInfo(filepath);
                file.Directory.Create();
                System.IO.File.WriteAllLines(file.FullName, settings.ToArray(), Encoding.UTF8);
                return true;
            }
            catch (Exception e)
            {
                //message zonk, cant save
                return false;
            }

        }
        public static string GetValue(string cfgline)
        {
            int index = cfgline.IndexOf("=") + 1;

            return cfgline.Substring(index, cfgline.Length - index);
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
                    Location = new Point(System.Convert.ToInt32(GetValue(settings[0])), System.Convert.ToInt32(GetValue(settings[1])));
                    var cvt = new FontConverter();
                    Font = cvt.ConvertFromString(GetValue(settings[2])) as Font;
                    Opacity = Convert.ToDouble(GetValue(settings[3]));
                    PingInterval = Convert.ToInt32(GetValue(settings[4]));
                    LabelsNr = Convert.ToInt32(GetValue(settings[5]));
                    BackColor = Color.FromArgb(Convert.ToInt16(GetValue(settings[6])), Convert.ToInt16(GetValue(settings[7])), Convert.ToInt16(GetValue(settings[8])));
                    GraphActivated = Convert.ToBoolean(GetValue(settings[9]));
                    BarsWidth = Convert.ToInt32(GetValue(settings[10]));
                    DotHeight = Convert.ToInt32(GetValue(settings[11]));
                    BarsSpacing = Convert.ToInt32(GetValue(settings[12]));
                    BytesActivated = Convert.ToBoolean(GetValue(settings[13]));
                    MoveButton = Convert.ToBoolean(GetValue(settings[14]));
                    AutoPing = Convert.ToBoolean(GetValue(settings[15]));
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
            Font = new Font("Consolas", (float)8.75);
            Opacity = 0.8;
            PingInterval = 300;
            LabelsNr = 1;
            BackColor = Color.FromArgb(64, 64, 64);
            GraphActivated = false;
            BarsWidth = 1;
            DotHeight = 1;
            BarsSpacing = 0;
            BytesActivated = false;
            MoveButton = true;
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

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        static readonly IntPtr HWND_TOP = new IntPtr(0);
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

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
            //not vivible for time of resizing
            Visible = false;

            BackColor = Color.FromArgb(64, 64, 64);
            TransparencyKey = Color.Black;
            TopMost = true;
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            Opacity = 0.6;
            //prepare dragbutton
            dragbutton = new DragButton();
            dragbutton.Show();
            dragbutton.Size = new Size(0, 0);
            dragbutton.ContextMenuStrip = contextMenuStrip;
            //make overlay not clickable
            int initialStyle = GetWindowLong(Handle, -20);
            SetWindowLong(Handle, -20, initialStyle | 0x80000 | 0x20);
            //try to make it always on top, experimental
            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);

            string filepath = Environment.GetEnvironmentVariable("APPDATA");
            if (File.Exists(filepath + "\\Pinger\\logback.txt"))
                File.Delete(filepath + "\\Pinger\\logback.txt");
            if (File.Exists(filepath + "\\Pinger\\log.txt"))
                File.Move(filepath + "\\Pinger\\log.txt", filepath + "\\Pinger\\logback.txt");

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

            //and visible again
            Visible = true;
        }
        public static int RandNumber(int Low, int High)
        {
            Random rndNum = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), System.Globalization.NumberStyles.HexNumber));

            int rnd = rndNum.Next(Low, High);

            return rnd;
        }
        public void RemoveLastLabel()
        {

            if (UsedSettings.LabelsNr <= 1)
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
            bar.LostFocus += Bar_LostFocus;
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

        private void Bar_LostFocus(object sender, EventArgs e)
        {
            ToolStripTextBox textbox = sender as ToolStripTextBox;
            if (textbox != null)
            {
                int number = Int32.Parse(textbox.Name[1].ToString()) - 1;
                Thread t = new Thread(() => Checkipadress(number));
                t.Start();
                graphPings[number].Clear();
            }
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
                    graphPings[number].Clear();
                }
            }
        }
        private void Checkipadress(int id) //checks if address is valid, without pingin, if yes, convert to IP4/6
        {
            try
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
                Log("(" + (id + 1) + ")" + menu[0].Text);
            }
            catch (Exception e)
            { }

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
                    LogPings();
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
                    if (graphPings[j][k] >= 0)
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

        private string FormatPingText(long ping, int id)
        {
            string txt = String.Empty;
            if (UsedSettings.AutoPing && id == UsedSettings.LabelsNr - 1)
            {
                txt += "(A)";
            }
            else
            {
                txt += "(" + (id + 1) + ")";
            }
            if (ping < 1)
            {
                txt += "<1ms";
            }
            else
            {
                txt += ping + "ms";
            }
            return txt;
        }
        private void InvokeUI<T>(Control control, object field, object value)
        {
            control.Invoke((MethodInvoker)(() => field = value));
        }

        private void pingthread(int id)
        {
            //add invoking methods to ensure thread safeness
            try
            {
                Label label = this.Controls.Find((id + 1).ToString(), true).FirstOrDefault() as Label;
                Ping pingClass = new Ping();
                string usedip = validatedAdresses[id].ToString();
                PingReply pingReply = pingClass.Send(validatedAdresses[id].ToString());
                long ping = pingReply.RoundtripTime;

                if (usedip != validatedAdresses[id].ToString()) //if ip change before ping was able to finish
                    return;


                //if we wanna show process name if autoping
                if (UsedSettings.AutoPing)
                {
                    if (id == UsedSettings.LabelsNr - 1)
                    {
                        if (timeout < 2000 / activeProcessTimer.Interval)
                        {
                            //label.ForeColor = Color.Aqua;
                            label.Invoke((MethodInvoker)(() => label.ForeColor = Color.Aqua));
                            return;
                        }
                        else if (usedip == "127.0.0.1")
                        {
                            //label.ForeColor = Color.Aqua;
                            label.Invoke((MethodInvoker)(() => label.ForeColor = Color.Aqua));
                            label.Text = "No IP";
                            return;
                        }
                    }

                }

                lock (label)
                {
                    if (pingReply.Status != IPStatus.Success)
                    {
                        label.Invoke((MethodInvoker)(() => label.Text = "Timeout!"));
                        label.Invoke((MethodInvoker)(() => label.ForeColor = Color.White));
                        //label.Text = "Timeout!";
                        //label.ForeColor = Color.White;
                    }
                    else
                    {
                        //TODO: All ui methods should make invokes if they are in a thread!!
                        label.Invoke((MethodInvoker)(() => label.Text = FormatPingText(ping, id)));
                        //InvokeUI(label, label.Text, FormatPingText(ping, id));    //why it is not working??
                        //label.Text = FormatPingText(ping, id);    //should be invoke'd
                        label.Invoke((MethodInvoker)(() => label.ForeColor = pingColor(ping)));
                        //label.ForeColor = pingColor(ping);
                    }
                }

                if (UsedSettings.GraphActivated == true)
                {
                    //if (ping == 0)
                    //   ping = -1;
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
                //label.Text = e.ToString();//"Unreachable!";
                label.Invoke((MethodInvoker)(() => label.ForeColor = Color.White));
                //label.ForeColor = Color.White;
            }
        }
        private Color pingColor(long ping) //using 2 diffrent functions to create green to yellow to red spectrum for the ranges 25 to 230 ms.
        {
            if (ping == -1)
            {
                return Color.DarkOrange;
            }
            if (ping <= 1)
            {
                return Color.Aqua;
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
                        if (UsedSettings.DotHeight == 1) //drawline cant draw single pixels
                        {
                            SolidBrush brush = new SolidBrush(pPen.Color);
                            e.Graphics.FillRectangle(brush, Canvas.Left + (UsedSettings.BarsWidth + UsedSettings.BarsSpacing) * k, Canvas.Bottom - pixelPerV - 1, 1, 1);
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
            //Makes sure that dragbutton is in the correct place
            Location = new Point(dragbutton.Location.X - 1, dragbutton.Location.Y);

            //Get actual form screen and use it to enable FullScreen
            Screen screen = Screen.FromControl(this);

            int h = screen.WorkingArea.Bottom;
            int w = screen.WorkingArea.Right;
            Point p = PointToClient(Control.MousePosition);
            p.X -= w / 64;
            p.Y -= h / 64;
            Rectangle r = new Rectangle(p, new Size(w / 32, h / 32));

            //Check if mouse if close enough, if yes, fade out, if not fade back in
            if (Opacity != UsedSettings.Opacity && (!ClientRectangle.IntersectsWith(r) || dragbutton.ContainsFocus))
            {
                if (Opacity <= UsedSettings.Opacity)
                {
                    Opacity += 0.03;
                    if (Opacity > UsedSettings.Opacity)
                        Opacity = UsedSettings.Opacity;
                }
            }
            if (!dragbutton.ContainsFocus)
            {
                if (ClientRectangle.IntersectsWith(r))
                {
                    if (Opacity >= UsedSettings.Opacity * 0.3)
                        Opacity -= 0.03;
                }
            }
            //Anchors to a corner
            dragbutton.AnchorToCorners(h, w, Size, screen);
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Adds new label
            bool autoping = false;
            if (UsedSettings.AutoPing)
            {
                ToggleAuto();
                autoping = true;
            }

            AddNewLabel();

            if (autoping)
            {
                ToggleAuto();
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Remove label
            bool autoping = false;
            if (UsedSettings.AutoPing)
            {
                ToggleAuto();
                autoping = true;
            }

            RemoveLastLabel();

            if (autoping)
            {
                ToggleAuto();
            }
        }

        private void OverlayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Save settings on closing
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
                //On close save everything
                List<string> adresses = new List<string>();
                //cause Im dumb and also lazy ^ DONT LOOK AT THAT LINE ^
                int i = 1;
                foreach (IPAddress ip in validatedAdresses)
                {
                    adresses.Add("[" + i + "]=" + ip.ToString());
                    i++;
                }

                string filepath = Environment.GetEnvironmentVariable("APPDATA") + "\\Pinger\\validatedadresses.dat";
                System.IO.FileInfo file = new System.IO.FileInfo(filepath);
                file.Directory.Create();
                System.IO.File.WriteAllLines(file.FullName, adresses.ToArray(), Encoding.UTF8);
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
                    List<string> adresses = System.IO.File.ReadAllLines(file.FullName, Encoding.UTF8).ToList<string>();

                    for (int i = 0; i < adresses.Count; i++)
                    {
                        string ip = Settings.GetValue(adresses[i]).ToString();
                        validatedAdresses.Add(IPAddress.Parse(ip));
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
            //Change font
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
            //Change background color
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
            //Refreshes whole overlay UI
            try
            {
                if (UsedSettings.LabelsNr == 1)
                    removeToolStripMenuItem.ForeColor = Color.Gray;
                else
                    removeToolStripMenuItem.ForeColor = SystemColors.ControlText;
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
                if (UsedSettings.BackColor == Color.FromArgb(255, 0, 0, 0))
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
                    graphsToggleToolStripMenuItem.BackColor = SystemColors.Control;
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
                    transferToolStripMenuItem.BackColor = SystemColors.Control;
                }
                throwPing.Interval = UsedSettings.PingInterval;
                Opacity = UsedSettings.Opacity;
                dragbutton.Opacity = 0.1;
                dragbutton.SetButtonSize(last.Width, last.Height);
                opacityTextBox.Text = "" + Opacity * 100 + "%";
                intervalStripTextBox.Text = "" + UsedSettings.PingInterval + "ms";

                if (UsedSettings.MoveButton)
                {
                    dragbutton.Show();
                    moveToolStripMenuItem.Text = "Lock OFF";
                    moveToolStripMenuItem.BackColor = SystemColors.Control;
                }
                else
                {
                    dragbutton.Hide();
                    moveToolStripMenuItem.Text = "Lock ON";
                    moveToolStripMenuItem.BackColor = Color.FromArgb(150, 210, 150);
                }
            }
            catch (Exception er)
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
                UsedSettings.GraphActivated = false;
            }
            else
            {
                UsedSettings.GraphActivated = true;
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
            //Gets traffic every second
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
                UsedSettings.BytesActivated = false;
            }
            else
            {
                UsedSettings.BytesActivated = true;
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
                if (Double.TryParse(opacityTextBox.Text, out i))
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
        private void LogPings()
        {
            string ting = "";
            string tmp = "";
            string repl = "";
            foreach (Label label in Controls.OfType<Label>())
            {
                if (!(label.Name == "bytesRLabel" || label.Name == "bytesSLabel"))
                {
                    int number = Int32.Parse(label.Name);
                    tmp = label.Text;
                    repl = "(" + number + ")";
                    tmp = tmp.Replace(repl, "");

                    repl = "ms";
                    tmp = tmp.Replace(repl, "");

                    ting += " " + tmp;
                }
            }
            Log(ting);
        }

        private static void Log(string logMessage)
        {
            string filepath = Environment.GetEnvironmentVariable("APPDATA") + "\\Pinger\\log.txt";  //using "using" to ensure no memory leaks while writing
            using (StreamWriter w = File.AppendText(filepath))
            {
                w.WriteLine("[{0}] {1} - {2}",
                DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString(), logMessage);
            }
        }

        private void notifyTrayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            //if they somehow lose frontness, a click on icon fixes that
            this.Activate();
            this.BringToFront();

            if (UsedSettings.MoveButton)
            {
                dragbutton.Activate();
                dragbutton.BringToFront();
            }
        }

        #region autoIpdetecion
        #region getprocess
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        string GetActiveProcessFileName()
        {
            try
            {
                IntPtr hwnd = GetForegroundWindow();
                uint pid;
                GetWindowThreadProcessId(hwnd, out pid);
                Process p = Process.GetProcessById((int)pid);
                return p.MainModule.FileName;
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }
        uint GetActiveProcessId()
        {
            try
            {
                IntPtr hwnd = GetForegroundWindow();
                uint pid;
                GetWindowThreadProcessId(hwnd, out pid);
                return pid;
            }
            catch (Exception ex)
            {
                return 9999;
            }
        }
        #endregion
        #region getbestinterface
        [DllImport("iphlpapi.dll", CharSet = CharSet.Auto)]
        public static extern int GetBestInterface(UInt32 destAddr, out UInt32 bestIfIndex);

        private string FindBestInterface()
        {
            try
            {
                IPAddress ipv4Address = new IPAddress(134744072); //its 8.8.8.8
                UInt32 ipv4AddressAsUInt32 = BitConverter.ToUInt32(ipv4Address.GetAddressBytes(), 0);
                UInt32 index;
                GetBestInterface(ipv4AddressAsUInt32, out index);
                string ipstr = String.Empty;
                foreach (UnicastIPAddressInformation ip in GetNetworkInterfaceByIndex(index).GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        ipstr = ip.Address.ToString();
                    }
                }

                return ipstr;
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }
        private static NetworkInterface GetNetworkInterfaceByIndex(uint index)
        {
            // Search in all network interfaces that support IPv4.
            NetworkInterface ipv4Interface = (from thisInterface in NetworkInterface.GetAllNetworkInterfaces()
                                              where thisInterface.Supports(NetworkInterfaceComponent.IPv4)
                                              let ipv4Properties = thisInterface.GetIPProperties().GetIPv4Properties()
                                              where ipv4Properties != null && ipv4Properties.Index == index
                                              select thisInterface).SingleOrDefault();
            if (ipv4Interface != null)
                return ipv4Interface;

            // Search in all network interfaces that support IPv6.
            NetworkInterface ipv6Interface = (from thisInterface in NetworkInterface.GetAllNetworkInterfaces()
                                              where thisInterface.Supports(NetworkInterfaceComponent.IPv6)
                                              let ipv6Properties = thisInterface.GetIPProperties().GetIPv6Properties()
                                              where ipv6Properties != null && ipv6Properties.Index == index
                                              select thisInterface).SingleOrDefault();

            return ipv6Interface;
        }
        #endregion
        #region packetshiffer
        private Socket mainSocket;                          //The socket which captures all incoming packets
        private byte[] byteData = new byte[8192];
        private bool continueCapturing = false;
        private List<Packet> Packets = new List<Packet>();
        public void ToggleSniffing()
        {
            try
            {
                if (!continueCapturing)
                {
                    //Start capturing the packets...

                    continueCapturing = true;

                    //For sniffing the socket to capture the packets has to be a raw socket, with the
                    //address family being of type internetwork, and protocol being IP
                    mainSocket = new Socket(AddressFamily.InterNetwork,
                        SocketType.Raw, ProtocolType.IP);

                    //Bind the socket to the selected IP address
                    mainSocket.Bind(new IPEndPoint(IPAddress.Parse(bestIp), 0));

                    //Set the socket  options
                    mainSocket.SetSocketOption(SocketOptionLevel.IP,            //Applies only to IP packets
                                               SocketOptionName.HeaderIncluded, //Set the include the header
                                               true);                           //option to true

                    byte[] byTrue = new byte[4] { 1, 0, 0, 0 };
                    byte[] byOut = new byte[4] { 1, 0, 0, 0 }; //Capture outgoing packets

                    //Socket.IOControl is analogous to the WSAIoctl method of Winsock 2
                    mainSocket.IOControl(IOControlCode.ReceiveAll,              //Equivalent to SIO_RCVALL constant
                                                                                //of Winsock 2
                                         byTrue,
                                         byOut);

                    //Start receiving the packets asynchronously
                    mainSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None,
                        new AsyncCallback(OnReceive), null);
                }
                else
                {
                    continueCapturing = false;
                    //To stop capturing the packets close the socket
                    mainSocket.Close();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "sniff", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                int nReceived = mainSocket.EndReceive(ar);

                //Analyze the bytes received...

                ParseData(byteData, nReceived);

                if (continueCapturing)
                {
                    byteData = new byte[8192];

                    //Another call to BeginReceive so that we continue to receive the incoming
                    //packets
                    mainSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None,
                        new AsyncCallback(OnReceive), null);
                }
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "OnReceive", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ParseData(byte[] byteData, int nReceived)
        {
            try
            {
                //Since all protocol packets are encapsulated in the IP datagram
                //so we start by parsing the IP header and see what protocol data
                //is being carried by it
                IPHeader ipHeader = new IPHeader(byteData, nReceived);

                //Now according to the protocol being carried by the IP datagram we parse 
                //the data field of the datagram
                switch (ipHeader.ProtocolType)
                {
                    case Protocol.TCP:

                        TCPHeader tcpHeader = new TCPHeader(ipHeader.Data,              //IPHeader.Data stores the data being 
                                                                                        //carried by the IP datagram
                                                            ipHeader.MessageLength);//Length of the data field                    


                        //If the port is equal to 53 then the underlying protocol is DNS
                        //Note: DNS can use either TCP or UDP thats why the check is done twice
                        if (tcpHeader.DestinationPort == "53" || tcpHeader.SourcePort == "53")
                        {

                        }
                        newPacketParse(new Packet(ipHeader, tcpHeader));
                        break;

                    case Protocol.UDP:

                        UDPHeader udpHeader = new UDPHeader(ipHeader.Data,              //IPHeader.Data stores the data being 
                                                                                        //carried by the IP datagram
                                                           (int)ipHeader.MessageLength);//Length of the data field                    

                        //If the port is equal to 53 then the underlying protocol is DNS
                        //Note: DNS can use either TCP or UDP thats why the check is done twice
                        if (udpHeader.DestinationPort == "53" || udpHeader.SourcePort == "53")
                        {
                            //Length of UDP header is always eight bytes so we subtract that out of the total 
                            //length to find the length of the data
                        }
                        newPacketParse(new Packet(ipHeader, udpHeader));
                        break;

                    case Protocol.Unknown:
                        break;

                        //Thread safe adding of the packets!!
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "parseData", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        Object packetparselock = new Object();
        Object packetslock = new Object();
        private void newPacketParse(Packet packet)
        {
            List<Packet> copy = new List<Packet>();
            lock (packetparselock)
            {
                copy = Packets;
            }
            if (packet.IP.SourceAddress.ToString() == bestIp || packet.IP.DestinationAddress.ToString() == bestIp)
            {
                foreach (Packet other in copy)
                {
                    if (other.IP.SourceAddress.ToString() == packet.IP.SourceAddress.ToString() && other.IP.DestinationAddress.ToString() == packet.IP.DestinationAddress.ToString())
                    {
                        other.Count++;
                        return;
                    }
                    else if (other.IP.SourceAddress.ToString() == packet.IP.DestinationAddress.ToString() && other.IP.DestinationAddress.ToString() == packet.IP.SourceAddress.ToString())
                    {
                        other.Count++;
                        return;
                    }
                }
                lock (packetslock)
                {
                    Packets = copy;
                    Packets.Add(packet);
                }
            }
        }

        public class Packet
        {
            public IPHeader IP;
            public UDPHeader UDP;
            public TCPHeader TCP;
            public DNSHeader DNS;
            public int Count = 0;

            public Packet(IPHeader IP, UDPHeader UDP)
            {
                this.IP = IP;
                this.UDP = UDP;
            }
            public Packet(IPHeader IP, TCPHeader TCP)
            {
                this.IP = IP;
                this.TCP = TCP;
            }
            public Packet(IPHeader IP)
            {
                this.IP = IP;
            }
            public override string ToString()
            {
                string info = String.Empty;

                if (UDP != null)
                {
                    info += "[UDP] ";
                    info += IP.SourceAddress + ":" + UDP.SourcePort + " -> " + IP.DestinationAddress + ":" + UDP.DestinationPort;
                }
                else if (TCP != null)
                {
                    info += "[TCP] ";
                    info += IP.SourceAddress + ":" + TCP.SourcePort + " -> " + IP.DestinationAddress + ":" + TCP.DestinationPort;
                }
                else
                {
                    info += IP.SourceAddress + " -> " + IP.DestinationAddress;
                }
                if (Count > 0)
                {
                    info += "(" + Count + ")";
                }

                return info;
            }
        }
        #endregion

        string bestIp;
        uint processId;
        List<Port> Ports = new List<Port>();
        List<Packet> ProcessPackets = new List<Packet>();
        string maxIp;
        Object processpacketslock = new Object();
        private void gameModeTimer_Tick(object sender, EventArgs e)
        {
            lock (processpacketslock)
            {
                ProcessPackets.Clear();
            }

            FindProcessPackets();
            FindBestDestinationIp();
            lock (packetslock)
            {
                Packets.Clear();
            }

            if (maxIp != String.Empty)
            {
                validatedAdresses[UsedSettings.LabelsNr - 1] = IPAddress.Parse(maxIp);
            }
            else
            {
                validatedAdresses[UsedSettings.LabelsNr - 1] = IPAddress.Parse("127.0.0.1");
            }
        }
        int timeout = 20;
        private void getPorts()
        {
            Ports = NetStatPorts.GetNetStatPorts();
        }
        private void getPortsTimer_Tick(object sender, EventArgs e)
        {
            Thread t = new Thread(getPorts);
            t.Start();
            bestIp = FindBestInterface();

        }
        private void activeProcessTimer_Tick(object sender, EventArgs e)
        {
            uint newprocess = GetActiveProcessId();

            if (newprocess != processId)
            {
                processId = newprocess;
                timeout = 0;
                graphPings[UsedSettings.LabelsNr - 1] = new List<int>();
                //Poopy workaround, change it!
                ToggleAuto();
                ToggleAuto();
            }
            if (timeout < 2000 / activeProcessTimer.Interval)
            {
                (this.Controls.Find((UsedSettings.LabelsNr).ToString(), true).FirstOrDefault() as Label).Text = NetStatPorts.LookupProcess(Convert.ToInt16(processId));
                timeout++;
            }
        }
        private void ToggleAuto()
        {
            if (IsAdministrator() == false)
            {
                DialogResult dialogResult = MessageBox.Show("This option requires admin rights.\nDo you want to restart as admin?", "Auto ping enable prompt", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    if (UsedSettings.AutoPing)
                    {
                        UsedSettings.AutoPing = false;
                    }
                    return;
                }

                // Restart program and run as admin
                var exeName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                ProcessStartInfo startInfo = new ProcessStartInfo(exeName);
                startInfo.Verb = "runas";
                startInfo.Arguments = "-a";

                try
                {
                    System.Diagnostics.Process.Start(startInfo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Didn't get admin permissions!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Close();
            }

            if (!continueCapturing)
            {
                UsedSettings.AutoPing = true;
                string name = "B" + (UsedSettings.LabelsNr);
                ToolStripItem[] menu = adressesToolStripMenuItem.DropDownItems.Find(name, true);
                menu[0].Enabled = false;
                menu[0].Text = "Auto Ping";
                bestIp = FindBestInterface();
                activeProcessTimer.Enabled = true;
                gameModeTimer.Enabled = true;
                getPortsTimer.Enabled = true;
                getPorts();
                ToggleSniffing();

                autoPingToolStripMenuItem.Text = "AutoPing ON";
                autoPingToolStripMenuItem.BackColor = Color.FromArgb(150, 210, 150);
            }
            else
            {
                UsedSettings.AutoPing = false;
                string name = "B" + (UsedSettings.LabelsNr);
                ToolStripItem[] menu = adressesToolStripMenuItem.DropDownItems.Find(name, true);
                menu[0].Enabled = true;
                menu[0].Text = validatedAdresses.Last().ToString();
                activeProcessTimer.Enabled = false;
                gameModeTimer.Enabled = false;
                getPortsTimer.Enabled = false;
                ToggleSniffing();

                autoPingToolStripMenuItem.Text = "AutoPing OFF";
                autoPingToolStripMenuItem.BackColor = SystemColors.Control;
            }
        }

        private void autoPingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleAuto();
            if (UsedSettings.AutoPing)
            {
                Log("AutoPing enabled");
            }
            else
            {
                Log("AutoPing disabled");
            }

        }
        private static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        // To run as admin, alter exe manifest file after building.
        // Or create shortcut with "as admin" checked.
        // Or ShellExecute(C# Process.Start) can elevate - use verb "runas".
        // Or an elevate vbs script can launch programs as admin.
        // (does not work: "runas /user:admin" from cmd-line prompts for admin pass)
        Object portslock = new Object();
        private void FindProcessPackets()
        {
            try
            {
                List<Packet> copy = new List<Packet>();
                lock (packetslock)
                {
                    copy = Packets;
                }
                foreach (Packet packet in copy)
                {
                    bool found = false;
                    string foundPort = String.Empty;

                    List<Port> copyPorts;
                    lock (portslock)
                    {
                        copyPorts = Ports;
                    }
                    foreach (Port port in copyPorts)
                    {
                        if (packet.IP.ProtocolType == Protocol.TCP)
                        {
                            if (packet.IP.SourceAddress.ToString() == bestIp)
                            {
                                foundPort = packet.TCP.SourcePort;

                                found = true;
                            }
                            else if (packet.IP.DestinationAddress.ToString() == bestIp)
                            {
                                foundPort = packet.TCP.DestinationPort;
                                found = true;
                            }
                        }
                        else if (packet.IP.ProtocolType == Protocol.UDP)
                        {
                            if (packet.IP.SourceAddress.ToString() == bestIp)
                            {
                                foundPort = packet.UDP.SourcePort;
                                found = true;
                            }
                            else if (packet.IP.DestinationAddress.ToString() == bestIp)
                            {
                                foundPort = packet.UDP.DestinationPort;
                                found = true;
                            }
                        }
                        if (found)
                        {
                            if (port.port_number == foundPort && port.process_pid == processId.ToString())
                            {
                                lock (processpacketslock)
                                {
                                    ProcessPackets.Add(packet);
                                }

                                break;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void FindBestDestinationIp()
        {
            maxIp = String.Empty;
            int maxcount = 0;
            List<Packet> copy;
            lock (processpacketslock)
            {
                copy = ProcessPackets;
            }
            foreach (Packet packet in copy)
            {
                if (maxcount < packet.Count)
                {
                    maxcount = packet.Count;

                    if (packet.IP.SourceAddress.ToString() == bestIp)
                    {
                        maxIp = packet.IP.DestinationAddress.ToString();
                    }
                    else if (packet.IP.DestinationAddress.ToString() == bestIp)
                    {
                        maxIp = packet.IP.SourceAddress.ToString();
                    }
                }
            }
        }

        private void OverlayForm_Shown(object sender, EventArgs e)
        {
            List<string> args = new List<string>(Environment.GetCommandLineArgs());

            if (args.Count > 1)
            {
                if (args.Contains("-a"))
                {
                    ToggleAuto();
                }
            }
            else if (UsedSettings.AutoPing)
            {
                ToggleAuto();
            }
        }
        #endregion

    }
}