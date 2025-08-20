/////////////////////////////////////////////////////////////////////////////////////
//  File:   SubscriberData.cs                                       10 Oct 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

using SipLib.Core;
using System.Net;

namespace SipLib.Subscriptions;

/// <summary>
/// Contains data and state information about a SIP subscription for a subscriber.
/// </summary>
public class SubscriberData
{
    /// <summary>
    /// Contains the last SUBSCRIBE request
    /// </summary>
    public SIPRequest SubscribeRequest { get; set; }

    /// <summary>
    /// Specifies the type of SIP subscription. Must be one of the values defined by the SubscriptionEvents class.
    /// </summary>
    public string Event {  get; set; }

    /// <summary>
    /// Time that the last SUBSCRIBE request was sent.
    /// </summary>
    public DateTime LastSubscribeTime { get; set; }

    private int m_ExpiresSeconds = 0;

    /// <summary>
    /// Gets or sets the epires time for the subscription. The expires time is time before which the
    /// subscriber must send a new SUBSCRIBE request in order for the subscription to remain active.
    /// </summary>
    public int ExpiresSeconds
    {
        get {  return m_ExpiresSeconds; }
        set
        {
            if (value < 5)
                m_ExpiresSeconds = value - 1;
            else
                m_ExpiresSeconds = value - 5;
        }
    }

    /// <summary>
    /// Remote IP End Point to send the SUBSCRIBE request to.
    /// </summary>
    public IPEndPoint RemoteEndPoint { get; private set; }

    public SubscriberData(SIPRequest subscribeRequest, string subscribeEvent, IPEndPoint remoteEndPoint)
    {
        SubscribeRequest = subscribeRequest;
        Event = subscribeEvent;
        LastSubscribeTime = DateTime.Now;
        ExpiresSeconds = SubscribeRequest.Header.Expires;
        RemoteEndPoint = remoteEndPoint;
    }
}
