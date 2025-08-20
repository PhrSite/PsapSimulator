/////////////////////////////////////////////////////////////////////////////////////
//  File:   Form1.cs                                                2 Feb 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

using PsapSimulator.CallManagement;
using PsapSimulator.Settings;
using SipLib.Network;
using SipLib.Logging;
using System.Net;
using System.Diagnostics;
using PsapSimulator.WindowsVideo;
using System.Text;
using System.Threading;

namespace PsapSimulator;

/// <summary>
/// Main form class for this application
/// </summary>
public partial class Form1 : Form
{
    private string? m_CurrentCallID = null;
    private CallForm? m_CallForm = null;
    private Dictionary<string, List<VideoDeviceFormat>>? m_VideoDevices = null;

    /// <summary>
    /// Constructor
    /// </summary>
    public Form1()
    {
        InitializeComponent();

    }

    private bool m_CloseBtnClicket = false;

    private void CloseBtn_Click(object sender, EventArgs e)
    {
        m_CloseBtnClicket = true;
        Close();
    }

    private void SettingsBtn_Click(object sender, EventArgs e)
    {
        AppSettings appSettings = AppSettings.GetAppSettings();
        AppSettings temp = (AppSettings)Utils.CopyObject(appSettings);
        SettingsForm settingsForm = new SettingsForm(temp, m_VideoDevices!);
        DialogResult result = settingsForm.ShowDialog();
        if (result == DialogResult.OK)
            AppSettings.SaveAppSettings(temp);
    }

    private async void Form1_Load(object sender, EventArgs e)
    {
        SetCallListViewColumns();
        m_VideoDevices = await VideoDeviceEnumerator.GetVideoFrameSources();
        ShowStatus();
    }

    private void SetCallListViewColumns()
    {
        int ColWidth = CallListView.ClientRectangle.Width / CallListView.Columns.Count - 4;
        for (int i = 0; i < CallListView.Columns.Count; i++)
        {
            CallListView.Columns[i].Width = ColWidth;
        }
    }

    private void Form1_FormClosed(object sender, FormClosedEventArgs e)
    {

    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
        // Allow only the Close button in the form to close this form.
        if (m_CloseBtnClicket == false)
            e.Cancel = true;
    }

    private void Form1_SizeChanged(object sender, EventArgs e)
    {
        SetCallListViewColumns();
    }

    private CallManager? m_CallManager = null;

    private async void StartBtn_Click(object sender, EventArgs e)
    {
        if (m_CallManager == null)
        {
            AppSettings appSettings = AppSettings.GetAppSettings();
            if (OkToStart(appSettings) == false)
                return;     // The user has already been notified of the problem.

            Application.UseWaitCursor = true;
            CallListView.Items.Clear();
            SettingsBtn.Enabled = false;
            CloseBtn.Enabled = false;
            StartBtn.Text = "Starting...";

            try
            {
                m_CallManager = new CallManager(appSettings);
            }
            catch (Exception ex)
            {
                string strMessage = "Unable to start the call manager. See the log file located " +
                    $"in: '{Program.LoggingDirectory}'";
                MessageBox.Show(strMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SipLogger.LogCritical(ex, "Unable to start the call manager");
                Application.UseWaitCursor = false;
                return;
            }

            m_CallManager.NewCall += OnNewCall;
            m_CallManager.CallEnded += OnCallEnded;
            m_CallManager.CallStateChanged += OnCallStateChanged;
            m_CallManager.CallManagerError += OnCallManagerError;

            await m_CallManager.Start();
            StartBtn.Text = "Stop";
            Application.UseWaitCursor = false;
        }
        else
        {   // The CallManager is currently running so shut it down.
            if (CallListView.Items.Count > 0)
            {   // But there are currently active calls that need to be terminated first
                MessageBox.Show("There are active calls. Press End All first", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            Application.UseWaitCursor = true;
            await ShutdownCallManager();
            StartBtn.Text = "Start";
            CloseBtn.Enabled = true;
            SettingsBtn.Enabled = true;
            Application.UseWaitCursor = false;
        }

        ShowStatus();
    }

    private void ShowStatus()
    {
        if (m_CallManager == null)
        {
            StatusLbl.Text = "Not listening. Press Start";
            StatusLbl.BackColor = Color.Red;
            StatusLbl.ForeColor = Color.White;
            StatesBtn.Enabled = false;
        }
        else
        {
            AppSettings appSettings = AppSettings.GetAppSettings();
            NetworkSettings netSet = appSettings.NetworkSettings;

            StringBuilder sb = new StringBuilder();
            sb.Append("Listening: ");
            if (netSet.EnableIPv4 == true)
                sb.Append($"IPv4: {netSet.IPv4Address}  ");

            if (netSet.EnableIPv6 == true)
                sb.Append($"IPv6: {netSet.IPv6Address} ");

            List<string> protoList = new List<string>();
            if (netSet.EnableUdp == true)
                protoList.Add("UDP");

            if (netSet.EnableTcp == true)
                protoList.Add("TCP");

            if (netSet.EnableTls == true)
                protoList.Add("TLS");

            int i;
            sb.Append("       Protocols: ");
            for (i = 0; i < protoList.Count; i++)
            {
                if (i >= 1)
                    sb.Append(",");

                sb.Append(protoList[i]);
            }

            sb.Append($"       SIP: {netSet.SipPort}, ");
            sb.Append($"SIPS: {netSet.SipsPort}");

            StatusLbl.Text = sb.ToString();
            StatusLbl.BackColor = Color.Green;
            StatusLbl.ForeColor = Color.White;
            StatesBtn.Enabled = true;
        }
    }

    private async Task ShutdownCallManager()
    {
        if (m_CallManager == null)
            return;

        m_CallManager.NewCall -= OnNewCall;
        m_CallManager.CallEnded -= OnCallEnded;
        m_CallManager.CallStateChanged -= OnCallStateChanged;
        m_CallManager.CallManagerError -= OnCallManagerError;

        await m_CallManager.Shutdown();
        m_CallManager = null;
    }

    private bool OkToStart(AppSettings appSettings)
    {
        NetworkSettings Ns = appSettings.NetworkSettings;
        if (Ns.EnableIPv4 == true)
        {   // Make sure that the configured IPv4 address is available
            if (IpAddressExists(Ns.IPv4Address!, IpUtils.GetIPv4Addresses(), "IPv4") == false)
                return false;
        }

        if (Ns.EnableIPv6 == true)
        {   // Make sure that the configured IPv6 address is available
            if (IpAddressExists(Ns.IPv6Address!, IpUtils.GetIPv6Addresses(), "IPv6") == false)
                return false;
        }

        return true;
    }

    private bool IpAddressExists(string ipAddress, List<IPAddress> addresses, string networkType)
    {
        foreach (IPAddress ip in addresses)
        {
            if (ipAddress == ip.ToString())
                return true;
        }

        SipLogger.LogError($"Unable to start because the {networkType} address '{ipAddress}' is not available.");
        MessageBox.Show($"The {networkType} address: {ipAddress} is not available. Select an available " +
            $"{networkType} address in the Network tab in the Settings dialog box", "Error", MessageBoxButtons.OK,
            MessageBoxIcon.Error);
        return false;
    }

    private void OnNewCall(CallSummary callSummary)
    {
        BeginInvoke(() => { AddNewCall(callSummary); });
    }

    private const int FromIndex = 0;
    private const int StartTimeIndex = 1;
    private const int CallStateIndex = 2;
    private const int QueueURIIndex = 3;
    private const int ConferencedIndex = 4;
    private const int CallMediaIndex = 5;

    private void AddNewCall(CallSummary callSummary)
    {
        ListViewItem Lvi = new ListViewItem(callSummary.From);
        Lvi.SubItems.Add(callSummary.StartTime.ToString("HH:mm:ss"));
        Lvi.SubItems.Add(Call.CallStateToString(callSummary.CallState));
        Lvi.SubItems.Add(callSummary.QueueURI);
        Lvi.SubItems.Add(callSummary.Conferenced == true ? "Yes" : "No");
        Lvi.SubItems.Add(callSummary.CallMedia);
        Lvi.Tag = callSummary.CallID;
        CallListView.Items.Add(Lvi);

        UpdateCallCounts();
    }

    private void OnCallStateChanged(CallSummary callSummary)
    {
        BeginInvoke(() => { UpdateCallState(callSummary); });
    }

    private void UpdateCallState(CallSummary callSummary)
    {
        ListViewItem? Lvi = FindCallListViewItem(callSummary.CallID);
        if (Lvi == null)
            return;     // Error: call not found

        Lvi.SubItems[CallStateIndex].Text = callSummary.CallState.ToString();
        Lvi.SubItems[ConferencedIndex].Text = callSummary.Conferenced == true ? "Yes" : "No";
        Lvi.SubItems[CallMediaIndex].Text = callSummary.CallMedia;

        UpdateCallCounts();
    }

    private void UpdateCallCounts()
    {
        CallCounts callCounts = m_CallManager!.GetCallCounts();
        TotalCallsLbl.Text = callCounts.TotalCalls.ToString();
        RingingLbl.Text = callCounts.Ringing.ToString();
        AnsweredLbl.Text = callCounts.AutoAnswered.ToString();
        OnLineLbl.Text = callCounts.OnLine.ToString();
        HoldLbl.Text = callCounts.OnHold.ToString();
    }

    private void ShowCallForm(string callID)
    {
        if (m_CallForm != null && m_CurrentCallID != null && m_CurrentCallID != callID)
        {
            m_CallForm.Close();
            m_CallForm = null;
            m_CurrentCallID = null;
        }

        Call? call = m_CallManager!.GetCall(callID);
        if (call == null)
        {
            return;
        }

        m_CurrentCallID = callID;
        m_CallForm = new CallForm(m_CallManager, call);
        //m_CallForm.WindowState = FormWindowState.Maximized;
        m_CallForm.ShowDialog();

        m_CurrentCallID = null;
        m_CallForm = null;
    }

    private void OnCallEnded(string callID)
    {
        BeginInvoke(() => { RemoveCall(callID); });
    }

    private void RemoveCall(string callID)
    {
        ListViewItem? lvi = FindCallListViewItem(callID);
        if (lvi != null)
            CallListView.Items.Remove(lvi);

        UpdateCallCounts();
    }

    private ListViewItem? FindCallListViewItem(string callID)
    {
        ListViewItem? callLvi = null;
        foreach (ListViewItem lvi in CallListView.Items)
        {
            if (lvi.Tag != null && lvi.Tag.ToString() == callID)
                return lvi;
        }

        return callLvi;
    }

    private void ShowNotRunning()
    {
        MessageBox.Show("Not running. Press Start.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private void AnswerBtn_Click(object sender, EventArgs e)
    {
        if (m_CallManager == null)
        {
            ShowNotRunning();
            return;
        }

        m_CallManager.Answer();
    }

    private void OnCallManagerError(string strMessage)
    {
        if (m_CallForm == null)
            BeginInvoke(() => MessageBox.Show(strMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error));
    }

    private string? GetSelecteCallID()
    {
        string? callID = null;
        if (CallListView.SelectedIndices.Count > 0)
        {
            int index = CallListView.SelectedIndices[0];
            callID = CallListView.Items[index].Tag!.ToString();

        }

        return callID;
    }

    private void ShowBtn_Click(object sender, EventArgs e)
    {
        if (m_CallManager == null)
        {
            ShowNotRunning();
            return;
        }

        if (CallListView.Items.Count == 0)
        {
            MessageBox.Show("No calls to pick up", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        string? callID = GetSelecteCallID();
        if (callID == null)
        {
            MessageBox.Show("Please select a call to pickup", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (m_CallForm == null)
        {
            ShowCallForm(callID);
            m_CurrentCallID = null;
            m_CallForm = null;
        }

        //m_CallManager.PickupCall(callID);
    }

    private void EndCallBtn_Click(object sender, EventArgs e)
    {
        if (m_CallManager == null)
        {
            ShowNotRunning();
            return;
        }

        string? callID = GetSelecteCallID();
        if (callID == null)
        {
            MessageBox.Show("Please select a call to end", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        m_CallManager.EndCall(callID);
    }

    private void HoldBtn_Click(object sender, EventArgs e)
    {
        if (m_CallManager == null)
        {
            ShowNotRunning();
            return;
        }

        string? callID = GetSelecteCallID();
        if (callID == null)
        {
            MessageBox.Show("Please select a call to put on hold", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        m_CallManager.PutCallOnHold(callID);
    }


    private void EndAllBtn_Click(object sender, EventArgs e)
    {
        if (m_CallManager == null)
        {
            ShowNotRunning();
            return;
        }

        if (CallListView.Items.Count == 0)
        {
            MessageBox.Show("No calls to end", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        m_CallManager?.EndAllCalls();
    }

    private void CallListView_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        string? callID = GetSelecteCallID();
        if (callID == null)
        {
            MessageBox.Show("Please select a call to pickup", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        ShowCallForm(callID);
        m_CurrentCallID = null;
        m_CallForm = null;
    }

    private void StatesBtn_Click(object sender, EventArgs e)
    {
        if (m_CallManager == null)
            return;

        PsapStatesForm form = new PsapStatesForm(m_CallManager);
        form.ShowDialog();
    }
}
