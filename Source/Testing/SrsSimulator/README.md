# Introduction
The SrsSimulator project is a Visual Studio Console application that targets Windows 10 or later.

This is a test program that simulates a SIP Recording Server (SRS) and its intended use is to test applications that implement SIP Recording Client (SRC) functionality.

The protocols implemented by a SRS or a SRC enable active SIP recording. These protocols are often referred to as SIPREC. Implementation of the SIPREC protocols is required by all NG9-1-1 functional elements that manage or handle media (audio, video, Real Time Text and Message Session Relay Protocol).

The following RFCs specify the SIPREC protocol.
1. An Architecture for Media Recording Using the Session Initiation Protocol, IETF, [RFC 7245](https://tools.ietf.org/html/rfc7245), May 2014.
2. Session Recording Protocol, IETF, [RFC 7866](https://tools.ietf.org/html/rfc7866), May 2016.
3. Session Initiation Protocol (SIP) Recording Metadata, IETF, [RFC 7865](https://tools.ietf.org/html/rfc7865), May 2016.
4. Session Initiation Protocol (SIP) Recording Call Flows, IETF, [RFC 8068](https://tools.ietf.org/html/rfc8068), February 2017.

The SrsSimulator supports recording for the following media types.
- Audio: PCMU, PCMA, G722, G729, AMR-WB
- Video: H264 and VP8
- Message Session Relay Protocol (MSRP)
- Real Time Text (RTT)

The SrsSimulator can except connections from multiple SRCs and is capable of recording multiple SIPREC calls simultaneously.

# Running SrsSimulator
The SrsSimulator project targets .NET 9 (or later) so .NET 9 must be installed on your computer.

The SrsSimulator program uses ffmpeg.exe to convert video streams into MP4 files. You can install the FFMPEG application as follows.

```
winget install ffmpeg --force
```

Perform these steps to run the SrsSimulator program.
1. Open a command prompt window
2. Change directories to the SrsSimulator directory
3. Type: dotnet run

The SrsSimulator program listens on the first available IPv4 address and uses TCP for SIP. It used a fixed port number of 5080 for SIP.

The SrsSimulator program writes the SIP URI that it is listening on to the console window so you can use this information to configure an SRC to send SIPREC calls to it.

You can type "quit" (without quotes) to terminate the program.

It is possible to run the SrsSimulator application on the same computer as the SRC under test. However, care must be taken to avoid using the same port numbers that the SrsSimulator uses.

The SrsSimulator application uses port 5080 for SIP over TCP. It uses the following port ranges for RTP media and MSRP media.

| Media Type | Start Port | Number of Ports |
|------------|------------|-----------------|
| Audio      | 10000      | 1000 |
| Video      | 11000      | 1000 |
| RTT        | 12000      | 1000 |
| MSRP       | 12000      | 1000 |

# Recording Storage
The SrsSimulator program stores recordings for SIPREC calls in a directory named "c:\SrsSimulatorRecordings". The following figure shows the directory structure.

```
c:\SrsSimulatorRecordings
    |
    +-YYYY
        |
        +- MM
            |
            +-DD
                |
                +-CallID1
                +-CallID2
                +- ...
```

"YYYY" represents the four digit year. "MM" represents the two digit month and "DD" represents the two digit day of the month.

The recording files for calls are stored in individual directories below the day directory. The name of the directory for each call is the SIP Call-ID of the call.

The following table shows the file names that may be created for each SIPREC recording.

| File Name | Description |
|-----------|-------------|
| \*.wav    | Audio recordings in the Windows WAV file format |
| \*.mp4    | Video recordings in the MP4 file format |
| \*.msrp   | Text recordings for MSRP text media |
| \*.rtt    | Text recordings for RTT text media |
| MetaData.xml | Contains the last received SIPREC metadata XML document |

There will be two files for each type of media in the call. One file contains the media stream sent by the caller and the other contains the media stream sent by the called party. Files are named using the name information in the SIPREC metadata XML document. For instance, if one call participant has a name of 6306826062 and the other call participant has a name of 911, the then audio files will be named 6306826062.wav and 911.wav.

Text media for MSRP and RTT media are stored as JSON text documents. The following table shows the JSON schema.

| Field Name | Type | Description |
|------------|------|-------------|
| TextLines  | Array\<TextLine> | Array of TextLine objects. |

The schema for for a single TextLine object is:

| Field Name | Type | Description |
|------------|------|-------------|
| Timestamp  | String | Timestamp that the text was received. The format is in the NENA timestamp format as specified in Section 2.3 of NENA-STA-010.3 (yyyy-MM-ddTHH:mm:ss.fffffzzz), where fffff is the fractional seconds and zzz is the offset from GMT. For example: 2024-11-18T02:25:01.12345+7:00. |
| From       | String | Identifies the sender of the text |
| Characters | String | Characters that were received. For MSRP this is typically a complete message. For RTT this may be one (typically) or more characters. |

