/////////////////////////////////////////////////////////////////////////////////////
//  File:   ServiceStateSubscriptionManager.cs                      31 Jul 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

using I3SubNot;
using I3V3.LogEvents;
using I3V3.LoggingHelpers;
using SipLib.Core;
using SipLib.Transactions;

namespace SipLib.Subscriptions;

/// <summary>
/// Class that manages multiple subscribers to the Service State subscribe/notify event package.
/// </summary>
public class ServiceStateSubscriptionManager : SubscriptionManager
{
    private string m_ServiceName;
    private string m_ServiceId;

    private ServiceState m_CurrentServiceState;
    private I3LogEventClientMgr m_I3LogEventClientMgr;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="serviceName">Identifies the type of service that the Service State event package applies to.
    /// Must be one of the constant values defined in the ServiceType class. For example: ServiceType.PSAP.</param>
    /// <param name="serviceId">Identifies the domain of the service that the service state applies to. This
    /// should be the Service Identifier of the service. See Section 2.1.5 of NENA-STA-010.3f.</param>
    public ServiceStateSubscriptionManager(string serviceName, string serviceId, I3LogEventClientMgr i3LogEventClientMgr) : base()
    {
        m_ServiceName = serviceName;
        m_ServiceId = serviceId;
        m_I3LogEventClientMgr = i3LogEventClientMgr;

        m_CurrentServiceState = new ServiceState();
        m_CurrentServiceState.service.name = m_ServiceName;
        m_CurrentServiceState.service.domain = m_ServiceId;
        m_CurrentServiceState.service.serviceId = m_ServiceId;
        m_CurrentServiceState.serviceState.state = ServiceStateType.Normal;
        m_CurrentServiceState.securityPosture.posture = SecurityPostureType.Green;
    }

    /// <summary>
    /// Processes a SUBSCRIBE request for the Service State subscribe/notify event package.
    /// </summary>
    /// <param name="sipRequest">SIP SUBSCRIBE request for the Service State subscribe/notify event package.</param>
    /// <param name="remoteEndPoint">SIPEndPoint that sent the SUBSCRIBE request.</param>
    /// <param name="sipTransport">SipTransport that the SUBSCRIBE request was sent on.</param>
    public void ProcessSubscribeRequest(SIPRequest sipRequest, SIPEndPoint remoteEndPoint, SipTransport sipTransport)
    {
        string subscriptionID = sipRequest.Header.CallId;
        if (Subscriptions.ContainsKey(subscriptionID) == true)
        {   // Its an existing subscription
            SIPResponse OkResponse = SipUtils.BuildResponse(sipRequest, SIPResponseStatusCodesEnum.Ok, "OK", sipTransport.SipChannel, null);
            sipTransport.StartServerNonInviteTransaction(sipRequest, remoteEndPoint.GetIPEndPoint(), null, OkResponse);

            if (sipRequest.Header.Expires == 0)
            {   // The subscriber is requesting to terminate the subscription
                RemoveSubscription(subscriptionID);
            }
            else
            {   // Send a NOTIFY to tell the subscriber the current Service State
                ServiceStateSubscription serviceStateSubscription = (ServiceStateSubscription)Subscriptions[subscriptionID];
                serviceStateSubscription.LastSubscriptionTime = DateTime.Now;
                serviceStateSubscription.CurrentServiceState = m_CurrentServiceState;
                serviceStateSubscription.NotifySubscriber();
            }
        }
        else
        {   // Its a SUBSCRIBE request for a new subscription
            ServiceStateSubscription serviceStateSubscription = new ServiceStateSubscription(sipRequest, sipTransport,
                remoteEndPoint.GetIPEndPoint());
            AddSubscription(subscriptionID, serviceStateSubscription);

            SIPResponse OkResponse = serviceStateSubscription.BuildOkToSubscribe(sipRequest, sipTransport.SipChannel);
            sipTransport.StartServerNonInviteTransaction(sipRequest, remoteEndPoint.GetIPEndPoint(), null, OkResponse);
            serviceStateSubscription.CurrentServiceState = m_CurrentServiceState;
            serviceStateSubscription.NotifySubscriber();
        }
    }

    /// <summary>
    /// Sends a NOTIFY request to each subscriber of the Service State event package. This method must be called
    /// when the Service State or the Security Posture changes.
    /// </summary>
    /// <param name="serviceState">Specifies the new service state. Must be one of the constant values defined
    /// in the ServiceStateType class. For example: ServiceStateType.Normal</param>
    /// <param name="securityPosture">Specifies the new security posture. Must be one of the constant values defined
    /// in the SecurityPostureType class. For example: SecurityPostureType.Green.</param>
    /// <param name="changeReason">Specifies the reason for the change in service state. Optional.</param>
    public void NotifyServiceStateChange(string serviceState, string securityPosture, string? changeReason)
    {
        bool Changed = false;
        if (m_CurrentServiceState.serviceState.state != serviceState || m_CurrentServiceState.securityPosture.posture != securityPosture)
            Changed = true;

        m_CurrentServiceState.serviceState.state = serviceState;
        m_CurrentServiceState.serviceState.reason = changeReason;
        m_CurrentServiceState.securityPosture.posture = securityPosture;
        m_CurrentServiceState.securityPosture.reason = changeReason;

        if (Changed == true)
        {
            ServiceStateChangeLogEvent Le = new ServiceStateChangeLogEvent();
            Le.newState = serviceState;
            Le.newSecurityPosture = securityPosture;
            Le.affectedServiceIdentifier = m_ServiceId;
            Le.direction = "outgoing";
            m_I3LogEventClientMgr.SendLogEvent(Le);
        }

        IEnumerable<Subscription> subs = Subscriptions.Values.ToArray<Subscription>();

        foreach (Subscription subscription in subs)
        {
            ServiceStateSubscription serviceStateSubscription = (ServiceStateSubscription) subscription;
            serviceStateSubscription.CurrentServiceState = m_CurrentServiceState;
            serviceStateSubscription.NotifySubscriber();
        }
    }
}
