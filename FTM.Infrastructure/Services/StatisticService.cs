using FTM.Domain.Models.StatisticModel;
using FTM.Domain.Services;
using FTM.Domain.Units;

namespace FTM.Infrastructure.Services;

public class StatisticService : IStatisticService
{
    private readonly IUnitOfWork<Statistic> _statisticUnit;
    private readonly IRepository<Statistic> _statisticRepository;

    public StatisticService(IUnitOfWork<Statistic> statisticUnit)
    {
        _statisticUnit = statisticUnit;
        _statisticRepository = statisticUnit.GetRepository();
    }

    public async Task<Statistic> AddOrIncrementEntry(string key, object? data = null)
    {
        var statisticEntry = await _statisticRepository.GetAsync(x => x.Key == key);

        var task = statisticEntry is null 
            ? CreateStatisticEntry(key, data) 
            : IncrementExisting(statisticEntry, data);
        
        statisticEntry = await task;
        await _statisticUnit.SaveChangesAsync();

        return statisticEntry;
    }

    private Task<Statistic> IncrementExisting(Statistic statistic, object? data = null)
    {
        statistic.Counter++;
        statistic.Data = data;
        return _statisticRepository.UpdateAsync(statistic);
    }

    private Task<Statistic> CreateStatisticEntry(string key, object? data = null)
    {
        var entry = new Statistic
        {
            Key = key,
            Data = data,
            Counter = 1
        };

        return _statisticRepository.AddAsync(entry);
    }
}