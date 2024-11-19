/////////////////////////////////////////////////////////////////////////////////////
//  File:   SrsCall.cs                                              21 Oct 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

using SipLib.Core;
using SipLib.Sdp;
using SipRecMetaData;
using SipLib.Rtp;
using SipLib.Msrp;
using SipLib.Transactions;
using SipLib.Media;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using Ng911Lib.Utilities;

namespace SrsSimulator;

/// <summary>
/// Class for a SIPREC Recording Server (SRS) incoming call from a SIPREC Recording Client (SRC).
/// </summary>
internal class SrsCall
{
    /// <summary>
    /// Last INVITE request that was sent by the SRC
    /// </summary>
    private SIPRequest m_Invite;

    private IPEndPoint m_RemoteEndpoint;

    private SipTransport m_Transport;

    private MediaPortManager m_PortManager;

    private X509Certificate2 m_Certificate;

    private SdpAnswerSettings m_AnswerSettings;

    /// <summary>
    /// Last SDP that was sent by the SRC
    /// </summary>
    private Sdp? m_OfferedSdp;

    /// <summary>
    /// OK response to the INVITE request that was sent to the SRC.
    /// </summary>
    public SIPResponse? m_OkResponse;

    /// <summary>
    /// SDP sent to the SRC in the OK response
    /// </summary>
    public Sdp? m_AnsweredSdp;

    /// <summary>
    /// Contains the last SIPREC metadata object that was received
    /// </summary>
    private recording? m_Recording;

    private string m_CallRecordingDirectory;

    private List<RtpRecordingChannelData> m_RtpRecordingChannels = new List<RtpRecordingChannelData>();
    private List<MsrpChannelData> m_MsrpRecordingChannels = new List<MsrpChannelData>();

    public SrsCall(SIPRequest invite, IPEndPoint remoteEndPoint, SipTransport sipTransportManager,
        MediaPortManager portManager, X509Certificate2 certificate, SdpAnswerSettings AnswerSettings,
        string callRecordingDirectory)
    {
        m_Invite = invite;
        m_RemoteEndpoint = remoteEndPoint;
        m_Transport = sipTransportManager;
        m_PortManager = portManager;
        m_Certificate = certificate;
        m_AnswerSettings = AnswerSettings;
        m_CallRecordingDirectory = callRecordingDirectory;
    }

    public SIPResponse StartCall()
    {
        string? strSdp = m_Invite.GetContentsOfType(SipLib.Body.ContentTypes.Sdp);
        if (strSdp == null)
        {   // Error: No SDP offered with the INVITE request
            return SipUtils.BuildResponse(m_Invite, SIPResponseStatusCodesEnum.BadRequest, "No SDP", 
                m_Transport.SipChannel, null);
        }

        // TODO: check other errors in the INVITE request

        m_OfferedSdp = Sdp.ParseSDP(strSdp);

        // Make sure every MediaDescription has a label attribute
        foreach (MediaDescription md in m_OfferedSdp.Media)
        {
            if (md.Label == null)
            {
                return SipUtils.BuildResponse(m_Invite, SIPResponseStatusCodesEnum.BadRequest, "Missing label attribute",
                    m_Transport.SipChannel, null);
            }
        }

        IPAddress localAddress = m_Transport.SipChannel.SIPChannelEndPoint!.GetIPEndPoint().Address;
        m_AnsweredSdp = Sdp.BuildAnswerSdp(m_OfferedSdp, localAddress, m_AnswerSettings);
        foreach (MediaDescription AnsMd in m_AnsweredSdp.Media)
        {
            AnsMd.MediaDirection = MediaDirectionEnum.recvonly;
        }

        m_OkResponse = SipUtils.BuildOkToInvite(m_Invite, m_Transport.SipChannel, m_AnsweredSdp.ToString(),
            SipLib.Body.ContentTypes.Sdp);

        // Add the headers required for SIPREC per RFC 7866
        if (m_OkResponse.Header.Contact != null && m_OkResponse.Header.Contact.Count > 0)
            m_OkResponse.Header.Contact[0].ContactParameters.Set("+sip-srs", null);
        m_OkResponse.Header.Require = "siprec";
        m_OkResponse.Header.Accept = "application/sdp, application/rs-metadata, application/rs-metadata-request";

        string? Error = ValidateMetaDataAndBuildRecordingChannels();

        if (Error != null)
        {
            // TODO: Log the error
            return SipUtils.BuildResponse(m_Invite, SIPResponseStatusCodesEnum.BadRequest, "Invalid SIPREC Metadata",
                m_Transport.SipChannel, null);
        }
        else
            return m_OkResponse;
    }

    public void EndCall()
    {
        foreach (RtpRecordingChannelData channelData in m_RtpRecordingChannels)
        {
            channelData.StopRecording();
        }

        foreach (MsrpChannelData msrpChannelData in m_MsrpRecordingChannels)
        {
            msrpChannelData.StopRecording();
        }
    }

    private string? ValidateMetaDataAndBuildRecordingChannels()
    {
        string? strRecording = m_Invite.GetContentsOfType(recording.ContentType);
        if (strRecording != null)
        {
            m_Recording = XmlHelper.DeserializeFromString<recording>(strRecording);
            if (m_Recording == null)
                return "Invalid SIPREC Metadata";

            // For debug only
            File.WriteAllText(Path.Combine(m_CallRecordingDirectory, "MetaData.xml"), strRecording);
        }
        else
            return "No SIPREC Metadata";

        if (m_Recording.participants == null || m_Recording.participants.Count != 2)
            return "Incorrect number of metadata participants";

        if (m_Recording.participantstreamassocs == null || m_Recording.participantstreamassocs.Count != 2)
            return "Incorrect participantstreamassociations";

        if (m_AnsweredSdp == null)
            return "No answered SDP";   // This error is not really possible

        string ParticipantName;
        foreach (MediaDescription AnsweredMd in m_AnsweredSdp.Media)
        {
            MediaDescription? OfferedMd = GetMediaDescriptionFromLabel(AnsweredMd.Label, m_OfferedSdp!);
            if (OfferedMd == null)
                return "Offered media description not found";

            ParticipantName = GetParticipantNameFromLabel(AnsweredMd.Label!);
            if (AnsweredMd.MediaType == MediaTypes.MSRP)
            {
                (MsrpConnection? msrpConnection, string? MsrpError) = MsrpConnection.CreateFromSdp(OfferedMd, 
                    AnsweredMd, true, m_Certificate!);
                if (msrpConnection == null)
                {
                    // TODO: Log this error
                    continue;
                }

                MsrpChannelData msrpChannelData = new MsrpChannelData(ParticipantName, msrpConnection,
                    m_CallRecordingDirectory);
                m_MsrpRecordingChannels.Add(msrpChannelData);
            }
            else
            {
                (RtpChannel? rtpChannel, string? RtpError) = RtpChannel.CreateFromSdp(true, m_OfferedSdp!,
                    OfferedMd, m_AnsweredSdp, AnsweredMd, false, null);
                if (rtpChannel == null)
                {
                    // TODO: Log this error
                    continue;
                }

                switch (AnsweredMd.MediaType)
                {
                    case MediaTypes.Audio:
                        AudioChannelData audioChannelData = new AudioChannelData(ParticipantName, rtpChannel,
                            m_CallRecordingDirectory, AnsweredMd);
                        m_RtpRecordingChannels.Add(audioChannelData);
                        break;
                    case MediaTypes.RTT:
                        // For debug only
                        Console.WriteLine($"Label: {AnsweredMd.Label} Participant: {ParticipantName}");
                        RttChannelData rttChannelData = new RttChannelData(ParticipantName, rtpChannel,
                            m_CallRecordingDirectory, AnsweredMd);
                        m_RtpRecordingChannels.Add(rttChannelData);
                        break;
                    case MediaTypes.Video:
                        VideoChannelData videoChannelData = new VideoChannelData(ParticipantName, rtpChannel,
                            m_CallRecordingDirectory, AnsweredMd);
                        m_RtpRecordingChannels.Add(videoChannelData);
                        break;
                }
            }
        }

        return null;
    }

    private MediaDescription? GetMediaDescriptionFromLabel(string? Label, Sdp sdp)
    {
        if (string.IsNullOrEmpty(Label) == true)
            return null;

        foreach (MediaDescription Md in sdp.Media)
        {
            if (Md.Label == Label)
                return Md;
        }

        return null;
    }

    private string GetParticipantNameFromLabel(string label)
    {
        string strDefaultParticipantName = $"Participant{label}";
        if (m_Recording == null)
            return strDefaultParticipantName;

        string? streamID = GetStreamIdFromLabel(label);
        if (streamID == null)
            return strDefaultParticipantName;

        string? participantSendId = null;
        foreach (participantstreamassoc psa in m_Recording.participantstreamassocs)
        {
            if (psa.send != null && psa.send.Count > 0 && psa.send[0] == streamID)
            {
                participantSendId = psa.participant_id;
                break;
            }
        }

        if (string.IsNullOrEmpty(participantSendId) == true)
            return strDefaultParticipantName;

        participant? Participant = GetParticipant(participantSendId);
        if (Participant == null)
            return strDefaultParticipantName;

        string? ParticipantName = GetName(Participant);
        if (string.IsNullOrEmpty(ParticipantName) == true)
            return strDefaultParticipantName;

        int Index = ParticipantName.IndexOfAny(Path.GetInvalidFileNameChars());
        if (Index < 0)
            return ParticipantName;
        else
            return strDefaultParticipantName;
    }



    private string? GetStreamIdFromLabel(string label)
    {
        foreach (stream strm in m_Recording!.streams)
        {
            if (strm.label == label)
                return strm.stream_id;
        }

        return null;
    }

    private participant? GetParticipant(string participantID)
    {
        foreach (participant Participant in m_Recording!.participants)
        {
            if (Participant.participant_id == participantID)
                return Participant;
        }

        return null;
    }

    private string? GetName(participant Participant)
    {
        if (Participant.nameIDs != null && Participant.nameIDs.Count > 0)
        {
            if (Participant.nameIDs[0].name != null && string.IsNullOrEmpty(Participant.nameIDs[0].name.Value) == false)
                return Participant.nameIDs[0].name.Value;
            else if (string.IsNullOrEmpty(Participant.nameIDs[0].aor) == false)
            {
                SIPURI? sipUri;
                if (SIPURI.TryParse(Participant.nameIDs[0].aor, out sipUri) == true)
                {
                    if (sipUri is not null && string.IsNullOrEmpty(sipUri.User) == true)
                        return sipUri.User;
                    else
                        return Participant.nameIDs[0].aor;
                }
                else
                    return Participant.nameIDs[0].aor;
            }
            else
                return null;
        }

        return null;
    }
}
