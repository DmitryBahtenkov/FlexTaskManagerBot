using FTM.Domain.Models.IssueModel;
using FTM.Infrastructure.DataAccess.Context;

namespace FTM.Infrastructure.DataAccess;

public class IssueRepository : BaseRepository<Issue>
{
    public IssueRepository(RepositoryContext context) : base(context)
    {
    }
}