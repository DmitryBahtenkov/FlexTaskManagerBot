using FTM.Domain.Services;

namespace FTM.Infrastructure.DataAccess.Context;

public class RepositoryContext
{
    public RepositoryContext(ICurrentUserService currentUserService, FtmDbContext ftmDbContext)
    {
        CurrentUserService = currentUserService;
        FtmDbContext = ftmDbContext;
    }

    public ICurrentUserService CurrentUserService { get; }
    public FtmDbContext FtmDbContext { get; }
}