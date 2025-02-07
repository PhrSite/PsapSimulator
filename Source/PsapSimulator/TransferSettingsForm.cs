/////////////////////////////////////////////////////////////////////////////////////
//  File:   TransferSettingsForm.cs                                 22 Jan 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator;
using PsapSimulator.Settings;

/// <summary>
/// Form for changing the Conference/Transfer settings. The settings are stored in a different
/// file than the PsapSimulator settings. This form reads the settings from the Conference/Transfer
/// settings and saves the modified settings to that file.
/// </summary>
public partial class TransferSettingsForm : Form
{
    private TransferSettings m_TransferSettings;

    /// <summary>
    /// Constructor.
    /// </summary>
    public TransferSettingsForm()
    {
        m_TransferSettings = TransferSettings.GetTransferSettings();
        InitializeComponent();
    }

    private void TransferSettingsForm_Load(object sender, EventArgs e)
    {
        DisplaySettings();
    }

    private void DisplaySettings()
    {
        TargetsListView.Items.Clear();
        foreach (TransferTarget target in m_TransferSettings.Targets)
        {
            ListViewItem Lvi = new ListViewItem(target.Name);
            Lvi.SubItems.Add(target.SipUri);
            Lvi.Tag = target;
            TargetsListView.Items.Add(Lvi);
        }
    }

    private void CloseBtn_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private bool GetSettings()
    {
        // TODO: Check the settngs for errors

        // TODO: Get the settings into m_TransferSettings

        return true;
    }

    private void OkBtn_Click(object sender, EventArgs e)
    {
        if (GetSettings() == false)
            return;
        
        TransferSettings.SaveTransferSettingsToFile(m_TransferSettings);
        DialogResult = DialogResult.OK;
        Close();
    }

    private void TargetsListView_DoubleClick(object sender, EventArgs e)
    {
        TransferTarget? selectedTarget = GetSelectedTarget();
        if (selectedTarget == null)
            return;

        AddEditTransferTarget form = new AddEditTransferTarget(m_TransferSettings.Targets, selectedTarget);
        DialogResult result = form.ShowDialog();
        if (result == DialogResult.OK)
        {
            TransferSettings.SaveTransferSettingsToFile(m_TransferSettings);
            DisplaySettings();
        }
    }

    private void AddBtn_Click(object sender, EventArgs e)
    {
        AddEditTransferTarget form = new AddEditTransferTarget(m_TransferSettings.Targets, null);
        DialogResult result = form.ShowDialog();
        if (result == DialogResult.OK)
        {
            TransferSettings.SaveTransferSettingsToFile(m_TransferSettings);
            DisplaySettings();
        }
    }

    private TransferTarget? GetSelectedTarget()
    {
        if (TargetsListView.SelectedItems.Count == 0)
            return null;
        else
        {
            ListViewItem item = TargetsListView.SelectedItems[0];
            return item.Tag as TransferTarget;
        }
    }

    private void EditBtn_Click(object sender, EventArgs e)
    {
        TransferTarget? target = GetSelectedTarget();
        if (target == null)
        {
            MessageBox.Show("Please select a Transfer Target to edit", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        AddEditTransferTarget form = new AddEditTransferTarget(m_TransferSettings.Targets, target);
        DialogResult result = form.ShowDialog();
        if (result == DialogResult.OK)
        {
            TransferSettings.SaveTransferSettingsToFile(m_TransferSettings);
            DisplaySettings();
        }
    }

    private void DeleteBtn_Click(object sender, EventArgs e)
    {
        TransferTarget? target = GetSelectedTarget();
        if (target == null)
        {
            MessageBox.Show("Please select a Transfer Target to delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        m_TransferSettings.Targets.Remove(target);
        TransferSettings.SaveTransferSettingsToFile(m_TransferSettings);
        DisplaySettings();
    }

}
