/////////////////////////////////////////////////////////////////////////////////////
//  File:   CallCounts.cs                                           26 Mar 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator.CallManagement;

/// <summary>
/// Data class for retrieving the call counts from the CallManager class.
/// </summary>
public class CallCounts
{
    /// <summary>
    /// Total number of calls
    /// </summary>
    /// <value></value>
    public int TotalCalls { get; internal set; } = 0;

    /// <summary>
    /// Number of calls that in the Ringing or Trying states
    /// </summary>
    /// <value></value>
    public int Ringing { get; internal set; } = 0;

    /// <summary>
    /// Number of calls that are in the AutoAnswered state
    /// </summary>
    /// <value></value>
    public int AutoAnswered { get; internal set; } = 0;

    /// <summary>
    /// Number of calls that are in the OnHold stae
    /// </summary>
    /// <value></value>
    public int OnHold { get; internal set; } = 0;

    /// <summary>
    /// Number of calls that are in the OnLine state
    /// </summary>
    /// <value></value>
    public int OnLine { get; internal set; } = 0;
}