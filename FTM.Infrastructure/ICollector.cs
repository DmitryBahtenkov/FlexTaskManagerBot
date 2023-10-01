using Microsoft.Extensions.DependencyInjection;

namespace FTM.Infrastructure;

public interface ICollector
{
    public void Collect(IServiceCollection serviceCollection, Type[] types);
}

public interface IServiceCollector
{
    Predicate<Type> NeedToCollect { get; }
    public void Collect(IServiceCollection serviceCollection, Type type);
}

[AttributeUsage(AttributeTargets.Class)]
public class CollectExcludeAttribute : Attribute {}