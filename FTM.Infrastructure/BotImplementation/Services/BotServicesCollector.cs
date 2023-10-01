using FTM.Domain.Factories;
using FTM.Domain.Services;
using FTM.Infrastructure.BotImplementation.Base;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Services;

public class BotHandlersCollector : IServiceCollector
{
    public Predicate<Type> NeedToCollect =>
        x => typeof(TelegramHandlerBase).IsAssignableFrom(x) && x != typeof(TelegramHandlerBase);
    public void Collect(IServiceCollection serviceCollection, Type type)
    {
        serviceCollection.AddScoped(typeof(IBotHandler<Update>), type);
    }
}

public class BotCommandsCollector : IServiceCollector
{
    public Predicate<Type> NeedToCollect =>
        x => typeof(TelegramCommandBase).IsAssignableFrom(x) && x != typeof(TelegramCommandBase);
    public void Collect(IServiceCollection serviceCollection, Type type)
    {
        serviceCollection.AddScoped(typeof(IBotCommand<Update>), type);
    }
}

public class BotProcessorsCollector : IServiceCollector
{
    public Predicate<Type> NeedToCollect =>
        x => typeof(IBotProcessor<Update>).IsAssignableFrom(x) && x != typeof(IBotProcessor<Update>);
    public void Collect(IServiceCollection serviceCollection, Type type)
    {
        serviceCollection.AddScoped(typeof(IBotProcessor<Update>), type);
    }
}