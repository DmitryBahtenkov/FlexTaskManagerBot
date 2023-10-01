using System.Text;
using FTM.Domain.Models.BotStatusModel;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Commands.Common;

public class InfoCommand : TelegramCommandBase
{
    private readonly MessengerService _messengerService;

    public InfoCommand(MessengerService messengerService)
    {
        _messengerService = messengerService;
    }

    public override string Command => "/info";
    public override Task Handle(Update update, BotStatus botStatus)
    {
        botStatus.State = BotState.ListenTasks;
        var sb = new StringBuilder();
        sb.AppendLine($"Идентификатор бота: {botStatus.Id}");
        sb.AppendLine($"Идентификатор пользователя в системе: {botStatus.UserId}");
        sb.AppendLine($"Режим бота: {botStatus.State}");

        if (botStatus.EntityId.HasValue)
        {
            sb.AppendLine($"Идентификатор редактируемой задачи: {botStatus.EntityId}");
            if (!string.IsNullOrEmpty(botStatus.EditingField))
            {
                sb.AppendLine($"Поле редактируемой задачи: {botStatus.EditingField}");
            }
        }

        return _messengerService.SendMessage(botStatus.ChatId, sb.ToString());
    }
    public override string? Description => "Вывести техническую информацию о боте";
}