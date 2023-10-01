using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;

namespace FTM.Infrastructure.Cron;

public class JobDecorator : JobBase
{
    private readonly Type _jobType;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public JobDecorator(
        Type jobType,
        IServiceScopeFactory serviceScopeFactory)
    {
        _jobType = jobType;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public override async Task Execute(IJobExecutionContext context)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<JobDecorator>>();
        try
        {
            var job = (JobBase) scope.ServiceProvider.GetRequiredService(_jobType);
            logger.LogInformation("try to execute job {jobName}", _jobType.Name);
            await job.Execute(context);
            logger.LogInformation("job {jobName} executed", _jobType.Name);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Job {jobName} error", _jobType.Name);
        }
    }
}