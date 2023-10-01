using FTM.Domain.Exceptions;
using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.Extensions;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Handlers.Issues;

public class ClearFieldHandler : TelegramHandlerBase
{
    private readonly IRepository<Issue> _issueRepository;
    private readonly MessengerService _messengerService;

    public ClearFieldHandler(IRepository<Issue> issueRepository, MessengerService messengerService)
    {
        _issueRepository = issueRepository;
        _messengerService = messengerService;
    }

    public override string Command => "clearfield";
    public override async Task Handle(Update update, BotStatus botStatus)
    {
        var field = ExactData(update.GetCallbackData());
        botStatus.State = BotState.ListenTasks;
        var issueId = botStatus.EntityId ?? int.Parse(ExactData(update.GetCallbackData(), 2) ?? string.Empty);
        var issue = await _issueRepository.ByIdAsync(issueId);
        if (issue is null)
        {
            throw new BusinessException("Задача не найдена");
        }

        var property = issue.GetType().GetProperty(field)!;

        if (field == nameof(issue.RemindTime))
        {
            issue.RemoveRetry();
        }

        if (field == nameof(issue.IssueFile))
        {
            issue.RemoveAttachment();
            await _messengerService.SendMessage(botStatus.ChatId, $"Вложение задачи {issue.Text} удалено");
        }

        property.SetValue(issue, null);

        await _issueRepository.UpdateAsync(issue);

        var buttons = BotButtons.GetForIssue(issue);
        await _messengerService.EditMessage(botStatus.ChatId, update.GetMessageId(), issue.ToBotString(), buttons);
    }
}