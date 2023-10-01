using FTM.Domain.Exceptions;
using FTM.Domain.Helpers;
using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.Extensions;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Handlers.Issues.WeeklyRetry
{
    class SetWeeklyRetryHandler : TelegramHandlerBase
    {
        private readonly IRepository<Issue> _issueRepository;
        private readonly MessengerService _messengerService;

        public SetWeeklyRetryHandler(IRepository<Issue> issueRepository, MessengerService messengerService)
        {
            _issueRepository = issueRepository;
            _messengerService = messengerService;
        }

        public override string Command => "setretry-weekly";

        public override async Task Handle(Update update, BotStatus botStatus)
        {
            var issueParam = ExactData(update.GetCallbackData(), 1) ?? throw new BusinessException("Задача не найдена");
            var issueId = int.Parse(issueParam);
            var @params = ExactData(update.GetCallbackData(), 2)?.Split('-');

            List<int> selectedParam = new List<int>();
            foreach (var item in @params)
            {
                if (int.TryParse(item, out int i))
                {
                    selectedParam.Add(i);
                }
            }

            if (selectedParam.Count == 0)
            {
                throw new BusinessException("Не выбраны дни повтора");
            }

            var selectedDays = selectedParam.Select(x => x.ToRuWeekday()).ToArray();

            var issue = await _issueRepository.ByIdAsync(issueId);

            if (issue is null)
            {
                throw new BusinessException("Задача не найдена");
            }

            var today = update.CallbackQuery!.Message!.Date.DayOfWeek.ToRuWeekday();
            var retry = RetryService.GetRetryPeriods(selectedDays, today);

            issue.SetRetry(retry.orderedSelection, retry.retryWeekday, retry.nextRetryInDays);

            await _messengerService.EditMessage(
                botStatus.ChatId,
                update.GetMessageId(),
                issue.ToBotString(),
                BotButtons.GetForIssue(issue));
        }
    }
}