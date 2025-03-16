using MediatR;

namespace EnterpriseApiIntegration.Application.Features.Customers.Commands;

public record DeleteCustomerCommand : IRequest
{
    public int Id { get; init; }
}

public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
{
    public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        // Demo implementation - would typically delete from database
        await Task.CompletedTask;
    }
} 