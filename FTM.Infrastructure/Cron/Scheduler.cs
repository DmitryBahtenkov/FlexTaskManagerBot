using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;

namespace FTM.Infrastructure.Cron;

public class Scheduler
{
    public static async Task Start(IServiceProvider serviceProvider)
    {
        if (Environment.GetEnvironmentVariable("DISABLE_JOBS") == "1")
        {
            return;
        }
        var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        scheduler.JobFactory = serviceProvider.GetRequiredService<JobFactory>();
        await scheduler.Start();

        var jobDetail = JobBuilder.Create<RemindJob>().Build();
        var trigger = TriggerBuilder.Create()
            .WithIdentity(nameof(RemindJob), "default")
            .WithSimpleSchedule(x => x
                .WithIntervalInMinutes(1)
                .RepeatForever())
            .Build();

        var dailyJobDetail = JobBuilder.Create<DailyJob>().Build();
        var dailyJobTrigger = TriggerBuilder.Create()
            .WithIdentity(nameof(DailyJob), "default")
            .WithSimpleSchedule(x => x.WithIntervalInHours(1).RepeatForever())
            .StartAt(DateTime.UtcNow.Date.AddHours(DateTime.UtcNow.Hour + 1))
            .Build();

        await scheduler.ScheduleJob(jobDetail, trigger);
        await scheduler.ScheduleJob(dailyJobDetail, dailyJobTrigger);
    }
}