using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.BotImplementation.Services.ButtonStyling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FTM.Infrastructure.BotImplementation.Handlers.Issues;

public class GetFolderIssuesHandler : TelegramHandlerBase
{

    private readonly IRepository<Issue> _issueRepository;
    private readonly MessengerService _messengerService;

    public GetFolderIssuesHandler(IRepository<Issue> issueRepository, MessengerService messengerService)
    {
        _issueRepository = issueRepository;
        _messengerService = messengerService;
    }

    public override string Command => "fromfolder";
    
    public override async Task Handle(Update update, BotStatus botStatus)
    {
        var folder = ExactData(update.CallbackQuery!.Data!);
        var filter = new IssueFilter
        {
            Folder = folder,
            Status = IssueStatus.Started
        };

        var issues = await _issueRepository.Page(filter.GetExpressions(), 1);

        var buttons = BotPagingButtons.GetForPaging(issues, InlineKeyboardButton.WithCallbackData("Назад", $"backtofolder#{folder}"));
        await _messengerService.EditMessage(botStatus.ChatId, update.CallbackQuery!.Message!.MessageId,
            $"Всего элементов: {issues.TotalItems}", buttons);
    }
}