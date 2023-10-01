using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FTM.Domain.ServiceBus;

public class AutoSubscriber
{
    private readonly AutoSubscriberContext _autoSubscriberContext;

    public AutoSubscriber(AutoSubscriberContext autoSubscriberContext)
    {
        _autoSubscriberContext = autoSubscriberContext;
    }

    public async Task ExecuteSubscribe()
    {
        var globalScope = _autoSubscriberContext.ServiceProvider.CreateScope();
        foreach (var consumer in _autoSubscriberContext.Consumers)
        {
            var generics = consumer.GetType().BaseType?.GetGenericArguments();
            if (generics?.Any() is not true)
            {
                _autoSubscriberContext.Logger.LogWarning("consumer doesnt have a generic parameter");
                continue;
            }

            var wrapper = new ConsumerWrapper(_autoSubscriberContext, consumer.GetType());
            var generic = generics.First();
            await _autoSubscriberContext.Subscriber.Subscribe(generic.Name, async data =>
            {
                var @event = (BaseEvent?)JsonSerializer.Deserialize(data, generic);
                if (@event is null)
                {
                    _autoSubscriberContext.Logger.LogError("event is empty");
                    return;
                }

                await wrapper.Consume(@event, globalScope.ServiceProvider);
            });
        }
    }
}