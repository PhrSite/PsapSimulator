/////////////////////////////////////////////////////////////////////////////////////
//  File:   SrsUa.cs                                                21 Oct 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

using SipLib.Channels;
using SipLib.Core;
using SipLib.Logging;
using SipLib.Transactions;
using SipLib.Media;
using SipLib.Rtp;
using SipLib.Threading;

using System.Collections.Concurrent;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using SipLib.Sdp;

namespace SrsSimulator;

/// <summary>
/// 
/// </summary>
internal class SrsUa : QueuedActionWorkerTask
{
    private SipTransport m_SipTransport;
    private SIPChannel m_SipChannel;
    private IPAddress m_localAddress;
    private string m_userName;
    private X509Certificate2 m_MyCertificate;
    private MediaPortManager m_MediaPortManager;
    private SdpAnswerSettings m_SdpAnswerSettings;

    private List<string> AudioCodecs = new List<string>() { "PCMU", "PCMA", "G722" };
    private List<string> VideoCodecs = new List<string>() { "H264", "VP8" };

    private bool m_Started = false;

    private Dictionary<string, SrsCall> m_Calls = new Dictionary<string, SrsCall>();
    private string m_RecordingsDirectory;

    public SrsUa(SIPChannel sipChannel, string userName, X509Certificate2 myCertificate,
        string recordingsDirectory) : base(10)
    {
        m_SipChannel = sipChannel;
        m_SipTransport = new SipTransport(m_SipChannel);
        m_userName = userName;
        m_MyCertificate = myCertificate;
        m_localAddress = m_SipTransport.SipChannel.SIPChannelEndPoint.GetIPEndPoint().Address;
        m_RecordingsDirectory = recordingsDirectory;

        MediaPortSettings PortSettings = new MediaPortSettings();
        PortSettings.AudioPorts = new PortRange() { StartPort = 10000, Count = 1000 };
        PortSettings.VideoPorts = new PortRange() { StartPort = 11000, Count = 1000 };
        PortSettings.RttPorts = new PortRange() { StartPort = 12000, Count = 1000 };
        PortSettings.MsrpPorts = new PortRange() { StartPort = 12000, Count = 1000 };
        m_MediaPortManager = new MediaPortManager(PortSettings);

        m_SdpAnswerSettings = new SdpAnswerSettings(AudioCodecs, VideoCodecs, m_userName, RtpChannel.CertificateFingerprint!,
            m_MediaPortManager);
    }

    private string GetCallRecordingDirectory(string callID)
    {
        DateTime Now = DateTime.Now;
        string DayDirectory = Path.Combine(m_RecordingsDirectory, $"{Now:yyyy}\\{Now:MM}\\{Now:dd}");
        string CallRecordingDirectory = Path.Combine(DayDirectory, callID);
        if (Directory.Exists(CallRecordingDirectory) == false)
            Directory.CreateDirectory(CallRecordingDirectory);

        return CallRecordingDirectory;
    }

    public new void Start()
    {
        base.Start();

        if (m_Started == true)
            return;


        m_SipTransport.SipRequestReceived += OnSipRequestReceived;
        m_SipTransport.Start();

        m_Started = true;
    }

    /// <summary>
    /// Shuts down all SIP transport connections and releases resources.
    /// </summary>
    public new async Task Shutdown()
    {
        if (m_Started == false)
            return;


        // TODO: Terminate any calls that are currently active

        await base.Shutdown();

        // Unhook the event handlers
        m_SipTransport.SipRequestReceived -= OnSipRequestReceived;
        m_SipTransport.Shutdown();
    }

    /// <summary>
    /// Gets the Call object for a specified call ID
    /// </summary>
    /// <param name="callID">Call-ID header value for the call.</param>
    /// <returns>Returns the Call object if it exists or null if it does not</returns>
    private SrsCall? GetCall(string? callID)
    {
        if (string.IsNullOrEmpty(callID))
            return null;
        else
            return m_Calls.GetValueOrDefault(callID);
    }

    private void OnSipRequestReceived(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransportManager)
    {
        EnqueueWork(() => { HandleSipRequest(sipRequest, remoteEndPoint, sipTransportManager); });
    }

    // Handle incoming SIP requests from SIP transport manager in the thread context of the SrsUserAgentTask
    private void HandleSipRequest(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransportManager)
    {
        switch (sipRequest.Method)
        {
            case SIPMethodsEnum.INVITE:
                ProcessInviteRequest(sipRequest, remoteEndPoint, sipTransportManager);
                break;
            case SIPMethodsEnum.CANCEL:
                ProcessCancelRequest(sipRequest, remoteEndPoint, sipTransportManager);
                break;
            case SIPMethodsEnum.ACK:
                ProcessAckRequest(sipRequest, remoteEndPoint, sipTransportManager);
                break;
            case SIPMethodsEnum.BYE:
                ProcessByeRequest(sipRequest, remoteEndPoint, sipTransportManager);
                break;
            case SIPMethodsEnum.OPTIONS:
                ProcessOptionsRequest(sipRequest, remoteEndPoint, sipTransportManager);
                break;
            case SIPMethodsEnum.UPDATE:
                ProcessUpdateRequest(sipRequest, remoteEndPoint, sipTransportManager);
                break;
            default:
                SendMethodNotAllowed(sipRequest, remoteEndPoint, sipTransportManager);
                break;
        }
    }

    private void ProcessInviteRequest(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransportManager)
    {
        SrsCall? srsCall = GetCall(sipRequest.Header.CallId);
        if (srsCall != null)
        {
            ProcessReInviteRequest(srsCall, sipRequest, remoteEndPoint, sipTransportManager);
            return;
        }

        // Its a new call request
        IPEndPoint ipEndPoint = remoteEndPoint.GetIPEndPoint();
        SrsCall call = new SrsCall(sipRequest, ipEndPoint, sipTransportManager, m_MediaPortManager,
            m_MyCertificate, m_SdpAnswerSettings, GetCallRecordingDirectory(sipRequest.Header.CallId));
        SIPResponse response = call.StartCall();
        if (response.Status == SIPResponseStatusCodesEnum.Ok)
            m_Calls.Add(sipRequest.Header.CallId, call);

        sipTransportManager.StartServerInviteTransaction(sipRequest, ipEndPoint, null, response);
    }

    private void ProcessReInviteRequest(SrsCall srsCall, SIPRequest sipRequest, SIPEndPoint remoteEndPoint, 
        SipTransport sipTransportManager)
    {
        SIPResponse response = srsCall.HandleReInviteRequest(sipRequest);
        sipTransportManager.StartServerInviteTransaction(sipRequest, remoteEndPoint.GetIPEndPoint(), null,
            response);
    }

    private void ProcessUpdateRequest(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransportManager)
    {
        SrsCall? srsCall = GetCall(sipRequest.Header.CallId);
        SIPResponse response;
        if (srsCall == null)
            // The call does not exist, perhaps the call ended.
            response = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.NotFound, "Not Found", 
                sipTransportManager.SipChannel, m_userName);
        else
            response = srsCall.HandleUpdateRequest(sipRequest, sipTransportManager.SipChannel);

        sipTransportManager.StartServerNonInviteTransaction(sipRequest, remoteEndPoint.GetIPEndPoint(), null,
            response);
    }

    private void ProcessCancelRequest(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransportManager)
    {
    }

    private void ProcessAckRequest(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransportManager)
    {

    }

    private void ProcessByeRequest(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransportManager)
    {
        SrsCall? call = GetCall(sipRequest.Header.CallId);
        SIPResponse response;
        if (call == null)
            response = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.CallLegTransactionDoesNotExist,
                "Call Does Not Exist", sipTransportManager.SipChannel, m_userName);
        else
        {
            response = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.Ok, "OK", sipTransportManager.SipChannel,
                m_userName);
            call.EndCall();
            m_Calls.Remove(sipRequest.Header.CallId);
        }

        sipTransportManager.StartServerNonInviteTransaction(sipRequest, remoteEndPoint.GetIPEndPoint(),
            null, response);
    }

    private void ProcessOptionsRequest(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransportManager)
    {
        SIPResponse OkResponse = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.Ok, "OK",
            m_SipChannel, m_userName);
        // Just fire and forget
        sipTransportManager.StartServerNonInviteTransaction(sipRequest, remoteEndPoint.GetIPEndPoint(),
            null, OkResponse);
    }

    private void SendMethodNotAllowed(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransportManager)
    {
        SIPResponse sipResponse = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.MethodNotAllowed,
            "Method Not Allowed", sipTransportManager.SipChannel, m_userName);

        // Just fire and forget.
        sipTransportManager.StartServerNonInviteTransaction(sipRequest, remoteEndPoint.GetIPEndPoint(),
            null, sipResponse);

    }
}
