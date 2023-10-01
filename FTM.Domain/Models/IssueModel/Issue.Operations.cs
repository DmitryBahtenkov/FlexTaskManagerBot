using FTM.Domain.Exceptions;
using FTM.Domain.Helpers;
using FTM.Domain.Models.IssueModel.DTO;
using FTM.Domain.Services.TextParser;
using Mapster;

namespace FTM.Domain.Models.IssueModel;

public partial class Issue
{
    public static Issue Create(CreateIssueDto createIssueDto, int userId)
    {
        createIssueDto.ValidateAndThrow();
        var issue = createIssueDto.Adapt<Issue>();
        issue.UserId = userId;

        return issue;
    }

    public void SetRetry(int days)
    {
        RetrySettings = new()
        {
            CurrentRetryPeriod = days
        };
    }

    public void SetRetry(RuDayOfWeek[] weekdays, int currentRetryWeekday, int nextRetryInDay)
    {
        RetrySettings = new()
        {
            RetryWeekDays = weekdays,
            CurrentRetryPeriod = nextRetryInDay,
            CurrentRetryWeekday = currentRetryWeekday
        };
    }

    public void RemoveRetry()
    {
        RetrySettings = null;
    }

    public void ChangeStatus(IssueStatus status)
    {
        Status = status;
    }

    public void RemoveAttachment()
    {
        IssueFile = null;
    }
}