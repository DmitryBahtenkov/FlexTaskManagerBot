using FTM.Domain.Models.BotStatusModel;

namespace FTM.Domain.Services;

public interface IBotHandler<in TUpdate> : IHandler<TUpdate, BotStatus>
{
}