using FTM.Domain.Exceptions;
using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FTM.Infrastructure.BotImplementation.Handlers.Issues
{
    public class ShowAttachmentHandler : TelegramHandlerBase
    {
        private readonly MessengerService _messengerService;
        private readonly IRepository<Issue> _issueRepository;

        public override string Command => "get-attach";

        public ShowAttachmentHandler(
            MessengerService messengerService,
            IRepository<Issue> issueRepository)
        {
            _issueRepository = issueRepository;
            _messengerService = messengerService;
        }

        public override async Task Handle(Update update, BotStatus botStatus)
        {
            var issueId = int.Parse(ExactData(update.GetCallbackData(), 1)!);
            var issue = await _issueRepository.ByIdAsync(issueId);

            if (issue is null)
            {
                throw new BusinessException("Задача не найдена");
            }

            if (issue.IssueFile is null)
            {
                throw new BusinessException("В задаче нет вложения");
            }

            int? attachmentMsgId = null;
            if (int.TryParse(ExactData(update.GetCallbackData(), 2), out int messageId))
            {
                await _messengerService.DeleteMessage(botStatus.ChatId, messageId);
            }
            else
            {
                var attachmentMsg = await _messengerService.SendAttachment(botStatus.ChatId, issue!.IssueFile!, $"Приложение к задаче {issue.Text}");
                attachmentMsgId = attachmentMsg.MessageId;
            }

            var buttons = update!.CallbackQuery!.Message!.ReplyMarkup!.InlineKeyboard!.Skip(1).ToList();
            buttons.Insert(0, BotButtons.GetForIssue(issue, attachmentMsgId).InlineKeyboard.First().ToList());

            await _messengerService.EditMessage(botStatus.ChatId, update.GetMessageId(), update.GetMessageText(), new InlineKeyboardMarkup(buttons));
        }
    }
}