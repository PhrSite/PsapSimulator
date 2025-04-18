namespace PsapSimulator
{
    partial class TestCallSettingsForm
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
            SaveBtn = new Button();
            EnableTestCallsCheck = new CheckBox();
            label1 = new Label();
            MaxTestCallsTb = new TextBox();
            label2 = new Label();
            TestCallDurationUnitsCombo = new ComboBox();
            label3 = new Label();
            TestCallDurationTb = new TextBox();
            SuspendLayout();
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(256, 301);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(94, 45);
            CancelBtn.TabIndex = 15;
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // SaveBtn
            // 
            SaveBtn.Location = new Point(366, 301);
            SaveBtn.Name = "SaveBtn";
            SaveBtn.Size = new Size(94, 45);
            SaveBtn.TabIndex = 14;
            SaveBtn.Text = "Save";
            SaveBtn.UseVisualStyleBackColor = true;
            SaveBtn.Click += SaveBtn_Click;
            // 
            // EnableTestCallsCheck
            // 
            EnableTestCallsCheck.AutoSize = true;
            EnableTestCallsCheck.Location = new Point(29, 29);
            EnableTestCallsCheck.Name = "EnableTestCallsCheck";
            EnableTestCallsCheck.Size = new Size(205, 35);
            EnableTestCallsCheck.TabIndex = 16;
            EnableTestCallsCheck.Text = "Enable Test Calls";
            EnableTestCallsCheck.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(29, 83);
            label1.Name = "label1";
            label1.Size = new Size(217, 31);
            label1.TabIndex = 17;
            label1.Text = "Maximum Test Calls";
            // 
            // MaxTestCallsTb
            // 
            MaxTestCallsTb.Location = new Point(297, 83);
            MaxTestCallsTb.Name = "MaxTestCallsTb";
            MaxTestCallsTb.Size = new Size(125, 38);
            MaxTestCallsTb.TabIndex = 18;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(29, 140);
            label2.Name = "label2";
            label2.Size = new Size(252, 31);
            label2.TabIndex = 19;
            label2.Text = "Test Call Duration Units";
            // 
            // TestCallDurationUnitsCombo
            // 
            TestCallDurationUnitsCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            TestCallDurationUnitsCombo.FormattingEnabled = true;
            TestCallDurationUnitsCombo.Items.AddRange(new object[] { "RTP Packets", "Minutes" });
            TestCallDurationUnitsCombo.Location = new Point(297, 140);
            TestCallDurationUnitsCombo.Name = "TestCallDurationUnitsCombo";
            TestCallDurationUnitsCombo.Size = new Size(163, 39);
            TestCallDurationUnitsCombo.TabIndex = 20;
            TestCallDurationUnitsCombo.SelectedIndexChanged += TestCallDurationUnitsCombo_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(29, 203);
            label3.Name = "label3";
            label3.Size = new Size(193, 31);
            label3.TabIndex = 21;
            label3.Text = "Test Call Duration";
            // 
            // TestCallDurationTb
            // 
            TestCallDurationTb.Location = new Point(297, 203);
            TestCallDurationTb.Name = "TestCallDurationTb";
            TestCallDurationTb.Size = new Size(125, 38);
            TestCallDurationTb.TabIndex = 22;
            // 
            // TestCallSettingsForm
            // 
            AutoScaleDimensions = new SizeF(13F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(497, 366);
            ControlBox = false;
            Controls.Add(TestCallDurationTb);
            Controls.Add(label3);
            Controls.Add(TestCallDurationUnitsCombo);
            Controls.Add(label2);
            Controls.Add(MaxTestCallsTb);
            Controls.Add(label1);
            Controls.Add(EnableTestCallsCheck);
            Controls.Add(CancelBtn);
            Controls.Add(SaveBtn);
            Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(5);
            Name = "TestCallSettingsForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Test Call Settings";
            Load += TestCallSettingsForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button CancelBtn;
        private Button SaveBtn;
        private CheckBox EnableTestCallsCheck;
        private Label label1;
        private TextBox MaxTestCallsTb;
        private Label label2;
        private ComboBox TestCallDurationUnitsCombo;
        private Label label3;
        private TextBox TestCallDurationTb;
    }
}