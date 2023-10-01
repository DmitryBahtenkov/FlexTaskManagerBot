using System.Text.Json;
using FTM.Domain.ServiceBus;

namespace FTM.Infrastructure.Redis;

public class RedisPublisher : IPublisherService
{
    public async Task Publish<TEvent>(TEvent data) where TEvent : BaseEvent
    {
        await RedisConnection.RedisCache.PublishAsync(typeof(TEvent).Name, JsonSerializer.Serialize(data));
    }
}