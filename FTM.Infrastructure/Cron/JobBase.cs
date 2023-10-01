using Quartz;

namespace FTM.Infrastructure.Cron;

public abstract class JobBase : IJob
{
    public abstract Task Execute(IJobExecutionContext context);
}