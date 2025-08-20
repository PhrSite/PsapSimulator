/////////////////////////////////////////////////////////////////////////////////////
//  File:   SubscriptionEvents.cs                                   4 Oct 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace SipLib.Subscriptions;
using I3SubNot;

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
    /// List of supported event packages.
    /// </summary>
    private static string[] SupportedEvents =
    {
        Presence,
        Conference,
        ElementState.EventName,
        ServiceState.EventName,
        QueueState.EventName
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
