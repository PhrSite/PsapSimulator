# Introduction
The SimpleCadIfClient project is a Visual Studio Console application that subscribes to a NG9-1-1 EIDO server (i.e. an EIDO notifier such as a PSAP system) using the Web Sockets subscribe/notifiy protocol specified in the following NENA standard.

>NENA Standard for the Conveyance of Emergency Incident Data Objects (EIDOs) between Next Generation (NG9-1-1) Systems and Applications, NENA, [NENA-STA-024.1a-2023](https://cdn.ymaws.com/www.nena.org/resource/resmgr/standards/nena-sta-024.1a-2023_eidocon.pdf), January 10, 2023.

This project is a very basic test program that demonstrates how to use the CadIfWebSocketClient class from the [Ng911CadIfLib](https://github.com/PhrSite/Ng911CadIfLib) class library.

When this program receives an EIDO from the server, it simply writes the entire EIDO JSON text document to the console window.

# Running the Application
This sample test program requires that .NET 9 or later be installed on your Windows computer.

To run this program:
1. Change directories to the SimpleCalIfClient directory.
2. Type: dotnet run "wss://IPv4Address:Port/IncidentData/ent", where IPv4Address is the IPv4 IP address of the server and "Port" is the port number that the server is listening on.

You can press the Enter key to terminate the program.

The following shows how to connect to a system using an IPv6 address:
```
dotnet run "wss://[IPv6Address]:Port/IncidentData/ent"
```

If you are using this program to subscribe to EIDOs from the PsapSimulator application, then you must specify 16000 as the port number.
