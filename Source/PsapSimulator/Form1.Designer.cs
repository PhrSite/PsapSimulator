namespace PsapSimulator
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            CloseBtn = new Button();
            SettingsBtn = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            CallListView = new ListView();
            FromHeader = new ColumnHeader();
            TimeHeader = new ColumnHeader();
            State = new ColumnHeader();
            QueueUriHeader = new ColumnHeader();
            ConferenceHeader = new ColumnHeader();
            MediaHeader = new ColumnHeader();
            StartBtn = new Button();
            label1 = new Label();
            TotalCallsLbl = new Label();
            label2 = new Label();
            RingingLbl = new Label();
            label4 = new Label();
            AnsweredLbl = new Label();
            label6 = new Label();
            HoldLbl = new Label();
            AnswerBtn = new Button();
            ShowBtn = new Button();
            EndCallBtn = new Button();
            EndAllBtn = new Button();
            HoldBtn = new Button();
            label5 = new Label();
            OnLineLbl = new Label();
            StatusLbl = new Label();
            toolTip1 = new ToolTip(components);
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // CloseBtn
            // 
            CloseBtn.Dock = DockStyle.Fill;
            CloseBtn.Location = new Point(1173, 659);
            CloseBtn.Name = "CloseBtn";
            CloseBtn.Size = new Size(124, 36);
            CloseBtn.TabIndex = 0;
            CloseBtn.Text = "Close";
            CloseBtn.UseVisualStyleBackColor = true;
            CloseBtn.Click += CloseBtn_Click;
            // 
            // SettingsBtn
            // 
            SettingsBtn.Dock = DockStyle.Fill;
            SettingsBtn.Location = new Point(1043, 659);
            SettingsBtn.Name = "SettingsBtn";
            SettingsBtn.Size = new Size(124, 36);
            SettingsBtn.TabIndex = 1;
            SettingsBtn.Text = "Settings";
            SettingsBtn.UseVisualStyleBackColor = true;
            SettingsBtn.Click += SettingsBtn_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 10;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.Controls.Add(CloseBtn, 9, 3);
            tableLayoutPanel1.Controls.Add(SettingsBtn, 8, 3);
            tableLayoutPanel1.Controls.Add(CallListView, 0, 1);
            tableLayoutPanel1.Controls.Add(StartBtn, 0, 3);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(TotalCallsLbl, 1, 0);
            tableLayoutPanel1.Controls.Add(label2, 2, 0);
            tableLayoutPanel1.Controls.Add(RingingLbl, 3, 0);
            tableLayoutPanel1.Controls.Add(label4, 4, 0);
            tableLayoutPanel1.Controls.Add(AnsweredLbl, 5, 0);
            tableLayoutPanel1.Controls.Add(label6, 8, 0);
            tableLayoutPanel1.Controls.Add(HoldLbl, 9, 0);
            tableLayoutPanel1.Controls.Add(AnswerBtn, 1, 3);
            tableLayoutPanel1.Controls.Add(ShowBtn, 2, 3);
            tableLayoutPanel1.Controls.Add(EndCallBtn, 4, 3);
            tableLayoutPanel1.Controls.Add(EndAllBtn, 5, 3);
            tableLayoutPanel1.Controls.Add(HoldBtn, 3, 3);
            tableLayoutPanel1.Controls.Add(label5, 6, 0);
            tableLayoutPanel1.Controls.Add(OnLineLbl, 7, 0);
            tableLayoutPanel1.Controls.Add(StatusLbl, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tableLayoutPanel1.Size = new Size(1300, 698);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // CallListView
            // 
            CallListView.Columns.AddRange(new ColumnHeader[] { FromHeader, TimeHeader, State, QueueUriHeader, ConferenceHeader, MediaHeader });
            tableLayoutPanel1.SetColumnSpan(CallListView, 10);
            CallListView.Dock = DockStyle.Fill;
            CallListView.FullRowSelect = true;
            CallListView.GridLines = true;
            CallListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            CallListView.Location = new Point(3, 45);
            CallListView.MultiSelect = false;
            CallListView.Name = "CallListView";
            CallListView.Size = new Size(1294, 566);
            CallListView.TabIndex = 2;
            CallListView.UseCompatibleStateImageBehavior = false;
            CallListView.View = View.Details;
            CallListView.MouseDoubleClick += CallListView_MouseDoubleClick;
            // 
            // FromHeader
            // 
            FromHeader.Text = "From";
            FromHeader.Width = 150;
            // 
            // TimeHeader
            // 
            TimeHeader.Text = "Time";
            TimeHeader.Width = 100;
            // 
            // State
            // 
            State.Text = "State";
            State.Width = 100;
            // 
            // QueueUriHeader
            // 
            QueueUriHeader.Text = "Queue URI";
            QueueUriHeader.Width = 200;
            // 
            // ConferenceHeader
            // 
            ConferenceHeader.Text = "Conferenced?";
            ConferenceHeader.Width = 150;
            // 
            // MediaHeader
            // 
            MediaHeader.Text = "Media Available";
            MediaHeader.Width = 200;
            // 
            // StartBtn
            // 
            StartBtn.Dock = DockStyle.Fill;
            StartBtn.Location = new Point(3, 659);
            StartBtn.Name = "StartBtn";
            StartBtn.Size = new Size(124, 36);
            StartBtn.TabIndex = 3;
            StartBtn.Text = "Start";
            toolTip1.SetToolTip(StartBtn, "Starts or stop listening for calls");
            StartBtn.UseVisualStyleBackColor = true;
            StartBtn.Click += StartBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(124, 42);
            label1.TabIndex = 4;
            label1.Text = "Total Calls";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // TotalCallsLbl
            // 
            TotalCallsLbl.BorderStyle = BorderStyle.Fixed3D;
            TotalCallsLbl.Dock = DockStyle.Fill;
            TotalCallsLbl.Location = new Point(133, 0);
            TotalCallsLbl.Name = "TotalCallsLbl";
            TotalCallsLbl.Size = new Size(124, 42);
            TotalCallsLbl.TabIndex = 5;
            TotalCallsLbl.Text = "0";
            TotalCallsLbl.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(263, 0);
            label2.Name = "label2";
            label2.Size = new Size(124, 42);
            label2.TabIndex = 6;
            label2.Text = "Ringing";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // RingingLbl
            // 
            RingingLbl.BorderStyle = BorderStyle.Fixed3D;
            RingingLbl.Dock = DockStyle.Fill;
            RingingLbl.Location = new Point(393, 0);
            RingingLbl.Name = "RingingLbl";
            RingingLbl.Size = new Size(124, 42);
            RingingLbl.TabIndex = 7;
            RingingLbl.Text = "0";
            RingingLbl.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Fill;
            label4.Location = new Point(523, 0);
            label4.Name = "label4";
            label4.Size = new Size(124, 42);
            label4.TabIndex = 8;
            label4.Text = "Answered";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AnsweredLbl
            // 
            AnsweredLbl.BorderStyle = BorderStyle.Fixed3D;
            AnsweredLbl.Dock = DockStyle.Fill;
            AnsweredLbl.Location = new Point(653, 0);
            AnsweredLbl.Name = "AnsweredLbl";
            AnsweredLbl.Size = new Size(124, 42);
            AnsweredLbl.TabIndex = 9;
            AnsweredLbl.Text = "0";
            AnsweredLbl.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Fill;
            label6.Location = new Point(1043, 0);
            label6.Name = "label6";
            label6.Size = new Size(124, 42);
            label6.TabIndex = 10;
            label6.Text = "Hold";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // HoldLbl
            // 
            HoldLbl.BorderStyle = BorderStyle.Fixed3D;
            HoldLbl.Dock = DockStyle.Fill;
            HoldLbl.Location = new Point(1173, 0);
            HoldLbl.Name = "HoldLbl";
            HoldLbl.Size = new Size(124, 42);
            HoldLbl.TabIndex = 11;
            HoldLbl.Text = "0";
            HoldLbl.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // AnswerBtn
            // 
            AnswerBtn.Dock = DockStyle.Fill;
            AnswerBtn.Location = new Point(133, 659);
            AnswerBtn.Name = "AnswerBtn";
            AnswerBtn.Size = new Size(124, 36);
            AnswerBtn.TabIndex = 12;
            AnswerBtn.Text = "Answer";
            toolTip1.SetToolTip(AnswerBtn, "Answers the longest ringing call");
            AnswerBtn.UseVisualStyleBackColor = true;
            AnswerBtn.Click += AnswerBtn_Click;
            // 
            // ShowBtn
            // 
            ShowBtn.Dock = DockStyle.Fill;
            ShowBtn.Location = new Point(263, 659);
            ShowBtn.Name = "ShowBtn";
            ShowBtn.Size = new Size(124, 36);
            ShowBtn.TabIndex = 13;
            ShowBtn.Text = "Show";
            toolTip1.SetToolTip(ShowBtn, "Shows a selected call in its current state");
            ShowBtn.UseVisualStyleBackColor = true;
            ShowBtn.Click += ShowBtn_Click;
            // 
            // EndCallBtn
            // 
            EndCallBtn.Dock = DockStyle.Fill;
            EndCallBtn.Location = new Point(523, 659);
            EndCallBtn.Name = "EndCallBtn";
            EndCallBtn.Size = new Size(124, 36);
            EndCallBtn.TabIndex = 14;
            EndCallBtn.Text = "End Call";
            toolTip1.SetToolTip(EndCallBtn, "Ends the selected call");
            EndCallBtn.UseVisualStyleBackColor = true;
            EndCallBtn.Click += EndCallBtn_Click;
            // 
            // EndAllBtn
            // 
            EndAllBtn.Dock = DockStyle.Fill;
            EndAllBtn.Location = new Point(653, 659);
            EndAllBtn.Name = "EndAllBtn";
            EndAllBtn.Size = new Size(124, 36);
            EndAllBtn.TabIndex = 15;
            EndAllBtn.Text = "End All";
            toolTip1.SetToolTip(EndAllBtn, "Ends all calls");
            EndAllBtn.UseVisualStyleBackColor = true;
            EndAllBtn.Click += EndAllBtn_Click;
            // 
            // HoldBtn
            // 
            HoldBtn.Dock = DockStyle.Fill;
            HoldBtn.Location = new Point(393, 659);
            HoldBtn.Name = "HoldBtn";
            HoldBtn.Size = new Size(124, 36);
            HoldBtn.TabIndex = 16;
            HoldBtn.Text = "Hold";
            toolTip1.SetToolTip(HoldBtn, "Puts the selected call on hold");
            HoldBtn.UseVisualStyleBackColor = true;
            HoldBtn.Click += HoldBtn_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Fill;
            label5.Location = new Point(783, 0);
            label5.Name = "label5";
            label5.Size = new Size(124, 42);
            label5.TabIndex = 18;
            label5.Text = "On Line";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // OnLineLbl
            // 
            OnLineLbl.AutoSize = true;
            OnLineLbl.Dock = DockStyle.Fill;
            OnLineLbl.Location = new Point(913, 0);
            OnLineLbl.Name = "OnLineLbl";
            OnLineLbl.Size = new Size(124, 42);
            OnLineLbl.TabIndex = 19;
            OnLineLbl.Text = "0";
            OnLineLbl.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // StatusLbl
            // 
            StatusLbl.AutoSize = true;
            tableLayoutPanel1.SetColumnSpan(StatusLbl, 10);
            StatusLbl.Dock = DockStyle.Fill;
            StatusLbl.Location = new Point(3, 614);
            StatusLbl.Name = "StatusLbl";
            StatusLbl.Size = new Size(1294, 42);
            StatusLbl.TabIndex = 20;
            StatusLbl.Text = "Not listening. Press Start";
            StatusLbl.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1300, 698);
            Controls.Add(tableLayoutPanel1);
            Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(5);
            Name = "Form1";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PSAP Simulator";
            FormClosing += Form1_FormClosing;
            FormClosed += Form1_FormClosed;
            Load += Form1_Load;
            SizeChanged += Form1_SizeChanged;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button CloseBtn;
        private Button SettingsBtn;
        private TableLayoutPanel tableLayoutPanel1;
        private ListView CallListView;
        private ColumnHeader FromHeader;
        private ColumnHeader TimeHeader;
        private ColumnHeader State;
        private ColumnHeader QueueUriHeader;
        private ColumnHeader ConferenceHeader;
        private ColumnHeader MediaHeader;
        private Button StartBtn;
        private Label label1;
        private Label TotalCallsLbl;
        private Label label2;
        private Label RingingLbl;
        private Label label4;
        private Label AnsweredLbl;
        private Label label6;
        private Label HoldLbl;
        private Button AnswerBtn;
        private Button ShowBtn;
        private Button EndCallBtn;
        private Button EndAllBtn;
        private Button HoldBtn;
        private Label label5;
        private Label OnLineLbl;
        private ToolTip toolTip1;
        private Label StatusLbl;
    }
}
