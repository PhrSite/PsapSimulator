#!/bin/bash
# Sends a single bad SIP MESSAGE request to an IP address
if [ $# -ne 1 ] ; then
echo "usage: BadMessageUac.sh IPv4Address"
exit 1
fi
sipp $1 -m 1 -rtp_echo -sf sipp_uac_bad_message.xml

