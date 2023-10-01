using FTM.Infrastructure.BotImplementation.Services;
using Microsoft.Extensions.Options;

namespace FTM.Infrastructure.Initialization;

public class BotClientInitializer : IAsyncInitializer
{
    private readonly BotCommandsService _botCommandsService;
    private readonly IOptions<BotOptions> _options;

    public BotClientInitializer(
        BotCommandsService botCommandsService,
        IOptions<BotOptions> options)
    {
        _botCommandsService = botCommandsService;
        _options = options;
    }

    public Task Initialize()
    {
        return BotClientService.Initialize(_options.Value, _botCommandsService);
    }
}