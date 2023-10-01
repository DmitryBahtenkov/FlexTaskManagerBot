using FTM.Domain.Models.UserModel;
using FTM.Domain.Services;
using FTM.Domain.Units;
using Microsoft.Extensions.Logging;

namespace FTM.Domain.ServiceBus;

public class AutoSubscriberContext
{
    public AutoSubscriberContext(ISubscriberService subscriber, IEnumerable<IConsumer> consumers, ILogger<AutoSubscriberContext> logger, IServiceProvider serviceProvider)
    {
        Subscriber = subscriber;
        Consumers = consumers;
        Logger = logger;
        ServiceProvider = serviceProvider;
    }

    public ISubscriberService Subscriber { get; }
    public IEnumerable<IConsumer> Consumers { get; }
    public ILogger<AutoSubscriberContext> Logger { get; }
    public IServiceProvider ServiceProvider { get; }
}