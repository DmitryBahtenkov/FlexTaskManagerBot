using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.BotImplementation.Services.ButtonStyling;
using FTM.Infrastructure.Extensions;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Handlers.Issues;

public class IssuesListHandler : TelegramHandlerBase
{
    private readonly IRepository<Issue> _issueRepository;
    private readonly MessengerService _messenger;

    public IssuesListHandler(IRepository<Issue> issueRepository, MessengerService messenger)
    {
        _issueRepository = issueRepository;
        _messenger = messenger;
    }

    public override string Command => "issues";
    public override async Task Handle(Update update, BotStatus botStatus)
    {
        var filter = new IssueFilter
        {
            Status = IssueStatus.Started
        };

        botStatus.Filter = filter;

        var paging = await _issueRepository.Page(filter.GetExpressions(), 1);

        await _messenger.EditMessage(
            botStatus.ChatId,
            update.GetMessageId(),
            $"Всего элементов: {paging.TotalItems}",
            BotPagingButtons.GetForPaging(paging));
    }
}