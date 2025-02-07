/////////////////////////////////////////////////////////////////////////////////////
//  File:   Call.cs                                                 17 Feb 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator.CallManagement;

using PsapSimulator.Settings;

using SipLib.Core;
using SipLib.Media;
using SipLib.Msrp;
using SipLib.RealTimeText;
using SipLib.Rtp;
using SipLib.Sdp;
using SipLib.Transactions;
using SipLib.Collections;
using SipLib.SipRec;
using Pidf;

using System.Net;
using System.Text;
using System.Collections.Concurrent;
using System.Security.Cryptography.X509Certificates;

using SipLib.Body;
using PsapSimulator.WindowsVideo;
using AdditionalData;
using Held;
using HttpUtils;
using Ng911Lib.Utilities;
using I3V3.LogEvents;
using SipLib.Logging;
using SipLib.Subscriptions;
using I3V3.LoggingHelpers;
using ConferenceEvent;
using System.Security.Permissions;

internal delegate void SetAdditionalDataDelegate(object Obj);

/// <summary>
/// Delegate type for the NewLocation event of the Call class.
/// </summary>
/// <param name="newPresence">Contains the new location data</param>
public delegate void NewLocationDelegate(Presence newPresence);

/// <summary>
/// Delegate type for the NewOnferenceInfo event of the Call class.
/// </summary>
/// <param name="conferenceType"></param>
public delegate void NewConferenceInfoDelegate(conferencetype conferenceType);

/// <summary>
/// Delegate type for the CallVideoReceiverChanged event of the Call class.
/// </summary>
/// <param name="oldVideoReceiver">If not null, then the event handler for this event must unhook the FrameReady
/// event of this object.</param>
/// <param name="newVideoReceiver">The event handler must hook the FrameReady event of this object.</param>
public delegate void CallVideoReceiverChangedDelegate(VideoReceiver? oldVideoReceiver, VideoReceiver newVideoReceiver);

/// <summary>
/// Delegate type for the ReferResponseStatus event of the Call class.
/// </summary>
/// <param name="responseEnum">Response status code that was received in response to the REFER request.</param>
/// <param name="reason">Reason text of the response.</param>
public delegate void ReferResponseStatusDelegate(SIPResponseStatusCodesEnum responseEnum, string reason);

/// <summary>
/// Delegate type for the ReferNotifyStatus event of the Call class.
/// </summary>
/// <param name="responseEnum">Status code from the SIPFRAG in the NOTIFY request's body.</param>
/// <param name="reason">Reason text from the SIPFRAG.</param>
public delegate void ReferNotifyStatusDelegate(SIPResponseStatusCodesEnum responseEnum, string reason);

/// <summary>
/// Class for maintaining information about a call
/// </summary>
public class Call
{
    /// <summary>
    /// If true, then call is an incoming call. Else it is an outgoing call
    /// </summary>
    public bool IsIncoming { get; private set; } = true;

    /// <summary>
    /// Last invite request that was received or sent.
    /// </summary>
    internal SIPRequest? InviteRequest { get; set; } = null;

    private SIPResponse? m_OKResponse = null;
    /// <summary>
    /// 200 OK response that was sent or received.
    /// </summary>
    internal SIPResponse? OKResponse
    {
        get { return m_OKResponse; }
        set
        {   
            m_OKResponse = value;
            if (IsIncoming == true)
                LocalTag = m_OKResponse?.Header.To?.ToTag;
            else
                LocalTag = m_OKResponse?.Header.From?.FromTag;
        }
    }

    internal string? RemoteTag = string.Empty;

    internal string? LocalTag = string.Empty;

    internal bool OfferlessInvite { get; set; } = false;

    /// <summary>
    /// Call-ID header value for the call
    /// </summary>
    public string CallID { get; private set; } = string.Empty;

    /// <summary>
    /// ServerInviteTransaction to use to send interim and final responses. Used for incoming calls only.
    /// </summary>
    internal ServerInviteTransaction? serverInviteTransaction { get; set; } = null;

    /// <summary>
    /// SipTransport for the call
    /// </summary>
    internal SipTransport sipTransport { get; set; }

    /// <summary>
    /// Time that the last 180 Ringing response was sent for an incoming call if the current call state is
    /// Ringing.
    /// </summary>
    internal DateTime LastRingingSentTime { get; set; } = DateTime.MinValue;

    /// <summary>
    /// NG9-1-1 emergency-CallId for the call
    /// </summary>
    public string? EmergencyCallIdentifier { get; set; } = null;

    /// <summary>
    /// NG9-1-1 emergency-incidentId for the call
    /// </summary>
    public string? EmergencyIncidentIdentifier { get; set; } = null;

    /// <summary>
    /// Call start time
    /// </summary>
    public DateTime CallStartTime = DateTime.Now;

    /// <summary>
    /// Call answered time.
    /// </summary>
    public DateTime CallAnsweredTime = DateTime.MinValue;

    /// <summary>
    /// Call state for the call
    /// </summary>
    public CallStateEnum CallState { get; internal set; } = CallStateEnum.Idle;

    /// <summary>
    /// Last CSeq number used for the call
    /// </summary>
    public int LastInviteSequenceNumber { get; internal set; } = 0;

    /// <summary>
    /// IPEndPoint of the remote party
    /// </summary>
    public IPEndPoint? RemoteIpEndPoint { get; internal set; } = null;

    internal Sdp? OfferedSdp { get; set; } = null;

    internal Sdp? AnsweredSdp { get; set; } = null;

    internal List<RtpChannel> RtpChannels = new List<RtpChannel>();

    internal MsrpConnection? MsrpConnection { get; set; } = null;

    /// <summary>
    /// Stores all of the MSRP text messages that have been sent or received for the call
    /// </summary>
    public TextMessagesCollection MsrpMessages { get; private set; } = new TextMessagesCollection(TextTypeEnum.MSRP);

    /// <summary>
    /// Stores all of the RTT text messages that have been set or received for the call
    /// </summary>
    public TextMessagesCollection RttMessages { get; private set; } = new TextMessagesCollection(TextTypeEnum.RTT);

    /// <summary>
    /// This object uses an RtpChannel to send audio packets to the remote endpoint. The CurrentAudioSample
    /// source uses this object to send packets by calling the SendAudioSamples() method of this object when
    /// a full RTP packet's worth of samples are available.
    /// </summary>
    internal AudioSource? AudioSampleSource = null;

    /// <summary>
    /// This object is the source of audio samples to send to the remote endpoint. The current audio source
    /// could be the PC's microphone or a file source of audio samples. When this object changes, be sure
    /// to hook it's AudioSamples ready event to the SendAudioSamples() methof of the AudioSampleSource object
    /// for this call.
    /// </summary>
    internal IAudioSampleSource? CurrentAudioSampleSource = null;

    /// <summary>
    /// Receives audio RTP packets from a RtpChannel, decodes them and then calls its AudioDestinationDelegate.
    /// You can change the audio destination handler by calling the SetDestinationHandler() method.
    /// </summary>
    internal AudioDestination? AudioDestination = null;

    internal RttSender? RttSender = null;

    internal RttReceiver? RttReceiver = null;

    internal VideoSender? VideoSender = null;

    internal IVideoCapture? CurrentVideoCapture = null;

    /// <summary>
    /// For processing video that is being received by the caller.
    /// </summary>
    public VideoReceiver? VideoReceiver = null;

    internal DateTime LastAutoAnsweredTextSentTime = DateTime.MinValue;

    internal DateTime LastOnHoldTextSentTime = DateTime.MinValue;

    /// <summary>
    /// Contains a list of locations that have been received.
    /// </summary>
    public ThreadSafeGenericList<Presence> Locations { get; private set; } = new ThreadSafeGenericList<Presence>();

    /// <summary>
    /// Contains the time that the last (most recent) location information data was received. A value of
    /// DateTime.MinValue indicates that no location information has been received
    /// </summary>
    public DateTime LastLocationReceivedTime = DateTime.MinValue;

    /// <summary>
    /// This event is fired when new location for the call is received.
    /// </summary>
    public event NewLocationDelegate? NewLocation = null;

    /// <summary>
    /// Fired when new Additional Data has become available for the call.
    /// </summary>
    public event Action? AdditionalDataAvailable = null;

    /// <summary>
    /// This event is fired when new information about the conference members is available.
    /// </summary>
    public event NewConferenceInfoDelegate? NewConferenceInfo = null;

    /// <summary>
    /// This event is fired if the VideoReceiver object of this call is changed due to a re-INVITE condition.
    /// </summary>
    public event CallVideoReceiverChangedDelegate? CallVideoReceiverChanged = null;
    
    /// <summary>
    /// This event is fired when a response is received for a REFER request to add a member to a conference
    /// </summary>
    public event ReferResponseStatusDelegate? ReferResponseStatus = null;

    /// <summary>
    /// This event is fired when a NOTIFY request for a REFER is received when adding a member to a conference
    /// </summary>
    public event ReferNotifyStatusDelegate? ReferNotifyStatus = null;

    internal void FireReferResponseStatus(SIPResponseStatusCodesEnum responseEnum, string reason)
    {
        ReferResponseStatus?.Invoke(responseEnum, reason);
    }

    internal void FireReferNotifyStatus(SIPResponseStatusCodesEnum responseEnum, string reason)
    {
        ReferNotifyStatus?.Invoke(responseEnum, reason);
    }

    /// <summary>
    /// Fires the AdditionalDataAvailable event.
    /// </summary>
    internal void FireAdditionalDataAvailable()
    {
        AdditionalDataAvailable?.Invoke();
    }

    /// <summary>
    /// Fires the CallVideoReceiverChanged event.
    /// </summary>
    /// <param name="oldVideoReceiver"></param>
    /// <param name="newVideoReceiver"></param>
    internal void FireCallVideoReceiverChanged(VideoReceiver? oldVideoReceiver, VideoReceiver newVideoReceiver)
    {
        CallVideoReceiverChanged?.Invoke(oldVideoReceiver, newVideoReceiver);
    }

    /// <summary>
    /// Gets or sets the caller's ServiceInfo additional data information
    /// </summary>
    public ServiceInfoType? ServiceInfo { get; set; } = null;

    /// <summary>
    /// Gets or sets the caller's SubscriberInfo additional data information
    /// </summary>
    public SubscriberInfoType? SubscriberInfo { get; set; } = null;

    /// <summary>
    /// Contains a dictionary of data providers. The key is the ProviderID property of the ProviderInfoType class.
    /// </summary>
    public ConcurrentDictionary<string, ProviderInfoType> Providers { get; private set; } = new ConcurrentDictionary<string, ProviderInfoType>();

    /// <summary>
    /// Gets or sets the caller's DeviceInfo additional data information
    /// </summary>
    public DeviceInfoType? DeviceInfo { get; set; }

    /// <summary>
    /// Contains a list of Comment additional data blocks.
    /// </summary>
    public ThreadSafeGenericList<CommentType> Comments = new ThreadSafeGenericList<CommentType>();

    /// <summary>
    /// Contains the state and other data for the call's subscription to the presence event package.
    /// RFC 3856 describes the presence event package
    /// </summary>
    public SubscriberData? PresenceSubscriber { get; set; } = null;

    /// <summary>
    /// Contains the state and other data for the call's subscription to the conference event package.
    /// RFC 4575 describes the conference event package.
    /// </summary>
    public SubscriberData? ConferenceSubscriber { get; set; } = null;

    private X509Certificate2 m_Certificate;

    private I3LogEventClientMgr m_I3LogEventClientMgr;
    private IdentitySettings m_IdentitySettings;
    private EventLoggingSettings m_EventLoggingSettings;

    /// <summary>
    /// Gets or sets the conference information
    /// </summary>
    public conferencetype? ConferenceInfo { get; set; } = null;

    // Will be set to true if at least one other party was successfully added to the call.
    internal bool IsConferenced { get; set; } = false;

    /// <summary>
    /// Request URI for the EIDO for this call that was passed to this application in a Call-Info header from a conferenc
    /// aware user agent.
    /// </summary>
    internal string EidoRequestUri { get; set; } = string.Empty;

    private Call(SipTransport transport, X509Certificate2 certificate, I3LogEventClientMgr i3LogEventClientMgr,
        IdentitySettings identitySettings, EventLoggingSettings eventLoggingSettings)
    {
        m_Certificate = certificate;
        sipTransport = transport;
        m_I3LogEventClientMgr = i3LogEventClientMgr;
        m_IdentitySettings = identitySettings;
        m_EventLoggingSettings = eventLoggingSettings;
    }

    /// <summary>
    /// Call this method when the call has ended. This method sends the MediaEndLogEvent to the logging server
    /// for each media type.
    /// </summary>
    public void EndCall()
    {
        if (m_EventLoggingSettings.EnableLogging == true)
        {
            foreach (RtpChannel rtpChannel in RtpChannels)
            {
                switch (rtpChannel.MediaType)
                {
                    case MediaTypes.Audio:
                        SendAudioMediaEndLogEvents();
                        break;
                    case MediaTypes.Video:
                        SendVideoMediaEndLogEvents();
                        break;
                    case MediaTypes.RTT:
                        SendRttMediaEndLogEvents();
                        break;
                }
            }

            if (MsrpConnection != null)
                SendMsrpMediaEndEvents();
        }
 
        if (AudioSampleSource != null)
        {
            AudioSampleSource.Stop();
            AudioSampleSource = null;
        }

        if (CurrentAudioSampleSource != null)
        {
            CurrentAudioSampleSource.Stop();
            CurrentAudioSampleSource = null;
        }

        if (VideoSender != null)
        {
            if (CurrentVideoCapture != null)
                CurrentVideoCapture.FrameReady -= VideoSender.SendVideoFrame;

            VideoSender.Shutdown();
        }

        if (VideoReceiver != null)
        {
            VideoReceiver.Shutdown();
        }

        foreach (RtpChannel rtpChannel in RtpChannels)
        {
            if (rtpChannel.MediaType == MediaTypes.RTT && RttReceiver != null)
                rtpChannel.RtpPacketReceived -= RttReceiver.ProcessRtpPacket;

            rtpChannel.Shutdown();
        }

        RtpChannels.Clear();

        if (RttSender != null)
        {
            RttSender.Stop();
            RttSender = null;
        }

        if (MsrpConnection != null)
        {
            MsrpConnection.Shutdown();
            MsrpConnection = null;
        }

        if (VideoSender != null)
        {
            VideoSender.Dispose();
            VideoSender = null;
        }

        if (VideoReceiver != null)
        {
            VideoReceiver.Dispose();
            VideoReceiver = null;
        }
    }

    private void SendAudioMediaEndLogEvents()
    {
        if (m_FirstAudioPacketReceived == true)
            SendMediaEndLogEvent(MediaLabel.ReceivedAudio);

        if (m_FirstAudioPacketSent == true)
            SendMediaEndLogEvent (MediaLabel.SentAudio);
    }

    private void SendVideoMediaEndLogEvents()
    {
        if (m_FirstVideoPacketReceived == true)
            SendMediaEndLogEvent(MediaLabel.ReceivedVideo);

        if (m_FirstVideoPacketSent == true)
            SendMediaEndLogEvent(MediaLabel.SentVideo);
    }

    private void SendRttMediaEndLogEvents()
    {
        if (m_FirstRttPacketReceived == true)
            SendMediaEndLogEvent(MediaLabel.ReceivedRTT);

        if (m_FirstRttPacketSent == true)
            SendMediaEndLogEvent(MediaLabel.SentRTT);
    }

    private void SendMsrpMediaEndEvents()
    {
        if (m_FirstMsrpMessageReceived == true)
            SendMediaEndLogEvent(MediaLabel.ReceivedMsrp);

        if (m_FirstMsrpMessageSent == true)
            SendMediaEndLogEvent(MediaLabel.SentMsrp);
    }

    /// <summary>
    /// Gets the display data for the call
    /// </summary>
    /// <returns></returns>
    public CallData GetCallData()
    {
        CallData callData = new CallData();
        callData.CallID = CallID;

        string? strFromUser = InviteRequest?.Header?.From?.FromURI?.User;
        if (strFromUser != null)
            callData.From = InviteRequest!.Header!.From!.FromURI!.User!;
        else
            callData.From = "Unknown";

        callData.CallStateString = CallStateToString(CallState);
        callData.MediaTypes = GetMediaTypeDisplayList();

        return callData;
    }

    /// <summary>
    /// Gets the location and additonal data from the INVITE request for an incoming call.
    /// </summary>
    public void GetLocationAndAdditionalData()
    {
        string? strPresence = InviteRequest!.GetContentsOfType(SipLib.Body.ContentTypes.Pidf);
        if (strPresence != null)
        {   // Location by value was sent
            Presence presence = XmlHelper.DeserializeFromString<Presence>(strPresence);
            if (presence != null)
                AddNewLocation(presence);
            else
                SipLogger.LogError("Unable to deserialize a PIDF Presence string for location " +
                    $"by-value:\n{strPresence}");
        }

        // Check for location by reference
        foreach (SIPGeolocationHeader Sgh in InviteRequest.Header.Geolocation)
        {
            if (Sgh.GeolocationField.URI is null)
                continue;   // Not expected 

            SIPURI sghURI = Sgh.GeolocationField.URI;
            if (sghURI.Scheme == SIPSchemesEnum.http || sghURI.Scheme == SIPSchemesEnum.https)
            {   // Do a HELD request to get the dispatch location.
                GetLocationByReference(sghURI);
            }
            else if (sghURI.Scheme == SIPSchemesEnum.sip || sghURI.Scheme == SIPSchemesEnum.sips)
            {   // Send a SUBSCRIBE request for the Presence event package. See RFC 3856 and RFC 6442
                StartPresenceSubscription(sghURI);
            }
        }

        // ServiceInfo by-value
        GetAdditionalDataByValue(SipLib.Body.ContentTypes.ServiceInfo, typeof(ServiceInfoType), SetServiceInfo);

        // ServiceInfo by-reference
        SIPCallInfoHeader? serviceInfoCallInfo = SipUtils.GetCallInfoHeaderForPurpose(InviteRequest.Header,
            PurposeTypes.ServiceInfo, SIPSchemesEnum.cid);
        if (serviceInfoCallInfo != null)
            GetAdditionalDataByReference(serviceInfoCallInfo, m_Certificate, PurposeTypes.ServiceInfo,
                Ng911Lib.Utilities.ContentTypes.ServiceInfo, typeof(ServiceInfoType), SetServiceInfo);

        // Subscriber Info by-value
        GetAdditionalDataByValue(SipLib.Body.ContentTypes.SubscriberInfo, typeof(SubscriberInfoType), SetSubscriberInfo);

        // SubscriberInfo by-reference
        SIPCallInfoHeader? subCallInfo = SipUtils.GetCallInfoHeaderForPurpose(InviteRequest.Header,
            PurposeTypes.SubscriberInfo, SIPSchemesEnum.cid);
        if (subCallInfo != null)
            GetAdditionalDataByReference(subCallInfo, m_Certificate, PurposeTypes.SubscriberInfo,
                Ng911Lib.Utilities.ContentTypes.SubscriberInfo, typeof(SubscriberInfoType), SetSubscriberInfo);

        // Provider Info by-value -- There may be multiple ProviderInfo blocks
        List<MessageContentsContainer> contentsContainers = InviteRequest.GetBodyContents();
        foreach (MessageContentsContainer container in contentsContainers)
        {
            if (container.ContentType == SipLib.Body.ContentTypes.ProviderInfo)
            {
                string? strProvider = container.StringContents;
                if (strProvider != null)
                {
                    ProviderInfoType? Pit = XmlHelper.DeserializeFromString<ProviderInfoType>(strProvider);
                    if (Pit != null)
                    {
                        if (string.IsNullOrEmpty(Pit.ProviderID) == false)
                            Providers.TryAdd(Pit.ProviderID, Pit);
                        else
                            SipLogger.LogError($"The ProviderID of a ProviderInfoType is null or empty:\n{strProvider}");
                    }
                    else
                        SipLogger.LogError($"Unable to deserialize ProviderInfo by-value:\n{strProvider}");
                }
            }
        }

        // Provider Info by-reference  -- There may be multiple ProviderInfo blocks
        foreach (SIPCallInfoHeader Sch in InviteRequest.Header.CallInfo)
        {
            string? strPurpose = Sch.CallInfoField.Parameters.Get("purpose");

            if (string.IsNullOrEmpty(strPurpose) == false && strPurpose == PurposeTypes.ProviderInfo &&
                Sch.CallInfoField.URI is not null && Sch.CallInfoField.URI.Scheme != SIPSchemesEnum.cid)
            {
                GetAdditionalDataByReference(Sch, m_Certificate, PurposeTypes.ProviderInfo,
                    Ng911Lib.Utilities.ContentTypes.ProviderInfo, typeof(ProviderInfoType), SetProviderInfo);
            }
        }

        // DeviceInfo by-value
        GetAdditionalDataByValue(SipLib.Body.ContentTypes.DeviceInfo, typeof(DeviceInfoType), SetDeviceInfo);

        // DeviceInfo by-reference
        SIPCallInfoHeader? deviceCallInfo = SipUtils.GetCallInfoHeaderForPurpose(InviteRequest.Header,
            PurposeTypes.DeviceInfo, SIPSchemesEnum.cid);
        if (deviceCallInfo != null)
            GetAdditionalDataByReference(deviceCallInfo, m_Certificate, PurposeTypes.DeviceInfo,
                Ng911Lib.Utilities.ContentTypes.DeviceInfo, typeof(DeviceInfoType), SetDeviceInfo);

        // Comments by-value -- There may be multiple blocks in the body
        List<MessageContentsContainer> commentsContainers = InviteRequest.GetBodyContents();
        foreach (MessageContentsContainer container in contentsContainers)
        {
            if (container.ContentType == SipLib.Body.ContentTypes.Comment)
            {
                if (container.StringContents != null)
                {
                    CommentType comment = XmlHelper.DeserializeFromString<CommentType>(container.StringContents);
                    if (comment != null)
                        Comments.Add(comment);
                    else
                        SipLogger.LogError($"Unable to deserialize CommentType by-value:\n{container.StringContents}");
                }
            }
        }

        // Comments by-reference -- There may be multiple
        foreach (SIPCallInfoHeader Sch in InviteRequest.Header.CallInfo)
        {
            string? strPurpose = Sch.CallInfoField.Parameters.Get("purpose");

            if (string.IsNullOrEmpty(strPurpose) == false && strPurpose == PurposeTypes.Comment &&
                Sch.CallInfoField.URI is not null && Sch.CallInfoField.URI.Scheme != SIPSchemesEnum.cid)
            {
                GetAdditionalDataByReference(Sch, m_Certificate, PurposeTypes.Comment,
                    Ng911Lib.Utilities.ContentTypes.Comment, typeof(CommentType), SetComment);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool RefreshLocationByReference()
    {
        if (InviteRequest == null)
            return false;

        bool HasLocationByReference = false;
        foreach (SIPGeolocationHeader Sgh in InviteRequest.Header.Geolocation)
        {
            if (Sgh.GeolocationField.URI is null)
                continue;   // Not expected 

            SIPURI sghURI = Sgh.GeolocationField.URI;
            if (sghURI.Scheme == SIPSchemesEnum.http || sghURI.Scheme == SIPSchemesEnum.https)
            {   // Do a HELD request to get the dispatch location.
                HasLocationByReference = true;
                GetLocationByReference(sghURI);
            }
        }

        return HasLocationByReference;
    }

    /// <summary>
    /// Does a HELD request to get the dispatch location.
    /// </summary>
    /// <param name="httpUri">The HTTP or HTTPS URI to send the HELD request to.</param>
    private void GetLocationByReference(SIPURI httpUri)
    {
        LocationRequest locRequest = new LocationRequest();
        locRequest.responseTime = "emergencyDispatch";
        locRequest.locationType = new LocationType();
        locRequest.locationType.exact = false;
        locRequest.locationType.Value.Add("any");
        locRequest.device = new HeldDevice();
        locRequest.device.uri = InviteRequest!.Header!.From!.FromURI!.ToParameterlessString();
        string strLocReq = XmlHelper.SerializeToString(locRequest);

        Task.Factory.StartNew(async () =>
        {
            string queryId = Guid.NewGuid().ToString();
            if (m_EventLoggingSettings.EnableLogging == true)
            {
                LocationQueryLogEvent Lqle = new LocationQueryLogEvent();
                SetLogEventParams(Lqle);
                Lqle.uri = httpUri.ToString();
                Lqle.text = strLocReq;
                Lqle.queryId = queryId;
                Lqle.direction = "outgoing";
                m_I3LogEventClientMgr.SendLogEvent(Lqle);
            }

            AsyncHttpRequestor Ahr = new AsyncHttpRequestor(m_Certificate, 10000, null);
            HttpResults results = await Ahr.DoRequestAsync(HttpMethodEnum.POST, httpUri.ToString(),
                Ng911Lib.Utilities.ContentTypes.Held, strLocReq, true);
            if (results.StatusCode == HttpStatusCode.OK)
            {
                if (results.Body != null && results.ContentType != null && results.ContentType ==
                    Ng911Lib.Utilities.ContentTypes.Held)
                {
                    LocationResponse Lr = XmlHelper.DeserializeFromString<LocationResponse>(results.Body);
                    if (Lr != null && Lr.presence != null)
                        AddNewLocation(Lr.presence);
                    else
                        SipLogger.LogError($"Unable to deserialize LocationResponse:\n{results.Body}");
                }
            }

            if (m_EventLoggingSettings.EnableLogging == true)
            {
                LocationResponseLogEvent Lrle = new LocationResponseLogEvent();
                SetLogEventParams(Lrle);
                Lrle.text = results.Body;
                Lrle.direction = "incoming";
                Lrle.responseStatus = ((int) results.StatusCode).ToString();
                Lrle.responseId = queryId;
                m_I3LogEventClientMgr.SendLogEvent(Lrle);
            }

            Ahr.Dispose();
        });
    }

    /// <summary>
    /// Starts a subscription to the conference event package described in RFC 4575.
    /// The CallManager class will call this method if it has determined that the sender of the call is
    /// a conference aware user agent. The CallManager class will handle the NOTIFY requests and manage
    /// the subscription.
    /// </summary>
    public void StartConferenceSubscription()
    {
        SIPURI contactUri = InviteRequest!.Header.Contact![0].ContactURI!.CopyOf();
        contactUri.Parameters.RemoveAll();
        IPEndPoint remoteEndPoint = contactUri.ToSIPEndPoint()!.GetIPEndPoint();
        SIPRequest subscribe = SIPRequest.CreateBasicRequest(SIPMethodsEnum.SUBSCRIBE, contactUri,
            contactUri, null, sipTransport.SipChannel.SIPChannelContactURI, null);
        subscribe.Header.CallId = InviteRequest!.Header.CallId;
        subscribe.Header.Event = SubscriptionEvents.Conference;
        subscribe.Header.Expires = 3600;
        ConferenceSubscriber = new SubscriberData(subscribe, SubscriptionEvents.Conference, remoteEndPoint);
        ClientNonInviteTransaction Cnit = sipTransport.StartClientNonInviteTransaction(subscribe,
            remoteEndPoint, OnConferenceSubscribeTransactionComplete, 500);

    }

    /// <summary>
    /// Processes a NOTIFY request for the conference event package
    /// </summary>
    /// <param name="notifyRequest"></param>
    internal void ProcessConferenceNotify(SIPRequest notifyRequest)
    {
        string? strConferenceType = notifyRequest.GetContentsOfType(SipLib.Body.ContentTypes.ConferenceEvent);
        if (strConferenceType != null)
        {
            conferencetype ct = XmlHelper.DeserializeFromString<conferencetype>(strConferenceType);
            if (ct != null)
            {
                ConferenceInfo = ct;
                NewConferenceInfo?.Invoke(ct);
            }
            else
                SipLogger.LogError($"Failed to deserialize a conferencetype object from {notifyRequest.Header.From!.FromURI!} +" +
                    $"for Call-ID = {CallID}");
        }

        // Handle subscription termination.
        string? strSubscriptionState = notifyRequest.Header.SubscriptionState;
        if (strSubscriptionState != null && strSubscriptionState.IndexOf("terminated") >= 0)
        {   // The subscription has been terminated
            ConferenceSubscriber = null;
        }

    }

    private void StartPresenceSubscription(SIPURI geolocationSipUri)
    {
        if (geolocationSipUri.ToSIPEndPoint() is null)
            return;

        SIPRequest subscribe = SIPRequest.CreateBasicRequest(SIPMethodsEnum.SUBSCRIBE, geolocationSipUri,
            geolocationSipUri, null, sipTransport.SipChannel.SIPChannelContactURI, null);
        // For now, don't send an in-dialog request, just use the same Call-ID header to associate the
        // subscription with this call.
        subscribe.Header.CallId = InviteRequest!.Header.CallId;
        subscribe.Header.Event = SubscriptionEvents.Presence;
        subscribe.Header.Expires = 3600;    // Set a default value of one hour
        IPEndPoint? remoteEndpoint = geolocationSipUri.ToSIPEndPoint()!.GetIPEndPoint();
        PresenceSubscriber = new SubscriberData(subscribe, SubscriptionEvents.Presence, remoteEndpoint);
        ClientNonInviteTransaction Cnit = sipTransport.StartClientNonInviteTransaction(subscribe,
            remoteEndpoint, OnPresenceSubscribeTransactionComplete, 500);
    }

    private void OnConferenceSubscribeTransactionComplete(SIPRequest sipRequest, SIPResponse? sipResponse,
        IPEndPoint remoteEndPoint, SipTransport sipTransport, SipTransactionBase Transaction)
    {
        if (ConferenceSubscriber == null)
            return;

        if (sipResponse == null || sipResponse.Status != SIPResponseStatusCodesEnum.Ok)
        {   // The SUBSCRIBE request failed.
            ConferenceSubscriber = null;
            SipLogger.LogError($"Conference event SUBSCRIBE to: {remoteEndPoint}, reason = {Transaction.TerminationReason}");
            return;
        }

        ConferenceSubscriber.SubscribeRequest.Header.To!.ToTag = sipResponse.Header.To!.ToTag;
        if (sipResponse.Header.Expires != 0)
            ConferenceSubscriber.ExpiresSeconds = sipResponse.Header.Expires;
    }

    private void OnPresenceSubscribeTransactionComplete(SIPRequest sipRequest, SIPResponse? sipResponse,
        IPEndPoint remoteEndPoint, SipTransport sipTransport, SipTransactionBase Transaction)
    {
        if (PresenceSubscriber == null)
            return;

        if (sipResponse == null || sipResponse.Status != SIPResponseStatusCodesEnum.Ok)
        {   // The SUBSCRIBE request failed.
            PresenceSubscriber = null;
            SipLogger.LogError($"Presence event SUBSCRIBE to: {remoteEndPoint}, reason = {Transaction.TerminationReason}");
            return;
        }

        PresenceSubscriber.SubscribeRequest.Header.To!.ToTag = sipResponse.Header.To!.ToTag;
        if (sipResponse.Header.Expires != 0)
            PresenceSubscriber.ExpiresSeconds = sipResponse.Header.Expires;
    }

    /// <summary>
    /// Processes a NOTIFY request for the presence event subscription
    /// </summary>
    /// <param name="notifyRequest"></param>
    internal void ProcessPresenceNotify(SIPRequest notifyRequest)
    {
        string? strPresence = notifyRequest.GetContentsOfType(SipLib.Body.ContentTypes.Pidf);
        if (strPresence != null)
        {
            Presence? presence = XmlHelper.DeserializeFromString<Presence>(strPresence);
            if (presence != null)
                AddNewLocation(presence);
            else
                SipLogger.LogError($"Failed to deserialize a presence object from {notifyRequest.Header.From!.FromURI!} +" +
                    $"for Call-ID = {CallID}");
        }

        // Handle subscription termination.
        string? strSubscriptionState = notifyRequest.Header.SubscriptionState;
        if (strSubscriptionState != null && strSubscriptionState.IndexOf("terminated") >= 0)
        {   // The subscription has been terminated
            PresenceSubscriber = null;
        }
    }

    private void SetComment(object Obj)
    {
        if (Obj is CommentType)
            Comments.Add((CommentType)Obj);
    }

    private void SetProviderInfo(object Obj)
    {
        if (Obj is ProviderInfoType)
        {
            ProviderInfoType Pit = (ProviderInfoType)Obj;
            if (Pit.ProviderID != null)
                Providers.TryAdd(Pit.ProviderID, Pit);
        }
    }

    private void SetServiceInfo(object Obj)
    {
        if (Obj is ServiceInfoType)
            ServiceInfo = (ServiceInfoType)Obj;
    }

    private void SetSubscriberInfo(object Obj)
    {
        if (Obj is SubscriberInfoType)
            SubscriberInfo = (SubscriberInfoType)Obj;
    }

    private void SetDeviceInfo(object Obj)
    {
        if (Obj is DeviceInfoType)
            DeviceInfo = (DeviceInfoType)Obj;
    }

    private void GetAdditionalDataByValue(string strContentType, Type AddDataType, SetAdditionalDataDelegate SetDelegate)
    {
        if (InviteRequest == null)
            return;

        string? strContents = InviteRequest.GetContentsOfType(strContentType);
        if (strContents == null)
            return;

        object Obj = XmlHelper.DeserializeFromString(strContents, AddDataType);
        if (Obj != null)
            SetDelegate(Obj);
        else
            SipLogger.LogError($"Failed to de-serialize additional data type {AddDataType} from: \n{strContents}");
    }

    private void GetAdditionalDataByReference(SIPCallInfoHeader CallInfo, X509Certificate2 certificate, string strPurpose,
        string strContentType, Type AddDataType, SetAdditionalDataDelegate SetDelegate)
    {
        if (InviteRequest == null)
            return;

        SIPURI? uri = CallInfo?.CallInfoField?.URI;
        if (uri is not null)
        {
            Task.Factory.StartNew(async () =>
            {
                string queryId = Guid.NewGuid().ToString();
                if (m_EventLoggingSettings.EnableLogging == true)
                {
                    AdditionalDataQueryLogEvent Adqle = new AdditionalDataQueryLogEvent();
                    SetLogEventParams(Adqle);
                    Adqle.direction = "outgoing";
                    Adqle.uri = uri.ToString();
                    m_I3LogEventClientMgr.SendLogEvent(Adqle);
                }

                AsyncHttpRequestor Ahr = new AsyncHttpRequestor(certificate, 10000, null);
                HttpResults results = await Ahr.DoRequestAsync(HttpMethodEnum.GET, uri.ToString(),
                    null, null, true);
                if (results.StatusCode == HttpStatusCode.OK)
                {
                    if (results.Body != null && results.ContentType != null && results.ContentType == strContentType)
                    {
                        object Obj = XmlHelper.DeserializeFromString(results.Body, AddDataType);
                        if (Obj != null)
                            SetDelegate(Obj);
                        else
                            SipLogger.LogError($"Unable to deserialize {AddDataType} by-reference:\n{results.Body}");
                    }
                }

                if (m_EventLoggingSettings.EnableLogging == true)
                {
                    AdditionalDataResponseLogEvent Adrle = new AdditionalDataResponseLogEvent();
                    SetLogEventParams(Adrle);
                    Adrle.text = results.Body;
                    Adrle.direction = "incoming";
                    Adrle.responseStatus = ((int) results.StatusCode).ToString();
                    Adrle.responseId = queryId;
                    m_I3LogEventClientMgr.SendLogEvent(Adrle);
                }

                Ahr.Dispose();
            });
        }
    }

    /// <summary>
    /// Returns true if the call has RTT media.
    /// </summary>
    /// <returns></returns>
    public bool CallHasRtt()
    {
        bool HasRtt = false;
        foreach (RtpChannel rtpChannel in RtpChannels)
        {
            if (rtpChannel.MediaType == MediaTypes.RTT)
            {
                HasRtt = true;
                break;
            }
        }
        return HasRtt;
    }

    /// <summary>
    /// Adds a new location to the list of locations for the call
    /// </summary>
    /// <param name="presence"></param>
    internal void AddNewLocation(Presence presence)
    {
        Locations.Add(presence);
        LastLocationReceivedTime = DateTime.Now;
        NewLocation?.Invoke(presence);  // Notify anyone that is listening.
    }

    /// <summary>
    /// Sets the list of locations for the call and fires the NewLocation event for the last location (i.e. the most recent one).
    /// This method is called when one or more locations are received in an EIDO for the call.
    /// </summary>
    /// <param name="locations"></param>
    internal void SetLocations(ThreadSafeGenericList<Presence> locations)
    {
        Locations = locations;
        LastLocationReceivedTime = DateTime.Now;
        Presence[] presArray = locations.ToArray();
        if (presArray.Length > 0)
            NewLocation?.Invoke(presArray[presArray.Length - 1]);
    }

    /// <summary>
    /// Converts the call statue enumeration value to a display string
    /// </summary>
    /// <param name="callState">Input call state</param>
    /// <returns>A string appropriate for display purposed.</returns>
    public static string CallStateToString(CallStateEnum callState)
    {
        string callStateString;
        switch (callState)
        {
            case CallStateEnum.Ringing:
                callStateString = "Ringing";
                break;
            case CallStateEnum.Trying:
                callStateString = "Trying";
                break;
            case CallStateEnum.OnLine:
                callStateString = "On-Line";
                break;
            case CallStateEnum.Idle:
                callStateString = "Idle";
                break;
            case CallStateEnum.AutoAnswered:
                callStateString = "Auto-Answered";
                break;
            case CallStateEnum.OnHold:
                callStateString = "On-Hold";
                break;
            default:
                callStateString = "Unknown";
                break;
        }

        return callStateString;
    }

    /// <summary>
    /// Gets a comma deliminated list of media display names.
    /// </summary>
    /// <returns></returns>
    public string GetMediaTypeDisplayList()
    {
        //Sdp? sdp = AnsweredSdp != null ? AnsweredSdp : OfferedSdp;
        Sdp? sdp = OfferedSdp != null ? OfferedSdp : AnsweredSdp;
        if (sdp == null)
        {   // The OfferedSdp and the AnsweredSdp have not been set up yet. Use the offered SDP from the
            // Invite request
            string? strSdp = InviteRequest?.GetContentsOfType(SipLib.Body.ContentTypes.Sdp);
            if (strSdp != null)
                sdp = Sdp.ParseSDP(InviteRequest!.GetContentsOfType(SipLib.Body.ContentTypes.Sdp)!);
        }

        if (sdp != null)
        {
            List<string> mediaList = new List<string>();
            foreach (MediaDescription md in sdp.Media)
            {
                if (md.Port != 0)
                    mediaList.Add(Sdp.MediaTypeToDisplayString(md.MediaType));
            }

            // In case the media was offered but rejected
            if (AnsweredSdp != null)
            {
                foreach (MediaDescription AnswerMd in AnsweredSdp.Media)
                {
                    if (AnswerMd.Port == 0)
                        mediaList.Remove(Sdp.MediaTypeToDisplayString(AnswerMd.MediaType));
                }
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < mediaList.Count; i++)
            {
                sb.Append(mediaList[i]);
                if (i < mediaList.Count - 1)
                    sb.Append(", ");
            }

            return sb.ToString();
        }
        else
            return string.Empty;
    }

    // Event handler for the RttCharactersReceived event of the RttReceiver class.
    internal void OnRttCharactersReceived(string RxChars, string source)
    {
        RttMessages.AddReceivedMessage(source, RxChars);
    }

    /// <summary>
    /// Sends one or more text characters to the caller if the call has RTT medial
    /// </summary>
    /// <param name="TxChars"></param>
    public void SendRttMessage(string TxChars)
    {
        if (RttSender != null)
        {
            RttSender.SendMessage(TxChars);
            RttMessages.AddSentMessage("Me", TxChars);
        }
    }

    private bool m_FirstMsrpMessageSent = false;

    /// <summary>
    /// Event handler for the MsrpMessageSend event of the MsrpConnection for the call. This event is fired when the 
    /// MsrpConnection object for the call sends an MSRP message. This event handler is used only for NG9-1-1 event logging.
    /// </summary>
    /// <param name="ContentType">Content-Type header of the MSRP message that was sent.</param>
    /// <param name="Contents">Binary contents of the MSRP message that was sent.</param>
    public void OnMsrpMessageSent(string ContentType, byte[] Contents)
    {
        if (m_FirstMsrpMessageSent == true)
            return;

        m_FirstMsrpMessageSent = true;
        SendMediaStartLogEvent(MediaTypes.MSRP, MediaLabel.SentMsrp, "outgoing");
    }

    private bool m_FirstMsrpMessageReceived = false;
    /// <summary>
    /// Event handler for MsrpMessageReceived event of the MsrpConnection object for this call.
    /// </summary>
    /// <param name="ContentType">Content-Type header of the MSRP message that was received.</param>
    /// <param name="Contents">Binary contents of the MSRP message that was received</param>
    /// <param name="from">Identifies the source of the MSRP message. Not used here.</param>
    public void OnMsrpMessageReceived(string ContentType, byte[] Contents, string from)
    {
        if (m_FirstMsrpMessageReceived == false)
        {
            m_FirstMsrpMessageReceived = true;
            SendMediaStartLogEvent(MediaTypes.MSRP, MediaLabel.ReceivedMsrp, "incoming");
        }

        string strContents = Encoding.UTF8.GetString(Contents);

        if (ContentType == "text/plain")
            MsrpMessages.AddReceivedMessage("Caller", strContents);
        else if (ContentType.ToLower() == "message/cpim")
        {
            CpimMessage? cpim = CpimMessage.ParseCpimBytes(Contents);
            if (cpim != null)
            {
                string? From = cpim.From?.URI?.User;
                if (From == null) From = "Caller";
                if (cpim.Body != null)
                    strContents = Encoding.UTF8.GetString(cpim.Body);
                else
                    strContents = "Unknown";

                MsrpMessages.AddReceivedMessage(From, strContents);
            }
        }
    }

    /// <summary>
    /// Sends a text message as text/plain to the remote endpoint.
    /// </summary>
    /// <param name="message">Text message to send.</param>
    public void SendTextPlainMsrp(string message)
    {
        if (MsrpConnection == null)
            return;

        MsrpConnection.SendMsrpMessage("text/plain", Encoding.UTF8.GetBytes(message));
        MsrpMessages.AddSentMessage("Me", message);
    }

    /// <summary>
    /// Creates a new incoming call
    /// </summary>
    /// <param name="invite">INVITE request that was received</param>
    /// <param name="transaction">SIP transaction that was created for the INVITE for the call</param>
    /// <param name="transport">SIP transport that the call came in on.</param>
    /// <param name="initialState">Initial call state.</param>
    /// <param name="certificate">X.509 certificate to use for MSRP processing</param>
    /// <returns></returns>
    public static Call CreateIncomingCall(SIPRequest invite, ServerInviteTransaction transaction,
        SipTransport transport, CallStateEnum initialState, X509Certificate2 certificate, I3LogEventClientMgr i3LogEventClientMgr,
        IdentitySettings identitySettings, EventLoggingSettings eventLoggingSettings)
    {
        Call call = new Call(transport, certificate, i3LogEventClientMgr, identitySettings, eventLoggingSettings);
        call.IsIncoming = true;
        call.InviteRequest = invite;
        call.serverInviteTransaction = transaction;
        call.CallState = initialState;
        call.CallID = invite.Header!.CallId!;   // Will never be null here
        call.LastInviteSequenceNumber = invite.Header.CSeq;
        call.RemoteIpEndPoint = transaction.RemoteEndPoint;
        call.RemoteTag = invite.Header.From?.FromTag;

        return call;
    }

    /// <summary>
    /// Gets the RtpChannel for a specified media type
    /// </summary>
    /// <param name="mediaType">Media type to search for</param>
    /// <returns>Returns the RtpChannel if found or null if there is no RtpChannel for the specified media type.</returns>
    public RtpChannel? GetRtpChannelForMediaType(string mediaType)
    {
        foreach (RtpChannel rtpChannel in RtpChannels)
        {
            if (rtpChannel.MediaType == mediaType)
                return rtpChannel;
        }

        return null;
    }

    /// <summary>
    /// Gets the local port used by a RtpChannel for a specified media type. 
    /// </summary>
    /// <param name="mediaType">Media type to search for</param>
    /// <returns>Returns the local port if the media type exists for the call or 0 if it does not</returns>
    internal int GetLocalRtpPortForMediaType(string mediaType)
    {
        RtpChannel? rtpChannel = GetRtpChannelForMediaType(mediaType);
        if (rtpChannel != null)
            return rtpChannel.LocalPort;
        else
            return 0;
    }

    /// <summary>
    /// Gets the local MsrpUri if there is MSRP media for this call or null if there is no MSRP.
    /// </summary>
    /// <returns></returns>
    internal MsrpUri? GetLocalMsrpUri()
    {
        return MsrpConnection?.LocalMsrpUri;  
    }

    internal void HookMediaEvents()
    {
        foreach (RtpChannel rtpChannel in RtpChannels)
        {
            switch (rtpChannel.MediaType)
            {
                case MediaTypes.Audio:
                    rtpChannel.RtpPacketReceived += OnAudioPacketReceived;
                    rtpChannel.RtpPacketSent += OnAudioPacketSent;
                    break;
                case MediaTypes.Video:
                    rtpChannel.RtpPacketReceived += OnVideoPacketReceived;
                    rtpChannel.RtpPacketSent += OnVideoPacketSent;
                    break;
                case MediaTypes.RTT:
                    rtpChannel.RtpPacketReceived += OnRttPacketReceived;
                    rtpChannel.RtpPacketSent += OnRttPacketSent;
                    break;
            }
        }

        if (MsrpConnection != null)
        {
            // Note: The MsrpMessageReceived event is hooked externally. See OnMsrpMessageReceived.
            MsrpConnection.MsrpMessageSent += OnMsrpMessageSent;
        }
    }

    internal void UnHookRtpEvents(RtpChannel rtpChannel)
    {
        switch (rtpChannel.MediaType)
        {
            case MediaTypes.Audio:
                rtpChannel.RtpPacketReceived -= OnAudioPacketReceived;
                rtpChannel.RtpPacketSent -= OnAudioPacketSent;
                break;
            case MediaTypes.Video:
                rtpChannel.RtpPacketReceived -= OnVideoPacketReceived;
                rtpChannel.RtpPacketSent -= OnVideoPacketSent;
                break;
            case MediaTypes.RTT:
                rtpChannel.RtpPacketReceived -= OnRttPacketReceived;
                rtpChannel.RtpPacketSent -= OnRttPacketSent;
                break;
        }
    }

    private void SendMediaStartLogEvent(string mediaType, MediaLabel mediaLabel, string direction)
    {
        if (m_EventLoggingSettings.EnableLogging == false)
            return;

        MediaStartLogEvent Msle = new MediaStartLogEvent();
        SetLogEventParams(Msle);
        Msle.sdp = AnsweredSdp?.GetMediaType(mediaType)?.ToString();
        Msle.mediaLabel = new string[1];
        Msle.mediaLabel[0] = ((int)mediaLabel).ToString();
        Msle.direction = direction;
        m_I3LogEventClientMgr.SendLogEvent(Msle);
    }

    private void SendMediaEndLogEvent(MediaLabel mediaLabel)
    {
        if (m_EventLoggingSettings.EnableLogging == false)
            return;

        MediaEndLogEvent Mele = new MediaEndLogEvent();
        SetLogEventParams(Mele);
        Mele.mediaLabel = new string[1];
        Mele.mediaLabel[0] = ((int)mediaLabel).ToString();
        m_I3LogEventClientMgr?.SendLogEvent(Mele);
    }

    private void SetLogEventParams(LogEvent logEvent)
    {
        logEvent.elementId = m_IdentitySettings.ElementID;
        logEvent.agencyId = m_IdentitySettings.AgencyID;
        logEvent.agencyAgentId = m_IdentitySettings.AgentID;
        logEvent.callId = EmergencyCallIdentifier;
        logEvent.incidentId = EmergencyIncidentIdentifier;
        logEvent.callIdSip = CallID;
        logEvent.ipAddressPort = RemoteIpEndPoint?.ToString();
    }

    private bool m_FirstAudioPacketReceived = false;
    private void OnAudioPacketReceived(RtpPacket rtpPacket)
    {
        if (m_FirstAudioPacketReceived == true)
            return;

        m_FirstAudioPacketReceived = true;
        SendMediaStartLogEvent(MediaTypes.Audio, MediaLabel.ReceivedAudio, "incoming");
    }

    private bool m_FirstAudioPacketSent = false;
    private void OnAudioPacketSent(RtpPacket rtpPacket)
    {
        if (m_FirstAudioPacketSent == true)
            return;

        m_FirstAudioPacketSent = true;
        SendMediaStartLogEvent(MediaTypes.Audio, MediaLabel.SentAudio, "outgoing");
    }

    private bool m_FirstVideoPacketReceived = false;
    private void OnVideoPacketReceived(RtpPacket rtpPacket)
    {
        if (m_FirstVideoPacketReceived == true)
            return;

        m_FirstVideoPacketReceived = true;
        SendMediaStartLogEvent(MediaTypes.Video, MediaLabel.ReceivedVideo, "incoming");
    }

    private bool m_FirstVideoPacketSent = false;
    private void OnVideoPacketSent(RtpPacket rtpPacket)
    {
        if (m_FirstVideoPacketSent == true)
            return;

        m_FirstVideoPacketSent = true;
        SendMediaStartLogEvent(MediaTypes.Video, MediaLabel.SentVideo, "outgoing");
    }

    private bool m_FirstRttPacketReceived = false;
    private void OnRttPacketReceived(RtpPacket rtpPacket)
    {
        if (m_FirstRttPacketReceived == true)
            return;

        m_FirstRttPacketReceived = true;
        SendMediaStartLogEvent(MediaTypes.RTT, MediaLabel.ReceivedRTT, "incoming");
    }

    private bool m_FirstRttPacketSent = false;
    private void OnRttPacketSent(RtpPacket rtpPacket)
    {
        if (m_FirstRttPacketSent == true)
            return;

        m_FirstRttPacketSent = true;
        SendMediaStartLogEvent(MediaTypes.RTT, MediaLabel.SentRTT, "outgoing");
    }
}

/// <summary>
/// Enumeration of the possible call states
/// </summary>
public enum CallStateEnum
{
    Idle,
    Trying,
    Ringing,
    AutoAnswered,
    OnHold,
    OnLine,
    Ended,
}

