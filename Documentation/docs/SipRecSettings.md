# SIPREC Recording Settings
This dialog box allows you to add, edit and delete SIP Recording Servers (SRS).

The Enable SIPREC check box enables (if checked) or disables SIPREC recording for the PsapSimulator application. If this box is checked, the application will record media for calls to each enabled SRS.

Click on the Add button to add settings for a new SRS. To edit an existing SRS, double click on the SRS listed in the list box, or select the SRS in the list and then click on the Edit button.

To delete an SRS, select it from the list box and then click on the Delete button.

## <a name="SipRecRecorderSettings">SIPREC Recorder Settings</a>
This dialog box allows you to configure a new SRS or edit the settings of an existing SRS.

### Name
Specifies the name of the SRS. Each SRS must have a unique name.

### Enabled
If checked the SIPREC media recording is enabled for this SRS.

### SRS IP Endpoint
Specifies the IP endpoint for the SRS. Each SRS must have a unique IP endpoint. The IP endpoint may be either an IPv4 or an IPv6 endpoint.

### SIP Transport
This setting specifies the transport to use for this SRS. The choices are UDP, TCP and TLS. The default is TCP.

### Local IP Endpoint
This setting specifies the local IP endpoint that the PsapSimulator will use for SIP and media communications with the SRS. The IP endpoint may be either an IPv4 or an IPv6 endpoint.

There must be a unique local IP endpoint for each SRS. This IP endpoint cannot be the same as that is used to handle incoming and outgoing calls. For example, if the IP endpoint for incoming and outgoing calls is 192.168.1.82:5060, you should use something like 192.168.1.82:5080 for the first SRS, 192.168.1.82:5082 for the second SRS, etc.

The address family (IPv4 or IPv6) for the Local IP Endpoint must be the same as that of the SRS IP Endpoint setting.

### Media Encryption
These settings specify the type of media encryption that the PsapSimulator application will offer to the SRS when it sets up a new SIPREC call.

The available settings for RTP type media (audio, video and RTT) are None, SDES-SRTP and DTLS-SRTP. The default setting is None.

The available settings for MSRP media are None and MSRP over TLS (MSRPS). The default setting is None.

### SIP OPTIONS
These settings determine if and how often the PsapSimulator application will send SIP OPTIONS request to the SRS.

#### Enable
If checked then SIP OPTIONS requests will be sent to the SRS.

#### Interval 
Specifies the interval in seconds that SIP OPTIONS will be sent.
