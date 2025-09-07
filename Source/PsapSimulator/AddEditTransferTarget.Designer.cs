namespace PsapSimulator
{
    partial class AddEditTransferTarget
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
            label1 = new Label();
            NameTb = new TextBox();
            label2 = new Label();
            SipUriTb = new TextBox();
            SuspendLayout();
            // 
            // OkBtn
            // 
            OkBtn.AutoSize = true;
            OkBtn.Location = new Point(260, 164);
            OkBtn.Name = "OkBtn";
            OkBtn.Size = new Size(94, 43);
            OkBtn.TabIndex = 2;
            OkBtn.Text = "OK";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // CancelBtn
            // 
            CancelBtn.AutoSize = true;
            CancelBtn.Location = new Point(382, 165);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(94, 41);
            CancelBtn.TabIndex = 3;
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(23, 27);
            label1.Name = "label1";
            label1.Size = new Size(156, 31);
            label1.TabIndex = 2;
            label1.Text = "Display Name";
            // 
            // NameTb
            // 
            NameTb.Location = new Point(186, 27);
            NameTb.Name = "NameTb";
            NameTb.Size = new Size(542, 38);
            NameTb.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(23, 81);
            label2.Name = "label2";
            label2.Size = new Size(87, 31);
            label2.TabIndex = 4;
            label2.Text = "SIP URI";
            // 
            // SipUriTb
            // 
            SipUriTb.Location = new Point(186, 78);
            SipUriTb.Name = "SipUriTb";
            SipUriTb.Size = new Size(542, 38);
            SipUriTb.TabIndex = 1;
            // 
            // AddEditTransferTarget
            // 
            AutoScaleDimensions = new SizeF(13F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(740, 230);
            ControlBox = false;
            Controls.Add(SipUriTb);
            Controls.Add(label2);
            Controls.Add(NameTb);
            Controls.Add(label1);
            Controls.Add(CancelBtn);
            Controls.Add(OkBtn);
            Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(5);
            Name = "AddEditTransferTarget";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AddEditTransferTarget";
            Load += AddEditTransferTarget_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button OkBtn;
        private Button CancelBtn;
        private Label label1;
        private TextBox NameTb;
        private Label label2;
        private TextBox SipUriTb;
    }
}