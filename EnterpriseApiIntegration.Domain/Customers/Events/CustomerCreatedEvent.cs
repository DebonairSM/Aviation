using System;
using EnterpriseApiIntegration.Domain.Customers.ValueObjects;
using MediatR;

namespace EnterpriseApiIntegration.Domain.Customers.Events;

public record CustomerCreatedEvent(
    Guid CustomerId,
    string Name,
    Email Email,
    CustomerRole Role) : INotification; 