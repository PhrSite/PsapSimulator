/////////////////////////////////////////////////////////////////////////////////////
//  File:   PsapStatesFormc.cs                                      8 Jul 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator;
using CallManagement;
using I3SubNot;

/// <summary>
/// Windows Form class that allows the user to set the Element State, Service State and Queue State for the PSAP.
/// </summary>
public partial class PsapStatesForm : Form
{
    CallManager m_CallManager;

    public PsapStatesForm(CallManager callManager)
    {
        InitializeComponent();
        m_CallManager = callManager;
    }

    private void CloseBtn_Click(object sender, EventArgs e)
    {
        Close();
    }

    private bool m_IsLoading = true;
    private void PsapStatesForm_Load(object sender, EventArgs e)
    {
        LoadComboBoxValues(ElementStateCombo, ElementState.ElementStateValues, m_CallManager.CurrentElementState);
        LoadComboBoxValues(ServiceStateCombo, ServiceStateType.ServiceStateValues, m_CallManager.CurrentServiceState);
        LoadComboBoxValues(SecurityPostureCombo, SecurityPostureType.SecurityPostureValues, m_CallManager.CurrentSecurityPosture);
        LoadComboBoxValues(QueueStateCombo, QueueState.QueueStateValues, m_CallManager.CurrentQueueState);
        m_IsLoading = false;
    }

    private void LoadComboBoxValues(ComboBox combo, string[] values, string currentSetting)
    {
        foreach (string value in values)
        {
            combo.Items.Add(value);
        }

        // Select the current setting
        int index = combo.FindStringExact(currentSetting);
        if (index >= 0)
            combo.SelectedIndex = index;
        else
            combo.SelectedIndex = 0;
    }

    private void ElementStateCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (m_IsLoading == true)
            return;
    }

    private void ServiceStateCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (m_IsLoading == true)
            return;
    }

    private void SecurityPostureCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (m_IsLoading == true)
            return;
    }

    private void QueueStateCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (m_IsLoading == true)
            return;

    }

    private void NotifyBtn_Click(object sender, EventArgs e)
    {
        m_CallManager.CurrentElementState = ElementStateCombo.Text;
        m_CallManager.CurrentServiceState = ServiceStateCombo.Text;
        m_CallManager.CurrentSecurityPosture = SecurityPostureCombo.Text;
        m_CallManager.CurrentQueueState = QueueStateCombo.Text;

        m_CallManager.NotifyStateSubscribers();
    }
}
