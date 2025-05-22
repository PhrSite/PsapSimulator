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

        //for (long i = 0; i < Wfr.Length; i += 2)
        //    Samples[i / 2] = BitConverter.ToInt16(buffer, (int)i);
        // For debug only
        MemoryStream memoryStream = new MemoryStream(buffer);
        BinaryReader binaryReader = new BinaryReader(memoryStream);
        for (long i = 0; i < Wfr.Length / 2; i++)
            Samples[i] = binaryReader.ReadInt16();

        binaryReader.Close();
        memoryStream.Close();
        memoryStream.Dispose();

        AudioSampleData Asd = new AudioSampleData(Samples, waveFormat.SampleRate);
        return Asd;
    }
}
