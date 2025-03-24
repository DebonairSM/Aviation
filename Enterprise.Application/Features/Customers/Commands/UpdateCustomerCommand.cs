using MediatR;

namespace EnterpriseApiIntegration.Application.Features.Customers.Commands;

public class UpdateCustomerCommand : IRequest<CustomerDto>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string CompanyName { get; set; }
    public string Role { get; set; }

    public class Handler : IRequestHandler<UpdateCustomerCommand, CustomerDto>
    {
        public async Task<CustomerDto> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            // TODO: Replace with actual data access
            return new CustomerDto
            {
                Id = request.Id,
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                CompanyName = request.CompanyName,
                Role = request.Role,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
    }
} 