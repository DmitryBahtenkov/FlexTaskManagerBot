using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.BotImplementation.Services.ButtonStyling;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Commands.Issues;

public class UnsuccessIssuesCommand : TelegramCommandBase
{
    private readonly IRepository<Issue> _issueRepository;
    private readonly MessengerService _messenger;
    
    public UnsuccessIssuesCommand(IRepository<Issue> issueRepository, MessengerService messenger)
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
        botStatus.State = BotState.ListenTasks;

        var paging = await _issueRepository.Page(filter.GetExpressions(), 1);

        await _messenger.SendMessage(
            botStatus.ChatId,
            $"Всего элементов: {paging.TotalItems}",
            BotPagingButtons.GetForPaging(paging));
    }

    public override string? Description => "Список незавершённых задач";
}