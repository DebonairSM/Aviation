using System;
using System.Threading;
using System.Threading.Tasks;
using EnterpriseApiIntegration.Domain.Customers;
using EnterpriseApiIntegration.Domain.Customers.ValueObjects;
using MediatR;

namespace EnterpriseApiIntegration.Application.Customers.Commands.CreateCustomer;

public record CreateCustomerCommand(
    string Name,
    string Email,
    CustomerRole Role) : IRequest<Guid>;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
{
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        // Check if customer with email already exists
        if (await _customerRepository.ExistsByEmailAsync(request.Email, cancellationToken))
        {
            throw new InvalidOperationException($"Customer with email {request.Email} already exists");
        }

        // Create customer aggregate
        var customer = Customer.Create(request.Name, request.Email, request.Role);

        // Persist to database
        await _customerRepository.AddAsync(customer, cancellationToken);

        return customer.Id;
    }
} 