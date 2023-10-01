using FTM.Domain.Events.Issues;
using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.ServiceBus;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.BotImplementation.Services.ButtonStyling;

namespace FTM.Infrastructure.BotImplementation.Consumers;

public class TelegramDailyScheduleConsumer : IConsumer<DailyIssuesEvent>
{
    private readonly IRepository<Issue> _issueRepository;
    private readonly IRepository<BotStatus> _botRepository;
    private readonly MessengerService _messengerService;

    public TelegramDailyScheduleConsumer(
        IRepository<Issue> issueRepository,
        IRepository<BotStatus> botRepository,
        MessengerService messengerService)
    {
        _issueRepository = issueRepository;
        _botRepository = botRepository;
        _messengerService = messengerService;
    }

    public override async Task Consume(DailyIssuesEvent data)
    {
        var bot = await _botRepository.GetAsync(x => x.UserId == data.UserId);

        if (bot is null)
        {
            return;
        }
        
        var issues = await _issueRepository.ListByIdsAsync(data.IssueIds.ToArray());

        var buttons = BotPagingButtons.GetForPaging(new PagingResult<Issue>(issues, 0, 0, 0));
        await _messengerService.SendMessage(bot.ChatId, "Задачи на сегодня", buttons);
    }
}