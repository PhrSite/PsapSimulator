#!/bin/bash
# Sends a single SIP INVITE with SDP offer that does not have a c= line
if [ $# -ne 1 ] ; then
echo "usage: UacNoCLineMultipleStreams.sh IPv4Address"
exit 1
fi
sipp $1 -m 1 -rtp_echo -sf sipp_uac_no_c_line_multiple_streams.xml

