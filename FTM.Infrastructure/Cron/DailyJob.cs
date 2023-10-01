using FTM.Domain.Events.Issues;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.Models.SettingsModel;
using FTM.Domain.ServiceBus;
using FTM.Domain.Services;
using FTM.Domain.Units;
using FTM.Infrastructure.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;

namespace FTM.Infrastructure.Cron;

public class DailyJob : JobBase
{
    private readonly IRepository<Settings> _settingsRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPublisherService _publisherService;
    private readonly ILogger<DailyJob> _logger;
    private readonly FtmDbContext _dbContext;



    public DailyJob(
        IRepository<Settings> settingsRepository,
        ICurrentUserService currentUserService, IPublisherService publisherService, ILogger<DailyJob> logger, FtmDbContext dbContext)
    {
        _settingsRepository = settingsRepository;
        _currentUserService = currentUserService;
        _publisherService = publisherService;
        _logger = logger;
        _dbContext = dbContext;
    }

    public override async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Daily job started");

        try
        {
            await Process();
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Daily job error");
        }
        
        _logger.LogInformation("Daily job started");
    }

    private async Task Process()
    {
        var data = _dbContext.SpIssuesForDailies.AsEnumerable();

        foreach (var d in data.GroupBy(x => x.UserId))
        {
            if (d?.Any() is true)
            {
                await _publisherService.Publish(new DailyIssuesEvent
                {
                    UserId = d.Key,
                    IssueIds = d.Select(x => x.IssueId)
                });
            }
        }
    }
}