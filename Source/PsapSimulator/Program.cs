/////////////////////////////////////////////////////////////////////////////////////
//  File:   Program.cs                                              4 Feb 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Extensions.Logging;
using SipLib.Logging;

using PsapSimulator.Settings;
using System.Text.RegularExpressions;

internal static class Program
{
    public static string LoggingDirectory = @"\var\log\PsapSimulator";
    private const string LoggingFileName = "PsapSimulator.log";
    private static LoggingLevelSwitch m_LevelSwitch = new LoggingLevelSwitch();
    
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // Setup application logging using Serilog
        LoggingDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\{AppSettings.AppName}\\Logs";
        if (Directory.Exists(LoggingDirectory) == false)
            Directory.CreateDirectory(LoggingDirectory);
        string LoggingPath = Path.Combine(LoggingDirectory, LoggingFileName);
        Logger log = new LoggerConfiguration()
            .MinimumLevel.ControlledBy(m_LevelSwitch)
            .WriteTo.File(LoggingPath, fileSizeLimitBytes: 1000000, retainedFileCountLimit: 5,
            outputTemplate: "{Timestamp:yyyy-MM-ddTHH:mm:ss.ffffffzzz} [{Level}] {Message}{NewLine}{Exception}")
            .CreateLogger();
        SerilogLoggerFactory factory = new SerilogLoggerFactory(log);
        SipLogger.Log = factory.CreateLogger("PsapSimulator");

        //m_LevelSwitch.MinimumLevel = LogEventLevel.Error;

        SipLogger.LogInformation("Starting PsapSimulator now");

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        try
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
        catch (Exception ex)
        {
            SipLogger.LogCritical(ex, "Critical exception in PsapSimulator");
        }

        SipLogger.LogInformation("Exiting PsapSimulator now");
    }

    /// <summary>
    /// Sets the application logging level.
    /// </summary>
    /// <param name="level"></param>
    public static void SetLoggingLevel(LogEventLevel level)
    {
        m_LevelSwitch.MinimumLevel = level;
    }
}