using MediatR;

namespace EnterpriseApiIntegration.Application.Features.Customers.Commands;

public class DeleteCustomerCommand : IRequest<bool>
{
    public required string Id { get; set; }

    public class Handler : IRequestHandler<DeleteCustomerCommand, bool>
    {
        public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            // TODO: Replace with actual data access
            return true;
        }
    }
} 