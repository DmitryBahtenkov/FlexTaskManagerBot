using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Commands.Settings;

public class EditSettingsCommand : TelegramCommandBase
{
    public EditSettingsCommand(MessengerService messengerService)
    {
        _messengerService = messengerService;
    }

    public override string Command => "settings";

    private readonly MessengerService _messengerService;

    public override Task Handle(Update update, BotStatus botStatus)
    {
        botStatus.State = BotState.ListenTasks;
        var buttons = BotButtons.Settings();

        return _messengerService.SendMessage(botStatus.ChatId, "Выберите настройку", buttons);
    }

    public override string? Description => "Просмотр и редактирование настроек";
}