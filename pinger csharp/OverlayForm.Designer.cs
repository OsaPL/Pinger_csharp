namespace pinger_csharp
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
            this.apperanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.graphsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dotsHeightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dotsHeightTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.barsSpacingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.barsSpacingTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.barsWidthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.barsWidthTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyTrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.throwPing = new System.Windows.Forms.Timer(this.components);
            this.graphsToggleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.apperanceToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(153, 142);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // adressesToolStripMenuItem
            // 
            this.adressesToolStripMenuItem.Name = "adressesToolStripMenuItem";
            this.adressesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.adressesToolStripMenuItem.Text = "Adresses";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // apperanceToolStripMenuItem
            // 
            this.apperanceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fontToolStripMenuItem,
            this.backgroundColorToolStripMenuItem,
            this.graphsToolStripMenuItem});
            this.apperanceToolStripMenuItem.Name = "apperanceToolStripMenuItem";
            this.apperanceToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.apperanceToolStripMenuItem.Text = "Apperance";
            // 
            // fontToolStripMenuItem
            // 
            this.fontToolStripMenuItem.Name = "fontToolStripMenuItem";
            this.fontToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.fontToolStripMenuItem.Text = "Font";
            this.fontToolStripMenuItem.Click += new System.EventHandler(this.fontToolStripMenuItem_Click);
            // 
            // backgroundColorToolStripMenuItem
            // 
            this.backgroundColorToolStripMenuItem.Name = "backgroundColorToolStripMenuItem";
            this.backgroundColorToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.backgroundColorToolStripMenuItem.Text = "Color";
            this.backgroundColorToolStripMenuItem.Click += new System.EventHandler(this.backgroundColorToolStripMenuItem_Click);
            // 
            // graphsToolStripMenuItem
            // 
            this.graphsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.graphsToggleToolStripMenuItem,
            this.dotsHeightToolStripMenuItem,
            this.barsSpacingToolStripMenuItem,
            this.barsWidthToolStripMenuItem});
            this.graphsToolStripMenuItem.Name = "graphsToolStripMenuItem";
            this.graphsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.graphsToolStripMenuItem.Text = "Graphs";
            // 
            // dotsHeightToolStripMenuItem
            // 
            this.dotsHeightToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dotsHeightTextBox});
            this.dotsHeightToolStripMenuItem.Name = "dotsHeightToolStripMenuItem";
            this.dotsHeightToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.dotsHeightToolStripMenuItem.Text = "Dots height";
            // 
            // dotsHeightTextBox
            // 
            this.dotsHeightTextBox.Name = "dotsHeightTextBox";
            this.dotsHeightTextBox.Size = new System.Drawing.Size(100, 23);
            this.dotsHeightTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dotsHeightTextBox_KeyPress);
            // 
            // barsSpacingToolStripMenuItem
            // 
            this.barsSpacingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.barsSpacingTextBox});
            this.barsSpacingToolStripMenuItem.Name = "barsSpacingToolStripMenuItem";
            this.barsSpacingToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.barsSpacingToolStripMenuItem.Text = "Bars spacing";
            // 
            // barsSpacingTextBox
            // 
            this.barsSpacingTextBox.Name = "barsSpacingTextBox";
            this.barsSpacingTextBox.Size = new System.Drawing.Size(100, 23);
            this.barsSpacingTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.barsSpacingTextBox_KeyPress);
            // 
            // barsWidthToolStripMenuItem
            // 
            this.barsWidthToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.barsWidthTextBox});
            this.barsWidthToolStripMenuItem.Name = "barsWidthToolStripMenuItem";
            this.barsWidthToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.barsWidthToolStripMenuItem.Text = "Bars width";
            // 
            // barsWidthTextBox
            // 
            this.barsWidthTextBox.Name = "barsWidthTextBox";
            this.barsWidthTextBox.Size = new System.Drawing.Size(100, 23);
            this.barsWidthTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.barsWidthTextBox_KeyPress);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // notifyTrayIcon
            // 
            this.notifyTrayIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyTrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyTrayIcon.Icon")));
            this.notifyTrayIcon.Text = "notifyIcon1";
            this.notifyTrayIcon.Visible = true;
            // 
            // throwPing
            // 
            this.throwPing.Enabled = true;
            this.throwPing.Tick += new System.EventHandler(this.throwPing_Tick);
            // 
            // graphsToggleToolStripMenuItem
            // 
            this.graphsToggleToolStripMenuItem.Name = "graphsToggleToolStripMenuItem";
            this.graphsToggleToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.graphsToggleToolStripMenuItem.Text = "Graphs Toggle";
            this.graphsToggleToolStripMenuItem.Click += new System.EventHandler(this.graphsToggleToolStripMenuItem_Click);
            // 
            // OverlayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 261);
            this.Name = "OverlayForm";
            this.Text = "OverlayForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OverlayForm_FormClosing);
            this.Load += new System.EventHandler(this.OverlayForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OverlayForm_Paint);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer refresh;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem adressesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem apperanceToolStripMenuItem;
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
    }
}