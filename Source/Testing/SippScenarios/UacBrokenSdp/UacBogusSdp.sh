#!/bin/bash
# Sends a single SIP INVITE with a broken SDP to an IP address
if [ $# -ne 1 ] ; then
echo "usage: UacBrokenSdp.sh IPv4Address"
exit 1
fi
sipp $1 -m 1 -rtp_echo -sf sipp_uac_broken_sdp.xml

