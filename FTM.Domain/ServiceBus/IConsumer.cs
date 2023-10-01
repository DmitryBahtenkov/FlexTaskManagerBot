namespace FTM.Domain.ServiceBus;

public abstract class IConsumer<TEvent> : IConsumer where TEvent : BaseEvent
{
    public abstract Task Consume(TEvent data);

    public Task Consume(object data)
    {
        return Consume((TEvent)data);
    }
}

public interface IConsumer
{
    public Task Consume(object data);
}