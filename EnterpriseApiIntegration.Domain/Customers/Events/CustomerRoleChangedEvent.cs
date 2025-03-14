using System;
using EnterpriseApiIntegration.Domain.Customers.ValueObjects;
using MediatR;

namespace EnterpriseApiIntegration.Domain.Customers.Events;

public record CustomerRoleChangedEvent(
    Guid CustomerId,
    CustomerRole NewRole) : INotification; 