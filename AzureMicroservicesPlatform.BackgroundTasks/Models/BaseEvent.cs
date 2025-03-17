using System;

namespace AzureMicroservicesPlatform.BackgroundTasks.Models
{
    public abstract class BaseEvent
    {
        public string EventId { get; set; } = Guid.NewGuid().ToString();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public required string EventType { get; set; }
        public required string CorrelationId { get; set; }
        public required string UserId { get; set; }
    }
} 