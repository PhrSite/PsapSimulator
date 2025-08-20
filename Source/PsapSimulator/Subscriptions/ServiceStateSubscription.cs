/////////////////////////////////////////////////////////////////////////////////////
//  File:   ServiceStateSubscription.cs                             31 Jul 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

using SipLib.Core;
using SipLib.Transactions;
using System.Net;
using I3SubNot;

namespace SipLib.Subscriptions;

/// <summary>
/// Class for handling a Service State subscription from a subscriber. See Section 2.4.2 of NENA-STA-010.3b for a
/// description of the Service State subscribe/notify event package.
/// </summary>
public class ServiceStateSubscription : Subscription
{
    private ServiceState m_CurrentServiceState;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="request">Initial SIP SUBSCRIBE request received from the subscriber.</param>
    /// <param name="sipTransport">SipTransport that the SUBSCRIBE request was received on.</param>
    /// <param name="remoteEndPoint">IPEndPoint that sent the SUBSCRIBE request.</param>
    public ServiceStateSubscription(SIPRequest request, SipTransport sipTransport, IPEndPoint remoteEndPoint) :
        base(request, sipTransport, remoteEndPoint)
    {
        Event = ServiceState.EventName;
        m_CurrentServiceState = new ServiceState();
    }

    /// <summary>
    /// Gets or sets the current service state.
    /// </summary>
    public ServiceState CurrentServiceState
    {
        get { return m_CurrentServiceState; }
        set { m_CurrentServiceState = value; }
    }

    /// <summary>
    /// Sends a NOTIFY request to the subscriber containing the current service state.
    /// </summary>
    public void NotifySubscriber()
    {
        SendNotifyRequest("active", Ng911Lib.Utilities.ContentTypes.ServiceState, CurrentServiceState);
    }
}
