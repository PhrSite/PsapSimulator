/////////////////////////////////////////////////////////////////////////////////////
//  File:   NetworkSettings.cs                                      6 Feb 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator.Settings;
using SipLib.Media;

/// <summary>
/// Settings for the networks that the application will listen on.
/// </summary>
public class NetworkSettings
{
    /// <summary>
    /// If true then the application will listen on an IPv4 network
    /// </summary>
    public bool EnableIPv4 { get; set; } = true;
    
    /// <summary>
    /// If true then the application will listen on an IPv6 network
    /// </summary>
    public bool EnableIPv6 { get; set;} = false;

    /// <summary>
    /// If true then the application will listen on UDP for SIP messages
    /// </summary>
    public bool EnableUdp { get; set; } = false;

    /// <summary>
    /// If true, then the application will listen on TCP for SIP messages
    /// </summary>
    public bool EnableTcp { get; set; } = true;

    /// <summary>
    /// If true, then the application will listen on TLS for SIP messages
    /// </summary>
    public bool EnableTls { get; set; } = false;

    /// <summary>
    /// Specifies the IPv4 network address to listen on
    /// </summary>
    public string? IPv4Address { get; set; } = null;

    /// <summary>
    /// Specifies the IPv6 network address to listen on
    /// </summary>
    public string? IPv6Address { get;set; } = null;

    /// <summary>
    /// Specifies the SIP port to use for UDP and TCP
    /// </summary>
    public int SipPort { get; set; } = 5060;

    /// <summary>
    /// Specifies the SIP port to use for SIPS (SIP over TLS)
    /// </summary>
    public int SipsPort { get; set; } = 5061;

    /// <summary>
    /// Contains the port ranges for each type of media
    /// </summary>
    public MediaPortSettings MediaPorts { get; set; } = new MediaPortSettings();
    
    /// <summary>
    /// If true, then use mutual TLS authentication for SIP TLS connections both as a server and as a client.
    /// </summary>
    public bool UseMutualTlsAuthentication { get; set; } = true;

    /// <summary>
    /// Constructor
    /// </summary>
    public NetworkSettings()
    {
    }

}
