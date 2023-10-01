using FTM.Domain.Models.IssueModel;
using FTM.Domain.ServiceBus;

namespace FTM.Domain.Events.Issues;

public class RemindIssueEvent : BaseEvent
{
    public IssueData IssueToRemind { get; set; }
}