/////////////////////////////////////////////////////////////////////////////////////
//  File:   CallSummary.cs                                          29 Feb 24 PHR  
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator.CallManagement;

/// <summary>
/// Data class that provides a summary of the current call state of a single call.
/// </summary>
public class CallSummary
{
    /// <summary>
    /// Call-ID SIP header value for the call.
    /// </summary>
    public string CallID { get; set; } = string.Empty;

    /// <summary>
    /// User part of the URI from the From header of the incoming SIP INVITE request.
    /// </summary>
    public string From { get; set; } = string.Empty;

    /// <summary>
    /// Time that the call arrived.
    /// </summary>
    public DateTime StartTime { get; set; } = DateTime.Now;

    /// <summary>
    /// Current state of the call.
    /// </summary>
    public CallStateEnum CallState { get; set; } = CallStateEnum.Idle;

    /// <summary>
    /// SIP request URI or the SIP URI from the Route header. For example: "urn:service:sos" or 
    /// sip:911@10.1.23.4.
    /// </summary>
    public string QueueURI { get; set; } = string.Empty;

    /// <summary>
    /// If true then the call is conferenced.
    /// </summary>
    public bool Conferenced { get; set; } = false;

    /// <summary>
    /// Show a list of the media that is available for the call.
    /// </summary>
    public string CallMedia { get; set; } = string.Empty;
}

