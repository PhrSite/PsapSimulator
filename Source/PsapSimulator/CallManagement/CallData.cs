/////////////////////////////////////////////////////////////////////////////////////
//  File:   CallData.cs                                             21 Mar 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator.CallManagement;

/// <summary>
/// 
/// </summary>
public class CallData
{
    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    public string CallID { get; internal set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    public string From { get; internal set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    public string CallStateString { get; internal set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    public string MediaTypes { get; internal set; } = string.Empty;
}
