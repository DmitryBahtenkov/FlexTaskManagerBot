using FTM.Domain.Models.BotStatusModel;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Handlers.SettingsPage;

public class BackToSettingsHandler : TelegramHandlerBase
{
    private readonly MessengerService _messengerService;

    public BackToSettingsHandler(MessengerService messengerService)
    {
        _messengerService = messengerService;
    }

    public override string Command => "backtosettings";
    public override Task Handle(Update update, BotStatus botStatus)
    {
        var buttons = BotButtons.Settings();

        return _messengerService.EditMessage(botStatus.ChatId, update.CallbackQuery!.Message!.MessageId,"Выберите настройку", buttons);
    }
}