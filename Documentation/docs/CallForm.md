# The Call Control Window
This window displays information about a single call and allows the user to interact with the caller.

## Basic Call Information
The basic call information includes the From, Media and Call State displays.

The From field displays the user part of the SIP URI from the incoming INVITE request.

The Media display shows the media types available for the call such as Audio, Video, RTT or MSRP.

The Call State display indicates the current state of the call. For example: Ringing, On-Line, On-Hold or Auto-Answered.

### Add Media Button
The Add Media button allows the user to add media to a call. This button displays a dialog box that allows you to select the new media to add to the current call.

Select the new media to add in the pop-up dialog box and then click on the OK button. For example, if the current incoming call has only audio, then it may be possible to add video and one of the types of text media (RTT or MSRP) to the call.

The current call must be in the On-Line state in order to add media to it.

Only media that is enabled in the [configuration settings](/docs/CallHandlingSettings.html#EnabledMedia) of the PsapSimulator application can be added to a call.

Only one type of text media can be added to a call. For example, if a call already has RTT then it will not be possible to add MSRP media to the call.

## Text Messages
The Text Type display indicates the type of text media that is available for the call. This display will show "None" if no text media is available for the call, "RTT" if Real Time Text media is available for the call or "MSRP" if Message Session Relay Protocol media is available for the call.

If RTT or MSRP text media is available, the list box shows the text messages that have been sent and received during this call.

To send a new text message, type it in the New Message text box. If the text type is MSRP you can press the Enter keyboard key or click on the Send button to send the message. For RTT, each character is sent individually as you type them. Press the Enter key to clear the New Message text box in preparation for the next message.

If the text type is MSRP, a check box labeled "Use CPIM" will be visible. Check this checkbox to force the application to send MSRP text messages encapsulated within a CPIM message body. Uncheck this checkbox to send MSRP messages as plain text.

## Location Data
The location of the caller may be provided with the call as a geodetic location, a civic location of as both types of location.

The time display indicates the time that the location data was received by the applicaton.

If location data for the call was provided by-reference then you can click on the Refresh button to request an update for the location data.

| Display Name | Description |
|--------------|-------------|
| Latitude, Longitude | Displays the latitude and longitude in decimal degrees if geodetic location is provided with the call. |
| Location Method | Displays a string that describes how the location information was obtained for the call. For example: GPS, Cell, etc. |
| Radius | If the geodetic location is a circle then this field displays the radius of the circle in meters. |
| Elevation | Displays the elevation in meters above sea level if available. |
| Confidence | Displays a number between 0 and 100 that indicates the confidence level of the accuracy of the geodetic location. |
| Street | Street address of the caller if civic location is provided. This field will contain the house number, street name and any address modifiers provided in the PIDF-LO XML document. |
| City   | Name of the city if a civic location is provided. |
| State  | Name of the state if a civic location is provided. |
| County | Name of the county if a civic location is provided. |
| Location Provider | Contains a string that identifies the provider of the location data. The Providers tab provides details pertaining to each data provider for location and additional data. |

## Subscriber Information
Displays the subscriber's identity and address information from the xCard included in the ServiceInfo additional data block sent with the call.

## Comments
Displays additional data comments received with the call.

## Service/Device Information
Displays information about the service type and the device of the caller received with the call.

| Display Name | Description |
|--------------|-------------|
| Environment  | Describes the Service Environment of the service. Expected values: Business, Residence, Unknown |
| Type         | Describes the type of service. For example: wireless, POTS, MLTS-hosted, MLTS-local, etc. |
| Mobility     | Describes the type of mobility of the caller's service. For example: Mobile, Fixed, Nomadic, Unknnown |
| Provider     | Displays a string that identifies the data provider for the service information. |
| Device Class | Describes the type of device used to make the call. For example: cordless, fixed, satellite, etc. |
| Manufacturer | Name of the manufacturer of the device used to make the call. |
| Model        | Model number of the device used to make the call. |
| Device ID    | Unique identifier of the device used to make the call.  |
| Device Provider | Displays a string that identifies the provider of the device information. |

## Provider Information
This tab displays the following information for each provider of additional data.

| Display Name | Description |
|--------------|-------------|
| Name         | This is the name of the data provider. It should match the provider information in one or more of the additional data blocks. |
| Type         | The type of data provider. For example: Client, Telecom Provider, etc. |
| Contact      | URI at which to contact the data provider for technical support. |

## AACN Information

This tab displays the following Advanced Automated Crash Notification (AACN) information if AACN information is available for the call either by-reference or by-value. The following information is only a small subset of the data about a crash that might be available.

| Display Name | Description |
|--------------|-------------|
| Description  | AACN document description |
| Notifier     | Notifying organization (organization that originated the AACN information) |
| Notifier Contact | Notifying organization contact telephone number |
| Airbag Deployed | Indicates if any airbags in the vehicle were deployed |
| Latitude | Latitude in decimal degrees of the vehicle location as reported in the AACN document |
| Longitude | Longitude in decimal degrees of the vehicle location as reported in the AACN document |

## Video Displays
The large picture box located in the upper right-hand corner of this form displays video received from the caller if the call has video media.

The small picture box located to the left of the large picture box displays the video media that is being sent to the the remote party if the call has video media.

## Conference Information and Controls
It is possible to conference an incoming call if that call's user agent is conference aware. A conference aware user agent is a user agent that is capable of acting as a conference bridge.

Click on the Conference/Transfer button to conference the current call with another remote party and the "Select a Transfer Target" dialog box will appear.

Select the transfer target from the combo box and click on OK.

You can add another participant (transfer target) to the conference by clicking on the Conference/Transfer button again.

The "Drop" button allows you to drop (remove) a selected participant from the conference.

The "Drop Last" button removes the most recently added participant from the conference.

If the call is currently conferenced the Conference Participants list shows the following information for each conference participant.

| Column Name | Description |
|--------------|-------------|
| Participant  | Displays the SIP URI of the participant |
| Media        | Displays a list of the media that the participant is handling (audio, video, etc.) |
| Status       | Indicates the current status of the participant. For example: Connected, Disconnected, etc. |
| Roles        | Indicates the role of the participant. For example: Caller, Call Taker, etc. |


## Call Controls
The following call control buttons are located at the bottom of this window.

### Answer
Answers the call if it is in the Ringing state. Picks up the call if it is in the On-Hold or Auto-Answer states.

The call state changes to On-Line. If there is another call currently in the On-Line state, it is put into the On-Hold state.

### Hold
Puts the current call On-Hold if it is in the On-Line or Auto-Answer states.

### End Call
Ends the call by sending a BYE request if the call is in the Auto-Answered, On-Hold or On-Line states.

If the call is in the Ringing state, then this button rejects the call by sending a 486 Busy Here response.

This button also closes the Call Window.

### Mic Mute
This button mutes or un-mutes audio sent to the remote party if the call has audio media and if the call is in the On-Line, On-Hold or Auto-Answer states.

The button label indicates the current mute status.

### Keypad
Displays a keypad window so the user can send DTMF events via RTP if the call has audio media.

### Close
Closes the this form. The call state is not changed.





