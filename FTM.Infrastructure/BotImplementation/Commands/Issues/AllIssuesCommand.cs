using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.BotImplementation.Services.ButtonStyling;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Commands.Issues;

public class AllIssuesCommand : TelegramCommandBase
{
    private readonly IRepository<Issue> _issueRepository;
    private readonly MessengerService _messenger;
    
    public AllIssuesCommand(IRepository<Issue> issueRepository, MessengerService messenger)
    {
        _issueRepository = issueRepository;
        _messenger = messenger;
    }

    public override string Command => "all";
    
    public override async Task Handle(Update update, BotStatus botStatus)
    {
        botStatus.Filter = null;
        botStatus.State = BotState.ListenTasks;

        var paging = await _issueRepository.Page(x => true, 1);
        var markup = BotPagingButtons.GetForPaging(paging);

        await _messenger.SendMessage(botStatus.ChatId, $"Всего элементов: {paging.TotalItems}", markup);
    }

    public override string? Description => "Списов всех задач";
}