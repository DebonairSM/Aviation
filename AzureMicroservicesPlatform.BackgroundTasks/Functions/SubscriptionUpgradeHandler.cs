using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureMicroservicesPlatform.BackgroundTasks.Events;
using AzureMicroservicesPlatform.BackgroundTasks.Services;

namespace AzureMicroservicesPlatform.BackgroundTasks.Functions
{
    public class SubscriptionUpgradeHandler
    {
        private readonly ILogger<SubscriptionUpgradeHandler> _logger;
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionUpgradeHandler(
            ILogger<SubscriptionUpgradeHandler> logger,
            ISubscriptionService subscriptionService)
        {
            _logger = logger;
            _subscriptionService = subscriptionService;
        }

        [Function(nameof(ProcessSubscriptionUpgrade))]
        public async Task ProcessSubscriptionUpgrade(
            [ServiceBusTrigger("subscription-upgraded", Connection = "ServiceBusConnection")] string message)
        {
            try
            {
                _logger.LogInformation($"Received subscription upgrade message: {message}");

                var eventData = JsonConvert.DeserializeObject<SubscriptionUpgradedEvent>(message);
                await _subscriptionService.ProcessSubscriptionUpgradeAsync(eventData);

                _logger.LogInformation($"Successfully processed subscription upgrade for SubscriptionId: {eventData.SubscriptionId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing subscription upgrade");
                throw; // Retrying the message by throwing the exception
            }
        }
    }
} 