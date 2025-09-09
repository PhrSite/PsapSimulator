# Introduction
This project is a Visual Studio Windows Forms application that targets .NET 9. It may be run on Windows 10 or later.

The PsapSimulator application is a test program. The intended uses of this application are:
1. Assist in interoperability testing of Next Generation 9-1-1 (NG9-1-1) call Emergency Services IP Network (ESInet) functional elements that deliver NG9-1-1 calls to NG9-1-1 capable Public Safety Answering Points (PSAPs).
2. Provide a way to perform integration testing of the various NG9-1-1 interfaces that have been implemented in the [SipLib](https://github.com/PhrSite/SipLib), [Ng911Lib](https://github.com/PhrSite/Ng911Lib), [EidoLib](https://github.com/PhrSite/EidoLib) and [Ng911CadIfLib](https://github.com/PhrSite/Ng911CadIfLib) open source class libraries.
3. Provide a proof of concept implementation of the less commonly implemented interfaces specified in the most recent version of the NENA i3 Standard for Next Generation 9-1-1 Standard (NENA-STA-010.3f).

This application is a simplified PSAP call handling functional element. It is a single call taker position application that can handle multiple calls simultaneously, but the call taker can only communicate with a single caller at a time. There is no centralized PSAP call controller so functions such as automatic call distribution, call queue pickup, call takeover, barge-in, local transfers (within the same PSAP), local conferences, administrative (non-emergency, i.e. an interface to an agency’s PBX) call handling and other functions that are normally expected in a PSAP application will not be available.

The following block diagram shows the NG9-1-1 functional elements that the PsapSimulator application can interface to.

![PsapSimulator Block Diagram](PsapSimulatorBlockDiagram.jpg)

This application currently only handles incoming calls. All calls are treated as NG9-1-1 calls regardless of the origin of the call.

# NG9-1-1 Functional Element Interface Support
Section 4.6 of NENA-STA-010.3f specifies which functional element interfaces that the PSAP call handling functional element must support.

The following table shows which interfaces and the degree of support that this application provides. The degree of support is indicated in the “Supported?” column.

| NENA-STA-010.3f Section | Supported? | Implemented? | Description |
|-------------------------|------------|-------------|--------------|
| 4.6.1 SIP Call Interface | Full Support | Yes |  |
| 4.6.2 Media | Full Support | Yes | This application supports multimedia calls with any combination of audio, video, Real Time Text (RTT) and MSRP text |
| 4.6.3 LoST Interface | Full Support | No |   |
| 4.6.4 LIS Interfaces | Full Support | Yes |  |
| 4.6.5 Bridge Interface | Yes | Yes | This section of NENA-STA-010.3f states that the PSAP MAY provide its own conference bridge. The PsapSimulator application does not provide its own conference bridge but it does support call conferencing and call transfers via an external conference aware user agent (i.e. a conference bridge). |
| 4.6.6 Element State | Full Support | Yes |  |
| 4.6.7 Service State | Full Support | Yes |  |
| 4.6.8 Abandoned Call Event | No | No | Future |
| 4.6.9 De-queue Registration | Full Support | No |  |
| 4.6.10 Queue State | Full Support | Yes |  |
| 4.6.11 SI | No | No | Support for the Spatial Interface of a GIS server is optional in NENA-STA-010.3f. |
| 4.6.12 Logging Service | Full Support | Yes |  |
| 4.6.13 Security Posture | Full Support | Yes |  |
| 4.6.14 Policy | No | No | Support for the Policy Store is optional in NENA-STA-010.3f. |
| 4.6.15 Additional Data Dereference | Full Support | No |  |
| 4.6.16 Time Interface | Yes | No | Via the Windows NTP Interface |
| 4.6.17 Test Call | Full Support | Yes |  |
| 4.6.18 Testing of Policy Rules | No | No | Support for this function is optional in NENA-STA-010.3f. |
| 4.6.19 Call Diversion | Yes | No | Because De-Queue Registration will be supported. Element State, Service State and Queue state are already implemented. |

# Documentation
The GitHub documentation pages for this application are located at [https://phrsite.github.io/PsapSimulator](https://phrsite/github.io/PsapSimulator). This site contains the operator's manual for this application.

The software requirements specification for this application is located in the Documents/Requirements directory.

A document that describes the current software status is located in the Documents/Status directory.

# Installation


# Dependancies
The PsapSimulator project uses the following NG9-1-1 related NuGet packages.

1. SipLib (0.0.4)
2. Ng911Lib (2.0.0)
3. Ng911CadIfLib (1.2.0)
4. SipRecClient (1.0.0)
5. EidoLib (1.0.1)

The PsapSimulator project uses the following general purpose NuGet packages.
1. Microsoft.Extensions.Logging (8.0.0)
2. NAudio (2.2.1)
3. Serilog (3.1.1)
4. Serilog.Extensions.Logging (8.0.0)
5. Serilog.Sinks.File (5.0.0)
6. SIPSorceryMedia.FFmpeg (8.0.10)

## FFMPEG Libraries
This project depends on the FFMPEG libraries for video codecs and other video related functions. These DLL files are automatically installed in the FFMPEG directory located under the applications installation directory.

This project contains the FFMPEG DLL files that it needs in the FFMPEG directory. These DLL files are distrubuted automatically when the project is published.

The application currently uses version 7.0.0 of FFMPEG. The version of the FFMPEG DLL files must match the version of FFMPEG.AutoGen. If FFMPEG.AutoGen is updated, the FFMPEG DLL files in this repository must also be updated.

The best place to get the correct version of the FFMPEG DLL files is: https://github.com/Ruslan-B/FFmpeg.AutoGen/tree/master/FFmpeg/bin/x64.

# Project Structure

| Directory | Description |
|--------|--------|
| docs   | Documention files generated by docfx for this project |
| Documentation | Markdown and YML source files used by docfx to generate the documentation file in the docs directory |
| Documents/Requirements | Contains the software requirement specification for the PsapSimulator application. |
| Documents/Status | Contains a document that describes the current status of the software for the PsapSimulator. This document indicates which functionality has been completed and which requirements have yet to be implemented and tested. |
| PsapSimulatorSetup | This directory contains a Visual Studio Setup project that can be used to build an MSI package for installation of the PsapSimulator application. |
| Source | All source code is located in subdirectories  |
| Source/PsapSimulator | Source code and Visual Studio project files for the PsapSimulator applicaton |
| Source/Testing | Contains various projects that can be used to test the PsapSimulator  |
| Source/Testing/SimpleCadIfClient | This project simulates a Computer Aided Dispatch (CAD) system that subscribes to a PSAP to receive Emergency Incident Data Objects (EIDOs). It implements the client side (i.e., the subscriber) of the NENA EIDO Conveyance protocol specified in NENA-STA-024.1a. |
| Source/Testing/SimpleLoggingServer | This project simulates an NG9-1-1 Event Logging Server and can be used to test the NG9-1-1 Log Events produced by the PsapSimulator application. |
| Source/Testing/SippScenarious | Contains various SIPPp scenario files that have been used to test the PsapSimulator application. |
| Source/Testing/SrsSimulator | This project implements a simple SIP Recording Server (SRS). It is capable of recording multimedia NG9-1-1 calls. |
| Source/Testing/StateSubscriber | This project simulates the way an NG9-1-1 Emergency Services Routing Proxy (ESRP) or other functional elements subscribe to the Element State, Service State and Queue State NG9-1-1 event packages. |



