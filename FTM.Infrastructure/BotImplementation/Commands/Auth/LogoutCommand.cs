using FTM.Domain.Models.BotStatusModel;
using FTM.Infrastructure.BotImplementation.Base;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Commands.Auth;

public class LogoutCommand : TelegramCommandBase
{
    public override string Command => "logout";
    
    public override Task Handle(Update update, BotStatus botStatus)
    {
        throw new NotImplementedException();
    }

    public override string? Description { get; }
}