using Microsoft.Extensions.Logging;

namespace FTM.Infrastructure.Initialization;

public class InitializerService
{
    private readonly IEnumerable<IAsyncInitializer> _asyncInitializers;
    private readonly ILogger<InitializerService> _logger;

    public InitializerService(
        IEnumerable<IAsyncInitializer> asyncInitializers,
        ILogger<InitializerService> logger)
    {
        _asyncInitializers = asyncInitializers;
        _logger = logger;
    }

    public async Task Init()
    {
        foreach (var initializer in _asyncInitializers)
        {
            await TryInit(initializer);
        }
    }

    private async Task TryInit(IAsyncInitializer initializer)
    {
        var initializerName = initializer.GetType().Name;
        _logger.LogInformation("Start init {initializerType}", initializerName);
        try
        {
            await initializer.Initialize();
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "Error execute initializer {initializerType}", initializerName);
            throw;
        }
        
        _logger.LogInformation("Finish init {initializerType}", initializerName);
    }
}