using System.Linq.Expressions;

namespace FTM.Domain.Models.Base;

public interface IListFilter<TModel> where TModel : BaseEntity
{
    public IEnumerable<Expression<Func<TModel, bool>>> GetExpressions();
}