using System;
using AzureMicroservicesPlatform.BackgroundTasks.Models;

namespace AzureMicroservicesPlatform.BackgroundTasks.Events
{
    public class SubscriptionUpgradedEvent : BaseEvent
    {
        public SubscriptionUpgradedEvent()
        {
            EventType = "SubscriptionUpgraded";
        }

        public Guid SubscriptionId { get; set; }
        public required string NewPlanId { get; set; }
        public DateTime UpgradeDate { get; set; }
        public decimal NewPrice { get; set; }
        public required string Currency { get; set; }
    }
} 