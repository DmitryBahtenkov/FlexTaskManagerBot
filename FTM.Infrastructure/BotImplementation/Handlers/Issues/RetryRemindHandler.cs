using FTM.Domain.Exceptions;
using FTM.Domain.Helpers;
using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.Extensions;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Handlers.Issues;

public class RetryRemindHandler : TelegramHandlerBase
{
    private readonly MessengerService _messengerService;
    private readonly IRepository<Issue> _issueRepository;

    public RetryRemindHandler(
        IRepository<Issue> issueRepository,
        MessengerService messengerService)
    {
        _issueRepository = issueRepository;
        _messengerService = messengerService;
    }

    public override string Command => "retryremind";
    public override async Task Handle(Update update, BotStatus botStatus)
    {
        var issueId = int.Parse(ExactData(update.GetCallbackData(), 1)!);
        var dateTime = ExactData(update.GetCallbackData(), 2)!.FromString();

        var issue = await _issueRepository.ByIdAsync(issueId);
        if (issue is null)
        {
            throw new BusinessException("Задача не найдена");
        }
        
        issue.RemindTime = dateTime;
        await _issueRepository.UpdateAsync(issue);
        
        var message = $"Напоминание установлено\n{issue.ToBotString()}";
        var buttons = BotButtons.GetForIssue(issue);

        await _messengerService.EditMessage(botStatus.ChatId, update.GetMessageId(), message, buttons);
    }
}