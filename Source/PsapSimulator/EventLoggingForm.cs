/////////////////////////////////////////////////////////////////////////////////////
//  File:   EventLoggingForm.cs                                     9 Feb 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

using PsapSimulator.Settings;

namespace PsapSimulator;

/// <summary>
/// Form class for the NG9-1-1 Event Logging settings
/// </summary>
public partial class EventLoggingForm : Form
{
    private EventLoggingSettings m_Settings;

    public EventLoggingForm(EventLoggingSettings settings)
    {
        m_Settings = settings;
        InitializeComponent();
    }

    private void EventLoggingForm_Load(object sender, EventArgs e)
    {
        EnableLoggingCheck.Checked = m_Settings.EnableLogging;
        LoadListView();
    }

    private void LoadListView()
    {
        LoggerListView.Items.Clear();
        foreach (EventLoggerSettings loggerSettings in m_Settings.Loggers)
        {
            ListViewItem Lvi = new ListViewItem(loggerSettings.Name);
            Lvi.SubItems.Add(loggerSettings.Enabled.ToString());
            Lvi.SubItems.Add(loggerSettings.LoggerUri);
            Lvi.Tag = loggerSettings;
            LoggerListView.Items.Add(Lvi);
        }
    }

    private void SaveBtn_Click(object sender, EventArgs e)
    {
        m_Settings.EnableLogging = EnableLoggingCheck.Checked;
        m_Settings.Loggers.Clear();
        for (int i = 0; i < LoggerListView.Items.Count; i++)
            m_Settings.Loggers.Add((EventLoggerSettings)LoggerListView.Items[i].Tag!);

        DialogResult = DialogResult.OK;
        Close();
    }

    private void CancelBtn_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void AddBtn_Click(object sender, EventArgs e)
    {
        EventLoggerSettings NewLogger = new EventLoggerSettings();
        EventLoggerForm Elf = new EventLoggerForm(NewLogger, false, m_Settings.Loggers);
        DialogResult Result = Elf.ShowDialog();
        if (Result == DialogResult.OK)
        {
            m_Settings.Loggers.Add(NewLogger);
            LoadListView();
        }
    }

    private int GetSelectedItemIndex()
    {
        if (LoggerListView.SelectedIndices.Count == 0)
        {
            MessageBox.Show("Please select an event logger", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return -1;
        }

        return LoggerListView.SelectedIndices[0];
    }

    private void EditBtn_Click(object sender, EventArgs e)
    {
        int index = GetSelectedItemIndex();
        if (index == -1)
            return;

        EventLoggerSettings? logger = LoggerListView.Items[index].Tag as EventLoggerSettings;
        EventLoggerSettings temp = (EventLoggerSettings)Utils.CopyObject(logger!);
        EventLoggerForm Elf = new EventLoggerForm(temp, true, m_Settings.Loggers);
        DialogResult Result = Elf.ShowDialog();
        if (Result == DialogResult.OK)
        {
            LoggerListView.Items[index].Tag = temp;
            LoggerListView.Items[index].SubItems[1].Text = temp.Enabled.ToString();
            LoggerListView.Items[index].SubItems[2].Text = temp.LoggerUri;
        }
    }

    private void DeleteBtn_Click(object sender, EventArgs e)
    {
        int index = GetSelectedItemIndex();
        if (index == -1) return;

        DialogResult result = MessageBox.Show("Are you sure you want to delete the selected " +
            "event logger?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (result == DialogResult.Yes)
            LoggerListView.Items.RemoveAt(index);
    }

    private void LoggerListView_DoubleClick(object sender, EventArgs e)
    {
        EditBtn_Click(sender, e);
    }
}
