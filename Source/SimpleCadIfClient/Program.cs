/////////////////////////////////////////////////////////////////////////////////////
//  File:   Program.cs                                              13 Feb 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

using Eido;
using Ng911Lib.Utilities;
using Ng911CadIfLib;

namespace WebSocketClient;

public class Program
{
    private const string strWssUrl = "wss://192.168.1.84:16000/IncidentData/ent";

    private static CadIfWebSocketClient? Cws = null;
    private const int ExpiresSeconds = 60;
    private static X509Certificate2? ClientCert = null;

    static async Task Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine($"Please specify the server's URI. For example: {strWssUrl}");
            return;
        }

        Uri? uri = null; ;
        try
        {
            uri = new Uri(args[0]);
        }
        catch (UriFormatException)
        {
            Console.WriteLine($"Invalid server URI.");
            return;
        }

        ClientCert = new X509Certificate2("WebSocketClient.pfx", "Wss12345");

        Console.Title = "SimpleCadIfClient";
        Console.WriteLine("Press Enter to end the program.");
        Console.WriteLine("Connecting to the EIDO server...");
        Cws = new CadIfWebSocketClient(ClientCert, RemoteCertificateValidationCallback!, args[0], 60);
        Cws.EidoReceived += OnEidoReceived;
        Cws.CadIfConnectionState += OnCadIfConnectionState;
        Cws.CadIfSubscriptionState += OnCadIfSubscriptionState;
        Cws.Start();

        bool Done = false;
        string? strMsg;

        while (Done == false)
        {
            strMsg = Console.ReadLine();
            if (string.IsNullOrEmpty(strMsg) == true)
                Done = true;
        }

        Cws.EidoReceived -= OnEidoReceived;
        Cws.CadIfConnectionState -= OnCadIfConnectionState;
        Cws.CadIfSubscriptionState -= OnCadIfSubscriptionState;
        await Cws.Shutdown();
    }

    private static void OnCadIfSubscriptionState(bool IsSubscribed, string strServerUri)
    {
        Console.WriteLine($"Subscribed = {IsSubscribed}");
    }

    private static void OnCadIfConnectionState(bool IsConnected, string strServerUri)
    {
        Console.WriteLine($"Connected = {IsConnected}");
    }

    private static void OnEidoReceived(EidoType eido, string strServerUri)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"EIDO received at {TimeUtils.GetCurrentNenaTimestamp()}");
        Console.ResetColor();
        Console.WriteLine(EidoHelper.SerializeToString(eido));
    }

    private static bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, 
        X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }
}