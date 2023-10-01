using FTM.Domain.Factories;
using FTM.Domain.Helpers;
using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.StatisticModel;
using FTM.Infrastructure.BotImplementation.Base;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Commands;

[Anon]
[WithoutStatistic]
public class EmptyCommand : TelegramCommandBase
{
    private readonly BotProcessorFactory<Update> _botProcessorFactory;

    public EmptyCommand(BotProcessorFactory<Update> botProcessorFactory)
    {
        _botProcessorFactory = botProcessorFactory;
    }

    public override string Command => string.Empty;
    
    public override async Task Handle(Update update, BotStatus botStatus)
    {
        var processor = _botProcessorFactory.Get(botStatus.State);

        await processor.Process(update, botStatus);
    }

    public override string? Description { get; }
}