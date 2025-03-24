using System;
using Enterprise.Domain.Common;
using Enterprise.Domain.Customers.ValueObjects;
using MediatR;

namespace Enterprise.Domain.Customers.Events;

public class CustomerUpdatedEvent : DomainEvent
{
    public Guid CustomerId { get; }
    public string Name { get; }
    public Email Email { get; }

    public CustomerUpdatedEvent(Guid customerId, string name, Email email)
    {
        CustomerId = customerId;
        Name = name;
        Email = email;
    }
} 