/////////////////////////////////////////////////////////////////////////////////////
//  File:   Subscription.cs                                         15 Jul 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

using Ng911Lib.Utilities;
using SipLib.Body;
using SipLib.Channels;
using SipLib.Core;
using SipLib.Transactions;
using System.Net;

namespace SipLib.Subscriptions;

internal delegate void NotifyFailedDelegate(string subscriptionID);

/// <summary>
/// Base class for subscription classes such as ElementStateSubscription, ServiceStateSubscription and 
/// QueueStateSubscription. These classes implement the notifier side of the NG9-1-1 subscribe/notify interfaces.
/// </summary>
public class Subscription
{
    private SipTransport m_SipTransport;
    private string m_LocalTag = string.Empty;
    private string m_RemoteTag = string.Empty;

    /// <summary>
    /// Subscription name that is used for the SIP SUBSCRIBE request's Event header field.
    /// </summary>
    protected string Event = string.Empty;

    private const int DEFAULT_EXPIRES_SECONDS = 3600;

    /// <summary>
    /// The last time that the subscriber sent a SUBSCRIBE request
    /// </summary>
    public DateTime LastSubscriptionTime = DateTime.Now;

    /// <summary>
    /// Subscription expiration time in seconds.
    /// </summary>
    public int ExpiresSeconds = DEFAULT_EXPIRES_SECONDS;

    private SIPRequest m_SubscribeRequest;
    private int m_NotifyCSeqNum = 1;
    private IPEndPoint m_RemoteEndPoint;

    /// <summary>
    /// ID of the subscription. Taken from the Call-ID of the SUBSCRIBE request.
    /// </summary>
    public string SubscriptionID;

    /// <summary>
    /// Event that is fired when a NOTIFY request transaction fails.
    /// </summary>
    internal event NotifyFailedDelegate? NotifyFailed = null;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="request">SUBSCRIBE SIP request</param>
    /// <param name="sipTransport">SipTransport that the SUBSCRIBE request was received on.</param>
    /// <param name="remoteEndPoint">IPEndPoint that sent the SUBSCRIBE request.</param>
    public Subscription(SIPRequest request, SipTransport sipTransport, IPEndPoint remoteEndPoint)
    {
        m_SipTransport = sipTransport;
        m_SubscribeRequest = request;
        m_RemoteTag = m_SubscribeRequest.Header.From!.FromTag!;
        m_LocalTag = CallProperties.CreateNewTag();
        Event = m_SubscribeRequest.Header.Event!;
        SubscriptionID = request.Header.CallId;

        if (m_SubscribeRequest.Header.Expires > 0)
        {   // The client is requesting an expiration interval
            if (m_SubscribeRequest.Header.Expires < ExpiresSeconds)
                ExpiresSeconds = m_SubscribeRequest.Header.Expires;
        }

        m_RemoteEndPoint = remoteEndPoint;
    }

    /// <summary>
    /// Builds a 200 OK SIP response to a SUBSCRIBE request.
    /// </summary>
    /// <param name="subscribeRequest">SIP SUBSCRIBE request to build the response for.</param>
    /// <param name="SipChan">SIPChannel to use for building the SIP response. Must be taken from the SipTransport used for
    /// the subscribe/notify dialog.</param>
    /// <returns>Returns a SIPResponse object that can be sent to the subscriber.</returns>
    internal SIPResponse BuildOkToSubscribe(SIPRequest subscribeRequest, SIPChannel SipChan)
    {
        SIPResponse okResponse = new SIPResponse(SIPResponseStatusCodesEnum.Ok, "OK", subscribeRequest.LocalSIPEndPoint!);
        okResponse.Header = new SIPHeader(new SIPContactHeader(null,
            new SIPURI(subscribeRequest.URI!.Scheme, SipChan.SIPChannelEndPoint)), subscribeRequest.Header.From!, 
            subscribeRequest.Header.To!, subscribeRequest.Header.CSeq, subscribeRequest.Header.CallId);
        okResponse.Header.Contact = new List<SIPContactHeader>();
        okResponse.Header.Contact.Add(new SIPContactHeader(null, SipChan.SIPChannelContactURI));
        okResponse.Header.To!.ToTag = m_LocalTag;

        okResponse.Header.CSeqMethod = subscribeRequest.Header.CSeqMethod;
        okResponse.Header.Vias = subscribeRequest.Header.Vias;
        // Don't send a MaxForwards header
        okResponse.Header.MaxForwards = int.MinValue;
        okResponse.Header.RecordRoutes = subscribeRequest.Header.RecordRoutes;
        okResponse.Header.Expires = ExpiresSeconds;

        return okResponse;
    }

    /// <summary>
    /// Sends a SIP NOTIFY request to the subscriber.
    /// </summary>
    /// <param name="subscriptionstate">Current state of the subscription. Must be one of the values specified in
    /// Section 4.2.2 of RFC 6665 (active, pending or terminated).</param>
    /// <param name="contentType">Specifies the Content-Type header of the NOTIFY request. Optional. If null, then
    /// not body will be sent in the NOTIFY request. Must be one of the constant values defined by the NotifyContents
    /// class.</param>
    /// <param name="bodyObj">Object to serialize to a JSON string for the body of the NOTIFY request. Optional. If
    /// null, then no body will be sent in the NOTIFY request.</param>
    internal void SendNotifyRequest(string subscriptionstate, string? contentType, object? bodyObj)
    {
        SIPURI ruri = m_SubscribeRequest.Header.Contact![0].ContactURI!;
        SIPURI fromUri = m_SipTransport.SipChannel.SIPChannelContactURI!;
        SIPRequest notify = SIPRequest.CreateBasicRequest(SIPMethodsEnum.NOTIFY, ruri, ruri, null, fromUri, null);

        // NOTIFY requests must be in-dialog
        notify.Header.CallId = m_SubscribeRequest.Header.CallId;
        notify.Header.From!.FromTag = m_LocalTag;
        notify.Header.To!.ToTag = m_RemoteTag;
        notify.Header.CSeq = m_NotifyCSeqNum++;
        notify.Header.Event = Event;
        notify.Header.SubscriptionState = subscriptionstate;

        // Set the Expires header value to the time remaining time for the subscription
        notify.Header.Expires = (int) (ExpiresSeconds - (DateTime.Now - LastSubscriptionTime).TotalSeconds);

        if (contentType != null && bodyObj != null)
        {
            SipBodyBuilder Sbb = new SipBodyBuilder();
            string strBody = JsonHelper.SerializeToString(bodyObj);
            Sbb.AddContent(contentType, strBody, null, null);
            Sbb.AttachMessageBody(notify);
        }

        m_SipTransport.StartClientNonInviteTransaction(notify, m_RemoteEndPoint, OnNotifyTransactionComplete, 1000);
    }

    /// <summary>
    /// Callback delegate that is called when a NOTIFY request has been completed.
    /// </summary>
    /// <param name="sipRequest"></param>
    /// <param name="sipResponse"></param>
    /// <param name="remoteEndPoint"></param>
    /// <param name="sipTransport"></param>
    /// <param name="Transaction"></param>
    private void OnNotifyTransactionComplete(SIPRequest sipRequest, SIPResponse? sipResponse, IPEndPoint remoteEndPoint, 
        SipTransport sipTransport, SipTransactionBase Transaction)
    {
        if (sipResponse == null || sipResponse.StatusCode >= 400)
        {   // The NOTIFY request timed out or was rejected
            NotifyFailed?.Invoke(sipRequest.Header.CallId);
        }
    }

}
