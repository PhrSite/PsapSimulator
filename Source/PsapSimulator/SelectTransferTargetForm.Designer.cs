namespace PsapSimulator
{
    partial class SelectTransferTargetForm
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
            OkBtn = new Button();
            CancelBtn = new Button();
            TargetComboBox = new ComboBox();
            label1 = new Label();
            AddBtn = new Button();
            SuspendLayout();
            // 
            // OkBtn
            // 
            OkBtn.Location = new Point(201, 97);
            OkBtn.Name = "OkBtn";
            OkBtn.Size = new Size(94, 37);
            OkBtn.TabIndex = 1;
            OkBtn.Text = "OK";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(345, 97);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(94, 37);
            CancelBtn.TabIndex = 2;
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // TargetComboBox
            // 
            TargetComboBox.DropDownHeight = 130;
            TargetComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            TargetComboBox.FormattingEnabled = true;
            TargetComboBox.IntegralHeight = false;
            TargetComboBox.Location = new Point(181, 21);
            TargetComboBox.Name = "TargetComboBox";
            TargetComboBox.Size = new Size(317, 39);
            TargetComboBox.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 24);
            label1.Name = "label1";
            label1.Size = new Size(163, 31);
            label1.TabIndex = 3;
            label1.Text = "Transfer Target";
            // 
            // AddBtn
            // 
            AddBtn.Location = new Point(517, 25);
            AddBtn.Name = "AddBtn";
            AddBtn.Size = new Size(94, 35);
            AddBtn.TabIndex = 3;
            AddBtn.Text = "Add";
            AddBtn.UseVisualStyleBackColor = true;
            AddBtn.Click += AddBtn_Click;
            // 
            // SelectTransferTargetForm
            // 
            AutoScaleDimensions = new SizeF(13F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(640, 156);
            ControlBox = false;
            Controls.Add(AddBtn);
            Controls.Add(label1);
            Controls.Add(TargetComboBox);
            Controls.Add(CancelBtn);
            Controls.Add(OkBtn);
            Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(5);
            Name = "SelectTransferTargetForm";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Select a Transfer Target";
            Load += SelectTransferTargetForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button OkBtn;
        private Button CancelBtn;
        private ComboBox TargetComboBox;
        private Label label1;
        private Button AddBtn;
    }
}