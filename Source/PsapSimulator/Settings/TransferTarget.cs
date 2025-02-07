/////////////////////////////////////////////////////////////////////////////////////
//  File:   TransferTarget.cs                                       21 Jan 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator.Settings;

/// <summary>
/// Configuration settings for a single transfer targed.
/// </summary>
public class TransferTarget
{
    /// <summary>
    /// Display name for the transfer. Required. Must be unique.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// SIP URI for the transfer target. Required.
    /// </summary>
    public string SipUri { get; set; } = string.Empty;
}
