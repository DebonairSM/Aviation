using EnterpriseApiIntegration.Application.Features.Subscriptions;

namespace EnterpriseApiIntegration.Application.Interfaces;

public interface ISubscriptionService
{
    Task<IEnumerable<SubscriptionDto>> GetAllSubscriptionsAsync();
    Task<SubscriptionDto?> GetSubscriptionByIdAsync(string id);
    Task<IEnumerable<SubscriptionDto>> GetSubscriptionsByCustomerIdAsync(string customerId);
    Task<SubscriptionDto> CreateSubscriptionAsync(SubscriptionDto subscription);
    Task<SubscriptionDto> UpdateSubscriptionAsync(string id, SubscriptionDto subscription);
    Task<bool> DeleteSubscriptionAsync(string id);
    Task<bool> ActivateSubscriptionAsync(string id);
    Task<bool> DeactivateSubscriptionAsync(string id);
} 