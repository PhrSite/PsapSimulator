namespace PsapSimulator
{
    partial class CadIfSettingsForm
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
            CloseBtn = new Button();
            label1 = new Label();
            IPv4UrlLbl = new Label();
            label2 = new Label();
            IPv6UrlLbl = new Label();
            CopyIPv4Btn = new Button();
            CopyIPv6Btn = new Button();
            HelpBtn = new Button();
            SuspendLayout();
            // 
            // CloseBtn
            // 
            CloseBtn.AutoSize = true;
            CloseBtn.Location = new Point(905, 346);
            CloseBtn.Name = "CloseBtn";
            CloseBtn.Size = new Size(94, 41);
            CloseBtn.TabIndex = 0;
            CloseBtn.Text = "Close";
            CloseBtn.UseVisualStyleBackColor = true;
            CloseBtn.Click += CloseBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.Location = new Point(21, 27);
            label1.Name = "label1";
            label1.Size = new Size(58, 33);
            label1.TabIndex = 1;
            label1.Text = "IPv4";
            // 
            // IPv4UrlLbl
            // 
            IPv4UrlLbl.AutoSize = true;
            IPv4UrlLbl.BorderStyle = BorderStyle.FixedSingle;
            IPv4UrlLbl.Location = new Point(98, 29);
            IPv4UrlLbl.Name = "IPv4UrlLbl";
            IPv4UrlLbl.Size = new Size(143, 33);
            IPv4UrlLbl.TabIndex = 2;
            IPv4UrlLbl.Text = "Not Enabled";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BorderStyle = BorderStyle.FixedSingle;
            label2.Location = new Point(21, 176);
            label2.Name = "label2";
            label2.Size = new Size(58, 33);
            label2.TabIndex = 3;
            label2.Text = "IPv6";
            // 
            // IPv6UrlLbl
            // 
            IPv6UrlLbl.AutoSize = true;
            IPv6UrlLbl.BorderStyle = BorderStyle.FixedSingle;
            IPv6UrlLbl.Location = new Point(98, 176);
            IPv6UrlLbl.Name = "IPv6UrlLbl";
            IPv6UrlLbl.Size = new Size(143, 33);
            IPv6UrlLbl.TabIndex = 4;
            IPv6UrlLbl.Text = "Not Enabled";
            // 
            // CopyIPv4Btn
            // 
            CopyIPv4Btn.AutoSize = true;
            CopyIPv4Btn.Location = new Point(98, 84);
            CopyIPv4Btn.Name = "CopyIPv4Btn";
            CopyIPv4Btn.Size = new Size(94, 43);
            CopyIPv4Btn.TabIndex = 5;
            CopyIPv4Btn.Text = "Copy";
            CopyIPv4Btn.UseVisualStyleBackColor = true;
            CopyIPv4Btn.Click += CopyIPv4Btn_Click;
            // 
            // CopyIPv6Btn
            // 
            CopyIPv6Btn.AutoSize = true;
            CopyIPv6Btn.Location = new Point(98, 232);
            CopyIPv6Btn.Name = "CopyIPv6Btn";
            CopyIPv6Btn.Size = new Size(94, 43);
            CopyIPv6Btn.TabIndex = 6;
            CopyIPv6Btn.Text = "Copy";
            CopyIPv6Btn.UseVisualStyleBackColor = true;
            CopyIPv6Btn.Click += CopyIPv6Btn_Click;
            // 
            // HelpBtn
            // 
            HelpBtn.AutoSize = true;
            HelpBtn.Location = new Point(742, 346);
            HelpBtn.Name = "HelpBtn";
            HelpBtn.Size = new Size(94, 41);
            HelpBtn.TabIndex = 7;
            HelpBtn.Text = "Help";
            HelpBtn.UseVisualStyleBackColor = true;
            HelpBtn.Click += HelpBtn_Click;
            // 
            // CadIfSettingsForm
            // 
            AutoScaleDimensions = new SizeF(13F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(1036, 399);
            ControlBox = false;
            Controls.Add(HelpBtn);
            Controls.Add(CopyIPv6Btn);
            Controls.Add(CopyIPv4Btn);
            Controls.Add(IPv6UrlLbl);
            Controls.Add(label2);
            Controls.Add(IPv4UrlLbl);
            Controls.Add(label1);
            Controls.Add(CloseBtn);
            Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(5);
            Name = "CadIfSettingsForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CAD IF/EIDO Server Settings";
            Load += CadIfSettingsForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button CloseBtn;
        private Label label1;
        private Label IPv4UrlLbl;
        private Label label2;
        private Label IPv6UrlLbl;
        private Button CopyIPv4Btn;
        private Button CopyIPv6Btn;
        private Button HelpBtn;
    }
}