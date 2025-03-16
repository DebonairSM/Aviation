using MediatR;

namespace EnterpriseApiIntegration.Application.Features.Customers.Queries;

public record GetCustomerByIdQuery : IRequest<CustomerDto?>
{
    public int Id { get; init; }
}

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto?>
{
    public async Task<CustomerDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        // Demo implementation
        if (request.Id == 1)
        {
            return new CustomerDto
            {
                Id = 1,
                Name = "Demo Customer 1",
                Email = "demo1@example.com",
                Phone = "123-456-7890"
            };
        }

        return null;
    }
} 