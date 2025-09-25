/////////////////////////////////////////////////////////////////////////////////////
//  File:   HelpUtils.cs                                            10 Sep 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

using System.Diagnostics;

namespace PsapSimulator;

internal class HelpUtils
{

    public static void ShowHelpTopic(string strUrl)
    {
        ProcessStartInfo psi = new ProcessStartInfo(strUrl)
        {
            UseShellExecute = true
        };
        Process.Start(psi);
    }

    public const string HELP_URL_BASE = "https://phrsite.github.io/PsapSimulator";
    
    //public const string HELP_URL_BASE = "http://localhost:8080";

    public const string MAIN_WINDOW_HELP_URL = HELP_URL_BASE + "/docs/MainWindow.html";
    public const string NETWORK_PAGE_HELP_URL = HELP_URL_BASE + "/docs/NetworkSettings.html";
    public const string IDENTITY_PAGE_HELP_URL = HELP_URL_BASE + "/docs/IdentitySettings.html";
    public const string CALL_HANDLING_PAGE_HELP_URI = HELP_URL_BASE + "/docs/CallHandlingSettings.html";
    public const string MEDIA_SOURCE_PAGE_HELP_URI = HELP_URL_BASE + "/docs/MediaSourceSettings.html";
    public const string DEVICE_SETTINGS_PAGE_HELP_URI = HELP_URL_BASE + "/docs/DeviceSettings.html";
    public const string INTERFACE_SETTINGS_PAGE_HELP_URI = HELP_URL_BASE + "/docs/InterfaceSettings.html";
    public const string CAD_IF_SETTINGS_HELP_URI = HELP_URL_BASE + "/docs/InterfaceSettings.html#CadIfSettings";
    public const string SIPREC_RECORDING_SETTINGS_HELP_URI = HELP_URL_BASE + "/docs/SipRecSettings.html";
    public const string SIPREC_RECORDER_SETTINGS_HELP_URI = SIPREC_RECORDING_SETTINGS_HELP_URI + "#SipRecRecorderSettings";
    public const string EVENT_LOGGING_SETTINGS_HELP_URI = HELP_URL_BASE + "/docs/EventLoggingSettings.html";
    public const string EVENT_LOGGER_SETTINGS_HELP_URI = EVENT_LOGGING_SETTINGS_HELP_URI + "#EventLoggerSettings";
    public const string TEST_CALL_SETTINGS_HELP_URI = HELP_URL_BASE + "/docs/TestCallSettings.html";
    public const string CALL_FORM_HELP_URI = HELP_URL_BASE + "/docs/CallForm.html";

}
