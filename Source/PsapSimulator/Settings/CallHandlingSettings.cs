/////////////////////////////////////////////////////////////////////////////////////
//  File:   CallHandlingSettings.cs                                 7 Feb 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

using SipLib.Media;

namespace PsapSimulator.Settings;

/// <summary>
/// Settings that determine the types of media that the application will handle
/// </summary>
public class CallHandlingSettings
{
    /// <summary>
    /// Default audio to play when a call is on hold
    /// </summary>
    public const string DefaultCallHoldAudioFile = @".\Recordings\CallHoldAudioFile.wav";
    /// <summary>
    /// Default video image file to play when a call is on hold
    /// </summary>
    public const string DefaultCallHoldVideoFile = @".\Recordings\OnHold.jpg";
    /// <summary>
    /// Default audio file to play when a call is auto-answered
    /// </summary>
    public const string DefaultAutoAnswerAudioFile = @".\Recordings\AutoAnswerAudioFile.wav";
    /// <summary>
    /// Default video image file to play when a call is auto-answered
    /// </summary>
    public const string DefaultAutoAnswerVideoFile = @".\Recordings\AutoAnswered.jpg";
    /// <summary>
    /// Default video image file to play when the camera is disabled
    /// </summary>
    public const string DefaultCameraDisabledVideoFile = @".\Recordings\DisabledCamera.jpg";
    /// <summary>
    /// Default text message to send when a call is on hold
    /// </summary>
    public const string DefaultCallHoldTextMessage = "You are currently on hold, please wait.";
    /// <summary>
    /// Default repeat interval in seconds at which to send the call hold text message.
    /// </summary>
    public const int DefaultCallHoldTextRepeatInterval = 30;
    /// <summary>
    /// Default text message to send when a call is auto answered.
    /// </summary>
    public const string DefaultAutoAnswerTextMessage = "All call takers are currently busy. " +
        "Please wait for the next call taker";
    /// <summary>
    /// Default repeat interval in seconds at which to send the auto-answer text message.
    /// </summary>
    public const int DefaultAutoAnswerTextRepeatInterval = 30;

    /// <summary>
    /// Specifies the maximum number of calls that have some kind of media (audio, video, RTT or MSRP)
    /// </summary>
    public int MaximumCalls { get; set; } = 10;

    /// <summary>
    /// Specifies the maximum of calls that do not have media.
    /// </summary>
    public int MaximumNonInteractiveCalls { get; set; } = 10;

    /// <summary>
    /// If true, the audio is enabled
    /// </summary>
    public bool EnableAudio {  get; set; } = true;

    /// <summary>
    /// If true, then video is enabled
    /// </summary>
    public bool EnableVideo { get; set; } = true;

    /// <summary>
    /// If true, then RTT media is enabled
    /// </summary>
    public bool EnableRtt { get; set; } = true;

    /// <summary>
    /// If true, then MSRP media is enabled
    /// </summary>
    public bool EnableMsrp { get; set; } = true;

    /// <summary>
    /// If true, then video from the selected videa capture device will be sent to the caller if video is
    /// enabled and a call has video media. Else, a fixed image will be sent to the caller if video is
    /// enabled.
    /// </summary>
    public bool EnableTransmitVideo { get; set; } = true;

    /// <summary>
    /// If true then calls will be automatically answered when they arrive, up to the number of calls
    /// specified in the MaximumCalls setting 
    /// </summary>
    public bool EnableAutoAnswer { get; set; } = false;

    /// <summary>
    /// Specifies the encryption to offer the recorder for RTP type media (audio, video, RTT).
    /// </summary>
    public RtpEncryptionEnum RtpEncryption { get; set; } = RtpEncryptionEnum.None;

    /// <summary>
    /// Specifies the encryption to offer the recorder for MSRP media
    /// </summary>
    public MsrpEncryptionEnum MsrpEncryption { get; set; } = MsrpEncryptionEnum.None;

    /// <summary>
    /// Specifies the audio source for calls placed on hold
    /// </summary>
    public CallHoldAudioSource CallHoldAudio { get; set; } = CallHoldAudioSource.CallHoldRecording;

    /// <summary>
    /// Specifies the file path for the audio file to play when a call is placed on hold.
    /// </summary>
    public string? CallHoldAudioFile { get; set; } = DefaultCallHoldAudioFile;
    /// <summary>
    /// Specifies the static image pattern image file to send when a call with video is placed on hold
    /// </summary>
    public string? CallHoldVideoFile { get; set; } = DefaultCallHoldVideoFile;
    /// <summary>
    /// Specifies the text message to send when a call is placed on hold. This applies to calls with
    /// RTT or MSRP media.
    /// </summary>
    public string? CallHoldTextMessage { get; set; } = DefaultCallHoldTextMessage;
    /// <summary>
    /// Specifies the interval in seconds to repeat the call hold text message. A value of 0 specifies
    /// that the message will only be sent once.
    /// </summary>
    public int CallHoldTextMessageRepeatSeconds { get; set; } = DefaultCallHoldTextRepeatInterval;

    /// <summary>
    /// Specifies the file path of the audio file to send if the an incoming call has audio and
    /// is auto-answered.
    /// </summary>
    public string? AutoAnswerAudioFile { get; set; } = DefaultAutoAnswerAudioFile;
    /// <summary>
    /// Specifies the file path of the static video image to send if the incoming call has video and
    /// is auto-answered.
    /// </summary>
    public string? AutoAnswerVideoFile { get; set; } = DefaultAutoAnswerVideoFile;
    /// <summary>
    /// Specifies the text message to send when a call is auto-answered. This applies to calls with
    /// RTT or MSRP media.
    /// </summary>
    public string? AutoAnswerTextMessage { get; set;} = DefaultAutoAnswerTextMessage;
    /// <summary>
    /// Specifies the interval in seconds to repeat the auto-answer text message. A value of 0 specifies
    /// that the message will only be sent once.
    /// </summary>
    public int AutoAnswerTextMessageRepeatSeconds { get; set; } = DefaultAutoAnswerTextRepeatInterval;

    /// <summary>
    /// Specifies the image to transmit for a call with video if transmit video is disabled (EnableTransmitVideo 
    /// is false)
    /// </summary>
    public string? TransmitVideoDisabledImageFile { get; set; } = DefaultCameraDisabledVideoFile;

    /// <summary>
    /// Constructor
    /// </summary>
    public CallHandlingSettings()
    {
    }
}

/// <summary>
/// Enumeration of the types of audio sources for calls on hond
/// </summary>
public enum CallHoldAudioSource : int
{
    /// <summary>
    /// Play silence
    /// </summary>
    Silence,
    /// <summary>
    /// Play the standard hold beep sound
    /// </summary>
    CallHoldBeepSound,
    /// <summary>
    /// Play the configured call hold audio file
    /// </summary>
    CallHoldRecording,
}

