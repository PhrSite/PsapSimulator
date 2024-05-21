/////////////////////////////////////////////////////////////////////////////////////
//  File:   EventLoggingSettings.cs                                 9 Feb 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator.Settings;

/// <summary>
/// Settings for NG9-1-1 event logging
/// </summary>
public class EventLoggingSettings
{
    /// <summary>
    /// If true, then NG9-1-1 events will be sent to all enabled event loggers
    /// </summary>
    public bool EnableLogging { get; set; } = false;

    /// <summary>
    /// List of NG9-1-1 event loggers
    /// </summary>
    public List<EventLoggerSettings> Loggers { get; set; } = new List<EventLoggerSettings>();

    /// <summary>
    /// Constructor
    /// </summary>
    public EventLoggingSettings()
    {
    }
}
