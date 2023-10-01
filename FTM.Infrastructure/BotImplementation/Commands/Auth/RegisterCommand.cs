using FTM.Domain.Helpers;
using FTM.Domain.Models.BotStatusModel;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Commands.Auth;

[Anon]
public class RegisterCommand : TelegramCommandBase
{
    private readonly MessengerService _messengerService;

    public RegisterCommand(MessengerService messengerService)
    {
        _messengerService = messengerService;
    }

    public override string Command => "register";
    
    public override async Task Handle(Update update, BotStatus botStatus)
    {
        botStatus.State = BotState.Register;
        await _messengerService.SendMessage(GetChatId(update), "Пожалуйста, введите адрес электронной почты");
    }

    public override string? Description => "Зарегистрироваться";
}