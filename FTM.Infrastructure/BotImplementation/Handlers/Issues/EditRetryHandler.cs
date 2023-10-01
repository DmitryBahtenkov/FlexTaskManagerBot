using FTM.Domain.Models.BotStatusModel;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.Extensions;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Handlers.Issues;

public class EditRetryHandler : TelegramHandlerBase
{
    private readonly MessengerService _messengerService;

    public EditRetryHandler(MessengerService messengerService)
    {
        _messengerService = messengerService;
    }

    public override string Command => "editretry";
    public override async Task Handle(Update update, BotStatus botStatus)
    {
        var issueId = int.Parse(ExactData(update.GetCallbackData()));
        var buttons = BotButtons.ForRetry(issueId);

        await _messengerService.EditMessage(botStatus.ChatId,
            update.GetMessageId(),
            "Редактирование повтора задачи",
            buttons);
    }
}