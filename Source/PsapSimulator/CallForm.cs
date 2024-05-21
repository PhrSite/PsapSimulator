/////////////////////////////////////////////////////////////////////////////////////
//  File:   CallForm.cs                                             19 Mar 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator;
using PsapSimulator.CallManagement;
using PsapSimulator.WindowsVideo;

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
        else
        {
            TextTypeLbl.Text = "RTT";
            UseCpimCheck.Visible = false;
            PrivateMsgCheck.Visible = false;
            SendBtn.Visible = false;
        }
        
        m_TextMessages.MessageAdded += OnMessageAdded;
        m_TextMessages.MessageUpdated += OnMessageUpdated;
        m_CallManager.CallStateChanged += OnCallStateChanged;
        m_CallManager.CallEnded += OnCallEnded;

        //m_CallManager.FrameBitmapReady += OnPreviewFrameBitmapReady;
        //m_Call.CurrentVideoCapture!.FrameBitmapReady += OnPreviewFrameBitmapReady;
        SetVideoPreviewSource();

        if (m_Call.VideoReceiver != null)
        {
            m_Call.VideoReceiver.FrameReady += OnFrameReady;
        }
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

        BeginInvoke(() => { CallStateLbl.Text = Call.CallStateToString(m_Call.CallState); });
        BeginInvoke(() => SetVideoPreviewSource());

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
        // TODO: unhook the events
        m_TextMessages.MessageAdded -= OnMessageAdded;
        m_TextMessages.MessageUpdated -= OnMessageUpdated;
        m_CallManager.CallStateChanged -= OnCallStateChanged;
        m_CallManager.CallEnded -= OnCallEnded;

        m_CallManager.FrameBitmapReady -= OnPreviewFrameBitmapReady;

        if (m_Call.VideoReceiver != null)
        {
            m_Call.VideoReceiver.FrameReady -= OnFrameReady;
        }

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
            m_Call.SendTextPlainMsrp(strMessage);
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
        if (m_Call.CallState == CallStateEnum.AutoAnswered || m_Call.CallState == CallStateEnum.OnHold)
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

}
