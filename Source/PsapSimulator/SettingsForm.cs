/////////////////////////////////////////////////////////////////////////////////////
//  File:   SettingsForm.cs                                         6 Feb 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator;
using PsapSimulator.Settings;
using SipLib.Media;
using SipLib.Network;
using System.Net;
using WindowsWaveAudio;

using PsapSimulator.WindowsVideo;
using System.Windows.Forms;
using System.Collections.Generic;

/// <summary>
/// Main settings form
/// </summary>
public partial class SettingsForm : Form
{
    private AppSettings m_AppSettings;
    private bool m_IsLoading = true;

    public SettingsForm(AppSettings appSettings, Dictionary<string, List<VideoDeviceFormat>> videoDevices)
    {
        m_AppSettings = appSettings;
        m_VideoDevices = videoDevices;
        InitializeComponent();
    }

    private void SaveBtn_Click(object sender, EventArgs e)
    {
        if (ValidateSettings() == false)
            return;

        // Network tab
        NetworkSettings Ns = m_AppSettings.NetworkSettings;
        Ns.EnableIPv4 = IPv4Check.Checked;
        if (Ns.EnableIPv4 == true)
            Ns.IPv4Address = IPv4Combo.Text;
        Ns.EnableIPv6 = IPv6Check.Checked;
        if (Ns.EnableIPv6 == true)
            Ns.IPv6Address = IPv6Combo.Text;

        Ns.SipPort = int.Parse(SipPortTb.Text);
        Ns.SipsPort = int.Parse(SipsPortTb.Text);

        Ns.EnableUdp = EnableUdpCb.Checked;
        Ns.EnableTcp = EnableTcpCb.Checked;
        Ns.EnableTls = EnableTlsCb.Checked;

        Ns.MediaPorts.AudioPorts = GetMediaPortRange(AudioRow);
        Ns.MediaPorts.VideoPorts = GetMediaPortRange(VideoRow);
        Ns.MediaPorts.RttPorts = GetMediaPortRange(RttRow);
        Ns.MediaPorts.MsrpPorts = GetMediaPortRange(MsrpRow);
        Ns.UseMutualTlsAuthentication = MutualAuthCheck.Checked;

        // Identity tab
        IdentitySettings Is = m_AppSettings.Identity;
        Is.AgencyID = AgencyIDTb.Text;
        Is.AgentID = AgentIDTb.Text;
        Is.ElementID = ElementIDTb.Text;
        CertificateSettings Cs = m_AppSettings.CertificateSettings;
        Cs.UseDefaultCertificate = DefaultCertCb.Checked;
        Cs.CertificateFilePath = CertFileTb.Text;
        Cs.CertificatePassword = CertPasswordTb.Text;

        // Call Handling tab
        CallHandlingSettings Chs = m_AppSettings.CallHandling;
        Chs.MaximumCalls = int.Parse(MaxCallsTb.Text);
        Chs.MaximumNonInteractiveCalls = int.Parse(NonInteractiveCallsTb.Text);
        Chs.EnableAutoAnswer = EnableAutoAnswerCb.Checked;
        Chs.EnableAudio = EnableAudioCb.Checked;
        Chs.EnableVideo = EnableVideoCb.Checked;
        Chs.EnableRtt = EnableRttCb.Checked;
        Chs.EnableMsrp = EnableMsrpCb.Checked;
        Chs.EnableTransmitVideo = EnableTransmitVideoCb.Checked;
        Chs.RtpEncryption = (RtpEncryptionEnum)RtpEncryptionCombo.SelectedIndex;
        Chs.MsrpEncryption = (MsrpEncryptionEnum)MsrpEncryptionCombo.SelectedIndex;

        // Media Sources tab
        Chs.CallHoldAudio = (CallHoldAudioSource)CallHoldCombo.SelectedIndex;
        Chs.CallHoldAudioFile = HoldAudioTb.Text;
        Chs.CallHoldVideoFile = HoldVideoTb.Text;
        Chs.CallHoldTextMessage = HoldTextTb.Text;
        Chs.CallHoldTextMessageRepeatSeconds = int.Parse(HoldTextRepeatTb.Text);
        Chs.AutoAnswerAudioFile = AutoAnswerAudioTb.Text;
        Chs.AutoAnswerVideoFile = AutoAnswerVideoTb.Text;
        Chs.AutoAnswerTextMessage = AutoAnswerTextTb.Text;
        Chs.AutoAnswerTextMessageRepeatSeconds = int.Parse(AutoAnswerTextRepeatTb.Text);

        // Devices tab
        DeviceSettings Ds = m_AppSettings.Devices;
        if (AudioDeviceCombo.SelectedIndex >= 0)
            Ds.AudioDeviceName = AudioDeviceCombo.Text;

        if (VideoDevicesCombo.SelectedIndex >= 0)
        {
            Ds.VideoDevice = new VideoSourceSettings();
            Ds.VideoDevice.SelectedDeviceName = VideoDevicesCombo.Text;
            if (VideoListView.CheckedIndices.Count > 0)
            {
                int index = VideoListView.CheckedIndices[0];
                Ds.VideoDevice.DeviceFormat = (VideoDeviceFormat)VideoListView.Items[index].Tag!;
            }
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void CancelBtn_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private bool ValidateSettings()
    {
        if (IPv4Check.Checked == true)
        {
            if (IPv4Combo.SelectedIndex < 0)
            {
                MessageBox.Show("An IPv4 IP address must be selected if IPv4 is enabled", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        if (IPv6Check.Checked == true)
        {
            if (IPv6Combo.SelectedIndex < 0)
            {
                MessageBox.Show("An IPv6 IP address must be selected if IPv6 is enabled", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        if (IPv4Check.Checked == false && IPv6Check.Checked == false)
        {
            MessageBox.Show("IPv4 or IPv6 or both must be enabled", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return false;
        }

        if (string.IsNullOrEmpty(SipPortTb.Text) == true && int.TryParse(SipPortTb.Text, out int sipPort) == false)
        {
            MessageBox.Show("The SIP Port must be specified", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            SipPortTb.Focus();
            return false;
        }

        if (string.IsNullOrEmpty(SipsPortTb.Text) == true && int.TryParse(SipsPortTb.Text, out int sipsPort) == false)
        {
            MessageBox.Show("The SIP Port must be specified", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            SipPortTb.Focus();
            return false;
        }

        if (SipPortTb.Text == SipsPortTb.Text)
        {
            MessageBox.Show("The SIP Port and the SIPS Port must be different", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            SipPortTb.Focus();
            return false;
        }

        if (EnableUdpCb.Checked == false && EnableTcpCb.Checked == false && EnableTlsCb.Checked == false)
        {
            MessageBox.Show("At least one transport protocol (UDP, TCP, TLS) must be enabled", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        if (ValidateMediaPortSettings() == false)
            return false;

        if (string.IsNullOrEmpty(AgencyIDTb.Text) == true)
        {
            MessageBox.Show("The Agency ID must be specified", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            AgencyIDTb.Focus();
            return false;
        }

        if (string.IsNullOrEmpty(AgentIDTb.Text) == true)
        {
            MessageBox.Show("The Agent ID must be specified", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            AgentIDTb.Focus();
            return false;
        }

        if (string.IsNullOrEmpty(ElementIDTb.Text) == true)
        {
            MessageBox.Show("The Element ID must be specified", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            ElementIDTb.Focus();
            return false;
        }

        if (DefaultCertCb.Checked == false)
        {
            if (string.IsNullOrEmpty(CertFileTb.Text) == true)
            {
                MessageBox.Show("The X.509 certificate file location must be set", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                DefaultCertCb.Focus();
                return false;
            }

            if (File.Exists(CertFileTb.Text) == false)
            {
                MessageBox.Show("The X.509 certificate file does not exist", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                DefaultCertCb.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(CertPasswordTb.Text) == true)
            {
                MessageBox.Show("The X.509 certificate password is required", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                CertPasswordTb.Focus();
                return false;
            }
        }

        // Call Handling page
        if (string.IsNullOrEmpty(MaxCallsTb.Text) == true)
        {
            MessageBox.Show("Maximum Calls must be set", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            MaxCallsTb.Focus();
            return false;
        }

        if (int.TryParse(MaxCallsTb.Text, out int MaxCalls) == false || MaxCalls <= 0)
        {
            MessageBox.Show("Maximum Calls must be an integer value greater than 0", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            MaxCallsTb.Focus();
            return false;
        }

        if (string.IsNullOrEmpty(NonInteractiveCallsTb.Text) == true)
        {
            MessageBox.Show("Maximum Non-Interactive Calls must be set", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            NonInteractiveCallsTb.Focus();
            return false;
        }

        if (int.TryParse(NonInteractiveCallsTb.Text, out int NonInterCalls) == false || NonInterCalls <= 0)
        {
            MessageBox.Show("Maximum Non-Interactive Calls must be an integer value greater than 0",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            NonInteractiveCallsTb.Focus();
            return false;
        }

        if (EnableAudioCb.Checked == false && EnableVideoCb.Checked == false && EnableRttCb.Checked == false &&
            EnableMsrpCb.Checked == false)
        {
            MessageBox.Show("At least one media type must be enabled", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return false;
        }

        // Media Sources page
        if (string.IsNullOrEmpty(HoldAudioTb.Text) == true || File.Exists(HoldAudioTb.Text) == false)
        {
            MessageBox.Show("The Hold Audio file is not specified or does not exist", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            HoldAudioTb.Focus();
            return false;
        }

        if (string.IsNullOrEmpty(HoldVideoTb.Text) == true || File.Exists(HoldVideoTb.Text) == false)
        {
            MessageBox.Show("The Hold Video file is not specified or does not exist", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            HoldVideoTb.Focus();
            return false;
        }

        if (string.IsNullOrEmpty(HoldTextTb.Text) == true)
        {
            MessageBox.Show("The Hold Text Message is required", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            HoldTextTb.Focus();
            return false;
        }

        if (int.TryParse(HoldTextRepeatTb.Text, out int holdRepeat) == false || holdRepeat < 0)
        {
            MessageBox.Show("Hold Text Repeat must be an integer greater than or equal to 0", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            HoldTextRepeatTb.Focus();
            return false;
        }

        if (string.IsNullOrEmpty(AutoAnswerAudioTb.Text) == true || File.Exists(AutoAnswerAudioTb.Text) == false)
        {
            MessageBox.Show("The Auto Answer Audio file is not specified or does not exist", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            AutoAnswerAudioTb.Focus();
            return false;
        }

        if (string.IsNullOrEmpty(AutoAnswerVideoTb.Text) == true || File.Exists(AutoAnswerVideoTb.Text) == false)
        {
            MessageBox.Show("The Auto Answer Audio file is not specified or does not exist", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            AutoAnswerVideoTb.Focus();
            return false;
        }

        if (string.IsNullOrEmpty(AutoAnswerTextTb.Text) == true)
        {
            MessageBox.Show("The Auto Answer Text Message is required", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            AutoAnswerTextTb.Focus();
            return false;
        }

        if (int.TryParse(AutoAnswerTextRepeatTb.Text, out int autoRepeat) == false || autoRepeat < 0)
        {
            MessageBox.Show("Hold Text Repeat must be an integer greater than or equal to 0", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            AutoAnswerTextRepeatTb.Focus();
            return false;
        }

        // Devices tab
        if (EnableAudioCb.Checked == true)
        {   // Make sure that an audio device is selected
            if (AudioDeviceCombo.SelectedIndex < 0)
            {
                MessageBox.Show("Audio media is enabled but no audio device is selected in the Devices tab", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        if (EnableVideoCb.Checked == true)
        {
            if (VideoDevicesCombo.SelectedIndex < 0)
            {
                MessageBox.Show("Video media is enabled but no video device is selected in the Devices tab", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (VideoListView.CheckedIndices.Count == 0)
            {
                MessageBox.Show("Video media is enabled but no video format is selected in the Devices tab", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;

            }
        }

        return true;
    }

    private const int AudioRow = 0;
    private const int VideoRow = 1;
    private const int RttRow = 2;
    private const int MsrpRow = 3;

    private const int StartPortCell = 1;
    private const int CountCell = 2;

    private readonly string[] MediaTypes = { "Audio", "Video", "RTT", "MSRP" };

    private bool ValidateMediaPortSettings()
    {
        int i;
        for (i = AudioRow; i <= MsrpRow; i++)
        {
            if (ValidateMediaRange(i) == false)
                return false;
        }

        List<PortRange> ports = new List<PortRange>();
        for (i = AudioRow; i <= MsrpRow; i++)
            ports.Add(GetMediaPortRange(i));

        // Test for port range overlaps. Don't care about MSRP because it uses TCP and the other media
        // types use UDP so port range overlaps are not a problem.
        for (i = AudioRow; i < MsrpRow; i++)
        {
            int CurrentStart = ports[i].StartPort;
            int CurrentEnd = CurrentStart + ports[i].Count - 1;
            int NextStart;
            int NextEnd;
            for (int j = i + 1; j < MsrpRow; j++)
            {
                NextStart = ports[j].StartPort;
                NextEnd = NextStart + ports[j].Count - 1;

                if (CurrentStart >= NextStart && CurrentStart <= NextEnd || (CurrentEnd >= NextStart &&
                    CurrentEnd <= NextEnd))
                {
                    MessageBox.Show($"The port range for {MediaTypes[i]} overlaps the port range for {MediaTypes[j]}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (NextStart >= CurrentStart && NextStart <= CurrentEnd || (NextEnd >= CurrentStart &&
                    NextEnd <= CurrentEnd))
                {
                    MessageBox.Show($"The port range for {MediaTypes[i]} overlaps the port range for {MediaTypes[j]}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        return true;
    }

    private PortRange GetMediaPortRange(int MediaIndex)
    {
        PortRange range = new PortRange();
        range.StartPort = int.Parse(PortsGridView.Rows[MediaIndex].Cells[StartPortCell].Value.ToString()!);
        range.Count = int.Parse(PortsGridView.Rows[MediaIndex].Cells[CountCell].Value.ToString()!);
        return range;
    }

    private bool ValidateMediaRange(int Row)
    {
        int StartPort = 0, Count = 0;

        object StartVal = PortsGridView.Rows[Row].Cells[StartPortCell].Value;
        object CountVal = PortsGridView.Rows[Row].Cells[CountCell].Value;

        if (StartVal == null)
        {
            MessageBox.Show($"The Start Port for {MediaTypes[Row]} must be set", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return false;
        }

        if (CountVal == null)
        {
            MessageBox.Show($"The Count for {MediaTypes[Row]} must be set", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return false;
        }

        if (int.TryParse(StartVal.ToString(), out StartPort) == false || StartPort <= 1024 || StartPort > 65535)
        {
            MessageBox.Show($"The Start Port for {MediaTypes[Row]} must be an integer value greater than 1024 " +
                "and less than or equal to 65535", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        if (int.TryParse(CountVal.ToString(), out Count) == false || Count <= 0)
        {
            MessageBox.Show($"The Count for {MediaTypes[Row]} must be an integer value greater than 0", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        return true;
    }

    private void SettingsForm_Load(object sender, EventArgs e)
    {
        IPv4Check.Checked = m_AppSettings.NetworkSettings.EnableIPv4;
        IPv6Check.Checked = m_AppSettings.NetworkSettings.EnableIPv6;

        SetIpAddress(IpUtils.GetIPv4Addresses(), IPv4Combo, m_AppSettings.NetworkSettings.IPv4Address);
        SetIpAddress(IpUtils.GetIPv6Addresses(), IPv6Combo, m_AppSettings.NetworkSettings.IPv6Address);

        SipPortTb.Text = m_AppSettings.NetworkSettings.SipPort.ToString();
        SipsPortTb.Text = m_AppSettings.NetworkSettings.SipsPort.ToString();

        EnableUdpCb.Checked = m_AppSettings.NetworkSettings.EnableUdp;
        EnableTcpCb.Checked = m_AppSettings.NetworkSettings.EnableTcp;
        EnableTlsCb.Checked = m_AppSettings.NetworkSettings.EnableTls;

        MutualAuthCheck.Checked = m_AppSettings.NetworkSettings.UseMutualTlsAuthentication;

        MediaPortSettings Mps = m_AppSettings.NetworkSettings.MediaPorts;
        PortsGridView.Rows.Add("Audio", Mps.AudioPorts.StartPort, Mps.AudioPorts.Count);
        PortsGridView.Rows.Add("Video", Mps.VideoPorts.StartPort, Mps.VideoPorts.Count);
        PortsGridView.Rows.Add("RTT", Mps.RttPorts.StartPort, Mps.RttPorts.Count);
        PortsGridView.Rows.Add("MSRP", Mps.MsrpPorts.StartPort, Mps.MsrpPorts.Count);

        // Identity page
        IdentitySettings identity = m_AppSettings.Identity;
        AgencyIDTb.Text = identity.AgencyID;
        AgentIDTb.Text = identity.AgentID;
        ElementIDTb.Text = identity.ElementID;

        DefaultCertCb.Checked = m_AppSettings.CertificateSettings.UseDefaultCertificate;
        CertFileTb.Text = m_AppSettings.CertificateSettings.CertificateFilePath;
        CertPasswordTb.Text = m_AppSettings.CertificateSettings.CertificatePassword;

        // Call Handling and Media Sources pages
        CallHandlingSettings Handling = m_AppSettings.CallHandling;
        MaxCallsTb.Text = Handling.MaximumCalls.ToString();
        NonInteractiveCallsTb.Text = Handling.MaximumNonInteractiveCalls.ToString();
        EnableAutoAnswerCb.Checked = Handling.EnableAutoAnswer;
        EnableAudioCb.Checked = Handling.EnableAudio;
        EnableVideoCb.Checked = Handling.EnableVideo;
        EnableRttCb.Checked = Handling.EnableRtt;
        EnableMsrpCb.Checked = Handling.EnableMsrp;
        EnableTransmitVideoCb.Checked = Handling.EnableTransmitVideo;
        RtpEncryptionCombo.SelectedIndex = (int)Handling.RtpEncryption;
        MsrpEncryptionCombo.SelectedIndex = (int)Handling.MsrpEncryption;
        CallHoldCombo.SelectedIndex = (int)Handling.CallHoldAudio;
        HoldAudioTb.Text = Handling.CallHoldAudioFile;
        HoldVideoTb.Text = Handling.CallHoldVideoFile;
        HoldTextTb.Text = Handling.CallHoldTextMessage;
        HoldTextRepeatTb.Text = Handling.CallHoldTextMessageRepeatSeconds.ToString();
        AutoAnswerAudioTb.Text = Handling.AutoAnswerAudioFile;
        AutoAnswerVideoTb.Text = Handling.AutoAnswerVideoFile;
        AutoAnswerTextTb.Text = Handling.AutoAnswerTextMessage;
        AutoAnswerTextRepeatTb.Text = Handling.AutoAnswerTextMessageRepeatSeconds.ToString();

        // Devices Page
        List<string> AudioDevices = WindowsAudioIo.GetAudioDeviceNames();
        foreach (string audioDevice in AudioDevices)
        {
            AudioDeviceCombo.Items.Add(audioDevice);
        }
        if (AudioDevices.Count > 0)
        {
            if (string.IsNullOrEmpty(m_AppSettings.Devices.AudioDeviceName) == false)
            {
                int AudioIndex = AudioDeviceCombo.FindString(m_AppSettings.Devices.AudioDeviceName);
                if (AudioIndex >= 0)
                    AudioDeviceCombo.SelectedIndex = AudioIndex;
                else
                    AudioDeviceCombo.SelectedIndex = 0;
            }
            else
                AudioDeviceCombo.SelectedIndex = 0;
        }

        int ColWidth = VideoListView.ClientRectangle.Width / 4 - 6;
        for (int i = 0; i < VideoListView.Columns.Count; i++)
            VideoListView.Columns[i].Width = ColWidth;

        SetVideoDeviceSelections();

        SetSipRecSummary();
        SetEventLoggingSummary();

        m_IsLoading = false;
    }

    private void SetSipRecSummary()
    {
        string RecordersEnabled;
        SipRecSettings Settings = m_AppSettings.SipRec;
        if (Settings.SipRecRecorders.Count == 0)
            RecordersEnabled = "No recorders enabled";
        else if (Settings.SipRecRecorders.Count == 1)
            RecordersEnabled = "1 recorder enabled";
        else
            RecordersEnabled = $"{Settings.SipRecRecorders.Count} recorders enabled";

        string Enabled = Settings.EnableSipRec == true ? "Enabled" : "Disabled";
        SipRecLbl.Text = $"{Enabled}: {RecordersEnabled}";
    }

    private void SetEventLoggingSummary()
    {
        string LoggersEnabled;
        EventLoggingSettings Settings = m_AppSettings.EventLogging;
        if (Settings.Loggers.Count == 0)
            LoggersEnabled = "No event loggers enabled";
        else if (Settings.Loggers.Count == 1)
            LoggersEnabled = "1 event logger enabled";
        else
            LoggersEnabled = $"{Settings.Loggers.Count} event loggers enabled";

        string Enabled = Settings.EnableLogging == true ? "Enabled" : "Disabled";
        EventLoggingLbl.Text = $"{Enabled}: {LoggersEnabled}";
    }

    private Dictionary<string, List<VideoDeviceFormat>>? m_VideoDevices = null;

    private void SetVideoDeviceSelections()
    {
        if (m_VideoDevices == null || m_VideoDevices.Keys.Count == 0)
            return;     // No video capture devices available

        foreach (string deviceName in m_VideoDevices.Keys)
            VideoDevicesCombo.Items.Add(deviceName);

        VideoSourceSettings? Vss = m_AppSettings.Devices.VideoDevice;
        string? SelectedDevice;
        if (Vss == null || Vss.SelectedDeviceName == null)
        {   // No video settings yet, so pick the first available device
            VideoDevicesCombo.SelectedIndex = 0;
            SelectedDevice = VideoDevicesCombo.Text;
            LoadVideoFormats(m_VideoDevices[SelectedDevice]);
        }
        else
        {
            SelectedDevice = Vss.SelectedDeviceName;
            int index = VideoDevicesCombo.FindString(SelectedDevice);
            if (index < 0)
                // The previously selected device device was not found, default to the first device
                index = 0;

            VideoDevicesCombo.SelectedIndex = index;
            SelectedDevice = VideoDevicesCombo.Text;
            LoadVideoFormats(m_VideoDevices[SelectedDevice]);
            SelectVideoFormat(Vss.DeviceFormat, m_VideoDevices[SelectedDevice]);
        }
    }

    private void SelectVideoFormat(VideoDeviceFormat format, List<VideoDeviceFormat> formats)
    {
        int foundIndex = -1;
        for (int i = 0; i < formats.Count; i++)
        {
            if (formats[i].SubType == format.SubType && formats[i].Width == format.Width &&
                formats[i].Height == format.Height && formats[i].Framerate == format.Framerate)
            {
                foundIndex = i;
                break;
            }
        }

        if (foundIndex >= 0)
            VideoListView.Items[foundIndex].Checked = true;
        else
        {   // Nothing found, pick the first format
            if (formats.Count > 0)
                VideoListView.Items[0].Checked = true;
        }
    }

    private void LoadVideoFormats(List<VideoDeviceFormat> formats)
    {
        VideoListView.Items.Clear();
        foreach (VideoDeviceFormat format in formats)
        {
            ListViewItem Lvi = new ListViewItem(format.SubType);
            Lvi.SubItems.Add(format.Width.ToString());
            Lvi.SubItems.Add(format.Height.ToString());
            Lvi.SubItems.Add(format.Framerate.ToString());
            Lvi.Tag = format;
            VideoListView.Items.Add(Lvi);
        }
    }

    private void SetIpAddress(List<IPAddress> ips, ComboBox combo, string? setting)
    {
        if (ips.Count == 0)
            return;

        foreach (IPAddress ip in ips)
        {
            combo.Items.Add(ip.ToString());
        }

        if (string.IsNullOrEmpty(setting) == false)
        {
            int index = combo.FindString(setting);
            if (index >= 0)
                combo.SelectedIndex = index;
            else
                combo.SelectedIndex = 0;
        }
        else
            combo.SelectedIndex = 0;
    }

    private void SipRecBtn_Click(object sender, EventArgs e)
    {
        SipRecSettings Srs = (SipRecSettings)Utils.CopyObject(m_AppSettings.SipRec);
        SipRecForm Srf = new SipRecForm(Srs);
        DialogResult result = Srf.ShowDialog();
        if (result == DialogResult.OK)
        {
            m_AppSettings.SipRec = Srs;
            SetSipRecSummary();
        }
    }

    private void EventLoggingBtn_Click(object sender, EventArgs e)
    {
        EventLoggingSettings EventLoggingSettings = (EventLoggingSettings)Utils.CopyObject(
            m_AppSettings.EventLogging);
        EventLoggingForm Elf = new EventLoggingForm(EventLoggingSettings);
        DialogResult result = Elf.ShowDialog();
        if (result == DialogResult.OK)
        {
            m_AppSettings.EventLogging = EventLoggingSettings;
            SetEventLoggingSummary();
        }
    }

    private void RestoreIdentityBtn_Click(object sender, EventArgs e)
    {
        AgencyIDTb.Text = IdentitySettings.AgencyIDDefault;
        AgentIDTb.Text = IdentitySettings.AgentIDDefault;
        ElementIDTb.Text = IdentitySettings.ElementIDDefault;
    }

    private void CertFileBrowseBtn_Click(object sender, EventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "PFX files (*.pfx)|*.pfx";
        DialogResult result = ofd.ShowDialog();
        if (result == DialogResult.OK)
            CertFileTb.Text = ofd.FileName;
    }

    private void HoldAudioSelectBtn_Click(object sender, EventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "WAV files (*.wav)|*.wav";
        DialogResult result = ofd.ShowDialog();
        if (result == DialogResult.OK)
            HoldAudioTb.Text = ofd.FileName;
    }

    private void HoldVideoSelectBtn_Click(object sender, EventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "JPG files (*.jpg)|*.jpg|JPEG files(*.jpeg)|*.jpeg";
        DialogResult result = ofd.ShowDialog();
        if (result == DialogResult.OK)
            HoldAudioTb.Text = ofd.FileName;
    }

    private void AutoAnswerAudioSelectBtn_Click(object sender, EventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "WAV files (*.wav)|*.wav";
        DialogResult result = ofd.ShowDialog();
        if (result == DialogResult.OK)
            AutoAnswerAudioTb.Text = ofd.FileName;
    }

    private void AutoAnswerVideoSelectBtn_Click(object sender, EventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "JPG files (*.jpg)|*.jpg|JPEG files(*.jpeg)|*.jpeg";
        DialogResult result = ofd.ShowDialog();
        if (result == DialogResult.OK)
            AutoAnswerVideoTb.Text = ofd.FileName;
    }

    private void RestoreCallHoldDefaultsBtn_Click(object sender, EventArgs e)
    {
        CallHoldCombo.SelectedIndex = (int)CallHoldAudioSource.CallHoldRecording;
        HoldAudioTb.Text = CallHandlingSettings.DefaultCallHoldAudioFile;
        HoldVideoTb.Text = CallHandlingSettings.DefaultCallHoldVideoFile;
        HoldTextTb.Text = CallHandlingSettings.DefaultCallHoldTextMessage;
        HoldTextRepeatTb.Text = CallHandlingSettings.DefaultCallHoldTextRepeatInterval.ToString();
    }

    private void RestoreAutoAnswerDefaultsBtn_Click(object sender, EventArgs e)
    {
        AutoAnswerAudioTb.Text = CallHandlingSettings.DefaultAutoAnswerAudioFile;
        AutoAnswerVideoTb.Text = CallHandlingSettings.DefaultAutoAnswerVideoFile;
        AutoAnswerTextTb.Text = CallHandlingSettings.DefaultAutoAnswerTextMessage;
        AutoAnswerTextRepeatTb.Text = CallHandlingSettings.DefaultAutoAnswerTextRepeatInterval.ToString();
    }

    private void VideoDevicesCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (m_IsLoading == true)
            return;

        string SelectedDevice = VideoDevicesCombo.Text;
        LoadVideoFormats(m_VideoDevices![SelectedDevice]);
        VideoListView.Items[0].Checked = true;       // Just pick the first available format
    }

    /// <summary>
    /// Fired just before the checked state of an item actually changes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void VideoListView_ItemCheck(object sender, ItemCheckEventArgs e)
    {
        if (m_IsLoading == true)
            return;

        if (e.NewValue == CheckState.Checked)
        {   // Un-check any currently checked items
            foreach (ListViewItem checkedItem in VideoListView.CheckedItems)
                checkedItem.Checked = false;
        }
    }

    private void ConfSettingsBtn_Click(object sender, EventArgs e)
    {
        TransferSettingsForm Tsf = new TransferSettingsForm();
        Tsf.ShowDialog();
    }
}
