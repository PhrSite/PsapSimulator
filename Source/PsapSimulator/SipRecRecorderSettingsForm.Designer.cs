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
            groupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // SaveBtn
            // 
            SaveBtn.Location = new Point(475, 401);
            SaveBtn.Name = "SaveBtn";
            SaveBtn.Size = new Size(94, 45);
            SaveBtn.TabIndex = 8;
            SaveBtn.Text = "Save";
            SaveBtn.UseVisualStyleBackColor = true;
            SaveBtn.Click += SaveBtn_Click;
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(356, 401);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(94, 45);
            CancelBtn.TabIndex = 9;
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
            label3.Size = new Size(132, 31);
            label3.TabIndex = 7;
            label3.Text = "IP Endpoint";
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
            groupBox4.Location = new Point(17, 203);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(368, 159);
            groupBox4.TabIndex = 5;
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
            MsrpEncryptionCombo.Size = new Size(185, 39);
            MsrpEncryptionCombo.TabIndex = 7;
            // 
            // RtpEncryptionCombo
            // 
            RtpEncryptionCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            RtpEncryptionCombo.FormattingEnabled = true;
            RtpEncryptionCombo.Items.AddRange(new object[] { "None", "SDES-SRTP", "DTLS-SRTP" });
            RtpEncryptionCombo.Location = new Point(161, 56);
            RtpEncryptionCombo.Name = "RtpEncryptionCombo";
            RtpEncryptionCombo.Size = new Size(185, 39);
            RtpEncryptionCombo.TabIndex = 6;
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
            // SipRecRecorderSettingsForm
            // 
            AutoScaleDimensions = new SizeF(13F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(590, 458);
            ControlBox = false;
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
    }
}