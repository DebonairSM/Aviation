using Enterprise.Domain.Common;
using Enterprise.Domain.Customers.ValueObjects;

namespace Enterprise.Domain.Customers.Events;

public class CustomerCreatedEvent : DomainEvent
{
    public Guid CustomerId { get; }
    public string Name { get; }
    public Email Email { get; }
    public CustomerRole Role { get; }

    public CustomerCreatedEvent(Guid customerId, string name, Email email, CustomerRole role)
    {
        CustomerId = customerId;
        Name = name;
        Email = email;
        Role = role;
    }
} 