using MediatR;
using EnterpriseApiIntegration.Application.Features.Customers;

namespace EnterpriseApiIntegration.Application.Features.Customers.Queries;

public class GetCustomersQuery : IRequest<IEnumerable<CustomerDto>>
{
    public class Handler : IRequestHandler<GetCustomersQuery, IEnumerable<CustomerDto>>
    {
        public async Task<IEnumerable<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            // TODO: Replace with actual data access
            return new List<CustomerDto>
            {
                new CustomerDto 
                { 
                    Id = "1", 
                    Name = "John Doe", 
                    Email = "john@example.com",
                    PhoneNumber = "+1234567890",
                    CompanyName = "Aviation Corp",
                    Role = "Customer",
                    CreatedAt = DateTime.UtcNow
                },
                new CustomerDto 
                { 
                    Id = "2", 
                    Name = "Jane Smith", 
                    Email = "jane@example.com",
                    PhoneNumber = "+0987654321",
                    CompanyName = "Sky Ltd",
                    Role = "VIP",
                    CreatedAt = DateTime.UtcNow
                }
            };
        }
    }
} 