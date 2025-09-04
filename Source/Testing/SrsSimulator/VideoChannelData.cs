/////////////////////////////////////////////////////////////////////////////////////
//  File:   VideoChannelData.cs                                     9 Nov 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

using SipLib.Rtp;
using SipLib.Sdp;
using SipLib.Video;
using SipLib.Logging;

using System.Diagnostics;

namespace SrsSimulator;

/// <summary>
/// Class for recording a single video stream that is received on an RtpChannel from the SRC.
/// <para>
/// This class writes raw, encoded video frames to a temporary file (*.h264 or *.vp8) and then uses
/// the ffmpeg application to convert the temporary file to an MP4 file. Ffmpeg.exe must be installed on
/// the PC. If it is not, then the temporary file containing the raw, encoded video frames will be saved.
/// </para>
/// </summary>
internal class VideoChannelData : RtpRecordingChannelData
{
    private VideoRtpReceiver? m_VideoReceiver = null;
    private FileStream? m_FileStream = null;
    private string m_RawFilePath = string.Empty;
    private string m_MP4FilePath = string.Empty;

    /// <summary>
    /// Constructor. Hooks the events of the RtpChannel and calls its StartListening() method.
    /// </summary>
    /// <param name="name">String that identifies the call participant that is sending the media for the RTP channel being recorded.
    /// This string is used to name the video recording file.</param>
    /// <param name="Channel">RtpChannel that is receiving the RTP media from the SRC.</param>
    /// <param name="filePath">Specifies the directory in which to save the video file. Must already
    /// exist.</param>
    /// <param name="answerMediaDescription">MediaDescription object that the SRS sent with its OK response
    /// to the INVITE request from the SRC.</param>
    public VideoChannelData(string name, RtpChannel Channel, string filePath, MediaDescription answerMediaDescription)
        : base(name, Channel)
    {
        m_MP4FilePath = Path.Combine(filePath, Name + ".mp4");

        if (answerMediaDescription.RtpMapAttributes.Count == 0)
        {
            SipLogger.LogError("Cannot record video because there are no rtpmap media discription attributes");
            return;
        }

        RtpMapAttribute rtpMapAttribute = answerMediaDescription.RtpMapAttributes[0];
        if (rtpMapAttribute.EncodingName == "H264")
        {
            m_RawFilePath = Path.Combine(filePath, Name + ".h264");
            m_VideoReceiver = new H264RtpReceiver();
        }
        else if (rtpMapAttribute.EncodingName == "VP8")
        {
            m_RawFilePath = Path.Combine(filePath, Name + ".vp8");
            m_VideoReceiver = new VP8RtpReceiver();
        }
        else
        {
            SipLogger.LogError($"Cannot record video because the {rtpMapAttribute.EncodingName} encoding is not supported");
            return;
        }

        m_FileStream = new FileStream(m_RawFilePath, FileMode.OpenOrCreate);
        rtpChannel.RtpPacketReceived += OnVideoRtpPacketReceived;
        rtpChannel.StartListening();
    }

    private void OnVideoRtpPacketReceived(RtpPacket rtpPacket)
    {
        if (m_VideoReceiver == null)
            return;

        byte[]? EncodedFrameData = m_VideoReceiver.ProcessRtpPacket(rtpPacket);
        if (EncodedFrameData != null)
        {   // A complete encoded frame is available.
            if (m_FileStream != null)
                m_FileStream.Write(EncodedFrameData, 0, EncodedFrameData.Length);
        }
    }

    /// <summary>
    /// This method must be called when the SIPREC call has ended. This method shuts down the RtpChannel,
    /// and converts the temporary file containing the raw, encoded video frames to an MP4 video file.
    /// </summary>
    public override void StopRecording()
    {
        rtpChannel.RtpPacketReceived -= OnVideoRtpPacketReceived;
        rtpChannel.Shutdown();
        if (m_FileStream != null)
        {  
            m_FileStream.Close();
            string strParams = $"-i {m_RawFilePath} -c copy {m_MP4FilePath}";

            bool Success = CallCommand("ffmpeg.exe", strParams);

            if (Success == true)
            {
                if (File.Exists(m_RawFilePath) == true)
                    File.Delete(m_RawFilePath);
            }
        }
    }

    private bool CallCommand(string Command, string Params)
    {
        try
        {
            ProcessStartInfo Psi = new ProcessStartInfo();
            Psi.FileName = Command;
            Psi.Arguments = Params;
            Psi.UseShellExecute = false;
            Psi.RedirectStandardOutput = true;
            Psi.RedirectStandardError = true;

            Process Proc = new Process();
            Proc.StartInfo = Psi;

            string standardError = string.Empty;
            // It is necessary to read from the standard error output of the process asynchronously
            // when redirecting both the starndard output and the standard error in order to prevent
            // a deadlock that occurs when reading from both synchronously.
            // See the Remarks section at: https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.redirectstandardoutput?view=net-8.0
            Proc.ErrorDataReceived += new DataReceivedEventHandler((sender, e) =>
                { standardError += e.Data; });

            bool Success = Proc.Start();
            if (Success == true)
            {
                Proc.BeginErrorReadLine();
                string standardOutput = Proc.StandardOutput.ReadToEnd();
                Proc.WaitForExit();
            }

            return Success;
        }
        catch (Exception Ex)
        {
            SipLogger.LogError(Ex, "Exception occurred while launching ffmpeg.");
            return false;
        }
    }
}
