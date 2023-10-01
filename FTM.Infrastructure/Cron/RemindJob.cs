using FTM.Domain.Events.Issues;
using FTM.Domain.Models.Base;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.ServiceBus;
using FTM.Domain.Units;
using Microsoft.Extensions.Logging;
using Quartz;

namespace FTM.Infrastructure.Cron;

public class RemindJob : JobBase
{
    private readonly IPublisherService _publisherService;
    private readonly ILogger<RemindJob> _logger;
    private readonly IRepository<Issue> _issueRepository;

    public RemindJob(IRepository<Issue> issueRepository, ILogger<RemindJob> logger, IPublisherService publisherService)
    {
        _issueRepository = issueRepository;
        _logger = logger;
        _publisherService = publisherService;
    }
    
    
    public override async Task Execute(IJobExecutionContext context)
    {
        try
        {
            await Process();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Remind Job Error");
        }
    }

    private async Task Process()
    {
        _logger.LogInformation("Start Reminder Job");

        var now = DateTime.UtcNow;
        
        var filter = new IssueFilter
        {
            Status = IssueStatus.Started,
            FromRemindTime = DateTime.UtcNow.Date.Add(new TimeSpan(0, now.Hour, now.Minute, 0)),
            ToRemindTime = DateTime.UtcNow
        };

        var cursor = _issueRepository.Query(filter.GetExpressions());
        
        foreach (var issue in cursor)
        {
            await _publisherService.Publish(new RemindIssueEvent
            {
                IssueToRemind = (IssueData)issue.ToData() 
            });
        }
        
        _logger.LogInformation("End Reminder Job");
    }
}