using System.Linq.Expressions;
using FTM.Domain.Models.Base;
using FTM.Domain.Services;
using FTM.Domain.Units;
using FTM.Infrastructure.DataAccess.Context;
using FTM.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FTM.Infrastructure.DataAccess;

public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly RepositoryContext RepositoryContext;
    protected readonly DbSet<TEntity> Set;

    public BaseRepository(
        RepositoryContext context)
    {
        RepositoryContext = context;
        Set = context.FtmDbContext.Set<TEntity>();
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        var entry = await Set.AddAsync(entity);

        return entry.Entity;
    }

    public Task<TEntity> UpdateAsync(TEntity entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        var entry = Set.Update(entity);

        return Task.FromResult(entry.Entity);
    }

    public Task DeleteAsync(TEntity entity)
    {
        Set.Remove(entity);

        return Task.CompletedTask;
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await Set
            .Where(GetSecurityExpression())
            .FirstOrDefaultAsync(filter);
    }

    public async Task<TEntity?> ByIdAsync(int id)
    {
        return await Set
            .Where(GetSecurityExpression())
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await Set
            .Where(filter)
            .Where(GetSecurityExpression())
            .ToListAsync();
    }

    public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter)
    {
        return Set.Where(filter).Where(GetSecurityExpression());
    }

    public IQueryable<TEntity> Query(IEnumerable<Expression<Func<TEntity, bool>>> filter)
    {
        var query = Set.Where(GetSecurityExpression());
        foreach (var func in filter)
        {
            query = query.Where(func);
        }

        return query;
    }

    public async Task<List<TEntity>> ListByIdsAsync(params int[] ids)
    {
        return await Set
            .Where(GetSecurityExpression())
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();
    }

    public async Task<PagingResult<TEntity>> Page(Expression<Func<TEntity, bool>> filter, int page, int size = 5)
    {
        var result = await Set.Where(GetSecurityExpression()).Where(filter).Page(page, size).ToListAsync();
        var total = await Set.Where(GetSecurityExpression()).Where(filter).CountAsync();

        return new PagingResult<TEntity>(result, page, total, (int)Math.Ceiling((double)total / size), size);
    }

    public async Task<PagingResult<TEntity>> Page(IEnumerable<Expression<Func<TEntity, bool>>> filter, int page, int size = 5)
    {
        var query = Query(filter);
        var total = await query.CountAsync();
        var result = await query.Page(page, size).ToListAsync();
        
        return new PagingResult<TEntity>(result, page, total, (int)Math.Ceiling((double)total / size), size);
    }

    public async Task<List<TResult>> SelectDistinct<TResult>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TResult>> selector)
    {
        return await Set.Where(GetSecurityExpression()).Where(filter).Select(selector).Distinct().ToListAsync();
    }

    public Task<List<TResult>> RawQuery<TResult>(string query)
    {
        return Task.FromResult(new List<TResult>());
    }

    private Expression<Func<TEntity, bool>> GetSecurityExpression()
    {
        if (RepositoryContext.CurrentUserService.UserId.HasValue)
        {
            if (typeof(IEntityWithUserId).IsAssignableFrom(typeof(TEntity)))
            {
                return x => ((IEntityWithUserId)x).UserId == RepositoryContext.CurrentUserService.UserId;
            }
        }

        return _ => true;
    }
}