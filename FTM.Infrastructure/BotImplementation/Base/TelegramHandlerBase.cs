using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Services;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Base;

public abstract class TelegramHandlerBase : IBotHandler<Update>
{
    public abstract string Command { get; }
    public abstract Task Handle(Update update, BotStatus botStatus);

    public bool Contains(string text)
    {
        return Command == text.Split('#').First();
    }
    
    protected string ExactData(string command)
    {
        return command.Split('#')[1];
    }
    
    protected string? ExactData(string command, int index)
    {
        var splitted = command.Split('#');

        if (splitted.Length > index)
        {
            return splitted[index];
        }

        return default;
    }
}