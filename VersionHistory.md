# Version History

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



