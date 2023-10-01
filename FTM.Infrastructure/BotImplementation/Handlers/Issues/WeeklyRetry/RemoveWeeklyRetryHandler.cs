using FTM.Domain.Exceptions;
using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.Extensions;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Handlers.Issues.WeeklyRetry
{
    class RemoveWeeklyRetryHandler : TelegramHandlerBase
    {
        private readonly IRepository<Issue> _issueRepository;
        private readonly MessengerService _messengerService;
        public RemoveWeeklyRetryHandler(IRepository<Issue> issueRepository, MessengerService messengerService)
        {
            _issueRepository = issueRepository;
            _messengerService = messengerService;
        }
        public override string Command => "removeretry";

        public override async Task Handle(Update update, BotStatus botStatus)
        {
            var issueParam = ExactData(update.GetCallbackData(), 1) ?? throw new BusinessException("Задача не найдена");
            var issueId = int.Parse(issueParam);

            var issue = await _issueRepository.ByIdAsync(issueId);

            if (issue is null)
            {
                throw new BusinessException("Задача не найдена");
            }

            issue.RemoveRetry();

            await _messengerService.EditMessage(
                botStatus.ChatId,
                update.GetMessageId(),
                issue.ToBotString(),
                BotButtons.GetForIssue(issue));
        }
    }
}