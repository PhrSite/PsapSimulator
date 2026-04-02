# Version History

## 1.2.0 -- 2 Apr 2026

| Issue No. | Change Type | Description |
|--------|--------|-------|
| NA     | Fix    | Changed the message terminator from "\r\n" to "\r" in order to fix the problem of two messages sent via RTT by the same source appearing on the same line in the text list view. |
| NA     | Change | Add New Media form -- Changed the operation to check on click and added a test to prevent the user from selecting two text media types. |
| NA     | Fix    | The application was not setting up transmit audio when the application received a re-INVITE to add audio media to a call. |
| NA     | Fix    | The application was not properly setting the payload type in RTP packets if the application sent a re-INVITE to add media to a call. |
| NA     | Change | The Answer button in the main form now displays the Call Form if a ringing or Auto-Answered call is picked up and put on-line. |
| NA     | Fix    | The user was allowed to put a ringing call on-hold. |
| NA     | Change | Updated the AudoAnswer.jpg static image file. |
| NA     | Change | Updated to use verion 1.0.1 of the SipLib.Video.Window DLL.|
| NA     | Change | Limit the video resolution to VGA for capture so that the application can run reliably on older versions of Windows with low performance hardware on low bandwidth networks when media encryption is enabled. |

## v1.1.0 - 20 Feb 2026
| Issue No. | Change Type | Description |
|--------|--------|-------|
| NA     | Change   | Changed the DisabledCamera.jpg file in the Recordings directory to a head and shoulders image. |
| NA     | Addition | Added the Add Media button to the Current Call dialog. This button allows the user to add a new media type to the call. |
| NA     | Change   | Added an a=record:on attribute to each media description if SIPREC is enabled. |
| NA     | Fix      | Was polling configured and enabled NG9-1-1 Event Loggers even though NG9-1-1 Event Logging was disabled. |
| NA     | Fix      | The number of enabled NG9-1-1 Event Loggers was not properly displayed in the Interfaces tab of the Settings form. |
| NA     | Fix      | A SIPValidationException occurred in the NG9-1-1 Event Logger Settings form if the Logger URI setting was not a valid URI. |
| NA     | Change   | Refactored to use the SipLib.Video.Windows class library NuGet package. |
| NA     | Change   | Refactored to use the SipLib.Audio.Widows class library NuGet package. |
| NA     | Fix      | Was not handling the absence of an Expires header in the OK response to a SUBSCRIBE request for the Presence and Conference event packages. |
| NA     | Addition | Added support for the Advanced Automated Crash Notification (AACN) additional data as described in RFC 8148 and APCO/NENA ANS 2.102.1-2022. |
| NA     | Change   | Upgraded the SipLib NuGet package to v1.0.0. |
| NA     | Change   | Upgraded the SipRecClient NuGet package to v1.1.0. |

## v1.0.0 - 25 Sep 2025
| Issue No. | Change Type | Description |
|--------|--------|-------|
| NA       |  New      | Initial version |



