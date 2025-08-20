/////////////////////////////////////////////////////////////////////////////////////
//  File:   QueueStateSubscriptionManager.cs                        31 Jul 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

using SipLib.Core;
using SipLib.Transactions;
using I3SubNot;

namespace SipLib.Subscriptions;

/// <summary>
/// Class that manages multiple subscribers to the Queue State subscribe/notify event package for a single call
/// queue. If the application must manage Queue State for multiple call queues, then it must create an instance of
/// this class for each call queue and route SUBSCRIBE requests to the appropriate class instance.
/// </summary>
public class QueueStateSubscriptionManager : SubscriptionManager
{
    private string m_CurrentQueueState = QueueState.Active;
    private int m_MaxQueueLength;
    private int m_QueueLength = 0;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="maxQueueLength">Maximum number of calls that the specified queue can handle.</param>
    public QueueStateSubscriptionManager(int maxQueueLength) : base()
    {
        m_MaxQueueLength = maxQueueLength;
    }

    /// <summary>
    /// Processes a SUBSCRIBE request for the Queue State subscribe/notify event package.
    /// </summary>
    /// <param name="sipRequest">SIP SUBSCRIBE request for the Queue State subscribe/notify event package.</param>
    /// <param name="remoteEndPoint">SIPEndPoint that sent the SUBSCRIBE request.</param>
    /// <param name="sipTransport">SipTransport that the SUBSCRIBE request was sent on.</param>
    public void ProcessSubscribeRequest(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransport)
    {
        string subscriptionID = sipRequest.Header.CallId;
        if (Subscriptions.ContainsKey(subscriptionID) == true)
        {   // Its an existing subscription
            SIPResponse OkResponse = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.Ok, "OK", sipTransport.SipChannel, null);
            sipTransport.StartServerNonInviteTransaction(sipRequest, remoteEndPoint.GetIPEndPoint(), null, OkResponse);

            if (sipRequest.Header.Expires == 0)
            {   // The subscriber is requesting to terminate the subscription
                RemoveSubscription(subscriptionID);
            }
            else
            {   // Send a NOTIFY to tell the subscriber the current Queue State
                QueueStateSubscription queueStateSubscription = (QueueStateSubscription)Subscriptions[subscriptionID];
                queueStateSubscription.LastSubscriptionTime = DateTime.Now;
                queueStateSubscription.CurrentQueueState = m_CurrentQueueState;
                queueStateSubscription.QueueLength = m_QueueLength;
                queueStateSubscription.NotifySubscriber();
            }
        }
        else
        {   // Its a SUBSCRIBE request for a new subscription
            QueueStateSubscription queueStateSubscription = new QueueStateSubscription(sipRequest, sipTransport,
                remoteEndPoint.GetIPEndPoint(), sipRequest.GetQueueUri(), m_MaxQueueLength);
            AddSubscription(subscriptionID, queueStateSubscription);

            SIPResponse OkResponse = queueStateSubscription.BuildOkToSubscribe(sipRequest, sipTransport.SipChannel);
            sipTransport.StartServerNonInviteTransaction(sipRequest, remoteEndPoint.GetIPEndPoint(), null, OkResponse);
            queueStateSubscription.CurrentQueueState = m_CurrentQueueState;
            queueStateSubscription.QueueLength = m_QueueLength;
            queueStateSubscription.NotifySubscriber();
        }
    }

    /// <summary>
    /// Sends a NOTIFY request to each subscriber of the Queue State event package. This method must be called
    /// when the Queue State changes.
    /// </summary>
    /// <param name="queueState">Specifies the new Queue State. Must be one of the constant values defined by the
    /// QueueState class. For example: QueueState.Active.</param>
    /// <param name="queueLength">Current number of calls in the call queue.</param>
    public void NotifyQueueStateChange(string queueState, int queueLength)
    {
        m_CurrentQueueState = queueState;
        m_QueueLength = queueLength;

        IEnumerable<Subscription> subs = Subscriptions.Values.ToArray<Subscription>();

        foreach (Subscription subscription in subs)
        {
            QueueStateSubscription queueStateSubscription = (QueueStateSubscription)subscription;
            queueStateSubscription.CurrentQueueState = queueState;
            queueStateSubscription.QueueLength = queueLength;
            queueStateSubscription.NotifySubscriber();
        }
    }
}
