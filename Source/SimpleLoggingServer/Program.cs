/////////////////////////////////////////////////////////////////////////////////////
//  File:   Program.cs                                              21 Aug 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace SimpleLoggingServer;

public class Program
{
    private const int LOCALPORT = 11000;

    public static void Main(string[] args)
    {
        string strVer = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
        Console.Title = $"SimpleLoggingServer -- Version: {strVer}";

        // For testing only. Don't put a password in the application code if security is required.
        X509Certificate2 ServerCert = X509CertificateLoader.LoadPkcs12FromFile("SimpleLoggingServer.pfx", "SimpleLoggingServer");

        List<IPAddress> ipAddresses = GetIPv4Addresses();
        if (ipAddresses.Count == 0)
        {
            Console.WriteLine("Error: No IPv4 addresses available");
            return;
        }

        IPAddress ipAddress = ipAddresses[0];
        string strBaseUri = $"https://{ipAddress}:{LOCALPORT}";
        Console.WriteLine($"Listening on {strBaseUri}");
        Console.WriteLine($"\tPOST to {strBaseUri}/LogEvents to send log events");
        Console.WriteLine($"\tGET from {strBaseUri}/Versions to get version information");
        Console.WriteLine("Type Ctrl-C to terminate this program");
        Console.WriteLine();

        // See: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel/endpoints?view=aspnetcore-7.0
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.WebHost.UseKestrel(options =>
        {
            options.Listen(ipAddress, LOCALPORT, listenOptions =>
            {
                listenOptions.UseHttps(new HttpsConnectionAdapterOptions
                {
                    ServerCertificate = ServerCert,
                    ClientCertificateMode = ClientCertificateMode.AllowCertificate,
                    ClientCertificateValidation = CustomCertificateValidation
                });
            });
        })
            .ConfigureLogging((context, logging) =>
            {   // Turn off logging because the ASP .NET CORE framework generates a lot of meaningless log messages.
                logging.ClearProviders();
            });

        // Add services to the container.
        builder.Services.AddControllers();

        WebApplication app = builder.Build();
        // Configure the HTTP request pipeline.
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }

    /// <summary>
    /// Allows this application to perform custom X.509 certificate validation.
    /// </summary>
    /// <param name="certificate">The certificate used to authenticate the remote party.</param>
    /// <param name="chain">The chain of certificate authorities associated with the remote certificate.</param>
    /// <param name="errors">One or more errors associated with the remote certificate.</param>
    /// <returns>A Boolean value that determines whether the specified certificate is accepted for authentication.
    /// A return value of true indicates that the remote certificate is accepted.</returns>
    public static bool CustomCertificateValidation(X509Certificate2 certificate, X509Chain chain, SslPolicyErrors errors)
    {
        return true;
    }

    // Used by hosts attempting to acquire a DHCP address. See RFC 3330.
    private const string LINK_LOCAL_BLOCK_PREFIX = "169.254";
    private const string IPV4_LOCAL_LOOPBACK = "127.0.0.1";

    /// <summary>
    /// Gets a list of all available IPv4 IP addresses on the local machine. The list will not contain the local loopback 
    /// IPv4 address.
    /// </summary>
    /// <returns>Returns a list of addresses. The list may be empty but it will never be null.</returns>
    public static List<IPAddress> GetIPv4Addresses()
    {
        List<IPAddress> localAddresses = new List<IPAddress>();
        NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface adapter in adapters)
        {
            if (adapter.OperationalStatus != OperationalStatus.Up)
                continue;

            IPInterfaceProperties adapterProperties = adapter.GetIPProperties();

            UnicastIPAddressInformationCollection localIPs = adapterProperties.UnicastAddresses;
            foreach (UnicastIPAddressInformation localIP in localIPs)
            {
                string strIpv4Addr = localIP.Address.ToString();
                if (localIP.Address.AddressFamily != AddressFamily.InterNetwork)
                    continue;

                if (strIpv4Addr.StartsWith(LINK_LOCAL_BLOCK_PREFIX) == false && strIpv4Addr != IPV4_LOCAL_LOOPBACK)
                    localAddresses.Add(localIP.Address);
            }
        }

        return localAddresses;
    }
}