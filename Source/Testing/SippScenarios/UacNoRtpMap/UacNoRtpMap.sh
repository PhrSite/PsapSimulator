#!/bin/bash
# Sends a single SIP INVITE with SDP offer that does not have a rtpmap attribute for audio
if [ $# -ne 1 ] ; then
echo "usage: UacNoRtpMap.sh IPv4Address"
exit 1
fi
sipp $1 -m 1 -rtp_echo -sf sipp_uac_no_rtpmap.xml

