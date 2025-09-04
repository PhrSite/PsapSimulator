# Introduction
The StateSubscriber project is a Visual Studio Console application that can be used to test the following NG9-1-1 SIP subscribe/notify event packages.
1. Element State -- See Section 2.4.1 of [NENA-STA-010.3f](https://cdn.ymaws.com/www.nena.org/resource/resmgr/standards/nena-sta-010.3f-2021_i3_stan.pdf).
2. Service State -- See Section 2.4.2 of NENA-STA-010.3f.
3. Queue State -- See Section 4.2.1.3 of NENA-STA-010.3f.

This application subscribes to the above event packages, reports the subscription status and reports changes in state when the application under test (i.e. the notifier) notify it of state changes.

# Running the StateSubscriber Program
This application requires that .NET 9 be installed on you computer.

Follow these steps to run this application
1. Open a console window
2. Change directories to the StateSubscriber directory
2. Type: dotnet run "IPEndPoint LocalPort"

IPEndPoint represents the IP End Point of the application to subscribe to. This can be either an IPv4 endpoint or an IPv6 endpoint.

IPv4 End Point Example: 192.168.1.82:5060

IPv6 End Point Example: [2600:1700:7A40:4740:4350:AFAC:5484:4372]:5060

LocalPort specifies the local TCP port to bind to.

The application will attempt to subscribe to the event packages using SIP over TCP.

You can terminate the program by typing "quit" (without quotes) in the console window.


