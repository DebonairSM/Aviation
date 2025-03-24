namespace EnterpriseApiIntegration.Application.Features.Subscriptions;

public class SubscriptionDto
{
    public required string Id { get; set; }
    public required string CustomerId { get; set; }
    public required string PlanType { get; set; }
    public required decimal Price { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required string Status { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
} 