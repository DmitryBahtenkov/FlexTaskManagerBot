using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace FTM.Infrastructure.Cron;

public class JobFactory : IJobFactory
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public JobFactory(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        return new JobDecorator(bundle.JobDetail.JobType, _serviceScopeFactory);
    }

    public void ReturnJob(IJob job)
    {
        // по кайфу
    }
}