namespace FTM.Domain.ServiceBus;

public interface ISubscriberService
{
    public Task Subscribe(string topic, Func<string, Task> callback);
}