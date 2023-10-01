using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Services;

namespace FTM.Domain.Factories;

public class BotHandlerFactory<TUpdate>
{
    private readonly IEnumerable<IBotHandler<TUpdate>> _botHandlers;

    public BotHandlerFactory(IEnumerable<IBotHandler<TUpdate>> botHandlers)
    {
        _botHandlers = botHandlers;
    }

    public IBotHandler<TUpdate>? Get(string text)
    {
        return _botHandlers.FirstOrDefault(x => x.Contains(text));
    }
}

public class BotCommandFactory<TUpdate>
{
    private readonly IEnumerable<IBotCommand<TUpdate>> _botCommands;

    public BotCommandFactory(IEnumerable<IBotCommand<TUpdate>> botHandlers)
    {
        _botCommands = botHandlers;
    }

    public IBotCommand<TUpdate> Get(string text)
    {
        return _botCommands.FirstOrDefault(x => x.Contains(text)) 
               ?? _botCommands.Single(x => string.IsNullOrEmpty(x.Command));
    }
}

public class BotProcessorFactory<TUpdate>
{
    private readonly IEnumerable<IBotProcessor<TUpdate>> _botProcessors;

    public BotProcessorFactory(IEnumerable<IBotProcessor<TUpdate>> botProcessors)
    {
        _botProcessors = botProcessors;
    }

    public IBotProcessor<TUpdate> Get(BotState state)
    {
        return _botProcessors.First(x => x.State == state);
    }
}