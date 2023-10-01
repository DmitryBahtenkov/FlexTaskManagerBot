using FTM.Domain.Models.BotStatusModel;

namespace FTM.Domain.Services;

public interface IBotProcessor<in TUpdate>
{
    public BotState State { get; }
    public Task Process(TUpdate update, BotStatus botStatus);
}