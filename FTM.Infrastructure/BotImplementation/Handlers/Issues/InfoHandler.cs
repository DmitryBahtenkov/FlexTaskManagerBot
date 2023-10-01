using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.Extensions;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Handlers.Issues
{
    public class InfoHandler : TelegramHandlerBase
    {
        private readonly IRepository<Issue> _issueRepository;
        private readonly MessengerService _messengerService;

        public InfoHandler(
            IRepository<Issue> repository,
            MessengerService messengerService)
        {
            _messengerService = messengerService;
            _issueRepository = repository;
        }

        public override string Command => "info";

        public override async Task Handle(Update update, BotStatus botStatus)
        {
            var issueId = ExactData(update.CallbackQuery!.Data!);
            var messageId = update.CallbackQuery!.Message!.MessageId;
            var issue = await _issueRepository.ByIdAsync(int.Parse(issueId));

            if (issue is null)
            {
                await _messengerService.EditMessage(botStatus.ChatId, messageId, "Задача не найдена");
            }
            else
            {
                var buttons = BotButtons.GetForIssue(issue);
                await _messengerService.EditMessage(botStatus.ChatId, messageId, issue.ToBotString(), buttons);
            }
        }
    }
}