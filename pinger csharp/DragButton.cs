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
        private bool mouseDown;
        private Point lastPos;
        private Point lastFormPos;
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
        private void button_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                int xoffset = MousePosition.X - lastPos.X;
                int yoffset = MousePosition.Y - lastPos.Y;
                Left += xoffset;
                Top += yoffset;
                lastPos = MousePosition;
                //OwnX.Text = "" + Location.X;
                //OwnY.Text = "" + Location.Y;
                if (!IsOnScreen())
                {
                    Location = lastFormPos;
                    mouseDown = false;
                    this.Focus();
                }
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

    }
}
