using FTM.Domain.Models.UserModel;
using FTM.Infrastructure.DataAccess.Context;

namespace FTM.Infrastructure.DataAccess;

public class UserRepository : BaseRepository<User>
{
    public UserRepository(RepositoryContext context) : base(context)
    {
    }
}