/////////////////////////////////////////////////////////////////////////////////////
//  File:   CallForm.cs                                             19 Mar 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator;
using AdditionalData;
using ConferenceEvent;
using Pidf;
using PsapSimulator.CallManagement;
using SipLib.Video.Windows;
using CameraCapture;
using SipLib.Core;
using SipLib.Media;
using SipRecMetaData;
using System.Diagnostics;
using System.Text;

/// <summary>
/// Form class for showing all of the data and controls for a single call.
/// </summary>
public partial class CallForm : Form
{
    private CallManager m_CallManager;
    private Call m_Call;
    private TextMessagesCollection m_TextMessages;
    private bool m_IsClosing = false;

    public CallForm(CallManager callManager, Call call)
    {
        m_CallManager = callManager;
        m_Call = call;

        if (m_Call.MsrpConnection != null)
            m_TextMessages = call.MsrpMessages;
        else
            m_TextMessages = call.RttMessages;

        InitializeComponent();
    }

    private void CallForm_Load(object sender, EventArgs e)
    {
        CallData callData = m_Call.GetCallData();

        FromLbl.Text = callData.From;
        CallStateLbl.Text = callData.CallStateString;
        MediaLbl.Text = callData.MediaTypes;

        SetupForTextMedia();

        m_CallManager.CallStateChanged += OnCallStateChanged;
        m_CallManager.CallEnded += OnCallEnded;
        m_Call.NewLocation += OnNewLocation;
        m_Call.NewConferenceInfo += OnNewConferenceInfo;
        m_Call.AdditionalDataAvailable += OnAdditionalDataAvailable;
        m_Call.ReferResponseStatus += OnReferResponseStatus;
        m_Call.ReferNotifyStatus += OnReferNotifyStatus;

        SetVideoPreviewSource();

        if (m_Call.VideoReceiver != null)
            m_Call.VideoReceiver.FrameReady += OnFrameReady;

        m_Call.CallVideoReceiverChanged += OnCallVideoReceiverChanged;

        // Display the last received location information
        if (m_Call.Locations.Count > 0)
        {
            Presence LastPresence = m_Call.Locations.ToArray()[m_Call.Locations.Count - 1];
            DisplayLocation(LastPresence);
        }

        DisplayAdditionalData();

        if (m_Call.ConferenceInfo != null)
            DisplayConferenceInfo(m_Call.ConferenceInfo);

        m_CallManager.CallManagerError += OnCallManagerError;

        // Hide the AACN tab in the Location/Additional Data tab control because AACN calls have not been implemented yet
        CallInfoTabCtrl.TabPages.RemoveAt(AACN_TAB_INDEX);
    }

    private void OnCallManagerError(string errMessage)
    {
        BeginInvoke(() =>
        {
            MessageBox.Show(errMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        });

    }

    private void SetupForTextMedia()
    {
        foreach (TextMessage textMessage in m_TextMessages.Messages)
        {
            ListViewItem item = BuildListViewItem(textMessage);
            item.Tag = textMessage;
            TextListView.Items.Add(item);
        }

        // Scroll to the end of the list view so that the most recent message is visible
        if (TextListView.Items.Count > 0)
            TextListView.EnsureVisible(TextListView.Items.Count - 1);

        if (m_Call.MsrpConnection != null)
        {
            TextTypeLbl.Text = "MSRP";
        }
        else if (m_Call.CallHasRtt() == true)
        {
            TextTypeLbl.Text = "RTT";
            UseCpimCheck.Visible = false;
            PrivateMsgCheck.Visible = false;
            SendBtn.Visible = false;
        }
        else
        {
            TextTypeLbl.Text = "None";
            UseCpimCheck.Visible = false;
            PrivateMsgCheck.Visible = false;
            SendBtn.Visible = false;
        }

        m_TextMessages.MessageAdded += OnMessageAdded;
        m_TextMessages.MessageUpdated += OnMessageUpdated;
    }

    private const int AACN_TAB_INDEX = 5;

    private void OnReferNotifyStatus(SIPResponseStatusCodesEnum responseEnum, string reason)
    {
        int StatusCode = (int)responseEnum;
        // For debug only
        Debug.WriteLine($"NOTIFY: Status Code = {StatusCode}, Reason = {reason}");
    }

    private void OnReferResponseStatus(SIPResponseStatusCodesEnum responseEnum, string reason)
    {
        int StatusCode = (int)responseEnum;
        if (StatusCode >= 400)
        {   // The REFER request to the conference-aware user agent was rejected.
            BeginInvoke(() =>
            {
                MessageBox.Show($"The conference/tranfer request was rejected.\nStatus Code = {StatusCode}, " +
                    $"Reason = {reason}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            });
        }
    }

    private void ClearServiceInfo()
    {
        EnvironmentLbl.Text = string.Empty;
        ServiceTypeLbl.Text = string.Empty;
        MobilityLbl.Text = string.Empty;
        ServiceDataProviderLbl.Text = string.Empty;
    }

    private void ClearSubscriberInfo()
    {
        LastNameLbl.Text = string.Empty;
        FirstNameLbl.Text = string.Empty;
        MiddleNameLbl.Text = string.Empty;
        LanguagesLbl.Text = string.Empty;
        SubStreetLbl.Text = string.Empty;
        SubCityLbl.Text = string.Empty;
        SubStreetLbl.Text = string.Empty;
        SubCountryLbl.Text = string.Empty;
    }

    private void ClearDeviceInfo()
    {
        DeviceClassLbl.Text = string.Empty;
        ManufacturerLbl.Text = string.Empty;
        ModelLbl.Text = string.Empty;
        DeviceIdLbl.Text = string.Empty;
        DeviceDataProviderLbl.Text = string.Empty;
    }

    private void ClearComments()
    {
        CommentsTb.Text = string.Empty;
    }

    private void ClearProviders()
    {
        ProvidersTb.Text = string.Empty;
    }

    private void DisplayAdditionalData()
    {
        ClearServiceInfo();
        ClearSubscriberInfo();
        ClearDeviceInfo();
        ClearComments();
        ClearProviders();

        if (m_Call.ServiceInfo != null)
            DisplayServiceInfo(m_Call.ServiceInfo);

        if (m_Call.SubscriberInfo != null)
            DisplaySubscriberInfo(m_Call.SubscriberInfo);

        if (m_Call.DeviceInfo != null)
            DisplayDeviceInfo(m_Call.DeviceInfo);

        DisplayComments();
        DisplayProviders();
    }

    private void OnAdditionalDataAvailable()
    {
        BeginInvoke(() => DisplayAdditionalData());
    }

    /// <summary>
    /// Event handler for the NewConferenceInfo event of the Call class
    /// </summary>
    /// <param name="conferenceType"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnNewConferenceInfo(conferencetype conferenceType)
    {
        BeginInvoke(() => DisplayConferenceInfo(conferenceType));
    }

    private void DisplayConferenceInfo(conferencetype conferenceType)
    {
        ConfListView.Items.Clear();
        foreach (usertype userType in conferenceType.users.user)
        {
            ListViewItem Lvi = new ListViewItem(GetCallUserName(userType.entity));
            Lvi.Tag = userType;

            if (userType.endpoint.Count > 0)
            {
                // Multiple user endpoints are possible but not likely so just use the first endpoint for now.
                endpointtype Ept = userType.endpoint[0];
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < Ept.media.Count; i++)
                {
                    if (i >= 1)
                        sb.Append(", ");

                    sb.Append(MediaTypeToDisplayString(Ept.media[i].type));
                }

                Lvi.SubItems.Add(sb.ToString());
                if (Ept.statusSpecified == true)
                    Lvi.SubItems.Add(EndpointStatusTypeToString(Ept.status));
                else
                    Lvi.SubItems.Add("Unknown");
            }
            else
            {   // No endpoints for this user
                Lvi.SubItems.Add("Unknown");
                Lvi.SubItems.Add("Unknown");
            }

            if (userType.roles.entry.Count > 0)
            {
                StringBuilder rolesSb = new StringBuilder();
                for (int i = 0; i < userType.roles.entry.Count; i++)
                {
                    if (i >= 1)
                        rolesSb.Append(", ");

                    Lvi.SubItems.Add(userType.roles.entry[i]);
                }
            }
            else
                Lvi.SubItems.Add("Unknown");

            ConfListView.Items.Add(Lvi);
        }
    }

    private string MediaTypeToDisplayString(string mediaType)
    {
        string strMedia = "Unknown";
        switch (mediaType)
        {
            case "audio":
                strMedia = "Audio";
                break;
            case "video":
                strMedia = "Video";
                break;
            case "message":
                strMedia = "MSRP";
                break;
            case "text":
                strMedia = "RTT";
                break;
        }

        return strMedia;
    }

    private string MediaDisplayTypeToMediaType(string displayType)
    {
        string strMedia = "Unknown";
        switch (displayType)
        {
            case "Audio":
                strMedia = MediaTypes.Audio;
                break;
            case "Video":
                strMedia = MediaTypes.Video;
                break;
            case "RTT":
                strMedia = MediaTypes.RTT;
                break;
            case "MSRP":
                strMedia = MediaTypes.MSRP;
                break;
        }

        return strMedia;
    }

    private string EndpointStatusTypeToString(endpointstatustype status)
    {
        string strStatus = "Unknown";
        switch (status)
        {
            case endpointstatustype.pending:
                strStatus = "Pending";
                break;
            case endpointstatustype.dialingout:
                strStatus = "Dialing Out";
                break;
            case endpointstatustype.dialingin:
                strStatus = "Dialing In";
                break;
            case endpointstatustype.alerting:
                strStatus = "Alerting";
                break;
            case endpointstatustype.onhold:
                strStatus = "On-Hold";
                break;
            case endpointstatustype.connected:
                strStatus = "Connected";
                break;
            case endpointstatustype.mutedviafocus:
                strStatus = "Muted Via Focus";
                break;
            case endpointstatustype.disconnecting:
                strStatus = "Disconnecting";
                break;
            case endpointstatustype.disconnected:
                strStatus = "Disconnected";
                break;
        }

        return strStatus;
    }

    private string GetCallUserName(string entity)
    {
        string strName = "Unknown";
        if (string.IsNullOrEmpty(entity) == true)
            return strName;

        if (SIPURI.TryParse(entity, out SIPURI? sipUri) == true)
        {
            if (sipUri is not null)
            {
                if (sipUri.User != null)
                    strName = sipUri.User;
                else
                    strName = sipUri.ToParameterlessString();
            }
        }


        return strName;
    }

    private void OnCallVideoReceiverChanged(VideoReceiver? oldVideoReceiver, VideoReceiver newVideoReceiver)
    {
        if (oldVideoReceiver != null)
            oldVideoReceiver.FrameReady -= OnFrameReady;

        newVideoReceiver.FrameReady += OnFrameReady;
    }

    private void DisplayComments()
    {
        StringBuilder Sb = new StringBuilder();

        CommentType[] CommentsArray = m_Call.Comments.ToArray();
        foreach (CommentType Comment in CommentsArray)
        {
            if (Comment.Comment != null && Comment.Comment.Length > 0)
            {
                foreach (CommentSubType CommentSubType in Comment.Comment)
                {
                    if (string.IsNullOrEmpty(CommentSubType.Value) == false)
                    {
                        Sb.Append(CommentSubType.Value);
                        Sb.Append("\r\n\r\n");
                    }
                }
            }
        }

        CommentsTb.Text = Sb.ToString();
    }

    private void DisplayProviders()
    {
        if (m_Call.Providers.Count == 0)
            return;

        StringBuilder Sb = new StringBuilder();
        foreach (ProviderInfoType Pit in m_Call.Providers.Values)
        {
            if (string.IsNullOrEmpty(Pit.DataProviderString) == false)
                Sb.Append($"Name:\t{Pit.DataProviderString}\r\n");
            if (string.IsNullOrEmpty(Pit.TypeOfProvider) == false)
                Sb.Append($"Type:\t{Pit.TypeOfProvider}\r\n");
            if (string.IsNullOrEmpty(Pit.ContactURI) == false)
                Sb.Append($"Contact:\t{Pit.ContactURI}\r\n");

            Sb.Append("\r\n");
        }

        ProvidersTb.Text = Sb.ToString();
    }

    private void DisplayServiceInfo(ServiceInfoType ServiceInfo)
    {
        EnvironmentLbl.Text = string.Empty;
        ServiceTypeLbl.Text = string.Empty;
        MobilityLbl.Text = string.Empty;

        EnvironmentLbl.Text = ServiceInfo.ServiceEnvironment;
        if (ServiceInfo.ServiceType.Length > 0)
            ServiceTypeLbl.Text = ServiceInfo.ServiceType[0];   // Just display the first one

        MobilityLbl.Text = ServiceInfo.ServiceMobility;

        // Get the ProviderInfo for the ServiceInfo if available
        if (ServiceInfo.DataProviderReference != null)
            ServiceDataProviderLbl.Text = GetProviderName(ServiceInfo.DataProviderReference);
    }

    private string? GetProviderName(string strProviderReference)
    {
        string? strProviderName = null;
        if (m_Call.Providers.Count > 0)
        {
            foreach (ProviderInfoType Pit in m_Call.Providers.Values)
            {
                if (strProviderReference == Pit.DataProviderReference)
                {
                    strProviderName = Pit.DataProviderString;
                    break;
                }
            }
        }

        return strProviderName;

    }

    private void DisplayDeviceInfo(DeviceInfoType deviceInfoType)
    {
        DeviceClassLbl.Text = deviceInfoType.DeviceClassification;
        if (deviceInfoType.DataProviderReference != null)
            DeviceDataProviderLbl.Text = GetProviderName(deviceInfoType.DataProviderReference);

        if (deviceInfoType.DeviceMfgr != null)
            ManufacturerLbl.Text = deviceInfoType.DeviceMfgr;

        if (deviceInfoType.DeviceModelNr != null)
            ModelLbl.Text = deviceInfoType.DeviceModelNr;

        if (deviceInfoType.UniqueDeviceID != null && deviceInfoType.UniqueDeviceID.Length > 0)
            DeviceIdLbl.Text = deviceInfoType.UniqueDeviceID[0].Value;
    }

    private void DisplaySubscriberInfo(SubscriberInfoType SubscriberInfo)
    {
        if (SubscriberInfo.SubscriberData == null || SubscriberInfo.SubscriberData.Length == 0)
            return;

        // Just pick the first one
        vcardType Vcard = SubscriberInfo.SubscriberData[0];
        LastNameLbl.Text = Vcard.LastName;
        FirstNameLbl.Text = Vcard.FirstName;
        MiddleNameLbl.Text = Vcard.MiddleName;

        SubStreetLbl.Text = Vcard.Street;
        SubCityLbl.Text = Vcard.City;
        SubStateLbl.Text = Vcard.State;
        SubCountryLbl.Text = Vcard.Country;

        // Get and display the list of spoken languages
        StringBuilder Sb = new StringBuilder();
        if (Vcard.lang != null && Vcard.lang.Length > 0)
        {
            for (int i = 0; i < Vcard.lang.Length; i++)
            {
                if (string.IsNullOrEmpty(Vcard.lang[i].languagetag) == false)
                {
                    Sb.Append(Vcard.lang[i].languagetag);
                    if (i < Vcard.lang.Length - 1)
                        Sb.Append(", ");
                }
            }

            LanguagesLbl.Text = Sb.ToString();
        }

        if (SubscriberInfo.DataProviderReference != null)
            SubscriberDataProviderLbl.Text = GetProviderName(SubscriberInfo.DataProviderReference);
    }

    private void OnNewLocation(Call call, Presence newPresence)
    {
        BeginInvoke(() => DisplayLocation(newPresence));
    }

    private void DisplayLocation(Presence presence)
    {
        // Clear all of the location display fields.
        LatitudeLbl.Text = string.Empty;
        LongitudeLbl.Text = string.Empty;
        RadiusLbl.Text = string.Empty;
        MethodLbl.Text = string.Empty;
        ElevationLbl.Text = string.Empty;
        ConfidenceLbl.Text = string.Empty;
        StreetLbl.Text = string.Empty;
        CityLbl.Text = string.Empty;
        StateLbl.Text = string.Empty;
        CountyLbl.Text = string.Empty;
        LocTimeLbl.Text = string.Empty;

        GeoPriv geoPriv = presence.GetFirstGeoGeoPriv();

        if (geoPriv != null && geoPriv.LocationInfo != null)
        {
            locInfoType locInfo = geoPriv.LocationInfo;
            if (locInfo.Point != null)
            {
                LatitudeLbl.Text = locInfo.Point.pos.Latitude.ToString();
                LongitudeLbl.Text = locInfo.Point.pos.Longitude.ToString();
                if (double.IsNaN(locInfo.Point.pos.Altitude) == false)
                    ElevationLbl.Text = locInfo.Point.pos.Altitude.ToString();
            }
            else if (locInfo.Circle != null)
            {
                LatitudeLbl.Text = locInfo.Circle.pos.Latitude.ToString();
                LongitudeLbl.Text = locInfo.Circle.pos.Longitude.ToString();
                RadiusLbl.Text = locInfo.Circle.radius.Value.ToString();
                if (double.IsNaN(locInfo.Circle.pos.Altitude) == false)
                    ElevationLbl.Text = locInfo.Circle.pos.Altitude.ToString();
            }

            MethodLbl.Text = geoPriv.locMethod;
            ConfidenceLbl.Text = geoPriv.LocationInfo.confidence?.Value;

            // See if the ProviderInfo is available by-value
            ProviderInfoType[]? PitArray = geoPriv.ProvidedBy?.EmergencyCallDataValue?.EmergencyCallDataProviderInfo;
            if (PitArray != null && PitArray.Length > 0)
                ProvidedByLbl.Text = PitArray[0].DataProviderString;
        }

        CivicAddress civicAddress = presence.GetFirstCivicAddress();
        if (civicAddress != null)
        {
            StreetLbl.Text = civicAddress.BuildFormattedStreetAddress();
            CityLbl.Text = civicAddress.A3;
            StateLbl.Text = civicAddress.A1;
            CountyLbl.Text = civicAddress.A2;
        }

        if (m_Call.LastLocationReceivedTime != DateTime.MinValue)
            LocTimeLbl.Text = m_Call.LastLocationReceivedTime.ToString("HH:mm:ss");
    }

    private IVideoCapture? m_CurrentPreviewVideoCapture = null;

    private void SetVideoPreviewSource()
    {
        if (m_Call.VideoReceiver == null || m_CallManager.CameraDisabledCapture == null ||
            m_CallManager.AutoAnswerCapture == null || m_CallManager.OnHoldCapture == null ||
            m_CallManager.CameraCapture == null)
        {
            PreviewVideoPb.Image = null;
            return;
        }

        if (m_CurrentPreviewVideoCapture != null)
        {   // Unhook the current event handler
            m_CurrentPreviewVideoCapture.FrameBitmapReady -= OnPreviewFrameBitmapReady;
            m_CurrentPreviewVideoCapture = null;
        }

        if (m_CallManager.TransmitVideoEnabled == false)
        {
            m_CurrentPreviewVideoCapture = m_CallManager.CameraDisabledCapture;
            m_CurrentPreviewVideoCapture.FrameBitmapReady += OnPreviewFrameBitmapReady;
            return;
        }

        switch (m_Call.CallState)
        {
            case CallStateEnum.OnLine:
                m_CurrentPreviewVideoCapture = m_CallManager.CameraCapture;
                m_CurrentPreviewVideoCapture.FrameBitmapReady += OnPreviewFrameBitmapReady;
                break;
            case CallStateEnum.OnHold:
                m_CurrentPreviewVideoCapture = m_CallManager.OnHoldCapture;
                m_CurrentPreviewVideoCapture.FrameBitmapReady += OnPreviewFrameBitmapReady;
                break;
            case CallStateEnum.AutoAnswered:
                m_CurrentPreviewVideoCapture = m_CallManager.AutoAnswerCapture;
                m_CurrentPreviewVideoCapture.FrameBitmapReady += OnPreviewFrameBitmapReady;
                break;
        }
    }

    private void OnCallStateChanged(CallSummary callSummary)
    {
        if (callSummary.CallID != m_Call.CallID)
            return;

        BeginInvoke(() => DoCallStateChanged(callSummary));
    }

    private void DoCallStateChanged(CallSummary callSummary)
    {
        CallStateLbl.Text = Call.CallStateToString(m_Call.CallState);
        MediaLbl.Text = callSummary.CallMedia;
        SetVideoPreviewSource();

        if (TextTypeLbl.Text == "None" && (m_Call.CallHasRtt() == true || m_Call.MsrpConnection != null))
        {   // Text media has been added to the call
            if (m_Call.MsrpConnection != null)
                m_TextMessages = m_Call.MsrpMessages;
            else
                m_TextMessages = m_Call.RttMessages;

//            SetupForTextMedia();
            if (m_Call.MsrpConnection != null)
            {
                TextTypeLbl.Text = "MSRP";
                m_TextMessages.MessageAdded += OnMessageAdded;
                m_TextMessages.MessageUpdated += OnMessageUpdated;
            }
            else if (m_Call.CallHasRtt() == true)
            {
                TextTypeLbl.Text = "RTT";
                UseCpimCheck.Visible = false;
                PrivateMsgCheck.Visible = false;
                SendBtn.Visible = false;
            }
        }
    }

    private void OnCallEnded(string CallID)
    {
        if (CallID != m_Call.CallID) return;

        m_Call.CallState = CallStateEnum.Ended;
        BeginInvoke(() =>
        {
            CallStateLbl.Text = "Ended";
            CallStateLbl.BackColor = Color.Red;
            CallStateLbl.ForeColor = Color.White;
        });
    }

    private void OnFrameReady(Bitmap bitmap)
    {
        Invoke(() => { ReceiveVideoPb.Image = bitmap; });
    }

    private ListViewItem BuildListViewItem(TextMessage textMessage)
    {
        ListViewItem item = new ListViewItem(textMessage.From);

        if (textMessage.Source == TextSourceEnum.Received)
            item.BackColor = Color.Beige;
        else
            item.BackColor = Color.LightCyan;

        item.SubItems.Add(textMessage.Message);
        item.SubItems.Add(textMessage.Time.ToString("HH:mm:ss"));

        return item;
    }

    private void OnMessageAdded(int index)
    {
        BeginInvoke(() =>
        {
            TextMessage textMessage = m_TextMessages.Messages[index];
            ListViewItem item = BuildListViewItem(textMessage);
            item.Tag = textMessage;
            TextListView.Items.Add(item);
            TextListView.EnsureVisible(TextListView.Items.Count - 1);
        });
    }

    private void OnMessageUpdated(int index)
    {
        BeginInvoke(() =>
        {
            if (index < m_TextMessages.Messages.Count)
            {
                ListViewItem? item = TextListView.Items[index];
                if (item != null)
                {
                    item.SubItems[1].Text = m_TextMessages.Messages[index].Message;
                }
            }
        });
    }

    private void CloseBtn_Click(object sender, EventArgs e)
    {
        // Unhook the events
        m_TextMessages.MessageAdded -= OnMessageAdded;
        m_TextMessages.MessageUpdated -= OnMessageUpdated;
        m_CallManager.CallStateChanged -= OnCallStateChanged;
        m_CallManager.CallEnded -= OnCallEnded;
        m_Call.NewLocation -= OnNewLocation;
        m_Call.NewConferenceInfo -= OnNewConferenceInfo;
        m_Call.AdditionalDataAvailable -= OnAdditionalDataAvailable;
        m_Call.ReferResponseStatus -= OnReferResponseStatus;
        m_Call.ReferNotifyStatus -= OnReferNotifyStatus;

        m_CallManager.FrameBitmapReady -= OnPreviewFrameBitmapReady;
        m_CallManager.CallManagerError -= OnCallManagerError;

        if (m_Call.VideoReceiver != null)
            m_Call.VideoReceiver.FrameReady -= OnFrameReady;

        m_Call.CallVideoReceiverChanged -= OnCallVideoReceiverChanged;

        if (m_CurrentPreviewVideoCapture != null)
        {
            m_CurrentPreviewVideoCapture.FrameBitmapReady -= OnPreviewFrameBitmapReady;
            m_CurrentPreviewVideoCapture = null;
        }

        m_IsClosing = true;
        Close();
    }

    private void EndBtn_Click(object sender, EventArgs e)
    {
        m_CallManager.EndCall(m_Call.CallID);
        CloseBtn_Click(this, new EventArgs());
    }

    private void SendBtn_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(NewMessageTb.Text) == true)
        {

            return;
        }

        string strMessage = new string(NewMessageTb.Text);
        NewMessageTb.Text = string.Empty;
        if (m_Call.MsrpConnection != null)
        {
            // TODO: Implement sending CPIM messages
            m_Call.SendTextPlainMsrp(strMessage);
        }
        else
            m_Call.SendRttMessage(strMessage);
    }

    private void NewMessageTb_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (m_Call.MsrpConnection != null)
        {
            if (e.KeyChar == '\r')
            {
                SendBtn_Click(this, new EventArgs());
            }
        }
        else
        {
            m_Call.SendRttMessage(e.KeyChar.ToString());
            if (e.KeyChar == '\r' || e.KeyChar == '\n')
            {
                NewMessageTb.Text = string.Empty;
                m_TextMessages.ClearLastSource();   // Force the next characters into a new row
            }
        }
    }

    private void AnswerBtn_Click(object sender, EventArgs e)
    {
        if (m_Call.CallState != CallStateEnum.OnLine)
        {
            m_CallManager.PickupCall(m_Call.CallID);
        }
    }

    private void HoldBtn_Click(object sender, EventArgs e)
    {
        if (m_Call.CallState == CallStateEnum.OnLine || m_Call.CallState == CallStateEnum.AutoAnswered)
            m_CallManager.PutCallOnHold(m_Call.CallID);
    }

    private void OnPreviewFrameBitmapReady(Bitmap bitmap)
    {
        if (m_IsClosing == true)
            return;

        BeginInvoke(() => { PreviewVideoPb.Image = bitmap; });
    }

    private void LocRefreshBtn_Click(object sender, EventArgs e)
    {
        if (m_Call.CallState == CallStateEnum.Ended)
        {
            MessageBox.Show("The call has ended", "Call Ended", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        bool LocByRefAvailable = m_Call.RefreshLocationByReference();
        if (LocByRefAvailable == false)
            MessageBox.Show("Location refresh is not available", "Information", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
    }

    // Conference/Transfer button click event handler
    private void ReferBtn_Click(object sender, EventArgs e)
    {
        if (m_Call.CallState != CallStateEnum.OnLine)
        {
            MessageBox.Show("Cannot conference or transfer the call because it is not on-line.\nPlease answer " +
                "the call and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (m_Call.CallerUserAgentIsConferenceAware == false)
        {
            MessageBox.Show("Cannot transfer or conference this call because no conference bridge is available " +
                "for this call", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        SelectTransferTargetForm form = new SelectTransferTargetForm();
        DialogResult Dr = form.ShowDialog();
        if (Dr == DialogResult.OK)
        {
            if (form.SelectedTransferTarget != null)
                m_CallManager.StartTransfer(m_Call, form.SelectedTransferTarget);
            else
                MessageBox.Show("No Transfer Target selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DropBtn_Click(object sender, EventArgs e)
    {
        if (m_Call.IsConferenced == false)
        {
            MessageBox.Show("Cannot drop a conference member because the call is not conferenced",
                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (m_Call.CallState != CallStateEnum.OnLine)
        {
            MessageBox.Show("Cannot drop a conference member because the call is not on-line.\nPlease answer " +
                "the call and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        ListViewItem? Lvi = GetCheckedListViewItem();
        if (Lvi == null)
            return;     // Error message already displayed

        usertype user = (usertype)Lvi.Tag!;
        string strEntity = user.entity;
        SIPURI? transferTargetUri = null;
        if (SIPURI.TryParse(strEntity, out transferTargetUri) == true)
            m_CallManager.StartDropTransferTarget(m_Call, transferTargetUri!.ToParameterlessString());
        else
        {
            // TODO: Handle this error.
        }
    }

    private ListViewItem? GetCheckedListViewItem()
    {
        if (ConfListView.CheckedIndices.Count == 0)
        {
            MessageBox.Show("Please select a conference member from the list first.", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return null;
        }

        if (ConfListView.CheckedIndices.Count > 1)
        {
            MessageBox.Show("Please select a single conference member from the list first.", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return null;
        }

        return ConfListView.Items[ConfListView.CheckedIndices[0]];
    }

    private void DropLastBtn_Click(object sender, EventArgs e)
    {
        if (m_Call.IsConferenced == false)
        {
            MessageBox.Show("Cannot drop the last added conference member because the call is not conferenced",
                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (m_Call.CallState != CallStateEnum.OnLine)
        {
            MessageBox.Show("Cannot drop the last added conference member for the call because it is not on-line.\nPlease answer " +
                "the call and try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        m_CallManager.StartDropLast(m_Call);
    }

    // 3 Jun 25 PHR -- For debug only
    private void MicMuteBtn_Click(object sender, EventArgs e)
    {
        if (m_Call.CallState != CallStateEnum.OnLine && m_Call.CallState != CallStateEnum.AutoAnswered &&
            m_Call.CallState != CallStateEnum.OnHold)
        {
            MessageBox.Show("The call must be Auto Answered, On-Line or On-Hold in order to use Mic Mute", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (m_Call.MicMuteOn == false)
        {
            m_CallManager.SetMicMuteOn(m_Call);
            MicMuteBtn.Text = "Mic Mute On";
        }
        else
        {
            m_CallManager.SetMicMuteOff(m_Call);
            MicMuteBtn.Text = "Mic Mute Off";
        }
    }

    private void KeypadBtn_Click(object sender, EventArgs e)
    {
        if (m_Call.AudioSampleSource == null)
        {
            MessageBox.Show("Error", "No audio media for this call", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        KeypadForm form = new KeypadForm(m_Call, m_Call.AudioSampleSource);
        form.ShowDialog();
    }

    private void HelpBtn_Click(object sender, EventArgs e)
    {
        HelpUtils.ShowHelpTopic(HelpUtils.CALL_FORM_HELP_URI);
    }

    private void AddMediaBtn_Click(object sender, EventArgs e)
    {
        if (m_Call.CallState != CallStateEnum.OnLine)
        {
            MessageBox.Show("The call must be on-line to add media to it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        List<string> availableMediaTypes = new List<string>();

        // Allow the user to select from only enabled media types.
        if (m_CallManager.HandlingSettings.EnableAudio == true)
            availableMediaTypes.Add("Audio");
        if (m_CallManager.HandlingSettings.EnableVideo == true)
            availableMediaTypes.Add("Video");
        if (m_CallManager.HandlingSettings.EnableRtt == true)
            availableMediaTypes.Add("RTT");
        if (m_CallManager.HandlingSettings.EnableMsrp == true)
            availableMediaTypes.Add("MSRP");

        // Remove media types that the call already has.
        List<string> callMediaTypes = new List<string>();
        if (string.IsNullOrEmpty(MediaLbl.Text) == false)
        {
            string[] mediaTypes = MediaLbl.Text.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string strMedia in mediaTypes)
            {
                callMediaTypes.Add(strMedia);
                if (availableMediaTypes.Contains(strMedia) == true)
                    availableMediaTypes.Remove(strMedia);
            }
        }

        // Check for two text types -- RTT and MSRP, can only have one of these for the call.
        if (callMediaTypes.Contains("RTT") == true && availableMediaTypes.Contains("MSRP") == true)
            availableMediaTypes.Remove("MSRP");
        else if (callMediaTypes.Contains("MSRP") == true && availableMediaTypes.Contains("RTT") == true)
            availableMediaTypes.Remove("RTT");

        if (availableMediaTypes.Count == 0)
        {
            MessageBox.Show("There are no media types that can be added to this call.", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        AddNewMediaForm newMediaForm = new AddNewMediaForm(availableMediaTypes);
        DialogResult result = newMediaForm.ShowDialog();
        if (result != DialogResult.OK)
            return;

        List<string> selectedMediaTypes = newMediaForm.GetSelectedMedia();
        if (selectedMediaTypes.Count == 0)
            return;

        List<string> mediaTypesToAdd = new List<string>();
        foreach (string strSelected in selectedMediaTypes)
            mediaTypesToAdd.Add(MediaDisplayTypeToMediaType(strSelected));

        m_CallManager.SendReInviteToAddMedia(mediaTypesToAdd, m_Call.CallID);
    }
}
