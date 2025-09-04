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
using SipLib.Channels;

namespace SrsSimulator;

/// <summary>
/// Class for a single SIP Recording Server (SRS) incoming call from a SIPREC Recording Client (SRC).
/// </summary>
internal class SrsCall
{
    /// <summary>
    /// Last INVITE request that was sent by the SRC
    /// </summary>
    private SIPRequest m_Invite;
    private SipTransport m_Transport;
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

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="invite">SIP INVITE request that was received from the SRC for a new call.</param>
    /// <param name="sipTransportManager">SipTransport that the INVITE request was received on.</param>
    /// <param name="certificate">X.509 certificate to use for secure MSRP</param>
    /// <param name="AnswerSettings">Configuration settings that determine how to build the SDP to send in the OK response.</param>
    /// <param name="callRecordingDirectory">Directory path to store the recordings in. Must already be created.</param>
    public SrsCall(SIPRequest invite, SipTransport sipTransportManager, X509Certificate2 certificate, SdpAnswerSettings AnswerSettings,
        string callRecordingDirectory)
    {
        m_Invite = invite;
        m_Transport = sipTransportManager;
        m_Certificate = certificate;
        m_AnswerSettings = AnswerSettings;
        m_CallRecordingDirectory = callRecordingDirectory;
    }

    /// <summary>
    /// Starts the call.
    /// </summary>
    /// <returns>SIPResponse to send to the caller in response to the INVITE request.</returns>
    public SIPResponse StartCall()
    {
        string? strSdp = m_Invite.GetContentsOfType(SipLib.Body.ContentTypes.Sdp);
        if (strSdp == null)
        {   // Error: No SDP offered with the INVITE request
            return SipUtils.BuildResponse(m_Invite, SIPResponseStatusCodesEnum.BadRequest, "No SDP", m_Transport.SipChannel, null);
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
            return SipUtils.BuildResponse(m_Invite, SIPResponseStatusCodesEnum.BadRequest, Error, m_Transport.SipChannel, null);
        }
        else
            return m_OkResponse;
    }

    /// <summary>
    /// Handles a re-INVITE request.
    /// </summary>
    /// <param name="sipRequest"></param>
    /// <returns>Returns the SIPResponse message to return to the caller.</returns>
    public SIPResponse HandleReInviteRequest(SIPRequest sipRequest)
    {
        SIPResponse response;
        if (SipUtils.IsInDialog(sipRequest) == false || m_OkResponse == null || sipRequest.Header.From!.FromTag !=
            m_Invite.Header.From!.FromTag || sipRequest.Header.To!.ToTag != m_OkResponse.Header.To!.ToTag)
        {
            response = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.CallLegTransactionDoesNotExist,
                "Dialog Does Not Exist", m_Transport.SipChannel, null);
            return response;
        }

        string? strSdp = sipRequest.GetContentsOfType(SipLib.Body.ContentTypes.Sdp);
        if (strSdp == null)
        {   // Error: No SDP offered with the re-INVITE request
            return SipUtils.BuildResponse(m_Invite, SIPResponseStatusCodesEnum.BadRequest, "No SDP",
                m_Transport.SipChannel, null);
        }

        Sdp OfferedSdp = Sdp.ParseSDP(strSdp);

        // Make sure every MediaDescription has a label attribute
        foreach (MediaDescription md in OfferedSdp.Media)
        {
            if (string.IsNullOrEmpty(md.Label) == true)
            {
                return SipUtils.BuildResponse(m_Invite, SIPResponseStatusCodesEnum.BadRequest, "Missing label attribute",
                    m_Transport.SipChannel, null);
            }
        }

        string? strError = ValidateAndSaveMetaData(sipRequest);
        if (strError != null)
            return SipUtils.BuildResponse(m_Invite, SIPResponseStatusCodesEnum.BadRequest, strError, m_Transport.SipChannel, null);

        if (OfferedSdp.Media.Count < m_OfferedSdp!.Media.Count)
        {
            return SipUtils.BuildResponse(m_Invite, SIPResponseStatusCodesEnum.BadRequest, "Invalid SDP for re-INVITE",
                m_Transport.SipChannel, null);
        }

        if (OfferedSdp.Media.Count == m_OfferedSdp.Media.Count)
        {   // No media is being added to the call. Update the CSeq number and respond with the same OK response
            UpdateOkResponse(sipRequest, m_OkResponse);
            m_Invite = sipRequest;
            return m_OkResponse;
        }

        // Media is being added to the call
        int StartIdx = m_OfferedSdp.Media.Count;
        IPAddress localAddress = m_Transport.SipChannel.SIPChannelEndPoint!.GetIPEndPoint().Address;
        for (int i = StartIdx; i < OfferedSdp.Media.Count; i++)
        {
            MediaDescription OfferedMd = OfferedSdp.Media[i];
            MediaDescription AnswerMd;
            string ParticipantName = GetParticipantNameFromLabel(OfferedMd.Label!);

            switch (OfferedMd.MediaType)
            {
                case MediaTypes.Audio:
                    AnswerMd = Sdp.BuildAudioAnswerMediaDescription(OfferedMd, m_AnswerSettings, 0);
                    break;
                case MediaTypes.Video:
                    AnswerMd = Sdp.BuildVideoAnswerMediaDescription(OfferedMd, m_AnswerSettings, 0);
                    break;
                case MediaTypes.RTT:
                    AnswerMd = Sdp.BuildRttAnswerMediaDescription(OfferedMd, m_AnswerSettings, 0);
                    break;
                case MediaTypes.MSRP:
                    AnswerMd = Sdp.BuildMsrpAnswerMediaDescription(OfferedMd, localAddress, m_AnswerSettings, null);
                    break;
                default:    // Unknown media type, reject it
                    AnswerMd = new MediaDescription(OfferedMd.MediaType, 0, OfferedMd.PayloadTypes);
                    break;
            }

            AnswerMd.Label = OfferedMd.Label;
            AnswerMd.MediaDirection = MediaDirectionEnum.recvonly;
            m_AnsweredSdp!.Media.Add(AnswerMd);

            if (OfferedMd.MediaType == MediaTypes.MSRP)
                BuildMsrpChannelData(OfferedMd, AnswerMd, ParticipantName);
            else
                BuildRtpChannelData(OfferedMd, AnswerMd, ParticipantName);
        }

        m_Invite = sipRequest;
        m_OkResponse = SipUtils.BuildOkToInvite(sipRequest, m_Transport.SipChannel, m_AnsweredSdp!.ToString(),
            SipLib.Body.ContentTypes.Sdp);

        return m_OkResponse;
    }

    /// <summary>
    /// Handles a SIP UPDATE request that can update the SIPREC metadata.
    /// </summary>
    /// <param name="sipRequest"></param>
    /// <param name="sipChannel"></param>
    /// <returns></returns>
    public SIPResponse HandleUpdateRequest(SIPRequest sipRequest, SIPChannel sipChannel)
    {
        string? error = ValidateAndSaveMetaData(sipRequest);
        SIPResponse response;
        if (error != null)
            response = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.BadRequest, error, sipChannel,
                null);
        else
            response = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.Ok, "OK", sipChannel, null);

        return response;
    }

    private void UpdateOkResponse(SIPRequest ReInvite, SIPResponse OkResponse)
    {
        OkResponse.Header.CSeq = ReInvite.Header.CSeq;
        OkResponse.Header.Vias.TopViaHeader!.Branch = ReInvite.Header.Vias.TopViaHeader!.Branch;
    }

    /// <summary>
    /// Terminates recording for RTP media and MSRP media when the SIPREC call ends.
    /// </summary>
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

    /// <summary>
    /// Validates the SIPREC meta attatched to a request. If it is a valid XML document, then it saves it
    /// to a file called MetaData.xml in the directory for the recording.
    /// </summary>
    /// <param name="request">SIPRequest object that the metadata XML document should be attached to. This
    /// can be an INVITE or an UPDATE request.</param>
    /// <returns>Returns null if the metadata is valid or a string describing the error if it is not valid.</returns>
    private string? ValidateAndSaveMetaData(SIPRequest request)
    {
        string? strRecording = request.GetContentsOfType(recording.ContentType);
        if (strRecording != null)
        {
            m_Recording = XmlHelper.DeserializeFromString<recording>(strRecording);
            if (m_Recording == null)
                return "Invalid SIPREC Metadata";

            File.WriteAllText(Path.Combine(m_CallRecordingDirectory, "MetaData.xml"), strRecording);

            if (m_Recording.participants == null || m_Recording.participants.Count < 2)
                return "Incorrect number of metadata participants";

            if (m_Recording.participantstreamassocs == null || m_Recording.participantstreamassocs.Count < 2)
                return "Incorrect participantstreamassociations";

            return null;
        }
        else
            return "No SIPREC Metadata";
    }

    private string? ValidateMetaDataAndBuildRecordingChannels()
    {
        string? metaDataError = ValidateAndSaveMetaData(m_Invite);
        if (metaDataError != null)
            return metaDataError;

        if (m_AnsweredSdp == null)
            return "No answered SDP";   // This error is not really possible

        string ParticipantName;
        foreach (MediaDescription AnsweredMd in m_AnsweredSdp.Media)
        {
            MediaDescription? OfferedMd = GetMediaDescriptionFromLabel(AnsweredMd.Label, m_OfferedSdp!);
            if (OfferedMd == null)
                return "Offered media description not found";

            if (AnsweredMd.Port == 0)
                continue;   // This media type is being rejected because it is not supported

            ParticipantName = GetParticipantNameFromLabel(AnsweredMd.Label!);
            if (AnsweredMd.MediaType == MediaTypes.MSRP)
                BuildMsrpChannelData(OfferedMd, AnsweredMd, ParticipantName);
            else
                BuildRtpChannelData(OfferedMd, AnsweredMd, ParticipantName);
        }

        return null;
    }

    private void BuildMsrpChannelData(MediaDescription OfferedMd, MediaDescription AnsweredMd, string ParticipantName)
    {
        (MsrpConnection? msrpConnection, string? MsrpError) = MsrpConnection.CreateFromSdp(OfferedMd,
            AnsweredMd, true, m_Certificate!);
        if (msrpConnection == null)
        {
            // TODO: Log this error
            return;
        }

        MsrpChannelData msrpChannelData = new MsrpChannelData(ParticipantName, msrpConnection,
            m_CallRecordingDirectory);
        m_MsrpRecordingChannels.Add(msrpChannelData);
    }

    private void BuildRtpChannelData(MediaDescription OfferedMd, MediaDescription AnsweredMd, string ParticipantName)
    {
        (RtpChannel? rtpChannel, string? RtpError) = RtpChannel.CreateFromSdp(true, m_OfferedSdp!,
            OfferedMd, m_AnsweredSdp!, AnsweredMd, false, null);
        if (rtpChannel == null)
        {
            // TODO: Log this error
            return;
        }

        switch (AnsweredMd.MediaType)
        {
            case MediaTypes.Audio:
                AudioChannelData audioChannelData = new AudioChannelData(ParticipantName, rtpChannel,
                    m_CallRecordingDirectory, AnsweredMd);
                m_RtpRecordingChannels.Add(audioChannelData);
                break;
            case MediaTypes.RTT:
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
        string strDefaultParticipantName = $"Stream{label}";
        if (m_Recording == null)
            return strDefaultParticipantName;

        string? streamID = GetStreamIdFromLabel(label);
        if (streamID == null)
            return strDefaultParticipantName;

        string? participantSendId = null;
        foreach (participantstreamassoc psa in m_Recording.participantstreamassocs)
        {
            foreach (string str in psa.send)
            {
                if (str == streamID)
                {
                    participantSendId = psa.participant_id;
                    break;
                }
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
