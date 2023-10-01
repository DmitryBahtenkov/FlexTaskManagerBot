using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Services;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Base;

public abstract class TelegramCommandBase : IBotCommand<Update>
{
    private string ChatId { get; set; }
    public abstract string Command { get; }
    public abstract Task Handle(Update update, BotStatus botStatus);

    public bool Contains(string text)
    {
        return text.StartsWith("/") && !string.IsNullOrEmpty(Command) && text.Contains(Command);
    }

    protected string GetChatId(Update update)
    {
        if (string.IsNullOrEmpty(ChatId))
        {
            ChatId = update.Message!.Chat.Id.ToString();
        }

        return ChatId;
    }

    public abstract string? Description { get; }
}