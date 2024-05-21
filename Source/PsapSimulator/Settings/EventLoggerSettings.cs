/////////////////////////////////////////////////////////////////////////////////////
//  File:   EventLoggerSettings.cs                                  9 Feb 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator.Settings;

/// <summary>
/// Settings for an single NG9-1-1 event logger
/// </summary>
public class EventLoggerSettings
{
    /// <summary>
    /// Name of the event logger
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// If true then NG9-1-1 events will be sent to this event logger
    /// </summary>
    public bool Enabled { get; set; } = false;

    /// <summary>
    /// HTTP or HTTPS URI of the event logger
    /// </summary>
    public string LoggerUri { get; set; } = string.Empty;

    /// <summary>
    /// Constructor
    /// </summary>
    public EventLoggerSettings()
    {
    }

}
