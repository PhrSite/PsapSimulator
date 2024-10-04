/////////////////////////////////////////////////////////////////////////////////////
//  File:   NotifyContents.cs                                       4 Oct 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace SipLib.Subscriptions;

/// <summary>
/// Definitions of the Content-Type header value of NOTIFY requests for the Subscribe/Notify packages
/// that are supported.
/// </summary>
public static class NotifyContents
{
    /// <summary>
    /// Content-Type header value for the presence event.
    /// </summary>
    public const string Presence = "application/pidf+xml";

    /// <summary>
    /// Content-Type header value for the conference event.
    /// </summary>
    public const string Conference = "application/conference+xml";

    /// <summary>
    /// Content-Type for the NOTIFY body for the Element State Event.
    /// </summary>
    public const string ElementState = "application/EmergencyCallData.ElementState+json";

    /// <summary>
    /// Content-Type for the NOTIFY body for the Service State Event.
    /// </summary>
    public const string ServiceState = "application/EmergencyCallData.ServiceState+json";

    /// <summary>
    /// Content-Type for the NOTIFY body for the Queue State Event
    /// </summary>
    public const string QueueState = "application/EmergencyCallData.queuestate+json";

}
