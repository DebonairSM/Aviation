using MediatR;

namespace EnterpriseApiIntegration.Application.Features.Customers.Queries;

public class GetCustomerByIdQuery : IRequest<CustomerDto?>
{
    public string Id { get; set; }

    public class Handler : IRequestHandler<GetCustomerByIdQuery, CustomerDto?>
    {
        public async Task<CustomerDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            // TODO: Replace with actual data access
            if (request.Id == "1")
            {
                return new CustomerDto
                {
                    Id = request.Id,
                    Name = "John Doe",
                    Email = "john@example.com",
                    PhoneNumber = "+1234567890",
                    CompanyName = "Aviation Corp",
                    Role = "Customer",
                    CreatedAt = DateTime.UtcNow
                };
            }

            return null;
        }
    }
} 