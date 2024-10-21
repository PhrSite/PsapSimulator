namespace PsapSimulator
{
    partial class SipRecRecorderSettingsForm
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
            SaveBtn = new Button();
            CancelBtn = new Button();
            label1 = new Label();
            NameTb = new TextBox();
            EnabledCb = new CheckBox();
            label2 = new Label();
            SipTransportCombo = new ComboBox();
            label3 = new Label();
            IPEndpointTb = new TextBox();
            groupBox4 = new GroupBox();
            MsrpEncryptionCombo = new ComboBox();
            RtpEncryptionCombo = new ComboBox();
            label13 = new Label();
            label12 = new Label();
            LocalIpEndpointTb = new TextBox();
            label4 = new Label();
            groupBox1 = new GroupBox();
            label6 = new Label();
            label5 = new Label();
            OptionsIntervalTb = new TextBox();
            OptionsCheck = new CheckBox();
            groupBox4.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // SaveBtn
            // 
            SaveBtn.Location = new Point(528, 564);
            SaveBtn.Name = "SaveBtn";
            SaveBtn.Size = new Size(94, 45);
            SaveBtn.TabIndex = 12;
            SaveBtn.Text = "Save";
            SaveBtn.UseVisualStyleBackColor = true;
            SaveBtn.Click += SaveBtn_Click;
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(418, 564);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(94, 45);
            CancelBtn.TabIndex = 13;
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(17, 24);
            label1.Name = "label1";
            label1.Size = new Size(75, 31);
            label1.TabIndex = 2;
            label1.Text = "Name";
            // 
            // NameTb
            // 
            NameTb.Location = new Point(116, 21);
            NameTb.Name = "NameTb";
            NameTb.Size = new Size(269, 38);
            NameTb.TabIndex = 1;
            // 
            // EnabledCb
            // 
            EnabledCb.AutoSize = true;
            EnabledCb.Location = new Point(418, 24);
            EnabledCb.Name = "EnabledCb";
            EnabledCb.Size = new Size(119, 35);
            EnabledCb.TabIndex = 2;
            EnabledCb.Text = "Enabled";
            EnabledCb.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(418, 96);
            label2.Name = "label2";
            label2.Size = new Size(147, 31);
            label2.TabIndex = 5;
            label2.Text = "SIP Transport";
            // 
            // SipTransportCombo
            // 
            SipTransportCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            SipTransportCombo.FormattingEnabled = true;
            SipTransportCombo.Items.AddRange(new object[] { "UDP", "TCP", "TLS" });
            SipTransportCombo.Location = new Point(418, 130);
            SipTransportCombo.Name = "SipTransportCombo";
            SipTransportCombo.Size = new Size(151, 39);
            SipTransportCombo.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(17, 96);
            label3.Name = "label3";
            label3.Size = new Size(176, 31);
            label3.TabIndex = 7;
            label3.Text = "SRS IP Endpoint";
            // 
            // IPEndpointTb
            // 
            IPEndpointTb.Location = new Point(17, 130);
            IPEndpointTb.Name = "IPEndpointTb";
            IPEndpointTb.Size = new Size(368, 38);
            IPEndpointTb.TabIndex = 3;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(MsrpEncryptionCombo);
            groupBox4.Controls.Add(RtpEncryptionCombo);
            groupBox4.Controls.Add(label13);
            groupBox4.Controls.Add(label12);
            groupBox4.Location = new Point(12, 320);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(324, 184);
            groupBox4.TabIndex = 6;
            groupBox4.TabStop = false;
            groupBox4.Text = "Media Encryption";
            // 
            // MsrpEncryptionCombo
            // 
            MsrpEncryptionCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            MsrpEncryptionCombo.FormattingEnabled = true;
            MsrpEncryptionCombo.Items.AddRange(new object[] { "None", "MSRP over TLS" });
            MsrpEncryptionCombo.Location = new Point(161, 101);
            MsrpEncryptionCombo.Name = "MsrpEncryptionCombo";
            MsrpEncryptionCombo.Size = new Size(139, 39);
            MsrpEncryptionCombo.TabIndex = 8;
            // 
            // RtpEncryptionCombo
            // 
            RtpEncryptionCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            RtpEncryptionCombo.FormattingEnabled = true;
            RtpEncryptionCombo.Items.AddRange(new object[] { "None", "SDES-SRTP", "DTLS-SRTP" });
            RtpEncryptionCombo.Location = new Point(161, 56);
            RtpEncryptionCombo.Name = "RtpEncryptionCombo";
            RtpEncryptionCombo.Size = new Size(139, 39);
            RtpEncryptionCombo.TabIndex = 7;
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
            // LocalIpEndpointTb
            // 
            LocalIpEndpointTb.Location = new Point(17, 235);
            LocalIpEndpointTb.Name = "LocalIpEndpointTb";
            LocalIpEndpointTb.Size = new Size(368, 38);
            LocalIpEndpointTb.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(17, 201);
            label4.Name = "label4";
            label4.Size = new Size(191, 31);
            label4.TabIndex = 11;
            label4.Text = "Local IP Endpoint";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(OptionsIntervalTb);
            groupBox1.Controls.Add(OptionsCheck);
            groupBox1.Location = new Point(357, 320);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(265, 184);
            groupBox1.TabIndex = 9;
            groupBox1.TabStop = false;
            groupBox1.Text = "SIP OPTIONS";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(153, 121);
            label6.Name = "label6";
            label6.Size = new Size(109, 31);
            label6.TabIndex = 3;
            label6.Text = "(5 - 3600)";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(22, 84);
            label5.Name = "label5";
            label5.Size = new Size(195, 31);
            label5.TabIndex = 2;
            label5.Text = "Interval (Seconds)";
            // 
            // OptionsIntervalTb
            // 
            OptionsIntervalTb.Location = new Point(22, 121);
            OptionsIntervalTb.Name = "OptionsIntervalTb";
            OptionsIntervalTb.Size = new Size(125, 38);
            OptionsIntervalTb.TabIndex = 11;
            // 
            // OptionsCheck
            // 
            OptionsCheck.AutoSize = true;
            OptionsCheck.Location = new Point(22, 37);
            OptionsCheck.Name = "OptionsCheck";
            OptionsCheck.Size = new Size(105, 35);
            OptionsCheck.TabIndex = 10;
            OptionsCheck.Text = "Enable";
            OptionsCheck.UseVisualStyleBackColor = true;
            // 
            // SipRecRecorderSettingsForm
            // 
            AutoScaleDimensions = new SizeF(13F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(643, 621);
            ControlBox = false;
            Controls.Add(groupBox1);
            Controls.Add(label4);
            Controls.Add(LocalIpEndpointTb);
            Controls.Add(groupBox4);
            Controls.Add(IPEndpointTb);
            Controls.Add(label3);
            Controls.Add(SipTransportCombo);
            Controls.Add(label2);
            Controls.Add(EnabledCb);
            Controls.Add(NameTb);
            Controls.Add(label1);
            Controls.Add(CancelBtn);
            Controls.Add(SaveBtn);
            Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(5);
            Name = "SipRecRecorderSettingsForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SIPREC RecorderSettings";
            Load += SipRecRecorderSettingsForm_Load;
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button SaveBtn;
        private Button CancelBtn;
        private Label label1;
        private TextBox NameTb;
        private CheckBox EnabledCb;
        private Label label2;
        private ComboBox SipTransportCombo;
        private Label label3;
        private TextBox IPEndpointTb;
        private GroupBox groupBox4;
        private ComboBox MsrpEncryptionCombo;
        private ComboBox RtpEncryptionCombo;
        private Label label13;
        private Label label12;
        private TextBox LocalIpEndpointTb;
        private Label label4;
        private GroupBox groupBox1;
        private Label label6;
        private Label label5;
        private TextBox OptionsIntervalTb;
        private CheckBox OptionsCheck;
    }
}