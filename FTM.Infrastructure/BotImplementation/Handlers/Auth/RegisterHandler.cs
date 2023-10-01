using FTM.Domain.Helpers;
using FTM.Domain.Models.BotStatusModel;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FTM.Infrastructure.BotImplementation.Handlers.Auth;

[Anon]
public class RegisterHandler : TelegramHandlerBase
{
    private readonly MessengerService _messengerService;

    public RegisterHandler(MessengerService messengerService)
    {
        _messengerService = messengerService;
    }

    public override string Command => "register";
    
    public override async Task Handle(Update update, BotStatus botStatus)
    {
        botStatus.State = BotState.Register;
        await _messengerService.EditMessage(botStatus.ChatId, 
            update.GetMessageId(),
            "Пожалуйста, введите адрес электронной почты",
            new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("Назад","backtologin#")));
    }
}