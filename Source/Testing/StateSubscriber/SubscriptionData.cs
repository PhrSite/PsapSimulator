/////////////////////////////////////////////////////////////////////////////////////
//  File:   SubscriptionData.cs                                     15 Aug 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

using SipLib.Core;

namespace SipLib.Subscriptions;

/// <summary>
/// Class for storing data about a SIP subscription.
/// </summary>
internal class SubscriptionData
{
    /// <summary>
    /// Contains the last SUBSCRIBE request that was sent
    /// </summary>
    internal SIPRequest SubscribeRequest { get; set; }

    /// <summary>
    /// Specifies the type of SIP subscription. Must be one of the values defined by the SipNotConsts class.
    /// </summary>
    internal string Event { get; private set; }

    /// <summary>
    /// Time that the last SUBSCRIBE request was sent.
    /// </summary>
    internal DateTime LastSubscribeTime { get; set; }

    internal SubscriptionStateEnum SubscriptionState { get; set; } = SubscriptionStateEnum.Idle;

    private int m_ExpiresSeconds = DEFAULT_EXPIRES_SECONDS;

    internal int LastCSeqNumber { get; set; }

    /// <summary>
    /// Gets or sets the epires time for the subscription. The expires time is time before which the
    /// subscriber must send a new SUBSCRIBE request in order for the subscription to remain active.
    /// </summary>
    public int ExpiresSeconds
    {
        get { return m_ExpiresSeconds; }
        set
        {
            if (value < 5)
                m_ExpiresSeconds = value - 1;
            else
                m_ExpiresSeconds = value - 5;
        }
    }

    private const int DEFAULT_EXPIRES_SECONDS = 60;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="eventName">Specifies the type of the event being subscribed to. Must be one of the values
    /// defined in the SubNotConsts class. For example: SubNotConsts.ElementState, SubNotConsts.ServiceState or
    /// SubNotConsts.QueueState.</param>
    /// <param name="subscribeRequest">SUBSCRIBE SIP request that will be used for this subscription.</param>
    public SubscriptionData(string eventName, SIPRequest subscribeRequest)
    {
        SubscribeRequest = subscribeRequest;
        Event = eventName;
        LastCSeqNumber = SubscribeRequest.Header.CSeq;
    }
}

internal enum SubscriptionStateEnum
{
    Idle,
    Subscribing,
    ReSubscribing,
    Subscribed,
    Terminated
}
