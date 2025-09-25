# Media Source Settings
The Media Sources Setting determine the sources of media that the PsapSimulator application will send to the remote party of a call when a call is placed on hold or auto-answered.

## Call Hold Media Source Settings
When a call is placed on-hold, the PsapSimulator application will send pre-recorded media to the remote party for audio, video, RTT and MSRP media.

### Audio Source
This setting determines the type of audio that the application will send when a call is placed on-hold. The choices are:
1. Silence
1. Hold Beep Sound
1. Hold Recording File

If Hold Recording File is selected, then the application will play a recording stored in the file specified by the Audio File text box. This application provides a default audio recording but the user may provide a custom file. See the <a href="#HoldAudioFile">Audio File</a> setting below.

The default setting is Hold Recording File.

#### Hold Beep Sound
The Hold Beep Sound is a sequence of two short "beep" sounds separated by a short silence interval. The tone sequence repeats every five seconds.

The PsapSimulator application uses a fixed file called HoldBeepSound.wav located in the Recordings folder underneath the application's installation folder. The characteristic of this WAV file are:
- Sample Rate: 8000 Hz.
- Sample Format: 16-bit linear
- Number of Channels: 1

### <a name="HoldAudioFile">Audio File</a>
This setting specifies the file path of the WAV file that is played when a call is placed on-hold. This setting applies only when the Audio Source setting is Hold Recording File.

The application provides a default WAV file called CallHoldAudioFile.wav that is located in the Recordings folder underneath the application's installation folder.

The user can provide a custom WAV file by changing this setting. The characteristics of the custom WAV file must be:
- Sample Rate: 8000 Hz. or 16000 Hz.
- Sample Format: 16-bit linear
- Number of Channels: 1

The application plays this file repeatedly to the remote party until the call is picked up again or the call ends.

### Video File
This setting specifies the file path for a static image JPEG file that the application sends to the remote party when a call is placed on-hold.

The default file is called OnHold.jpg and it is located in the Recordings folder underneath the application's installation directory.

The user may replace this default file with another JPEG image file by changing this path setting.

The application supports only static image JPEG files.

### Text Message
The Text Message setting specifies a static text message that is sent periodically to the remote party when a call is placed on-hold. This applies only if a call has RTT or MSRP media.

### Text Repeat
This setting specifies the repeat interval in seconds for the Text Message.

## Auto Answer Media Source Settings
These settings specify the media that is sent to a remote party when a call is auto-answered.

### Audio File
This setting specifies the file path of the WAV file that is played when a call is auto-answered.

The application provides a default WAV file called AutoAnswerAudioFile.wav that is located in the Recordings folder underneath the application's installation folder.

The user can provide a custom WAV file by changing this setting. The characteristics of the custom WAV file must be:
- Sample Rate: 8000 Hz. or 16000 Hz.
- Sample Format: 16-bit linear
- Number of Channels: 1

The application plays this file repeatedly to the remote party until the call is picked up (answered by the user) or the call ends.

### Video File
This setting specifies the file path for a static image JPEG file that the application sends to the remote party when a call is auto-answered.

The default file is called AutoAnswered.jpg and it is located in the Recordings folder underneath the application's installation directory.

The user may replace this default file with another JPEG image file by changing this path setting.

The application supports only static image JPEG files.

### Text Message
The Text Message setting specifies a static text message that is sent periodically to the remote party when a call is auto-answered. This applies only if a call has RTT or MSRP media.

### Text Repeat
This setting specifies the repeat interval in seconds for the Text Message.
