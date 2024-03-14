namespace ModemChorus
{
    partial class frmSymphony
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
            this.dgvSymphony = new System.Windows.Forms.DataGridView();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Command = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModemIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btStartSep = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btOneByOne = new System.Windows.Forms.Button();
            this.btAllTogether = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSymphony)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvSymphony
            // 
            this.dgvSymphony.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSymphony.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSymphony.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Time,
            this.Command,
            this.ModemIndex});
            this.dgvSymphony.Location = new System.Drawing.Point(12, 12);
            this.dgvSymphony.Name = "dgvSymphony";
            this.dgvSymphony.RowTemplate.Height = 25;
            this.dgvSymphony.Size = new System.Drawing.Size(444, 426);
            this.dgvSymphony.TabIndex = 0;
            // 
            // Time
            // 
            this.Time.HeaderText = "Time";
            this.Time.Name = "Time";
            // 
            // Command
            // 
            this.Command.HeaderText = "Command";
            this.Command.Name = "Command";
            // 
            // ModemIndex
            // 
            this.ModemIndex.HeaderText = "ModemIndex";
            this.ModemIndex.Name = "ModemIndex";
            this.ModemIndex.Width = 200;
            // 
            // btStartSep
            // 
            this.btStartSep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btStartSep.Location = new System.Drawing.Point(462, 415);
            this.btStartSep.Name = "btStartSep";
            this.btStartSep.Size = new System.Drawing.Size(75, 23);
            this.btStartSep.TabIndex = 2;
            this.btStartSep.Text = "Start";
            this.btStartSep.UseVisualStyleBackColor = true;
            this.btStartSep.Click += new System.EventHandler(this.btStart_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // btOneByOne
            // 
            this.btOneByOne.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOneByOne.Location = new System.Drawing.Point(462, 194);
            this.btOneByOne.Name = "btOneByOne";
            this.btOneByOne.Size = new System.Drawing.Size(75, 23);
            this.btOneByOne.TabIndex = 3;
            this.btOneByOne.Text = "1-by-1";
            this.btOneByOne.UseVisualStyleBackColor = true;
            this.btOneByOne.Click += new System.EventHandler(this.btOneByOne_Click);
            // 
            // btAllTogether
            // 
            this.btAllTogether.Location = new System.Drawing.Point(462, 223);
            this.btAllTogether.Name = "btAllTogether";
            this.btAllTogether.Size = new System.Drawing.Size(75, 23);
            this.btAllTogether.TabIndex = 4;
            this.btAllTogether.Text = "AllTogether";
            this.btAllTogether.UseVisualStyleBackColor = true;
            this.btAllTogether.Click += new System.EventHandler(this.btAllTogether_Click);
            // 
            // frmSymphony
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 450);
            this.Controls.Add(this.btAllTogether);
            this.Controls.Add(this.btOneByOne);
            this.Controls.Add(this.btStartSep);
            this.Controls.Add(this.dgvSymphony);
            this.Name = "frmSymphony";
            this.Text = "Symphony control";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSymphony)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DataGridView dgvSymphony;
        private DataGridViewTextBoxColumn Time;
        private DataGridViewTextBoxColumn Command;
        private DataGridViewTextBoxColumn ModemIndex;
        private Button btStartSep;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Button btOneByOne;
        private Button btAllTogether;
    }
}