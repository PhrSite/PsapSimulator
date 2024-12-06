/////////////////////////////////////////////////////////////////////////////////////
//  File:   CallManager.cs                                          15 Feb 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator.CallManagement;
using SipLib.Core;
using System.Net;
using System.Drawing;

using SipLib.Channels;
using SipLib.Body;
using PsapSimulator.Settings;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Concurrent;
using SipLib.Transactions;
using SipLib.Logging;
using AdditionalData;
using SipLib.Sdp;
using SipLib.Media;
using SipLib.Rtp;

using WindowsWaveAudio;
using System.Diagnostics;

using SipLib.RealTimeText;
using SipLib.Msrp;
using Pidf;
using Ng911Lib.Utilities;
using Held;
using HttpUtils;
using SIPSorceryMedia.FFmpeg;
using I3V3.LoggingHelpers;
using I3V3.LogEvents;

using PsapSimulator.WindowsVideo;
using System.Net.Security;
using SipLib.Subscriptions;
using SipLib.SipRec;
using System.Text;

/// <summary>
/// Class for managing all of the calls for the PsapSimulator application
/// </summary>
public class CallManager
{
    private AppSettings m_Settings;
    private List<SipTransport> m_SipTransports = new List<SipTransport>();
    private string UserName = "PsapSimulator";
    private List<SIPChannel> m_SipChannels = new List<SIPChannel>();
    private X509Certificate2 m_Certificate;
    private ConcurrentDictionary<string, Call> m_Calls = new ConcurrentDictionary<string, Call>();

    private CancellationTokenSource m_CancellationTokenSource = new CancellationTokenSource();

    private ConcurrentQueue<Action> m_WorkQueue = new ConcurrentQueue<Action>();

    private MediaPortManager m_PortManager;

    private string m_Fingerprint;

    private SdpAnswerSettings m_AnswerSettings;

    private WindowsAudioIo? m_WaveAudio = null;

    public event CallManagerErrorDelegate? CallManagerError;

    public StaticImageCapture? CameraDisabledCapture = null;
    public StaticImageCapture? AutoAnswerCapture = null;
    public StaticImageCapture? OnHoldCapture = null;

    private static List<string> m_SupportedVideoCodecs = new List<string>()
    {
        "H264", "VP8"
    };

    private static List<string> m_SupportedAudioCodecs = new List<string>()
    {
        "PCMU", "PCMA", "G722", "AMR-WB"
    };

    private AudioSampleData m_AutoAnswerAudioSampleData;

    private AudioSampleData m_OnHoldAudioSampleData;

    private Call? m_OnLineCall = null;

    public WindowsCameraCapture? CameraCapture = null;

    private SrcManager? m_SrcManager = null;

    private I3LogEventClientMgr m_I3LogEventClientMgr;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="appSettings">Applications settings</param>
    public CallManager(AppSettings appSettings)
    {
        m_Settings = appSettings;

        try
        {
            m_Certificate = new X509Certificate2(m_Settings.CertificateSettings.CertificateFilePath,
                m_Settings.CertificateSettings.CertificatePassword);
        }
        catch (Exception certEx)
        {
            SipLogger.LogCritical(certEx, "Unable to load the X.509 Certificate");
            throw;
        }

        //UserName = m_Settings.Identity.ElementID;
        m_PortManager = new MediaPortManager(m_Settings.NetworkSettings.MediaPorts);
        m_Fingerprint = RtpChannel.CertificateFingerprint!;

        m_AnswerSettings = new SdpAnswerSettings(m_SupportedAudioCodecs, m_SupportedVideoCodecs, UserName,
            m_Fingerprint, m_PortManager);
        m_AnswerSettings.EnableAudio = m_Settings.CallHandling.EnableAudio;
        m_AnswerSettings.EnableVideo = m_Settings.CallHandling.EnableVideo;
        m_AnswerSettings.EnableRtt = m_Settings.CallHandling.EnableRtt;
        m_AnswerSettings.EnableMsrp = m_AnswerSettings.EnableMsrp;

        // Set up the media source for auto answer
        m_AutoAnswerAudioSampleData = WindowsAudioUtils.ReadWaveFile(m_Settings.CallHandling.AutoAnswerAudioFile!);

        // Set up the media sources for hold
        m_OnHoldAudioSampleData = WindowsAudioUtils.ReadWaveFile(m_Settings.CallHandling.CallHoldAudioFile!);

        m_I3LogEventClientMgr = new I3LogEventClientMgr();
    }

    /// <summary>
    /// Event that is fired when a new incoming call is received
    /// </summary>
    public event CallStateDelegate? NewCall = null;

    /// <summary>
    /// Event that is fired when a call has ended.
    /// </summary>
    public event CallEndedDelegate? CallEnded = null;

    /// <summary>
    /// Event that is fired when the state of a call changes.
    /// </summary>
    public event CallStateDelegate? CallStateChanged = null;

    public event FrameBitmapReadyDelegate? FrameBitmapReady = null;

    private bool m_Started = false;

    public async Task Start()
    {
        if (m_Started == true)
            return;

        try
        {
            FFmpegInit.Initialise(FfmpegLogLevelEnum.AV_LOG_FATAL, @".\FFMPEG");
        }
        catch (Exception ffmpegEx)
        {
            SipLogger.LogCritical(ffmpegEx, "Unable to initialize the FFMPEG libraries");
            throw;
        }

        // Setup for I3 event logging.
        m_I3LogEventClientMgr = new I3LogEventClientMgr();
        foreach (EventLoggerSettings loggerSettings in m_Settings.EventLogging.Loggers)
        {
            I3LogEventClient client = new I3LogEventClient(loggerSettings.LoggerUri, loggerSettings.Name,
                m_Certificate, loggerSettings.Enabled);
            m_I3LogEventClientMgr.AddLoggingClient(client);
            client.Start();
        }
        m_I3LogEventClientMgr.LoggingServerError += OnLoggingServerError;
        m_I3LogEventClientMgr.LoggingServerStatusChanged += OnLoggingServerStatusChanged;
        m_I3LogEventClientMgr.Start();

        NetworkSettings Ns = m_Settings.NetworkSettings;
        CallHandlingSettings Ch = m_Settings.CallHandling;

        List<string?> ipAddresses = new List<string?>();
        if (Ns.EnableIPv4 == true)
            ipAddresses.Add(Ns.IPv4Address);

        if (Ns.EnableIPv6 == true)
            ipAddresses.Add(Ns.IPv6Address);

        foreach (string? ipAddr in ipAddresses)
        {
            if (ipAddr == null)
                continue;

            IPAddress Ipr = IPAddress.Parse(ipAddr);
            IPEndPoint Ipe;
            if (Ns.EnableUdp == true)
            {
                Ipe = new IPEndPoint(Ipr, Ns.SipPort);
                try
                {
                    SIPUDPChannel udpChannel = new SIPUDPChannel(Ipe, UserName);
                    m_SipChannels.Add(udpChannel);
                }
                catch (Exception UdpEx)
                {
                    SipLogger.LogError(UdpEx, $"Unable to create a SIPUDPChannel for IP endpoint: {Ipe}");
                }
            }

            if (Ns.EnableTcp == true)
            {
                Ipe = new IPEndPoint(Ipr, Ns.SipPort);
                try
                {
                    SIPTCPChannel tcpChannel = new SIPTCPChannel(Ipe, UserName);
                    m_SipChannels.Add(tcpChannel);
                }
                catch (Exception TcpEx)
                {
                    SipLogger.LogError(TcpEx, $"Unable to create a SIPTCPChannel for IP endpoint: {Ipe}");
                }
            }

            if (Ns.EnableTls == true)
            {
                Ipe = new IPEndPoint(Ipr, Ns.SipsPort);
                try
                {
                    SIPTLSChannel sipTlsChannel = new SIPTLSChannel(m_Certificate, Ipe, UserName, Ns.UseMutualTlsAuthentication);
                    m_SipChannels.Add(sipTlsChannel);
                }
                catch (Exception TlsEx)
                {
                    SipLogger.LogError(TlsEx, $"Unable to create a SIPTLSChannel for IP endpoint: {Ipe}");
                }
            }
        }

        foreach (SIPChannel channel in m_SipChannels)
        {
            SipTransport sipTransport = new SipTransport(channel);

            // Hook the events of the SipTransport object
            sipTransport.SipRequestReceived += OnSipRequestReceived;
            sipTransport.SipResponseReceived += OnSipResponseReceived;
            sipTransport.LogSipRequest += OnLogSipRequest;
            sipTransport.LogSipResponse += OnLogSipResponse;
            sipTransport.LogInvalidSipMessage += OnLogInvalidSipMessage;
            m_SipTransports.Add(sipTransport);
            sipTransport.Start();
        }

        Task task = Task.Run(() => { CallManagerTask(m_CancellationTokenSource.Token); });

        m_WaveAudio = new WindowsAudioIo(8000, m_Settings.Devices.AudioDeviceName);
        m_WaveAudio.AudioDeviceStateChanged += OnAudioDeviceStateChanged;
        WaveAudioStatusEnum AudioStatus = m_WaveAudio.StartAudio();

        int ImageWidth = 640;
        int ImageHeight = 480;

        if (m_Settings.Devices.VideoDevice != null)
        {
            CameraCapture = new WindowsCameraCapture(m_Settings.Devices.VideoDevice);
            CameraCapture.FrameBitmapReady += OnFrameBitmapReady;
            ImageWidth = (int) m_Settings.Devices.VideoDevice.DeviceFormat.Width;
            ImageHeight = (int) m_Settings.Devices.VideoDevice.DeviceFormat.Height;

            bool Success = await CameraCapture.StartCapture();
            if (Success == false)
                await ShutdownCameraCapture();
        }

        CameraDisabledCapture = new StaticImageCapture(Ch.TransmitVideoDisabledImageFile!, 30, ImageWidth,
            ImageHeight);
        await CameraDisabledCapture.StartCapture();
        AutoAnswerCapture = new StaticImageCapture(Ch.AutoAnswerVideoFile!, 30, ImageWidth, ImageHeight);
        await AutoAnswerCapture.StartCapture();
        OnHoldCapture = new StaticImageCapture(Ch.CallHoldVideoFile!, 30, ImageWidth, ImageHeight);
        await OnHoldCapture.StartCapture();

        m_SrcManager = new SrcManager(m_Settings.SipRec, m_PortManager, m_Certificate, m_Settings.Identity.AgencyID,
            m_Settings.Identity.AgentID, m_Settings.Identity.ElementID, m_I3LogEventClientMgr, m_Settings.EventLogging.EnableLogging);

        m_SrcManager.Start();

        m_Started = true;
        
    }

    private void OnLogInvalidSipMessage(byte[] msgBytes, IPEndPoint remoteEndPoint, SIPMessageTypesEnum messageType, SipTransport sipTransport)
    {
        // For debug only
        if (messageType == SIPMessageTypesEnum.Request)
        {
            try
            {
                SIPRequest sipRequest = SIPRequest.ParseSIPRequest(Encoding.UTF8.GetString(msgBytes));
                SIPValidationFieldsEnum ValidationEnum;
                string? strError;
                if (sipRequest != null)
                {
                    bool isValid = sipRequest.IsValid(out ValidationEnum, out strError);
                    if (isValid == false)
                    {
                    }
                }    
            }
            catch (SIPValidationException Sve)
            {

            }

        }


        if (m_Settings.EventLogging.EnableLogging == false)
            return;

        MalformedMessageLogEvent Mmle = new MalformedMessageLogEvent();
        Mmle.elementId = m_Settings.Identity.ElementID;
        Mmle.agencyId = m_Settings.Identity.AgencyID;
        Mmle.agencyAgentId = m_Settings.Identity.AgentID;
        Mmle.ipAddressPort = remoteEndPoint.ToString();
        Mmle.ipAddress = remoteEndPoint.Address.ToString();
        Mmle.text = Encoding.UTF8.GetString(msgBytes);
        Mmle.explanationText = $"Unable to parse a SIP message of type: {messageType.ToString()}";
        m_I3LogEventClientMgr.SendLogEvent(Mmle);
    }

    private void OnLoggingServerStatusChanged(string strLoggerName, bool Responding)
    {
        // TODO: handle this event
    }

    private void OnLoggingServerError(HttpResults Hr, string strLoggerName, string strLogEvent)
    {
        // TODO: handle this event
    }

    private bool ValidateClientTlsCertificate(X509Certificate? certificate, X509Chain? chain,
        SslPolicyErrors? sslPolicyErrors)
    {
        return true;
    }

    /// <summary>
    /// Event handler for the FrameBitmapReady event of the WindowsCameraCapture object.
    /// </summary>
    /// <param name="bitmap"></param>
    private void OnFrameBitmapReady(Bitmap bitmap)
    {
        // For debug only
        FrameBitmapReady?.Invoke(bitmap);
        // For debug only
        //if (m_CameraDisabledBitmap != null)
        //    FrameBitmapReady?.Invoke(m_CameraDisabledBitmap);
    }

    public bool TransmitVideoEnabled
    {
        get
        {
            return m_Settings.CallHandling.EnableTransmitVideo;
        }
    }

    /// <summary>
    /// Shuts down the CallManager and releases all resources used by it. Do not attempt to use this
    /// object again after calling this method.
    /// </summary>
    /// <returns></returns>
    public async Task Shutdown()
    {
        if (m_Started == false)
            return;

        m_CancellationTokenSource.Cancel();

        m_I3LogEventClientMgr.LoggingServerError -= OnLoggingServerError;
        m_I3LogEventClientMgr.LoggingServerStatusChanged -= OnLoggingServerStatusChanged;
        m_I3LogEventClientMgr.Shutdown();

        foreach (SipTransport sipTransport in m_SipTransports)
        {
            // Unhook the events
            sipTransport.SipRequestReceived -= OnSipRequestReceived;
            sipTransport.SipResponseReceived -= OnSipResponseReceived;
            sipTransport.LogSipRequest -= OnLogSipRequest;
            sipTransport.LogSipResponse -= OnLogSipResponse;
            sipTransport.LogInvalidSipMessage -= OnLogInvalidSipMessage;

            sipTransport.Shutdown();
        }

        m_SipChannels.Clear();
        m_SipTransports.Clear();

        if (m_WaveAudio != null)
        {
            m_WaveAudio.StopAudio();
            m_WaveAudio.AudioDeviceStateChanged -= OnAudioDeviceStateChanged;
            m_WaveAudio = null;
        }

        if (CameraCapture != null)
        {
            await ShutdownCameraCapture();
            CameraCapture = null;
        }

        if (CameraDisabledCapture != null)
        {
            await CameraDisabledCapture.StopCapture();
            CameraDisabledCapture = null;
        }

        if (AutoAnswerCapture != null)
        {
            await AutoAnswerCapture.StopCapture();
            AutoAnswerCapture = null;
        }

        if (OnHoldCapture != null)
        {
            await OnHoldCapture.StopCapture();
            OnHoldCapture = null;
        }

        m_Started = false;

        if (m_SrcManager != null)
            await m_SrcManager.Shutdown();
    }

    private async Task ShutdownCameraCapture()
    {
        if (CameraCapture == null) return;

        CameraCapture.FrameBitmapReady -= OnFrameBitmapReady;
        await CameraCapture.StopCapture();
        CameraCapture = null;
    }

    private void OnAudioDeviceStateChanged(bool Connected)
    {
        // TODO: Handle this
    }

    private const int WaitIntervalMsec = 100;
    private SemaphoreSlim m_Semaphore = new SemaphoreSlim(0, int.MaxValue);

    private const int TryingIntervalMs = 100;
    private const int RingingIntervalMs = 5000;

    private void CallManagerTask(CancellationToken cancellationToken)
    {
        CancellationToken token = cancellationToken;
        try
        {
            while (token.IsCancellationRequested == false)
            {
                m_Semaphore.Wait(WaitIntervalMsec);

                DoTimedEvents();

                while (token.IsCancellationRequested == false && m_WorkQueue.TryDequeue(out Action? action) == true)
                {
                    if (action != null)
                        action();
                }

            } // end while
        }
        catch (Exception ex)
        {
            SipLogger.LogError(ex, "Unexpected exception.");
        }
    }

    private void DoTimedEvents()
    {
        DateTime Now = DateTime.Now;
        Call[] calls = m_Calls.Values.ToArray();

        CallHandlingSettings Chs = m_Settings.CallHandling;
        if (calls != null)
        {
            foreach (Call call in calls)
            {
                if (call.IsIncoming == true)
                {
                    if (call.CallState == CallStateEnum.Trying)
                    {
                        if ((Now - call.CallStartTime).TotalMilliseconds > TryingIntervalMs)
                        {
                            call.CallState = CallStateEnum.Ringing;
                            SendRinging(call);
                        }
                    }
                    else if (call.CallState == CallStateEnum.Ringing)
                    {
                        if ((Now - call.LastRingingSentTime).TotalMilliseconds > RingingIntervalMs)
                            SendRinging(call);
                    }
                    else if (call.CallState == CallStateEnum.AutoAnswered)
                        DoAutoAnsweredStateWork(call, Chs, Now);
                    else if (call.CallState == CallStateEnum.OnHold)
                        DoOnHoldStateWork(call, Chs, Now);

                    if (call.PresenceSubscriber != null)
                        CheckSubscription(call, call.PresenceSubscriber);

                    if (call.ConferenceSubscriber != null)
                        CheckSubscription(call, call.ConferenceSubscriber);
                }
            }
        }

    }

    /// <summary>
    /// Checks to see if the subscription needs to be renewed
    /// </summary>
    /// <param name="call"></param>
    /// <param name="subscriberData"></param>
    private void CheckSubscription(Call call, SubscriberData subscriberData)
    {
        DateTime Now = DateTime.Now;
        if (Now >= (subscriberData.LastSubscribeTime + TimeSpan.FromSeconds(subscriberData.ExpiresSeconds)))
        {   // Its time to send a new SUBSCRIBE request
            subscriberData.SubscribeRequest.Header.CSeq += 1;
            // Just send the request and don't wait for a response.
            call.sipTransport.StartClientNonInviteTransaction(subscriberData.SubscribeRequest,
                subscriberData.RemoteEndPoint, null, 500);
            subscriberData.LastSubscribeTime = Now;
        }
    }

    private void DoAutoAnsweredStateWork(Call call, CallHandlingSettings Chs, DateTime Now)
    {
        if (call.RttSender != null && (Now - call.LastAutoAnsweredTextSentTime).TotalSeconds >
            Chs.AutoAnswerTextMessageRepeatSeconds && string.IsNullOrEmpty(
                Chs.AutoAnswerTextMessage) == false)
        {
            call.SendRttMessage(Chs.AutoAnswerTextMessage + "\r\n");
            call.LastAutoAnsweredTextSentTime = Now;
        }
        else if (call.MsrpConnection != null && (Now - call.LastAutoAnsweredTextSentTime).TotalSeconds >
            Chs.AutoAnswerTextMessageRepeatSeconds && string.IsNullOrEmpty(Chs.AutoAnswerTextMessage) == false)
        {
            call.SendTextPlainMsrp(Chs.AutoAnswerTextMessage);
            call.LastAutoAnsweredTextSentTime = Now;
        }
    }

    private void DoOnHoldStateWork(Call call, CallHandlingSettings Chs, DateTime Now)
    {
        if (call.RttSender != null && (Now - call.LastOnHoldTextSentTime).TotalSeconds >
            Chs.CallHoldTextMessageRepeatSeconds && string.IsNullOrEmpty(
                Chs.CallHoldTextMessage) == false)
        {
            call.SendRttMessage(Chs.CallHoldTextMessage + "\r\n");
            call.LastOnHoldTextSentTime = Now;
        }
        else if (call.MsrpConnection != null && (Now - call.LastOnHoldTextSentTime).TotalSeconds >
            Chs.CallHoldTextMessageRepeatSeconds && string.IsNullOrEmpty(Chs.CallHoldTextMessage) == false)
        {
            call.SendTextPlainMsrp(Chs.CallHoldTextMessage);
            call.LastOnHoldTextSentTime = Now;
        }
    }

    private void EnqueueWorkItem(Action action)
    {
        m_WorkQueue?.Enqueue(action);
        m_Semaphore.Release();
    }

    /// <summary>
    /// Ends the call identified by the callID parameter
    /// </summary>
    /// <param name="CallID"></param>
    public void EndCall(string CallID)
    {
        EnqueueWorkItem(() => DoEndCall(CallID));
    }

    private void DoEndCall(string CallID)
    {
        Call? call = GetCall(CallID);
        if (call == null)
            return;     // Call already ended

        if (call.CallState == CallStateEnum.OnLine || call.CallState == CallStateEnum.AutoAnswered || 
            call.CallState == CallStateEnum.OnHold)
        {
            SIPRequest ByeRequest = SipUtils.BuildByeRequest(call.InviteRequest!, call.sipTransport!.SipChannel,
                call.RemoteIpEndPoint!, call.IsIncoming, call.LastInviteSequenceNumber, call.OKResponse!);
            
            // TODO: Get the correct SipTransport to used based on the SIP URI in the Contact header
            SipTransport transport = call.sipTransport; // For debug only
            transport.StartClientNonInviteTransaction(ByeRequest, call.RemoteIpEndPoint!, null!, 1000);

            // Notify the application
            CallEnded?.Invoke(CallID);
            EndCall(call);
            // TODO: Cleanup the RtpChannels and MSRP transports -- Terminate the call

        }
        else
        {
            if (call.serverInviteTransaction != null && call.InviteRequest != null && call.sipTransport != null)
            {
                SIPResponse BusyResponse = SipUtils.BuildResponse(call.InviteRequest, SIPResponseStatusCodesEnum.
                    BusyHere, "Busy Here", call.sipTransport.SipChannel, UserName);
                call.serverInviteTransaction.SendResponse(BusyResponse);
                CallEnded?.Invoke(CallID);
                EndCall(call);
            }
        }
    }

    /// <summary>
    /// Puts the call identified by the callID parameter on hold
    /// </summary>
    /// <param name="callID"></param>
    public void PutCallOnHold(string callID)
    {
        EnqueueWorkItem(() => DoPutCallOnHold(callID));
    }

    private void DoPutCallOnHold(string callID)
    {
        Call? call = GetCall(callID);
        if (call == null)
            return;     // The call no longer exists

        if (call.CallState == CallStateEnum.OnHold)
            return;     // Already on-hold

        if (call.CurrentAudioSampleSource != null)
        {
            call.CurrentAudioSampleSource.Stop();
            call.CurrentAudioSampleSource.AudioSamplesReady -= call.AudioSampleSource!.SendAudioSamples;
        }

        if (call.CurrentAudioSampleSource != null && call.AudioDestination != null)
        {
            call.CurrentAudioSampleSource = new FileAudioSource(m_OnHoldAudioSampleData, null!);
            call.CurrentAudioSampleSource.AudioSamplesReady += call.AudioSampleSource!.SendAudioSamples;
            call.CurrentAudioSampleSource.Start();
            call.AudioDestination.SetDestionationHandler(null);
        }

        if (call.VideoSender != null)
        {
            if (call.CurrentVideoCapture != null)
            {
                call.CurrentVideoCapture.FrameReady -= call.VideoSender.SendVideoFrame;
            }

            if (OnHoldCapture != null)
            {
                call.CurrentVideoCapture = OnHoldCapture;
                OnHoldCapture.FrameReady += call.VideoSender.SendVideoFrame;
            }
        }

        if (call.RttSender != null)
            call.LastOnHoldTextSentTime = DateTime.MinValue;

        call.CallState = CallStateEnum.OnHold;
        // Notify the user
        CallStateChanged?.Invoke(GetCallSummary(call));

        if (m_OnLineCall != null && call.CallID == m_OnLineCall.CallID)
        {
            m_OnLineCall = null;
        }
    }

    public void PickupCall(string callID)
    {
        EnqueueWorkItem(() => { DoPickupCall(callID); });
    }

    private void DoPickupCall(string callID)
    {
        Call? call = GetCall(callID);
        if (call == null)
            return;

        if (call.CallState == CallStateEnum.OnLine)
            return;

        // Put the current call on-hold if there is a current call
        if (m_OnLineCall != null)
            DoPutCallOnHold(m_OnLineCall.CallID);

        if (call.CallState == CallStateEnum.Trying || call.CallState == CallStateEnum.Ringing)
        {   // Call not answered yet so answer it
            SIPResponse? OkResponse = BuildOkToIncomingInvite(call);
            if (OkResponse != null)
            {
                call.OKResponse = OkResponse;
                call.serverInviteTransaction!.SendResponse(OkResponse);
                call.serverInviteTransaction = null;
                try
                {
                    StartCall(call);
                }
                catch (Exception ex)
                {
                    SipLogger.LogError(ex, $"Failed to start call for CallID: {call.CallID}");
                }
            }
        }

        SetCallOnLine(call);
    }

    private void StartSipRecRecording(Call call)
    {
        SrcCallParameters parameters = new SrcCallParameters()
        {
            FromUri = call.InviteRequest!.Header.From!.FromURI!,
            ToUri = call.InviteRequest!.Header.To!.ToURI!,
            AnsweredSdp = call.AnsweredSdp!,
            CallRtpChannels = call.RtpChannels,
            CallMsrpConnection = call.MsrpConnection,
            CallId = call.CallID,
            EmergencyCallIdentifier = call.EmergencyCallIdentifier != null ? call.EmergencyCallIdentifier : string.Empty,
            EmergencyIncidentIdentifier = call.EmergencyIncidentIdentifier != null ? call.EmergencyIncidentIdentifier : string.Empty
        };

        if (m_SrcManager != null)
            m_SrcManager.StartRecording(parameters);
    }

    private void StopSipRecRecording(string strCallId)
    {
        if (m_SrcManager != null)
            m_SrcManager.StopRecording(strCallId);
    }

    /// <summary>
    /// Answers the oldest call that is ringing and puts the current call (if there is one) on-hold. If
    /// there are no calls in the ringing state, then this function picks up the oldest call that is
    /// in the auto-answered state.
    /// </summary>
    public void Answer()
    {
        EnqueueWorkItem(() =>
        {
            Call? oldestRinging = null;
            List<Call> ringingCalls = new List<Call>();
            Call[] calls = m_Calls.Values.ToArray();
            foreach (Call call in calls)
            {
                if (call.CallState == CallStateEnum.Trying || call.CallState == CallStateEnum.Ringing)
                    ringingCalls.Add(call);
            }

            if (ringingCalls.Count == 0)
            {   // There are no calls in the ringing state. Answer the oldest call that is in the
                // auto-answered.
                foreach (Call call in calls)
                {
                    if (call.CallState == CallStateEnum.AutoAnswered)
                        ringingCalls.Add(call);
                }
            
            }

            if (ringingCalls.Count == 0)
            {
                CallManagerError?.Invoke("No ringing calls to pickup");
                return;
            }

            oldestRinging = ringingCalls[0];
            foreach (Call ringingCall in ringingCalls)
            {
                if (ringingCall.CallStartTime < oldestRinging.CallStartTime)
                    oldestRinging = ringingCall;
            }

            if (m_OnLineCall != null)
                DoPutCallOnHold(m_OnLineCall.CallID);

            DoPickupCall(oldestRinging.CallID);
        });
    }

    /// <summary>
    /// Terminates all calls.
    /// </summary>
    public void EndAllCalls()
    {
        EnqueueWorkItem(() =>
        { 
            if (m_Calls.Count == 0)
            {
                CallManagerError?.Invoke("No calls to end");
                return;
            }

            Call[] calls = m_Calls.Values.ToArray();

            foreach (Call call in calls)
            {
                DoEndCall(call.CallID);
            }
        });
    }

    private void SetCallOnLine(Call call)
    {
        foreach (RtpChannel rtpChannel in call.RtpChannels)
        {
            if (rtpChannel.MediaType == MediaTypes.Audio)
            {
                if (call.CurrentAudioSampleSource != null)
                {
                    call.CurrentAudioSampleSource!.Stop();
                    call.CurrentAudioSampleSource.AudioSamplesReady -= call.AudioSampleSource!.SendAudioSamples;
                }

                // Connect audio input from the windows microphone audio source
                call.CurrentAudioSampleSource = m_WaveAudio;
                m_WaveAudio!.AudioSamplesReady += call.AudioSampleSource!.SendAudioSamples;
                call.AudioSampleSource.Start();

                // Connect the audio destination handler
                call.AudioDestination!.SetDestionationHandler(m_WaveAudio.AudioOutSamplesReady);
            }

            // For debug only
            Debug.WriteLine($"In SetCallOnLine(), ThreadID = {Thread.CurrentThread.ManagedThreadId}");
            // TODO: handle video
        }

        CallHandlingSettings Chs = m_Settings.CallHandling;
        if (call.VideoSender != null)
        {   // The call has video
            if (call.CurrentVideoCapture != null)
            {
                call.CurrentVideoCapture.FrameReady -= call.VideoSender.SendVideoFrame;
                call.CurrentVideoCapture = null;
            }

            if (CameraCapture != null && Chs.EnableTransmitVideo == true)
            {
                call.CurrentVideoCapture = CameraCapture;
                CameraCapture.FrameReady += call.VideoSender.SendVideoFrame;
            }
            else if (CameraDisabledCapture != null)
            {
                call.CurrentVideoCapture = CameraDisabledCapture;
                CameraDisabledCapture.FrameReady += call.VideoSender.SendVideoFrame;
            }
        }

        m_OnLineCall = call;
        call.CallState = CallStateEnum.OnLine;
        CallStateChanged?.Invoke(GetCallSummary(call));
    }

    // TODO: Implement this and use it
    private SipTransport? GetSipTransport(SIPURI sipUri)
    {
        SipTransport? transport = null;

        return transport;
    }

    private void SendRinging(Call call)
    {
        if (call.InviteRequest == null || call.sipTransport == null || call.serverInviteTransaction == null)
            return;

        SIPResponse ringing = SipUtils.BuildResponse(call.InviteRequest, SIPResponseStatusCodesEnum.Ringing,
            "Ringing", call.sipTransport.SipChannel, UserName);
        call.serverInviteTransaction.SendResponse(ringing);
        call.LastRingingSentTime = DateTime.Now;
    }

    /// <summary>
    /// Event handler for the SipRequestReceived event of a SipTransport object
    /// </summary>
    /// <param name="sipRequest"></param>
    /// <param name="remoteEndPoint"></param>
    /// <param name="sipTransport"></param>
    private void OnSipRequestReceived(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransport)
    {
        EnqueueWorkItem(() => HandleSipRequestReceived(sipRequest, remoteEndPoint, sipTransport));
    }

    private void HandleSipRequestReceived(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransport)
    {
        switch (sipRequest.Method)
        {
            case SIPMethodsEnum.INVITE:
                ProcessInviteRequest(sipRequest, remoteEndPoint, sipTransport);
                break;
            case SIPMethodsEnum.BYE:
                ProcessByeRequest(sipRequest, remoteEndPoint, sipTransport);
                break;
            case SIPMethodsEnum.ACK:
                
                break;
            case SIPMethodsEnum.CANCEL:
                ProcessCancelRequest(sipRequest, remoteEndPoint, sipTransport);
                break;
            case SIPMethodsEnum.OPTIONS:
                ProcessSipOptionsRequest(sipRequest, remoteEndPoint, sipTransport);
                break;
            case SIPMethodsEnum.INFO:
                SendMethdNotAllowed(sipRequest, remoteEndPoint, sipTransport);
                break;
            case SIPMethodsEnum.NOTIFY:
                ProcessSipNotifyRequest(sipRequest, remoteEndPoint, sipTransport);
                break;
            case SIPMethodsEnum.SUBSCRIBE:

                break;
            case SIPMethodsEnum.PUBLISH:
                SendMethdNotAllowed(sipRequest, remoteEndPoint, sipTransport);
                break;
            case SIPMethodsEnum.REFER:
                SendMethdNotAllowed(sipRequest, remoteEndPoint, sipTransport);
                break;
            case SIPMethodsEnum.MESSAGE:

                break;
            case SIPMethodsEnum.UPDATE:
                // Not used for incoming calls.
                SendMethdNotAllowed(sipRequest, remoteEndPoint, sipTransport);
                break;
            case SIPMethodsEnum.REGISTER:
                SendMethdNotAllowed(sipRequest, remoteEndPoint, sipTransport);
                break;
        } // end switch

    }

    /// <summary>
    /// Event handler for the SipResponseReceived event of a SipTransport object
    /// </summary>
    /// <param name="sipResponse"></param>
    /// <param name="remoteEndPoint"></param>
    /// <param name="sipTransport"></param>
    private void OnSipResponseReceived(SIPResponse sipResponse, SIPEndPoint remoteEndPoint, SipTransport sipTransport)
    {
        EnqueueWorkItem(() => HandleSipResponseReceived(sipResponse, remoteEndPoint, sipTransport));
    }

    private void HandleSipResponseReceived(SIPResponse sipResponse, SIPEndPoint remoteEndPoint, SipTransport sipTransport)
    {

    }

    /// <summary>
    /// Event handler for the LogSipRequest event of a SipTransport object
    /// </summary>
    /// <param name="sipRequest"></param>
    /// <param name="remoteEndPoint"></param>
    /// <param name="Sent"></param>
    /// <param name="sipTransport"></param>
    private void OnLogSipRequest(SIPRequest sipRequest, IPEndPoint remoteEndPoint, bool Sent, SipTransport sipTransport)
    {
        if (sipRequest.Method == SIPMethodsEnum.OPTIONS)
            return;

        EnqueueWorkItem(() => { SendCallSignalingEvent(sipRequest.ToString(), sipRequest.Header, remoteEndPoint, Sent, sipTransport); });
    }

    /// <summary>
    /// Event handler for the LogSipResponse event of a SipTransport object
    /// </summary>
    /// <param name="sipResponse"></param>
    /// <param name="remoteEndPoint"></param>
    /// <param name="Sent"></param>
    /// <param name="sipTransport"></param>
    private void OnLogSipResponse(SIPResponse sipResponse, IPEndPoint remoteEndPoint, bool Sent, SipTransport sipTransport)
    {
        if (sipResponse.Header.CSeqMethod == SIPMethodsEnum.OPTIONS)
            return;

        EnqueueWorkItem(() => { SendCallSignalingEvent(sipResponse.ToString(), sipResponse.Header, remoteEndPoint, Sent, sipTransport); });
    }

    private void SendCallSignalingEvent(string sipString, SIPHeader header, IPEndPoint remoteEndpoint, bool Sent, SipTransport sipTransport)
    {
        if (m_Settings.EventLogging.EnableLogging == false)
            return;

        CallSignalingMessageLogEvent Csm = new CallSignalingMessageLogEvent();
        Csm.elementId = m_Settings.Identity.ElementID;
        Csm.agencyId = m_Settings.Identity.AgencyID;
        Csm.agencyAgentId = m_Settings.Identity.AgentID;

        Call? call = GetCall(header.CallId);
        // Handle callId and incidentId
        string? EmergencyCallIdentifier = SipUtils.GetCallInfoValueForPurpose(header, "emergency-CallId");
        if (string.IsNullOrEmpty(EmergencyCallIdentifier) == true && call != null)
            Csm.callId = call.EmergencyCallIdentifier;
        else
            Csm.callId = EmergencyCallIdentifier;

        string? EmergencyIncidentIdentifier = SipUtils.GetCallInfoValueForPurpose(header, "emergency-IncidentId");
        if (string.IsNullOrEmpty(EmergencyIncidentIdentifier) == true && call != null)
            Csm.incidentId = call.EmergencyIncidentIdentifier;
        else
            Csm.incidentId = EmergencyIncidentIdentifier;

        Csm.callIdSip = header.CallId;
        Csm.ipAddressPort = remoteEndpoint.ToString();

        Csm.text = sipString;
        Csm.direction = Sent == true ? "outgoing" : "incoming";

        m_I3LogEventClientMgr.SendLogEvent(Csm);
    }

    private void SendCallStartLogEvent(Call call)
    {
        if (m_Settings.EventLogging.EnableLogging == false)
            return;

        CallStartLogEvent Csle = new CallStartLogEvent();
        Csle.direction = call.IsIncoming == true ? "incoming" : "outgoing";
        SetCallLogEventParams(call, Csle);
        m_I3LogEventClientMgr.SendLogEvent(Csle);
    }

    private void SendCallEndLogEvent(Call call)
    {
        if (m_Settings.EventLogging.EnableLogging == false)
            return;

        CallEndLogEvent Cele = new CallEndLogEvent();
        Cele.direction = call.IsIncoming == true ? "incoming" : "outgoing";
        SetCallLogEventParams(call, Cele);
        m_I3LogEventClientMgr.SendLogEvent(Cele);
    }

    private void SetCallLogEventParams(Call call, LogEvent logEvent)
    {
        logEvent.elementId = m_Settings.Identity.ElementID;
        logEvent.agencyId = m_Settings.Identity.AgentID;
        logEvent.agencyAgentId = m_Settings.Identity.AgentID;
        logEvent.callId = call.EmergencyCallIdentifier;
        logEvent.incidentId = call.EmergencyIncidentIdentifier;
        logEvent.callIdSip = call.CallID;
        logEvent.ipAddressPort = call.RemoteIpEndPoint?.ToString();
    }

    private void ProcessSipNotifyRequest(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransport)
    {
        if (string.IsNullOrEmpty(sipRequest.Header.Event) == true)
        {   // Error -- No Event header so this is a bad request

            return;
        }

        string Event = sipRequest.Header.Event;
        if (Event == SubscriptionEvents.Presence || Event == SubscriptionEvents.Conference)
        {
            Call? call = GetCall(sipRequest.Header.CallId);
            SIPResponse response;
            if (call == null)
                // The call does not exist
                response = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.CallLegTransactionDoesNotExist,
                    "Call Does Not Exist", sipTransport.SipChannel, UserName);
            else
                response = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.Ok,
                    "OK", sipTransport.SipChannel, UserName);

            // Send the response, don't care about the result of the transaction.
            sipTransport.StartServerNonInviteTransaction(sipRequest, remoteEndPoint.GetIPEndPoint(),
                null, response);

            if (call == null)
                return;

            if (Event == SubscriptionEvents.Presence)
                call.ProcessPresenceNotify(sipRequest);
            else if (Event == SubscriptionEvents.Conference)
            {
                // TODO: handle the conference event package.
            }
        }
        else
        {

        }

    }

    /// <summary>
    /// Handles a SIP OPTIONS request message by always responding with a 200 OK response
    /// </summary>
    /// <param name="sipRequest"></param>
    /// <param name="remoteEndPoint"></param>
    /// <param name="sipTransport"></param>
    private void ProcessSipOptionsRequest(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransport)
    {
        // Just send the response directly using the SIPChannel because its not really necessary
        // to use a server transaction to send it.
        SIPResponse OkResponse = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.Ok,
            "OK", sipTransport.SipChannel, UserName);
        sipTransport.SendSipResponse(OkResponse, remoteEndPoint);
    }

    private void SendMethdNotAllowed(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransport)
    {
        SIPResponse Response = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.MethodNotAllowed,
            "Not Allowed", sipTransport.SipChannel, UserName);
        sipTransport.SendSipResponse(Response, remoteEndPoint);
    }
    
    /// <summary>
    /// Gets the Call object for a specified call ID
    /// </summary>
    /// <param name="callID">Call-ID header value for the call.</param>
    /// <returns>Returns the Call object if it exists or null if it does not</returns>
    public Call? GetCall(string callID)
    {
        if (string.IsNullOrEmpty(callID)) 
            return null;
        else
            return m_Calls.GetValueOrDefault(callID);
    }

    /// <summary>
    /// Processes a SIP INVITE request
    /// </summary>
    /// <param name="sipRequest"></param>
    /// <param name="remoteEndPoint"></param>
    /// <param name="sipTransport"></param>
    private void ProcessInviteRequest(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransport)
    {
        string? callId = sipRequest.Header.CallId;
        if (string.IsNullOrEmpty(callId) == true)
            return;

        // Determine if this is a new call or an existing one
        Call? call = GetCall(sipRequest.Header.CallId);

        if (call != null)
        {   // TODO: Its an existing call so handle the re-INVITE

        }
        else
        {   // Its a new call
            if (m_Calls.Count >= m_Settings.CallHandling.MaximumCalls)
            {
                SIPResponse BusyResp = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.BusyHere, "Busy Here",
                    sipTransport.SipChannel, UserName);
                sipTransport.StartServerInviteTransaction(sipRequest, remoteEndPoint.GetIPEndPoint(), null,
                    BusyResp);
                return;
            }

            StartNewIncomingCall(sipRequest, remoteEndPoint, sipTransport);
        }
    }

    private void StartNewIncomingCall(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransport)
    {
        if (sipRequest.Header == null || sipRequest.Header.CallId == null)
            return;

        SIPResponse Trying = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.Trying,
            "Trying", sipTransport.SipChannel, UserName);
        ServerInviteTransaction Sit = sipTransport.StartServerInviteTransaction(sipRequest,
            remoteEndPoint.GetIPEndPoint(), OnServerInviteTransactionComplete, Trying);

        Call newCall = Call.CreateIncomingCall(sipRequest, Sit, sipTransport, CallStateEnum.Trying,
            m_Certificate, m_I3LogEventClientMgr, m_Settings.Identity, m_Settings.EventLogging);

        // Treating all incoming calls as emergency calls, so make sure that a call has both an emergency call
        // identifier and an emergency incident identifier.
        // Normally the emergency call identifier and the emergency incident identifier are assigned
        // by the origination service provider or an ESInet ingress SIP gateway such as an LNG or an
        // ingress BCF.
        newCall.EmergencyCallIdentifier = SipUtils.GetCallInfoValueForPurpose(sipRequest.Header,
            "emergency-CallId");
        if (string.IsNullOrEmpty(newCall.EmergencyCallIdentifier) == true)
            newCall.EmergencyCallIdentifier = SipUtils.BuildEmergencyIdUrn("callid", m_Settings.Identity.
                ElementID);
        newCall.EmergencyIncidentIdentifier = SipUtils.GetCallInfoValueForPurpose(sipRequest.Header,
            "emergency-IncidentId");
        if (string.IsNullOrEmpty(newCall.EmergencyIncidentIdentifier) == true)
            newCall.EmergencyIncidentIdentifier = SipUtils.BuildEmergencyIdUrn("incidentid", m_Settings.Identity.
            ElementID);

        m_Calls.TryAdd(sipRequest.Header.CallId, newCall);

        if (m_Settings.CallHandling.EnableAutoAnswer == true)
        {   // Answer the call and configure the media sources
            SIPResponse? OkResponse = BuildOkToIncomingInvite(newCall);
            if (OkResponse != null)
            {
                newCall.CallState = CallStateEnum.AutoAnswered;
                newCall.serverInviteTransaction = null;
                newCall.OKResponse = OkResponse;
                Sit.SendResponse(OkResponse);
                StartCall(newCall);
                AutoAnswer(newCall);
            }
        }
        else
        {
            newCall.CallState = CallStateEnum.Ringing;
        }

        EnqueueWorkItem(() =>  newCall.GetLocationAndAdditionalData());

        // TODO: EIDO stuff

        NewCall?.Invoke(GetCallSummary(newCall));
    }

    private void StartCall(Call call)
    {
        if (call.OfferedSdp == null || call.AnsweredSdp == null)
        {
            // TODO: Notify of this error
            return;
        }

        foreach (MediaDescription OfferedMd in call.OfferedSdp.Media)
        {
            MediaDescription? AnsweredMd = call.AnsweredSdp!.GetMediaType(OfferedMd.MediaType);
            if (AnsweredMd == null)
                break;

            if (AnsweredMd.Port == 0)
                continue;       // This media type was rejected so don't build an RtpChannel for it.

            if (OfferedMd.MediaType == MediaTypes.MSRP)
            {   // Media type is MSRP
                (MsrpConnection? msrpConnection, string? msrpError) = MsrpConnection.CreateFromSdp(
                    OfferedMd, AnsweredMd, call.IsIncoming, m_Certificate!);
                if (msrpConnection != null)
                {
                    call.MsrpConnection = msrpConnection;
                    // Hook the MsrpConnection's events
                    msrpConnection.MsrpMessageReceived += call.OnMsrpMessageReceived;
                    msrpConnection.Start();
                }
                else
                {
                    SipLogger.LogError($"Unable to create an MsrpConnection for CallID = {call.CallID}, " +
                        $"Reason: {msrpError}");
                }
            }
            else
            {   // Media types that use RTP
                (RtpChannel? rtpChannel, string? Error) = RtpChannel.CreateFromSdp(call.IsIncoming, call.OfferedSdp,
                    OfferedMd, call.AnsweredSdp, AnsweredMd, true, UserName);
                if (rtpChannel == null)
                {
                    SipLogger.LogError($"Unable to create a RtpChannel for CallID = {call.CallID}, Reason = " +
                        $"{Error}");
                    continue;
                }

                call.RtpChannels.Add(rtpChannel);

                // TODO: Hook the events and other things

                if (rtpChannel.MediaType == MediaTypes.Audio)
                {
                    MediaDescription? audioMd = call.AnsweredSdp!.GetMediaType(MediaTypes.Audio);
                    if (audioMd != null)
                    {
                        IAudioEncoder? encoder = WindowsAudioUtils.GetAudioEncoder(audioMd);
                        if (encoder != null)
                        {
                            call.AudioSampleSource = new AudioSource(audioMd, encoder, rtpChannel);
                        }
                        else
                        {
                            // TODO: handle this error
                        }

                        IAudioDecoder? decoder = WindowsAudioUtils.GetAudioDecoder(audioMd);
                        if (decoder != null)
                        {
                            call.AudioDestination = new AudioDestination(audioMd, decoder, rtpChannel, null,
                                m_WaveAudio!.SampleRate);
                        }
                        else
                        {
                            // TODO: handle this error
                        }
                    }
                }
                else if (rtpChannel.MediaType == MediaTypes.Video)
                {
                    MediaDescription? videoMd = call.AnsweredSdp!.GetMediaType(MediaTypes.Video);
                    if (videoMd != null)
                    {
                        call.VideoSender = new VideoSender(videoMd, rtpChannel);
                        call.VideoReceiver = new VideoReceiver(videoMd, rtpChannel);
                    }
                }
                else if (rtpChannel.MediaType == MediaTypes.RTT)
                {
                    RttParameters? rttParameters = RttParameters.FromMediaDescription(AnsweredMd);
                    if (rttParameters == null)
                    {
                        // TODO: Handle this error
                        continue;
                    }

                    call.RttSender = new RttSender(rttParameters, rtpChannel.Send);
                    call.RttSender.Start();
                    string? source;
                    if (call.IsIncoming == true)
                    {
                        source = call.InviteRequest?.Header?.From?.FromURI?.User!;
                        if (string.IsNullOrEmpty(source) == true)
                            source = "Caller";
                    }
                    else
                    {
                        source = call.InviteRequest?.Header?.To?.ToURI?.User!;
                        if (string.IsNullOrEmpty(source) == true)
                            source = "Called Party";
                    }

                    call.RttReceiver = new RttReceiver(rttParameters, rtpChannel, source);
                    call.RttReceiver.RttCharactersReceived += call.OnRttCharactersReceived;
                }

                rtpChannel.StartListening();
            }
        } // end foreach MediaDescription 

        call.HookMediaEvents();
        EnqueueWorkItem(() => SendCallStartLogEvent(call));
        StartSipRecRecording(call);
    }

    private void AutoAnswer(Call call)
    {
        foreach (RtpChannel rtpChannel in call.RtpChannels)
        {
            if (rtpChannel.MediaType == MediaTypes.Audio)
            {
                FileAudioSource Fas = new FileAudioSource(m_AutoAnswerAudioSampleData, null!);
                call.CurrentAudioSampleSource = Fas;
                Fas.AudioSamplesReady += call.AudioSampleSource!.SendAudioSamples;
                Fas.Start();
                call.AudioSampleSource.Start();
            }
            else if (rtpChannel.MediaType == MediaTypes.Video)
            {
                if (call.VideoSender != null && AutoAnswerCapture != null)
                {
                    call.CurrentVideoCapture = AutoAnswerCapture;
                    AutoAnswerCapture.FrameReady += call.VideoSender.SendVideoFrame;
                }
            }
            else if (rtpChannel.MediaType == MediaTypes.RTT)
            {
                // TODO: ?
            }
        }

        // Delay sending the auto-answer text message so that there is enough time for the call to
        // any SIPREC recorders be set up.
        call.LastAutoAnsweredTextSentTime = DateTime.Now - TimeSpan.FromSeconds(m_Settings.CallHandling.AutoAnswerTextMessageRepeatSeconds) -
            - TimeSpan.FromMilliseconds(500);
    }

    private void EndCall(Call call)
    {
        EnqueueWorkItem(() => SendCallEndLogEvent(call));
        if (m_SrcManager != null)
            m_SrcManager.StopRecording(call.CallID);

        call.EndCall();

        if (m_OnLineCall != null && m_OnLineCall.CallID == call.CallID)
            m_OnLineCall = null;

        m_Calls.TryRemove(call.CallID, out Call? removedCall);
    }

    /// <summary>
    /// Gets the current call counts.
    /// </summary>
    /// <returns></returns>
    public CallCounts GetCallCounts()
    {
        CallCounts callCounts = new CallCounts();
        ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        EnqueueWorkItem(() =>
            {
                callCounts.TotalCalls = m_Calls.Count;
                Call[] calls = m_Calls.Values.ToArray();
                foreach (Call call in calls)
                {
                    if (call.CallState == CallStateEnum.Ringing || call.CallState == CallStateEnum.Trying)
                        callCounts.Ringing += 1;
                    else if (call.CallState == CallStateEnum.AutoAnswered)
                        callCounts.AutoAnswered += 1;
                    else if (call.CallState == CallStateEnum.OnHold)
                        callCounts.OnHold += 1;
                    else if (call.CallState == CallStateEnum.OnLine)
                        callCounts.OnLine += 1;
                }
                manualResetEvent.Set();
            });

        manualResetEvent.WaitOne();
        return callCounts;
    }

    private CallSummary GetCallSummary(Call call)
    {
        CallSummary summary = new CallSummary();
        summary.CallID = call.CallID;
        summary.From = call.InviteRequest!.Header!.From!.FromURI!.User!;
        summary.StartTime = call.CallStartTime;
        summary.CallState = call.CallState;

        summary.QueueURI = call.InviteRequest.GetQueueUri(); ;

        // TODO: Implement this
        summary.Conferenced = false;

        summary.CallMedia = call.GetMediaTypeDisplayList();

        return summary;
    }

    private SIPResponse? BuildOkToIncomingInvite(Call call)
    {
        string? strSdp = call.InviteRequest?.GetContentsOfType(SipLib.Body.ContentTypes.Sdp);
        Sdp AnswerSdp;

        if (strSdp != null)
        {
            Sdp? OfferedSdp = null;
            try
            {
                OfferedSdp = Sdp.ParseSDP(strSdp);
                call.OfferedSdp = OfferedSdp;
            }
            catch (Exception ex)
            {
                SipLogger.LogError(ex, $"Invalid SDP received from {call.serverInviteTransaction!.RemoteEndPoint}");
                OfferedSdp = null;
            }

            if (OfferedSdp == null)
                return null;
            else
            {
                AnswerSdp = Sdp.BuildAnswerSdp(OfferedSdp, call.sipTransport!.SipChannel.SIPChannelEndPoint!.
                    GetIPEndPoint().Address, m_AnswerSettings);
                call.AnsweredSdp = AnswerSdp;
            }
        }
        else
            // No SDP provided in the INVITE request so this is an offer-less invite
            AnswerSdp = BuildOfferlessAnswerSdp(call);

        string strAnswerSdp = AnswerSdp.ToString();
        SIPResponse OkResponse = SipUtils.BuildOkToInvite(call.InviteRequest!, call.sipTransport!.SipChannel,
            strAnswerSdp, SipLib.Body.ContentTypes.Sdp);

        return OkResponse;
    }

    // TODO: Implement this function
    private Sdp BuildOfferlessAnswerSdp(Call call)
    {
        Sdp AnswerSdp = new Sdp(call.sipTransport!.SipChannel.SIPChannelEndPoint!.Address!, UserName);

        return AnswerSdp;
    }

    private void OnServerInviteTransactionComplete(SIPRequest sipRequest, SIPResponse? sipResponse,
        IPEndPoint remoteEndPoint, SipTransport sipTransport, SipTransactionBase Transaction)
    {

    }


    private void ProcessCancelRequest(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransport)
    {
        Call? call = GetCall(sipRequest.Header.CallId);
        if (call == null || call.IsIncoming == false || call.InviteRequest == null ||call.serverInviteTransaction == null)
        {

            return;
        }

        SIPResponse cancelResponse = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.Ok, "Ok",
            sipTransport.SipChannel, UserName);
        sipTransport.StartServerNonInviteTransaction(sipRequest, remoteEndPoint.GetIPEndPoint(), null!, cancelResponse);

        m_Calls.TryRemove(call.CallID, out call);

        SIPResponse reqTerminated = SipUtils.BuildResponse(call!.InviteRequest!, SIPResponseStatusCodesEnum.
            RequestTerminated, "Request Terminated", call.sipTransport!.SipChannel, UserName);
        call.serverInviteTransaction!.SendResponse(reqTerminated);

        // Notify the application
        CallEnded?.Invoke(call.CallID);
    }

    private void ProcessByeRequest(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransport)
    {
        Call? call = GetCall(sipRequest.Header.CallId);
        SIPResponse ByeResponse;
        if (call == null)
            ByeResponse = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.CallLegTransactionDoesNotExist,
                "Dialog Does Not Exist", sipTransport.SipChannel, UserName);
        else
        {
            ByeResponse = SipUtils.BuildOkToByeOrCancel(sipRequest, remoteEndPoint);
            m_Calls.TryRemove(call!.CallID, out call);
            // TODO: Notify the application
        }

        sipTransport.StartServerNonInviteTransaction(sipRequest, remoteEndPoint.GetIPEndPoint(), null!, ByeResponse);

        if (call != null)
        {
            CallEnded?.Invoke(call!.CallID);
            EndCall(call!);
        }
    }


}
