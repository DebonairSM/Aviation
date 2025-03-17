namespace EnterpriseApiIntegration.Application.Features.Customers;

public class CustomerDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string CompanyName { get; set; }
    public required string Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
} 