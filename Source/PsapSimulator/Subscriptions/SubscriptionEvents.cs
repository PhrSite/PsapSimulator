/////////////////////////////////////////////////////////////////////////////////////
//  File:   SubscriptionEvents.cs                                   4 Oct 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace SipLib.Subscriptions;

/// <summary>
/// Definitions of the types of subscription events for SUBSCRIBE/NOTIFY
/// </summary>
public static class SubscriptionEvents
{
    /// <summary>
    /// Event type for the SIP Presence Event Package. See RFC 3856 and RFC 6442. This event is related to a call.
    /// </summary>
    public const string Presence = "presence";

    /// <summary>
    /// Event type for the Conference Event Package. See RFC 4575. This event is related to a call.
    /// </summary>
    public const string Conference = "conference";

    /// <summary>
    /// Event type for the I3V3 Element State event. See Section 2.4.1 of NENA-STA-010.3.
    /// </summary>
    public const string ElementState = "emergency-ElementState";

    /// <summary>
    /// Event type for the I3V3 Service State event. See Section 2.4.2 of NENA-STA-010.3.
    /// </summary>
    public const string ServiceState = "emergency-ServiceState";

    /// <summary>
    /// Event type for the I3V3 Queue State Event. See Section 4.2.1.3 of NENA-STA-010.3.
    /// </summary>
    public const string QueueState = "emergency-QueueState";

    /// <summary>
    /// List of supported event packages.
    /// </summary>
    private static string[] SupportedEvents =
    {
        Presence,
        Conference,
        ElementState,
        ServiceState,
        QueueState
    };

    /// <summary>
    /// Checks to see a SIP subscription event package is supported.
    /// </summary>
    /// <param name="Event">Value of the Event header of a SUBSCRIBE request.
    /// </param>
    /// <returns>Returns true if the event package is supported or false if it is not supported.</returns>
    public static bool SubscriptionIsSupported(string Event)
    {
        if (SupportedEvents.Contains(Event))
            return true;
        else
            return false;
    }
}
