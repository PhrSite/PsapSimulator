namespace PsapSimulator
{
    partial class AddNewMediaForm
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
            checkedListBox1 = new CheckedListBox();
            SuspendLayout();
            // 
            // OkBtn
            // 
            OkBtn.AutoSize = true;
            OkBtn.Location = new Point(200, 287);
            OkBtn.Name = "OkBtn";
            OkBtn.Size = new Size(105, 43);
            OkBtn.TabIndex = 0;
            OkBtn.Text = "OK";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // CancelBtn
            // 
            CancelBtn.AutoSize = true;
            CancelBtn.Location = new Point(26, 287);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(105, 43);
            CancelBtn.TabIndex = 1;
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // checkedListBox1
            // 
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Location = new Point(53, 30);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new Size(225, 202);
            checkedListBox1.TabIndex = 2;
            // 
            // AddNewMediaForm
            // 
            AutoScaleDimensions = new SizeF(13F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(339, 367);
            ControlBox = false;
            Controls.Add(checkedListBox1);
            Controls.Add(CancelBtn);
            Controls.Add(OkBtn);
            Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(5);
            Name = "AddNewMediaForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Add New Media";
            Load += AddNewMediaForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button OkBtn;
        private Button CancelBtn;
        private CheckedListBox checkedListBox1;
    }
}