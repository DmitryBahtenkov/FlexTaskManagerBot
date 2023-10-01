using FTM.Domain.Services;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Services;

public class BotCommandsService
{
    private readonly IEnumerable<IBotCommand<Update>> _botCommands;

    public BotCommandsService(IEnumerable<IBotCommand<Update>> botCommands)
    {
        _botCommands = botCommands;
    }

    public List<BotCommand> GetCommands()
    {
        return _botCommands.Where(x => !string.IsNullOrEmpty(x.Description)).Select(x => new BotCommand
        {
            Command = x.Command,
            Description = x.Description!
        }).ToList();
    }
}