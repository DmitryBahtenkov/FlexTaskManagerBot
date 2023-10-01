using System.Linq.Expressions;
using FTM.Domain.Models.Base;

namespace FTM.Domain.Units;

public interface IRepository
{
    
}

public interface IRepository<TEntity> : IRepository where TEntity : BaseEntity
{
    Task<TEntity> AddAsync(TEntity entity);

    Task<TEntity> UpdateAsync(TEntity entity);

    Task DeleteAsync(TEntity entity);

    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter);
    Task<TEntity?> ByIdAsync(int id);

    Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> filter);
    IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter);
    IQueryable<TEntity> Query(IEnumerable<Expression<Func<TEntity, bool>>> filter);
    Task<List<TEntity>> ListByIdsAsync(params int[] ids);
    Task<PagingResult<TEntity>> Page(Expression<Func<TEntity, bool>> filter, int page, int size = 5);
    Task<PagingResult<TEntity>> Page(IEnumerable<Expression<Func<TEntity, bool>>> filter, int page, int size = 5);

    Task<List<TResult>> SelectDistinct<TResult>(
        Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, TResult>> selector);
}

public record PagingResult<TEntity>(List<TEntity> Items, int CurrentPage, int TotalItems, int TotalPages, int PageSize = 5);