#!/bin/bash
# Sends a single SIP INVITE with the SDP offer in the ACK request
if [ $# -ne 1 ] ; then
echo "usage: UacLateInvite.sh IPv4Address"
exit 1
fi
sipp $1 -m 1 -rtp_echo -sf sipp_uac_late_offer.xml

