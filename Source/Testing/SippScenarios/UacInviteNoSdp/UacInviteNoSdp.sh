#!/bin/bash
# Sends a single SIP call to an IP address with no SDP
if [ $# -ne 1 ] ; then
echo "usage: UacInviteNoSdp.sh IPv4Address"
exit 1
fi
sipp $1 -m 1 -rtp_echo -sf sipp_uac_invite_no_sdp.xml

