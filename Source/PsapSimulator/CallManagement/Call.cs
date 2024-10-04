/////////////////////////////////////////////////////////////////////////////////////
//  File:   Call.cs                                                 17 Feb 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator.CallManagement;

using SipLib.Channels;
using SipLib.Core;
using SipLib.Media;
using SipLib.Msrp;
using SipLib.RealTimeText;
using SipLib.Rtp;
using SipLib.Sdp;
using SipLib.Transactions;
using SipLib.Collections;
using Pidf;

using System.Net;
using System.Text;

using SipLib.Body;
using PsapSimulator.WindowsVideo;
using AdditionalData;
using System.Security.Cryptography.X509Certificates;
using Held;
using HttpUtils;
using Ng911Lib.Utilities;
using SipLib.Logging;
using System.Collections.Concurrent;

internal delegate void SetAdditionalDataDelegate(object Obj);

/// <summary>
/// Delegate type NewLocation event of the Call class.
/// </summary>
/// <param name="newPresence">Contains the new location data</param>
public delegate void NewLocationDelegate(Presence newPresence);

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
    /// Last invite request that was received
    /// </summary>
    internal SIPRequest? InviteRequest { get; set; } = null;

    /// <summary>
    /// 200 OK resonse that was received
    /// </summary>
    internal SIPResponse? OKResponse { get; set; } = null;

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
    internal SipTransport sipTransport {  get; set; }

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
    public string? EmergencyIncidentIdentifier {  get; set; } = null;

    /// <summary>
    /// Call start time
    /// </summary>
    public DateTime CallStartTime = DateTime.Now;

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
    /// 
    /// </summary>
    internal AudioSource? AudioSampleSource = null;

    /// <summary>
    /// 
    /// </summary>
    internal IAudioSampleSource? CurrentAudioSampleSource = null;

    internal AudioDestination? AudioDestination = null;

    internal RttSender? RttSender = null;

    internal RttReceiver? RttReceiver = null;

    internal VideoSender? VideoSender = null;
    internal IVideoCapture? CurrentVideoCapture = null;

    public VideoReceiver? VideoReceiver = null;
    
    internal DateTime LastRttAutoAnsweredSentTime = DateTime.MinValue;

    internal DateTime LastRttOnHoldSentTime = DateTime.MinValue;

    /// <summary>
    /// Contains a list of locations that have been received.
    /// </summary>
    public ThreadSafeGenericList<Presence> Locations { get; private set; } = new ThreadSafeGenericList<Presence>();

    /// <summary>
    /// This event is fired when new location for the call is received.
    /// </summary>
    public event NewLocationDelegate? NewLocation = null;

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

    private Call(SipTransport transport)
    {
        sipTransport = transport;
    }

    /// <summary>
    /// Call this method when the call has ended to terminate all call related operations and release
    /// any resources being held by the call.
    /// </summary>
    public void EndCall()
    {

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
    /// <param name="certificate">X.509 certificate to use for HTTPS requests</param>
    public void GetLocationAndAdditionalData(X509Certificate2 certificate)
    {
        string? strPresence = InviteRequest!.GetContentsOfType(SipLib.Body.ContentTypes.Pidf);
        if (strPresence != null)
        {   // Location by value was sent
            Presence presence = XmlHelper.DeserializeFromString<Presence>(strPresence);
            if (presence != null)
                Locations.Add(XmlHelper.DeserializeFromString<Presence>(strPresence));
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
                LocationRequest locRequest = new LocationRequest();
                locRequest.responseTime = "emergencyDispatch";
                locRequest.locationType = new LocationType();
                locRequest.locationType.exact = false;
                locRequest.locationType.Value.Add("any");
                locRequest.device = new HeldDevice();
                locRequest.device.uri = InviteRequest!.Header!.From!.FromURI!.ToParameterlessString();
                string? strLocReq = XmlHelper.SerializeToString(locRequest);
                if (strLocReq == null)
                    continue;

                Task.Factory.StartNew(async () =>
                {
                    AsyncHttpRequestor Ahr = new AsyncHttpRequestor(certificate, 10000, null);
                    HttpResults results = await Ahr.DoRequestAsync(HttpMethodEnum.POST, sghURI.ToString(),
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

                    Ahr.Dispose();
                });

            }
            else if (sghURI.Scheme == SIPSchemesEnum.sip || sghURI.Scheme == SIPSchemesEnum.sips)
            {   // TODO: Subscribe to the SIP Presence Event package

            }
        }

        // ServiceInfo by-value
        GetAdditionalDataByValue(SipLib.Body.ContentTypes.ServiceInfo, typeof(ServiceInfoType), SetServiceInfo);

        // ServiceInfo by-reference
        SIPCallInfoHeader? serviceInfoCallInfo = SipUtils.GetCallInfoHeaderForPurpose(InviteRequest.Header,
            PurposeTypes.ServiceInfo, SIPSchemesEnum.cid);
        if (serviceInfoCallInfo != null)
            GetAdditionalDataByReference(serviceInfoCallInfo, certificate, PurposeTypes.ServiceInfo, 
                Ng911Lib.Utilities.ContentTypes.ServiceInfo, typeof(ServiceInfoType), SetServiceInfo);

        // Subscriber Info by-value
        GetAdditionalDataByValue(SipLib.Body.ContentTypes.SubscriberInfo, typeof(SubscriberInfoType), SetSubscriberInfo);

        // SubscriberInfo by-reference
        SIPCallInfoHeader? subCallInfo = SipUtils.GetCallInfoHeaderForPurpose(InviteRequest.Header,
            PurposeTypes.SubscriberInfo, SIPSchemesEnum.cid);
        if (subCallInfo != null)
            GetAdditionalDataByReference(subCallInfo, certificate, PurposeTypes.SubscriberInfo, 
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
                GetAdditionalDataByReference(Sch, certificate, PurposeTypes.ProviderInfo,
                    Ng911Lib.Utilities.ContentTypes.ProviderInfo, typeof(ProviderInfoType), SetProviderInfo);
            }
        }

        // DeviceInfo by-value
        GetAdditionalDataByValue(SipLib.Body.ContentTypes.DeviceInfo, typeof(DeviceInfoType), SetDeviceInfo);

        // DeviceInfo by-reference
        SIPCallInfoHeader? deviceCallInfo = SipUtils.GetCallInfoHeaderForPurpose(InviteRequest.Header,
            PurposeTypes.DeviceInfo, SIPSchemesEnum.cid);
        if (deviceCallInfo != null)
            GetAdditionalDataByReference(deviceCallInfo, certificate, PurposeTypes.DeviceInfo, 
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
                GetAdditionalDataByReference(Sch, certificate, PurposeTypes.Comment,
                    Ng911Lib.Utilities.ContentTypes.Comment, typeof(CommentType), SetComment);
            }
        }
    }

    private void SetComment(object Obj)
    {
        if (Obj is CommentType)
            Comments.Add((CommentType) Obj);
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
                AsyncHttpRequestor Ahr = new AsyncHttpRequestor(certificate, 10000, null);
                HttpResults results = await Ahr.DoRequestAsync(HttpMethodEnum.GET, uri.ToString(),
                    null, null, true);
                if (results.StatusCode == HttpStatusCode.OK)
                {
                    if (results.Body != null && results.ContentType != null && results.ContentType ==
                        strContentType)
                    {
                        object Obj = XmlHelper.DeserializeFromString(results.Body, AddDataType);
                        if (Obj != null)
                            SetDelegate(Obj);
                        else
                            SipLogger.LogError($"Unable to deserialize {AddDataType} by-reference:\n{results.Body}");
                    }
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
            if (rtpChannel.MediaType == "text")
            {
                HasRtt = true;
                break;
            }
        }
        return HasRtt;
    }

    public void AddNewLocation(Presence presence)
    {
        Locations.Add(presence);
        NewLocation?.Invoke(presence);  // Notify anyone that is listening.
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
        Sdp? sdp = AnsweredSdp != null ? AnsweredSdp : OfferedSdp;

        if (sdp == null)
        {   // The OfferedSdp and the AnsweredSdp have not been set up yet. Use the offered SDP from the
            // Invite request
            sdp = Sdp.ParseSDP(InviteRequest!.GetContentsOfType(SipLib.Body.ContentTypes.Sdp)!);
        }

        if (sdp != null)
        {
            List<string> mediaList = new List<string>();
            foreach (MediaDescription md in sdp.Media)
                mediaList.Add(Sdp.MediaTypeToDisplayString(md.MediaType));

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
        //RttCharactersReceived?.Invoke(CallID, source, DateTime.Now, RxChars);
        RttMessages.AddReceivedMessage(source, RxChars);
    }

    public void SendRttMessage(string TxChars)
    {
        if (RttSender != null)
        {
            RttSender.SendMessage(TxChars);
            RttMessages.AddSentMessage("Me", TxChars);
        }
    }

    public void OnMsrpMessageReceived(string ContentType, byte[] Contents, string from)
    {
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
    /// 
    /// </summary>
    /// <param name="message"></param>
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
    /// <param name="invite"></param>
    /// <param name="transaction"></param>
    /// <param name="transport"></param>
    /// <param name="initialState"></param>
    /// <returns></returns>
    public static Call CreateIncomingCall(SIPRequest invite, ServerInviteTransaction transaction,
        SipTransport transport, CallStateEnum initialState)
    {
        Call call = new Call(transport);
        call.IsIncoming = true;
        call.InviteRequest = invite;
        call.serverInviteTransaction = transaction;
        call.CallState = initialState;
        call.CallID = invite.Header!.CallId!;   // Will never be null here
        call.LastInviteSequenceNumber = invite.Header.CSeq;
        call.RemoteIpEndPoint = transaction.RemoteEndPoint;

        return call;
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

