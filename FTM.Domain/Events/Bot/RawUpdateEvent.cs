using FTM.Domain.ServiceBus;

namespace FTM.Domain.Events.Bot;

public class RawUpdateEvent : BaseEvent
{
    public string UpdateRaw { get; set; }
    public Source Source { get; set; }
}

public enum Source
{
    Telegram = 0 
}