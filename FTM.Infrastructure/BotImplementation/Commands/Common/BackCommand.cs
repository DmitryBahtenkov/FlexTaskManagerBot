using FTM.Domain.Models.BotStatusModel;
using FTM.Infrastructure.BotImplementation.Base;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Commands.Common;

public class BackCommand : TelegramCommandBase
{
    public override string Command => "backtomain";
    
    public override Task Handle(Update update, BotStatus botStatus)
    {
        throw new NotImplementedException();
    }

    public override string? Description { get; }
}