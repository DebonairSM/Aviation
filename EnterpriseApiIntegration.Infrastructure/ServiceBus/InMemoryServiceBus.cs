using System.Collections.Concurrent;

namespace EnterpriseApiIntegration.Infrastructure.ServiceBus;

public interface IInMemoryServiceBus
{
    Task PublishAsync<T>(string topic, T message);
    Task SubscribeAsync<T>(string topic, Func<T, Task> handler);
    Task UnsubscribeAsync<T>(string topic, Func<T, Task> handler);
}

public class InMemoryServiceBus : IInMemoryServiceBus
{
    private readonly ConcurrentDictionary<string, List<Func<object, Task>>> _subscribers = new();

    public async Task PublishAsync<T>(string topic, T message)
    {
        if (!_subscribers.TryGetValue(topic, out var handlers))
            return;

        foreach (var handler in handlers.ToList())
        {
            try
            {
                await handler(message);
            }
            catch (Exception ex)
            {
                // In a real implementation, you'd want to log this and possibly retry
                Console.WriteLine($"Error handling message on topic {topic}: {ex.Message}");
            }
        }
    }

    public Task SubscribeAsync<T>(string topic, Func<T, Task> handler)
    {
        var handlers = _subscribers.GetOrAdd(topic, _ => new List<Func<object, Task>>());
        handlers.Add(message => handler((T)message));
        return Task.CompletedTask;
    }

    public Task UnsubscribeAsync<T>(string topic, Func<T, Task> handler)
    {
        if (_subscribers.TryGetValue(topic, out var handlers))
        {
            handlers.RemoveAll(h => h == (message => handler((T)message)));
        }
        return Task.CompletedTask;
    }
} 