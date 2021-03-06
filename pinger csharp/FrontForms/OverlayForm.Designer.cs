﻿namespace pinger_csharp
{
    partial class OverlayForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OverlayForm));
            this.refresh = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adressesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.opacityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.opacityTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.intervalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.intervalStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.graphsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.graphsToggleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dotsHeightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dotsHeightTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.barsSpacingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.barsSpacingTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.barsWidthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.barsWidthTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.transferToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoPingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyTrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.throwPing = new System.Windows.Forms.Timer(this.components);
            this.bytesTimer = new System.Windows.Forms.Timer(this.components);
            this.bytesRLabel = new System.Windows.Forms.Label();
            this.bytesSLabel = new System.Windows.Forms.Label();
            this.netQualityBar = new System.Windows.Forms.ProgressBar();
            this.gameModeTimer = new System.Windows.Forms.Timer(this.components);
            this.activeProcessTimer = new System.Windows.Forms.Timer(this.components);
            this.getPortsTimer = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // refresh
            // 
            this.refresh.Enabled = true;
            this.refresh.Interval = 10;
            this.refresh.Tick += new System.EventHandler(this.refresh_Tick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.adressesToolStripMenuItem,
            this.toolStripSeparator1,
            this.settingsToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(121, 120);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // adressesToolStripMenuItem
            // 
            this.adressesToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlText;
            this.adressesToolStripMenuItem.Name = "adressesToolStripMenuItem";
            this.adressesToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.adressesToolStripMenuItem.Text = "Adresses";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(117, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveToolStripMenuItem,
            this.fontToolStripMenuItem,
            this.backgroundColorToolStripMenuItem,
            this.opacityToolStripMenuItem,
            this.intervalToolStripMenuItem,
            this.graphsToolStripMenuItem,
            this.transferToolStripMenuItem,
            this.autoPingToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // moveToolStripMenuItem
            // 
            this.moveToolStripMenuItem.Name = "moveToolStripMenuItem";
            this.moveToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.moveToolStripMenuItem.Text = "Move";
            this.moveToolStripMenuItem.Click += new System.EventHandler(this.moveToolStripMenuItem_Click_1);
            // 
            // fontToolStripMenuItem
            // 
            this.fontToolStripMenuItem.Name = "fontToolStripMenuItem";
            this.fontToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.fontToolStripMenuItem.Text = "Font";
            this.fontToolStripMenuItem.Click += new System.EventHandler(this.fontToolStripMenuItem_Click);
            // 
            // backgroundColorToolStripMenuItem
            // 
            this.backgroundColorToolStripMenuItem.Name = "backgroundColorToolStripMenuItem";
            this.backgroundColorToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.backgroundColorToolStripMenuItem.Text = "Color";
            this.backgroundColorToolStripMenuItem.Click += new System.EventHandler(this.backgroundColorToolStripMenuItem_Click);
            // 
            // opacityToolStripMenuItem
            // 
            this.opacityToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.opacityTextBox});
            this.opacityToolStripMenuItem.Name = "opacityToolStripMenuItem";
            this.opacityToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.opacityToolStripMenuItem.Text = "Opacity";
            // 
            // opacityTextBox
            // 
            this.opacityTextBox.Name = "opacityTextBox";
            this.opacityTextBox.Size = new System.Drawing.Size(100, 23);
            this.opacityTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.opacityTextBox_KeyPress);
            this.opacityTextBox.Click += new System.EventHandler(this.wrongValue_Click);
            // 
            // intervalToolStripMenuItem
            // 
            this.intervalToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.intervalStripTextBox});
            this.intervalToolStripMenuItem.Name = "intervalToolStripMenuItem";
            this.intervalToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.intervalToolStripMenuItem.Text = "Interval";
            // 
            // intervalStripTextBox
            // 
            this.intervalStripTextBox.Name = "intervalStripTextBox";
            this.intervalStripTextBox.Size = new System.Drawing.Size(100, 23);
            this.intervalStripTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.intervalStripTextBox_KeyPress);
            this.intervalStripTextBox.Click += new System.EventHandler(this.wrongValue_Click);
            // 
            // graphsToolStripMenuItem
            // 
            this.graphsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.graphsToggleToolStripMenuItem,
            this.dotsHeightToolStripMenuItem,
            this.barsSpacingToolStripMenuItem,
            this.barsWidthToolStripMenuItem});
            this.graphsToolStripMenuItem.Name = "graphsToolStripMenuItem";
            this.graphsToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.graphsToolStripMenuItem.Text = "Graphs";
            // 
            // graphsToggleToolStripMenuItem
            // 
            this.graphsToggleToolStripMenuItem.Name = "graphsToggleToolStripMenuItem";
            this.graphsToggleToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.graphsToggleToolStripMenuItem.Text = "Graphs Toggle";
            this.graphsToggleToolStripMenuItem.Click += new System.EventHandler(this.graphsToggleToolStripMenuItem_Click);
            // 
            // dotsHeightToolStripMenuItem
            // 
            this.dotsHeightToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dotsHeightTextBox});
            this.dotsHeightToolStripMenuItem.Name = "dotsHeightToolStripMenuItem";
            this.dotsHeightToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.dotsHeightToolStripMenuItem.Text = "Dots height";
            // 
            // dotsHeightTextBox
            // 
            this.dotsHeightTextBox.Name = "dotsHeightTextBox";
            this.dotsHeightTextBox.Size = new System.Drawing.Size(100, 23);
            this.dotsHeightTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dotsHeightTextBox_KeyPress);
            this.dotsHeightTextBox.Click += new System.EventHandler(this.wrongValue_Click);
            // 
            // barsSpacingToolStripMenuItem
            // 
            this.barsSpacingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.barsSpacingTextBox});
            this.barsSpacingToolStripMenuItem.Name = "barsSpacingToolStripMenuItem";
            this.barsSpacingToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.barsSpacingToolStripMenuItem.Text = "Bars spacing";
            // 
            // barsSpacingTextBox
            // 
            this.barsSpacingTextBox.Name = "barsSpacingTextBox";
            this.barsSpacingTextBox.Size = new System.Drawing.Size(100, 23);
            this.barsSpacingTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.barsSpacingTextBox_KeyPress);
            this.barsSpacingTextBox.Click += new System.EventHandler(this.wrongValue_Click);
            // 
            // barsWidthToolStripMenuItem
            // 
            this.barsWidthToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.barsWidthTextBox});
            this.barsWidthToolStripMenuItem.Name = "barsWidthToolStripMenuItem";
            this.barsWidthToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.barsWidthToolStripMenuItem.Text = "Bars width";
            // 
            // barsWidthTextBox
            // 
            this.barsWidthTextBox.Name = "barsWidthTextBox";
            this.barsWidthTextBox.Size = new System.Drawing.Size(100, 23);
            this.barsWidthTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.barsWidthTextBox_KeyPress);
            this.barsWidthTextBox.Click += new System.EventHandler(this.wrongValue_Click);
            // 
            // transferToolStripMenuItem
            // 
            this.transferToolStripMenuItem.Name = "transferToolStripMenuItem";
            this.transferToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.transferToolStripMenuItem.Text = "Transfer";
            this.transferToolStripMenuItem.Click += new System.EventHandler(this.transferToolStripMenuItem_Click);
            // 
            // autoPingToolStripMenuItem
            // 
            this.autoPingToolStripMenuItem.Name = "autoPingToolStripMenuItem";
            this.autoPingToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.autoPingToolStripMenuItem.Text = "AutoPing OFF";
            this.autoPingToolStripMenuItem.Click += new System.EventHandler(this.autoPingToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // notifyTrayIcon
            // 
            this.notifyTrayIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyTrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyTrayIcon.Icon")));
            this.notifyTrayIcon.Text = "Pinger";
            this.notifyTrayIcon.Visible = true;
            this.notifyTrayIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyTrayIcon_MouseClick);
            // 
            // throwPing
            // 
            this.throwPing.Enabled = true;
            this.throwPing.Tick += new System.EventHandler(this.throwPing_Tick);
            // 
            // bytesTimer
            // 
            this.bytesTimer.Enabled = true;
            this.bytesTimer.Interval = 1000;
            this.bytesTimer.Tick += new System.EventHandler(this.bytesTimer_Tick);
            // 
            // bytesRLabel
            // 
            this.bytesRLabel.AutoSize = true;
            this.bytesRLabel.Location = new System.Drawing.Point(12, 9);
            this.bytesRLabel.Name = "bytesRLabel";
            this.bytesRLabel.Size = new System.Drawing.Size(0, 13);
            this.bytesRLabel.TabIndex = 1;
            // 
            // bytesSLabel
            // 
            this.bytesSLabel.AutoSize = true;
            this.bytesSLabel.Location = new System.Drawing.Point(12, 22);
            this.bytesSLabel.Name = "bytesSLabel";
            this.bytesSLabel.Size = new System.Drawing.Size(0, 13);
            this.bytesSLabel.TabIndex = 2;
            // 
            // netQualityBar
            // 
            this.netQualityBar.Enabled = false;
            this.netQualityBar.Location = new System.Drawing.Point(0, 0);
            this.netQualityBar.Maximum = 230;
            this.netQualityBar.Minimum = 25;
            this.netQualityBar.Name = "netQualityBar";
            this.netQualityBar.Size = new System.Drawing.Size(100, 23);
            this.netQualityBar.TabIndex = 3;
            this.netQualityBar.Value = 25;
            // 
            // gameModeTimer
            // 
            this.gameModeTimer.Interval = 1000;
            this.gameModeTimer.Tick += new System.EventHandler(this.gameModeTimer_Tick);
            // 
            // activeProcessTimer
            // 
            this.activeProcessTimer.Tick += new System.EventHandler(this.activeProcessTimer_Tick);
            // 
            // getPortsTimer
            // 
            this.getPortsTimer.Interval = 5000;
            this.getPortsTimer.Tick += new System.EventHandler(this.getPortsTimer_Tick);
            // 
            // OverlayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(0, 0);
            this.Controls.Add(this.netQualityBar);
            this.Controls.Add(this.bytesSLabel);
            this.Controls.Add(this.bytesRLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "OverlayForm";
            this.Text = "OverlayForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OverlayForm_FormClosing);
            this.Load += new System.EventHandler(this.OverlayForm_Load);
            this.Shown += new System.EventHandler(this.OverlayForm_Shown);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OverlayForm_Paint);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer refresh;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem adressesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyTrayIcon;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.Timer throwPing;
        private System.Windows.Forms.ToolStripMenuItem fontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backgroundColorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem graphsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dotsHeightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem barsWidthToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem barsSpacingToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox dotsHeightTextBox;
        private System.Windows.Forms.ToolStripTextBox barsWidthTextBox;
        private System.Windows.Forms.ToolStripTextBox barsSpacingTextBox;
        private System.Windows.Forms.ToolStripMenuItem graphsToggleToolStripMenuItem;
        private System.Windows.Forms.Timer bytesTimer;
        private System.Windows.Forms.Label bytesRLabel;
        private System.Windows.Forms.Label bytesSLabel;
        private System.Windows.Forms.ToolStripMenuItem transferToolStripMenuItem;
        private System.Windows.Forms.ProgressBar netQualityBar;
        private System.Windows.Forms.ToolStripMenuItem intervalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem opacityToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox opacityTextBox;
        private System.Windows.Forms.ToolStripTextBox intervalStripTextBox;
        private System.Windows.Forms.ToolStripMenuItem moveToolStripMenuItem;
        private System.Windows.Forms.Timer gameModeTimer;
        private System.Windows.Forms.ToolStripMenuItem autoPingToolStripMenuItem;
        private System.Windows.Forms.Timer activeProcessTimer;
        private System.Windows.Forms.Timer getPortsTimer;
    }
}