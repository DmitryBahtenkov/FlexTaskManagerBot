using FTM.Domain.Models.Base;

namespace FTM.Domain.Units;

public interface IUnitOfWork<TEntity> where TEntity : BaseEntity
{
    public Task SaveChangesAsync();
    public IRepository<TEntity> GetRepository();
}