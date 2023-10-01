using FTM.Domain.Models.BotStatusModel;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Commands.Common;

public class ResetCommand : TelegramCommandBase
{
    private readonly MessengerService _messengerService;

    public ResetCommand(MessengerService messengerService)
    {
        _messengerService = messengerService;
    }

    public override string Command => "reset";
    public override async Task Handle(Update update, BotStatus botStatus)
    {
        botStatus.State = BotState.ListenTasks;
        botStatus.Filter = default;
        botStatus.EditingField = default;
        botStatus.EntityId = null;

        await _messengerService.SendMessage(botStatus.ChatId, "Бот сброшен до дефолтного состояния");
    }

    public override string? Description => "Сбросить бота";
}