namespace PsapSimulator
{
    partial class EventLoggerForm
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
            CancelBtn = new Button();
            EnabledCb = new CheckBox();
            NameTb = new TextBox();
            label1 = new Label();
            label2 = new Label();
            LoggerUriTb = new TextBox();
            OkBtn = new Button();
            HelpBtn = new Button();
            SuspendLayout();
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(349, 272);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(94, 45);
            CancelBtn.TabIndex = 5;
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // EnabledCb
            // 
            EnabledCb.AutoSize = true;
            EnabledCb.Location = new Point(15, 95);
            EnabledCb.Name = "EnabledCb";
            EnabledCb.Size = new Size(119, 35);
            EnabledCb.TabIndex = 2;
            EnabledCb.Text = "Enabled";
            EnabledCb.UseVisualStyleBackColor = true;
            // 
            // NameTb
            // 
            NameTb.Location = new Point(114, 24);
            NameTb.Name = "NameTb";
            NameTb.Size = new Size(269, 38);
            NameTb.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 27);
            label1.Name = "label1";
            label1.Size = new Size(75, 31);
            label1.TabIndex = 14;
            label1.Text = "Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 165);
            label2.Name = "label2";
            label2.Size = new Size(128, 31);
            label2.TabIndex = 15;
            label2.Text = "Logger URI";
            // 
            // LoggerUriTb
            // 
            LoggerUriTb.Location = new Point(15, 199);
            LoggerUriTb.Name = "LoggerUriTb";
            LoggerUriTb.Size = new Size(563, 38);
            LoggerUriTb.TabIndex = 3;
            // 
            // OkBtn
            // 
            OkBtn.AutoSize = true;
            OkBtn.Location = new Point(483, 272);
            OkBtn.Name = "OkBtn";
            OkBtn.Size = new Size(94, 45);
            OkBtn.TabIndex = 16;
            OkBtn.Text = "OK";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // HelpBtn
            // 
            HelpBtn.AutoSize = true;
            HelpBtn.Location = new Point(215, 272);
            HelpBtn.Name = "HelpBtn";
            HelpBtn.Size = new Size(94, 45);
            HelpBtn.TabIndex = 17;
            HelpBtn.Text = "Help";
            HelpBtn.UseVisualStyleBackColor = true;
            HelpBtn.Click += HelpBtn_Click;
            // 
            // EventLoggerForm
            // 
            AutoScaleDimensions = new SizeF(13F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(590, 342);
            ControlBox = false;
            Controls.Add(HelpBtn);
            Controls.Add(OkBtn);
            Controls.Add(LoggerUriTb);
            Controls.Add(label2);
            Controls.Add(EnabledCb);
            Controls.Add(NameTb);
            Controls.Add(label1);
            Controls.Add(CancelBtn);
            Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(5);
            Name = "EventLoggerForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "NG9-1-1 Event Logger Settings";
            Load += EventLoggerForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button CancelBtn;
        private CheckBox EnabledCb;
        private TextBox NameTb;
        private Label label1;
        private Label label2;
        private TextBox LoggerUriTb;
        private Button OkBtn;
        private Button HelpBtn;
    }
}