namespace PsapSimulator
{
    partial class EventLoggingForm
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
            EditBtn = new Button();
            DeleteBtn = new Button();
            AddBtn = new Button();
            LoggerListView = new ListView();
            NameHeader = new ColumnHeader();
            EnabledHeader = new ColumnHeader();
            UriHeader = new ColumnHeader();
            EnableLoggingCheck = new CheckBox();
            SuspendLayout();
            // 
            // CancelBtn
            // 
            CancelBtn.AutoSize = true;
            CancelBtn.Location = new Point(602, 413);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(94, 42);
            CancelBtn.TabIndex = 5;
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // SaveBtn
            // 
            SaveBtn.AutoSize = true;
            SaveBtn.Location = new Point(728, 413);
            SaveBtn.Name = "SaveBtn";
            SaveBtn.Size = new Size(94, 42);
            SaveBtn.TabIndex = 4;
            SaveBtn.Text = "Save";
            SaveBtn.UseVisualStyleBackColor = true;
            SaveBtn.Click += SaveBtn_Click;
            // 
            // EditBtn
            // 
            EditBtn.AutoSize = true;
            EditBtn.Location = new Point(145, 358);
            EditBtn.Name = "EditBtn";
            EditBtn.Size = new Size(94, 44);
            EditBtn.TabIndex = 11;
            EditBtn.Text = "Edit";
            EditBtn.UseVisualStyleBackColor = true;
            EditBtn.Click += EditBtn_Click;
            // 
            // DeleteBtn
            // 
            DeleteBtn.AutoSize = true;
            DeleteBtn.Location = new Point(262, 358);
            DeleteBtn.Name = "DeleteBtn";
            DeleteBtn.Size = new Size(94, 44);
            DeleteBtn.TabIndex = 10;
            DeleteBtn.Text = "Delete";
            DeleteBtn.UseVisualStyleBackColor = true;
            DeleteBtn.Click += DeleteBtn_Click;
            // 
            // AddBtn
            // 
            AddBtn.AutoSize = true;
            AddBtn.Location = new Point(26, 358);
            AddBtn.Name = "AddBtn";
            AddBtn.Size = new Size(94, 44);
            AddBtn.TabIndex = 9;
            AddBtn.Text = "Add";
            AddBtn.UseVisualStyleBackColor = true;
            AddBtn.Click += AddBtn_Click;
            // 
            // LoggerListView
            // 
            LoggerListView.Columns.AddRange(new ColumnHeader[] { NameHeader, EnabledHeader, UriHeader });
            LoggerListView.FullRowSelect = true;
            LoggerListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            LoggerListView.Location = new Point(22, 67);
            LoggerListView.MultiSelect = false;
            LoggerListView.Name = "LoggerListView";
            LoggerListView.Size = new Size(787, 285);
            LoggerListView.TabIndex = 12;
            LoggerListView.UseCompatibleStateImageBehavior = false;
            LoggerListView.View = View.Details;
            LoggerListView.DoubleClick += LoggerListView_DoubleClick;
            // 
            // NameHeader
            // 
            NameHeader.Text = "Name";
            NameHeader.Width = 200;
            // 
            // EnabledHeader
            // 
            EnabledHeader.Text = "Enabled?";
            EnabledHeader.Width = 150;
            // 
            // UriHeader
            // 
            UriHeader.Text = "Logger URI";
            UriHeader.Width = 400;
            // 
            // EnableLoggingCheck
            // 
            EnableLoggingCheck.AutoSize = true;
            EnableLoggingCheck.Location = new Point(22, 12);
            EnableLoggingCheck.Name = "EnableLoggingCheck";
            EnableLoggingCheck.Size = new Size(258, 35);
            EnableLoggingCheck.TabIndex = 13;
            EnableLoggingCheck.Text = "Enable Event Logging";
            EnableLoggingCheck.UseVisualStyleBackColor = true;
            // 
            // EventLoggingForm
            // 
            AutoScaleDimensions = new SizeF(13F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(834, 467);
            ControlBox = false;
            Controls.Add(EnableLoggingCheck);
            Controls.Add(LoggerListView);
            Controls.Add(EditBtn);
            Controls.Add(DeleteBtn);
            Controls.Add(AddBtn);
            Controls.Add(CancelBtn);
            Controls.Add(SaveBtn);
            Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(5);
            Name = "EventLoggingForm";
            ShowIcon = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "NG9-1-1 Event Logging Settings";
            Load += EventLoggingForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button CancelBtn;
        private Button SaveBtn;
        private Button EditBtn;
        private Button DeleteBtn;
        private Button AddBtn;
        private ListView LoggerListView;
        private ColumnHeader NameHeader;
        private ColumnHeader EnabledHeader;
        private ColumnHeader UriHeader;
        private CheckBox EnableLoggingCheck;
    }
}