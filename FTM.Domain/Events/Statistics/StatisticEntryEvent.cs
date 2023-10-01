using FTM.Domain.ServiceBus;

namespace FTM.Domain.Events.Statistics;

public class StatisticEntryEvent : BaseEvent
{
    public string Key { get; set; }
    public object? Data { get; set; }
}