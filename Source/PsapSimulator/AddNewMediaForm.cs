/////////////////////////////////////////////////////////////////////////////////////
//  File:   AddNewMediaForm.cs                                      5 Nov 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PsapSimulator;

/// <summary>
/// Form class that allows the user to select new media to add to a call.
/// </summary>
public partial class AddNewMediaForm : Form
{
    private List<string> m_AvailableMedia;

    public AddNewMediaForm(List<string> availableMedia)
    {
        InitializeComponent();
        m_AvailableMedia = availableMedia;
    }

    private void CancelBtn_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
    }

    private List<string> m_SelectedMediaType = new List<string>();

    private void OkBtn_Click(object sender, EventArgs e)
    {
       if (checkedListBox1.CheckedItems.Count == 0)
       {
            DialogResult result = MessageBox.Show("No new media types selected. Do you wish to continue", "Error",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                return;
            else
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
       }

       foreach (object item in checkedListBox1.SelectedItems)
       {
            if (item != null)
                m_SelectedMediaType.Add(item.ToString()!);
       }

       DialogResult = DialogResult.OK;
    }

    /// <summary>
    /// Gets the list of selected media types to add to a call. Each item is the display type for the media
    /// (Audio, Video, RTT or MSRP).
    /// </summary>
    /// <returns></returns>
    public List<string> GetSelectedMedia()
    {
        return m_SelectedMediaType;
    }

    private void AddNewMediaForm_Load(object sender, EventArgs e)
    {
        foreach (string strMedia in m_AvailableMedia)
        {
            checkedListBox1.Items.Add(strMedia);
        }
    }
}
