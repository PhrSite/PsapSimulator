#!/bin/bash
# Sends a single SIP call to an IP address with Content-Type: application/sdp but no SDP
if [ $# -ne 1 ] ; then
echo "usage: UacInviteWithoutSdp.sh IPv4Address"
exit 1
fi
sipp $1 -m 1 -rtp_echo -sf sipp_uac_invite_without_sdp.xml

