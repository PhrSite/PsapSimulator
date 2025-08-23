# Introduction
The SimpleLoggingServer application is a Console application that implements a very simple NG9-1-1 Event Logging Server.

Section 4.12.3 of the following standard describes the NG9-1-1 Event Logging protocol.

>[NENA i3 Standard for Next Generation 9-1-1](https://cdn.ymaws.com/www.nena.org/resource/resmgr/standards/NENA-STA-010.3f-2021_i3_Stan.pdf), National Emergency Number Association (NENA) 911 Core Services Committee, i3 Architecture Working Group, NENA-STA-010.3f-2021, December 17, 2024.

This program listens on the first available IPv4 address and uses a fixed port of 11000. It uses the HTTPS transport protocol. It listens on the following paths for requests from one or more NG9-1-1 Event Logging Clients..
- POST: /LogEvents
- GET: /Versions

When this application receives a log event via an HTTP POST request, it decodes the Base64Url encoded log event string into a raw JSON string and writes the raw JSON string to the console window.

Follow these steps to run this application.
1. Open a command prompt window and change directories to the SimpleLoggingServer directory.
2. Type: dotnet run

You can press Ctrl-C to terminate the program.
