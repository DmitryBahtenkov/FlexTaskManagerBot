using FTM.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace FTM.Infrastructure.Initialization;

public class InitializerCollector : ICollector, IServiceCollector
{
    public void Collect(IServiceCollection serviceCollection, Type[] types)
    {

    }

    public Predicate<Type> NeedToCollect => 
        x => typeof(IAsyncInitializer).IsAssignableFrom(x) && x != typeof(IAsyncInitializer);
    public void Collect(IServiceCollection serviceCollection, Type type)
    {
        serviceCollection.AddScoped(typeof(IAsyncInitializer), type);
    }
}