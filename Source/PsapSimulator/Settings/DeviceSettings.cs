/////////////////////////////////////////////////////////////////////////////////////
//  File:   DeviceSettings.cs                                       7 Feb 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator.Settings;

/// <summary>
/// Settings for the audio capture and video capture devices
/// </summary>
public class DeviceSettings
{
    /// <summary>
    /// Name of the audio I/O device (microphone and speakers)
    /// </summary>
    public string? AudioDeviceName { get; set; } = null;

    /// <summary>
    /// Settings for the selected video capture device. A value of null indicates that a video
    /// device has not been configured.
    /// </summary>
    public VideoSourceSettings? VideoDevice { get; set;} = null;

    /// <summary>
    /// Constructor
    /// </summary>
    public DeviceSettings()
    {
    }


}
