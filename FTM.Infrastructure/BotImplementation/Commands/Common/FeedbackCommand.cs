using FTM.Domain.Models.BotStatusModel;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.Services;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Commands.Common;

public class FeedbackCommand : TelegramCommandBase
{
    private readonly MessengerService _messengerService;

    public FeedbackCommand(MessengerService messengerService)
    {
        _messengerService = messengerService;
    }

    public override string Command => "feedback";
    public override Task Handle(Update update, BotStatus botStatus)
    {
        return _messengerService.SendMessage(botStatus.ChatId, "Расскажи о проблеме или своих пожеланиях в этой форме: https://forms.gle/hWw9gcPfjFVYsELi8");
    }

    public override string? Description => "Рассказать о проблеме, пожеланиях или просто сказать 'спасибо!'";
}