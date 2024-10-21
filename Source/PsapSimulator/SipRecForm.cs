/////////////////////////////////////////////////////////////////////////////////////
//  File:   SipRecForm.cs                                           9 Feb 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator;
using PsapSimulator.Settings;
using SipLib.Core;
using SipLib.SipRec;

/// <summary>
/// SIPREC settings form
/// </summary>
public partial class SipRecForm : Form
{
    private SipRecSettings m_Settings;

    public SipRecForm(SipRecSettings sipRecSettings)
    {
        m_Settings = sipRecSettings;
        InitializeComponent();
    }

    private void CancelBtn_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void SaveBtn_Click(object sender, EventArgs e)
    {
        m_Settings.EnableSipRec = EnableSipRecCheck.Checked;
        m_Settings.SipRecRecorders.Clear();
        if (RecListView.Items.Count > 0 )
        {
            for (int i = 0; i < RecListView.Items.Count; i++ )
                m_Settings.SipRecRecorders.Add((SipRecRecorderSettings)RecListView.Items[i].Tag!);
        }

        DialogResult= DialogResult.OK;
        Close();
    }

    private void SipRecForm_Load(object sender, EventArgs e)
    {
        EnableSipRecCheck.Checked = m_Settings.EnableSipRec;
        LoadListBox();
    }

    private void LoadListBox()
    {
        RecListView.Items.Clear();
        foreach (SipRecRecorderSettings recSettings in m_Settings.SipRecRecorders)
        {
            ListViewItem Lvi = new ListViewItem(recSettings.Name);
            Lvi.SubItems.Add(recSettings.Enabled.ToString());
            Lvi.SubItems.Add(recSettings.SrsIpEndpoint.ToString());
            Lvi.SubItems.Add(GetSipProtocolString(recSettings.SipTransportProtocol));
            Lvi.Tag = recSettings;
            RecListView.Items.Add(Lvi);
        }
    }

    private string GetSipProtocolString(SIPProtocolsEnum protocol)
    {
        string proto = "UDP";
        switch (protocol)
        {
            case SIPProtocolsEnum.udp:
                proto = "UDP";
                break;
            case SIPProtocolsEnum.tcp:
                proto = "TCP";
                break;
            case SIPProtocolsEnum.tls:
                proto = "TLS";
                break;
        }

        return proto;
    }

    private void AddBtn_Click(object sender, EventArgs e)
    {
        SipRecRecorderSettings NewRecorder = new SipRecRecorderSettings();
        SipRecRecorderSettingsForm SrsForm = new SipRecRecorderSettingsForm(NewRecorder, false,
            m_Settings.SipRecRecorders);
        DialogResult result = SrsForm.ShowDialog();
        if (result == DialogResult.OK)
        {
            m_Settings.SipRecRecorders.Add(NewRecorder);
            LoadListBox();
        }
    }

    private int GetSelectedItemIndex()
    {
        if (RecListView.SelectedIndices.Count == 0)
        {
            MessageBox.Show("Please select a SIPREC recorder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return -1;
        }

        return RecListView.SelectedIndices[0];
    }

    private void EditBtn_Click(object sender, EventArgs e)
    {
        int index = GetSelectedItemIndex();
        if (index == -1)
            return;

        SipRecRecorderSettings? recSettings = RecListView.Items[index].Tag as SipRecRecorderSettings;
        SipRecRecorderSettings temp = (SipRecRecorderSettings) Utils.CopyObject(recSettings!);
        SipRecRecorderSettingsForm SrsForm = new SipRecRecorderSettingsForm(temp, true,
            m_Settings.SipRecRecorders);
        DialogResult result = SrsForm.ShowDialog();
        if (result == DialogResult.OK)
        {
            RecListView.Items[index].Tag = temp;
            RecListView.Items[index].SubItems[1].Text = temp.Enabled.ToString();
            RecListView.Items[index].SubItems[2].Text = temp.SrsIpEndpoint.ToString();
            RecListView.Items[index].SubItems[3].Text = GetSipProtocolString(temp.SipTransportProtocol);
        }
    }

    private void DeleteBtn_Click(object sender, EventArgs e)
    {
        int index = GetSelectedItemIndex();
        if (index == -1) return;

        DialogResult result = MessageBox.Show("Are you sure you want to delete the selected " +
            "SIPREC recorder?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (result == DialogResult.Yes)
            RecListView.Items.RemoveAt(index);
    }

    private void RecListView_DoubleClick(object sender, EventArgs e)
    {
        EditBtn_Click(sender, e);
    }
}
