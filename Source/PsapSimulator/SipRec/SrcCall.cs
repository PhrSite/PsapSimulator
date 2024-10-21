/////////////////////////////////////////////////////////////////////////////////////
//  File:   SrcCall.cs                                              18 Oct 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

using SipLib.Core;
using SipLib.Sdp;
using SipRecMetaData;
using SipLib.Rtp;
using SipLib.Msrp;
using SipLib.Transactions;
using SipLib.Media;
using SipLib.Channels;
using System.Security.Cryptography.X509Certificates;

namespace SipLib.SipRec;

/// <summary>
/// Class for a SIPREC Recording Client (SRC) call.
/// </summary>
internal class SrcCall
{
    public SrcCallParameters CallParameters;

    /// <summary>
    /// Last INVITE request that was sent to the SRS
    /// </summary>
    public SIPRequest Invite;

    /// <summary>
    /// Last used CSeq number. Increment before using in a re-INVITE request.
    /// </summary>
    public int LastCSeq;

    /// <summary>
    /// Last SDP that was sent to the SRS
    /// </summary>
    public Sdp.Sdp OfferedSdp;

    /// <summary>
    /// OK reqponse to the INVITE request that was received from the SRS.
    /// </summary>
    public SIPResponse? OkResponse = null;

    /// <summary>
    /// SDP received from the SRS in the OK response
    /// </summary>
    public Sdp.Sdp? AnsweredSdp = null;

    /// <summary>
    /// Contains the last SIPREC metadata object that was sent to the SRS
    /// </summary>
    public recording Recording;

    /// <summary>
    /// Contains a list of RtpChannels for sending RTP media to the SRS
    /// </summary>
    private List<RtpChannel> m_Channels = new List<RtpChannel>();

    private RtpChannel? m_ReceivedAudioChannel = null;
    private RtpChannel? m_SentAudioChannel = null;
    private RtpChannel? m_ReceiveVideoChannel = null;
    private RtpChannel? m_SentVideoChannel = null;
    private RtpChannel? m_ReceiveRttChannel = null;
    private RtpChannel? m_SentRttChannel = null;
    private MsrpConnection? m_ReceivedMsrpConnection = null;
    private MsrpConnection? m_SentMsrpConnection = null;

    private string m_UaName;
    private X509Certificate2 m_Certificate;

    /// <summary>
    /// MsrpConnection for sending MSRP messages received from the remote endpoint to the SRS
    /// </summary>
    public MsrpConnection? ReceivedMsrpConnection = null;

    /// <summary>
    /// MsrpConnection for sending MSRP messages sent to the remote endpoint to the SRS
    /// </summary>
    public MsrpConnection? SentMsrpConnection = null;

    /// <summary>
    /// 
    /// </summary>
    public ClientInviteTransaction? ClientInviteTransaction = null;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="callParameters">Parameters from the original call that is being recorded</param>
    /// <param name="invite">Original INVITE request sent to the SRS</param>
    /// <param name="offeredSdp">SDP that was offered to the SRS in the INVITE request</param>
    /// <param name="recording">Initial SIPREC metadata send with the INVITE to the SRS</param>
    public SrcCall(SrcCallParameters callParameters, SIPRequest invite, Sdp.Sdp offeredSdp, recording recording,
        string UaName, X509Certificate2 certificate)
    {
        CallParameters = callParameters;
        Invite = invite;
        LastCSeq = Invite.Header.CSeq;
        OfferedSdp = offeredSdp;
        Recording = recording;
        m_UaName = UaName;
        m_Certificate = certificate;
    }

    /// <summary>
    /// Sets up the RTP and MSRP connections for sending media to the SRS.
    /// <para>
    /// The OfferedSdp and the AnsweredSdp properties must be set before calling this method
    /// </para>
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if OfferedSdp or the AnsweredSdp properties
    /// are not set yet.</exception>
    public void SetupMediaChannels()
    {
        if (OfferedSdp == null)
            throw new InvalidOperationException("The OfferedSdp must be set before calling this method");

        if (AnsweredSdp == null)
            throw new InvalidOperationException("The AnsweredSdp must be set before calling this method");

        // Hook the events of the original call's RTP channels.
        foreach (RtpChannel rtpChannel in CallParameters.CallRtpChannels)
        {
            switch (rtpChannel.MediaType)
            {
                case MediaTypes.Audio:
                    rtpChannel.RtpPacketReceived += OnReceivedAudio;
                    rtpChannel.RtpPacketSent += OnSentAudio;
                    break;
                case MediaTypes.Video:
                    rtpChannel.RtpPacketReceived += OnReceivedVideo;
                    rtpChannel.RtpPacketSent += OnSentVideo;
                    break;
                case MediaTypes.RTT:
                    rtpChannel.RtpPacketReceived += OnReceivedRtt;
                    rtpChannel.RtpPacketSent += OnSentRtt;
                    break;
            }
        }

        // If the original call has MSRP media then hook the events of the MsrpConnection object
        if (CallParameters.CallMsrpConnection != null)
        {
            CallParameters.CallMsrpConnection.MsrpMessageReceived += OnMsrpMessageReceived;
            CallParameters.CallMsrpConnection.MsrpMessageSent += OnMsrpMessageSent;
        }

        bool IsForReceive;
        int iLabel;
        MediaDescription offeredMediaDescription, answeredMediaDescription;
        RtpChannel? callChannel;

        for (int i = 0; i < OfferedSdp.Media.Count; i++)
        {
            offeredMediaDescription = OfferedSdp.Media[i];
            answeredMediaDescription = AnsweredSdp.Media[i];

            iLabel = int.Parse(offeredMediaDescription.Label!);
            IsForReceive = (iLabel % 2) != 0 ? true : false;
            if (offeredMediaDescription.MediaType == MediaTypes.MSRP && CallParameters.CallMsrpConnection != null)
            {
                (MsrpConnection? connection, string? MsrpError) = MsrpConnection.CreateFromSdp(
                    offeredMediaDescription, answeredMediaDescription, false, m_Certificate);
                if (connection != null)
                {
                    if (IsForReceive == true)
                        m_ReceivedMsrpConnection = connection;
                    else
                        m_SentMsrpConnection = connection;

                    connection.Start();
                }
                else
                {
                    // TODO: handle this error
                }
            }
            else
            {   // Media that is handled by RTP
                callChannel = GetCallRtpChannel(CallParameters, offeredMediaDescription.MediaType);
                if (callChannel != null)
                {
                    (RtpChannel? srcRtpChannel, string? Error) = RtpChannel.CreateFromSdp(false, OfferedSdp, offeredMediaDescription,
                        AnsweredSdp, answeredMediaDescription, false, m_UaName);
                    if (srcRtpChannel != null)
                    {
                        if (IsForReceive == true)
                        {
                            switch (offeredMediaDescription.MediaType)
                            {
                                case MediaTypes.Audio:
                                    m_ReceivedAudioChannel = srcRtpChannel;
                                    break;
                                case MediaTypes.Video:
                                    m_ReceiveVideoChannel = srcRtpChannel;
                                    break;
                                case MediaTypes.RTT:
                                    m_ReceiveRttChannel = srcRtpChannel;
                                    break;
                            }
                        }
                        else
                        {
                            switch (offeredMediaDescription.MediaType)
                            {
                                case MediaTypes.Audio:
                                    m_SentAudioChannel = srcRtpChannel;
                                    break;
                                case MediaTypes.Video:
                                    m_SentVideoChannel = srcRtpChannel;
                                    break;
                                case MediaTypes.RTT:
                                    m_SentRttChannel = srcRtpChannel;
                                    break;
                            }
                        }

                        m_Channels.Add(srcRtpChannel);
                        srcRtpChannel.StartListening();
                    }
                    else
                    {
                        // TODO: handle this error
                    }
                }
                else
                {
                    // TODO: Handle this error
                }
            }

        } // end for i

    }

    /// <summary>
    /// Call this method to shutdown all of the media channels to the SRS when the call has ended or
    /// when the media of the call being recorded has changed in a significant way and there is a
    /// need to re-invite the SRS to the call.
    /// </summary>
    public void ShutdownMediaConnections()
    {
        // Unhook the events of the original call's RTP channels.
        foreach (RtpChannel rtpChannel in CallParameters.CallRtpChannels)
        {
            switch (rtpChannel.MediaType)
            {
                case MediaTypes.Audio:
                    rtpChannel.RtpPacketReceived -= OnReceivedAudio;
                    rtpChannel.RtpPacketSent -= OnSentAudio;
                    break;
                case MediaTypes.Video:
                    rtpChannel.RtpPacketReceived -= OnReceivedVideo;
                    rtpChannel.RtpPacketSent -= OnSentVideo;
                    break;
                case MediaTypes.RTT:
                    rtpChannel.RtpPacketReceived -= OnReceivedRtt;
                    rtpChannel.RtpPacketSent -= OnSentRtt;
                    break;
            }
        }

        if (CallParameters.CallMsrpConnection != null)
        {
            CallParameters.CallMsrpConnection.MsrpMessageReceived -= OnMsrpMessageReceived;
            CallParameters.CallMsrpConnection.MsrpMessageSent -= OnMsrpMessageSent;
        }

        foreach (RtpChannel rtpChannel in m_Channels)
        {
            rtpChannel.Shutdown();
        }

        m_Channels.Clear();

        if (m_ReceivedMsrpConnection != null)
        {
            m_ReceivedMsrpConnection.Shutdown();
            m_ReceivedMsrpConnection = null;
        }

        if (m_SentMsrpConnection != null)
        {
            m_SentMsrpConnection.Shutdown();
            m_SentMsrpConnection = null;
        }
    }

    private void OnReceivedAudio(RtpPacket packet)
    {
        if (m_ReceivedAudioChannel != null)
            m_ReceivedAudioChannel.Send(packet);
    }

    private void OnSentAudio(RtpPacket packet)
    {
        if (m_SentAudioChannel != null)
            m_SentAudioChannel.Send(packet);
    }

    private void OnReceivedVideo(RtpPacket packet)
    {
        if (m_ReceiveVideoChannel != null)
            m_ReceiveVideoChannel.Send(packet);
    }

    private void OnSentVideo(RtpPacket packet)
    {
        if (m_SentVideoChannel != null)
            m_SentVideoChannel.Send(packet);
    }

    private void OnReceivedRtt(RtpPacket packet)
    {
        if (m_ReceiveRttChannel != null)
            m_ReceiveRttChannel.Send(packet);
    }

    private void OnSentRtt(RtpPacket packet)
    {
        if (m_SentRttChannel != null)
            m_SentRttChannel.Send(packet);
    }

    private void OnMsrpMessageReceived(string ContentType, byte[] Contents, string from)
    {
        if (m_ReceivedMsrpConnection != null)
            m_ReceivedMsrpConnection.SendMsrpMessage(ContentType, Contents);
    }

    private void OnMsrpMessageSent(string ContentType, byte[] Contents)
    {
        if (m_SentMsrpConnection != null)
            m_SentMsrpConnection.SendMsrpMessage(ContentType, Contents);
    }

    private RtpChannel? GetCallRtpChannel(SrcCallParameters srcCallParameters, string strMediaType)
    {
        foreach (RtpChannel rtpChannel in srcCallParameters.CallRtpChannels)
        {
            if (rtpChannel.MediaType == strMediaType)
                return rtpChannel;
        }

        return null;
    }

}
