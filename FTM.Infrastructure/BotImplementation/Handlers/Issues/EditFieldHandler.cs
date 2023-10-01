using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.Services;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FTM.Infrastructure.BotImplementation.Handlers.Issues;

public class EditFieldHandler : TelegramHandlerBase
{
    private readonly MessengerService _messengerService;
    private readonly IRepository<Issue> _issueRepository;
    private readonly ICurrentUserService _currentUserService;

    public EditFieldHandler(MessengerService messengerService,
        IRepository<Issue> issueRepository,
        ICurrentUserService currentUserService)
    {
        _messengerService = messengerService;
        _issueRepository = issueRepository;
        _currentUserService = currentUserService;
    }

    public override string Command => "editfield";

    public override async Task Handle(Update update, BotStatus botStatus)
    {
        var field = ExactData(update.CallbackQuery!.Data!);
        botStatus.State = BotState.UpdatingTask;
        botStatus.EditingField = field;

        var messageId = update.CallbackQuery!.Message!.MessageId;

        var userTimeZone = _currentUserService.Timezone ?? 3;

        if (!botStatus.EntityId.HasValue)
        {
            await _messengerService.EditMessage(
                botStatus.ChatId, messageId, "Ошибка");
            return;
        }

        var issue = await _issueRepository.ByIdAsync(botStatus.EntityId.Value);

        if (issue is null)
        {
            await _messengerService.EditMessage(
                botStatus.ChatId, messageId, "Ошибка");
            return;
        }

        var property = issue.GetType().GetProperty(field)!;
        var issueValue = property.GetValue(issue);

        var message = "Введите значение";

        switch (field)
        {
            case nameof(Issue.RemindTime):
                message += $" в формате dd.MM.yyyy HH:mm, например: {DateTime.UtcNow.AddHours(userTimeZone):dd.MM.yyy HH:mm} " +
                           "или свободным вводом, например 'Завтра в 10', '14 марта в 11:00'";
                break;

            case nameof(Issue.IssueFile):
                message = "Отправьте вложение, которое хотите прикрепить к задаче";
                break;
        }

        var buttons = new List<IEnumerable<InlineKeyboardButton>>(2);
        if (issueValue is not null)
        {
            var str =
                property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?)
                    ? ((DateTime)issueValue).ToString("dd.MM.yyyy")
                    : issueValue.ToString();

            if (property.Name == nameof(Issue.RemindTime))
            {
                str =
                    property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?)
                        ? ((DateTime)issueValue).AddHours(userTimeZone).ToString("dd.MM.yyyy HH:mm")
                        : issueValue.ToString();
            }

            if (!string.IsNullOrEmpty(str) && property.Name != nameof(Issue.IssueFile))
            {
                message += $"\nТекущее значение: {str}";
            }
            
            buttons.Add(new[] { InlineKeyboardButton.WithCallbackData("Очистить", $"clearfield#{field}#{issue.Id}") });
        }

        buttons.Add(new[] { InlineKeyboardButton.WithCallbackData("Назад", $"backtoissue#{issue.Id}") });
        var markup = new InlineKeyboardMarkup(buttons);

        await _messengerService.EditMessage(botStatus.ChatId, messageId, message, markup);
    }
}