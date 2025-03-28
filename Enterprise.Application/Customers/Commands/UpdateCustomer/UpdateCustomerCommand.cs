using System;
using System.Threading;
using System.Threading.Tasks;
using Enterprise.Domain.Customers;
using Enterprise.Domain.Customers.ValueObjects;
using MediatR;

namespace Enterprise.Application.Customers.Commands.UpdateCustomer;

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

        var name = request.Name ?? customer.Name;
        var email = request.Email != null ? Email.Create(request.Email) : customer.Email;
        
        customer.Update(name, email);
        await _customerRepository.UpdateAsync(customer, cancellationToken);
    }
} 