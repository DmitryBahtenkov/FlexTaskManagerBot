namespace FTM.Domain.ServiceBus;

public abstract class BaseEvent
{
    public Guid EventId { get; }
    public int? UserId { get; set; }

    public BaseEvent()
    {
        EventId = Guid.NewGuid();
    }
}