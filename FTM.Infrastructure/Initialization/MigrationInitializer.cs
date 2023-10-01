using FTM.Infrastructure.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FTM.Infrastructure.Initialization;

public class MigrationInitializer : IAsyncInitializer
{
    private readonly FtmDbContext _ftmDbContext;
    private readonly ILogger<MigrationInitializer> _logger;

    public MigrationInitializer(
        FtmDbContext ftmDbContext,
        ILogger<MigrationInitializer> logger)
    {
        _ftmDbContext = ftmDbContext;
        _logger = logger;
    }

    public async Task Initialize()
    {
        var migrations = await _ftmDbContext.Database.GetPendingMigrationsAsync();
        _logger.LogInformation("Try to apply migrations: {migrations}", string.Join(", ", migrations));
        try
        {
            await _ftmDbContext.Database.MigrateAsync();
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "Migrations error");
            throw;
        }
    }
}