using Enterprise.Domain.Common;

namespace Enterprise.Domain.Aircraft.Events;

public class AircraftCreatedEvent : DomainEvent
{
    public Guid AircraftId { get; }
    public string Registration { get; }
    public string Type { get; }
    public string Manufacturer { get; }
    public string Model { get; }
    public string SerialNumber { get; }

    public AircraftCreatedEvent(
        Guid aircraftId,
        string registration,
        string type,
        string manufacturer,
        string model,
        string serialNumber)
    {
        AircraftId = aircraftId;
        Registration = registration;
        Type = type;
        Manufacturer = manufacturer;
        Model = model;
        SerialNumber = serialNumber;
    }
} 