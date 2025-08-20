/////////////////////////////////////////////////////////////////////////////////////
//  File:   Program.cs                                              7 Aug 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

using SipLib.Channels;
using SipLib.Core;
using SipLib.Network;
using SipLib.Transactions;
using System.Net;
using System.Security.Cryptography;

namespace SipLib.Subscriptions;

internal class Program
{
    private static int m_LocalSipPort = 5090;

    static async Task Main(string[] args)
    {
        SIPTCPChannel Channel;
        string UserName = "StateSubscriber";
        IPAddress localAddress;
        Console.Title = UserName;

        List<IPAddress> addresses = IpUtils.GetIPv4Addresses();
        //List<IPAddress> addresses = IpUtils.GetIPv6Addresses();
        if (addresses == null || addresses.Count == 0)
        {
            Console.WriteLine("Error: No IPv4 addresses available");
            return;
        }

        if (args.Length != 2)
        {
            Console.WriteLine("Usage: StateSubscriber IPEndPoint LocalSipPort -- For example: StateSubscriber 192.168.1.5:5060 5090");
            return;
        }

        IPEndPoint? notifierEndPoint;
        if (IPEndPoint.TryParse(args[0], out notifierEndPoint) == false)
        {
            Console.WriteLine("Invalid IPEndPoint");
            return;
        }

        if (int.TryParse(args[1], out m_LocalSipPort) == false)
        {
            Console.WriteLine("Invalid LocalSipPort argument");
            return;
        }

        localAddress = addresses[0];    // Pick the first available IPv4 address to listen on
        IPEndPoint localIPEndPoint = new IPEndPoint(localAddress, m_LocalSipPort);
        Channel = new SIPTCPChannel(localIPEndPoint, UserName);
        SipTransport sipTransport = new SipTransport(Channel);
        sipTransport.Start();

        SIPEndPoint notifierSipEndPoint = new SIPEndPoint(sipTransport.SipChannel.GetProtocol(), notifierEndPoint);
        SIPURI notifierSipUri = new SIPURI(SIPSchemesEnum.sip, notifierSipEndPoint);

        SubscriberUac subscriberUac = new SubscriberUac(sipTransport, notifierSipUri);
        subscriberUac.Start();

        Console.WriteLine("Type quit to exit the program");

        string? strLine;
        while (true)
        {
            strLine = Console.ReadLine();
            if (string.IsNullOrEmpty(strLine))
                continue;

            if (strLine == "quit")
                break;

        }

        subscriberUac.UnsubscribeToAll();
        await subscriberUac.Shutdown();
        sipTransport.Shutdown();
    }
}
