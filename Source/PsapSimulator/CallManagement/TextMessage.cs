/////////////////////////////////////////////////////////////////////////////////////
//  File:   TextMessage.cs                                          23 Mar 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator.CallManagement;

/// <summary>
/// Data class for storing MSRP or RTT text message
/// </summary>
public class TextMessage
{
    /// <summary>
    /// Identifies the sender of the text message
    /// </summary>
    public string From { get; set; } = string.Empty;

    /// <summary>
    /// Contents of the text message
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Time that the text message was sent or received
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// Source of the text message
    /// </summary>
    public TextSourceEnum Source { get; set; } = TextSourceEnum.Unknown;
}
