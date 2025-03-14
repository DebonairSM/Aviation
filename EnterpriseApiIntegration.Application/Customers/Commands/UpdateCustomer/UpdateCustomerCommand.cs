using System;
using System.Threading;
using System.Threading.Tasks;
using EnterpriseApiIntegration.Domain.Customers;
using MediatR;

namespace EnterpriseApiIntegration.Application.Customers.Commands.UpdateCustomer;

public record UpdateCustomerCommand(
    Guid CustomerId,
    string? Name,
    string? Email) : IRequest;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
{
    private readonly ICustomerRepository _customerRepository;

    public UpdateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken)
            ?? throw new InvalidOperationException($"Customer with ID {request.CustomerId} not found");

        // If new email is provided, check if it's not taken by another customer
        if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != customer.Email.Value)
        {
            if (await _customerRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            {
                throw new InvalidOperationException($"Email {request.Email} is already taken");
            }
        }

        customer.UpdateContactInfo(request.Name ?? customer.Name, request.Email ?? customer.Email);
        await _customerRepository.UpdateAsync(customer, cancellationToken);
    }
} 