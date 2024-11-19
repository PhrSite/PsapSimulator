/////////////////////////////////////////////////////////////////////////////////////
//  File:   RtpRecordingChannelData.cs                              7 Nov 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

using SipLib.Rtp;

namespace SrsSimulator;

/// <summary>
/// Base class for all recording media channels that receive media using an RtpChannel. The derived
/// classes are for audio, RTT and video media only.
/// </summary>
internal class RtpRecordingChannelData
{
    /// <summary>
    /// String that identifies the call participant that is sending the media for the RTP channel being recorded
    /// </summary>
    public string Name;

    /// <summary>
    /// RtpChannel that is receiving the RTP media from the SRC.
    /// </summary>
    public RtpChannel rtpChannel;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name">String that identifies the call participant that is sending the media for the RTP channel being recorded</param>
    /// <param name="Channel">RtpChannel that is receiving the RTP media from the SRC.</param>
    public RtpRecordingChannelData(string name,  RtpChannel Channel)
    {
        Name = name;
        rtpChannel = Channel;
    }

    /// <summary>
    /// Call when the SIPREC call has ended. Derived classes must close any files that they are writing to
    /// and call the Shutdown() method of the RtpChannel.
    /// </summary>
    public virtual void StopRecording()
    {
    }
}
