using FTM.Domain.Events.Statistics;
using FTM.Domain.ServiceBus;
using FTM.Domain.Services;

namespace FTM.Infrastructure.Consumers;

public class StatisticConsumer : IConsumer<StatisticEntryEvent>
{
    private readonly IStatisticService _statisticService;

    public StatisticConsumer(IStatisticService statisticService)
    {
        _statisticService = statisticService;
    }

    public override Task Consume(StatisticEntryEvent data)
    {
        return _statisticService.AddOrIncrementEntry(data.Key, data.Data);
    }
}