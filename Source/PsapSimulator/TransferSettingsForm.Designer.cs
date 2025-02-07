namespace PsapSimulator
{
    partial class TransferSettingsForm
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
            CloseBtn = new Button();
            groupBox1 = new GroupBox();
            TargetsListView = new ListView();
            NameColumn = new ColumnHeader();
            UriColumn = new ColumnHeader();
            AddBtn = new Button();
            EditBtn = new Button();
            DeleteBtn = new Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // OkBtn
            // 
            OkBtn.Location = new Point(355, 464);
            OkBtn.Name = "OkBtn";
            OkBtn.Size = new Size(94, 39);
            OkBtn.TabIndex = 0;
            OkBtn.Text = "OK";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // CloseBtn
            // 
            CloseBtn.Location = new Point(496, 464);
            CloseBtn.Name = "CloseBtn";
            CloseBtn.Size = new Size(94, 39);
            CloseBtn.TabIndex = 1;
            CloseBtn.Text = "Cancel";
            CloseBtn.UseVisualStyleBackColor = true;
            CloseBtn.Click += CloseBtn_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(DeleteBtn);
            groupBox1.Controls.Add(EditBtn);
            groupBox1.Controls.Add(AddBtn);
            groupBox1.Controls.Add(TargetsListView);
            groupBox1.Location = new Point(17, 24);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(852, 343);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Transfer Targets";
            // 
            // TargetsListView
            // 
            TargetsListView.Columns.AddRange(new ColumnHeader[] { NameColumn, UriColumn });
            TargetsListView.FullRowSelect = true;
            TargetsListView.GridLines = true;
            TargetsListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            TargetsListView.Location = new Point(23, 56);
            TargetsListView.MultiSelect = false;
            TargetsListView.Name = "TargetsListView";
            TargetsListView.Size = new Size(790, 217);
            TargetsListView.TabIndex = 0;
            TargetsListView.UseCompatibleStateImageBehavior = false;
            TargetsListView.View = View.Details;
            TargetsListView.DoubleClick += TargetsListView_DoubleClick;
            // 
            // NameColumn
            // 
            NameColumn.Text = "Display Name";
            NameColumn.Width = 250;
            // 
            // UriColumn
            // 
            UriColumn.Text = "SIP URI";
            UriColumn.Width = 500;
            // 
            // AddBtn
            // 
            AddBtn.Location = new Point(23, 289);
            AddBtn.Name = "AddBtn";
            AddBtn.Size = new Size(94, 39);
            AddBtn.TabIndex = 1;
            AddBtn.Text = "Add";
            AddBtn.UseVisualStyleBackColor = true;
            AddBtn.Click += AddBtn_Click;
            // 
            // EditBtn
            // 
            EditBtn.Location = new Point(135, 291);
            EditBtn.Name = "EditBtn";
            EditBtn.Size = new Size(94, 37);
            EditBtn.TabIndex = 2;
            EditBtn.Text = "Edit";
            EditBtn.UseVisualStyleBackColor = true;
            EditBtn.Click += EditBtn_Click;
            // 
            // DeleteBtn
            // 
            DeleteBtn.Location = new Point(245, 291);
            DeleteBtn.Name = "DeleteBtn";
            DeleteBtn.Size = new Size(94, 37);
            DeleteBtn.TabIndex = 3;
            DeleteBtn.Text = "Delete";
            DeleteBtn.UseVisualStyleBackColor = true;
            DeleteBtn.Click += DeleteBtn_Click;
            // 
            // TransferSettingsForm
            // 
            AutoScaleDimensions = new SizeF(13F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(910, 515);
            ControlBox = false;
            Controls.Add(groupBox1);
            Controls.Add(CloseBtn);
            Controls.Add(OkBtn);
            Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(5);
            Name = "TransferSettingsForm";
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Conference/Transfer Settings";
            Load += TransferSettingsForm_Load;
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button OkBtn;
        private Button CloseBtn;
        private GroupBox groupBox1;
        private ListView TargetsListView;
        private ColumnHeader NameColumn;
        private Button DeleteBtn;
        private Button EditBtn;
        private Button AddBtn;
        private ColumnHeader UriColumn;
    }
}