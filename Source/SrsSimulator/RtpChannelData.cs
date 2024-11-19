/////////////////////////////////////////////////////////////////////////////////////
//  File:   RecordingChannelData.cs                                 29 Oct 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

using SipLib.Rtp;

namespace SrsSimulator;

internal class RtpChannelData
{
    public string Name;

    public string MediaType;

    public RtpChannel rtpChannel;

    public RtpPacketReceivedDelegate packetReceivedDelegate;

    public RtpChannelData(string name, string mediaType, RtpChannel channel, RtpPacketReceivedDelegate packetDelegate)
    {
        Name = name;
        MediaType = mediaType;
        rtpChannel = channel;
        packetReceivedDelegate = packetDelegate;
    }
}
