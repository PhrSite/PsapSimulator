/////////////////////////////////////////////////////////////////////////////////////
//  File:   Program.cs                                              21 Oct 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

using SipLib.Channels;
using SipLib.Network;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace SrsSimulator;

internal class Program
{
    private const int LocalSipPort = 5080;
    private const string RecordingsDirectory = @"\SrsSimulatorRecordings";

    static async Task Main(string[] args)
    {
        X509Certificate2 myCertificate = X509CertificateLoader.LoadPkcs12FromFile("SrsSimulator.pfx", "SrsSimulator");

        SIPTCPChannel Channel;
        string UserName = "SrsSimulator";
        IPAddress localAddress;
        SrsUa srsUa;
        Console.Title = UserName;

        List<IPAddress> addresses = IpUtils.GetIPv4Addresses();
        //List<IPAddress> addresses = IpUtils.GetIPv6Addresses();
        if (addresses == null || addresses.Count == 0)
        {
            Console.WriteLine("Error: No IPv4 addresses available");
            return;
        }

        localAddress = addresses[0];    // Pick the first available IPv4 address to listen on
        IPEndPoint localIPEndPoint = new IPEndPoint(localAddress, LocalSipPort);
        Channel = new SIPTCPChannel(localIPEndPoint, UserName);

        Console.WriteLine($"Listening on {Channel.SIPChannelContactURI} ...");
        Console.WriteLine("Type quit to exit the program");

        if (Directory.Exists(RecordingsDirectory) == false)
            Directory.CreateDirectory(RecordingsDirectory);

        srsUa = new SrsUa(Channel, UserName, myCertificate, RecordingsDirectory);

        srsUa.Start();

        string? strLine;
        while (true)
        {
            strLine = Console.ReadLine();
            if (string.IsNullOrEmpty(strLine))
                continue;

            if (strLine == "quit")
                break;

        }

        await srsUa.Shutdown();
    }
}
