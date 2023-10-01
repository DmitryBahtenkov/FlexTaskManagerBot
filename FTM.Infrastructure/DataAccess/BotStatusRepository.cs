using FTM.Domain.Models.BotStatusModel;
using FTM.Infrastructure.DataAccess.Context;

namespace FTM.Infrastructure.DataAccess;

public class BotStatusRepository : BaseRepository<BotStatus>
{
    public BotStatusRepository(RepositoryContext context) : base(context)
    {
    }
}