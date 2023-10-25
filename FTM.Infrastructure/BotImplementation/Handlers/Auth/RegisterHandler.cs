using System.Security.Cryptography;
using System.Text;
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
        if (Environment.GetEnvironmentVariable("DISABLE_EMAIL_CONFIRMATION") is "1")
        {
            var sha = SHA512.Create();
            var data = botStatus.ChatId + Guid.NewGuid();
            var hashed = Convert.ToBase64String(sha.ComputeHash(Encoding.Default.GetBytes(data)));
            botStatus.Token = hashed;
            botStatus.State = BotState.Auth;
            await _messengerService.EditMessage(botStatus.ChatId, 
                update.GetMessageId(),
                $"Подтверждение по Email отключено. Пожалуйста, введите токен <code>{hashed}</code> для выполнения авторизации",
                new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("Назад","backtologin#")));
        }
        
        botStatus.State = BotState.Register;
        await _messengerService.EditMessage(botStatus.ChatId, 
            update.GetMessageId(),
            "Пожалуйста, введите адрес электронной почты",
            new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("Назад","backtologin#")));
    }
}