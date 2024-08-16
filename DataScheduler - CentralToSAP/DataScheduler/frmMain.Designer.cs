namespace DataScheduler
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.bgSchedule = new System.ComponentModel.BackgroundWorker();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.pnlMessage = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlImage = new System.Windows.Forms.Panel();
            this.timer_Central = new System.Windows.Forms.Timer(this.components);
            this.pnlHeader.SuspendLayout();
            this.pnlMessage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(950, 66);
            this.pnlHeader.TabIndex = 7;
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.ForestGreen;
            this.lblHeader.Font = new System.Drawing.Font("Verdana", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(0, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(947, 40);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "GREENPLY CENTRAL DATA SCHEDULER";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bgSchedule
            // 
            //this.bgSchedule.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgSchedule_DoWork);
            // 
            // lblVersion
            // 
            this.lblVersion.BackColor = System.Drawing.Color.Green;
            this.lblVersion.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = System.Drawing.Color.White;
            this.lblVersion.Location = new System.Drawing.Point(786, 1);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(160, 29);
            this.lblVersion.TabIndex = 37;
            this.lblVersion.Text = "Version : 3.0.0";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("Verdana", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.Firebrick;
            this.lblMessage.Location = new System.Drawing.Point(100, 6);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(678, 70);
            this.lblMessage.TabIndex = 38;
            // 
            // pnlMessage
            // 
            this.pnlMessage.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMessage.Controls.Add(this.pictureBox2);
            this.pnlMessage.Controls.Add(this.pictureBox1);
            this.pnlMessage.Controls.Add(this.lblMessage);
            this.pnlMessage.Controls.Add(this.lblVersion);
            this.pnlMessage.Location = new System.Drawing.Point(0, 389);
            this.pnlMessage.Name = "pnlMessage";
            this.pnlMessage.Size = new System.Drawing.Size(949, 85);
            this.pnlMessage.TabIndex = 11;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::DataScheduler.Properties.Resources.bcillogo;
            this.pictureBox2.Location = new System.Drawing.Point(786, 32);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(160, 50);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 42;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DataScheduler.Properties.Resources.GreenplyLogos_02;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(95, 83);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 41;
            this.pictureBox1.TabStop = false;
            // 
            // pnlImage
            // 
            this.pnlImage.BackgroundImage = global::DataScheduler.Properties.Resources.DataSchedulerPic1;
            this.pnlImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlImage.Location = new System.Drawing.Point(0, 41);
            this.pnlImage.Name = "pnlImage";
            this.pnlImage.Size = new System.Drawing.Size(950, 347);
            this.pnlImage.TabIndex = 8;
            // 
            // timer_Central
            // 
            this.timer_Central.Tick += new System.EventHandler(this.timer_Central_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(950, 472);
            this.Controls.Add(this.pnlMessage);
            this.Controls.Add(this.pnlImage);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(966, 511);
            this.MinimumSize = new System.Drawing.Size(966, 511);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Greenply Central Data Scheduler V3.0.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlMessage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Panel pnlImage;
        private System.ComponentModel.BackgroundWorker bgSchedule;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Panel pnlMessage;
        public System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Timer timer_Central;
    }
}

