/////////////////////////////////////////////////////////////////////////////////////
//  File:   EventLoggerForm.cs                                      10 Feb 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

using PsapSimulator.Settings;
using SipLib.Core;

namespace PsapSimulator;

/// <summary>
/// Form class for the settings for a single NG9-1-1 event logger.
/// </summary>
public partial class EventLoggerForm : Form
{
    private EventLoggerSettings m_Settings;
    private bool m_IsForEdit;
    private List<EventLoggerSettings> m_Loggers;

    public EventLoggerForm(EventLoggerSettings loggerSettings, bool isForEdit, List<EventLoggerSettings> loggers)
    {
        m_Settings = loggerSettings;
        m_IsForEdit = isForEdit;
        m_Loggers = loggers;
        InitializeComponent();
    }

    private void OkBtn_Click(object sender, EventArgs e)
    {
        if (ValidateSettings() == false)
            return;

        m_Settings.Name = NameTb.Text;
        m_Settings.Enabled = EnabledCb.Checked;
        m_Settings.LoggerUri = LoggerUriTb.Text;
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
            foreach (EventLoggerSettings logger in m_Loggers)
            {
                if (logger.Name == NameTb.Text)
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

        if (string.IsNullOrEmpty(LoggerUriTb.Text) == true)
        {
            MessageBox.Show("The Logger URI must be unique", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            LoggerUriTb.Focus();
            return false;
        }

        SIPURI? sipUri = null;
        if (SIPURI.TryParse(LoggerUriTb.Text, out sipUri) == false)
        {
            MessageBox.Show("The Logger URI is not valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            LoggerUriTb.Focus();
            return false;
        }
        else
        {
            if (sipUri!.Scheme != SIPSchemesEnum.http && sipUri.Scheme != SIPSchemesEnum.https)
            {
                MessageBox.Show("The Logger URI must be a HTTP or a HTTPS URI", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggerUriTb.Focus();
                return false;
            }
        }

        return true;
    }

    private void CancelBtn_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void EventLoggerForm_Load(object sender, EventArgs e)
    {
        NameTb.Text = m_Settings.Name;
        EnabledCb.Checked = m_Settings.Enabled;
        LoggerUriTb.Text = m_Settings.LoggerUri;

        if (m_IsForEdit == true)
            NameTb.ReadOnly = true;
    }

    private void HelpBtn_Click(object sender, EventArgs e)
    {
        HelpUtils.ShowHelpTopic(HelpUtils.EVENT_LOGGER_SETTINGS_HELP_URI);
    }
}
