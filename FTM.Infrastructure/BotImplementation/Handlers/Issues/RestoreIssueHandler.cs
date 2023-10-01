using FTM.Domain.Exceptions;
using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Handlers.Issues
{
    internal class RestoreIssueHandler : TelegramHandlerBase
    {
    private readonly MessengerService _messengerService;
        private readonly IRepository<Issue> _repository;
        public override string Command => "restore";

        public RestoreIssueHandler(MessengerService messengerService, IRepository<Issue> repository)
        {
            _repository= repository;
            _messengerService= messengerService;
        }

        public override async Task Handle(Update update, BotStatus botStatus)
        {
            if (!int.TryParse(ExactData(update.GetCallbackData()), out int issueId))
            {
                throw new BusinessException("Внутренняя ошибка");
            }

            var issue = await _repository.ByIdAsync(issueId);

            if (issue is null)
            {
                throw new BusinessException("Задача не была найдена");
            }

            issue.ChangeStatus(IssueStatus.Started);

            await _messengerService.EditMessage(botStatus.ChatId, update.GetMessageId(), issue.ToBotString(), BotButtons.GetForIssue(issue));
        }
    }
}
