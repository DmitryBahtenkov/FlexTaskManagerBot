using FTM.Domain.Models.Base;
using FTM.Domain.Units;
using FTM.Infrastructure.DataAccess.Context;
using Microsoft.Extensions.Logging;

namespace FTM.Infrastructure;

public class UnitOfWork<TEntity> : IUnitOfWork<TEntity> where TEntity : BaseEntity
{
    private readonly FtmDbContext _ftmDbContext;
    private readonly IRepository<TEntity> _repository;
    private readonly ILogger<UnitOfWork<TEntity>> _logger;

    public UnitOfWork(FtmDbContext ftmDbContext,
        IRepository<TEntity> repository,
        ILogger<UnitOfWork<TEntity>> logger)
    {
        _ftmDbContext = ftmDbContext;
        _repository = repository;
        _logger = logger;
    }

    public async Task SaveChangesAsync()
    {
        try
        {
            await _ftmDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception on save changes");
            throw;
        }
    }

    public IRepository<TEntity> GetRepository()
    {
        return _repository;
    }
}