using System;
using Enterprise.Domain.Common;
using Enterprise.Domain.Aircraft.Events;
using Enterprise.Domain.Aircraft.ValueObjects;

namespace Enterprise.Domain.Aircraft;

public class Aircraft : AggregateRoot
{
    public string Registration { get; private set; }
    public string Type { get; private set; }
    public string Manufacturer { get; private set; }
    public string Model { get; private set; }
    public string SerialNumber { get; private set; }
    public AircraftStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Aircraft() { } // For EF Core

    public Aircraft(string registration, string type, string manufacturer, string model, string serialNumber)
    {
        Registration = registration;
        Type = type;
        Manufacturer = manufacturer;
        Model = model;
        SerialNumber = serialNumber;
        Status = AircraftStatus.Available;
        CreatedAt = DateTime.UtcNow;

        AddDomainEvent(new AircraftCreatedEvent(Id, registration, type, manufacturer, model, serialNumber));
    }

    public void Update(string registration, string type, string manufacturer, string model, string serialNumber)
    {
        Registration = registration;
        Type = type;
        Manufacturer = manufacturer;
        Model = model;
        SerialNumber = serialNumber;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new AircraftUpdatedEvent(Id, registration, type, manufacturer, model, serialNumber));
    }

    public void ChangeStatus(AircraftStatus newStatus)
    {
        if (Status == newStatus)
            return;

        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new AircraftStatusChangedEvent(Id, newStatus));
    }
} 