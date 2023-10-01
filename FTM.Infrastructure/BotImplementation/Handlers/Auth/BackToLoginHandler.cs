using FTM.Domain.Helpers;
using FTM.Domain.Models.BotStatusModel;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FTM.Infrastructure.BotImplementation.Handlers.Auth;

[Anon]
public class BackToLoginHandler : TelegramHandlerBase
{
    private readonly MessengerService _messengerService;

    public BackToLoginHandler(MessengerService messengerService)
    {
        _messengerService = messengerService;
    }

    public override string Command => "backtologin";
    public override async Task Handle(Update update, BotStatus botStatus)
    {
        var markup = new InlineKeyboardMarkup(new[]
        {
            InlineKeyboardButton.WithCallbackData("Войти", "login#"), 
            InlineKeyboardButton.WithCallbackData("Я новый пользователь", "register#"), 
        });

        botStatus.State = BotState.Started;
            
        await _messengerService.EditMessage(botStatus.ChatId, update.GetMessageId(),"Добро пожаловать!", markup);
    }
}