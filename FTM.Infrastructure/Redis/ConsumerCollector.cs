using FTM.Domain.ServiceBus;
using Microsoft.Extensions.DependencyInjection;

namespace FTM.Infrastructure.Redis;

public class ConsumerCollector : ICollector, IServiceCollector
{
    public void Collect(IServiceCollection serviceCollection, Type[] types)
    {
        var consumers = types.Where(x => typeof(IConsumer).IsAssignableFrom(x) && !x.IsAbstract);

        foreach (var consumer in consumers)
        {
            serviceCollection.AddScoped(typeof(IConsumer), consumer);
            serviceCollection.AddScoped(consumer);
        }
    }

    public Predicate<Type> NeedToCollect => x => typeof(IConsumer).IsAssignableFrom(x) && !x.IsAbstract;
    public void Collect(IServiceCollection serviceCollection, Type type)
    {
        serviceCollection.AddScoped(typeof(IConsumer), type);
        serviceCollection.AddScoped(type);
    }
}