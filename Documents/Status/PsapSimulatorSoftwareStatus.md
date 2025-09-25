# PsapSimulator Software Status
*Last Updated: 25 Sep 2025*

The software requirements specification for the PsapSimulator application is located in the Documents/Requirements directory.

The following table indicates indicates the current software status for this application. The Section and Title columns refer to the sections in the SRS that contain specific software requirements.

| Section | Title | Status | Description |
|--------|--------|--------|-------------|
| 2.1.1       |  Windows Operating System | Complete |  |
| 2.1.2  | Minimum PC Requirements |  | Not determined yet |
| 2.1.3  | Application Installation Requirements  | Complete  |
| 2.2    | Network Configurations | Complete |   |
| 2.3    | SIP Transport Protocols | Complete |  |
| 2.4    | Media Requirements | Complete |   |
| 2.4.1  | Media Security Requirements | Complete |  |
| 2.4.1.1  | Security Descriptors for SRTP (SDES-SRTP) | Complete |  |
| 2.4.1.2  | Datagram Transport Layer Security for SRTP (DTLS-SRTP) | Complete |   |
| 2.4.1.3  | MSRP Over TLS | Complete |  |
| 2.4.2    | Audio and Video Codecs | Complete |  |
| 2.4.3    | MSRP Connection Mode Requirements | Complete |  |
| 2.4.4    | Quality of Service DSCP Requirements for Media | Complete |  |
| 2.4.5    | Language Support |  |  |
| 3.1      | SIP Call Interface | Complete |  |
| 3.1.1    | Support for re-INVITE Requests | Partially Complete | The ability to change the media destinations during a re-INVITE for conferencing/transfer operations is complete. The ability to add new media to a call has not been implemented yet. |
| 3.1.2    | Offer-Less INVITE Requests | Complete |  |
| 3.1.3    | Outbound Call Interface |  |  |
| 3.1.3.1  | Callback Call Requirements |  |  |
| 3.1.3.2  | Outgoing Call Requirements |  |  |
| 3.1.4    | Quality of Service DSCP Requirements for SIP | Complete |  |
| 3.2      | LoST Client Interface |  |  |
| 3.3      | LIS Interfaces | Complete |  |
| 3.4      | Element State Interface | Complete |  |
| 3.5      | Service State Interface | Complete |  |
| 3.6      | De-Queue Registration |  |  |
| 3.7      | Queue State Interface | Complete |  |
| 3.8.1    | Presence Event Package | Complete |  |
| 3.8.2    | Conference Event Package | Complete |  |
| 3.8.3    | Refer Event Package | Complete |  |
| 3.9.1    | Media Recoding (SIPREC) Requirements | Complete |  |
| 3.9.1.1  | SIPREC Media Recording Configuration Settings | Complete |  |
| 3.9.2    | Event Logging Requirements | Complete |  |
| 3.9.2.1  | Event Logging Configuration Settings | Complete |  |
| 3.10     | Test Call Interface Requirements | Complete |  |
| 3.10.1   | Additional Test Call Functional Requirements | Complete |  |
| 3.11     | Advanced Automatic Crash Notification Calls |  |  |
| 3.12     | Non-Interactive Calls |  |  |
| 3.13     | Conference Bridge Interface | 50% Complete | The "Route All Calls Via a Conference Aware UA" method is complete. The "Ad-hoc" method has not been implemented yet. |
| 3.13.1   | Conference Bridge Configuration Settings |  |  |
| 3.13.2   | Transfer Target Phone Book | Complete |  |
| 3.14     | CAD Interface | Complete |  |
| 3.15     | EIDO Retrieval Service | Complete |  |
| 3.15.1   | Server-Side Requirements | Complete |  |
| 3.15.2   | Client-Side Requirements | Complete |  |
| 3.16     | EIDO Document Handling | Complete |  |
| 4        | Call Handling Requirements | Partially Complete | The add media function has not been implemented yet. |
| 4.1      | Placing Calls on Hold | Complete |  |
| 4.2      | Miscellaneous Special SIP Protocol Requirements | Complete |  |
| 4.3      | Call Identifier and Incident Tracking Identifier Requirements | Complete |  |
| 4.4      | DTMF Digits Transmission Requirements | Complete |  |
| 4.5      | Selected Call Display Requirements | Complete |  |
| 4.5.1    | Basic Call Information | Complete |  |
| 4.5.1.1  | Call Participant Information | Complete |  |
| 4.5.2    | Video Display Requirements | Complete |  |
| 4.5.3    | Text Display | Complete |  |
| 4.5.4    | Location | Complete |  |
| 4.5.5    | Subscriber Information | Complete |  |
| 4.5.6    | Service Information | Complete |  |
| 4.5.7    | Device Information | Complete |  |
| 4.5.8    | Provider Information | Complete |  |
| 4.5.9    | Comments | Complete |  |
| 4.5.10   | AACN Information |  |  |
| 5.1      | Network Settings | Complete |  |
| 5.1.1    | Media Port Ranges | Complete |  |
| 5.2      | Certificate Settings | Complete |  |
| 5.3      | Call Handling Settings | Complete |  |
| 5.3.1    | Maximum Calls | Complete |  |
| 5.3.2    | Non-Interactive Maximum Calls |  |  |
| 5.3.3    | Auto Answer |  Complete |  |
| 5.3.4    | Media Settings | Complete |  |
| 5.3.4.1  | Auto Answer Media Source Settings | Complete |  |
| 5.3.4.2  | Call Hold Media Source Settings | Complete |  |
| 5.3.5    | Transmit Video Disabled Image File | Complete |  |
| 5.3.6    | Enabled Media | Complete |  |
| 5.3.7    | Outgoing Call Media Encryption Settings | Complete |  |
| 5.4      | Audio Device Settings | Complete |  |
| 5.5      | Video Device Settings | Complete |  |
| 5.6      | Identity Settings | Complete |  |
| 5.6.1    | Agency ID | Complete |  |
| 5.6.2    | Agent ID | Complete |  |
| 5.6.3    | Element ID | Complete |  |
| 5.7      | ESRP Settings |  |  |
| 5.7.1    | Enable De-Queue Registration |  |  |
| 5.7.2    | ESRP HTTP or HTTPS URI |  |  |
| 5.7.3    | List of Queues |  |  |
| 5.7.3.1  | Queue Settings |  |  |
| 5.8      | ECRF Settings |  |  |
| 5.9      | Outbound Call Interface Function (OCIF) Settings |  |  |
| 5.9.1    | IP Endpoint |  |  |
| 5.9.2    | SIP Transport Setting |  |  |
| 5.9.3    | Media Encryption Settings |  |  |
| 5.10     | NG9-1-1 Logging Service Settings | Complete |  |
| 5.10.1   | SIPREC Media Recording Settings | Complete |  |
| 5.10.1.1 | SIPREC Recorder Settings | Complete |  |
| 5.10.2   | NG9-1-1 Event Logging Settings | Complete |  |
| 5.10.2.1 | Event Logger Settings | Complete |  |
| 5.11     | Test Call Settings | Complete |  |
| 6        | Application Logging Requirements | Complete |  |


