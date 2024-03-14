namespace ModemChorus
{
    partial class frmModemControl
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbPorts = new System.Windows.Forms.ListBox();
            this.bt_Dialup = new System.Windows.Forms.Button();
            this.tbConsole = new System.Windows.Forms.TextBox();
            this.btHangup = new System.Windows.Forms.Button();
            this.btAtz = new System.Windows.Forms.Button();
            this.tbAtCmd = new System.Windows.Forms.TextBox();
            this.lbMdmStatus = new System.Windows.Forms.ListBox();
            this.btRztPort = new System.Windows.Forms.Button();
            this.btSymphony = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbPorts
            // 
            this.lbPorts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPorts.FormattingEnabled = true;
            this.lbPorts.ItemHeight = 15;
            this.lbPorts.Location = new System.Drawing.Point(11, 7);
            this.lbPorts.Margin = new System.Windows.Forms.Padding(2);
            this.lbPorts.Name = "lbPorts";
            this.lbPorts.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbPorts.Size = new System.Drawing.Size(258, 199);
            this.lbPorts.TabIndex = 0;
            // 
            // bt_Dialup
            // 
            this.bt_Dialup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Dialup.Location = new System.Drawing.Point(636, 11);
            this.bt_Dialup.Margin = new System.Windows.Forms.Padding(2);
            this.bt_Dialup.Name = "bt_Dialup";
            this.bt_Dialup.Size = new System.Drawing.Size(78, 20);
            this.bt_Dialup.TabIndex = 1;
            this.bt_Dialup.Text = "DIAL-UP";
            this.bt_Dialup.UseVisualStyleBackColor = true;
            this.bt_Dialup.Click += new System.EventHandler(this.btDialup_Click);
            // 
            // tbConsole
            // 
            this.tbConsole.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbConsole.Location = new System.Drawing.Point(728, 7);
            this.tbConsole.Margin = new System.Windows.Forms.Padding(2);
            this.tbConsole.Multiline = true;
            this.tbConsole.Name = "tbConsole";
            this.tbConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbConsole.Size = new System.Drawing.Size(364, 204);
            this.tbConsole.TabIndex = 2;
            this.tbConsole.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseDoubleClick);
            // 
            // btHangup
            // 
            this.btHangup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btHangup.Location = new System.Drawing.Point(636, 35);
            this.btHangup.Margin = new System.Windows.Forms.Padding(2);
            this.btHangup.Name = "btHangup";
            this.btHangup.Size = new System.Drawing.Size(78, 20);
            this.btHangup.TabIndex = 3;
            this.btHangup.Text = "HANG UP";
            this.btHangup.UseVisualStyleBackColor = true;
            this.btHangup.Click += new System.EventHandler(this.button2_Click);
            // 
            // btAtz
            // 
            this.btAtz.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btAtz.Location = new System.Drawing.Point(636, 113);
            this.btAtz.Margin = new System.Windows.Forms.Padding(2);
            this.btAtz.Name = "btAtz";
            this.btAtz.Size = new System.Drawing.Size(78, 20);
            this.btAtz.TabIndex = 4;
            this.btAtz.Text = "ATZ";
            this.btAtz.UseVisualStyleBackColor = true;
            this.btAtz.Click += new System.EventHandler(this.button3_Click);
            // 
            // tbAtCmd
            // 
            this.tbAtCmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAtCmd.Location = new System.Drawing.Point(636, 137);
            this.tbAtCmd.Margin = new System.Windows.Forms.Padding(2);
            this.tbAtCmd.Name = "tbAtCmd";
            this.tbAtCmd.Size = new System.Drawing.Size(78, 23);
            this.tbAtCmd.TabIndex = 5;
            this.tbAtCmd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox2_KeyDown);
            // 
            // lbMdmStatus
            // 
            this.lbMdmStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbMdmStatus.FormattingEnabled = true;
            this.lbMdmStatus.ItemHeight = 15;
            this.lbMdmStatus.Location = new System.Drawing.Point(273, 7);
            this.lbMdmStatus.Margin = new System.Windows.Forms.Padding(2);
            this.lbMdmStatus.Name = "lbMdmStatus";
            this.lbMdmStatus.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lbMdmStatus.Size = new System.Drawing.Size(350, 199);
            this.lbMdmStatus.TabIndex = 6;
            // 
            // btRztPort
            // 
            this.btRztPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btRztPort.Location = new System.Drawing.Point(636, 85);
            this.btRztPort.Name = "btRztPort";
            this.btRztPort.Size = new System.Drawing.Size(78, 23);
            this.btRztPort.TabIndex = 7;
            this.btRztPort.Text = "RSTPRT";
            this.btRztPort.UseVisualStyleBackColor = true;
            this.btRztPort.Click += new System.EventHandler(this.button4_Click);
            // 
            // btSymphony
            // 
            this.btSymphony.Enabled = false;
            this.btSymphony.Location = new System.Drawing.Point(639, 183);
            this.btSymphony.Name = "btSymphony";
            this.btSymphony.Size = new System.Drawing.Size(75, 23);
            this.btSymphony.TabIndex = 8;
            this.btSymphony.Text = "Symphony";
            this.btSymphony.UseVisualStyleBackColor = true;
            this.btSymphony.Click += new System.EventHandler(this.button5_Click);
            // 
            // frmModemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1103, 223);
            this.Controls.Add(this.btSymphony);
            this.Controls.Add(this.btRztPort);
            this.Controls.Add(this.lbMdmStatus);
            this.Controls.Add(this.tbAtCmd);
            this.Controls.Add(this.btAtz);
            this.Controls.Add(this.btHangup);
            this.Controls.Add(this.tbConsole);
            this.Controls.Add(this.bt_Dialup);
            this.Controls.Add(this.lbPorts);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(1000, 227);
            this.Name = "frmModemControl";
            this.Text = "Dial-up modem control";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ModemControl_FormClosing);
            this.Load += new System.EventHandler(this.ModemControl_Load);
            this.Shown += new System.EventHandler(this.ModemControl_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBox lbPorts;
        private Button bt_Dialup;
        private TextBox tbConsole;
        private Button btHangup;
        private Button btAtz;
        private TextBox tbAtCmd;
        private ListBox lbMdmStatus;
        private Button btRztPort;
        private Button btSymphony;
    }
}