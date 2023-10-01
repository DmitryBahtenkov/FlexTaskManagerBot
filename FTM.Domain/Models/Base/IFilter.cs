using System.Linq.Expressions;

namespace FTM.Domain.Models.Base;

public interface IFilter<TEntity> where TEntity : BaseEntity
{
    public Expression<Func<TEntity, bool>> ToExpression();
}