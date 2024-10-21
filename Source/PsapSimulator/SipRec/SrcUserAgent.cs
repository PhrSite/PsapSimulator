/////////////////////////////////////////////////////////////////////////////////////
//  File:   SrcUserAgent.cs                                         14 Oct 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

using System.Security.Cryptography.X509Certificates;

using SipLib.Core;
using SipLib.Body;
using SipLib.Sdp;
using SipLib.Media;
using SipLib.Channels;
using SipLib.Transactions;
using SipLib.I3EventLogging;
using System.Net;
using SipLib.Logging;
using SipLib.Rtp;

using System.Collections.Concurrent;
using SipRecMetaData;
using Ng911Lib.Utilities;
using PsapSimulator.CallManagement;

namespace SipLib.SipRec;

/// <summary>
/// SIPREC Recording Client User Agent class. This class handles all calls being recorded by a SIPREC Recording
/// Server (SRS) using a single, permanent connection to the SRS. It also logs I3V3 events.
/// <para>
/// To use this class, construct an instance of it, hook the events and then call the Start() method.
/// </para>
/// <para>
/// Call the Shutdown() method to close all network connections and release resources when the application
/// is closing or when the interface to the SIPREC recorder is no longer needed.
/// </para>
/// </summary>
public class SrcUserAgent
{
    private SipRecRecorderSettings m_Settings;
    private MediaPortManager m_PortManager;    
    private X509Certificate2 m_Certificate;
    private string m_ElementID;
    private string m_AgencyID;
    private string m_AgentID;
    private I3EventLoggingManager m_I3EventLoggingManager;

    private const string UaName = "SrcUserAgent";

    private IPEndPoint m_LocalEndPoint;
    private SIPChannel m_SipChannel;
    private SipTransport m_SipTransport;

    private bool m_IsStarted = false;
    private bool m_IsShutdown = false;
    private CancellationTokenSource m_CancellationTokenSource = new CancellationTokenSource();
    private Task? m_UserAgentTask = null;
    private SemaphoreSlim m_Semaphore = new SemaphoreSlim(0, int.MaxValue);
    private const int WaitIntervalMsec = 100;
    private ConcurrentQueue<Action> m_WorkQueue = new ConcurrentQueue<Action>();

    /// <summary>
    /// The key is the Call-ID header value of the original call, which is the same as the Call-ID of
    /// the call to the SRS.
    /// </summary>
    private Dictionary<string, SrcCall> m_Calls = new Dictionary<string, SrcCall>();

    private bool m_SrsResponding = true;
    private DateTime m_LastOptionsTime = DateTime.Now;
    private DateTime m_LastOptionsResponseTime = DateTime.Now;
    private SIPRequest m_OptionsReq;
    private IPEndPoint m_SrsRemoteEndPoint;
    private const int OPTIONS_TIMEOUT_MS = 1000;

    private SIPURI m_SrsUri;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="settings">Configuration settings</param>
    /// <param name="portManager">Used for allocating UDP and TCP ports for media streams.</param>
    /// <param name="certificate">X.509 certificate to use for SIP over TLS (SIPS) and MSRP over
    /// TLS (MSRPS). Required even if TLS is not currently in use.</param>
    /// <param name="agencyID">Identity of the agency that is recording and logging calls</param>
    /// <param name="agentID">Identity of the agent or call taker that is recording and logging calls.</param>
    /// <param name="elementID">NG9-1-1 Element Identifier of the entity recording calls.</param>
    /// <param name="i3EventLoggingManager">Used for logging NG9-1-1 events.</param>
    public SrcUserAgent(SipRecRecorderSettings settings, MediaPortManager portManager, X509Certificate2 certificate,
        string agencyID, string agentID, string elementID, I3EventLoggingManager i3EventLoggingManager)
    {
        m_Settings = settings;
        m_PortManager = portManager;
        m_Certificate = certificate;
        m_ElementID = elementID;
        m_AgencyID = agencyID;
        m_AgentID = agentID;
        m_I3EventLoggingManager = i3EventLoggingManager;

        m_LocalEndPoint = IPEndPoint.Parse(m_Settings.LocalIpEndpoint);

        if (m_Settings.SipTransportProtocol == SIPProtocolsEnum.udp)
            m_SipChannel = new SIPUDPChannel(m_LocalEndPoint, UaName, null);
        else if (m_Settings.SipTransportProtocol == SIPProtocolsEnum.tcp)
            m_SipChannel = new SIPTCPChannel(m_LocalEndPoint, UaName, null);
        else
            m_SipChannel = new SIPTLSChannel(m_Certificate, m_LocalEndPoint, UaName);

        m_SipTransport = new SipTransport(m_SipChannel);

        m_SrsRemoteEndPoint = IPEndPoint.Parse(m_Settings.SrsIpEndpoint);
        SIPEndPoint SrsEP = new SIPEndPoint(m_Settings.SipTransportProtocol, m_SrsRemoteEndPoint);
        SIPSchemesEnum SipScheme = m_SipChannel.SIPChannelContactURI.Scheme;
        m_SrsUri = new SIPURI(SipScheme, SrsEP);
        m_SrsUri.User = m_Settings.Name;

        m_OptionsReq = SIPRequest.CreateBasicRequest(SIPMethodsEnum.OPTIONS, m_SrsUri, m_SrsUri, m_Settings.Name, 
            m_SipChannel.SIPChannelContactURI, UaName);
    }

    /// <summary>
    /// Initializes the SIP transport interface to the SRS and starts communication with the SRS.
    /// </summary>
    public void Start()
    {
        if (m_IsStarted == true || m_IsShutdown == true)
            return;

        m_SipTransport.SipRequestReceived += OnSipRequestReceived;
        m_SipTransport.SipResponseReceived += OnSipResponseReceived;
        m_SipTransport.LogSipRequest += OnLogSipRequest;
        m_SipTransport.LogSipResponse += OnLogSipResponse;

        // Force an OPTIONS request to be sent to the SRS as soon as this SRC is started.
        m_LastOptionsTime = DateTime.Now - TimeSpan.FromSeconds(m_Settings.OptionsIntervalSeconds);

        m_UserAgentTask = Task.Run(() => { SrcUserAgentTask(m_CancellationTokenSource.Token); });
        m_IsStarted = true;
        m_SipTransport.Start();
    }

    /// <summary>
    /// Shuts down all SIP transport connections and releases resources.
    /// </summary>
    public async Task Shutdown()
    {
        if (m_IsStarted == false)
            return;

        if (m_IsShutdown == true)
            return;

        m_IsShutdown = true;

        // TODO: Terminate any calls that are currently active


        // Unhook the event handlers
        m_SipTransport.SipRequestReceived -= OnSipRequestReceived;
        m_SipTransport.SipResponseReceived -= OnSipResponseReceived;
        m_SipTransport.LogSipRequest -= OnLogSipRequest;
        m_SipTransport.LogSipResponse -= OnLogSipResponse;
        m_SipTransport.Shutdown();

        m_CancellationTokenSource.Cancel();
        if (m_UserAgentTask != null)
            await m_UserAgentTask.WaitAsync(TimeSpan.FromMilliseconds(500));

        m_UserAgentTask = null;
    }

    private void SrcUserAgentTask(CancellationToken cancellationToken)
    {
        CancellationToken token = cancellationToken;
        try
        {
            while (token.IsCancellationRequested == false)
            {
                m_Semaphore.Wait(WaitIntervalMsec);

                DoTimedEvents();

                while (token.IsCancellationRequested == false && m_WorkQueue.TryDequeue(out Action? action) == true)
                {
                    if (action != null)
                        action();
                }

            } // end while
        }
        catch (Exception ex)
        {
            SipLogger.LogError(ex, "Unexpected exception.");
        }
    }

    private void DoTimedEvents()
    {
        DateTime Now = DateTime.Now;
        if (m_Settings.EnableOptions == true)
        {
            if (Now >= (m_LastOptionsTime + TimeSpan.FromSeconds(m_Settings.OptionsIntervalSeconds)))
            {
                m_OptionsReq.Header.CSeq += 1;
                m_OptionsReq.Header.Vias.TopViaHeader!.Branch = CallProperties.CreateBranchId();
                m_SipTransport.StartClientNonInviteTransaction(m_OptionsReq, m_SrsRemoteEndPoint, OnOptionsRequestComplete,
                    OPTIONS_TIMEOUT_MS);
                m_LastOptionsTime = Now;
            }
        }
    }

    private void OnOptionsRequestComplete(SIPRequest sipRequest, SIPResponse? sipResponse,
        IPEndPoint remoteEndPoint, SipTransport sipTransport, SipTransactionBase Transaction)
    {
        if (sipResponse == null)
        {   // No response was received.
            m_SrsResponding = false;
        }
        else
        {
            if (sipResponse.Status == SIPResponseStatusCodesEnum.Ok)
            {
                m_SrsResponding = true;
                m_LastOptionsResponseTime = DateTime.Now;
            }
            else
            {
                // TODO: Handle a responses other than 200 OK
            }
        }
    }

    /// <summary>
    /// Gets the Call object for a specified call ID
    /// </summary>
    /// <param name="callID">Call-ID header value for the call.</param>
    /// <returns>Returns the SrcCall object if it exists or null if it does not</returns>
    private SrcCall? GetCall(string callID)
    {
        try
        {
            return m_Calls[callID];
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
    }

    /// <summary>
    /// Gets the Enabled setting
    /// </summary>
    public bool Enabled
    {
        get { return m_Settings.Enabled; }
    }

    /// <summary>
    /// Starts the SIPREC recording process for a new call
    /// </summary>
    /// <param name="srcCallParameters">Contains the parameters for the new call</param>
    public void StartRecording(SrcCallParameters srcCallParameters)
    {
        m_WorkQueue.Enqueue(() => { DoStartRecording(srcCallParameters); });
    }

    private void DoStartRecording(SrcCallParameters srcCallParameters)
    {
        SIPRequest Invite = BuildInitialInviteRequest(srcCallParameters, out Sdp.Sdp offerSdp,
            out recording Recording);
        SrcCall srcCall = new SrcCall(srcCallParameters, Invite, offerSdp, Recording, UaName, m_Certificate);
        m_Calls.Add(srcCallParameters.CallId, srcCall);

        srcCall.ClientInviteTransaction = m_SipTransport.StartClientInvite(Invite, m_SrsRemoteEndPoint,
            OnInviteTransactionComplete, null);
    }

    private void OnInviteTransactionComplete(SIPRequest sipRequest, SIPResponse? sipResponse,
        IPEndPoint remoteEndPoint, SipTransport sipTransport, SipTransactionBase Transaction)
    {
        m_WorkQueue.Enqueue(() => {
            DoInviteTransactionComplete(sipRequest, sipResponse, remoteEndPoint, sipTransport, 
                Transaction);
        });
    }   

    private void DoInviteTransactionComplete(SIPRequest sipRequest, SIPResponse? sipResponse,
        IPEndPoint remoteEndPoint, SipTransport sipTransport, SipTransactionBase Transaction)
    {
        if (string.IsNullOrEmpty(sipRequest.Header.CallId) == true)
        {
            // TODO: log the error
            return;
        }

        SrcCall? srcCall = GetCall(sipRequest.Header.CallId);
        if (srcCall == null)
        {   // Error: Call not found

            // TODO: log the error
            return;
        }

        srcCall.ClientInviteTransaction = null;     // The client INVITE transaction is complete

        if (sipResponse == null)
        {   // The SRS did not respond to the INVITE request

            // TODO: log the error

            m_Calls.Remove(sipRequest.Header.CallId);
            return;
        }

        if (sipResponse.Status != SIPResponseStatusCodesEnum.Ok)
        {   // The SRS rejected the SIPREC call

            // TODO: log the error

            m_Calls.Remove(sipRequest.Header.CallId);
            return;
        }

        string? strAnsweredSdp = sipResponse.GetContentsOfType(SipLib.Body.ContentTypes.Sdp);
        if (strAnsweredSdp == null)
        {   // Error: No answered SDP in the 200 OK response

            // TODO: Log the error

            m_Calls.Remove(sipRequest.Header.CallId);
            return;
        }

        Sdp.Sdp AnsweredSdp = Sdp.Sdp.ParseSDP(strAnsweredSdp);
        if (AnsweredSdp.Media.Count != srcCall.OfferedSdp.Media.Count)
        {
            // TODO: Log the error

            m_Calls.Remove(sipRequest.Header.CallId);
            return;
        }

        srcCall.OkResponse = sipResponse;
        srcCall.AnsweredSdp = AnsweredSdp;
        srcCall.Invite.Header.To!.ToTag = sipResponse.Header.To!.ToTag;

        srcCall.SetupMediaChannels();
    }

    private SIPRequest BuildInitialInviteRequest(SrcCallParameters srcCallParameters, out Sdp.Sdp offerSdp,
        out recording Recording)
    {
        SIPRequest Invite = SIPRequest.CreateBasicRequest(SIPMethodsEnum.INVITE, m_SrsUri, m_SrsUri,
            m_Settings.Name, m_SipChannel.SIPChannelContactURI, UaName);
        // Add the +sip.src feature tag to the Contact header. See Section 6.1.1 of RFC 7866.
        if (Invite.Header.Contact != null && Invite.Header.Contact.Count > 0)
            Invite.Header.Contact[0].ContactParameters.Set("+sip-rec", null);

        // Use the same Call-ID as the original call
        Invite.Header.CallId = srcCallParameters.CallId;

        // Add the headers required for SIPREC per RFC 7866
        Invite.Header.Require = "siprec";
        Invite.Header.Accept = "application/sdp, application/rs-metadata, application/rs-metadata-request";

        if (string.IsNullOrEmpty(srcCallParameters.EmergencyCallIdentifier) == false)
            SipUtils.AddEmergencyIdUrnCallInfoHeader(Invite, srcCallParameters.EmergencyCallIdentifier,
                "emergency-CallId");

        if (string.IsNullOrEmpty(srcCallParameters.EmergencyIncidentIdentifier) == false)
            SipUtils.AddEmergencyIdUrnCallInfoHeader(Invite, srcCallParameters.EmergencyIncidentIdentifier,
                "emergency-IncidentId");

        // Build the SDP to offer to the SRS
        offerSdp = new Sdp.Sdp(m_LocalEndPoint.Address, UaName);
        foreach (MediaDescription mediaDescription in srcCallParameters.AnsweredSdp.Media)
        {
            if (mediaDescription.Port == 0)
                // This media type was rejected in the original call so it cannot be recorded
                continue;

            if (mediaDescription.MediaType == MediaTypes.MSRP)
            {
                offerSdp.Media.Add(BuildOfferMsrpMediaDescription(mediaDescription, true));
                offerSdp.Media.Add(BuildOfferMsrpMediaDescription(mediaDescription, false));
            }
            else
            {   // The media type is audio, video or RTT
                offerSdp.Media.Add(BuildOfferRtpMediaDescription(mediaDescription, true));
                offerSdp.Media.Add(BuildOfferRtpMediaDescription(mediaDescription, false));
            }
        }

        // Build the initial SIPREC metadata
        Recording = BuildInitialMetaData(srcCallParameters, offerSdp);

        // Attach the SDP and the SIPREC metadata to the body of the INVITE request
        SipBodyBuilder sipBodyBuilder = new SipBodyBuilder();
        sipBodyBuilder.AddContent(Body.ContentTypes.Sdp, offerSdp.ToString(), null, null);
        sipBodyBuilder.AddContent(recording.ContentType, XmlHelper.SerializeToString(Recording), null, null);
        sipBodyBuilder.AttachMessageBody(Invite);

        return Invite;
    }

    private recording BuildInitialMetaData(SrcCallParameters srcCallParameters, Sdp.Sdp offerSdp)
    {
        DateTime Now = DateTime.Now;

        recording Recording = new recording();
        Recording.datamode = dataMode.complete;
        group Group = new group();
        Recording.groups.Add(Group);
        Group.associatetime = Now;
        session Session = new session();
        Session.starttime = Now;
        Session.sipSessionID.Add(srcCallParameters.CallId);

        // Assign the session object to the Communication Session Group.
        Session.groupref = Group.group_id;
        Recording.sessions.Add(Session);
        sessionrecordingassoc Sra = new sessionrecordingassoc();
        Sra.associatetime = Now;
        Sra.session_id = Session.session_id;
        Recording.sessionrecordingassocs.Add(Sra);

        // Create the participants
        participant FromParticipant = new participant();
        nameID FromNameId = new nameID();
        FromNameId.aor = srcCallParameters.FromUri.ToString();
        FromParticipant.nameIDs.Add(FromNameId);
        Recording.participants.Add(FromParticipant);

        participant ToParticipant = new participant();
        nameID ToNameId = new nameID();
        ToNameId.aor = srcCallParameters.ToUri.ToString();
        ToParticipant.nameIDs.Add(ToNameId);
        Recording.participants.Add(ToParticipant);

        // Associate the participants with the session.
        participantsessionassoc FromPsa = new participantsessionassoc();
        FromPsa.associatetime = Now;
        FromPsa.participant_id = FromParticipant.participant_id;
        FromPsa.session_id = Session.session_id;
        Recording.participantsessionassocs.Add(FromPsa);

        participantsessionassoc ToPsa = new participantsessionassoc();
        ToPsa.associatetime = Now;
        ToPsa.participant_id = ToParticipant.participant_id;
        ToPsa.session_id = Session.session_id;
        Recording.participantsessionassocs.Add(ToPsa);

        // Create the participant stream associations.
        participantstreamassoc FromPartSteamAssoc = new participantstreamassoc();
        FromPartSteamAssoc.associatetime = Now;
        FromPartSteamAssoc.participant_id = FromParticipant.participant_id;
        Recording.participantstreamassocs.Add(FromPartSteamAssoc);

        participantstreamassoc ToPartStreamAssoc = new participantstreamassoc();
        ToPartStreamAssoc.associatetime = Now;
        ToPartStreamAssoc.participant_id = ToParticipant.participant_id;
        Recording.participantstreamassocs.Add(ToPartStreamAssoc);

        // Create the streams and build the associations
        foreach (MediaDescription mediaDescription in offerSdp.Media)
        {
            stream stream = new stream();
            stream.session_id = Session.session_id;
            stream.label = mediaDescription.Label;
            Recording.streams.Add(stream);
        }

        return Recording;
    }

    private MediaDescription BuildOfferRtpMediaDescription(MediaDescription Original, bool ForReceived)
    {
        MediaDescription offerMd = Original.CreateCopy();
        MediaLabel label = MediaLabel.ReceivedAudio;
        switch (Original.MediaType)
        {
            case MediaTypes.Audio:
                offerMd.Port = m_PortManager.NextAudioPort;
                label = ForReceived == true ? MediaLabel.ReceivedAudio : MediaLabel.SentAudio;
                break;
            case MediaTypes.Video:
                offerMd.Port = m_PortManager.NextVideoPort;
                label = ForReceived == true ? MediaLabel.ReceivedVideo : MediaLabel.SentVideo;
                break;
            case MediaTypes.RTT:
                offerMd.Port = m_PortManager.NextRttPort;
                label = ForReceived == true ? MediaLabel.ReceivedRTT : MediaLabel.SentRTT;
                break;
        }

        offerMd.Label = SipRecUtils.MediaLabelToString(label);
        offerMd.MediaDirection = MediaDirectionEnum.sendonly;

        // Handle media encryption
        if (m_Settings.RtpEncryption == RtpEncryptionEnum.SdesSrtp)
            SdpUtils.AddSdesSrtpEncryption(offerMd);
        else if (m_Settings.RtpEncryption == RtpEncryptionEnum.DtlsSrtp && RtpChannel.CertificateFingerprint != null)
            SdpUtils.AddDtlsSrtp(offerMd, RtpChannel.CertificateFingerprint);

        return offerMd;
    }

    private MediaDescription BuildOfferMsrpMediaDescription(MediaDescription Original, bool ForReceived)
    {
        MediaDescription offerMd = SdpUtils.CreateMsrpMediaDescription(m_LocalEndPoint.Address,
            m_PortManager.NextMsrpPort, m_Settings.MsrpEncryption == MsrpEncryptionEnum.Msrps ?
            true : false, SetupType.active, m_Certificate, UaName);
        offerMd.MediaDirection = MediaDirectionEnum.sendonly;
        MediaLabel label = ForReceived == true ? MediaLabel.ReceivedMsrp : MediaLabel.SentMsrp;
        offerMd.Label = SipRecUtils.MediaLabelToString(label);
        SdpAttribute? AcceptTypes = Original.GetNamedAttribute("accept-types");
        if (AcceptTypes != null)
            offerMd.Attributes.Add(AcceptTypes);

        return offerMd;
    }

    /// <summary>
    /// Call this method to stop recording when a call has ended.
    /// </summary>
    /// <param name="strCallId">Call-ID for the call.</param>
    public void StopRecording(string strCallId)
    {
        m_WorkQueue.Enqueue(() => { DoStopRecording(strCallId); });
    }

    private void DoStopRecording(string strCallId)
    {
        SrcCall? srcCall = GetCall(strCallId);
        if (srcCall == null)
        {
            // TODO: Call not found -- log the error
            return;
        }

        if (srcCall.ClientInviteTransaction != null)
        {   // An OK response has not been received yet
            srcCall.ClientInviteTransaction.CancelInvite();
            return;
        }

        SIPRequest ByeRequest = SipUtils.BuildByeRequest(srcCall.Invite, m_SipChannel, m_SrsRemoteEndPoint,
            false, srcCall.LastCSeq, srcCall.OkResponse!);
        // Fire and forget the BYE request transaction
        m_SipTransport.StartClientNonInviteTransaction(ByeRequest, m_SrsRemoteEndPoint, null, 1000);
        srcCall.ShutdownMediaConnections();
        m_Calls.Remove(strCallId);
    }

    private void OnLogSipRequest(SIPRequest sipRequest, IPEndPoint remoteEndPoint, bool Sent, SipTransport sipTransport)
    {
        
    }

    private void OnLogSipResponse(SIPResponse sipResponse, IPEndPoint remoteEndPoint, bool Sent, SipTransport sipTransport)
    {

    }

    private void OnSipRequestReceived(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransportManager)
    {

    }

    private void OnSipResponseReceived(SIPResponse sipResponse, SIPEndPoint remoteEndPoint, SipTransport sipTransportManager)
    {

    }

}
