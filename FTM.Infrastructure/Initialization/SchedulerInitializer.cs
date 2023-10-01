using FTM.Infrastructure.Cron;
using Microsoft.Extensions.DependencyInjection;

namespace FTM.Infrastructure.Initialization;

public class SchedulerInitializer : IAsyncInitializer
{
    private readonly IServiceProvider _serviceProvider;

    public SchedulerInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Initialize()
    {
        var scope = _serviceProvider.CreateScope();
        await Scheduler.Start(scope.ServiceProvider);
    }
}