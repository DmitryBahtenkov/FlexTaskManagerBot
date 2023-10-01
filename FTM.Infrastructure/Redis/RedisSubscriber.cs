using FTM.Domain.ServiceBus;

namespace FTM.Infrastructure.Redis;

public class RedisSubscriber : ISubscriberService
{
    public async Task Subscribe(string topic, Func<string, Task> callback)
    {
        var sub = RedisConnection.Connection.GetSubscriber();
        await sub.SubscribeAsync(topic, (channel, value) => callback(value));
    }
}