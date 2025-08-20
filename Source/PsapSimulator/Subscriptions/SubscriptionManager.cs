/////////////////////////////////////////////////////////////////////////////////////
//  File:   SubscriptionManager.cs                                  15 Jul 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Concurrent;

namespace SipLib.Subscriptions;

/// <summary>
/// This is the base class for classes such as ElementStateSubscriptionManager, ServiceStateSubscriptionManager and
/// QueueStateSubscriptionManager.
/// </summary>
public class SubscriptionManager
{
    /// <summary>
    /// The key is the CallID from a SUBSCRIBE request that uniquely identifies the subscription.
    /// </summary>
    protected ConcurrentDictionary<string, Subscription> Subscriptions = new ConcurrentDictionary<string, Subscription>();

    /// <summary>
    /// Constructor
    /// </summary>
    public SubscriptionManager()
    {
    }

    /// <summary>
    /// Adds a Subscription to the dictionary of subscriptions.
    /// </summary>
    /// <param name="subscriptionID">ID (Call-ID) of the subscription.</param>
    /// <param name="subscription">New Subscription derived object containing information about the subscription.</param>
    protected void AddSubscription(string subscriptionID, Subscription subscription)
    {
        subscription.NotifyFailed += OnNotifyFailed;
        bool Success = Subscriptions.TryAdd(subscriptionID, subscription);
    }

    /// <summary>
    /// Removes a Subscription from the dictionary of subscriptions.
    /// </summary>
    /// <param name="subscriptionID">ID (Call-ID) of the subscription.</param>
    protected void RemoveSubscription(string subscriptionID)
    {
        Subscription? sub;
        Subscriptions.Remove(subscriptionID, out sub);
        if (sub != null)
            sub.NotifyFailed -= OnNotifyFailed;
    }

    /// <summary>
    /// Event handler for the NotifyFailed event of a subscription. This event is fired by a Subscription when
    /// a NOTIFY transaction fails.
    /// </summary>
    /// <param name="subscriptionID">ID (Call-ID) of the subscription.</param>
    private void OnNotifyFailed(string subscriptionID)
    {
        RemoveSubscription(subscriptionID);
    }

    /// <summary>
    /// Checks each subscription to see if it has timed out. If it has, then the subscription is terminated
    /// and removed. This method must be called periodically.
    /// </summary>
    public void DoTimedEvents()
    {
        List<string> ExpiredSubscriptions = new List<string>();
        DateTime Now = DateTime.Now;

        foreach (Subscription subscription in Subscriptions.Values)
        {
            if ((Now - subscription.LastSubscriptionTime).TotalSeconds > subscription.ExpiresSeconds)
            {   // The subscription has expired
                ExpiredSubscriptions.Add(subscription.SubscriptionID); ;
            }
        }

        foreach (string subscriptionID in ExpiredSubscriptions)
        {
            RemoveSubscription(subscriptionID);
            Subscription subscription = Subscriptions[subscriptionID];
            subscription.SendNotifyRequest("terminated", null, null);
        }
    }
}
