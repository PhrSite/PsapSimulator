/////////////////////////////////////////////////////////////////////////////////////
//  File:   SipRecRecorderSettingsForm.cs                           9 Feb 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

using PsapSimulator.Settings;
using System.Net;
using SipLib.Core;

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
        m_Settings.IpEndpoint = IPEndpointTb.Text;
        m_Settings.SipTransportProtocol = (SIPProtocolsEnum) (SipTransportCombo.SelectedIndex  + 1);
        m_Settings.RtpEncryption = (RtpEncryptionEnum)RtpEncryptionCombo.SelectedIndex;
        m_Settings.MsrpEncryption = (MsrpEncryptionEnum) MsrpEncryptionCombo.SelectedIndex;

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

        IPEndPoint? Ipe;
        if (string.IsNullOrEmpty(IPEndpointTb.Text) == true)
        {
            MessageBox.Show("The IP Endpoint must be set", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            IPEndpointTb.Focus();
            return false;
        }

        if (IPEndPoint.TryParse(IPEndpointTb.Text, out Ipe) == false)
        {
            MessageBox.Show("The IP Endpoint must be set to a valid IPv4 or IPv6 endpoint with a " +
                "port number.", "Error", MessageBoxButtons.OK, 
                MessageBoxIcon.Error);
            IPEndpointTb.Focus();
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
        IPEndpointTb.Text = m_Settings.IpEndpoint;
        SipTransportCombo.SelectedIndex = (int)m_Settings.SipTransportProtocol - 1;
        RtpEncryptionCombo.SelectedIndex = (int)m_Settings.RtpEncryption;
        MsrpEncryptionCombo.SelectedIndex = (int)m_Settings.MsrpEncryption;

        NameTb.ReadOnly = m_IsForEdit;
    }
}
