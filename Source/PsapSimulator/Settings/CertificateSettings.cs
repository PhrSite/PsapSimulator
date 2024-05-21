/////////////////////////////////////////////////////////////////////////////////////
//  File:   CertificateSettings.cs                                  6 Feb 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator.Settings;

/// <summary>
/// Class for storing the X.509 certificate settings for the application
/// </summary>
public class CertificateSettings
{
    public const string DefaultCertificateFile = @".\PsapSimulator.pfx";
    public const string DefaultCertificatePassword = "PsapSimulator";

    /// <summary>
    /// If true, then the default X.509 certificate will be used, else the certificate file specified
    /// in the CertificateFilePath property will be used and the CertificatePassword property specifies
    /// the password for the certificate.
    /// </summary>
    public bool UseDefaultCertificate { get; set; } = true;

    public string CertificateFilePath { get; set; } = DefaultCertificateFile;
    public string CertificatePassword { get; set; } = DefaultCertificatePassword;

    /// <summary>
    /// Constructor
    /// </summary>
    public CertificateSettings()
    {
    }
}
