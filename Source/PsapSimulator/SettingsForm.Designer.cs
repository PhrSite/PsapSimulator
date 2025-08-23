namespace PsapSimulator
{
    partial class SettingsForm
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
            tabControl1 = new TabControl();
            NetworkPage = new TabPage();
            MutualAuthCheck = new CheckBox();
            EnableTlsCb = new CheckBox();
            EnableTcpCb = new CheckBox();
            EnableUdpCb = new CheckBox();
            SipsPortTb = new MaskedTextBox();
            SipPortTb = new MaskedTextBox();
            PortsGridView = new DataGridView();
            MediaCol = new DataGridViewTextBoxColumn();
            StartPortCol = new DataGridViewTextBoxColumn();
            PortCountCol = new DataGridViewTextBoxColumn();
            label4 = new Label();
            label3 = new Label();
            IPv6Group = new GroupBox();
            CopyIpV6Btn = new Button();
            IPv6Combo = new ComboBox();
            label2 = new Label();
            IPv6Check = new CheckBox();
            IPv4Group = new GroupBox();
            CopyIpV4Btn = new Button();
            IPv4Combo = new ComboBox();
            label1 = new Label();
            IPv4Check = new CheckBox();
            IdentityPage = new TabPage();
            groupBox2 = new GroupBox();
            CertPasswordTb = new TextBox();
            label9 = new Label();
            CertFileBrowseBtn = new Button();
            CertFileTb = new TextBox();
            label8 = new Label();
            DefaultCertCb = new CheckBox();
            groupBox1 = new GroupBox();
            RestoreIdentityBtn = new Button();
            ElementIDTb = new TextBox();
            AgentIDTb = new TextBox();
            AgencyIDTb = new TextBox();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            CallHandlingPage = new TabPage();
            ConfSettingsBtn = new Button();
            NonInteractiveCallsTb = new MaskedTextBox();
            MaxCallsTb = new MaskedTextBox();
            groupBox4 = new GroupBox();
            MsrpEncryptionCombo = new ComboBox();
            RtpEncryptionCombo = new ComboBox();
            label13 = new Label();
            label12 = new Label();
            groupBox3 = new GroupBox();
            EnableTransmitVideoCb = new CheckBox();
            EnableMsrpCb = new CheckBox();
            EnableRttCb = new CheckBox();
            EnableVideoCb = new CheckBox();
            EnableAudioCb = new CheckBox();
            EnableAutoAnswerCb = new CheckBox();
            label11 = new Label();
            label10 = new Label();
            MediaSourcesPage = new TabPage();
            groupBox6 = new GroupBox();
            RestoreAutoAnswerDefaultsBtn = new Button();
            AutoAnswerVideoSelectBtn = new Button();
            AutoAnswerAudioSelectBtn = new Button();
            label24 = new Label();
            AutoAnswerTextRepeatTb = new MaskedTextBox();
            AutoAnswerVideoTb = new TextBox();
            AutoAnswerAudioTb = new TextBox();
            label23 = new Label();
            AutoAnswerTextTb = new TextBox();
            label22 = new Label();
            label21 = new Label();
            label20 = new Label();
            groupBox5 = new GroupBox();
            RestoreCallHoldDefaultsBtn = new Button();
            label19 = new Label();
            HoldTextRepeatTb = new MaskedTextBox();
            label18 = new Label();
            HoldTextTb = new TextBox();
            label17 = new Label();
            HoldVideoSelectBtn = new Button();
            HoldVideoTb = new TextBox();
            label16 = new Label();
            HoldAudioSelectBtn = new Button();
            HoldAudioTb = new TextBox();
            label15 = new Label();
            CallHoldCombo = new ComboBox();
            label14 = new Label();
            DevicesPage = new TabPage();
            groupBox8 = new GroupBox();
            VideoDevicesCombo = new ComboBox();
            label26 = new Label();
            VideoListView = new ListView();
            SubTypeHeader = new ColumnHeader();
            WidthHeader = new ColumnHeader();
            HeightHeader = new ColumnHeader();
            FpsHeader = new ColumnHeader();
            groupBox7 = new GroupBox();
            AudioDeviceCombo = new ComboBox();
            label25 = new Label();
            InterfacesPage = new TabPage();
            TestCallsEnabledLbl = new Label();
            TestCallSettingsBtn = new Button();
            EventLoggingLbl = new Label();
            SipRecLbl = new Label();
            EventLoggingBtn = new Button();
            SipRecBtn = new Button();
            SaveBtn = new Button();
            CancelBtn = new Button();
            tabControl1.SuspendLayout();
            NetworkPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PortsGridView).BeginInit();
            IPv6Group.SuspendLayout();
            IPv4Group.SuspendLayout();
            IdentityPage.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            CallHandlingPage.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            MediaSourcesPage.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox5.SuspendLayout();
            DevicesPage.SuspendLayout();
            groupBox8.SuspendLayout();
            groupBox7.SuspendLayout();
            InterfacesPage.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(NetworkPage);
            tabControl1.Controls.Add(IdentityPage);
            tabControl1.Controls.Add(CallHandlingPage);
            tabControl1.Controls.Add(MediaSourcesPage);
            tabControl1.Controls.Add(DevicesPage);
            tabControl1.Controls.Add(InterfacesPage);
            tabControl1.Location = new Point(16, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(847, 624);
            tabControl1.TabIndex = 0;
            // 
            // NetworkPage
            // 
            NetworkPage.Controls.Add(MutualAuthCheck);
            NetworkPage.Controls.Add(EnableTlsCb);
            NetworkPage.Controls.Add(EnableTcpCb);
            NetworkPage.Controls.Add(EnableUdpCb);
            NetworkPage.Controls.Add(SipsPortTb);
            NetworkPage.Controls.Add(SipPortTb);
            NetworkPage.Controls.Add(PortsGridView);
            NetworkPage.Controls.Add(label4);
            NetworkPage.Controls.Add(label3);
            NetworkPage.Controls.Add(IPv6Group);
            NetworkPage.Controls.Add(IPv4Group);
            NetworkPage.Location = new Point(4, 40);
            NetworkPage.Name = "NetworkPage";
            NetworkPage.Padding = new Padding(3);
            NetworkPage.Size = new Size(839, 580);
            NetworkPage.TabIndex = 0;
            NetworkPage.Text = " Network ";
            NetworkPage.UseVisualStyleBackColor = true;
            // 
            // MutualAuthCheck
            // 
            MutualAuthCheck.AutoSize = true;
            MutualAuthCheck.Location = new Point(311, 523);
            MutualAuthCheck.Name = "MutualAuthCheck";
            MutualAuthCheck.Size = new Size(388, 35);
            MutualAuthCheck.TabIndex = 12;
            MutualAuthCheck.Text = "Use Mutual SIP TLS Authentication";
            MutualAuthCheck.UseVisualStyleBackColor = true;
            // 
            // EnableTlsCb
            // 
            EnableTlsCb.AutoSize = true;
            EnableTlsCb.Location = new Point(28, 523);
            EnableTlsCb.Name = "EnableTlsCb";
            EnableTlsCb.Size = new Size(146, 35);
            EnableTlsCb.TabIndex = 11;
            EnableTlsCb.Text = "Enable TLS";
            EnableTlsCb.UseVisualStyleBackColor = true;
            // 
            // EnableTcpCb
            // 
            EnableTcpCb.AutoSize = true;
            EnableTcpCb.Location = new Point(28, 482);
            EnableTcpCb.Name = "EnableTcpCb";
            EnableTcpCb.Size = new Size(149, 35);
            EnableTcpCb.TabIndex = 10;
            EnableTcpCb.Text = "Enable TCP";
            EnableTcpCb.UseVisualStyleBackColor = true;
            // 
            // EnableUdpCb
            // 
            EnableUdpCb.AutoSize = true;
            EnableUdpCb.Location = new Point(28, 441);
            EnableUdpCb.Name = "EnableUdpCb";
            EnableUdpCb.Size = new Size(156, 35);
            EnableUdpCb.TabIndex = 9;
            EnableUdpCb.Text = "Enable UDP";
            EnableUdpCb.UseVisualStyleBackColor = true;
            // 
            // SipsPortTb
            // 
            SipsPortTb.Location = new Point(140, 380);
            SipsPortTb.Mask = "00000";
            SipsPortTb.Name = "SipsPortTb";
            SipsPortTb.Size = new Size(125, 38);
            SipsPortTb.TabIndex = 8;
            // 
            // SipPortTb
            // 
            SipPortTb.Location = new Point(140, 320);
            SipPortTb.Mask = "00000";
            SipPortTb.Name = "SipPortTb";
            SipPortTb.Size = new Size(125, 38);
            SipPortTb.TabIndex = 7;
            // 
            // PortsGridView
            // 
            PortsGridView.AllowUserToAddRows = false;
            PortsGridView.AllowUserToDeleteRows = false;
            PortsGridView.AllowUserToResizeColumns = false;
            PortsGridView.AllowUserToResizeRows = false;
            PortsGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            PortsGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            PortsGridView.BackgroundColor = SystemColors.Control;
            PortsGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            PortsGridView.Columns.AddRange(new DataGridViewColumn[] { MediaCol, StartPortCol, PortCountCol });
            PortsGridView.Location = new Point(311, 334);
            PortsGridView.MultiSelect = false;
            PortsGridView.Name = "PortsGridView";
            PortsGridView.RowHeadersVisible = false;
            PortsGridView.RowHeadersWidth = 51;
            PortsGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            PortsGridView.ScrollBars = ScrollBars.None;
            PortsGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
            PortsGridView.Size = new Size(495, 174);
            PortsGridView.TabIndex = 6;
            // 
            // MediaCol
            // 
            MediaCol.HeaderText = "MediaType";
            MediaCol.MinimumWidth = 6;
            MediaCol.Name = "MediaCol";
            MediaCol.ReadOnly = true;
            MediaCol.Resizable = DataGridViewTriState.False;
            MediaCol.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // StartPortCol
            // 
            StartPortCol.HeaderText = "Start Port";
            StartPortCol.MinimumWidth = 6;
            StartPortCol.Name = "StartPortCol";
            StartPortCol.Resizable = DataGridViewTriState.False;
            StartPortCol.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // PortCountCol
            // 
            PortCountCol.HeaderText = "Port Count";
            PortCountCol.MinimumWidth = 6;
            PortCountCol.Name = "PortCountCol";
            PortCountCol.Resizable = DataGridViewTriState.False;
            PortCountCol.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(21, 383);
            label4.Name = "label4";
            label4.Size = new Size(104, 31);
            label4.TabIndex = 4;
            label4.Text = "SIPS Port";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(21, 323);
            label3.Name = "label3";
            label3.Size = new Size(92, 31);
            label3.TabIndex = 2;
            label3.Text = "SIP Port";
            // 
            // IPv6Group
            // 
            IPv6Group.Controls.Add(CopyIpV6Btn);
            IPv6Group.Controls.Add(IPv6Combo);
            IPv6Group.Controls.Add(label2);
            IPv6Group.Controls.Add(IPv6Check);
            IPv6Group.Location = new Point(19, 174);
            IPv6Group.Name = "IPv6Group";
            IPv6Group.Size = new Size(800, 140);
            IPv6Group.TabIndex = 1;
            IPv6Group.TabStop = false;
            IPv6Group.Text = "IPv6";
            // 
            // CopyIpV6Btn
            // 
            CopyIpV6Btn.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            CopyIpV6Btn.Location = new Point(305, 92);
            CopyIpV6Btn.Name = "CopyIpV6Btn";
            CopyIpV6Btn.Size = new Size(94, 29);
            CopyIpV6Btn.TabIndex = 4;
            CopyIpV6Btn.Text = "Copy";
            CopyIpV6Btn.UseVisualStyleBackColor = true;
            CopyIpV6Btn.Click += CopyIpV6Btn_Click;
            // 
            // IPv6Combo
            // 
            IPv6Combo.DropDownStyle = ComboBoxStyle.DropDownList;
            IPv6Combo.FormattingEnabled = true;
            IPv6Combo.Location = new Point(305, 47);
            IPv6Combo.Name = "IPv6Combo";
            IPv6Combo.Size = new Size(482, 39);
            IPv6Combo.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(202, 50);
            label2.Name = "label2";
            label2.Size = new Size(97, 31);
            label2.TabIndex = 1;
            label2.Text = "Address";
            // 
            // IPv6Check
            // 
            IPv6Check.AutoSize = true;
            IPv6Check.Location = new Point(21, 49);
            IPv6Check.Name = "IPv6Check";
            IPv6Check.Size = new Size(153, 35);
            IPv6Check.TabIndex = 0;
            IPv6Check.Text = "Enable IPv6";
            IPv6Check.UseVisualStyleBackColor = true;
            // 
            // IPv4Group
            // 
            IPv4Group.Controls.Add(CopyIpV4Btn);
            IPv4Group.Controls.Add(IPv4Combo);
            IPv4Group.Controls.Add(label1);
            IPv4Group.Controls.Add(IPv4Check);
            IPv4Group.Location = new Point(17, 28);
            IPv4Group.Name = "IPv4Group";
            IPv4Group.Size = new Size(802, 140);
            IPv4Group.TabIndex = 0;
            IPv4Group.TabStop = false;
            IPv4Group.Text = "IPv4";
            // 
            // CopyIpV4Btn
            // 
            CopyIpV4Btn.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            CopyIpV4Btn.Location = new Point(307, 96);
            CopyIpV4Btn.Name = "CopyIpV4Btn";
            CopyIpV4Btn.Size = new Size(94, 29);
            CopyIpV4Btn.TabIndex = 3;
            CopyIpV4Btn.Text = "Copy";
            CopyIpV4Btn.UseVisualStyleBackColor = true;
            CopyIpV4Btn.Click += CopyIpV4Btn_Click;
            // 
            // IPv4Combo
            // 
            IPv4Combo.DropDownStyle = ComboBoxStyle.DropDownList;
            IPv4Combo.FormattingEnabled = true;
            IPv4Combo.Location = new Point(307, 47);
            IPv4Combo.Name = "IPv4Combo";
            IPv4Combo.Size = new Size(482, 39);
            IPv4Combo.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(204, 47);
            label1.Name = "label1";
            label1.Size = new Size(97, 31);
            label1.TabIndex = 1;
            label1.Text = "Address";
            // 
            // IPv4Check
            // 
            IPv4Check.AutoSize = true;
            IPv4Check.Location = new Point(23, 46);
            IPv4Check.Name = "IPv4Check";
            IPv4Check.Size = new Size(153, 35);
            IPv4Check.TabIndex = 0;
            IPv4Check.Text = "Enable IPv4";
            IPv4Check.UseVisualStyleBackColor = true;
            // 
            // IdentityPage
            // 
            IdentityPage.Controls.Add(groupBox2);
            IdentityPage.Controls.Add(groupBox1);
            IdentityPage.Location = new Point(4, 29);
            IdentityPage.Name = "IdentityPage";
            IdentityPage.Padding = new Padding(3);
            IdentityPage.Size = new Size(839, 591);
            IdentityPage.TabIndex = 1;
            IdentityPage.Text = " Identity ";
            IdentityPage.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(CertPasswordTb);
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(CertFileBrowseBtn);
            groupBox2.Controls.Add(CertFileTb);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(DefaultCertCb);
            groupBox2.Location = new Point(14, 322);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(819, 241);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "X.509 Certificate";
            // 
            // CertPasswordTb
            // 
            CertPasswordTb.Location = new Point(213, 156);
            CertPasswordTb.Name = "CertPasswordTb";
            CertPasswordTb.PasswordChar = '*';
            CertPasswordTb.Size = new Size(466, 38);
            CertPasswordTb.TabIndex = 5;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(18, 159);
            label9.Name = "label9";
            label9.Size = new Size(110, 31);
            label9.TabIndex = 4;
            label9.Text = "Password";
            // 
            // CertFileBrowseBtn
            // 
            CertFileBrowseBtn.Location = new Point(690, 107);
            CertFileBrowseBtn.Name = "CertFileBrowseBtn";
            CertFileBrowseBtn.Size = new Size(107, 42);
            CertFileBrowseBtn.TabIndex = 3;
            CertFileBrowseBtn.Text = "Browse";
            CertFileBrowseBtn.UseVisualStyleBackColor = true;
            CertFileBrowseBtn.Click += CertFileBrowseBtn_Click;
            // 
            // CertFileTb
            // 
            CertFileTb.Location = new Point(213, 105);
            CertFileTb.Name = "CertFileTb";
            CertFileTb.Size = new Size(466, 38);
            CertFileTb.TabIndex = 2;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(18, 108);
            label8.Name = "label8";
            label8.Size = new Size(159, 31);
            label8.TabIndex = 1;
            label8.Text = "Certificate File";
            // 
            // DefaultCertCb
            // 
            DefaultCertCb.AutoSize = true;
            DefaultCertCb.Location = new Point(18, 54);
            DefaultCertCb.Name = "DefaultCertCb";
            DefaultCertCb.Size = new Size(264, 35);
            DefaultCertCb.TabIndex = 0;
            DefaultCertCb.Text = "Use Default Certificate";
            DefaultCertCb.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(RestoreIdentityBtn);
            groupBox1.Controls.Add(ElementIDTb);
            groupBox1.Controls.Add(AgentIDTb);
            groupBox1.Controls.Add(AgencyIDTb);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label5);
            groupBox1.Location = new Point(16, 28);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(819, 275);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "NG9-1-1 Identity";
            // 
            // RestoreIdentityBtn
            // 
            RestoreIdentityBtn.Location = new Point(18, 206);
            RestoreIdentityBtn.Name = "RestoreIdentityBtn";
            RestoreIdentityBtn.Size = new Size(210, 44);
            RestoreIdentityBtn.TabIndex = 6;
            RestoreIdentityBtn.Text = "Restore Defaults";
            RestoreIdentityBtn.UseVisualStyleBackColor = true;
            RestoreIdentityBtn.Click += RestoreIdentityBtn_Click;
            // 
            // ElementIDTb
            // 
            ElementIDTb.Location = new Point(187, 147);
            ElementIDTb.Name = "ElementIDTb";
            ElementIDTb.Size = new Size(449, 38);
            ElementIDTb.TabIndex = 5;
            // 
            // AgentIDTb
            // 
            AgentIDTb.Location = new Point(187, 98);
            AgentIDTb.Name = "AgentIDTb";
            AgentIDTb.Size = new Size(449, 38);
            AgentIDTb.TabIndex = 4;
            // 
            // AgencyIDTb
            // 
            AgencyIDTb.Location = new Point(187, 45);
            AgencyIDTb.Name = "AgencyIDTb";
            AgencyIDTb.Size = new Size(449, 38);
            AgencyIDTb.TabIndex = 3;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(18, 150);
            label7.Name = "label7";
            label7.Size = new Size(125, 31);
            label7.TabIndex = 2;
            label7.Text = "Element ID";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(18, 101);
            label6.Name = "label6";
            label6.Size = new Size(104, 31);
            label6.TabIndex = 1;
            label6.Text = "Agent ID";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(18, 52);
            label5.Name = "label5";
            label5.Size = new Size(118, 31);
            label5.TabIndex = 0;
            label5.Text = "Agency ID";
            // 
            // CallHandlingPage
            // 
            CallHandlingPage.Controls.Add(ConfSettingsBtn);
            CallHandlingPage.Controls.Add(NonInteractiveCallsTb);
            CallHandlingPage.Controls.Add(MaxCallsTb);
            CallHandlingPage.Controls.Add(groupBox4);
            CallHandlingPage.Controls.Add(groupBox3);
            CallHandlingPage.Controls.Add(EnableAutoAnswerCb);
            CallHandlingPage.Controls.Add(label11);
            CallHandlingPage.Controls.Add(label10);
            CallHandlingPage.Location = new Point(4, 29);
            CallHandlingPage.Name = "CallHandlingPage";
            CallHandlingPage.Size = new Size(839, 591);
            CallHandlingPage.TabIndex = 2;
            CallHandlingPage.Text = " Call Handling ";
            CallHandlingPage.UseVisualStyleBackColor = true;
            // 
            // ConfSettingsBtn
            // 
            ConfSettingsBtn.Location = new Point(26, 386);
            ConfSettingsBtn.Name = "ConfSettingsBtn";
            ConfSettingsBtn.Size = new Size(331, 39);
            ConfSettingsBtn.TabIndex = 9;
            ConfSettingsBtn.Text = "Conference/Transfer Settings";
            ConfSettingsBtn.UseVisualStyleBackColor = true;
            ConfSettingsBtn.Click += ConfSettingsBtn_Click;
            // 
            // NonInteractiveCallsTb
            // 
            NonInteractiveCallsTb.Location = new Point(388, 80);
            NonInteractiveCallsTb.Mask = "000000";
            NonInteractiveCallsTb.Name = "NonInteractiveCallsTb";
            NonInteractiveCallsTb.Size = new Size(125, 38);
            NonInteractiveCallsTb.TabIndex = 8;
            NonInteractiveCallsTb.ValidatingType = typeof(int);
            // 
            // MaxCallsTb
            // 
            MaxCallsTb.Location = new Point(388, 26);
            MaxCallsTb.Mask = "000000";
            MaxCallsTb.Name = "MaxCallsTb";
            MaxCallsTb.Size = new Size(125, 38);
            MaxCallsTb.TabIndex = 7;
            MaxCallsTb.ValidatingType = typeof(int);
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(MsrpEncryptionCombo);
            groupBox4.Controls.Add(RtpEncryptionCombo);
            groupBox4.Controls.Add(label13);
            groupBox4.Controls.Add(label12);
            groupBox4.Location = new Point(388, 149);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(445, 159);
            groupBox4.TabIndex = 4;
            groupBox4.TabStop = false;
            groupBox4.Text = "Outgoing Call Encryption";
            // 
            // MsrpEncryptionCombo
            // 
            MsrpEncryptionCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            MsrpEncryptionCombo.FormattingEnabled = true;
            MsrpEncryptionCombo.Items.AddRange(new object[] { "None", "MSRP over TLS" });
            MsrpEncryptionCombo.Location = new Point(161, 105);
            MsrpEncryptionCombo.Name = "MsrpEncryptionCombo";
            MsrpEncryptionCombo.Size = new Size(207, 39);
            MsrpEncryptionCombo.TabIndex = 3;
            // 
            // RtpEncryptionCombo
            // 
            RtpEncryptionCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            RtpEncryptionCombo.FormattingEnabled = true;
            RtpEncryptionCombo.Items.AddRange(new object[] { "None", "SDES-SRTP", "DTLS-SRTP" });
            RtpEncryptionCombo.Location = new Point(161, 56);
            RtpEncryptionCombo.Name = "RtpEncryptionCombo";
            RtpEncryptionCombo.Size = new Size(207, 39);
            RtpEncryptionCombo.TabIndex = 2;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(15, 97);
            label13.Name = "label13";
            label13.Size = new Size(74, 31);
            label13.TabIndex = 1;
            label13.Text = "MSRP";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(15, 53);
            label12.Name = "label12";
            label12.Size = new Size(123, 31);
            label12.TabIndex = 0;
            label12.Text = "RTP Media";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(EnableTransmitVideoCb);
            groupBox3.Controls.Add(EnableMsrpCb);
            groupBox3.Controls.Add(EnableRttCb);
            groupBox3.Controls.Add(EnableVideoCb);
            groupBox3.Controls.Add(EnableAudioCb);
            groupBox3.Location = new Point(26, 149);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(331, 198);
            groupBox3.TabIndex = 3;
            groupBox3.TabStop = false;
            groupBox3.Text = "Enabled Media";
            // 
            // EnableTransmitVideoCb
            // 
            EnableTransmitVideoCb.AutoSize = true;
            EnableTransmitVideoCb.Location = new Point(26, 148);
            EnableTransmitVideoCb.Name = "EnableTransmitVideoCb";
            EnableTransmitVideoCb.Size = new Size(263, 35);
            EnableTransmitVideoCb.TabIndex = 4;
            EnableTransmitVideoCb.Text = "Enable Transmit Video";
            EnableTransmitVideoCb.UseVisualStyleBackColor = true;
            // 
            // EnableMsrpCb
            // 
            EnableMsrpCb.AutoSize = true;
            EnableMsrpCb.Location = new Point(215, 93);
            EnableMsrpCb.Name = "EnableMsrpCb";
            EnableMsrpCb.Size = new Size(96, 35);
            EnableMsrpCb.TabIndex = 3;
            EnableMsrpCb.Text = "MSRP";
            EnableMsrpCb.UseVisualStyleBackColor = true;
            // 
            // EnableRttCb
            // 
            EnableRttCb.AutoSize = true;
            EnableRttCb.Location = new Point(215, 52);
            EnableRttCb.Name = "EnableRttCb";
            EnableRttCb.Size = new Size(73, 35);
            EnableRttCb.TabIndex = 2;
            EnableRttCb.Text = "RTT";
            EnableRttCb.UseVisualStyleBackColor = true;
            // 
            // EnableVideoCb
            // 
            EnableVideoCb.AutoSize = true;
            EnableVideoCb.Location = new Point(23, 93);
            EnableVideoCb.Name = "EnableVideoCb";
            EnableVideoCb.Size = new Size(95, 35);
            EnableVideoCb.TabIndex = 1;
            EnableVideoCb.Text = "Video";
            EnableVideoCb.UseVisualStyleBackColor = true;
            // 
            // EnableAudioCb
            // 
            EnableAudioCb.AutoSize = true;
            EnableAudioCb.Location = new Point(23, 52);
            EnableAudioCb.Name = "EnableAudioCb";
            EnableAudioCb.Size = new Size(97, 35);
            EnableAudioCb.TabIndex = 0;
            EnableAudioCb.Text = "Audio";
            EnableAudioCb.UseVisualStyleBackColor = true;
            // 
            // EnableAutoAnswerCb
            // 
            EnableAutoAnswerCb.AutoSize = true;
            EnableAutoAnswerCb.Location = new Point(549, 22);
            EnableAutoAnswerCb.Name = "EnableAutoAnswerCb";
            EnableAutoAnswerCb.Size = new Size(241, 35);
            EnableAutoAnswerCb.TabIndex = 2;
            EnableAutoAnswerCb.Text = "Enable Auto Answer";
            EnableAutoAnswerCb.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(26, 83);
            label11.Name = "label11";
            label11.Size = new Size(336, 31);
            label11.TabIndex = 1;
            label11.Text = "Maximum Non-Interactive Calls";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(26, 29);
            label10.Name = "label10";
            label10.Size = new Size(171, 31);
            label10.TabIndex = 0;
            label10.Text = "Maximum Calls";
            // 
            // MediaSourcesPage
            // 
            MediaSourcesPage.AutoScroll = true;
            MediaSourcesPage.Controls.Add(groupBox6);
            MediaSourcesPage.Controls.Add(groupBox5);
            MediaSourcesPage.Location = new Point(4, 29);
            MediaSourcesPage.Name = "MediaSourcesPage";
            MediaSourcesPage.Size = new Size(839, 591);
            MediaSourcesPage.TabIndex = 3;
            MediaSourcesPage.Text = " Media Sources ";
            MediaSourcesPage.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(RestoreAutoAnswerDefaultsBtn);
            groupBox6.Controls.Add(AutoAnswerVideoSelectBtn);
            groupBox6.Controls.Add(AutoAnswerAudioSelectBtn);
            groupBox6.Controls.Add(label24);
            groupBox6.Controls.Add(AutoAnswerTextRepeatTb);
            groupBox6.Controls.Add(AutoAnswerVideoTb);
            groupBox6.Controls.Add(AutoAnswerAudioTb);
            groupBox6.Controls.Add(label23);
            groupBox6.Controls.Add(AutoAnswerTextTb);
            groupBox6.Controls.Add(label22);
            groupBox6.Controls.Add(label21);
            groupBox6.Controls.Add(label20);
            groupBox6.Location = new Point(15, 343);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(790, 266);
            groupBox6.TabIndex = 1;
            groupBox6.TabStop = false;
            groupBox6.Text = "Auto Answer";
            // 
            // RestoreAutoAnswerDefaultsBtn
            // 
            RestoreAutoAnswerDefaultsBtn.Location = new Point(402, 212);
            RestoreAutoAnswerDefaultsBtn.Name = "RestoreAutoAnswerDefaultsBtn";
            RestoreAutoAnswerDefaultsBtn.Size = new Size(232, 38);
            RestoreAutoAnswerDefaultsBtn.TabIndex = 18;
            RestoreAutoAnswerDefaultsBtn.Text = "Restore Defaults";
            RestoreAutoAnswerDefaultsBtn.UseVisualStyleBackColor = true;
            RestoreAutoAnswerDefaultsBtn.Click += RestoreAutoAnswerDefaultsBtn_Click;
            // 
            // AutoAnswerVideoSelectBtn
            // 
            AutoAnswerVideoSelectBtn.Location = new Point(640, 90);
            AutoAnswerVideoSelectBtn.Name = "AutoAnswerVideoSelectBtn";
            AutoAnswerVideoSelectBtn.Size = new Size(125, 40);
            AutoAnswerVideoSelectBtn.TabIndex = 17;
            AutoAnswerVideoSelectBtn.Text = "Select";
            AutoAnswerVideoSelectBtn.UseVisualStyleBackColor = true;
            AutoAnswerVideoSelectBtn.Click += AutoAnswerVideoSelectBtn_Click;
            // 
            // AutoAnswerAudioSelectBtn
            // 
            AutoAnswerAudioSelectBtn.Location = new Point(640, 45);
            AutoAnswerAudioSelectBtn.Name = "AutoAnswerAudioSelectBtn";
            AutoAnswerAudioSelectBtn.Size = new Size(125, 40);
            AutoAnswerAudioSelectBtn.TabIndex = 16;
            AutoAnswerAudioSelectBtn.Text = "Select";
            AutoAnswerAudioSelectBtn.UseVisualStyleBackColor = true;
            AutoAnswerAudioSelectBtn.Click += AutoAnswerAudioSelectBtn_Click;
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Location = new Point(269, 219);
            label24.Name = "label24";
            label24.Size = new Size(99, 31);
            label24.TabIndex = 15;
            label24.Text = "Seconds";
            // 
            // AutoAnswerTextRepeatTb
            // 
            AutoAnswerTextRepeatTb.Location = new Point(180, 216);
            AutoAnswerTextRepeatTb.Mask = "000";
            AutoAnswerTextRepeatTb.Name = "AutoAnswerTextRepeatTb";
            AutoAnswerTextRepeatTb.Size = new Size(83, 38);
            AutoAnswerTextRepeatTb.TabIndex = 14;
            // 
            // AutoAnswerVideoTb
            // 
            AutoAnswerVideoTb.Location = new Point(180, 95);
            AutoAnswerVideoTb.Name = "AutoAnswerVideoTb";
            AutoAnswerVideoTb.Size = new Size(454, 38);
            AutoAnswerVideoTb.TabIndex = 13;
            // 
            // AutoAnswerAudioTb
            // 
            AutoAnswerAudioTb.Location = new Point(180, 50);
            AutoAnswerAudioTb.Name = "AutoAnswerAudioTb";
            AutoAnswerAudioTb.Size = new Size(454, 38);
            AutoAnswerAudioTb.TabIndex = 12;
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Location = new Point(16, 219);
            label23.Name = "label23";
            label23.Size = new Size(132, 31);
            label23.TabIndex = 11;
            label23.Text = "Text Repeat";
            // 
            // AutoAnswerTextTb
            // 
            AutoAnswerTextTb.Location = new Point(180, 144);
            AutoAnswerTextTb.Multiline = true;
            AutoAnswerTextTb.Name = "AutoAnswerTextTb";
            AutoAnswerTextTb.Size = new Size(469, 62);
            AutoAnswerTextTb.TabIndex = 10;
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(12, 144);
            label22.Name = "label22";
            label22.Size = new Size(152, 31);
            label22.TabIndex = 2;
            label22.Text = "Text Message";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(16, 95);
            label21.Name = "label21";
            label21.Size = new Size(114, 31);
            label21.TabIndex = 1;
            label21.Text = "Video File";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(16, 50);
            label20.Name = "label20";
            label20.Size = new Size(116, 31);
            label20.TabIndex = 0;
            label20.Text = "Audio File";
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(RestoreCallHoldDefaultsBtn);
            groupBox5.Controls.Add(label19);
            groupBox5.Controls.Add(HoldTextRepeatTb);
            groupBox5.Controls.Add(label18);
            groupBox5.Controls.Add(HoldTextTb);
            groupBox5.Controls.Add(label17);
            groupBox5.Controls.Add(HoldVideoSelectBtn);
            groupBox5.Controls.Add(HoldVideoTb);
            groupBox5.Controls.Add(label16);
            groupBox5.Controls.Add(HoldAudioSelectBtn);
            groupBox5.Controls.Add(HoldAudioTb);
            groupBox5.Controls.Add(label15);
            groupBox5.Controls.Add(CallHoldCombo);
            groupBox5.Controls.Add(label14);
            groupBox5.Location = new Point(15, 14);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(790, 312);
            groupBox5.TabIndex = 0;
            groupBox5.TabStop = false;
            groupBox5.Text = "Call Hold";
            // 
            // RestoreCallHoldDefaultsBtn
            // 
            RestoreCallHoldDefaultsBtn.Location = new Point(402, 259);
            RestoreCallHoldDefaultsBtn.Name = "RestoreCallHoldDefaultsBtn";
            RestoreCallHoldDefaultsBtn.Size = new Size(232, 38);
            RestoreCallHoldDefaultsBtn.TabIndex = 13;
            RestoreCallHoldDefaultsBtn.Text = "Restore Defaults";
            RestoreCallHoldDefaultsBtn.UseVisualStyleBackColor = true;
            RestoreCallHoldDefaultsBtn.Click += RestoreCallHoldDefaultsBtn_Click;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(269, 262);
            label19.Name = "label19";
            label19.Size = new Size(99, 31);
            label19.TabIndex = 12;
            label19.Text = "Seconds";
            // 
            // HoldTextRepeatTb
            // 
            HoldTextRepeatTb.Location = new Point(180, 259);
            HoldTextRepeatTb.Mask = "000";
            HoldTextRepeatTb.Name = "HoldTextRepeatTb";
            HoldTextRepeatTb.Size = new Size(83, 38);
            HoldTextRepeatTb.TabIndex = 11;
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(14, 262);
            label18.Name = "label18";
            label18.Size = new Size(132, 31);
            label18.TabIndex = 10;
            label18.Text = "Text Repeat";
            // 
            // HoldTextTb
            // 
            HoldTextTb.Location = new Point(180, 185);
            HoldTextTb.Multiline = true;
            HoldTextTb.Name = "HoldTextTb";
            HoldTextTb.Size = new Size(454, 62);
            HoldTextTb.TabIndex = 9;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(14, 188);
            label17.Name = "label17";
            label17.Size = new Size(152, 31);
            label17.TabIndex = 8;
            label17.Text = "Text Message";
            // 
            // HoldVideoSelectBtn
            // 
            HoldVideoSelectBtn.Location = new Point(640, 133);
            HoldVideoSelectBtn.Name = "HoldVideoSelectBtn";
            HoldVideoSelectBtn.Size = new Size(125, 40);
            HoldVideoSelectBtn.TabIndex = 7;
            HoldVideoSelectBtn.Text = "Select";
            HoldVideoSelectBtn.UseVisualStyleBackColor = true;
            HoldVideoSelectBtn.Click += HoldVideoSelectBtn_Click;
            // 
            // HoldVideoTb
            // 
            HoldVideoTb.Location = new Point(180, 135);
            HoldVideoTb.Name = "HoldVideoTb";
            HoldVideoTb.Size = new Size(454, 38);
            HoldVideoTb.TabIndex = 6;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(14, 138);
            label16.Name = "label16";
            label16.Size = new Size(114, 31);
            label16.TabIndex = 5;
            label16.Text = "Video File";
            // 
            // HoldAudioSelectBtn
            // 
            HoldAudioSelectBtn.Location = new Point(640, 88);
            HoldAudioSelectBtn.Name = "HoldAudioSelectBtn";
            HoldAudioSelectBtn.Size = new Size(125, 39);
            HoldAudioSelectBtn.TabIndex = 4;
            HoldAudioSelectBtn.Text = "Select";
            HoldAudioSelectBtn.UseVisualStyleBackColor = true;
            HoldAudioSelectBtn.Click += HoldAudioSelectBtn_Click;
            // 
            // HoldAudioTb
            // 
            HoldAudioTb.Location = new Point(180, 85);
            HoldAudioTb.Name = "HoldAudioTb";
            HoldAudioTb.Size = new Size(454, 38);
            HoldAudioTb.TabIndex = 3;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(14, 88);
            label15.Name = "label15";
            label15.Size = new Size(116, 31);
            label15.TabIndex = 2;
            label15.Text = "Audio File";
            // 
            // CallHoldCombo
            // 
            CallHoldCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            CallHoldCombo.FormattingEnabled = true;
            CallHoldCombo.Items.AddRange(new object[] { "Silence", "Hold Beep Sound", "Hold Recording File" });
            CallHoldCombo.Location = new Point(180, 38);
            CallHoldCombo.Name = "CallHoldCombo";
            CallHoldCombo.Size = new Size(600, 39);
            CallHoldCombo.TabIndex = 1;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(14, 41);
            label14.Name = "label14";
            label14.Size = new Size(150, 31);
            label14.TabIndex = 0;
            label14.Text = "Audio Source";
            // 
            // DevicesPage
            // 
            DevicesPage.Controls.Add(groupBox8);
            DevicesPage.Controls.Add(groupBox7);
            DevicesPage.Location = new Point(4, 29);
            DevicesPage.Name = "DevicesPage";
            DevicesPage.Size = new Size(839, 591);
            DevicesPage.TabIndex = 4;
            DevicesPage.Text = " Devices ";
            DevicesPage.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            groupBox8.Controls.Add(VideoDevicesCombo);
            groupBox8.Controls.Add(label26);
            groupBox8.Controls.Add(VideoListView);
            groupBox8.Location = new Point(29, 195);
            groupBox8.Name = "groupBox8";
            groupBox8.Size = new Size(790, 336);
            groupBox8.TabIndex = 1;
            groupBox8.TabStop = false;
            groupBox8.Text = "Video";
            // 
            // VideoDevicesCombo
            // 
            VideoDevicesCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            VideoDevicesCombo.FormattingEnabled = true;
            VideoDevicesCombo.Location = new Point(202, 49);
            VideoDevicesCombo.Name = "VideoDevicesCombo";
            VideoDevicesCombo.Size = new Size(547, 39);
            VideoDevicesCombo.TabIndex = 2;
            VideoDevicesCombo.SelectedIndexChanged += VideoDevicesCombo_SelectedIndexChanged;
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Location = new Point(25, 52);
            label26.Name = "label26";
            label26.Size = new Size(157, 31);
            label26.TabIndex = 1;
            label26.Text = "Video Devices";
            // 
            // VideoListView
            // 
            VideoListView.CheckBoxes = true;
            VideoListView.Columns.AddRange(new ColumnHeader[] { SubTypeHeader, WidthHeader, HeightHeader, FpsHeader });
            VideoListView.FullRowSelect = true;
            VideoListView.GridLines = true;
            VideoListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            VideoListView.HideSelection = true;
            VideoListView.Location = new Point(20, 106);
            VideoListView.MultiSelect = false;
            VideoListView.Name = "VideoListView";
            VideoListView.Size = new Size(729, 210);
            VideoListView.TabIndex = 0;
            VideoListView.UseCompatibleStateImageBehavior = false;
            VideoListView.View = View.Details;
            VideoListView.ItemCheck += VideoListView_ItemCheck;
            // 
            // SubTypeHeader
            // 
            SubTypeHeader.Text = "Sub Type";
            SubTypeHeader.Width = 100;
            // 
            // WidthHeader
            // 
            WidthHeader.Text = "Width";
            WidthHeader.Width = 100;
            // 
            // HeightHeader
            // 
            HeightHeader.Text = "Height";
            HeightHeader.Width = 100;
            // 
            // FpsHeader
            // 
            FpsHeader.Text = "Frames/Sec.";
            FpsHeader.Width = 100;
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(AudioDeviceCombo);
            groupBox7.Controls.Add(label25);
            groupBox7.Location = new Point(29, 32);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(790, 125);
            groupBox7.TabIndex = 0;
            groupBox7.TabStop = false;
            groupBox7.Text = "Audio";
            // 
            // AudioDeviceCombo
            // 
            AudioDeviceCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            AudioDeviceCombo.FormattingEnabled = true;
            AudioDeviceCombo.Location = new Point(303, 50);
            AudioDeviceCombo.Name = "AudioDeviceCombo";
            AudioDeviceCombo.Size = new Size(469, 39);
            AudioDeviceCombo.TabIndex = 1;
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new Point(14, 53);
            label25.Name = "label25";
            label25.Size = new Size(259, 31);
            label25.TabIndex = 0;
            label25.Text = "Audio Input and Output";
            // 
            // InterfacesPage
            // 
            InterfacesPage.Controls.Add(TestCallsEnabledLbl);
            InterfacesPage.Controls.Add(TestCallSettingsBtn);
            InterfacesPage.Controls.Add(EventLoggingLbl);
            InterfacesPage.Controls.Add(SipRecLbl);
            InterfacesPage.Controls.Add(EventLoggingBtn);
            InterfacesPage.Controls.Add(SipRecBtn);
            InterfacesPage.Location = new Point(4, 29);
            InterfacesPage.Name = "InterfacesPage";
            InterfacesPage.Size = new Size(839, 591);
            InterfacesPage.TabIndex = 5;
            InterfacesPage.Text = " Interfaces ";
            InterfacesPage.UseVisualStyleBackColor = true;
            // 
            // TestCallsEnabledLbl
            // 
            TestCallsEnabledLbl.BorderStyle = BorderStyle.Fixed3D;
            TestCallsEnabledLbl.Location = new Point(335, 184);
            TestCallsEnabledLbl.Name = "TestCallsEnabledLbl";
            TestCallsEnabledLbl.Size = new Size(106, 39);
            TestCallsEnabledLbl.TabIndex = 5;
            TestCallsEnabledLbl.Text = "Enabled";
            // 
            // TestCallSettingsBtn
            // 
            TestCallSettingsBtn.Location = new Point(22, 184);
            TestCallSettingsBtn.Name = "TestCallSettingsBtn";
            TestCallSettingsBtn.Size = new Size(291, 39);
            TestCallSettingsBtn.TabIndex = 4;
            TestCallSettingsBtn.Text = "Test Call Settings";
            TestCallSettingsBtn.UseVisualStyleBackColor = true;
            TestCallSettingsBtn.Click += TestCallSettingsBtn_Click;
            // 
            // EventLoggingLbl
            // 
            EventLoggingLbl.AutoSize = true;
            EventLoggingLbl.BorderStyle = BorderStyle.Fixed3D;
            EventLoggingLbl.Location = new Point(335, 110);
            EventLoggingLbl.Name = "EventLoggingLbl";
            EventLoggingLbl.Size = new Size(106, 33);
            EventLoggingLbl.TabIndex = 3;
            EventLoggingLbl.Text = "Disabled";
            // 
            // SipRecLbl
            // 
            SipRecLbl.AutoSize = true;
            SipRecLbl.BorderStyle = BorderStyle.Fixed3D;
            SipRecLbl.Location = new Point(335, 31);
            SipRecLbl.Name = "SipRecLbl";
            SipRecLbl.Size = new Size(106, 33);
            SipRecLbl.TabIndex = 2;
            SipRecLbl.Text = "Disabled";
            // 
            // EventLoggingBtn
            // 
            EventLoggingBtn.Location = new Point(22, 105);
            EventLoggingBtn.Name = "EventLoggingBtn";
            EventLoggingBtn.Size = new Size(291, 41);
            EventLoggingBtn.TabIndex = 1;
            EventLoggingBtn.Text = "NG9-1-1 Event Logging";
            EventLoggingBtn.UseVisualStyleBackColor = true;
            EventLoggingBtn.Click += EventLoggingBtn_Click;
            // 
            // SipRecBtn
            // 
            SipRecBtn.Location = new Point(22, 31);
            SipRecBtn.Name = "SipRecBtn";
            SipRecBtn.Size = new Size(291, 41);
            SipRecBtn.TabIndex = 0;
            SipRecBtn.Text = "SIPREC Media Recording";
            SipRecBtn.UseVisualStyleBackColor = true;
            SipRecBtn.Click += SipRecBtn_Click;
            // 
            // SaveBtn
            // 
            SaveBtn.Location = new Point(782, 642);
            SaveBtn.Name = "SaveBtn";
            SaveBtn.Size = new Size(94, 44);
            SaveBtn.TabIndex = 1;
            SaveBtn.Text = "Save";
            SaveBtn.UseVisualStyleBackColor = true;
            SaveBtn.Click += SaveBtn_Click;
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(671, 642);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(94, 44);
            CancelBtn.TabIndex = 2;
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(13F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(908, 698);
            ControlBox = false;
            Controls.Add(CancelBtn);
            Controls.Add(SaveBtn);
            Controls.Add(tabControl1);
            Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(5);
            MinimizeBox = false;
            Name = "SettingsForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Settings";
            Load += SettingsForm_Load;
            tabControl1.ResumeLayout(false);
            NetworkPage.ResumeLayout(false);
            NetworkPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PortsGridView).EndInit();
            IPv6Group.ResumeLayout(false);
            IPv6Group.PerformLayout();
            IPv4Group.ResumeLayout(false);
            IPv4Group.PerformLayout();
            IdentityPage.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            CallHandlingPage.ResumeLayout(false);
            CallHandlingPage.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            MediaSourcesPage.ResumeLayout(false);
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            DevicesPage.ResumeLayout(false);
            groupBox8.ResumeLayout(false);
            groupBox8.PerformLayout();
            groupBox7.ResumeLayout(false);
            groupBox7.PerformLayout();
            InterfacesPage.ResumeLayout(false);
            InterfacesPage.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage NetworkPage;
        private TabPage IdentityPage;
        private Button SaveBtn;
        private Button CancelBtn;
        private GroupBox IPv4Group;
        private ComboBox IPv4Combo;
        private Label label1;
        private CheckBox IPv4Check;
        private GroupBox IPv6Group;
        private ComboBox IPv6Combo;
        private Label label2;
        private CheckBox IPv6Check;
        private Label label4;
        private Label label3;
        private DataGridView PortsGridView;
        private DataGridViewTextBoxColumn MediaCol;
        private DataGridViewTextBoxColumn StartPortCol;
        private DataGridViewTextBoxColumn PortCountCol;
        private GroupBox groupBox1;
        private TextBox AgentIDTb;
        private TextBox AgencyIDTb;
        private Label label7;
        private Label label6;
        private Label label5;
        private GroupBox groupBox2;
        private CheckBox DefaultCertCb;
        private TextBox ElementIDTb;
        private TextBox CertPasswordTb;
        private Label label9;
        private Button CertFileBrowseBtn;
        private TextBox CertFileTb;
        private Label label8;
        private TabPage CallHandlingPage;
        private Label label11;
        private Label label10;
        private GroupBox groupBox3;
        private CheckBox EnableMsrpCb;
        private CheckBox EnableRttCb;
        private CheckBox EnableVideoCb;
        private CheckBox EnableAudioCb;
        private CheckBox EnableAutoAnswerCb;
        private GroupBox groupBox4;
        private ComboBox RtpEncryptionCombo;
        private Label label13;
        private Label label12;
        private ComboBox MsrpEncryptionCombo;
        private CheckBox EnableTransmitVideoCb;
        private MaskedTextBox NonInteractiveCallsTb;
        private MaskedTextBox MaxCallsTb;
        private MaskedTextBox SipsPortTb;
        private MaskedTextBox SipPortTb;
        private TabPage MediaSourcesPage;
        private GroupBox groupBox6;
        private GroupBox groupBox5;
        private ComboBox CallHoldCombo;
        private Label label14;
        private TextBox HoldAudioTb;
        private Label label15;
        private Button HoldAudioSelectBtn;
        private Button HoldVideoSelectBtn;
        private TextBox HoldVideoTb;
        private Label label16;
        private TextBox HoldTextTb;
        private Label label17;
        private MaskedTextBox HoldTextRepeatTb;
        private Label label18;
        private Label label19;
        private Label label22;
        private Label label21;
        private Label label20;
        private TextBox AutoAnswerVideoTb;
        private TextBox AutoAnswerAudioTb;
        private Label label23;
        private TextBox AutoAnswerTextTb;
        private Button AutoAnswerVideoSelectBtn;
        private Button AutoAnswerAudioSelectBtn;
        private Label label24;
        private MaskedTextBox AutoAnswerTextRepeatTb;
        private TabPage DevicesPage;
        private GroupBox groupBox7;
        private ComboBox AudioDeviceCombo;
        private Label label25;
        private GroupBox groupBox8;
        private ListView VideoListView;
        private ColumnHeader SubTypeHeader;
        private ColumnHeader WidthHeader;
        private ColumnHeader HeightHeader;
        private ColumnHeader FpsHeader;
        private ComboBox VideoDevicesCombo;
        private Label label26;
        private CheckBox EnableTlsCb;
        private CheckBox EnableTcpCb;
        private CheckBox EnableUdpCb;
        private TabPage InterfacesPage;
        private Button EventLoggingBtn;
        private Button SipRecBtn;
        private Button RestoreIdentityBtn;
        private Button RestoreAutoAnswerDefaultsBtn;
        private Button RestoreCallHoldDefaultsBtn;
        private Label EventLoggingLbl;
        private Label SipRecLbl;
        private CheckBox MutualAuthCheck;
        private Button ConfSettingsBtn;
        private Button TestCallSettingsBtn;
        private Label TestCallsEnabledLbl;
        private Button CopyIpV4Btn;
        private Button CopyIpV6Btn;
    }
}