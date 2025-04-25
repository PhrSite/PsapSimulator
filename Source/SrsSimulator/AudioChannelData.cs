/////////////////////////////////////////////////////////////////////////////////////
//  File:   AudioChannelData.cs                                     7 Nov 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

using SipLib.Rtp;
using SipLib.Sdp;
using NAudio.Wave;
using SipLib.Logging;
using SipLib.Media;

namespace SrsSimulator;

/// <summary>
/// Class for recording a single audio stream that is received on an RtpChannel from the SRC.
/// <para>
/// This class supports PCMU, PCMA and G.722 encoded audio and saves the encoded stream as is in a
/// Windows WAV file.
/// </para>
/// </summary>
internal class AudioChannelData : RtpRecordingChannelData
{
    private WaveFileWriter? m_WaveFileWriter = null;

    /// <summary>
    /// Constructor. Hooks the events of the RtpChannel and calls its StartListening() method.
    /// </summary>
    /// <param name="name">String that identifies the call participant that is sending the media for the RTP channel being recorded.
    /// This string is used to name the audio recording file.</param>
    /// <param name="Channel">RtpChannel that is receiving the RTP media from the SRC.</param>
    /// <param name="filePath">Directory in which to save the audio file. Must already exist.</param>
    /// <param name="answerMediaDescription">MediaDescription object that the SRS sent with its OK response
    /// to the INVITE request from the SRC.</param>
    public AudioChannelData(string name, RtpChannel Channel, string filePath, MediaDescription answerMediaDescription) 
        : base(name, Channel)
    {
        int SampleRate = 8000;
        int AverageBytesPerSecond = 8000;

        RtpMapAttribute? audioRtpMap = null;
        // Get the encoding name from the forst RtpMapAttribute that is not for telephone-event.
        // Note: There should only be 1.
        foreach (RtpMapAttribute rtpMapAttribute in answerMediaDescription.RtpMapAttributes)
        {
            if (rtpMapAttribute.EncodingName != "telephone-event")
            {
                audioRtpMap = rtpMapAttribute;
                break;
            }
        }

        if (audioRtpMap == null)
        {
            SipLogger.LogError("Cannot record audio because there are no rtpmap media discription attributes");
            return;
        }

        WaveFormatEncoding encoding = WaveFormatEncoding.Unknown;
        switch (audioRtpMap.EncodingName!.ToUpper())
        {
            case "PCMU":
                encoding = WaveFormatEncoding.MuLaw;
                break;
            case "PCMA":
                encoding = WaveFormatEncoding.ALaw;
                break;
            case "G722":
                encoding = WaveFormatEncoding.WAVE_FORMAT_G722_ADPCM;
                SampleRate = 16000;
                AverageBytesPerSecond = 16000;
                break;
            case "G729":
                m_TranscodeG729ToPcmu = true;
                encoding = WaveFormatEncoding.MuLaw;
                break;
        }

        if (encoding == WaveFormatEncoding.Unknown)
        {
            SipLogger.LogError($"Cannot record video because the {encoding.ToString()} encoding is not supported");
            return;
        }

        WaveFormat format = WaveFormat.CreateCustomFormat(encoding, SampleRate, 1, AverageBytesPerSecond, 1, 8);
        string ChannelFile = Path.Combine(filePath, name + ".wav");

        m_WaveFileWriter = new WaveFileWriter(ChannelFile, format);
        rtpChannel.RtpPacketReceived += OnAudioRtpPacketReceived;
        rtpChannel.StartListening();
    }

    private bool m_TranscodeG729ToPcmu = false;
    private G729Decoder? m_G729Decoder = null;
    private PcmuEncoder? m_PcmuEncoder = null;

    private void OnAudioRtpPacketReceived(RtpPacket rtpPacket)
    {
        if (m_WaveFileWriter == null)
            return;

        byte[]? payload = rtpPacket.Payload;
        if (payload == null || payload.Length == 0)
            return;     // Nothing to write.

        if (m_TranscodeG729ToPcmu == true)
        {
            if (m_G729Decoder == null)
                m_G729Decoder = new G729Decoder();

            if (m_PcmuEncoder == null)
                m_PcmuEncoder = new PcmuEncoder();

            short[] PcmSamples = m_G729Decoder.Decode(payload);
            payload = m_PcmuEncoder.Encode(PcmSamples);
        }

        m_WaveFileWriter.Write(payload, 0, payload.Length);
    }

    /// <summary>
    /// This method must be called when the SIPREC call has ended. This method shuts down the RtpChannel and
    /// closes the WAV file.
    /// </summary>
    public override void StopRecording()
    {
        WaveFileWriter? waveFileWriter = m_WaveFileWriter;
        m_WaveFileWriter = null;

        rtpChannel.RtpPacketReceived -= OnAudioRtpPacketReceived;
        rtpChannel.Shutdown();

        if (waveFileWriter != null)
        {
            waveFileWriter.Flush();
            waveFileWriter.Close();
        }
    }
}
