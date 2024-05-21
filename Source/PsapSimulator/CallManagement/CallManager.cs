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
using System.Text;
using SipLib.RealTimeText;
using SipLib.Msrp;
using Pidf;
using Ng911Lib.Utilities;
using Held;
using HttpUtils;
using SIPSorceryMedia.FFmpeg;

using PsapSimulator.WindowsVideo;
using SIPSorceryMedia.Abstractions;

/// <summary>
/// Class for managing all of the calls for the PsapSimulator application
/// </summary>
public class CallManager
{
    private AppSettings m_Settings;
    private List<SipTransport> m_SipTransports = new List<SipTransport>();
    private string UserName = "PsapSimulator";
    private List<SIPChannel> m_SipChannels = new List<SIPChannel>();
    private X509Certificate2? m_Certificate = null;
    private ConcurrentDictionary<string, Call> m_Calls = new ConcurrentDictionary<string, Call>();

    private CancellationTokenSource m_CancellationTokenSource = new CancellationTokenSource();

    private ConcurrentQueue<Action> m_WorkQueue = new ConcurrentQueue<Action>();

    private MediaPortManager m_PortManager;

    private string m_Fingerprint;

    private SdpAnswerSettings m_AnswerSettings;

    private WindowsAudioIo? m_WaveAudio = null;

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

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="appSettings">Applications settings</param>
    public CallManager(AppSettings appSettings)
    {
        m_Settings = appSettings;
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

        CallHandlingSettings Chs = m_Settings.CallHandling;

    }

    private Bitmap? GetImageBitmap(string? path)
    {
        Bitmap? bitmap = null;
        if (path != null && File.Exists(path) == true)
        {
            try
            {
                bitmap = new Bitmap(path);
            }
            catch (Exception ex)
            {
                SipLogger.LogError(ex, $"Unable to read the bitmap image file: {path}");
            }
        }

        return bitmap;
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
            m_Certificate = new X509Certificate2(m_Settings.CertificateSettings.CertificateFilePath,
                m_Settings.CertificateSettings.CertificatePassword);
        }
        catch (Exception certEx)
        {
            SipLogger.LogCritical(certEx, "Unable to load the X.509 Certificate");
            throw;
        }

        try
        {
            FFmpegInit.Initialise(FfmpegLogLevelEnum.AV_LOG_FATAL, @".\FFMPEG");
        }
        catch (Exception ffmpegEx)
        {
            SipLogger.LogCritical(ffmpegEx, "Unable to initialize the FFMPEG libraries");
            throw;
        }

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
                m_SipChannels.Add(new SIPUDPChannel(Ipe, UserName));
            }

            if (Ns.EnableTcp == true)
            {
                Ipe = new IPEndPoint(Ipr, Ns.SipPort);
                m_SipChannels.Add(new SIPTCPChannel(Ipe, UserName));
            }

            if (Ns.EnableTls == true)
            {
                Ipe = new IPEndPoint(Ipr, Ns.SipsPort);
                m_SipChannels.Add(new SIPTLSChannel(m_Certificate, Ipe, UserName));
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

        m_Started = true;
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

    public async Task Shutdown()
    {
        if (m_Started == false) return;

        m_CancellationTokenSource.Cancel();

        foreach (SipTransport sipTransport in m_SipTransports)
        {
            // Unhook the events
            sipTransport.SipRequestReceived -= OnSipRequestReceived;
            sipTransport.SipResponseReceived -= OnSipResponseReceived;
            sipTransport.LogSipRequest -= OnLogSipRequest;
            sipTransport.LogSipResponse -= OnLogSipResponse;

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
    }

    private async Task ShutdownCameraCapture()
    {
        if (CameraCapture == null) return;

        CameraCapture.FrameBitmapReady -= OnFrameBitmapReady;
        //CameraCapture.FrameBytesReady -= OnFrameBytesReady;
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
                }
            }
        }

    }

    private void DoAutoAnsweredStateWork(Call call, CallHandlingSettings Chs, DateTime Now)
    {
        if (call.RttSender != null && (Now - call.LastRttAutoAnsweredSentTime).TotalSeconds >
            Chs.AutoAnswerTextMessageRepeatSeconds && string.IsNullOrEmpty(
                Chs.AutoAnswerTextMessage) == false)
        {
            call.SendRttMessage(Chs.AutoAnswerTextMessage + "\r\n");
            call.LastRttAutoAnsweredSentTime = Now;
        }
        else if (call.MsrpConnection != null && (Now - call.LastRttAutoAnsweredSentTime).TotalSeconds >
            Chs.AutoAnswerTextMessageRepeatSeconds && string.IsNullOrEmpty(Chs.AutoAnswerTextMessage) == false)
        {
            call.SendTextPlainMsrp(Chs.AutoAnswerTextMessage);
            call.LastRttAutoAnsweredSentTime = Now;
        }
    }

    private void DoOnHoldStateWork(Call call, CallHandlingSettings Chs, DateTime Now)
    {
        if (call.RttSender != null && (Now - call.LastRttOnHoldSentTime).TotalSeconds >
            Chs.CallHoldTextMessageRepeatSeconds && string.IsNullOrEmpty(
                Chs.CallHoldTextMessage) == false)
        {
            call.SendRttMessage(Chs.CallHoldTextMessage + "\r\n");
            call.LastRttOnHoldSentTime = Now;
        }
        else if (call.MsrpConnection != null && (Now - call.LastRttOnHoldSentTime).TotalSeconds >
            Chs.CallHoldTextMessageRepeatSeconds && string.IsNullOrEmpty(Chs.CallHoldTextMessage) == false)
        {
            call.SendTextPlainMsrp(Chs.CallHoldTextMessage);
            call.LastRttOnHoldSentTime = Now;
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

        if (call.CallState == CallStateEnum.OnLine || call.CallState == CallStateEnum.OnLine ||
            call.CallState == CallStateEnum.AutoAnswered || call.CallState == CallStateEnum.OnHold)
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
            call.CurrentAudioSampleSource.AudioSamplesReady -= call.AudioSampleSource!.OnAudioSamplesReady;
        }

        if (call.CurrentAudioSampleSource != null && call.AudioDestination != null)
        {
            call.CurrentAudioSampleSource = new FileAudioSource(m_OnHoldAudioSampleData, null!);
            call.CurrentAudioSampleSource.AudioSamplesReady += call.AudioSampleSource!.OnAudioSamplesReady;
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
            call.LastRttOnHoldSentTime = DateTime.MinValue;

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

    /// <summary>
    /// Answers the oldest call that is ringing and puts the current call (if there is one) on-hold
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
                return;

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
            if (m_Calls.Count == 0) return;
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
            if (rtpChannel.MediaType == "audio")
            {
                if (call.CurrentAudioSampleSource != null)
                {
                    call.CurrentAudioSampleSource!.Stop();
                    call.CurrentAudioSampleSource.AudioSamplesReady -= call.AudioSampleSource!.OnAudioSamplesReady;
                }

                // Connect audio input from the windows microphone audio source
                call.CurrentAudioSampleSource = m_WaveAudio;
                m_WaveAudio!.AudioSamplesReady += call.AudioSampleSource!.OnAudioSamplesReady;
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
                //ProcessCancelRequest(sipRequest, remoteEndPoint, sipTransport);
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
        // For debug only
        if (sipRequest.Method == SIPMethodsEnum.ACK)
        {

        }
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
    public Call? GetCall(string? callID)
    {
        if (string.IsNullOrEmpty(callID)) return null;

        try
        {
            return m_Calls[callID];
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
    }

    /// <summary>
    /// Processes a SIP INVITE request
    /// </summary>
    /// <param name="sipRequest"></param>
    /// <param name="remoteEndPoint"></param>
    /// <param name="sipTransport"></param>
    private void ProcessInviteRequest(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransport)
    {
        string? callId = sipRequest.Header?.CallId;
        if (string.IsNullOrEmpty(callId) == true)
            return;

        // Determine if this is a new call or an existing one
        Call? call = GetCall(sipRequest.Header?.CallId);

        if (call != null)
        {   // TODO: Its an existing call

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
        Call newCall = Call.CreateIncomingCall(sipRequest, Sit, sipTransport, CallStateEnum.Trying);

        // Treating all incoming calls as emergency calls, so make sure that a call has both an emergency call
        // identifier and an emergency incident identifier.
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

        // Get the location information for the incoming call
        string? strPresence = newCall.InviteRequest!.GetContentsOfType(SipLib.Body.ContentTypes.Pidf);
        if (strPresence != null)
            // Location by value was sent
            newCall.Locations.Add(XmlHelper.DeserializeFromString<Presence>(strPresence));

        // Check for location by reference
        foreach (SIPGeolocationHeader Sgh in newCall.InviteRequest.Header.Geolocation)
        {
            if (Sgh.GeolocationField.URI is null)
                continue;   // Not expected 

            SIPURI sghURI = Sgh.GeolocationField.URI;
            if (sghURI.Scheme == SIPSchemesEnum.http || sghURI.Scheme == SIPSchemesEnum.https)
            {   // Do a HELD request to get the dispatch location.
                LocationRequest locRequest = new LocationRequest();
                locRequest.responseTime = "emergencyDispatch";
                locRequest.locationType = new LocationType();
                locRequest.locationType.exact = false;
                locRequest.locationType.Value.Add("any");
                locRequest.device = new HeldDevice();
                locRequest.device.uri = newCall.InviteRequest!.Header!.From!.FromURI!.ToParameterlessString();
                string? strLocReq = XmlHelper.SerializeToString(locRequest);
                if (strLocReq == null)
                    continue;

                Task.Factory.StartNew(async () =>
                {
                    AsyncHttpRequestor Ahr = new AsyncHttpRequestor(m_Certificate, 10000, null);
                    HttpResults results = await Ahr.DoRequestAsync(HttpMethodEnum.POST, sghURI.ToString(),
                        Ng911Lib.Utilities.ContentTypes.Held, strLocReq, true);
                    if (results.StatusCode == HttpStatusCode.OK)
                    {
                        if (results.Body != null && results.ContentType != null && results.ContentType ==
                            Ng911Lib.Utilities.ContentTypes.Held)
                        {
                            LocationResponse Lr = XmlHelper.DeserializeFromString<LocationResponse>(results.Body);
                            if (Lr != null && Lr.presence != null)
                            {
                                // TODO: finish location
                            }
                        }
                    }

                    Ahr.Dispose();
                });

            }
            else if (sghURI.Scheme == SIPSchemesEnum.sip || sghURI.Scheme == SIPSchemesEnum.sips)
            {   // Subscribe to the SIP Presence Event package

            }
        }

        string? strServiceInfo = newCall.InviteRequest!.GetContentsOfType(SipLib.Body.ContentTypes.ServiceInfo);
        ServiceInfoType? serviceInfo = null;
        if (strServiceInfo != null)
        {
            serviceInfo = XmlHelper.DeserializeFromString<ServiceInfoType>(strServiceInfo);
        }

        // TODO: Get the additional data for the call

        // TODO: Notify the application of the new call

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

            if (OfferedMd.MediaType == "message")
            {   // Media type is MSRP
                (MsrpConnection? msrpConnection, string? msrpError) = MsrpConnection.CreateFromSdp(
                    call.OfferedSdp, OfferedMd, call.AnsweredSdp, AnsweredMd, call.IsIncoming, m_Certificate!);
                if (msrpConnection != null)
                {
                    call.MsrpConnection = msrpConnection;

                    // TODO: Hook the MsrpConnection's events
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

                if (rtpChannel.MediaType == "audio")
                {
                    MediaDescription? audioMd = call.AnsweredSdp!.GetMediaType("audio");
                    if (audioMd != null)
                    {
                        SipLib.Media.IAudioEncoder? encoder = WindowsAudioUtils.GetAudioEncoder(audioMd);
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
                else if (rtpChannel.MediaType == "video")
                {
                    MediaDescription? videoMd = call.AnsweredSdp!.GetMediaType("video");
                    if (videoMd != null)
                    {
                        call.VideoSender = new VideoSender(videoMd, rtpChannel);
                        call.VideoReceiver = new VideoReceiver(videoMd, rtpChannel);
                    }
                }
                else if (rtpChannel.MediaType == "text")
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
                }

                rtpChannel.StartListening();
            }
        } // end foreach MediaDescription 

    }

    private void AutoAnswer(Call call)
    {
        foreach (RtpChannel rtpChannel in call.RtpChannels)
        {
            if (rtpChannel.MediaType == "audio")
            {
                FileAudioSource Fas = new FileAudioSource(m_AutoAnswerAudioSampleData, null!);
                call.CurrentAudioSampleSource = Fas;
                Fas.AudioSamplesReady += call.AudioSampleSource!.OnAudioSamplesReady;
                Fas.Start();
                call.AudioSampleSource.Start();
            }
            else if (rtpChannel.MediaType == "video")
            {
                if (call.VideoSender != null && AutoAnswerCapture != null)
                {
                    call.CurrentVideoCapture = AutoAnswerCapture;
                    AutoAnswerCapture.FrameReady += call.VideoSender.SendVideoFrame;
                }
            }
            else if (rtpChannel.MediaType == "text")
            {

            }

        }
    }

    private void EndCall(Call call)
    {
        if (call.RtpChannels.Count > 0)
        {

            if (call.AudioSampleSource != null)
            {
                call.AudioSampleSource.Stop();
                call.AudioSampleSource= null;
            }

            if (call.CurrentAudioSampleSource != null)
            {
                call.CurrentAudioSampleSource.Stop();
                call.CurrentAudioSampleSource= null;
            }

            if (call.VideoSender != null && call.CurrentVideoCapture != null)
            {
                call.CurrentVideoCapture.FrameReady -= call.VideoSender.SendVideoFrame;
            }

            foreach (RtpChannel rtpChannel in call.RtpChannels)
            {
                // TODO: Unhook the events and other things
                if (rtpChannel.MediaType == "text" && call.RttReceiver != null)
                {
                    rtpChannel.RtpPacketReceived -= call.RttReceiver.ProcessRtpPacket;
                }

                rtpChannel.Shutdown();
            }

            call.RtpChannels.Clear();

        }

        if (call.RttSender != null)
        {
            call.RttSender.Stop();
            // TODO: unhook the events
            call.RttSender = null;
        }

        if (call.MsrpConnection != null)
        {
            call.MsrpConnection.Shutdown();
            call.MsrpConnection = null;
        }

        if (call.VideoSender != null)
        {
            if (call.CurrentVideoCapture != null)
                call.CurrentVideoCapture.FrameReady -= call.VideoSender.SendVideoFrame;

            call.VideoSender.Shutdown();
        }

        if (call.VideoReceiver != null)
        {
            call.VideoReceiver.Shutdown();
            call.VideoReceiver = null;
        }

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

    private void OnServerInviteTransactionComplete(SIPRequest sipRequest, SIPResponse sipResponse,
        IPEndPoint remoteEndPoint, SipTransport sipTransport, SipTransactionBase Transaction)
    {

    }

    private void ProcessCancelRequest(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransport)
    {
        Call? call = GetCall(sipRequest.Header?.CallId);
        if (call == null || call.IsIncoming == false || call.InviteRequest == null ||call.serverInviteTransaction == null)
        {

            return;
        }

        SIPResponse cancelResponse = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.Ok, "Ok",
            sipTransport.SipChannel, UserName);
        sipTransport.StartServerNonInviteTransaction(sipRequest, remoteEndPoint.GetIPEndPoint(), null!, cancelResponse);

        m_Calls.TryRemove(call.CallID, out call);

        SIPResponse cancel = SipUtils.BuildResponse(call!.InviteRequest!, SIPResponseStatusCodesEnum.RequestTerminated,
            "Request Terminated", call.sipTransport!.SipChannel, UserName);
        call.serverInviteTransaction!.SendResponse(cancel);

        // Notify the application
        CallEnded?.Invoke(call.CallID);
    }

    private void ProcessByeRequest(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransport)
    {
        Call? call = GetCall(sipRequest.Header?.CallId);
        if (call == null)
            return;

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

        CallEnded?.Invoke(call!.CallID);
        EndCall(call!);
    }


}
