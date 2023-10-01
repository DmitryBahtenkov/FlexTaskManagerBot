using FTM.Domain.Models.StatisticModel;

namespace FTM.Domain.Services;

public interface IStatisticService
{
    public Task<Statistic> AddOrIncrementEntry(string key, object? data = null);
}