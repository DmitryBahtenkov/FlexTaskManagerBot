using FTM.Domain.Events.Bot;
using FTM.Domain.Exceptions;
using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.ServiceBus;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FTM.Infrastructure.BotImplementation.Handlers.Issues;

public class DoneIssueHandler : TelegramHandlerBase
{
    private readonly IRepository<Issue> _repository;
    private readonly IPublisherService _publisherService;
    private readonly MessengerService _messengerService;

    public DoneIssueHandler(
        IRepository<Issue> repository,
        IPublisherService publisherService, MessengerService messengerService)
    {
        _repository = repository;
        _publisherService = publisherService;
        _messengerService = messengerService;
    }

    public override string Command => "done";
    public override async Task Handle(Update update, BotStatus botStatus)
    {
        var issueId = int.Parse(ExactData(update.GetCallbackData()));
        var issue = await _repository.ByIdAsync(issueId);

        if (issue is null)
        {
            throw new BusinessException("Задача не найдена");
        }
        
        issue.ChangeStatus(IssueStatus.Finished);
        await _publisherService.Publish(new DoneTaskEvent
        {
            IssueId = issue.Id
        });

        await _messengerService.EditMessage(
            botStatus.ChatId, 
            update.GetMessageId(),
            $"Задача '{issue.Text}' завершена",
            BotButtons.ForDone(issue));
    }
}