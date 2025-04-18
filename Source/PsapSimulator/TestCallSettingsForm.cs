/////////////////////////////////////////////////////////////////////////////////////
//  File:   TestCallSettingsForm.cs                                 15 Apr 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator;
using SipLib.TestCalls;

/// <summary>
/// Form that allows the user to change the settings for Test Calls.
/// </summary>
public partial class TestCallSettingsForm : Form
{
    private IncomingTestCallSettings m_Settings;

    public TestCallSettingsForm(IncomingTestCallSettings settings)
    {
        m_Settings = settings;
        InitializeComponent();
    }

    private void TestCallSettingsForm_Load(object sender, EventArgs e)
    {
        EnableTestCallsCheck.Checked = m_Settings.Enable;
        MaxTestCallsTb.Text = m_Settings.MaxTestCalls.ToString();
        TestCallDurationUnitsCombo.SelectedIndex = (int)m_Settings.DurationUnits;
        if (m_Settings.DurationUnits == TestCallDurationUnitsEnum.DurationUnitsPackets)
            TestCallDurationTb.Text = m_Settings.DurationPackets.ToString();
        else
            TestCallDurationTb.Text = m_Settings.DurationMinutes.ToString();
    }

    private bool VerifySettings()
    {
        int maxCalls = 0;
        if (int.TryParse(MaxTestCallsTb.Text, out maxCalls) == false)
        {
            MessageBox.Show("The Maximum Test Calls setting must be an integer.", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            MaxTestCallsTb.Focus();
            return false;
        }

        if (maxCalls > 1)
        {
            MessageBox.Show("The Maximum Test Calls setting must greater than or equal to 1.", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            MaxTestCallsTb.Focus();
            return false;
        }

        int duration = 0;
        if (int.TryParse(TestCallDurationTb.Text, out duration) == false)
        {
            MessageBox.Show("The Test Call Duration setting must be an integer.", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            TestCallDurationTb.Focus();
            return false;
        }

        TestCallDurationUnitsEnum units = (TestCallDurationUnitsEnum)TestCallDurationUnitsCombo.SelectedIndex;
        if (units == TestCallDurationUnitsEnum.DurationUnitsPackets)
        { 
            if (duration < 3)
            {
                MessageBox.Show("The Test Call Duration setting must be at least 3 RTP Packets.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                TestCallDurationTb.Focus();
                return false;
            }
        }
        else
        {
            if (duration < 1)
            {
                MessageBox.Show("The Test Call Duration setting must be at least 1 minute.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                TestCallDurationTb.Focus();
                return false;
            }
        }

        return true;
    }

    private void SaveBtn_Click(object sender, EventArgs e)
    {
        if (VerifySettings() == false)
            return;     // Error message already displayed.

        // Get the settings from the display objects
        m_Settings.Enable = EnableTestCallsCheck.Checked;
        m_Settings.MaxTestCalls = int.Parse(MaxTestCallsTb.Text);
        TestCallDurationUnitsEnum units = (TestCallDurationUnitsEnum)TestCallDurationUnitsCombo.SelectedIndex;
        m_Settings.DurationUnits = units;

        if (units == TestCallDurationUnitsEnum.DurationUnitsPackets)
            m_Settings.DurationPackets = int.Parse(TestCallDurationTb.Text);
        else
            m_Settings.DurationMinutes = int.Parse(TestCallDurationTb.Text);

        DialogResult = DialogResult.OK;
    }

    private void CancelBtn_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void TestCallDurationUnitsCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
        TestCallDurationUnitsEnum units = (TestCallDurationUnitsEnum) TestCallDurationUnitsCombo.SelectedIndex;
        if (units == TestCallDurationUnitsEnum.DurationUnitsPackets)
            TestCallDurationTb.Text = m_Settings.DurationPackets.ToString();
        else
            TestCallDurationTb.Text = m_Settings.DurationMinutes.ToString();
    }
}
