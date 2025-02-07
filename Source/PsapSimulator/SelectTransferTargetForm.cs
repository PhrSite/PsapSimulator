/////////////////////////////////////////////////////////////////////////////////////
//  File:   SelectTransferTargetForm.cs                             22 Jan 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator;
using PsapSimulator.Settings;

/// <summary>
/// Form that allows the user to select a transfer target.
/// </summary>
public partial class SelectTransferTargetForm : Form
{
    private TransferSettings m_TransferSettings;

    /// <summary>
    /// This is the TransferTarget that the user selected.
    /// </summary>
    public TransferTarget? SelectedTransferTarget {  get; private set; }

    /// <summary>
    /// Constructor
    /// </summary>
    public SelectTransferTargetForm()
    {
        m_TransferSettings = TransferSettings.GetTransferSettings();
        InitializeComponent();
    }

    private void SelectTransferTargetForm_Load(object sender, EventArgs e)
    {
        DisplaySettings();
    }

    private void DisplaySettings()
    {
        TargetComboBox.Items.Clear();

        if (m_TransferSettings.Targets.Count == 0)
            return;

        foreach (TransferTarget target in m_TransferSettings.Targets)
        {
            TargetComboBox.Items.Add(target.Name);
        }

        if (TargetComboBox.Items.Count > 0)
        {
            if (string.IsNullOrEmpty(m_TransferSettings.LastUsedTransferTargetName) == false)
            {
                int index = TargetComboBox.FindString(m_TransferSettings.LastUsedTransferTargetName);
                if (index >= 0)
                    TargetComboBox.SelectedIndex = index;
                else
                    TargetComboBox.SelectedIndex = 0;
            }
            else
                TargetComboBox.SelectedIndex = 0;
        }
    }

    private void OkBtn_Click(object sender, EventArgs e)
    {
        if (TargetComboBox.SelectedIndex < 0)
        {
            MessageBox.Show("Please select a Transfer Target first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        string TargetName = TargetComboBox.Text;
        SelectedTransferTarget = GetTransferTargetByName(TargetName);
        if (SelectedTransferTarget == null)
        {
            MessageBox.Show("Unable to find the selected Transfer Target", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
            return;
        }

        m_TransferSettings.LastUsedTransferTargetName = TargetName;
        TransferSettings.SaveTransferSettingsToFile(m_TransferSettings);

        DialogResult = DialogResult.OK;
        Close();
    }

    private TransferTarget? GetTransferTargetByName(string name)
    {
        TransferTarget? target = null;
        foreach (TransferTarget tt in m_TransferSettings.Targets)
        {
            if (tt.Name == name)
                return tt;
        }

        return target;
    }

    private void CancelBtn_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    /// <summary>
    /// The user clicked on the Add button to add a new Transfer Target
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AddBtn_Click(object sender, EventArgs e)
    {
        AddEditTransferTarget form = new AddEditTransferTarget(m_TransferSettings.Targets, null);
        DialogResult Dr = form.ShowDialog();
        if (Dr == DialogResult.OK)
        {
            TransferSettings.SaveTransferSettingsToFile(m_TransferSettings);
            DisplaySettings();
        }
    }
}
