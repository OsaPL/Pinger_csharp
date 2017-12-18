namespace SmartAdressIpDetectionTest
{
    partial class netstatform
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
            this.textBoxProcess = new System.Windows.Forms.TextBox();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.listBoxPackets = new System.Windows.Forms.ListBox();
            this.listBoxPorts = new System.Windows.Forms.ListBox();
            this.textBoxInterface = new System.Windows.Forms.TextBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.listBoxSummed = new System.Windows.Forms.ListBox();
            this.textBoxIp = new System.Windows.Forms.TextBox();
            this.timerActiveProcess = new System.Windows.Forms.Timer(this.components);
            this.timerGetPorts = new System.Windows.Forms.Timer(this.components);
            this.timerShowPackets = new System.Windows.Forms.Timer(this.components);
            this.textBoxPing = new System.Windows.Forms.TextBox();
            this.timerPing = new System.Windows.Forms.Timer(this.components);
            this.timerIgnoreCheck = new System.Windows.Forms.Timer(this.components);
            this.labelIgnored = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxProcess
            // 
            this.textBoxProcess.Enabled = false;
            this.textBoxProcess.Location = new System.Drawing.Point(12, 12);
            this.textBoxProcess.Name = "textBoxProcess";
            this.textBoxProcess.Size = new System.Drawing.Size(178, 20);
            this.textBoxProcess.TabIndex = 0;
            // 
            // textBoxPath
            // 
            this.textBoxPath.Enabled = false;
            this.textBoxPath.Location = new System.Drawing.Point(12, 38);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(883, 20);
            this.textBoxPath.TabIndex = 1;
            // 
            // listBoxPackets
            // 
            this.listBoxPackets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxPackets.Enabled = false;
            this.listBoxPackets.FormattingEnabled = true;
            this.listBoxPackets.Location = new System.Drawing.Point(250, 64);
            this.listBoxPackets.Name = "listBoxPackets";
            this.listBoxPackets.Size = new System.Drawing.Size(325, 121);
            this.listBoxPackets.TabIndex = 2;
            // 
            // listBoxPorts
            // 
            this.listBoxPorts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxPorts.FormattingEnabled = true;
            this.listBoxPorts.Location = new System.Drawing.Point(12, 64);
            this.listBoxPorts.Name = "listBoxPorts";
            this.listBoxPorts.Size = new System.Drawing.Size(231, 121);
            this.listBoxPorts.TabIndex = 3;
            // 
            // textBoxInterface
            // 
            this.textBoxInterface.Enabled = false;
            this.textBoxInterface.Location = new System.Drawing.Point(717, 12);
            this.textBoxInterface.Name = "textBoxInterface";
            this.textBoxInterface.Size = new System.Drawing.Size(178, 20);
            this.textBoxInterface.TabIndex = 4;
            // 
            // buttonStart
            // 
            this.buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStart.Location = new System.Drawing.Point(819, 198);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 20);
            this.buttonStart.TabIndex = 5;
            this.buttonStart.Text = "Sturt";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // listBoxSummed
            // 
            this.listBoxSummed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxSummed.Enabled = false;
            this.listBoxSummed.FormattingEnabled = true;
            this.listBoxSummed.Location = new System.Drawing.Point(581, 64);
            this.listBoxSummed.Name = "listBoxSummed";
            this.listBoxSummed.Size = new System.Drawing.Size(314, 121);
            this.listBoxSummed.TabIndex = 6;
            // 
            // textBoxIp
            // 
            this.textBoxIp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxIp.Enabled = false;
            this.textBoxIp.Location = new System.Drawing.Point(12, 198);
            this.textBoxIp.Name = "textBoxIp";
            this.textBoxIp.Size = new System.Drawing.Size(362, 20);
            this.textBoxIp.TabIndex = 7;
            // 
            // timerActiveProcess
            // 
            this.timerActiveProcess.Tick += new System.EventHandler(this.timerActiveProcess_Tick);
            // 
            // timerGetPorts
            // 
            this.timerGetPorts.Interval = 500;
            this.timerGetPorts.Tick += new System.EventHandler(this.timerGetPorts_Tick);
            // 
            // timerShowPackets
            // 
            this.timerShowPackets.Interval = 2000;
            this.timerShowPackets.Tick += new System.EventHandler(this.timerShowPackets_Tick);
            // 
            // textBoxPing
            // 
            this.textBoxPing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxPing.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.textBoxPing.Location = new System.Drawing.Point(380, 197);
            this.textBoxPing.Name = "textBoxPing";
            this.textBoxPing.Size = new System.Drawing.Size(100, 20);
            this.textBoxPing.TabIndex = 9;
            // 
            // timerPing
            // 
            this.timerPing.Interval = 500;
            this.timerPing.Tick += new System.EventHandler(this.timerPing_Tick);
            // 
            // timerIgnoreCheck
            // 
            this.timerIgnoreCheck.Tick += new System.EventHandler(this.timerIgnoreCheck_Tick);
            // 
            // labelIgnored
            // 
            this.labelIgnored.AutoSize = true;
            this.labelIgnored.ForeColor = System.Drawing.SystemColors.Control;
            this.labelIgnored.Location = new System.Drawing.Point(196, 19);
            this.labelIgnored.Name = "labelIgnored";
            this.labelIgnored.Size = new System.Drawing.Size(46, 13);
            this.labelIgnored.TabIndex = 10;
            this.labelIgnored.Text = "Ignored!";
            // 
            // netstatform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(901, 230);
            this.Controls.Add(this.labelIgnored);
            this.Controls.Add(this.textBoxPing);
            this.Controls.Add(this.textBoxIp);
            this.Controls.Add(this.listBoxSummed);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.textBoxInterface);
            this.Controls.Add(this.listBoxPorts);
            this.Controls.Add(this.listBoxPackets);
            this.Controls.Add(this.textBoxPath);
            this.Controls.Add(this.textBoxProcess);
            this.Name = "netstatform";
            this.Text = "netstatform";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.E_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxProcess;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.ListBox listBoxPackets;
        private System.Windows.Forms.ListBox listBoxPorts;
        private System.Windows.Forms.TextBox textBoxInterface;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.ListBox listBoxSummed;
        private System.Windows.Forms.TextBox textBoxIp;
        private System.Windows.Forms.Timer timerActiveProcess;
        private System.Windows.Forms.Timer timerGetPorts;
        private System.Windows.Forms.Timer timerShowPackets;
        private System.Windows.Forms.TextBox textBoxPing;
        private System.Windows.Forms.Timer timerPing;
        private System.Windows.Forms.Timer timerIgnoreCheck;
        private System.Windows.Forms.Label labelIgnored;
    }
}