/////////////////////////////////////////////////////////////////////////////////////
//  File:   AmrWbEncoder.cs                                         24 Sep 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace SipLib.Media;

using AmrWbLib;

/// <summary>
/// Class for encoding linear 16-bit PCM samples into AMR-WB data.
/// </summary>
public class AmrWbEncoder : IAudioEncoder
{
    private AmrWb? m_Encoder;

    /// <summary>
    /// Constructor. Initializes the encoder.
    /// </summary>
    public AmrWbEncoder()
    {
        m_Encoder = new AmrWb();
        // Sets up the encoder for a bit rate of 12.65 kbps
        m_Encoder.InitializeEncoder(2, 1);
    }

    /// <summary>
    /// Gets the RTP clock rate in samples/second
    /// </summary>
    public int ClockRate
    {
        get { return 16000; }
    }

    /// <summary>
    /// Gets the audio sample rate in samples/second
    /// </summary>
    public int SampleRate
    {
        get { return 16000; }
    }

    /// <summary>
    /// Amount to increment the RTP packet Time Stamp field by for each new packet.
    /// </summary>
    public uint TimeStampIncrement
    {
        get { return 320; }
    }

    /// <summary>
    /// Closes the encoder
    /// </summary>
    public void CloseEncoder()
    {
        if (m_Encoder != null)
            m_Encoder.CloseEncoder();

        m_Encoder = null;
    }

    /// <summary>
    /// Encodes linear 16-bit PCM samples into AMR-WB data to send in an RTP packet.
    /// </summary>
    /// <param name="InputSamples">Input linear 16-bit PCM samples</param>
    /// <returns>Returns the encoded AMR-WB bytes</returns>
    public byte[] Encode(short[] InputSamples)
    {
        byte[] encodedBytes = m_Encoder!.EncodePacketSamples(InputSamples);
        return encodedBytes;
    }
}
