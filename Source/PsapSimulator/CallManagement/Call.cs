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

using System.Diagnostics;
using SipLib.Body;
using System.Security.Permissions;
using PsapSimulator.WindowsVideo;

/// <summary>
/// Delegate definition for the RttCharactersReceive event of the Call class and the CallManager class.
/// </summary>
/// <param name="callID"></param>
/// <param name="From"></param>
/// <param name="TimeReceived"></param>
/// <param name="RttChars"></param>
public delegate void RttCharactersReceivedDelegate(string callID, string From, DateTime TimeReceived,
    string RttChars);

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
    /// Last invite request that was sent or received
    /// </summary>
    internal SIPRequest? InviteRequest { get; set; } = null;

    /// <summary>
    /// 200 OK resonse that was sent or received
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

    public event RttCharactersReceivedDelegate? RttCharactersReceived = null;

    /// <summary>
    /// Contains a list of locations that have been received.
    /// </summary>
    public ThreadSafeGenericList<Presence> Locations { get; private set; } = new ThreadSafeGenericList<Presence>();

    private Call(SipTransport transport)
    {
        sipTransport = transport;
    }

    /// <summary>
    /// Gets the display data for the call
    /// </summary>
    /// <returns></returns>
    public CallData GetCallData()
    {
        CallData callData = new CallData();
        callData.CallID = CallID;
        callData.From = InviteRequest!.Header!.From!.FromURI!.User!;
        callData.CallStateString = CallStateToString(CallState);
        callData.MediaTypes = GetMediaTypeDisplayList();

        return callData;
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

    public void OnMsrpMessageReceived(string ContentType, byte[] Contents)
    {
        // For debug only
        string strContents = Encoding.UTF8.GetString(Contents);
        Debug.WriteLine(strContents);
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

