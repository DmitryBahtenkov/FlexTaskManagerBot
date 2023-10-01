using System.Linq.Expressions;
using FTM.Domain.Models.Base;

namespace FTM.Domain.Models.IssueModel;

public class IssueFilter : IFilter<Issue>, IListFilter<Issue>
{
    public string? Folder { get; set; }
    public IssueStatus Status { get; set; } = IssueStatus.Started;
    public DateTime? FromRemindTime { get; set; }
    public DateTime? ToRemindTime { get; set; }
    public Expression<Func<Issue, bool>> ToExpression()
    {
        Func<Issue, bool> functions = x => x.Status == Status;

        if (!string.IsNullOrEmpty(Folder))
        {
            functions += x => x.Folder == Folder;
        }

        if (!string.IsNullOrEmpty(Folder))
        {
            functions += x => x.Folder == Folder;
        }

        if (FromRemindTime.HasValue && ToRemindTime.HasValue)
        {
            functions += x => x.RemindTime >= FromRemindTime && x.RemindTime <= ToRemindTime;
        }

        return x => functions(x);
    }

    public IEnumerable<Expression<Func<Issue, bool>>> GetExpressions()
    {
        yield return x => x.Status == Status;

        if (!string.IsNullOrEmpty(Folder))
        {
            yield return x => x.Folder == Folder;
        }

        if (FromRemindTime.HasValue && ToRemindTime.HasValue)
        {
            yield return x => x.RemindTime >= FromRemindTime && x.RemindTime <= ToRemindTime;
        }
    }
}