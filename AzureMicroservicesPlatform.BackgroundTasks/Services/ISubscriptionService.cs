using System.Threading.Tasks;
using AzureMicroservicesPlatform.BackgroundTasks.Events;

namespace AzureMicroservicesPlatform.BackgroundTasks.Services
{
    public interface ISubscriptionService
    {
        Task ProcessSubscriptionUpgradeAsync(SubscriptionUpgradedEvent eventData);
    }
} 