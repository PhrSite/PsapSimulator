/////////////////////////////////////////////////////////////////////////////////////
//  File:   AddEditTransferTarget.cs                                22 Jan 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator;
using PsapSimulator.Settings;
using SipLib.Core;

/// <summary>
/// Windows Forms class for adding a new transfer target or editing an existing one.
/// </summary>
public partial class AddEditTransferTarget : Form
{
    private List<TransferTarget> m_TransferTargets;
    private TransferTarget? m_TransferTargetToEdit;

    public AddEditTransferTarget(List<TransferTarget> CurrentTransferTargets, TransferTarget? TransferTargetToEdit)
    {
        m_TransferTargets = CurrentTransferTargets;
        m_TransferTargetToEdit = TransferTargetToEdit;
        InitializeComponent();
    }

    private void AddEditTransferTarget_Load(object sender, EventArgs e)
    {
        if (m_TransferTargetToEdit != null)
        {
            Text = "Edit a Transfer Target";
            NameTb.ReadOnly = true;
            LoadSettings();
        }
        else
        {
            Text = "Add a New Transfer Target";
        }
    }

    private void LoadSettings()
    {
        if (m_TransferTargetToEdit == null)
            return;

        NameTb.Text = m_TransferTargetToEdit.Name;
        SipUriTb.Text = m_TransferTargetToEdit.SipUri;
    }

    private bool GetSettings()
    {
        if (string.IsNullOrEmpty(NameTb.Text) == true)
        {
            MessageBox.Show("The Display Name must be specified.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            NameTb.Focus();
            return false;
        }

        if (m_TransferTargetToEdit == null)
        {   // Adding a new TransferTarget so the Name must be unique.
            if (TransferTargetNameIsUnique(NameTb.Text) == false)
            {
                MessageBox.Show("The Display Name must be unique.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                NameTb.Focus();
                return false;
            }
        }

        SIPURI? sipUri = null;
        if (SIPURI.TryParse(SipUriTb.Text, out sipUri) == false)
        {
            MessageBox.Show("The SIP URI is invalid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            SipUriTb.Focus();
            return false;
        }

        if (m_TransferTargetToEdit != null)
            m_TransferTargetToEdit.SipUri = SipUriTb.Text;
        else
        {
            TransferTarget target = new TransferTarget() { Name = NameTb.Text, SipUri = SipUriTb.Text };
            m_TransferTargets!.Add(target);
        }

        return true;
    }

    private bool TransferTargetNameIsUnique(string name)
    {
        foreach (TransferTarget target in m_TransferTargets)
        {
            if (target.Name == name)
                return false;
        }

        return true;
    }

    private void OkBtn_Click(object sender, EventArgs e)
    {
        if (GetSettings() == false)
            return;

        DialogResult = DialogResult.OK;
        Close();
    }

    private void CancelBtn_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
