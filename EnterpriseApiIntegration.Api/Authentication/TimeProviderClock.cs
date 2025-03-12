using Microsoft.AspNetCore.Authentication;

namespace EnterpriseApiIntegration.Api.Authentication;

public class TimeProviderClock : ISystemClock
{
    private readonly TimeProvider _timeProvider;

    public TimeProviderClock(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public DateTimeOffset UtcNow => _timeProvider.GetUtcNow();
} 