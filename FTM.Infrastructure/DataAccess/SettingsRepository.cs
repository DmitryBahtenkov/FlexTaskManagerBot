using FTM.Domain.Models.SettingsModel;
using FTM.Infrastructure.DataAccess.Context;

namespace FTM.Infrastructure.DataAccess;

public class SettingsRepository : BaseRepository<Settings>
{
    public SettingsRepository(RepositoryContext context) : base(context)
    {
    }
}