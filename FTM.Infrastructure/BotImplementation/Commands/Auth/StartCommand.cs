using FTM.Domain.Helpers;
using FTM.Domain.Models.BotStatusModel;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FTM.Infrastructure.BotImplementation.Commands.Auth;

[Anon]
public class StartCommand : TelegramCommandBase
{
    private readonly MessengerService _messengerService;

    public StartCommand(
        MessengerService messengerService)
    {
        _messengerService = messengerService;
    }

    public override string Command => "start";
    
    public override async Task Handle(Update update, BotStatus botStatus)
    {
        var markup = new InlineKeyboardMarkup(new[]
        {
            InlineKeyboardButton.WithCallbackData("Войти", "login#"), 
            InlineKeyboardButton.WithCallbackData("Я новый пользователь", "register#"), 
        });
            
        await _messengerService.SendMessage(GetChatId(update), 
            "Привет! Я бот для управления личными задачами в телеграме. " +
            "Я помогу тебе организовать свой день и не забывать важные дела. " +
            "Если ты только начинаешь пользоваться мной, " +
            "то рекомендую ознакомиться с этим гайдом: https://telegra.ph/Gajd-po-Flex-Task-Manager-04-05. " +
            "Здесь ты найдешь подробную инструкцию и много полезной информации. " +
            "Приятного использования!\n" + "\nВАЖНО! Настоятельно рекомендую вам не сохранять в боте приватную и личную информацию," +
            " такую как пароли, номера карт и т.д. Разработчики не несут ответственности за утечку или потерю этой информации",
            markup);
    }

    public override string? Description { get; }
}