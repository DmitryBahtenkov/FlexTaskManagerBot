using FTM.Domain.Models.StatisticModel;
using FTM.Infrastructure.DataAccess.Context;

namespace FTM.Infrastructure.DataAccess;

public class StatisticRepository : BaseRepository<Statistic>
{
    public StatisticRepository(RepositoryContext context) : base(context)
    {
    }
}