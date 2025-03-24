using System;
using Enterprise.Domain.Common;
using Enterprise.Domain.Customers.ValueObjects;
using MediatR;

namespace Enterprise.Domain.Customers.Events;

public class CustomerRoleChangedEvent : DomainEvent
{
    public Guid CustomerId { get; }
    public CustomerRole NewRole { get; }

    public CustomerRoleChangedEvent(Guid customerId, CustomerRole newRole)
    {
        CustomerId = customerId;
        NewRole = newRole;
    }
} 