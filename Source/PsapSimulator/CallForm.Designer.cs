namespace PsapSimulator
{
    partial class CallForm
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
            tableLayoutPanel1 = new TableLayoutPanel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            AnswerBtn = new Button();
            HoldBtn = new Button();
            EndBtn = new Button();
            KeypadBtn = new Button();
            CloseBtn = new Button();
            panel1 = new Panel();
            CallInfoTabCtrl = new TabControl();
            LocationTab = new TabPage();
            ProvidedByLbl = new Label();
            label27 = new Label();
            CountyLbl = new Label();
            label17 = new Label();
            StateLbl = new Label();
            label15 = new Label();
            CityLbl = new Label();
            label16 = new Label();
            StreetLbl = new Label();
            label14 = new Label();
            ConfidenceLbl = new Label();
            label13 = new Label();
            MethodLbl = new Label();
            label12 = new Label();
            ElevationLbl = new Label();
            label11 = new Label();
            RadiusLbl = new Label();
            label10 = new Label();
            LocRefreshBtn = new Button();
            label9 = new Label();
            LongitudeLbl = new Label();
            LatitudeLbl = new Label();
            label2 = new Label();
            SubscriberTab = new TabPage();
            label32 = new Label();
            SubscriberDataProviderLbl = new Label();
            SubCountryLbl = new Label();
            label24 = new Label();
            SubStateLbl = new Label();
            label26 = new Label();
            SubCityLbl = new Label();
            label28 = new Label();
            SubStreetLbl = new Label();
            label30 = new Label();
            LanguagesLbl = new Label();
            label20 = new Label();
            MiddleNameLbl = new Label();
            label21 = new Label();
            FirstNameLbl = new Label();
            label19 = new Label();
            LastNameLbl = new Label();
            label18 = new Label();
            CommentsTab = new TabPage();
            CommentsTb = new TextBox();
            ServiceTab = new TabPage();
            label29 = new Label();
            DeviceDataProviderLbl = new Label();
            label31 = new Label();
            ServiceDataProviderLbl = new Label();
            DeviceClassLbl = new Label();
            label22 = new Label();
            MobilityLbl = new Label();
            ServiceTypeLbl = new Label();
            EnvironmentLbl = new Label();
            label25 = new Label();
            label23 = new Label();
            label8 = new Label();
            AACN = new TabPage();
            PreviewVideoPb = new PictureBox();
            ReceiveVideoPb = new PictureBox();
            CallStateLbl = new Label();
            label7 = new Label();
            SendBtn = new Button();
            PrivateMsgCheck = new CheckBox();
            UseCpimCheck = new CheckBox();
            NewMessageTb = new TextBox();
            label6 = new Label();
            TextTypeLbl = new Label();
            label5 = new Label();
            TextListView = new ListView();
            FromHeader = new ColumnHeader();
            MessageHeader = new ColumnHeader();
            TimeHeader = new ColumnHeader();
            AddMediaBtn = new Button();
            DropLastBtn = new Button();
            DropBtn = new Button();
            ReferBtn = new Button();
            label4 = new Label();
            ConfListView = new ListView();
            UriHeader = new ColumnHeader();
            MediaHeader = new ColumnHeader();
            StatusHeader = new ColumnHeader();
            RolesHeader = new ColumnHeader();
            MediaLbl = new Label();
            label3 = new Label();
            FromLbl = new Label();
            label1 = new Label();
            ProvidersTab = new TabPage();
            ProvidersTb = new TextBox();
            tableLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            CallInfoTabCtrl.SuspendLayout();
            LocationTab.SuspendLayout();
            SubscriberTab.SuspendLayout();
            CommentsTab.SuspendLayout();
            ServiceTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PreviewVideoPb).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ReceiveVideoPb).BeginInit();
            ProvidersTab.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 0, 1);
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.Size = new Size(1902, 977);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(AnswerBtn);
            flowLayoutPanel1.Controls.Add(HoldBtn);
            flowLayoutPanel1.Controls.Add(EndBtn);
            flowLayoutPanel1.Controls.Add(KeypadBtn);
            flowLayoutPanel1.Controls.Add(CloseBtn);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(3, 930);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1896, 44);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // AnswerBtn
            // 
            AnswerBtn.Location = new Point(3, 3);
            AnswerBtn.Name = "AnswerBtn";
            AnswerBtn.Size = new Size(134, 41);
            AnswerBtn.TabIndex = 0;
            AnswerBtn.Text = "Answer";
            AnswerBtn.UseVisualStyleBackColor = true;
            AnswerBtn.Click += AnswerBtn_Click;
            // 
            // HoldBtn
            // 
            HoldBtn.Location = new Point(143, 3);
            HoldBtn.Name = "HoldBtn";
            HoldBtn.Size = new Size(147, 41);
            HoldBtn.TabIndex = 1;
            HoldBtn.Text = "Hold";
            HoldBtn.UseVisualStyleBackColor = true;
            HoldBtn.Click += HoldBtn_Click;
            // 
            // EndBtn
            // 
            EndBtn.Location = new Point(296, 3);
            EndBtn.Name = "EndBtn";
            EndBtn.Size = new Size(141, 41);
            EndBtn.TabIndex = 2;
            EndBtn.Text = "End Call";
            EndBtn.UseVisualStyleBackColor = true;
            EndBtn.Click += EndBtn_Click;
            // 
            // KeypadBtn
            // 
            KeypadBtn.Location = new Point(443, 3);
            KeypadBtn.Name = "KeypadBtn";
            KeypadBtn.Size = new Size(119, 41);
            KeypadBtn.TabIndex = 4;
            KeypadBtn.Text = "Keypad";
            KeypadBtn.UseVisualStyleBackColor = true;
            // 
            // CloseBtn
            // 
            CloseBtn.Location = new Point(568, 3);
            CloseBtn.Name = "CloseBtn";
            CloseBtn.Size = new Size(139, 41);
            CloseBtn.TabIndex = 3;
            CloseBtn.Text = "Close";
            CloseBtn.UseVisualStyleBackColor = true;
            CloseBtn.Click += CloseBtn_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(CallInfoTabCtrl);
            panel1.Controls.Add(PreviewVideoPb);
            panel1.Controls.Add(ReceiveVideoPb);
            panel1.Controls.Add(CallStateLbl);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(SendBtn);
            panel1.Controls.Add(PrivateMsgCheck);
            panel1.Controls.Add(UseCpimCheck);
            panel1.Controls.Add(NewMessageTb);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(TextTypeLbl);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(TextListView);
            panel1.Controls.Add(AddMediaBtn);
            panel1.Controls.Add(DropLastBtn);
            panel1.Controls.Add(DropBtn);
            panel1.Controls.Add(ReferBtn);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(ConfListView);
            panel1.Controls.Add(MediaLbl);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(FromLbl);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(1896, 921);
            panel1.TabIndex = 1;
            // 
            // CallInfoTabCtrl
            // 
            CallInfoTabCtrl.Controls.Add(LocationTab);
            CallInfoTabCtrl.Controls.Add(SubscriberTab);
            CallInfoTabCtrl.Controls.Add(CommentsTab);
            CallInfoTabCtrl.Controls.Add(ServiceTab);
            CallInfoTabCtrl.Controls.Add(ProvidersTab);
            CallInfoTabCtrl.Controls.Add(AACN);
            CallInfoTabCtrl.Location = new Point(1247, 508);
            CallInfoTabCtrl.Name = "CallInfoTabCtrl";
            CallInfoTabCtrl.SelectedIndex = 0;
            CallInfoTabCtrl.Size = new Size(640, 406);
            CallInfoTabCtrl.TabIndex = 24;
            // 
            // LocationTab
            // 
            LocationTab.AutoScroll = true;
            LocationTab.Controls.Add(ProvidedByLbl);
            LocationTab.Controls.Add(label27);
            LocationTab.Controls.Add(CountyLbl);
            LocationTab.Controls.Add(label17);
            LocationTab.Controls.Add(StateLbl);
            LocationTab.Controls.Add(label15);
            LocationTab.Controls.Add(CityLbl);
            LocationTab.Controls.Add(label16);
            LocationTab.Controls.Add(StreetLbl);
            LocationTab.Controls.Add(label14);
            LocationTab.Controls.Add(ConfidenceLbl);
            LocationTab.Controls.Add(label13);
            LocationTab.Controls.Add(MethodLbl);
            LocationTab.Controls.Add(label12);
            LocationTab.Controls.Add(ElevationLbl);
            LocationTab.Controls.Add(label11);
            LocationTab.Controls.Add(RadiusLbl);
            LocationTab.Controls.Add(label10);
            LocationTab.Controls.Add(LocRefreshBtn);
            LocationTab.Controls.Add(label9);
            LocationTab.Controls.Add(LongitudeLbl);
            LocationTab.Controls.Add(LatitudeLbl);
            LocationTab.Controls.Add(label2);
            LocationTab.Location = new Point(4, 40);
            LocationTab.Name = "LocationTab";
            LocationTab.Padding = new Padding(3);
            LocationTab.Size = new Size(632, 362);
            LocationTab.TabIndex = 0;
            LocationTab.Text = "Location";
            LocationTab.UseVisualStyleBackColor = true;
            // 
            // ProvidedByLbl
            // 
            ProvidedByLbl.BorderStyle = BorderStyle.Fixed3D;
            ProvidedByLbl.Location = new Point(92, 316);
            ProvidedByLbl.Name = "ProvidedByLbl";
            ProvidedByLbl.Size = new Size(534, 38);
            ProvidedByLbl.TabIndex = 22;
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Location = new Point(12, 316);
            label27.Name = "label27";
            label27.Size = new Size(64, 31);
            label27.TabIndex = 21;
            label27.Text = "Prov.";
            // 
            // CountyLbl
            // 
            CountyLbl.BorderStyle = BorderStyle.Fixed3D;
            CountyLbl.Location = new Point(416, 269);
            CountyLbl.Name = "CountyLbl";
            CountyLbl.Size = new Size(210, 38);
            CountyLbl.TabIndex = 20;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(324, 269);
            label17.Name = "label17";
            label17.Size = new Size(86, 31);
            label17.TabIndex = 19;
            label17.Text = "County";
            // 
            // StateLbl
            // 
            StateLbl.BorderStyle = BorderStyle.Fixed3D;
            StateLbl.Location = new Point(92, 269);
            StateLbl.Name = "StateLbl";
            StateLbl.Size = new Size(213, 38);
            StateLbl.TabIndex = 18;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(12, 269);
            label15.Name = "label15";
            label15.Size = new Size(65, 31);
            label15.TabIndex = 17;
            label15.Text = "State";
            // 
            // CityLbl
            // 
            CityLbl.BorderStyle = BorderStyle.Fixed3D;
            CityLbl.Location = new Point(92, 214);
            CityLbl.Name = "CityLbl";
            CityLbl.Size = new Size(534, 38);
            CityLbl.TabIndex = 16;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(12, 215);
            label16.Name = "label16";
            label16.Size = new Size(53, 31);
            label16.TabIndex = 15;
            label16.Text = "City";
            // 
            // StreetLbl
            // 
            StreetLbl.BorderStyle = BorderStyle.Fixed3D;
            StreetLbl.Location = new Point(92, 164);
            StreetLbl.Name = "StreetLbl";
            StreetLbl.Size = new Size(534, 38);
            StreetLbl.TabIndex = 14;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(6, 165);
            label14.Name = "label14";
            label14.Size = new Size(73, 31);
            label14.TabIndex = 13;
            label14.Text = "Street";
            // 
            // ConfidenceLbl
            // 
            ConfidenceLbl.BorderStyle = BorderStyle.Fixed3D;
            ConfidenceLbl.Location = new Point(337, 110);
            ConfidenceLbl.Name = "ConfidenceLbl";
            ConfidenceLbl.Size = new Size(132, 38);
            ConfidenceLbl.TabIndex = 12;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(245, 111);
            label13.Name = "label13";
            label13.Size = new Size(66, 31);
            label13.TabIndex = 11;
            label13.Text = "Conf.";
            // 
            // MethodLbl
            // 
            MethodLbl.BorderStyle = BorderStyle.Fixed3D;
            MethodLbl.Location = new Point(92, 111);
            MethodLbl.Name = "MethodLbl";
            MethodLbl.Size = new Size(132, 38);
            MethodLbl.TabIndex = 10;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(6, 112);
            label12.Name = "label12";
            label12.Size = new Size(73, 31);
            label12.TabIndex = 9;
            label12.Text = "Meth.";
            // 
            // ElevationLbl
            // 
            ElevationLbl.BorderStyle = BorderStyle.Fixed3D;
            ElevationLbl.Location = new Point(337, 59);
            ElevationLbl.Name = "ElevationLbl";
            ElevationLbl.Size = new Size(132, 38);
            ElevationLbl.TabIndex = 8;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(245, 66);
            label11.Name = "label11";
            label11.Size = new Size(60, 31);
            label11.TabIndex = 7;
            label11.Text = "Elev.";
            // 
            // RadiusLbl
            // 
            RadiusLbl.BorderStyle = BorderStyle.Fixed3D;
            RadiusLbl.Location = new Point(92, 59);
            RadiusLbl.Name = "RadiusLbl";
            RadiusLbl.Size = new Size(132, 38);
            RadiusLbl.TabIndex = 6;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(6, 66);
            label10.Name = "label10";
            label10.Size = new Size(59, 31);
            label10.TabIndex = 5;
            label10.Text = "Rad.";
            // 
            // LocRefreshBtn
            // 
            LocRefreshBtn.Location = new Point(487, 16);
            LocRefreshBtn.Name = "LocRefreshBtn";
            LocRefreshBtn.Size = new Size(114, 36);
            LocRefreshBtn.TabIndex = 4;
            LocRefreshBtn.Text = "Refresh";
            LocRefreshBtn.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(245, 14);
            label9.Name = "label9";
            label9.Size = new Size(70, 31);
            label9.TabIndex = 3;
            label9.Text = "Long.";
            // 
            // LongitudeLbl
            // 
            LongitudeLbl.BorderStyle = BorderStyle.Fixed3D;
            LongitudeLbl.Location = new Point(337, 13);
            LongitudeLbl.Name = "LongitudeLbl";
            LongitudeLbl.Size = new Size(132, 38);
            LongitudeLbl.TabIndex = 2;
            // 
            // LatitudeLbl
            // 
            LatitudeLbl.BorderStyle = BorderStyle.Fixed3D;
            LatitudeLbl.Location = new Point(92, 14);
            LatitudeLbl.Name = "LatitudeLbl";
            LatitudeLbl.Size = new Size(132, 38);
            LatitudeLbl.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 14);
            label2.Name = "label2";
            label2.Size = new Size(50, 31);
            label2.TabIndex = 0;
            label2.Text = "Lat.";
            // 
            // SubscriberTab
            // 
            SubscriberTab.Controls.Add(label32);
            SubscriberTab.Controls.Add(SubscriberDataProviderLbl);
            SubscriberTab.Controls.Add(SubCountryLbl);
            SubscriberTab.Controls.Add(label24);
            SubscriberTab.Controls.Add(SubStateLbl);
            SubscriberTab.Controls.Add(label26);
            SubscriberTab.Controls.Add(SubCityLbl);
            SubscriberTab.Controls.Add(label28);
            SubscriberTab.Controls.Add(SubStreetLbl);
            SubscriberTab.Controls.Add(label30);
            SubscriberTab.Controls.Add(LanguagesLbl);
            SubscriberTab.Controls.Add(label20);
            SubscriberTab.Controls.Add(MiddleNameLbl);
            SubscriberTab.Controls.Add(label21);
            SubscriberTab.Controls.Add(FirstNameLbl);
            SubscriberTab.Controls.Add(label19);
            SubscriberTab.Controls.Add(LastNameLbl);
            SubscriberTab.Controls.Add(label18);
            SubscriberTab.Location = new Point(4, 40);
            SubscriberTab.Name = "SubscriberTab";
            SubscriberTab.Padding = new Padding(3);
            SubscriberTab.Size = new Size(632, 362);
            SubscriberTab.TabIndex = 1;
            SubscriberTab.Text = "Subscriber";
            SubscriberTab.UseVisualStyleBackColor = true;
            // 
            // label32
            // 
            label32.AutoSize = true;
            label32.Location = new Point(6, 321);
            label32.Name = "label32";
            label32.Size = new Size(64, 31);
            label32.TabIndex = 30;
            label32.Text = "Prov.";
            // 
            // SubscriberDataProviderLbl
            // 
            SubscriberDataProviderLbl.BorderStyle = BorderStyle.Fixed3D;
            SubscriberDataProviderLbl.Location = new Point(92, 321);
            SubscriberDataProviderLbl.Name = "SubscriberDataProviderLbl";
            SubscriberDataProviderLbl.Size = new Size(534, 38);
            SubscriberDataProviderLbl.TabIndex = 29;
            // 
            // SubCountryLbl
            // 
            SubCountryLbl.BorderStyle = BorderStyle.Fixed3D;
            SubCountryLbl.Location = new Point(424, 266);
            SubCountryLbl.Name = "SubCountryLbl";
            SubCountryLbl.Size = new Size(202, 38);
            SubCountryLbl.TabIndex = 28;
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Location = new Point(324, 267);
            label24.Name = "label24";
            label24.Size = new Size(94, 31);
            label24.TabIndex = 27;
            label24.Text = "Country";
            // 
            // SubStateLbl
            // 
            SubStateLbl.BorderStyle = BorderStyle.Fixed3D;
            SubStateLbl.Location = new Point(92, 266);
            SubStateLbl.Name = "SubStateLbl";
            SubStateLbl.Size = new Size(213, 38);
            SubStateLbl.TabIndex = 26;
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Location = new Point(6, 267);
            label26.Name = "label26";
            label26.Size = new Size(65, 31);
            label26.TabIndex = 25;
            label26.Text = "State";
            // 
            // SubCityLbl
            // 
            SubCityLbl.BorderStyle = BorderStyle.Fixed3D;
            SubCityLbl.Location = new Point(92, 217);
            SubCityLbl.Name = "SubCityLbl";
            SubCityLbl.Size = new Size(534, 38);
            SubCityLbl.TabIndex = 24;
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Location = new Point(6, 224);
            label28.Name = "label28";
            label28.Size = new Size(53, 31);
            label28.TabIndex = 23;
            label28.Text = "City";
            // 
            // SubStreetLbl
            // 
            SubStreetLbl.BorderStyle = BorderStyle.Fixed3D;
            SubStreetLbl.Location = new Point(92, 170);
            SubStreetLbl.Name = "SubStreetLbl";
            SubStreetLbl.Size = new Size(534, 38);
            SubStreetLbl.TabIndex = 22;
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.Location = new Point(6, 177);
            label30.Name = "label30";
            label30.Size = new Size(73, 31);
            label30.TabIndex = 21;
            label30.Text = "Street";
            // 
            // LanguagesLbl
            // 
            LanguagesLbl.BorderStyle = BorderStyle.Fixed3D;
            LanguagesLbl.Location = new Point(424, 105);
            LanguagesLbl.Name = "LanguagesLbl";
            LanguagesLbl.Size = new Size(202, 38);
            LanguagesLbl.TabIndex = 8;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(284, 106);
            label20.Name = "label20";
            label20.Size = new Size(125, 31);
            label20.TabIndex = 7;
            label20.Text = "Languages";
            // 
            // MiddleNameLbl
            // 
            MiddleNameLbl.BorderStyle = BorderStyle.Fixed3D;
            MiddleNameLbl.Location = new Point(141, 105);
            MiddleNameLbl.Name = "MiddleNameLbl";
            MiddleNameLbl.Size = new Size(116, 38);
            MiddleNameLbl.TabIndex = 6;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(6, 106);
            label21.Name = "label21";
            label21.Size = new Size(87, 31);
            label21.TabIndex = 5;
            label21.Text = "Middle";
            // 
            // FirstNameLbl
            // 
            FirstNameLbl.BorderStyle = BorderStyle.Fixed3D;
            FirstNameLbl.Location = new Point(141, 62);
            FirstNameLbl.Name = "FirstNameLbl";
            FirstNameLbl.Size = new Size(485, 38);
            FirstNameLbl.TabIndex = 4;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(3, 63);
            label19.Name = "label19";
            label19.Size = new Size(124, 31);
            label19.TabIndex = 3;
            label19.Text = "First Name";
            // 
            // LastNameLbl
            // 
            LastNameLbl.BorderStyle = BorderStyle.Fixed3D;
            LastNameLbl.Location = new Point(141, 18);
            LastNameLbl.Name = "LastNameLbl";
            LastNameLbl.Size = new Size(485, 38);
            LastNameLbl.TabIndex = 2;
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(3, 18);
            label18.Name = "label18";
            label18.Size = new Size(122, 31);
            label18.TabIndex = 0;
            label18.Text = "Last Name";
            // 
            // CommentsTab
            // 
            CommentsTab.Controls.Add(CommentsTb);
            CommentsTab.Location = new Point(4, 40);
            CommentsTab.Name = "CommentsTab";
            CommentsTab.Size = new Size(632, 362);
            CommentsTab.TabIndex = 2;
            CommentsTab.Text = "Comments";
            CommentsTab.UseVisualStyleBackColor = true;
            // 
            // CommentsTb
            // 
            CommentsTb.AcceptsReturn = true;
            CommentsTb.Location = new Point(18, 13);
            CommentsTb.Multiline = true;
            CommentsTb.Name = "CommentsTb";
            CommentsTb.ReadOnly = true;
            CommentsTb.ScrollBars = ScrollBars.Vertical;
            CommentsTb.Size = new Size(596, 332);
            CommentsTb.TabIndex = 0;
            // 
            // ServiceTab
            // 
            ServiceTab.Controls.Add(label29);
            ServiceTab.Controls.Add(DeviceDataProviderLbl);
            ServiceTab.Controls.Add(label31);
            ServiceTab.Controls.Add(ServiceDataProviderLbl);
            ServiceTab.Controls.Add(DeviceClassLbl);
            ServiceTab.Controls.Add(label22);
            ServiceTab.Controls.Add(MobilityLbl);
            ServiceTab.Controls.Add(ServiceTypeLbl);
            ServiceTab.Controls.Add(EnvironmentLbl);
            ServiceTab.Controls.Add(label25);
            ServiceTab.Controls.Add(label23);
            ServiceTab.Controls.Add(label8);
            ServiceTab.Location = new Point(4, 40);
            ServiceTab.Name = "ServiceTab";
            ServiceTab.Size = new Size(632, 362);
            ServiceTab.TabIndex = 3;
            ServiceTab.Text = "Service";
            ServiceTab.UseVisualStyleBackColor = true;
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.Location = new Point(14, 297);
            label29.Name = "label29";
            label29.Size = new Size(99, 31);
            label29.TabIndex = 11;
            label29.Text = "Provider";
            // 
            // DeviceDataProviderLbl
            // 
            DeviceDataProviderLbl.BorderStyle = BorderStyle.Fixed3D;
            DeviceDataProviderLbl.Location = new Point(177, 297);
            DeviceDataProviderLbl.Name = "DeviceDataProviderLbl";
            DeviceDataProviderLbl.Size = new Size(401, 38);
            DeviceDataProviderLbl.TabIndex = 10;
            // 
            // label31
            // 
            label31.AutoSize = true;
            label31.Location = new Point(11, 182);
            label31.Name = "label31";
            label31.Size = new Size(99, 31);
            label31.TabIndex = 9;
            label31.Text = "Provider";
            // 
            // ServiceDataProviderLbl
            // 
            ServiceDataProviderLbl.BorderStyle = BorderStyle.Fixed3D;
            ServiceDataProviderLbl.Location = new Point(177, 182);
            ServiceDataProviderLbl.Name = "ServiceDataProviderLbl";
            ServiceDataProviderLbl.Size = new Size(401, 38);
            ServiceDataProviderLbl.TabIndex = 8;
            // 
            // DeviceClassLbl
            // 
            DeviceClassLbl.BorderStyle = BorderStyle.Fixed3D;
            DeviceClassLbl.Location = new Point(177, 249);
            DeviceClassLbl.Name = "DeviceClassLbl";
            DeviceClassLbl.Size = new Size(401, 38);
            DeviceClassLbl.TabIndex = 7;
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(14, 249);
            label22.Name = "label22";
            label22.Size = new Size(140, 31);
            label22.TabIndex = 6;
            label22.Text = "Device Class";
            // 
            // MobilityLbl
            // 
            MobilityLbl.BorderStyle = BorderStyle.Fixed3D;
            MobilityLbl.Location = new Point(177, 128);
            MobilityLbl.Name = "MobilityLbl";
            MobilityLbl.Size = new Size(401, 38);
            MobilityLbl.TabIndex = 5;
            // 
            // ServiceTypeLbl
            // 
            ServiceTypeLbl.BorderStyle = BorderStyle.Fixed3D;
            ServiceTypeLbl.Location = new Point(177, 76);
            ServiceTypeLbl.Name = "ServiceTypeLbl";
            ServiceTypeLbl.Size = new Size(401, 38);
            ServiceTypeLbl.TabIndex = 4;
            // 
            // EnvironmentLbl
            // 
            EnvironmentLbl.BorderStyle = BorderStyle.Fixed3D;
            EnvironmentLbl.Location = new Point(177, 19);
            EnvironmentLbl.Name = "EnvironmentLbl";
            EnvironmentLbl.Size = new Size(401, 38);
            EnvironmentLbl.TabIndex = 3;
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new Point(11, 128);
            label25.Name = "label25";
            label25.Size = new Size(99, 31);
            label25.TabIndex = 2;
            label25.Text = "Mobility";
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Location = new Point(11, 76);
            label23.Name = "label23";
            label23.Size = new Size(62, 31);
            label23.TabIndex = 1;
            label23.Text = "Type";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(11, 19);
            label8.Name = "label8";
            label8.Size = new Size(143, 31);
            label8.TabIndex = 0;
            label8.Text = "Environment";
            // 
            // AACN
            // 
            AACN.Location = new Point(4, 40);
            AACN.Name = "AACN";
            AACN.Size = new Size(632, 362);
            AACN.TabIndex = 4;
            AACN.Text = "AACN";
            AACN.UseVisualStyleBackColor = true;
            // 
            // PreviewVideoPb
            // 
            PreviewVideoPb.BackColor = Color.Black;
            PreviewVideoPb.BorderStyle = BorderStyle.Fixed3D;
            PreviewVideoPb.Location = new Point(1033, 11);
            PreviewVideoPb.Name = "PreviewVideoPb";
            PreviewVideoPb.Size = new Size(208, 149);
            PreviewVideoPb.SizeMode = PictureBoxSizeMode.StretchImage;
            PreviewVideoPb.TabIndex = 23;
            PreviewVideoPb.TabStop = false;
            // 
            // ReceiveVideoPb
            // 
            ReceiveVideoPb.BackColor = Color.Black;
            ReceiveVideoPb.BorderStyle = BorderStyle.Fixed3D;
            ReceiveVideoPb.Location = new Point(1247, 9);
            ReceiveVideoPb.Name = "ReceiveVideoPb";
            ReceiveVideoPb.Size = new Size(640, 480);
            ReceiveVideoPb.SizeMode = PictureBoxSizeMode.StretchImage;
            ReceiveVideoPb.TabIndex = 22;
            ReceiveVideoPb.TabStop = false;
            // 
            // CallStateLbl
            // 
            CallStateLbl.BorderStyle = BorderStyle.Fixed3D;
            CallStateLbl.Location = new Point(592, 20);
            CallStateLbl.Name = "CallStateLbl";
            CallStateLbl.Size = new Size(176, 34);
            CallStateLbl.TabIndex = 21;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(477, 23);
            label7.Name = "label7";
            label7.Size = new Size(109, 31);
            label7.TabIndex = 20;
            label7.Text = "Call State";
            // 
            // SendBtn
            // 
            SendBtn.Location = new Point(690, 499);
            SendBtn.Name = "SendBtn";
            SendBtn.Size = new Size(94, 40);
            SendBtn.TabIndex = 19;
            SendBtn.Text = "Send";
            SendBtn.UseVisualStyleBackColor = true;
            SendBtn.Click += SendBtn_Click;
            // 
            // PrivateMsgCheck
            // 
            PrivateMsgCheck.AutoSize = true;
            PrivateMsgCheck.Location = new Point(453, 499);
            PrivateMsgCheck.Name = "PrivateMsgCheck";
            PrivateMsgCheck.Size = new Size(203, 35);
            PrivateMsgCheck.TabIndex = 18;
            PrivateMsgCheck.Text = "Private Message";
            PrivateMsgCheck.UseVisualStyleBackColor = true;
            // 
            // UseCpimCheck
            // 
            UseCpimCheck.AutoSize = true;
            UseCpimCheck.Location = new Point(279, 499);
            UseCpimCheck.Name = "UseCpimCheck";
            UseCpimCheck.Size = new Size(134, 35);
            UseCpimCheck.TabIndex = 17;
            UseCpimCheck.Text = "Use CPIM";
            UseCpimCheck.UseVisualStyleBackColor = true;
            // 
            // NewMessageTb
            // 
            NewMessageTb.Location = new Point(188, 876);
            NewMessageTb.Name = "NewMessageTb";
            NewMessageTb.Size = new Size(1053, 38);
            NewMessageTb.TabIndex = 16;
            NewMessageTb.KeyPress += NewMessageTb_KeyPress;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(25, 876);
            label6.Name = "label6";
            label6.Size = new Size(157, 31);
            label6.TabIndex = 15;
            label6.Text = "New Message";
            // 
            // TextTypeLbl
            // 
            TextTypeLbl.AutoSize = true;
            TextTypeLbl.BorderStyle = BorderStyle.Fixed3D;
            TextTypeLbl.Location = new Point(143, 500);
            TextTypeLbl.Name = "TextTypeLbl";
            TextTypeLbl.Size = new Size(71, 33);
            TextTypeLbl.TabIndex = 14;
            TextTypeLbl.Text = "None";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(25, 500);
            label5.Name = "label5";
            label5.Size = new Size(109, 31);
            label5.TabIndex = 13;
            label5.Text = "Text Type";
            // 
            // TextListView
            // 
            TextListView.Columns.AddRange(new ColumnHeader[] { FromHeader, MessageHeader, TimeHeader });
            TextListView.GridLines = true;
            TextListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            TextListView.Location = new Point(25, 545);
            TextListView.Name = "TextListView";
            TextListView.Size = new Size(1216, 325);
            TextListView.TabIndex = 12;
            TextListView.UseCompatibleStateImageBehavior = false;
            TextListView.View = View.Details;
            // 
            // FromHeader
            // 
            FromHeader.Text = "From";
            FromHeader.Width = 200;
            // 
            // MessageHeader
            // 
            MessageHeader.Text = "Message";
            MessageHeader.Width = 850;
            // 
            // TimeHeader
            // 
            TimeHeader.Text = "Time";
            TimeHeader.Width = 150;
            // 
            // AddMediaBtn
            // 
            AddMediaBtn.Location = new Point(477, 68);
            AddMediaBtn.Name = "AddMediaBtn";
            AddMediaBtn.Size = new Size(291, 42);
            AddMediaBtn.TabIndex = 11;
            AddMediaBtn.Text = "Add Media";
            AddMediaBtn.UseVisualStyleBackColor = true;
            // 
            // DropLastBtn
            // 
            DropLastBtn.Location = new Point(223, 415);
            DropLastBtn.Name = "DropLastBtn";
            DropLastBtn.Size = new Size(157, 43);
            DropLastBtn.TabIndex = 10;
            DropLastBtn.Text = "Drop Last";
            DropLastBtn.UseVisualStyleBackColor = true;
            // 
            // DropBtn
            // 
            DropBtn.Location = new Point(123, 415);
            DropBtn.Name = "DropBtn";
            DropBtn.Size = new Size(94, 43);
            DropBtn.TabIndex = 9;
            DropBtn.Text = "Drop";
            DropBtn.UseVisualStyleBackColor = true;
            // 
            // ReferBtn
            // 
            ReferBtn.Location = new Point(23, 415);
            ReferBtn.Name = "ReferBtn";
            ReferBtn.Size = new Size(94, 43);
            ReferBtn.TabIndex = 8;
            ReferBtn.Text = "Refer";
            ReferBtn.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(25, 149);
            label4.Name = "label4";
            label4.Size = new Size(232, 31);
            label4.TabIndex = 7;
            label4.Text = "Conference Members";
            // 
            // ConfListView
            // 
            ConfListView.CheckBoxes = true;
            ConfListView.Columns.AddRange(new ColumnHeader[] { UriHeader, MediaHeader, StatusHeader, RolesHeader });
            ConfListView.FullRowSelect = true;
            ConfListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            ConfListView.Location = new Point(21, 194);
            ConfListView.Name = "ConfListView";
            ConfListView.Size = new Size(1220, 215);
            ConfListView.TabIndex = 6;
            ConfListView.UseCompatibleStateImageBehavior = false;
            ConfListView.View = View.Details;
            // 
            // UriHeader
            // 
            UriHeader.Text = "Agency";
            UriHeader.Width = 300;
            // 
            // MediaHeader
            // 
            MediaHeader.Text = "Media";
            MediaHeader.Width = 200;
            // 
            // StatusHeader
            // 
            StatusHeader.Text = "Status";
            StatusHeader.Width = 150;
            // 
            // RolesHeader
            // 
            RolesHeader.Text = "Roles";
            RolesHeader.Width = 200;
            // 
            // MediaLbl
            // 
            MediaLbl.BorderStyle = BorderStyle.Fixed3D;
            MediaLbl.Location = new Point(143, 73);
            MediaLbl.Name = "MediaLbl";
            MediaLbl.Size = new Size(304, 34);
            MediaLbl.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(21, 76);
            label3.Name = "label3";
            label3.Size = new Size(79, 31);
            label3.TabIndex = 4;
            label3.Text = "Media";
            // 
            // FromLbl
            // 
            FromLbl.BorderStyle = BorderStyle.Fixed3D;
            FromLbl.Location = new Point(143, 20);
            FromLbl.Name = "FromLbl";
            FromLbl.Size = new Size(304, 34);
            FromLbl.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(21, 23);
            label1.Name = "label1";
            label1.Size = new Size(66, 31);
            label1.TabIndex = 0;
            label1.Text = "From";
            // 
            // ProvidersTab
            // 
            ProvidersTab.Controls.Add(ProvidersTb);
            ProvidersTab.Location = new Point(4, 40);
            ProvidersTab.Name = "ProvidersTab";
            ProvidersTab.Size = new Size(632, 362);
            ProvidersTab.TabIndex = 5;
            ProvidersTab.Text = "Providers";
            ProvidersTab.UseVisualStyleBackColor = true;
            // 
            // ProvidersTb
            // 
            ProvidersTb.AcceptsReturn = true;
            ProvidersTb.Location = new Point(15, 12);
            ProvidersTb.Multiline = true;
            ProvidersTb.Name = "ProvidersTb";
            ProvidersTb.ReadOnly = true;
            ProvidersTb.ScrollBars = ScrollBars.Both;
            ProvidersTb.Size = new Size(599, 335);
            ProvidersTb.TabIndex = 0;
            // 
            // CallForm
            // 
            AutoScaleDimensions = new SizeF(13F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1902, 977);
            ControlBox = false;
            Controls.Add(tableLayoutPanel1);
            Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(5);
            Name = "CallForm";
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Current Call";
            Load += CallForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            CallInfoTabCtrl.ResumeLayout(false);
            LocationTab.ResumeLayout(false);
            LocationTab.PerformLayout();
            SubscriberTab.ResumeLayout(false);
            SubscriberTab.PerformLayout();
            CommentsTab.ResumeLayout(false);
            CommentsTab.PerformLayout();
            ServiceTab.ResumeLayout(false);
            ServiceTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PreviewVideoPb).EndInit();
            ((System.ComponentModel.ISupportInitialize)ReceiveVideoPb).EndInit();
            ProvidersTab.ResumeLayout(false);
            ProvidersTab.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button AnswerBtn;
        private Button HoldBtn;
        private Panel panel1;
        private Button EndBtn;
        private Button CloseBtn;
        private Label MediaLbl;
        private Label label3;
        private Label FromLbl;
        private Label label1;
        private ListView ConfListView;
        private ColumnHeader UriHeader;
        private ColumnHeader RolesHeader;
        private ColumnHeader MediaHeader;
        private Label label4;
        private ColumnHeader StatusHeader;
        private Button DropLastBtn;
        private Button DropBtn;
        private Button ReferBtn;
        private ListView TextListView;
        private ColumnHeader FromHeader;
        private ColumnHeader MessageHeader;
        private ColumnHeader TimeHeader;
        private Button AddMediaBtn;
        private Label TextTypeLbl;
        private Label label5;
        private TextBox NewMessageTb;
        private Label label6;
        private CheckBox UseCpimCheck;
        private Label CallStateLbl;
        private Label label7;
        private Button SendBtn;
        private CheckBox PrivateMsgCheck;
        private Button KeypadBtn;
        private PictureBox ReceiveVideoPb;
        private PictureBox PreviewVideoPb;
        private TabControl CallInfoTabCtrl;
        private TabPage LocationTab;
        private TabPage SubscriberTab;
        private TabPage CommentsTab;
        private TabPage ServiceTab;
        private TabPage AACN;
        private Label label2;
        private Label label10;
        private Button LocRefreshBtn;
        private Label label9;
        private Label LongitudeLbl;
        private Label LatitudeLbl;
        private Label label12;
        private Label ElevationLbl;
        private Label label11;
        private Label RadiusLbl;
        private Label CityLbl;
        private Label label16;
        private Label StreetLbl;
        private Label label14;
        private Label ConfidenceLbl;
        private Label label13;
        private Label MethodLbl;
        private Label CountyLbl;
        private Label label17;
        private Label StateLbl;
        private Label label15;
        private Label label19;
        private Label LastNameLbl;
        private Label label18;
        private Label LanguagesLbl;
        private Label label20;
        private Label MiddleNameLbl;
        private Label label21;
        private Label FirstNameLbl;
        private Label SubCountryLbl;
        private Label label24;
        private Label SubStateLbl;
        private Label label26;
        private Label SubCityLbl;
        private Label label28;
        private Label SubStreetLbl;
        private Label label30;
        private Label label25;
        private Label label23;
        private Label label8;
        private Label ServiceTypeLbl;
        private Label EnvironmentLbl;
        private Label MobilityLbl;
        private Label DeviceClassLbl;
        private Label label22;
        private TextBox CommentsTb;
        private Label ProvidedByLbl;
        private Label label27;
        private Label label31;
        private Label ServiceDataProviderLbl;
        private Label label29;
        private Label DeviceDataProviderLbl;
        private Label label32;
        private Label SubscriberDataProviderLbl;
        private TabPage ProvidersTab;
        private TextBox ProvidersTb;
    }
}