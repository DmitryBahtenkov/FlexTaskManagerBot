using FTM.Domain.Exceptions;
using FTM.Domain.Helpers;
using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.BotImplementation.Services.ButtonStyling;
using FTM.Infrastructure.Extensions;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Handlers.Issues.WeeklyRetry
{
    class EditWeeklyRetryHandler : TelegramHandlerBase
    {
        private readonly IRepository<Issue> _issueRepository;
        private readonly MessengerService _messengerService;

        public EditWeeklyRetryHandler(MessengerService messengerService, IRepository<Issue> issueRepository)
        {
            _messengerService = messengerService;
            _issueRepository = issueRepository;
        }
        public override string Command => "editretry-weekly";

        public override async Task Handle(Update update, BotStatus botStatus)
        {
            var issueId = int.Parse(ExactData(update.GetCallbackData()));

            var issue = await _issueRepository.ByIdAsync(issueId);

            if (issue is null)
            {
                throw new BusinessException("Задача не найдена");
            }

            //uncheck на единственный выбранный день или RetryDays Null -> будет использоваться дефолтное значение
            var checkedDays = issue.RetrySettings?.RetryWeekDays?.AsEnumerable();
            var data = ExactData(update.GetCallbackData(), 2);

            if (data?.Length > 0)
            {
                checkedDays = data.Split('-').Select(x => int.Parse(x).ToRuWeekday());
            }

            var buttons = BotWeeklyRetryButtons.ForWeeklyRetry(issueId, checkedDays);

            await _messengerService.EditMessage(botStatus.ChatId,
                update.GetMessageId(),
                "Редактирование еженедельного повтора задачи",
                buttons);
        }
    }
}