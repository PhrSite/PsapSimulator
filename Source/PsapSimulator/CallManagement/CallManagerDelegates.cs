/////////////////////////////////////////////////////////////////////////////////////
//  File:   CallManagerDelegates.cs                                 29 Feb 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator.CallManagement;

/// <summary>
/// Delegate type for the CallStateChanged event of the CallManager class.
/// </summary>
/// <param name="callSummary">Provides a summary of the current call state.</param>
public delegate void CallStateDelegate(CallSummary callSummary);

/// <summary>
/// Delegate type for the CallEnded event of the CallManager class.
/// </summary>
/// <param name="CallID">SIP Call-ID header value for the call.</param>
public delegate void CallEndedDelegate(string CallID);

/// <summary>
/// Delegate type for the CallManagerError event of the CallManager class.
/// </summary>
/// <param name="errMessage">Error message to display to the user.</param>
public delegate void CallManagerErrorDelegate(string errMessage);

/// <summary>
/// Delegate definition for the RttCharactersReceived event of the Call class and the CallManager class.
/// </summary>
/// <param name="callID"></param>
/// <param name="From"></param>
/// <param name="TimeReceived"></param>
/// <param name="RttChars"></param>
public delegate void RttCharactersReceivedDelegate(string callID, string From, DateTime TimeReceived,
    string RttChars);
