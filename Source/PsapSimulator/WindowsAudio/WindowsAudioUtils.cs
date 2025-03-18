/////////////////////////////////////////////////////////////////////////////////////
//  File:   WindowsAudioUtils.cs                                    5 Mar 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

using NAudio.Wave;
using SipLib.Media;
using SipLib.Sdp;

namespace WindowsWaveAudio;

internal class WindowsAudioUtils
{
    /// <summary>
    /// Reads the audio samples from a WAV file. The file format must be mono, 16 bits/sample linear (PCM)
    /// and the sample rate must be 8000 or 16000 samples per second.
    /// </summary>
    /// <param name="FilePath">Location of the file</param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException">Thrown if the file does not exist.</exception>
    /// <exception cref="ArgumentException">Thrown if unable to read the file or if the file format is
    /// incorrect.</exception>
    public static AudioSampleData ReadWaveFile(string FilePath)
    {
        short[] Samples;
        if (File.Exists(FilePath) == false)
        {
            throw new FileNotFoundException($"Wave file: '{FilePath}' not found");
        }

        WaveFileReader Wfr = new WaveFileReader(FilePath);
        WaveFormat waveFormat = Wfr.WaveFormat;
        if (waveFormat == null || waveFormat.Channels != 1 || (waveFormat.SampleRate != 8000 &&
            waveFormat.SampleRate != 16000) ||
            waveFormat.BitsPerSample != 16)
        {
            throw new ArgumentException($"Invalid wave file format for file: '{FilePath}'");
        }

        byte[] buffer = new byte[Wfr.Length];
        //Wfr.Read(buffer, 0, buffer.Length);
        Wfr.ReadExactly(buffer);    // 7 Mar 25 PHR
        Samples = new short[Wfr.SampleCount];

        for (long i = 0; i < Wfr.Length; i += 2)
            Samples[i / 2] = BitConverter.ToInt16(buffer, (int)i);

        AudioSampleData Asd = new AudioSampleData(Samples, waveFormat.SampleRate);
        return Asd;
    }

    /// <summary>
    /// Creates an IAudioEncoder given the negotiated MediaDescription object.
    /// </summary>
    /// <param name="mediaDescription">Input negotiated MediaDescription</param>
    /// <returns>Returns the negotiated IAudioEncoder. Returns null if the encoder is unknown.</returns>
    public static IAudioEncoder? GetAudioEncoder(MediaDescription mediaDescription)
    {
        IAudioEncoder? encoder = null;
        if (mediaDescription.RtpMapAttributes.Count > 0)
        {
            foreach (RtpMapAttribute Rma in mediaDescription.RtpMapAttributes)
            {
                switch (Rma.EncodingName!.ToUpper())
                {
                    case "PCMU":
                        encoder = new PcmuEncoder();
                        break;
                    case "PCMA":
                        encoder = new PcmaEncoder();
                        break;
                    case "G722":
                        encoder = new G722Encoder();
                        break;
                    case "AMR-WB":
                        encoder = new AmrWbEncoder();
                        break;
                }

                if (encoder != null)
                    break;      // Expect only one so pick the first one found
            }
        }

        if (encoder == null)
        {   // Unable to find the encoder to use from the list of rtpmap attributes so try
            // the well known payload (code) numbers
            foreach (int i in mediaDescription.PayloadTypes)
            {
                if (i == 0)
                    return new PcmuEncoder();
                else if (i == 8)
                    return new PcmaEncoder();
                else if (i == 9)
                    return new G722Encoder();
            }
        }

        return encoder;
    }

    /// <summary>
    /// Creates an IAudioDecoder object given the negotiated MediaDescription object.
    /// </summary>
    /// <param name="mediaDescription">Input negotiated MediaDescription</param>
    /// <returns>Returns the negotiated IAudioDecoder. Returns null if the decoder is unknown.</returns>
    public static IAudioDecoder? GetAudioDecoder(MediaDescription mediaDescription)
    {
        IAudioDecoder? decoder = null;
        if (mediaDescription.RtpMapAttributes.Count > 0)
        {
            foreach (RtpMapAttribute Rma in mediaDescription.RtpMapAttributes)
            {
                switch (Rma.EncodingName!.ToUpper())
                {
                    case "PCMU":
                        decoder = new PcmuDecoder();
                        break;
                    case "PCMA":
                        decoder = new PcmaDecoder();
                        break;
                    case "G722":
                        decoder = new G722Decoder();
                        break;
                    case "AMR-WB":
                        decoder = new AmrWbDecoder();
                        break;
                }

                if (decoder != null)
                    break;
            }

        }

        if (decoder == null)
        {   // Unable to find the decoder to use from the list of rtpmap attributes so try
            // the well known payload (code) numbers
            foreach (int i in mediaDescription.PayloadTypes)
            {
                if (i == 0)
                    return new PcmuDecoder();
                else if (i == 8)
                    return new PcmaDecoder();
                else if (i == 9)
                    return new G722Decoder();
            }
        }

        return decoder;
    }
}
