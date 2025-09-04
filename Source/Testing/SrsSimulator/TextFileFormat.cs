/////////////////////////////////////////////////////////////////////////////////////
//  File:   TextFileFormat.cs                                       18 Nov 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

using Ng911Lib.Utilities;

namespace SrsSimulator;

/// <summary>
/// Class for storing a block of text that was received via the Message Session Relay Protocol (MSRP) or the
/// Real Time Text (RTT) protocol.
/// </summary>
public class TextLine
{
    /// <summary>
    /// Timestamp that the text was received. The format must be in the NENA timestamp format as specified in
    /// Section 2.3 of NENA-STA-010.3 (yyyy-MM-ddTHH:mm:ss.fffffzzz), where fffff is the fractional seconds
    /// and zzz is the offset from GMT. For example: 2024-11-18T02:25:01.12345+7:00.
    /// </summary>
    public string Timestamp {get; set; }

    /// <summary>
    /// Identifies the sender of the text
    /// </summary>
    public string From { get; set; }

    /// <summary>
    /// Characters that were received. For MSRP this is typically a complete message. For RTT this may be
    /// one (typically) or more characters.
    /// </summary>
    public string Characters { get; set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="from">Identifies the sender of the text</param>
    /// <param name="receivedCharacters">Characters that were received. For MSRP this is typically a complete message. For RTT this may be
    /// one (typically) or more characters.</param>
    public TextLine(string from, string receivedCharacters)
    {
        Timestamp = TimeUtils.GetCurrentNenaTimestamp();
        From = from;
        Characters = receivedCharacters;
    }
}
    
/// <summary>
/// Class for storing all of the text received using MSRP or RTT.
/// </summary>
public class TextFileFormat
{
    /// <summary>
    /// Contains a list of text lines.
    /// </summary>
    public List<TextLine> TextLines { get; set; } = new List<TextLine>();
}
