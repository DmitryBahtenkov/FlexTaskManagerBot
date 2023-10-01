using FTM.Domain.Units;
using Microsoft.Extensions.DependencyInjection;

namespace FTM.Infrastructure.DataAccess;

public class RepositoryCollector : ICollector, IServiceCollector
{
    public void Collect(IServiceCollection serviceCollection, Type[] types)
    {
        var repositories = types.Where(x => typeof(IRepository).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);

        foreach (var repository in repositories)
        {
            foreach (var @interface in repository.GetInterfaces().Where(x => x.GenericTypeArguments.Any()))
            {
                serviceCollection.AddScoped(@interface, repository);
            }
        }
    }

    public Predicate<Type> NeedToCollect =>
        x => typeof(IRepository).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false };
    public void Collect(IServiceCollection serviceCollection, Type type)
    {
        foreach (var @interface in type.GetInterfaces().Where(x => x.GenericTypeArguments.Any()))
        {
            serviceCollection.AddScoped(@interface, type);
        }
    }
}