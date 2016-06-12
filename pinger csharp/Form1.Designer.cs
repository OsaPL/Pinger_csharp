namespace pinger_csharp
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label label2;
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.placementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.leftLowerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.leftHigherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rightLowerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rightHigherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.OwnX = new System.Windows.Forms.ToolStripTextBox();
            this.OwnY = new System.Windows.Forms.ToolStripTextBox();
            this.lookToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.boldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.biggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smallerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adress1 = new System.Windows.Forms.ToolStripMenuItem();
            this.pingadress1 = new System.Windows.Forms.ToolStripTextBox();
            this.adress2 = new System.Windows.Forms.ToolStripMenuItem();
            this.pingadress2 = new System.Windows.Forms.ToolStripTextBox();
            this.time = new System.Windows.Forms.ToolStripMenuItem();
            this.timeset = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.resetlocation = new System.Windows.Forms.Timer(this.components);
            label2 = new System.Windows.Forms.Label();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.ContextMenuStrip = this.contextMenuStrip;
            this.label1.Location = new System.Drawing.Point(12, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "(1)999ms";
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.placementToolStripMenuItem,
            this.lookToolStripMenuItem,
            this.adress1,
            this.adress2,
            this.time,
            this.toolStripSeparator1,
            this.aboutToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(131, 164);
            // 
            // placementToolStripMenuItem
            // 
            this.placementToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.leftLowerToolStripMenuItem,
            this.leftHigherToolStripMenuItem,
            this.rightLowerToolStripMenuItem,
            this.rightHigherToolStripMenuItem,
            this.toolStripSeparator2,
            this.OwnX,
            this.OwnY});
            this.placementToolStripMenuItem.Name = "placementToolStripMenuItem";
            this.placementToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.placementToolStripMenuItem.Text = "Placement";
            // 
            // leftLowerToolStripMenuItem
            // 
            this.leftLowerToolStripMenuItem.Name = "leftLowerToolStripMenuItem";
            this.leftLowerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D3)));
            this.leftLowerToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.leftLowerToolStripMenuItem.Text = "Left_Lower";
            this.leftLowerToolStripMenuItem.Click += new System.EventHandler(this.leftLowerToolStripMenuItem_Click);
            // 
            // leftHigherToolStripMenuItem
            // 
            this.leftHigherToolStripMenuItem.Name = "leftHigherToolStripMenuItem";
            this.leftHigherToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D4)));
            this.leftHigherToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.leftHigherToolStripMenuItem.Text = "Left_Higher";
            this.leftHigherToolStripMenuItem.Click += new System.EventHandler(this.leftHigherToolStripMenuItem_Click);
            // 
            // rightLowerToolStripMenuItem
            // 
            this.rightLowerToolStripMenuItem.Name = "rightLowerToolStripMenuItem";
            this.rightLowerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D2)));
            this.rightLowerToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.rightLowerToolStripMenuItem.Text = "Right_Lower";
            this.rightLowerToolStripMenuItem.Click += new System.EventHandler(this.rightLowerToolStripMenuItem_Click);
            // 
            // rightHigherToolStripMenuItem
            // 
            this.rightHigherToolStripMenuItem.Name = "rightHigherToolStripMenuItem";
            this.rightHigherToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D1)));
            this.rightHigherToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.rightHigherToolStripMenuItem.Text = "Right_Higher";
            this.rightHigherToolStripMenuItem.Click += new System.EventHandler(this.rightHigherToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(176, 6);
            // 
            // OwnX
            // 
            this.OwnX.Name = "OwnX";
            this.OwnX.Size = new System.Drawing.Size(100, 23);
            this.OwnX.Text = "OwnX";
            this.OwnX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OwnX_KeyPress);
            // 
            // OwnY
            // 
            this.OwnY.Name = "OwnY";
            this.OwnY.Size = new System.Drawing.Size(100, 23);
            this.OwnY.Text = "OwnY";
            this.OwnY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OwnX_KeyPress);
            // 
            // lookToolStripMenuItem
            // 
            this.lookToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.boldToolStripMenuItem,
            this.fontSizeToolStripMenuItem,
            this.backgroundToolStripMenuItem});
            this.lookToolStripMenuItem.Name = "lookToolStripMenuItem";
            this.lookToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.lookToolStripMenuItem.Text = "Look";
            // 
            // boldToolStripMenuItem
            // 
            this.boldToolStripMenuItem.Name = "boldToolStripMenuItem";
            this.boldToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Right)));
            this.boldToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.boldToolStripMenuItem.Text = "Bold";
            this.boldToolStripMenuItem.Click += new System.EventHandler(this.boldToolStripMenuItem_Click);
            // 
            // fontSizeToolStripMenuItem
            // 
            this.fontSizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.biggerToolStripMenuItem,
            this.smallerToolStripMenuItem});
            this.fontSizeToolStripMenuItem.Name = "fontSizeToolStripMenuItem";
            this.fontSizeToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.fontSizeToolStripMenuItem.Text = "Font_Size";
            // 
            // biggerToolStripMenuItem
            // 
            this.biggerToolStripMenuItem.Name = "biggerToolStripMenuItem";
            this.biggerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up)));
            this.biggerToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.biggerToolStripMenuItem.Text = "Bigger";
            this.biggerToolStripMenuItem.Click += new System.EventHandler(this.biggerToolStripMenuItem_Click);
            // 
            // smallerToolStripMenuItem
            // 
            this.smallerToolStripMenuItem.Name = "smallerToolStripMenuItem";
            this.smallerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down)));
            this.smallerToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.smallerToolStripMenuItem.Text = "Smaller";
            this.smallerToolStripMenuItem.Click += new System.EventHandler(this.smallerToolStripMenuItem_Click);
            // 
            // backgroundToolStripMenuItem
            // 
            this.backgroundToolStripMenuItem.Name = "backgroundToolStripMenuItem";
            this.backgroundToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Left)));
            this.backgroundToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.backgroundToolStripMenuItem.Text = "Background";
            this.backgroundToolStripMenuItem.Click += new System.EventHandler(this.backgroundToolStripMenuItem_Click);
            // 
            // adress1
            // 
            this.adress1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pingadress1});
            this.adress1.Name = "adress1";
            this.adress1.Size = new System.Drawing.Size(130, 22);
            this.adress1.Text = "Ping1";
            // 
            // pingadress1
            // 
            this.pingadress1.Name = "pingadress1";
            this.pingadress1.Size = new System.Drawing.Size(100, 23);
            this.pingadress1.Text = "google.com";
            this.pingadress1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox2_KeyPress);
            // 
            // adress2
            // 
            this.adress2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pingadress2});
            this.adress2.Name = "adress2";
            this.adress2.Size = new System.Drawing.Size(130, 22);
            this.adress2.Text = "Ping2";
            // 
            // pingadress2
            // 
            this.pingadress2.Name = "pingadress2";
            this.pingadress2.Size = new System.Drawing.Size(100, 23);
            this.pingadress2.Text = "wp.pl";
            this.pingadress2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox2_KeyPress);
            // 
            // time
            // 
            this.time.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.timeset});
            this.time.Name = "time";
            this.time.Size = new System.Drawing.Size(130, 22);
            this.time.Text = "Time";
            // 
            // timeset
            // 
            this.timeset.Name = "timeset";
            this.timeset.Size = new System.Drawing.Size(100, 23);
            this.timeset.Text = "500";
            this.timeset.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.timeset_KeyPress);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(127, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.ContextMenuStrip = this.contextMenuStrip;
            this.label1.Location = new System.Drawing.Point(12, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "(1)999ms";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button1.ContextMenuStrip = this.contextMenuStrip;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(14, 14);
            this.button1.TabIndex = 1;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button1_MouseDown);
            this.button1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.button1_MouseMove);
            this.button1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button1_MouseUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.ContextMenuStrip = this.contextMenuStrip;
            this.label2.Location = new System.Drawing.Point(58, 1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "(2)999ms";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // resetlocation
            // 
            this.resetlocation.Enabled = true;
            this.resetlocation.Interval = 300;
            this.resetlocation.Tick += new System.EventHandler(this.resetlocation_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(331, 238);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Opacity = 0.7D;
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Black;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem placementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem leftLowerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem leftHigherToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rightLowerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rightHigherToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem lookToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem boldToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fontSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem biggerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smallerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backgroundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem adress1;
        private System.Windows.Forms.ToolStripMenuItem adress2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox OwnX;
        private System.Windows.Forms.ToolStripTextBox OwnY;
        private System.Windows.Forms.ToolStripTextBox pingadress1;
        private System.Windows.Forms.ToolStripTextBox pingadress2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem time;
        private System.Windows.Forms.ToolStripTextBox timeset;
        private System.Windows.Forms.Timer resetlocation;
    }
}

