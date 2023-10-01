using FTM.Domain.Helpers;
using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.IssueModel.DTO;
using FTM.Domain.Services;
using FTM.Domain.Services.Issues;
using FTM.Domain.Services.TextParser;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.Extensions;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Processors;

public class ListenTasksProcessor : IBotProcessor<Update>
{
    private readonly MessengerService _messengerService;
    private readonly MessageAttachmentService _attachmentService;
    private readonly CreateIssueCommand _createIssueCommand;
    private readonly ICurrentUserService _currentUserService;

    public ListenTasksProcessor(
        MessengerService messengerService,
        MessageAttachmentService attachmentService,
        ITextParser textParser,
        CreateIssueCommand createIssueCommand,
        ICurrentUserService currentUserService)
    {
        _messengerService = messengerService;
        _attachmentService = attachmentService;
        _createIssueCommand = createIssueCommand;
        _currentUserService = currentUserService;
    }

    public BotState State => BotState.ListenTasks;
    public async Task Process(Update update, BotStatus botStatus)
    {
        if (!botStatus.UserId.HasValue)
        {
            await _messengerService.SendMessage(botStatus.ChatId, "Вы не авторизованы");
        }

        var inputIssue = CreateIssueDto.TextOnly(update.GetMessageText());

        if (update.HasAttachment())
        {
            var attachment = _attachmentService.GetAttached(update.Message);
            inputIssue.IssueFile = attachment;
        }

        var result = await _createIssueCommand.Create(
            inputIssue,
            botStatus.UserId!.Value,
            $"{botStatus.ChatId}.{update.GetMessageId()}");

        // если задача уже создана и всё ок
        if (result.NeedForSave)
        {
            var markup = BotButtons.GetForIssue(result.Issue);
            await _messengerService.SendMessage(botStatus.ChatId, result.Issue.ToBotString(), markup);
        }
        else
        {
            result.Variants = result.Variants.FindAll(x => !string.IsNullOrEmpty(x.Text));

            var text = $"{result.Issue.Text}\n";

            if (result.Variants.Any())
            {
                text += "Выберите корректный вариант времени для задачи";
            }
            else
            {
                text += "Не удалось корректно распознать время в задаче";
            }

            var markup = BotButtons.GetForConfirmation(result.TempIssueId, result.GetVariants(_currentUserService.Timezone ?? 3));
            await _messengerService.SendMessage(botStatus.ChatId,
                text,
                markup);
        }
    }
}