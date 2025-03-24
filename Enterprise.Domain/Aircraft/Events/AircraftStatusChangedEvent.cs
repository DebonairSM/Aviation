using Enterprise.Domain.Common;
using Enterprise.Domain.Aircraft.ValueObjects;

namespace Enterprise.Domain.Aircraft.Events;

public class AircraftStatusChangedEvent : DomainEvent
{
    public Guid AircraftId { get; }
    public AircraftStatus NewStatus { get; }

    public AircraftStatusChangedEvent(Guid aircraftId, AircraftStatus newStatus)
    {
        AircraftId = aircraftId;
        NewStatus = newStatus;
    }
} 