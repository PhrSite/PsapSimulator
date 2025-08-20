/////////////////////////////////////////////////////////////////////////////////////
//  File:   ElementStateSubscriptionManager.cs                      16 Jul 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace SipLib.Subscriptions;

using I3SubNot;
using SipLib.Core;
using SipLib.Transactions;

/// <summary>
/// Class that manages multiple subscribers to the Element State subscribe/notify event package.
/// </summary>
public class ElementStateSubscriptionManager : SubscriptionManager
{
    private string m_ElementID;
    private string m_CurrentState = ElementState.Normal;
    private string? m_Reason = null;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="elementID">Element ID of the element that is acting as the Element State notifier. Section 2.1.3
    /// of NENA-STA-010.3b specifies the format of this string. For example: esrp1.state.pa.us</param>
    public ElementStateSubscriptionManager(string elementID) : base()
    {
        m_ElementID = elementID;
    }

    /// <summary>
    /// Sends a NOTIFY request to each subscriber of the Element State event package. This method must be called
    /// when the Element State changes.
    /// </summary>
    /// <param name="newState">Specifies the new Element State. Must be one of the constant values defined in the 
    /// ElementState class.</param>
    /// <param name="changeReason">Specifies the reason for the change in Element State. Optional.</param>
    public void NotifyElementStateChange(string newState, string? changeReason)
    {
        m_CurrentState = newState;
        m_Reason = changeReason;
        ElementState State = new ElementState()
        {
            elementDomain = m_ElementID,
            state = m_CurrentState,
            reason = m_Reason
        };

        IEnumerable<Subscription> subs = Subscriptions.Values.ToArray<Subscription>();

        foreach (Subscription subscription in subs)
        {
            ElementStateSubscription elementStateSubscription = (ElementStateSubscription)subscription;
            elementStateSubscription.CurrentElementState = State;
            elementStateSubscription.NotifySubscriber();
        }

    }

    /// <summary>
    /// Processes a SUBSCRIBE request for the Element State subscribe/notify event package.
    /// </summary>
    /// <param name="sipRequest">SIP SUBSCRIBE request for the Element State subscribe/notify event package.</param>
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
            {   // Send a NOTIFY to tell the subscriber the current Element State
                ElementStateSubscription elementStateSubscription = (ElementStateSubscription)Subscriptions[subscriptionID];
                elementStateSubscription.LastSubscriptionTime = DateTime.Now;
                elementStateSubscription.NotifySubscriber();
            }
        }
        else
        {   // Its a SUBSCRIBE request for a new subscription
            ElementStateSubscription elementStateSubscription = new ElementStateSubscription(sipRequest, sipTransport,
                remoteEndPoint.GetIPEndPoint());
            AddSubscription(subscriptionID, elementStateSubscription);
            SIPResponse OkResponse = elementStateSubscription.BuildOkToSubscribe(sipRequest, sipTransport.SipChannel);
            sipTransport.StartServerNonInviteTransaction(sipRequest, remoteEndPoint.GetIPEndPoint(), null, OkResponse);

            elementStateSubscription.CurrentElementState = new ElementState()
            {
                elementDomain = m_ElementID,
                state = m_CurrentState,
                reason = m_Reason
            };
            elementStateSubscription.NotifySubscriber();
        }
    }
}
