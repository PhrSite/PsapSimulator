namespace PsapSimulator
{
    partial class SipRecForm
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
            EnableSipRecCheck = new CheckBox();
            RecListView = new ListView();
            NameHeader = new ColumnHeader();
            EnabledHeader = new ColumnHeader();
            EndpointHeader = new ColumnHeader();
            SipTransportHeader = new ColumnHeader();
            AddBtn = new Button();
            DeleteBtn = new Button();
            EditBtn = new Button();
            SuspendLayout();
            // 
            // SaveBtn
            // 
            SaveBtn.Location = new Point(720, 413);
            SaveBtn.Name = "SaveBtn";
            SaveBtn.Size = new Size(94, 42);
            SaveBtn.TabIndex = 0;
            SaveBtn.Text = "Save";
            SaveBtn.UseVisualStyleBackColor = true;
            SaveBtn.Click += SaveBtn_Click;
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(607, 413);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(94, 42);
            CancelBtn.TabIndex = 3;
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // EnableSipRecCheck
            // 
            EnableSipRecCheck.AutoSize = true;
            EnableSipRecCheck.Location = new Point(27, 12);
            EnableSipRecCheck.Name = "EnableSipRecCheck";
            EnableSipRecCheck.Size = new Size(182, 35);
            EnableSipRecCheck.TabIndex = 4;
            EnableSipRecCheck.Text = "Enable SIPREC";
            EnableSipRecCheck.UseVisualStyleBackColor = true;
            // 
            // RecListView
            // 
            RecListView.Columns.AddRange(new ColumnHeader[] { NameHeader, EnabledHeader, EndpointHeader, SipTransportHeader });
            RecListView.FullRowSelect = true;
            RecListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            RecListView.Location = new Point(27, 53);
            RecListView.MultiSelect = false;
            RecListView.Name = "RecListView";
            RecListView.Size = new Size(787, 304);
            RecListView.TabIndex = 5;
            RecListView.UseCompatibleStateImageBehavior = false;
            RecListView.View = View.Details;
            RecListView.DoubleClick += RecListView_DoubleClick;
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
            // EndpointHeader
            // 
            EndpointHeader.Text = "IP Endpoint";
            EndpointHeader.Width = 200;
            // 
            // SipTransportHeader
            // 
            SipTransportHeader.Text = "SIP Transport";
            SipTransportHeader.Width = 200;
            // 
            // AddBtn
            // 
            AddBtn.Location = new Point(27, 363);
            AddBtn.Name = "AddBtn";
            AddBtn.Size = new Size(94, 44);
            AddBtn.TabIndex = 6;
            AddBtn.Text = "Add";
            AddBtn.UseVisualStyleBackColor = true;
            AddBtn.Click += AddBtn_Click;
            // 
            // DeleteBtn
            // 
            DeleteBtn.Location = new Point(263, 363);
            DeleteBtn.Name = "DeleteBtn";
            DeleteBtn.Size = new Size(94, 44);
            DeleteBtn.TabIndex = 7;
            DeleteBtn.Text = "Delete";
            DeleteBtn.UseVisualStyleBackColor = true;
            DeleteBtn.Click += DeleteBtn_Click;
            // 
            // EditBtn
            // 
            EditBtn.Location = new Point(146, 363);
            EditBtn.Name = "EditBtn";
            EditBtn.Size = new Size(94, 44);
            EditBtn.TabIndex = 8;
            EditBtn.Text = "Edit";
            EditBtn.UseVisualStyleBackColor = true;
            EditBtn.Click += EditBtn_Click;
            // 
            // SipRecForm
            // 
            AutoScaleDimensions = new SizeF(13F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(834, 467);
            ControlBox = false;
            Controls.Add(EditBtn);
            Controls.Add(DeleteBtn);
            Controls.Add(AddBtn);
            Controls.Add(RecListView);
            Controls.Add(EnableSipRecCheck);
            Controls.Add(CancelBtn);
            Controls.Add(SaveBtn);
            Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(5);
            Name = "SipRecForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SIPREC Recording";
            Load += SipRecForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button SaveBtn;
        private Button CancelBtn;
        private CheckBox EnableSipRecCheck;
        private ListView RecListView;
        private ColumnHeader NameHeader;
        private ColumnHeader EnabledHeader;
        private ColumnHeader EndpointHeader;
        private ColumnHeader SipTransportHeader;
        private Button AddBtn;
        private Button DeleteBtn;
        private Button EditBtn;
    }
}