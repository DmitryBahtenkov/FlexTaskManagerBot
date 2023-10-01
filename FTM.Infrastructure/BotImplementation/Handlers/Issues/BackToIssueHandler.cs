using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.Extensions;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Handlers.Issues;

public class BackToIssueHandler : TelegramHandlerBase
{
    private readonly IRepository<Issue> _issueRepository;
    private readonly MessengerService _messenger;
    
    public BackToIssueHandler(IRepository<Issue> issueRepository, MessengerService messenger)
    {
        _issueRepository = issueRepository;
        _messenger = messenger;
    }
    public override string Command => "backtoissue";
    public override async Task Handle(Update update, BotStatus botStatus)
    {
        botStatus.State = BotState.ListenTasks;
        var issueId = ExactData(update.CallbackQuery!.Data!);

        var issue = await _issueRepository.ByIdAsync(int.Parse(issueId));

        if (issue is null)
        {
            await _messenger.SendMessage(botStatus.ChatId, "Задача не найдена");
            return;
        }

        var buttons = BotButtons.GetForIssue(issue);
        await _messenger.EditMessage(
            botStatus.ChatId,
            update.CallbackQuery!.Message!.MessageId, 
            issue.ToBotString(),
            buttons);
    }
}