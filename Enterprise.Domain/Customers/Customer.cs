using System;
using Enterprise.Domain.Common;
using Enterprise.Domain.Customers.Events;
using Enterprise.Domain.Customers.ValueObjects;

namespace Enterprise.Domain.Customers;

public class Customer : AggregateRoot
{
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public CustomerRole Role { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Customer() { } // For EF Core

    public Customer(string name, Email email, CustomerRole role)
    {
        Name = name;
        Email = email;
        Role = role;
        CreatedAt = DateTime.UtcNow;

        AddDomainEvent(new CustomerCreatedEvent(Id, name, email, role));
    }

    public void Update(string name, Email email)
    {
        Name = name;
        Email = email;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new CustomerUpdatedEvent(Id, name, email));
    }

    public void ChangeRole(CustomerRole newRole)
    {
        if (Role == newRole)
            return;

        Role = newRole;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new CustomerRoleChangedEvent(Id, newRole));
    }
} 