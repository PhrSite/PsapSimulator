/////////////////////////////////////////////////////////////////////////////////////
//  File:   RttChannelData.cs                                       7 Nov 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

using SipLib.Rtp;
using SipLib.Sdp;
using SipLib.RealTimeText;
using Ng911Lib.Utilities;
using SipLib.Logging;

namespace SrsSimulator;

/// <summary>
/// Class for recording a single stream of Real Time Text (RTT) characters received on a RtpChannel from the SRC.
/// <para>
/// This class saves all RTT characters to a JSON file when the SIPREC call ends. The file extension is *.rtt.
/// The TextFileFormat class defines the JSON schema.
/// </para>
/// </summary>
internal class RttChannelData : RtpRecordingChannelData
{
    private RttReceiver? m_rttReceiver = null;
    private string m_RttFilePath;
    private TextFileFormat m_TextFileFormat;

    /// <summary>
    /// Constructor. Hooks the events of the RtpChannel and calls its StartListening() method.
    /// </summary>
    /// <param name="name">String that identifies the call participant that is sending the media for the RTP channel being recorded.
    /// This string is used to name the RTT recording file.</param>
    /// <param name="Channel">RtpChannel that is receiving the RTP media from the SRC.</param>
    /// <param name="filePath">Directory in which to save the RTT file. Must already exist.</param>
    /// <param name="answerMediaDescription">MediaDescription object that the SRS sent with its OK response
    /// to the INVITE request from the SRC.</param>
    public RttChannelData(string name, RtpChannel Channel, string filePath, MediaDescription answerMediaDescription) : base(name, Channel)
    {
        m_RttFilePath = Path.Combine(filePath, name + ".rtt");
        m_TextFileFormat = new TextFileFormat();

        RttParameters? rttParameters = RttParameters.FromMediaDescription(answerMediaDescription);
        if (rttParameters == null)
        {
            SipLogger.LogError("Unable to build an RttParameters object from the answered MediaDescription object");
            return;
        }

        m_rttReceiver = new RttReceiver(rttParameters, rtpChannel, name);
        m_rttReceiver.RttCharactersReceived += OnRttCharactersReceived;
        rtpChannel.StartListening();
    }

    private void OnRttCharactersReceived(string RxChars, string Source)
    {
        if (string.IsNullOrEmpty(RxChars) == true)
            return;

        m_TextFileFormat.TextLines.Add(new TextLine(Source, RxChars));
    }

    /// <summary>
    /// This method must be called when the SIPREC call has ended. Shuts down the RtpChannel and saves all
    /// received text to a *.rtt file.
    /// </summary>
    public override void StopRecording()
    {   
        rtpChannel.Shutdown();

        string strJson = JsonHelper.SerializeToString(m_TextFileFormat);
        File.WriteAllText(m_RttFilePath, strJson);
    }
}
