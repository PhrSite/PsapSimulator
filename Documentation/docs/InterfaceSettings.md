# Interface Settings

## SIPREC Media Recording
The PsapSimulator application is able to connect to one or more active SIP recording servers as a SIPREC client and can record multiple multi-media calls simultaneously.

This button displays the SIPREC Settings dialog box that allows you to add, edit and delete the settings for each SIPREC server.

See [SIPREC Settings](SipRecSettings.md) for how to set up SIPREC recording.

## NG9-1-1 Event Logging
The PsapSimulator application is capable of sending NG9-1-1 log events to one or more NG9-1-1 Event Logging servers.

This button disaplays the NG9-1-1 Settings dialog box that allows you to add, edit and delete the settings for each NG9-1-1 logging server.

See [NG9-1-1 Event Logging Settings](EventLoggingSettings.md) for how to set up NG9-1-1 Event Logging.

## Test Call Settings
The PsapSimulator is capable of processing NG9-1-1 test calls sent to it from a test call generator functional element.

This button displays the Test Call Settings dialog box that allows you to configure how this application handles incoming test calls.

See [Test Call Settings](TestCallSettings.md) for how to configure the PsapSimulator application for test calls.

## <a name="CadIfSettings">CAD IF/EIDO Server Settings</a>
This button displays a window that displays the URLs that the PsapSimulator application is configured to listen on for the CAD I/F.

There are no configurable settings for the CAD I/F. This dialog shows the URLs so that you can configure a CAD system to subscribe to the PsapSimulator's EIDO server.

The PsapSimulator application can listen on IPv4 and/or IPv6 addresses depending on which address families are enabled in the [Network](NetworkSettings.md) tab of the settings dialog box.

The Copy buttons copy the CAD I/F URL string to the Windows clipboard so that you can paste it into the application that implements the client side (subscriber) of the NG9-1-1 CAD/IF subscribe/notify protocol.
