using FTM.Domain.ServiceBus;

namespace FTM.Infrastructure.Initialization;

public class RedisSubscribeInitializer : IAsyncInitializer
{
    private readonly AutoSubscriber _autoSubscriber;

    public RedisSubscribeInitializer(AutoSubscriber autoSubscriber)
    {
        _autoSubscriber = autoSubscriber;
    }

    public Task Initialize()
    {
        return _autoSubscriber.ExecuteSubscribe();
    }
}