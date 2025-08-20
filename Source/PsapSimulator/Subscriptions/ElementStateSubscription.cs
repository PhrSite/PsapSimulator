/////////////////////////////////////////////////////////////////////////////////////
//  File:   ElementStateSubscription.cs                             15 Jul 15 PHR
/////////////////////////////////////////////////////////////////////////////////////

using I3SubNot;
using SipLib.Core;
using SipLib.Transactions;
using System.Net;

namespace SipLib.Subscriptions;

/// <summary>
/// Class for handling an Element State subscription from a subscriber. See Section 2.4.1 of NENA-STA-010.3b for a
/// description of the Element State subscribe/notify event package.
/// </summary>
public class ElementStateSubscription : Subscription
{
    private ElementState m_CurrentElementState = new ElementState(ElementState.Normal, "DefaultDomain");

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="request">Initial SIP SUBSCRIBE request received from the subscriber.</param>
    /// <param name="sipTransport">SipTransport that the SUBSCRIBE request was received on.</param>
    /// <param name="remoteEndPoint">IPEndPoint that sent the SUBSCRIBE request.</param>
    public ElementStateSubscription(SIPRequest request, SipTransport sipTransport, IPEndPoint remoteEndPoint) : 
        base(request, sipTransport, remoteEndPoint)
    {
        Event = ElementState.EventName;
    }

    /// <summary>
    /// Gets or sets the current Element State.
    /// </summary>
    public ElementState CurrentElementState
    { 
        get { return m_CurrentElementState; }
        set { m_CurrentElementState = value; }
    }

    /// <summary>
    /// Sends a NOTIFY request to the subscriber containing the current element state.
    /// </summary>
    public void NotifySubscriber()
    {
        SendNotifyRequest("active", Ng911Lib.Utilities.ContentTypes.ElementState, CurrentElementState);
    }
}
