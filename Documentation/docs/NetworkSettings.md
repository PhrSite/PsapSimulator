# Network Settings
The Network settings determine which IP transport protocols are enabled and which addresses and ports to use.

## Internet Protocol (IP) Settings
The PsapSimulator application is capable of listening on IPv4 and IPv6.

Both IPv4 and IPv6 may be enabled. At least one of these must be enabled.

The drop down combo box contains a list of addresses for each protocol type. The displayed address is the current selection.

**Note:** Some networks may not support IPv6. In that case IPv6 must be disabled.

## SIP Port Settings
The SIP Port setting specifies the port number that the PsapSimulator application will listen on for SIP. This setting applies to the UDP and TCP transport protocols.

The SIPS Port setting specifies the port number that the PsapSimulator application will listen on for SIP over TLS. This setting applies to the Transport Layer Security (TLS) transport protocol.

The SIP Port and SIPS Port settings must not be the same.

The default setting for the SIP Port setting is 5060. The default setting for the SIPS Port is 5061.

## Enabled Transport Protocols (UDP, TCP, TLS)
The PsapSimulator application is capable of using any combination of the UDP, TCP and TLS transport protocols for SIP. At least one of these protocols must be enabled.

## Media Port Settings
This application allocates media ports within separate ranges for each type of media. A port range is defined by a starting port number and a number of ports.

For audio, video and RTT, the application increments the port number by 2 each time a new port is required. This allows the odd port number to be used for RTCP. This means that the actual number of media connections is 1/2 of the specified number of ports.

For MSRP, the application increments the port number by one because MSRP does have the equivalent of the RTCP protocol.

When the last port number in a port range is used, the application wraps around the next allocated port number to the first port number in the range.

The following table shows the default media port settings.

| Media Type | Start Port | Number of Ports |
|------------|------------|-----------------|
| Audio      | 6000       | 1000 |
| Video      | 7000       | 1000 |
| RTT        | 8000       | 1000 |
| MSRP       | 9000       | 1000 |

The port ranges for audio, video and RTT may not overlap.

The port range for MSRP may overlap the port ranges for audio, video and RTT because MSRP uses TCP and audio, video and RTT use RTP over UDP.

## Use Mutual SIP TLS Authentication
This checkbox determines how the application handles authentication for SIP over TLS (SIPS).

If this checkbox is checked, then the application will require that clients provide an X.509 certificate when the client attempts to connect using TLS. If the client does not provide an X.509 certificate then the connection request will be denied.

**Note:** This application does not validate a client's certificate.

When the application makes an outgoing connection request, it will offer its X.509 certificate when it makes a connection request over TLS if this checkbox is checked.

**Note:** This application does not validate a server's certificate.
