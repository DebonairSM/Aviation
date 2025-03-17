using EnterpriseApiIntegration.Application.Features.Customers;

namespace EnterpriseApiIntegration.Application.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
    Task<CustomerDto?> GetCustomerByIdAsync(string id);
    Task<CustomerDto> CreateCustomerAsync(CustomerDto customer);
    Task<CustomerDto> UpdateCustomerAsync(string id, CustomerDto customer);
    Task<bool> DeleteCustomerAsync(string id);
    Task<bool> ChangeCustomerRoleAsync(string id, string newRole);
} 