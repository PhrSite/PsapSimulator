/////////////////////////////////////////////////////////////////////////////////////
//  File:   SipRecRecorderSettings.cs                               8 Feb 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator.Settings;
using SipLib.Core;

/// <summary>
/// Configuration settings for a single SIPREC media recorder
/// </summary>
public class SipRecRecorderSettings
{
    /// <summary>
    /// Name of the SIPREC recorder.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// If true then this SIPREC recorder is enabled
    /// </summary>
    public bool Enabled { get; set; } = false;

    /// <summary>
    /// Specifies the transport protocol for SIP for the recorder
    /// </summary>
    public SIPProtocolsEnum SipTransportProtocol { get; set; } = SIPProtocolsEnum.tcp;

    /// <summary>
    /// Specifies the SIP interface IP endpoint of the recorder. For example 192.168.1.76:5060.
    /// </summary>
    public string IpEndpoint { get; set; } = string.Empty;

    /// <summary>
    /// Specifies the encryption to offer the recorder for RTP type media (audio, video, RTT).
    /// </summary>
    public RtpEncryptionEnum RtpEncryption { get; set; } = RtpEncryptionEnum.None;

    /// <summary>
    /// Specifies the encryption to offer the recorder for MSRP media
    /// </summary>
    public MsrpEncryptionEnum MsrpEncryption { get; set; } = MsrpEncryptionEnum.None;

    /// <summary>
    /// Constructor
    /// </summary>
    public SipRecRecorderSettings()
    {
    }
}
