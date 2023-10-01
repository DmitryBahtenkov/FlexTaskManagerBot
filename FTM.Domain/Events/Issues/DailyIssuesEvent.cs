using FTM.Domain.ServiceBus;

namespace FTM.Domain.Events.Issues;

public class DailyIssuesEvent : BaseEvent
{
    public IEnumerable<int> IssueIds { get; set; } = Enumerable.Empty<int>();

}