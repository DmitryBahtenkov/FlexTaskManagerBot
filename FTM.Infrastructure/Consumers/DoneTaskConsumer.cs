using FTM.Domain.Events.Bot;
using FTM.Domain.Helpers;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.ServiceBus;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Services;
using Microsoft.Extensions.Logging;

namespace FTM.Infrastructure.Consumers;

public class DoneTaskConsumer : IConsumer<DoneTaskEvent>
{
    private readonly IUnitOfWork<Issue> _issueUnit;
    private readonly ILogger<DoneTaskConsumer> _logger;

    public DoneTaskConsumer(IUnitOfWork<Issue> issueUnit, ILogger<DoneTaskConsumer> logger)
    {
        _issueUnit = issueUnit;
        _logger = logger;
    }

    public override async Task Consume(DoneTaskEvent data)
    {
        var repository = _issueUnit.GetRepository();
        var issue = await repository.ByIdAsync(data.IssueId);
        if (issue is null)
        {
            _logger.LogWarning("Issue with id {id} not found", data.IssueId);
            return;
        }

        if (issue.RetrySettings is null)
        {
            return;
        }

        var retrySettings = issue.RetrySettings;

        if (retrySettings.RetryWeekDays is not null)
        {
            var retry = RetryService.GetRetryPeriods(retrySettings.RetryWeekDays, DateTime.Now.DayOfWeek.ToRuWeekday());
            retrySettings.CurrentRetryPeriod = retry.nextRetryInDays;
            retrySettings.CurrentRetryWeekday = retry.retryWeekday;
            retrySettings.RetryWeekDays = retry.orderedSelection;
        }

        var newIssue = new Issue
        {
            Text = issue.Text,
            Folder = issue.Folder,
            Note = issue.Note,
            RetrySettings = retrySettings,
            Status = IssueStatus.Started,
            UserId = issue.UserId
        };

        if (issue.RemindTime.HasValue)
        {
            newIssue.RemindTime = issue.RemindTime.Value.AddDays(retrySettings.CurrentRetryPeriod);
        }

        await repository.AddAsync(newIssue);
        await _issueUnit.SaveChangesAsync();
    }
}