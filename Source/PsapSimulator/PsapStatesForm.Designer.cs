namespace PsapSimulator
{
    partial class PsapStatesForm
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
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            ElementStateCombo = new ComboBox();
            ServiceStateCombo = new ComboBox();
            SecurityPostureCombo = new ComboBox();
            QueueStateCombo = new ComboBox();
            NotifyBtn = new Button();
            SuspendLayout();
            // 
            // CloseBtn
            // 
            CloseBtn.Location = new Point(643, 303);
            CloseBtn.Name = "CloseBtn";
            CloseBtn.Size = new Size(94, 43);
            CloseBtn.TabIndex = 4;
            CloseBtn.Text = "Close";
            CloseBtn.UseVisualStyleBackColor = true;
            CloseBtn.Click += CloseBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 24);
            label1.Name = "label1";
            label1.Size = new Size(154, 31);
            label1.TabIndex = 1;
            label1.Text = "Element State";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(15, 94);
            label2.Name = "label2";
            label2.Size = new Size(143, 31);
            label2.TabIndex = 2;
            label2.Text = "Service State";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(15, 164);
            label3.Name = "label3";
            label3.Size = new Size(177, 31);
            label3.TabIndex = 3;
            label3.Text = "Security Posture";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(15, 234);
            label4.Name = "label4";
            label4.Size = new Size(138, 31);
            label4.TabIndex = 4;
            label4.Text = "Queue State";
            // 
            // ElementStateCombo
            // 
            ElementStateCombo.DropDownHeight = 300;
            ElementStateCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            ElementStateCombo.FormattingEnabled = true;
            ElementStateCombo.IntegralHeight = false;
            ElementStateCombo.Location = new Point(223, 24);
            ElementStateCombo.Name = "ElementStateCombo";
            ElementStateCombo.Size = new Size(514, 39);
            ElementStateCombo.TabIndex = 0;
            ElementStateCombo.SelectedIndexChanged += ElementStateCombo_SelectedIndexChanged;
            // 
            // ServiceStateCombo
            // 
            ServiceStateCombo.DropDownHeight = 300;
            ServiceStateCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            ServiceStateCombo.FormattingEnabled = true;
            ServiceStateCombo.IntegralHeight = false;
            ServiceStateCombo.Location = new Point(223, 94);
            ServiceStateCombo.Name = "ServiceStateCombo";
            ServiceStateCombo.Size = new Size(514, 39);
            ServiceStateCombo.TabIndex = 1;
            ServiceStateCombo.SelectedIndexChanged += ServiceStateCombo_SelectedIndexChanged;
            // 
            // SecurityPostureCombo
            // 
            SecurityPostureCombo.DropDownHeight = 300;
            SecurityPostureCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            SecurityPostureCombo.FormattingEnabled = true;
            SecurityPostureCombo.IntegralHeight = false;
            SecurityPostureCombo.Location = new Point(223, 164);
            SecurityPostureCombo.Name = "SecurityPostureCombo";
            SecurityPostureCombo.Size = new Size(514, 39);
            SecurityPostureCombo.TabIndex = 2;
            SecurityPostureCombo.SelectedIndexChanged += SecurityPostureCombo_SelectedIndexChanged;
            // 
            // QueueStateCombo
            // 
            QueueStateCombo.DropDownHeight = 300;
            QueueStateCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            QueueStateCombo.FormattingEnabled = true;
            QueueStateCombo.IntegralHeight = false;
            QueueStateCombo.Location = new Point(223, 234);
            QueueStateCombo.Name = "QueueStateCombo";
            QueueStateCombo.Size = new Size(514, 39);
            QueueStateCombo.TabIndex = 3;
            QueueStateCombo.SelectedIndexChanged += QueueStateCombo_SelectedIndexChanged;
            // 
            // NotifyBtn
            // 
            NotifyBtn.Location = new Point(532, 303);
            NotifyBtn.Name = "NotifyBtn";
            NotifyBtn.Size = new Size(105, 43);
            NotifyBtn.TabIndex = 5;
            NotifyBtn.Text = "Notify";
            NotifyBtn.UseVisualStyleBackColor = true;
            NotifyBtn.Click += NotifyBtn_Click;
            // 
            // PsapStatesForm
            // 
            AutoScaleDimensions = new SizeF(13F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(759, 362);
            ControlBox = false;
            Controls.Add(NotifyBtn);
            Controls.Add(QueueStateCombo);
            Controls.Add(SecurityPostureCombo);
            Controls.Add(ServiceStateCombo);
            Controls.Add(ElementStateCombo);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(CloseBtn);
            Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(5);
            Name = "PsapStatesForm";
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PSAP State Settings";
            Load += PsapStatesForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button CloseBtn;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private ComboBox ElementStateCombo;
        private ComboBox ServiceStateCombo;
        private ComboBox SecurityPostureCombo;
        private ComboBox QueueStateCombo;
        private Button NotifyBtn;
    }
}