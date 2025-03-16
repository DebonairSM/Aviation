using MediatR;

namespace EnterpriseApiIntegration.Application.Features.Customers.Queries;

public record GetCustomersQuery : IRequest<IEnumerable<CustomerDto>>;

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, IEnumerable<CustomerDto>>
{
    public async Task<IEnumerable<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        // Demo implementation
        return new List<CustomerDto>
        {
            new() { Id = 1, Name = "Demo Customer 1", Email = "demo1@example.com", Phone = "123-456-7890" },
            new() { Id = 2, Name = "Demo Customer 2", Email = "demo2@example.com", Phone = "098-765-4321" }
        };
    }
} 