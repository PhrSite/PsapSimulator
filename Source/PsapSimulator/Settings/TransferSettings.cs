/////////////////////////////////////////////////////////////////////////////////////
//  File:   TransferSettings.cs                                     21 Jan 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

using Ng911Lib.Utilities;
using SipLib.Logging;

namespace PsapSimulator.Settings;

/// <summary>
/// Configuration settings for performing NG9-1-1 style call conferencing and call transfers.
/// </summary>
public class TransferSettings
{
    /// <summary>
    /// SIP URI of the conference bridge to use for transfers if the call source is not a conference
    /// aware user agent. Optional unless UseAdHocMethod is true.
    /// </summary>
    public string ConferenceBridgeUri { get; set; } = string.Empty;

    /// <summary>
    /// If true, then always use the ad hoc bridge transfer method even if the call source is a conference aware
    /// user agent. Section 5.4 of RFC 4579 and Section 4.7.1.1 of NENA-STA-010.3b.
    /// </summary>
    public bool UseAdHocMethod { get; set; } = false;

    /// <summary>
    /// Contains a list of transfer targets.
    /// </summary>
    public List<TransferTarget> Targets { get; set; } = new List<TransferTarget>();

    /// <summary>
    /// Contains the Name of the last used transfer target.
    /// </summary>
    public string LastUsedTransferTargetName {  get; set; } = string.Empty;

    private const string TransferSettingsFileName = "TransferSettings.json";

    private static string GetTransferSettingsPath()
    {
        string SettingsFilePath = Path.Combine(GetSettingsDirectory(), TransferSettingsFileName);
        return SettingsFilePath;
    }

    private static string GetSettingsDirectory()
    {
        string MyDocmentsDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string SettingsDirectory = @$"{MyDocmentsDir}\{AppSettings.AppName}";
        return SettingsDirectory;
    }

    /// <summary>
    /// Gets the TransferSettings from a file.
    /// </summary>
    /// <returns>Returns the settings from the file or default settings if the file does not exist.</returns>
    public static TransferSettings GetTransferSettings()
    {
        TransferSettings transferSettings = new TransferSettings();
        string strFilePath = GetTransferSettingsPath();
        if (File.Exists(strFilePath) == true)
        {
            string strSettings = File.ReadAllText(strFilePath);
            transferSettings = JsonHelper.DeserializeFromString<TransferSettings>(strSettings);
            if (transferSettings == null)
            {
                transferSettings = new TransferSettings();
                SipLogger.LogError("Error deserializing the TransferSettings file. Using the default settings");
            }
        }

        return transferSettings;
    }

    /// <summary>
    /// Saves the TransferSettings to a file.
    /// </summary>
    /// <param name="transferSettings">Settings to save to file.</param>
    public static void SaveTransferSettingsToFile(TransferSettings transferSettings)
    {
        string settingsDirectory = GetSettingsDirectory();
        try
        {
            if (Directory.Exists(settingsDirectory) == false)
            {
                DirectoryInfo info = Directory.CreateDirectory(settingsDirectory);
                if (info.Exists == false)
                {
                    SipLogger.LogError($"Unable to create the settings directory: {settingsDirectory}. " +
                        "Settings not saved.");
                    return;

                }
            }
        }
        catch (Exception ex)
        {
            SipLogger.LogError(ex, $"Unable to create the settings directory: {settingsDirectory}. " +
                "Settings not saved.");
            return;
        }

        string strSettings = JsonHelper.SerializeToString(transferSettings);
        if (strSettings == null)
        {
            SipLogger.LogError("Unable to serialize the TransferSettings. Settings not saved");
            return;
        }

        string SettingsFilePath = GetTransferSettingsPath();
        try
        {
            File.WriteAllText(SettingsFilePath, strSettings);
        }
        catch (Exception Ex)
        {
            SipLogger.LogError(Ex, $"Unable to write the TransferSettings file: {SettingsFilePath}");
        }
    }
}
