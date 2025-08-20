/////////////////////////////////////////////////////////////////////////////////////
//  File:   QueueStateSubscription.cs                               31 Jul 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

using SipLib.Core;
using SipLib.Transactions;
using System.Net;
using I3SubNot;

namespace SipLib.Subscriptions;

/// <summary>
/// Class for handling a Queue State subscription from a subscriber. See Section 4.2.1.3 of NENA-STA-010.3b for a
/// description of the Queue State subscribe/notify event package.
/// </summary>
public class QueueStateSubscription : Subscription
{
    private QueueState m_CurrentQueueState;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="request">Initial SIP SUBSCRIBE request received from the subscriber.</param>
    /// <param name="sipTransport">SipTransport that the SUBSCRIBE request was received on.</param>
    /// <param name="remoteEndPoint">IPEndPoint that sent the SUBSCRIBE request.</param>
    /// <param name="queueUri">URI that identifies the queue. This is typically from the request URI of the
    /// initial SUBSCRIBE request.</param>
    /// <param name="maxQueueLength">Maximum number of calls that the specified queue can handle.</param>
    public QueueStateSubscription(SIPRequest request, SipTransport sipTransport, IPEndPoint remoteEndPoint,
        string queueUri, int maxQueueLength) : base(request, sipTransport, remoteEndPoint)
    {
        Event = QueueState.EventName;
        m_CurrentQueueState = new QueueState();
        m_CurrentQueueState.queueUri = queueUri;
        m_CurrentQueueState.queueMaxLength = maxQueueLength;
        m_CurrentQueueState.state = QueueState.Active;
        m_CurrentQueueState.queueLength = 0;
    }

    /// <summary>
    /// Gets or sets the current Queue State. Must be one of the constant values defined in the QueueState class.
    /// </summary>
    public string CurrentQueueState
    {
        get { return m_CurrentQueueState.state; }
        set { m_CurrentQueueState.state = value; }
    }

    /// <summary>
    /// Gets or sets the current queue length -- which is the number of calls currently in the queue.
    /// </summary>
    public int QueueLength
    {
        get { return m_CurrentQueueState.queueLength; }
        set { m_CurrentQueueState.queueLength= value; }
    }

    /// <summary>
    /// Sends a NOTIFY request to the subscriber containing the current queue state information.
    /// </summary>
    public void NotifySubscriber()
    {
        SendNotifyRequest("active", Ng911Lib.Utilities.ContentTypes.QueueState, m_CurrentQueueState);
    }


}
