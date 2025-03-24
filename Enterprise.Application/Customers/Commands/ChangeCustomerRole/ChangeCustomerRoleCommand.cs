using System;
using System.Threading;
using System.Threading.Tasks;
using Enterprise.Domain.Customers;
using Enterprise.Domain.Customers.ValueObjects;
using MediatR;

namespace Enterprise.Application.Customers.Commands.ChangeCustomerRole;

public record ChangeCustomerRoleCommand(
    Guid CustomerId,
    CustomerRole NewRole) : IRequest;

public class ChangeCustomerRoleCommandHandler : IRequestHandler<ChangeCustomerRoleCommand>
{
    private readonly ICustomerRepository _customerRepository;

    public ChangeCustomerRoleCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task Handle(ChangeCustomerRoleCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken)
            ?? throw new InvalidOperationException($"Customer with ID {request.CustomerId} not found");

        customer.ChangeRole(request.NewRole);
        await _customerRepository.UpdateAsync(customer, cancellationToken);
    }
} 