using System;
using EnterpriseApiIntegration.Domain.Customers.ValueObjects;
using MediatR;

namespace EnterpriseApiIntegration.Domain.Customers.Events;

public record CustomerUpdatedEvent(
    Guid CustomerId,
    string Name,
    Email Email) : INotification; 