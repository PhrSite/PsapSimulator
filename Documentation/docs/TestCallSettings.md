# Test Call Settings
The PsapSimulator can handle NG9-1-1 test calls from a functional element that generates test calls.. This dialog box allows you to configure how test calls are handled.

## Enable Test Calls
If enabled (checked) the PsapSimulator shall except test calls. If disabled, then the PsapSimulator will respond with a SIP response of 503 Service Not Available if it receives an INVITE request for a test call.

## Maximum Test Calls
This setting specifies the maximum number of concurrent test calls that the PsapSimulator application will handle. If an INVITE request for a test call arrives when the application is handling the maximum number of test calls then it will respond with a SIP 486 Busy Here response.

The default setting is 10.

## Test Call Duration Units
This setting specifies the criteria that the PsapSimulator application will use to terminate a test call. The available choices are:
1. RTP Packets
2. Minutes

The RTP Packets setting means that the application will automatically end a test call after it receives and echoes back a specified number of RTP packets. The Test Call Duration setting determines the number of RTP Packets.

The Minutes setting means that the application will automatically terminate a test call after a specified number of minutes. The Test Call Duration setting determines the number of minutes. 

The default setting is RTP Packets.

## Test Call Duration
If the Test Call Duration Units setting is RTP Packets then this setting specifies the number of RTP packets that the application will echo back before automatically ending the test call. The default number of RTP packets is 3.

If the Test Call Duration Units setting is Minutes, then this setting specifies the number of minutes after which the application will automatically end the test call. The default number of minutes is 1.
