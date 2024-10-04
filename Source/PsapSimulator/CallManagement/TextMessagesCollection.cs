/////////////////////////////////////////////////////////////////////////////////////
//  File:   TextMessagesCollection.cs                               23 Mar 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

namespace PsapSimulator.CallManagement;

/// <summary>
/// Delegate for the MessageAdded event of the TextMessagesCollection.
/// </summary>
/// <param name="index">Index of the message that was added.</param>
public delegate void MessageAddedDelegate(int index);

/// <summary>
/// Delegate for the MessageUpdated event of the TextMessagesCollection.
/// </summary>
/// <param name="index"></param>
public delegate void MessageUpdatedDelegate(int index);

/// <summary>
/// Container class for the MSRP or RTT messages that have been sent and received for a call.
/// </summary>
public class TextMessagesCollection
{
    /// <summary>
    /// Type of text message in this collection
    /// </summary>
    public TextTypeEnum TextType { get; set; }

    /// <summary>
    /// Contains all text messages send or received so far
    /// </summary>
    public List<TextMessage> Messages { get; set; } = new List<TextMessage>();

    /// <summary>
    /// Identifies the source of the last message in the Messages list.
    /// </summary>
    public TextSourceEnum LastSource { get; set; } = TextSourceEnum.Unknown;

    /// <summary>
    /// This event is fired when a new message has been added to the Messages list.
    /// </summary>
    public event MessageAddedDelegate? MessageAdded = null;

    /// <summary>
    /// Event that is fired when a message in the Messages list has been updated.
    /// </summary>
    public event MessageUpdatedDelegate? MessageUpdated = null;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="textType"></param>
    public TextMessagesCollection(TextTypeEnum textType)
    {
        TextType = textType;
    }

    /// <summary>
    /// Adds a new message that was sent to the list of messages.
    /// </summary>
    /// <param name="From"></param>
    /// <param name="message"></param>
    public void AddSentMessage(string From, string message)
    {
        if (TextType == TextTypeEnum.MSRP)
        {
            AddMessage(From, message, TextSourceEnum.Sent);
            LastSource = TextSourceEnum.Sent;
        }
        else
        {   // Its RTT characters
            if (Messages.Count == 0)
            {
                AddMessage(From, message, TextSourceEnum.Sent);
                LastSource = TextSourceEnum.Sent;
            }
            else
            {
                if (LastSource == TextSourceEnum.Sent)
                {   // Add the new characters to the last sent message
                    TextMessage textMessage = Messages.Last();
                    textMessage.Time = DateTime.Now;
                    textMessage.Message += message;
                    MessageUpdated?.Invoke(Messages.Count - 1);
                }
                else
                {
                    AddMessage(From, message, TextSourceEnum.Sent);
                    LastSource = TextSourceEnum.Sent;
                }
            }

            if (message.EndsWith("\r\n") == true)
                LastSource = TextSourceEnum.Unknown;    // Put the next message in a new row
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="From"></param>
    /// <param name="message"></param>
    public void AddReceivedMessage(string From, string message)
    {
        if (TextType == TextTypeEnum.MSRP)
        {
            AddMessage(From, message, TextSourceEnum.Received);
            LastSource = TextSourceEnum.Received;
        }
        else
        {
            if (Messages.Count == 0)
            {
                AddMessage(From, message, TextSourceEnum.Received);
                LastSource = TextSourceEnum.Received;
            }
            else
            {
                if (LastSource == TextSourceEnum.Received)
                {   // Add the new characters to the last received message
                    TextMessage textMessage = Messages.Last();
                    textMessage.Time = DateTime.Now;
                    if (message == "\b")
                    {   // Handle a single backspace character
                        if (textMessage.Message.Length > 0)
                            textMessage.Message = textMessage.Message.Substring(0, textMessage.Message.Length - 1);
                    }
                    else
                        textMessage.Message += message;
                    MessageUpdated?.Invoke(Messages.Count - 1);
                }
                else
                {
                    AddMessage(From, message, TextSourceEnum.Received);
                    LastSource = TextSourceEnum.Received;
                }
            }

            if (message.EndsWith("\r\n") == true)
                LastSource = TextSourceEnum.Unknown;    // Put the next message in a new row
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void ClearLastSource()
    {
        LastSource = TextSourceEnum.Unknown;
    }

    private void AddMessage(string From, string message, TextSourceEnum source)
    {
        TextMessage textMessage = new TextMessage()
        {
            From = From,
            Message = message,
            Time = DateTime.Now,
            Source = source
        };

        Messages.Add(textMessage);
        MessageAdded?.Invoke(Messages.Count - 1);
    }
}

/// <summary>
/// Enumeration for the possible types of text messages
/// </summary>
public enum TextTypeEnum
{
    /// <summary>
    /// Message Session Relay Protocol
    /// </summary>
    MSRP,
    /// <summary>
    /// Real Time Text
    /// </summary>
    RTT
}

/// <summary>
/// Enumeration for possible sources of a text message
/// </summary>
public enum TextSourceEnum
{
    Sent,

    Received,

    Unknown
}