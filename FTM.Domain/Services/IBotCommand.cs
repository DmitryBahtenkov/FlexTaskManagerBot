using FTM.Domain.Models.BotStatusModel;

namespace FTM.Domain.Services;

public interface IBotCommand<in TUpdate> : IHandler<TUpdate, BotStatus>
{
    public string? Description { get; }
}