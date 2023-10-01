using FTM.Domain.ServiceBus;

namespace FTM.Domain.Events.Bot;

public class DoneTaskEvent : BaseEvent
{
    public int IssueId { get; set; }
}