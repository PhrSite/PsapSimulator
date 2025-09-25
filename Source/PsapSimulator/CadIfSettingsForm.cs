/////////////////////////////////////////////////////////////////////////////////////
//  File:   CadIfSettingsForm.cs                                    22 Sep 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator;

using PsapSimulator.CallManagement;
using PsapSimulator.Settings;

/// <summary>
/// Form class for displaying the URLs for the EIDO CAD interface.
/// </summary>
public partial class CadIfSettingsForm : Form
{
    private NetworkSettings m_NetworkSettings;

    public CadIfSettingsForm(AppSettings appSettings)
    {
        InitializeComponent();
        m_NetworkSettings = appSettings.NetworkSettings;
    }

    private void CloseBtn_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void CadIfSettingsForm_Load(object sender, EventArgs e)
    {
        if (m_NetworkSettings.EnableIPv4 == true)
        {
            IPv4UrlLbl.Text = $"wss://{m_NetworkSettings.IPv4Address}:{CallManager.EidoPort}{CallManager.WsEidoPath}";
        }
        else
        {
            IPv4UrlLbl.Text = "Not Enabled";
            CopyIPv4Btn.Visible = false;
        }

        if (m_NetworkSettings.EnableIPv6 == true)
        {
            IPv6UrlLbl.Text = $"wss://[{m_NetworkSettings.IPv6Address}]:{CallManager.EidoPort}{CallManager.WsEidoPath}";
        }
        else
        {
            IPv6UrlLbl.Text = "Not Enabled";
            CopyIPv6Btn.Visible = false;
        }
    }

    private void CopyIPv4Btn_Click(object sender, EventArgs e)
    {
        Clipboard.SetText(IPv4UrlLbl.Text);
    }

    private void CopyIPv6Btn_Click(object sender, EventArgs e)
    {
        Clipboard.SetText(IPv6UrlLbl.Text);
    }

    private void HelpBtn_Click(object sender, EventArgs e)
    {
        HelpUtils.ShowHelpTopic(HelpUtils.CAD_IF_SETTINGS_HELP_URI);
    }
}
