using System.Text;
using FTM.Domain.Helpers;
using FTM.Domain.Models.IssueModel;

namespace FTM.Infrastructure.Extensions;

public static class IssueStringExtensions
{
    public static string ToBotString(this Issue issue)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Задача: {issue.Text}");
        sb.AppendLine($"Статус: {issue.Status.GetDisplay()}");

        if (!string.IsNullOrEmpty(issue.Folder))
        {
            sb.AppendLine($"Категория: {issue.Folder}");
        }

        if (issue.RemindTime.HasValue)
        {
            var tz = issue.User?.Settings.FirstOrDefault()?.Timezone ?? 3;
            var remindTime = issue.RemindTime.Value.AddHours(tz);
            sb.AppendLine($"Напоминание: {remindTime:dd.MM.yyyy HH:mm}");
        }

        if (issue.RetrySettings is not null)
        {
            string retryLine = string.Empty;
            if (issue.RetrySettings.RetryWeekDays is null)
            {
                var dayCount = issue.RetrySettings.CurrentRetryPeriod;
                if (dayCount == 1)
                {
                    retryLine = $"Повторять ежедневно";
                }
                else
                {
                    retryLine = $"Повторять каждые {dayCount} дней";
                }
            }
            else
            {
                retryLine = $"Повторять по дням: {issue.RetrySettings.RetryWeekDays.ToStringEnum()}";
            }

            sb.AppendLine(retryLine);
        }

        if (!string.IsNullOrEmpty(issue.Note))
        {
            sb.AppendLine(issue.Note);
        }

        return sb.ToString();
    }
}