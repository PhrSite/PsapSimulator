/////////////////////////////////////////////////////////////////////////////////////
//  File:   KeypadForm.cs                                           11 Jul 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator;
using CallManagement;
using SipLib.Media;
using SipLib.Rtp;

/// <summary>
/// Form class that allows the user to send DTMF digits (0-9, * and #).
/// </summary>
public partial class KeypadForm : Form
{
    private Call m_Call;
    private AudioSource m_AudioSource;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="call"></param>
    /// <param name="audioSource"></param>
    public KeypadForm(Call call, AudioSource audioSource)
    {
        m_Call = call;
        m_AudioSource = audioSource;
        InitializeComponent();
    }

    private void KeypadForm_Load(object sender, EventArgs e)
    {
        foreach (DtmfEventEnum digit in m_Call.SentDtmfDigits)
        {
            ShowDtmfDigit(digit);
        }
    }

    private void CloseBtn_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void SaveAndDisplayDtmfDigit(DtmfEventEnum digit)
    {
        m_Call.SentDtmfDigits.Add(digit);
        ShowDtmfDigit(digit);
    }

    private void ShowDtmfDigit(DtmfEventEnum digit)
    {
        string strDtmfDigit = "";
        switch (digit)
        {
            case DtmfEventEnum.One:
                strDtmfDigit = "1";
                break;
            case DtmfEventEnum.Two:
                strDtmfDigit = "2";
                break;
            case DtmfEventEnum.Three:
                strDtmfDigit = "3";
                break;
            case DtmfEventEnum.Four:
                strDtmfDigit = "4";
                break;
            case DtmfEventEnum.Five:
                strDtmfDigit = "5";
                break;
            case DtmfEventEnum.Six:
                strDtmfDigit = "6";
                break;
            case DtmfEventEnum.Seven:
                strDtmfDigit = "7";
                break;
            case DtmfEventEnum.Eight:
                strDtmfDigit = "8";
                break;
            case DtmfEventEnum.Nine:
                strDtmfDigit= "9";
                break;
            case DtmfEventEnum.Asterisk:
                strDtmfDigit = "*";
                break;
            case DtmfEventEnum.Zero:
                strDtmfDigit = "0";
                break;
            case DtmfEventEnum.Pound:
                strDtmfDigit = "#";
                break;
        }

        SentDigitsTb.Text += strDtmfDigit;
    }

    private void OneBtn_Click(object sender, EventArgs e)
    {
        m_AudioSource.SendDtmfEvent(DtmfEventEnum.One);
        SaveAndDisplayDtmfDigit(DtmfEventEnum.One);
    }

    private void TwoBtn_Click(object sender, EventArgs e)
    {
        m_AudioSource.SendDtmfEvent(DtmfEventEnum.Two);
        SaveAndDisplayDtmfDigit(DtmfEventEnum.Two);
    }

    private void ThreeBtn_Click(object sender, EventArgs e)
    {
        m_AudioSource.SendDtmfEvent(DtmfEventEnum.Three);
        SaveAndDisplayDtmfDigit(DtmfEventEnum.Three);
    }

    private void FourBtn_Click(object sender, EventArgs e)
    {
        m_AudioSource.SendDtmfEvent(DtmfEventEnum.Four);
        SaveAndDisplayDtmfDigit(DtmfEventEnum.Four);
    }

    private void FiveBtn_Click(object sender, EventArgs e)
    {
        m_AudioSource.SendDtmfEvent(DtmfEventEnum.Five);
        SaveAndDisplayDtmfDigit(DtmfEventEnum.Five);
    }

    private void SixBtn_Click(object sender, EventArgs e)
    {
        m_AudioSource.SendDtmfEvent(DtmfEventEnum.Six);
        SaveAndDisplayDtmfDigit(DtmfEventEnum.Six);
    }

    private void SevenBtn_Click(object sender, EventArgs e)
    {
        m_AudioSource.SendDtmfEvent(DtmfEventEnum.Seven);
        SaveAndDisplayDtmfDigit(DtmfEventEnum.Seven);
    }

    private void EightBtn_Click(object sender, EventArgs e)
    {
        m_AudioSource.SendDtmfEvent(DtmfEventEnum.Eight);
        SaveAndDisplayDtmfDigit(DtmfEventEnum.Eight);
    }

    private void NineBtn_Click(object sender, EventArgs e)
    {
        m_AudioSource.SendDtmfEvent(DtmfEventEnum.Nine);
        SaveAndDisplayDtmfDigit(DtmfEventEnum.Nine);
    }

    private void AsteriskBtn_Click(object sender, EventArgs e)
    {
        m_AudioSource.SendDtmfEvent(DtmfEventEnum.Asterisk);
        SaveAndDisplayDtmfDigit(DtmfEventEnum.Asterisk);
    }

    private void ZeroBtn_Click(object sender, EventArgs e)
    {
        m_AudioSource.SendDtmfEvent(DtmfEventEnum.Zero);
        SaveAndDisplayDtmfDigit(DtmfEventEnum.Zero);
    }

    private void PoundBtn_Click(object sender, EventArgs e)
    {
        m_AudioSource.SendDtmfEvent(DtmfEventEnum.Pound);
        SaveAndDisplayDtmfDigit(DtmfEventEnum.Pound);
    }

}
