/////////////////////////////////////////////////////////////////////////////////////
//  File:   CallManagerDelegates.cs                                 29 Feb 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator.CallManagement;

public class CallSummary
{
    public string CallID { get; set; } = string.Empty;

    public string From { get; set; } = string.Empty;

    public DateTime StartTime { get; set; } = DateTime.Now;

    public CallStateEnum CallState { get; set; } = CallStateEnum.Idle;

    public string QueueURI {  get; set; } = string.Empty;

    public bool Conferenced { get; set; } = false;

    public string CallMedia {  get; set; } = string.Empty;
}

public delegate void CallStateDelegate(CallSummary callSummary);

public delegate void CallEndedDelegate(string CallID);
