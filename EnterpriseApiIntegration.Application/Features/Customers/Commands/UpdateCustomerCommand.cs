using MediatR;

namespace EnterpriseApiIntegration.Application.Features.Customers.Commands;

public record UpdateCustomerCommand : IRequest<CustomerDto>
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
}

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, CustomerDto>
{
    public async Task<CustomerDto> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        // Demo implementation
        return new CustomerDto
        {
            Id = request.Id,
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone
        };
    }
} 