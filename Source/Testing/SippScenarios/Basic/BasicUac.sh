#!/bin/bash
# Sends a single SIP call to an IP address
if [ $# -ne 1 ] ; then
echo "usage: BasicUac.sh IPv4Address"
exit 1
fi
sipp $1 -m 1 -rtp_echo -sf BasicUac.xml

