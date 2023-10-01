using System.Text;
using FTM.Domain.Events.Issues;
using FTM.Domain.Helpers;
using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.IssueFileModel;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.ServiceBus;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Services;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types.ReplyMarkups;

namespace FTM.Infrastructure.BotImplementation.Consumers;

public class TelegramRemindConsumer : IConsumer<RemindIssueEvent>
{
    private readonly IRepository<BotStatus> _botStatusRepository;
    private readonly MessengerService _messengerService;
    private readonly ILogger<TelegramRemindConsumer> _logger;

    public TelegramRemindConsumer(IRepository<BotStatus> botStatusRepository, MessengerService messengerService, ILogger<TelegramRemindConsumer> logger)
    {
        _botStatusRepository = botStatusRepository;
        _messengerService = messengerService;
        _logger = logger;
    }

    public override async Task Consume(RemindIssueEvent data)
    {
        if (!data.IssueToRemind.RemindTime.HasValue)
        {
            return;
        }

        var status = await _botStatusRepository.GetAsync(x => x.UserId == data.IssueToRemind.UserId);

        if (status is null)
        {
            _logger.LogWarning("Bot Status with user id {id} not found", data.IssueToRemind.UserId);
            return;
        }

        var sb = new StringBuilder();
        sb.AppendLine($"Напоминание о задаче: {data.IssueToRemind.Text}");

        if (!string.IsNullOrEmpty(data.IssueToRemind.Note))
        {
            sb.AppendLine($"\n{data.IssueToRemind.Note}");
        }

        var hasAttachment = data.IssueToRemind.IssueFile is not null;
        var mainButtons = BotButtons.GetForIssue(data.IssueToRemind.Id, hasAttachment);
        var nextHourRemind = InlineKeyboardButton.WithCallbackData(
            "Напомнить через час",
            $"retryremind#{data.IssueToRemind.Id}#{data.IssueToRemind.RemindTime!.Value.AddHours(1).ToBotFormatWithTime()}");

        var nextDayRemind = InlineKeyboardButton.WithCallbackData(
            "Напомнить завтра",
            $"retryremind#{data.IssueToRemind.Id}#{data.IssueToRemind.RemindTime!.Value.AddDays(1).ToBotFormatWithTime()}");

        var nextCustomRemind = InlineKeyboardButton.WithCallbackData(
            "Указать следующее напоминание",
            $"editfield#{nameof(Issue.RemindTime)}#{data.IssueToRemind.Id}");

        var buttons = new List<List<InlineKeyboardButton>>(4)
        {
            new (mainButtons) ,
            new (1) { nextHourRemind },
            new (1) { nextDayRemind },
            new (1) { nextCustomRemind },
        };

        await _messengerService.SendMessage(status.ChatId, sb.ToString(), new InlineKeyboardMarkup(buttons));
    }
}