/////////////////////////////////////////////////////////////////////////////////////
//  File:   MsrpChannelData.cs                                      9 Nov 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

using SipLib.Msrp;
using Ng911Lib.Utilities;

namespace SrsSimulator;

/// <summary>
/// Class for recording a single stream of Message Session Relay Protocol (MSRP) messages received on a
/// MsrpConnection from the SRS. This class only handles text messages.
/// <para>
/// This class saves all MSRP messages to a JSON file when the SIPREC call ends. The file extension is *.msrp.
/// The TextFileFormat class defines the JSON schema.
/// </para>
/// </summary>
internal class MsrpChannelData
{
    public string Name;
    private MsrpConnection m_MsrpConnection;
    private string m_MsrpFilePath;
    private TextFileFormat m_TextFileFormat;

    /// <summary>
    /// Constructor. Hooks the events of the MsrpConnection and starts it.
    /// </summary>
    /// <param name="name">String that identifies the call participant that is sending the media for the RTP channel being recorded.
    /// This string is used to name the MSRP recording file.</param>
    /// <param name="msrpConnection">MsrpConnection object that is used to receive MSRP messages from the
    /// SRC.</param>
    /// <param name="filePath">Directory in which to save the MSRP file. Must already exist.</param>
    public MsrpChannelData(string name, MsrpConnection msrpConnection, string filePath)
    {
        Name = name;
        m_MsrpConnection = msrpConnection;
        m_MsrpFilePath = Path.Combine(filePath, name + ".msrp");
        m_TextFileFormat = new TextFileFormat();

        m_MsrpConnection.MsrpTextMessageReceived += OnMsrpTextMessageReceived;
        m_MsrpConnection.Start();
    }

    private void OnMsrpTextMessageReceived(string message, string from)
    {
        if (string.IsNullOrEmpty(message) == true)
            return;

        m_TextFileFormat.TextLines.Add(new TextLine(from, message.Replace("\r", "").Replace("\n", "")));
    }

    /// <summary>
    /// This method must be called when the SIPREC call has ended. Shuts down the MsrpConnection and saves all
    /// messages to a *.msrp file.
    /// </summary>
    public void StopRecording()
    {
        m_MsrpConnection.MsrpTextMessageReceived -= OnMsrpTextMessageReceived;
        m_MsrpConnection.Shutdown();

        string strJson = JsonHelper.SerializeToString(m_TextFileFormat);
        File.WriteAllText(m_MsrpFilePath, strJson);
    }
}
