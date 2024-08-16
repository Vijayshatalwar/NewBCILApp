namespace GreenplyCommServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.lstData = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Messagetab = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pnlLocalDBSetting = new System.Windows.Forms.Panel();
            this.txtSAPPassword = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSAPId = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtConString = new System.Windows.Forms.TextBox();
            this.cmdTestCon1 = new System.Windows.Forms.Button();
            this.cmbSchema1 = new System.Windows.Forms.ComboBox();
            this.cmbServer1 = new System.Windows.Forms.ComboBox();
            this.cmdSave1 = new System.Windows.Forms.Button();
            this.txtPwd1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUserID1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.rtResponce = new System.Windows.Forms.RichTextBox();
            this.lblserverandport = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.cmdStop = new System.Windows.Forms.Button();
            this.cmdStart = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lblHeader = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.Messagetab.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.pnlLocalDBSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // lstData
            // 
            this.lstData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lstData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstData.GridLines = true;
            this.lstData.Location = new System.Drawing.Point(3, 3);
            this.lstData.Name = "lstData";
            this.lstData.Size = new System.Drawing.Size(1070, 331);
            this.lstData.TabIndex = 62;
            this.lstData.UseCompatibleStateImageBehavior = false;
            this.lstData.View = System.Windows.Forms.View.Details;
            this.lstData.SelectedIndexChanged += new System.EventHandler(this.lstData_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "S.NO";
            this.columnHeader1.Width = 45;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Message";
            this.columnHeader2.Width = 350;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "IPAddress";
            this.columnHeader3.Width = 700;
            // 
            // Messagetab
            // 
            this.Messagetab.Controls.Add(this.tabPage3);
            this.Messagetab.Controls.Add(this.tabPage1);
            this.Messagetab.Controls.Add(this.tabPage2);
            this.Messagetab.Location = new System.Drawing.Point(1, 36);
            this.Messagetab.Name = "Messagetab";
            this.Messagetab.SelectedIndex = 0;
            this.Messagetab.Size = new System.Drawing.Size(1084, 363);
            this.Messagetab.TabIndex = 64;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.pictureBox3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1076, 337);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Communication Server";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(1, 1);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(1046, 335);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 0;
            this.pictureBox3.TabStop = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lstData);
            this.tabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1076, 337);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Message Tab";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Transparent;
            this.tabPage2.Controls.Add(this.pnlLocalDBSetting);
            this.tabPage2.Controls.Add(this.rtResponce);
            this.tabPage2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1076, 337);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Connection Tab";
            this.tabPage2.Click += new System.EventHandler(this.tabPage2_Click);
            // 
            // pnlLocalDBSetting
            // 
            this.pnlLocalDBSetting.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlLocalDBSetting.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLocalDBSetting.Controls.Add(this.txtSAPPassword);
            this.pnlLocalDBSetting.Controls.Add(this.label8);
            this.pnlLocalDBSetting.Controls.Add(this.txtSAPId);
            this.pnlLocalDBSetting.Controls.Add(this.label9);
            this.pnlLocalDBSetting.Controls.Add(this.label2);
            this.pnlLocalDBSetting.Controls.Add(this.label3);
            this.pnlLocalDBSetting.Controls.Add(this.txtConString);
            this.pnlLocalDBSetting.Controls.Add(this.cmdTestCon1);
            this.pnlLocalDBSetting.Controls.Add(this.cmbSchema1);
            this.pnlLocalDBSetting.Controls.Add(this.cmbServer1);
            this.pnlLocalDBSetting.Controls.Add(this.cmdSave1);
            this.pnlLocalDBSetting.Controls.Add(this.txtPwd1);
            this.pnlLocalDBSetting.Controls.Add(this.label4);
            this.pnlLocalDBSetting.Controls.Add(this.txtUserID1);
            this.pnlLocalDBSetting.Controls.Add(this.label5);
            this.pnlLocalDBSetting.Controls.Add(this.label6);
            this.pnlLocalDBSetting.Controls.Add(this.label7);
            this.pnlLocalDBSetting.Location = new System.Drawing.Point(5, 6);
            this.pnlLocalDBSetting.MaximumSize = new System.Drawing.Size(429, 338);
            this.pnlLocalDBSetting.MinimumSize = new System.Drawing.Size(429, 338);
            this.pnlLocalDBSetting.Name = "pnlLocalDBSetting";
            this.pnlLocalDBSetting.Size = new System.Drawing.Size(429, 338);
            this.pnlLocalDBSetting.TabIndex = 48;
            // 
            // txtSAPPassword
            // 
            this.txtSAPPassword.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSAPPassword.Location = new System.Drawing.Point(124, 184);
            this.txtSAPPassword.Name = "txtSAPPassword";
            this.txtSAPPassword.PasswordChar = '*';
            this.txtSAPPassword.Size = new System.Drawing.Size(215, 22);
            this.txtSAPPassword.TabIndex = 148;
            this.txtSAPPassword.Tag = "";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(7, 186);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(103, 14);
            this.label8.TabIndex = 147;
            this.label8.Text = "SAP Password";
            // 
            // txtSAPId
            // 
            this.txtSAPId.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSAPId.Location = new System.Drawing.Point(124, 158);
            this.txtSAPId.Name = "txtSAPId";
            this.txtSAPId.Size = new System.Drawing.Size(215, 22);
            this.txtSAPId.TabIndex = 145;
            this.txtSAPId.Tag = "";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(7, 160);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(83, 14);
            this.label9.TabIndex = 146;
            this.label9.Text = "SAP UserId";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.SteelBlue;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(427, 44);
            this.label2.TabIndex = 138;
            this.label2.Text = "Local Server Setting";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 234);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 14);
            this.label3.TabIndex = 45;
            this.label3.Text = "Connection String";
            // 
            // txtConString
            // 
            this.txtConString.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConString.Location = new System.Drawing.Point(7, 251);
            this.txtConString.Name = "txtConString";
            this.txtConString.ReadOnly = true;
            this.txtConString.Size = new System.Drawing.Size(414, 22);
            this.txtConString.TabIndex = 44;
            // 
            // cmdTestCon1
            // 
            this.cmdTestCon1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdTestCon1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdTestCon1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdTestCon1.Location = new System.Drawing.Point(202, 214);
            this.cmdTestCon1.Name = "cmdTestCon1";
            this.cmdTestCon1.Size = new System.Drawing.Size(133, 35);
            this.cmdTestCon1.TabIndex = 43;
            this.cmdTestCon1.Tag = "";
            this.cmdTestCon1.Text = "Test Connection";
            this.cmdTestCon1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdTestCon1.UseVisualStyleBackColor = true;
            this.cmdTestCon1.Click += new System.EventHandler(this.cmdTestCon1_Click);
            // 
            // cmbSchema1
            // 
            this.cmbSchema1.FormattingEnabled = true;
            this.cmbSchema1.Location = new System.Drawing.Point(124, 133);
            this.cmbSchema1.Name = "cmbSchema1";
            this.cmbSchema1.Size = new System.Drawing.Size(215, 24);
            this.cmbSchema1.TabIndex = 42;
            this.cmbSchema1.Tag = "";
            this.cmbSchema1.Enter += new System.EventHandler(this.cmbSchema1_Enter);
            // 
            // cmbServer1
            // 
            this.cmbServer1.FormattingEnabled = true;
            this.cmbServer1.Location = new System.Drawing.Point(124, 56);
            this.cmbServer1.Name = "cmbServer1";
            this.cmbServer1.Size = new System.Drawing.Size(215, 24);
            this.cmbServer1.TabIndex = 41;
            this.cmbServer1.Tag = "";
            this.cmbServer1.Enter += new System.EventHandler(this.cmbServer1_Enter);
            // 
            // cmdSave1
            // 
            this.cmdSave1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdSave1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSave1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdSave1.Location = new System.Drawing.Point(159, 291);
            this.cmdSave1.Name = "cmdSave1";
            this.cmdSave1.Size = new System.Drawing.Size(145, 40);
            this.cmdSave1.TabIndex = 39;
            this.cmdSave1.Tag = "";
            this.cmdSave1.Text = "&Save && Continue";
            this.cmdSave1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdSave1.UseVisualStyleBackColor = true;
            this.cmdSave1.Click += new System.EventHandler(this.cmdSave1_Click);
            // 
            // txtPwd1
            // 
            this.txtPwd1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPwd1.Location = new System.Drawing.Point(124, 107);
            this.txtPwd1.Name = "txtPwd1";
            this.txtPwd1.PasswordChar = '*';
            this.txtPwd1.Size = new System.Drawing.Size(215, 22);
            this.txtPwd1.TabIndex = 7;
            this.txtPwd1.Tag = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(7, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 14);
            this.label4.TabIndex = 6;
            this.label4.Text = "Password";
            // 
            // txtUserID1
            // 
            this.txtUserID1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserID1.Location = new System.Drawing.Point(124, 81);
            this.txtUserID1.Name = "txtUserID1";
            this.txtUserID1.Size = new System.Drawing.Size(215, 22);
            this.txtUserID1.TabIndex = 5;
            this.txtUserID1.Tag = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(7, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 14);
            this.label5.TabIndex = 4;
            this.label5.Text = "UserId";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(7, 134);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 14);
            this.label6.TabIndex = 2;
            this.label6.Text = "DataBase";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(7, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 14);
            this.label7.TabIndex = 0;
            this.label7.Text = "Server";
            // 
            // rtResponce
            // 
            this.rtResponce.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtResponce.BackColor = System.Drawing.Color.WhiteSmoke;
            this.rtResponce.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtResponce.ForeColor = System.Drawing.Color.White;
            this.rtResponce.Location = new System.Drawing.Point(3, 5);
            this.rtResponce.Name = "rtResponce";
            this.rtResponce.Size = new System.Drawing.Size(492, 290);
            this.rtResponce.TabIndex = 3;
            this.rtResponce.Text = "";
            // 
            // lblserverandport
            // 
            this.lblserverandport.BackColor = System.Drawing.Color.Transparent;
            this.lblserverandport.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblserverandport.ForeColor = System.Drawing.Color.Black;
            this.lblserverandport.Location = new System.Drawing.Point(677, 413);
            this.lblserverandport.Name = "lblserverandport";
            this.lblserverandport.Size = new System.Drawing.Size(194, 25);
            this.lblserverandport.TabIndex = 65;
            this.lblserverandport.Text = "Server And Port";
            this.lblserverandport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(520, 416);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(156, 18);
            this.label10.TabIndex = 69;
            this.label10.Text = "Server IP && Port :";
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(828, 374);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(53, 20);
            this.txtMessage.TabIndex = 9;
            this.txtMessage.Visible = false;
            this.txtMessage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMessage_KeyPress);
            // 
            // cmdStop
            // 
            this.cmdStop.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmdStop.Enabled = false;
            this.cmdStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdStop.Image = ((System.Drawing.Image)(resources.GetObject("cmdStop.Image")));
            this.cmdStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdStop.Location = new System.Drawing.Point(205, 404);
            this.cmdStop.Name = "cmdStop";
            this.cmdStop.Size = new System.Drawing.Size(131, 42);
            this.cmdStop.TabIndex = 5;
            this.cmdStop.Text = "Disconnect";
            this.cmdStop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdStop.UseVisualStyleBackColor = true;
            this.cmdStop.Click += new System.EventHandler(this.cmdStop_Click);
            // 
            // cmdStart
            // 
            this.cmdStart.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmdStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdStart.ForeColor = System.Drawing.Color.Black;
            this.cmdStart.Image = ((System.Drawing.Image)(resources.GetObject("cmdStart.Image")));
            this.cmdStart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdStart.Location = new System.Drawing.Point(89, 404);
            this.cmdStart.Name = "cmdStart";
            this.cmdStart.Size = new System.Drawing.Size(110, 42);
            this.cmdStart.TabIndex = 1;
            this.cmdStart.Text = "Connect";
            this.cmdStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdStart.UseVisualStyleBackColor = true;
            this.cmdStart.Click += new System.EventHandler(this.cmdStart_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.Image")));
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(342, 404);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(174, 42);
            this.btnExit.TabIndex = 73;
            this.btnExit.Text = "Exit Application";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(877, 399);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(179, 42);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 74;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(0, 398);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(88, 92);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 75;
            this.pictureBox2.TabStop = false;
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.ForestGreen;
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHeader.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(0, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(1056, 36);
            this.lblHeader.TabIndex = 76;
            this.lblHeader.Text = "GREENPLY CONVEYOR COMMUNICATION SERVER";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Verdana", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(873, 445);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(185, 14);
            this.label12.TabIndex = 77;
            this.label12.Text = "© 2020-22 Bar Code India";
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblStatus.Enabled = false;
            this.lblStatus.Font = new System.Drawing.Font("Verdana", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.Black;
            this.lblStatus.Location = new System.Drawing.Point(91, 456);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(423, 31);
            this.lblStatus.TabIndex = 79;
            this.lblStatus.Text = "Application Message Here";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCount
            // 
            this.lblCount.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblCount.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCount.ForeColor = System.Drawing.Color.Black;
            this.lblCount.Location = new System.Drawing.Point(903, 464);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(135, 24);
            this.lblCount.TabIndex = 80;
            this.lblCount.Text = "Version 3.0.1";
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1056, 490);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.lblserverandport);
            this.Controls.Add(this.Messagetab);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.cmdStop);
            this.Controls.Add(this.cmdStart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1072, 529);
            this.MinimumSize = new System.Drawing.Size(1072, 529);
            this.Name = "frmMain";
            this.Text = "Greenply Conveyor Communication Server @Version 3.0.1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Messagetab.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.pnlLocalDBSetting.ResumeLayout(false);
            this.pnlLocalDBSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdStart;
        private System.Windows.Forms.Button cmdStop;
        private System.Windows.Forms.ListView lstData;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.TabControl Messagetab;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label lblserverandport;
        private System.Windows.Forms.Panel pnlLocalDBSetting;
        private System.Windows.Forms.TextBox txtSAPPassword;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtSAPId;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtConString;
        private System.Windows.Forms.Button cmdTestCon1;
        private System.Windows.Forms.ComboBox cmbSchema1;
        private System.Windows.Forms.ComboBox cmbServer1;
        private System.Windows.Forms.Button cmdSave1;
        private System.Windows.Forms.TextBox txtPwd1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUserID1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RichTextBox rtResponce;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblCount;
    }
}

