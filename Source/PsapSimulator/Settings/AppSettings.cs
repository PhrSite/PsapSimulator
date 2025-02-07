/////////////////////////////////////////////////////////////////////////////////////
//  File:   AppSettings.cs                                          6 Feb 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator.Settings;
using Ng911Lib.Utilities;
using SipLib.Logging;
using System.Text.Json.Serialization;

/// <summary>
/// All settings for the application
/// </summary>
public class AppSettings
{
    /// <summary>
    /// Network configuration settings
    /// </summary>
    public NetworkSettings NetworkSettings {get; set; } = new NetworkSettings();

    /// <summary>
    /// Specifies the X.509 certificate to use
    /// </summary>
    public CertificateSettings CertificateSettings { get; set; } = new CertificateSettings();
    
    /// <summary>
    /// NG9-1-1 identity settings
    /// </summary>
    public IdentitySettings Identity {  get; set; } = new IdentitySettings();

    /// <summary>
    /// Call handling settings
    /// </summary>
    public CallHandlingSettings CallHandling {  get; set; } = new CallHandlingSettings();
    
    /// <summary>
    /// Audio and video device settings
    /// </summary>
    public DeviceSettings Devices { get; set; } = new DeviceSettings();

    /// <summary>
    /// SIPREC media recording settings.
    /// </summary>
    public SipRecSettings SipRec { get; set; } = new SipRecSettings();

    /// <summary>
    /// Settings for NG9-1-1 event logging
    /// </summary>
    public EventLoggingSettings EventLogging { get; set; } = new EventLoggingSettings();

    /// <summary>
    /// Constructor
    /// </summary>
    [JsonConstructor]
    AppSettings()
    {
    }

    public const string AppName = "PsapSimulator";
    private const string SettingsFileName = $"{AppName}.json";

    /// <summary>
    /// Gets the saved configuration settings if they exist or the default settings if they do no.
    /// </summary>
    /// <returns>Returns the configuration settings</returns>
    public static AppSettings GetAppSettings()
    {
        AppSettings? appSettings = new AppSettings();
        string MyDocmentsDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string SettingsDirectory = @$"{MyDocmentsDir}\{AppName}";
        string SettingsFilePath = Path.Combine(SettingsDirectory, SettingsFileName);
        if (File.Exists(SettingsFilePath) == true)
        {
            string strSettings = File.ReadAllText(SettingsFilePath);
            appSettings = JsonHelper.DeserializeFromString<AppSettings>(strSettings);
            if (appSettings == null)
            {   // An error occurred, use the default settings
                appSettings = new AppSettings();
                SipLogger.LogError("Error deserializing the settings file, using the default settings");
            }
        }
        else
        {   // Use the default settings
            SipLogger.LogInformation("The settings file does not exist, using the default settings");
        }

        return appSettings;
    }

    /// <summary>
    /// Saves the settings to a file.
    /// </summary>
    /// <param name="appSettings">Settings to save.</param>
    public static void SaveAppSettings(AppSettings appSettings)
    {
        string MyDocmentsDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string SettingsDirectory = @$"{MyDocmentsDir}\{AppName}";
        string SettingsFilePath = Path.Combine(SettingsDirectory, SettingsFileName);
        try
        {
            if (Directory.Exists(SettingsDirectory) == false)
            {
                DirectoryInfo dirInfo = Directory.CreateDirectory(SettingsDirectory);
                if (dirInfo.Exists == false)
                {
                    SipLogger.LogError($"Unable to create the settings directory: {SettingsDirectory}. " +
                        "Settings not saved.");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            SipLogger.LogError(ex, $"Unable to create the settings directory: {SettingsDirectory}. " +
                "Settings not saved.");
            return;
        }

        string strSettings = JsonHelper.SerializeToString(appSettings);
        if (string.IsNullOrEmpty(strSettings) == true)
        {
            SipLogger.LogError("Unable to serialize the settings. Settings not saved");
            return;
        }

        try
        {
            File.WriteAllText(SettingsFilePath, strSettings);
        }
        catch (Exception Ex)
        {
            SipLogger.LogError(Ex, $"Unable to write the settings file: {SettingsFileName}");
        }
    }
}
