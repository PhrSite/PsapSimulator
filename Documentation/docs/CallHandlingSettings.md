# Call Handling Settings

## Maximum Calls
This setting specifies how many calls the PsapSimulator will handle simultaneously. When the PsapSimulator application is currently handling the maximum number of calls and receives an INVITE for a new call, then it will reject the call with a 486 Busy Here response.

The default setting is 10.

## Maximum Non-Interactive Calls
Specifies the maximum number of non-interactive calls that the PsapSimulator application will handle.

This feature is not implemented yet.

## Enable Auto Answer
This checkbox enables the Auto Answer function of the PsapSimulator application.

If Auto Answer is enabled then the PsapSimulator application will automatically answer all incoming calls and send pre-recorded media to the caller. Auto answered calls are also recorded if SIPREC recording is enabled. The user can pick up any call that has been auto answered. Calls remain in the auto answered state until the caller ends the call or the PsapSimulator user either picks up the call or it.

This application provides auto-answer media recordings for audio, video, RTT and MSRP. The [Media](MediaSourceSettings.md) tab contains the configuration settings for the auto-answer media.

The default setting is un-checked (auto-answer is disabled).

## Enabled Media Settings
Each media type (audio, video, RTT and MSRP) can be enabled or disabled. If a media type is disabled then the PsapSimulator will reject that media type in its response to an incoming INVITE request. At least one media type must be enabled.

The default setting is enabled for all media types.

### Enable Transmit Video
This setting enables or disables transmit video to the caller. If enabled (checked) then video from the local camera will be sent to a caller when that call is on-line. If disabled then a static image file that indicates that the camera is disabled will be sent to the caller.

The default setting is enabled.

## Outgoing Call Encryption Settings
These settings determine which type of media encryption will be offered when the PsapSimulator makes an outgoing call.

**Note:** The outgoing call feature has not been implemented yet.

The choices for RTP type media (audio, video and RTT) are:
1. None
1. SDES-SRTP
1. DTLS-SRTP

The default setting is None.

The choices for MSRP media are:
1. None
1. MSRP over TLS (MSRPS)

The default setting is None.

## Conference/Transfer Settings Button
This button displays a dialog box that allows the user to pre-configure a list of transfer targets that can be used when setting up a transfer/conference for a call.

The user can add, edit or delete transfer targets.
