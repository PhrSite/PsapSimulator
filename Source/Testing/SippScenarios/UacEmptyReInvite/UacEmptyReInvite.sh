#!/bin/bash
# Sends a single SIP call to an IP address then sends a re-INVITE with no SDP
if [ $# -ne 1 ] ; then
echo "usage: UacEmptyReInvite.sh IPv4Address"
exit 1
fi
sipp $1 -m 1 -rtp_echo -sf sipp_uac_empty_reinvite.xml

