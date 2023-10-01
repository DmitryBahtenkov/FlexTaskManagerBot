namespace FTM.Domain.ServiceBus;

public interface IPublisherService
{
    public Task Publish<TEvent>(TEvent data) where TEvent : BaseEvent;
}