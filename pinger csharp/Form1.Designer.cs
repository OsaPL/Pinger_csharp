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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
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
            this.backgroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.opacityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.adress1 = new System.Windows.Forms.ToolStripMenuItem();
            this.pingadress1 = new System.Windows.Forms.ToolStripTextBox();
            this.adress2 = new System.Windows.Forms.ToolStripMenuItem();
            this.pingadress2 = new System.Windows.Forms.ToolStripTextBox();
            this.time = new System.Windows.Forms.ToolStripMenuItem();
            this.timeset = new System.Windows.Forms.ToolStripTextBox();
            this.graphCheck = new System.Windows.Forms.ToolStripMenuItem();
            this.rightBottomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.barsWidthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BarsWidthTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.dotsHeightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DotsHeightTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.lockWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.resetlocation = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.ContextMenuStrip = this.contextMenuStrip;
            this.label2.Location = new System.Drawing.Point(74, -1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "(2)9999ms";
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.placementToolStripMenuItem,
            this.lookToolStripMenuItem,
            this.adress1,
            this.adress2,
            this.time,
            this.graphCheck,
            this.lockWindowToolStripMenuItem,
            this.toolStripSeparator1,
            this.aboutToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(142, 208);
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
            this.placementToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
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
            this.backgroundToolStripMenuItem,
            this.opacityToolStripMenuItem});
            this.lookToolStripMenuItem.Name = "lookToolStripMenuItem";
            this.lookToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.lookToolStripMenuItem.Text = "Look";
            // 
            // boldToolStripMenuItem
            // 
            this.boldToolStripMenuItem.Name = "boldToolStripMenuItem";
            this.boldToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.boldToolStripMenuItem.Text = "Font";
            this.boldToolStripMenuItem.Click += new System.EventHandler(this.boldToolStripMenuItem_Click);
            // 
            // backgroundToolStripMenuItem
            // 
            this.backgroundToolStripMenuItem.Name = "backgroundToolStripMenuItem";
            this.backgroundToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.backgroundToolStripMenuItem.Text = "Background";
            this.backgroundToolStripMenuItem.Click += new System.EventHandler(this.backgroundToolStripMenuItem_Click);
            // 
            // opacityToolStripMenuItem
            // 
            this.opacityToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox1});
            this.opacityToolStripMenuItem.Name = "opacityToolStripMenuItem";
            this.opacityToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.opacityToolStripMenuItem.Text = "Opacity";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 23);
            this.toolStripTextBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox1_KeyPress);
            // 
            // adress1
            // 
            this.adress1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pingadress1});
            this.adress1.Name = "adress1";
            this.adress1.Size = new System.Drawing.Size(152, 22);
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
            this.adress2.Size = new System.Drawing.Size(152, 22);
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
            this.time.Size = new System.Drawing.Size(152, 22);
            this.time.Text = "Time";
            // 
            // timeset
            // 
            this.timeset.Name = "timeset";
            this.timeset.Size = new System.Drawing.Size(100, 23);
            this.timeset.Text = "500";
            this.timeset.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.timeset_KeyPress);
            // 
            // graphCheck
            // 
            this.graphCheck.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rightBottomToolStripMenuItem,
            this.barsWidthToolStripMenuItem,
            this.dotsHeightToolStripMenuItem});
            this.graphCheck.Name = "graphCheck";
            this.graphCheck.Size = new System.Drawing.Size(152, 22);
            this.graphCheck.Text = "graphCheck";
            this.graphCheck.Click += new System.EventHandler(this.graphCheck_Click);
            // 
            // rightBottomToolStripMenuItem
            // 
            this.rightBottomToolStripMenuItem.Name = "rightBottomToolStripMenuItem";
            this.rightBottomToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.rightBottomToolStripMenuItem.Text = "RightBottom";
            this.rightBottomToolStripMenuItem.Click += new System.EventHandler(this.rightBottomToolStripMenuItem_Click);
            // 
            // barsWidthToolStripMenuItem
            // 
            this.barsWidthToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BarsWidthTextBox});
            this.barsWidthToolStripMenuItem.Name = "barsWidthToolStripMenuItem";
            this.barsWidthToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.barsWidthToolStripMenuItem.Text = "BarsWidth";
            this.barsWidthToolStripMenuItem.Click += new System.EventHandler(this.barsWidthToolStripMenuItem_Click);
            // 
            // BarsWidthTextBox
            // 
            this.BarsWidthTextBox.Name = "BarsWidthTextBox";
            this.BarsWidthTextBox.Size = new System.Drawing.Size(100, 23);
            this.BarsWidthTextBox.Click += new System.EventHandler(this.toolStripTextBox3_Click);
            // 
            // dotsHeightToolStripMenuItem
            // 
            this.dotsHeightToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DotsHeightTextBox});
            this.dotsHeightToolStripMenuItem.Name = "dotsHeightToolStripMenuItem";
            this.dotsHeightToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.dotsHeightToolStripMenuItem.Text = "DotsHeight";
            // 
            // DotsHeightTextBox
            // 
            this.DotsHeightTextBox.Name = "DotsHeightTextBox";
            this.DotsHeightTextBox.Size = new System.Drawing.Size(100, 23);
            // 
            // lockWindowToolStripMenuItem
            // 
            this.lockWindowToolStripMenuItem.Name = "lockWindowToolStripMenuItem";
            this.lockWindowToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.lockWindowToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.lockWindowToolStripMenuItem.Text = "Lock";
            this.lockWindowToolStripMenuItem.Click += new System.EventHandler(this.lockWindowToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.ContextMenuStrip = this.contextMenuStrip;
            this.label1.Location = new System.Drawing.Point(12, -1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "(1)9999ms";
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
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // resetlocation
            // 
            this.resetlocation.Enabled = true;
            this.resetlocation.Interval = 30;
            this.resetlocation.Tick += new System.EventHandler(this.resetlocation_Tick);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Pinger";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pictureBox1.ContextMenuStrip = this.contextMenuStrip;
            this.pictureBox1.Location = new System.Drawing.Point(0, 17);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(108, 41);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pictureBox2.ContextMenuStrip = this.contextMenuStrip;
            this.pictureBox2.Location = new System.Drawing.Point(0, 55);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(108, 41);
            this.pictureBox2.TabIndex = 4;
            this.pictureBox2.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(331, 238);
            this.ContextMenuStrip = this.contextMenuStrip;
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Opacity = 0.7D;
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Black;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem placementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem leftLowerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem leftHigherToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rightLowerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rightHigherToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem lookToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem boldToolStripMenuItem;
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
        private System.Windows.Forms.ToolStripMenuItem backgroundToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripMenuItem opacityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lockWindowToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem graphCheck;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ToolStripMenuItem rightBottomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem barsWidthToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dotsHeightToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox BarsWidthTextBox;
        private System.Windows.Forms.ToolStripTextBox DotsHeightTextBox;
    }
}

