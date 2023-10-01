using FTM.Domain.Helpers;
using FTM.Domain.Models.BotStatusModel;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Commands.Auth;

[Anon]
public class LoginCommand : TelegramCommandBase
{
    private readonly MessengerService _messengerService;

    public LoginCommand(MessengerService messengerService)
    {
        _messengerService = messengerService;
    }

    public override string Command => "login";
    
    public override async Task Handle(Update update, BotStatus botStatus)
    {
        botStatus.State = BotState.Auth;
        await _messengerService.SendMessage(botStatus.ChatId, "Пожалуйста, введите уникальный токен");
    }

    public override string? Description => "Войти";
}