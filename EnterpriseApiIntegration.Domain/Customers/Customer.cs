using System;
using EnterpriseApiIntegration.Domain.Common;
using EnterpriseApiIntegration.Domain.Customers.Events;
using EnterpriseApiIntegration.Domain.Customers.ValueObjects;

namespace EnterpriseApiIntegration.Domain.Customers;

public class Customer : AggregateRoot
{
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public CustomerRole Role { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastModifiedAt { get; private set; }

    private Customer() { } // For EF Core

    private Customer(string name, Email email, CustomerRole role)
    {
        Name = name;
        Email = email;
        Role = role;
        CreatedAt = DateTime.UtcNow;
        
        AddDomainEvent(new CustomerCreatedEvent(Id, Name, Email, Role));
    }

    public static Customer Create(string name, string email, CustomerRole role)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        var emailVO = Email.Create(email);
        return new Customer(name, emailVO, role);
    }

    public void UpdateContactInfo(string name, string email)
    {
        if (!string.IsNullOrWhiteSpace(name) && name != Name)
        {
            Name = name;
        }

        if (!string.IsNullOrWhiteSpace(email) && email != Email.Value)
        {
            Email = Email.Create(email);
        }

        LastModifiedAt = DateTime.UtcNow;
        AddDomainEvent(new CustomerUpdatedEvent(Id, Name, Email));
    }

    public void ChangeRole(CustomerRole newRole)
    {
        if (Role == newRole)
            return;

        Role = newRole;
        LastModifiedAt = DateTime.UtcNow;
        AddDomainEvent(new CustomerRoleChangedEvent(Id, newRole));
    }
} 