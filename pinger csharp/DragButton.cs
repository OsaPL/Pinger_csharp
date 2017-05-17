using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pinger_csharp
{
    public partial class DragButton : Form
    {
        public DragButton()
        {
            InitializeComponent();
        }
        private void DragButton_Load(object sender, EventArgs e)
        {
            BackColor = Color.Black;
            TransparencyKey = Color.Black;
            TopMost = true;
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            Size = button.Size;
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
        // mouse dragging by button
        public bool mouseDown;
        private Point lastPos;
        private Point lastFormPos;
        private double lastopacity;
        private void button_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void button_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                mouseDown = true;
                lastPos = MousePosition;
                lastFormPos = this.Location;
            }
        }
        private bool IsOnScreen()
        {
            Screen[] screens = Screen.AllScreens;
            foreach (Screen screen in screens)
            {
                Rectangle controlRectangle = new Rectangle(this.Left, this.Top, this.button.Width, this.button.Height);

                if (screen.WorkingArea.Contains(controlRectangle))
                {
                    return true;
                }
            }
            return false;
        }
        public void SetButtonSize(int xSize,int ySize)
        {
            button.Size = new System.Drawing.Size(xSize, ySize+1);
            Size = button.Size;
        }
        private void button_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = PointToClient(Control.MousePosition);
            int h = Screen.PrimaryScreen.WorkingArea.Bottom;
            int w = Screen.PrimaryScreen.WorkingArea.Right;
            p.X -= w / 64;
            p.Y -= h / 64;
            Rectangle r = new Rectangle(p, new Size(w / 32, h / 32));
            if (ClientRectangle.IntersectsWith(r))
            {
                Opacity = 0.1;
                button.BackColor = Color.White;
                button.ForeColor = Color.White;
            }
            else
            {
                button.BackColor = Color.Black;
                button.ForeColor = Color.Black;
            }
            if (mouseDown)
            {
                Opacity = 0.05;
                int xoffset = MousePosition.X - lastPos.X;
                int yoffset = MousePosition.Y - lastPos.Y;
                Left += xoffset;
                Top += yoffset;
                lastPos = MousePosition;
                //OwnX.Text = "" + Location.X;
                //OwnY.Text = "" + Location.Y;
                /*if (!IsOnScreen())//not needed cause anchor now manages it
                {
                    Location = lastFormPos;
                    mouseDown = false;
                    this.Focus();
                }
                */
            }

        }

        private void follow_Tick(object sender, EventArgs e)
        {
            //button.Location = new Point(overlay.Location.X - button.Width, overlay.Location.Y);// czy cus podobnego? idk
        }
        public void SetButtonColor(Color color)
        {
            button.BackColor = color;
        }
        public void AnchorToCorners(int h, int w, Point p, Size size)
        {
            if (!mouseDown)
            {
                if (Location.X < w / 64 && Location.Y < h / 64)
                {
                    Location = new Point(0, 0);
                }
                if (Location.X > w - size.Width - w / 64 && Location.Y > h - size.Height - h / 64)
                {
                    Location = new Point(w-size.Width, h-size.Height);
                }
                if (Location.X > w - size.Width - w / 64)
                {
                    Location = new Point(w - size.Width, Location.Y);
                }
                if (Location.X < w / 64)
                {
                    Location = new Point(0,Location.Y);
                }
                if (Location.Y > h - size.Height - h / 64)
                {
                    Location = new Point(Location.X, h - size.Height);
                }
                if (Location.Y < h / 64)
                {
                    Location = new Point(Location.X,0);
                }
            }
        }
    }
}
