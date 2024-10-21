/////////////////////////////////////////////////////////////////////////////////////
//  File:   SipRecRecorderSettingsForm.cs                           9 Feb 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

using System.Net;
using SipLib.Core;
using SipLib.Media;
using SipLib.SipRec;

namespace PsapSimulator;

/// <summary>
/// Settings form for a single SIPREC media recorder
/// </summary>
public partial class SipRecRecorderSettingsForm : Form
{
    private SipRecRecorderSettings m_Settings;
    private bool m_IsForEdit;
    private List<SipRecRecorderSettings> m_RecordersList;

    public SipRecRecorderSettingsForm(SipRecRecorderSettings settings, bool IsForEdit,
        List<SipRecRecorderSettings> currentRecorders)
    {
        m_Settings = settings;
        m_IsForEdit = IsForEdit;
        m_RecordersList = currentRecorders;
        InitializeComponent();
    }

    private void SaveBtn_Click(object sender, EventArgs e)
    {
        if (ValidateSettings() == false)
            return;

        m_Settings.Name = NameTb.Text;
        m_Settings.Enabled = EnabledCb.Checked;
        m_Settings.SrsIpEndpoint = IPEndpointTb.Text;
        m_Settings.LocalIpEndpoint = LocalIpEndpointTb.Text;
        m_Settings.SipTransportProtocol = (SIPProtocolsEnum) (SipTransportCombo.SelectedIndex  + 1);
        m_Settings.RtpEncryption = (RtpEncryptionEnum)RtpEncryptionCombo.SelectedIndex;
        m_Settings.MsrpEncryption = (MsrpEncryptionEnum) MsrpEncryptionCombo.SelectedIndex;
        m_Settings.Enabled = EnabledCb.Checked;
        m_Settings.OptionsIntervalSeconds = int.Parse(OptionsIntervalTb.Text);

        DialogResult = DialogResult.OK;
        Close();
    }

    private bool ValidateSettings()
    {
        if (string.IsNullOrEmpty(NameTb.Text) == true)
        {
            MessageBox.Show("The Name must be set", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            NameTb.Focus();
            return false;
        }

        if (m_IsForEdit == false)
        {   // Adding a new recorder so make sure that the name is unique
            bool IsUnique = true;
            foreach (SipRecRecorderSettings recorder in m_RecordersList)
            {
                if (recorder.Name == NameTb.Text)
                {
                    IsUnique = false;
                    break;
                }
            }

            if (IsUnique == false)
            {
                MessageBox.Show("The Name must be unique", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                NameTb.Focus();
                return false;
            }
        }

        IPEndPoint? SrsIpe;
        if (string.IsNullOrEmpty(IPEndpointTb.Text) == true)
        {
            MessageBox.Show("The SRS IP Endpoint must be set", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            IPEndpointTb.Focus();
            return false;
        }

        if (IPEndPoint.TryParse(IPEndpointTb.Text, out SrsIpe) == false)
        {
            MessageBox.Show("The SRS IP Endpoint must be set to a valid IPv4 or IPv6 endpoint with a " +
                "port number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            IPEndpointTb.Focus();
            return false;
        }

        if (string.IsNullOrEmpty(LocalIpEndpointTb.Text) == true)
        {
            MessageBox.Show("The Local IP Endpoint must be set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            LocalIpEndpointTb.Focus();
            return false;
        }

        IPEndPoint? LocalIpe;
        if (IPEndPoint.TryParse(LocalIpEndpointTb.Text, out LocalIpe) == false)
        {
            MessageBox.Show("The Local IP Endpoint must be set to a valid IPv4 or IPv6 endpoint with a " +
                "port number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            LocalIpEndpointTb.Focus();
            return false;
        }

        if (SrsIpe.ToString() == LocalIpe.ToString())
        {
            MessageBox.Show("The SRS and Local IP Endpoints must be different.", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            LocalIpEndpointTb.Focus();
            return false;
        }

        if (SrsIpe.AddressFamily != LocalIpe.AddressFamily)
        {
            MessageBox.Show("The SRS and Local IP Endpoint addresses must be the same address types. " +
                "Both must be IPv4 or IPv6", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            LocalIpEndpointTb.Focus();
            return false;
        }

        if (string.IsNullOrEmpty(OptionsIntervalTb.Text) == true)
        {
            MessageBox.Show("The OPTIONS Interval must be specified.", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            OptionsIntervalTb.Focus();
            return false;
        }

        if (int.TryParse(OptionsIntervalTb.Text, out int Interval) == false || (Interval < 5 || Interval > 3600))
        {
            MessageBox.Show("The OPTIONS Interval must be an integer between 5 and 3600 seconds", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            OptionsIntervalTb.Focus();
            return false;
        }

        return true;
    }

    private void CancelBtn_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void SipRecRecorderSettingsForm_Load(object sender, EventArgs e)
    {
        NameTb.Text = m_Settings.Name;
        EnabledCb.Checked = m_Settings.Enabled;
        IPEndpointTb.Text = m_Settings.SrsIpEndpoint;
        LocalIpEndpointTb.Text = m_Settings.LocalIpEndpoint;
        SipTransportCombo.SelectedIndex = (int)m_Settings.SipTransportProtocol - 1;
        RtpEncryptionCombo.SelectedIndex = (int)m_Settings.RtpEncryption;
        MsrpEncryptionCombo.SelectedIndex = (int)m_Settings.MsrpEncryption;
        OptionsCheck.Checked = m_Settings.EnableOptions;
        OptionsIntervalTb.Text = m_Settings.OptionsIntervalSeconds.ToString();
        NameTb.ReadOnly = m_IsForEdit;
    }
}
