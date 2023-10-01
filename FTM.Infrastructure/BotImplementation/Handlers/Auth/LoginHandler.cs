using FTM.Domain.Helpers;
using FTM.Domain.Models.BotStatusModel;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FTM.Infrastructure.BotImplementation.Handlers.Auth;

[Anon]
public class LoginHandler : TelegramHandlerBase
{
    private readonly MessengerService _messengerService;

    public LoginHandler(MessengerService messengerService)
    {
        _messengerService = messengerService;
    }

    public override string Command => "login";
    
    public override async Task Handle(Update update, BotStatus botStatus)
    {
        botStatus.State = BotState.Auth;
        await _messengerService.EditMessage(
            botStatus.ChatId,
            update.GetMessageId(), 
            "Пожалуйста, введите свой уникальный токен",
            new InlineKeyboardMarkup(new []{InlineKeyboardButton.WithCallbackData("Назад", "backtologin#")}));
    }
}