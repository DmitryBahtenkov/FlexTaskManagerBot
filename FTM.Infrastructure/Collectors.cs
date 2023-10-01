using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.DataAccess;
using FTM.Infrastructure.Initialization;
using FTM.Infrastructure.Redis;

namespace FTM.Infrastructure;

public class Collectors
{
    public static ICollector[] GetCollectors => Array.Empty<ICollector>();
    
    public static IServiceCollector[] ServiceCollectors => new[]
    {
        (IServiceCollector)new RepositoryCollector(),
        (IServiceCollector)new ConsumerCollector(),
        (IServiceCollector)new BotCommandsCollector(),
        (IServiceCollector)new BotHandlersCollector(),
        (IServiceCollector)new BotProcessorsCollector(),
        (IServiceCollector)new InitializerCollector(),
    };
}