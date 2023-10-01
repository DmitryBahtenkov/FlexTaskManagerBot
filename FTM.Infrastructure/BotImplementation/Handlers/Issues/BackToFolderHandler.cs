using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FTM.Infrastructure.BotImplementation.Handlers.Issues;

public class BackToFolderHandler : TelegramHandlerBase
{
    private readonly IRepository<Issue> _issueRepository;
    private readonly MessengerService _messenger;

    public BackToFolderHandler(IRepository<Issue> issueRepository, MessengerService messenger)
    {
        _issueRepository = issueRepository;
        _messenger = messenger;
    }

    public override string Command => "backtofolder";
    public override async Task Handle(Update update, BotStatus botStatus)
    {
        var folder = ExactData(update.CallbackQuery!.Data!);
        var folders = await _issueRepository.SelectDistinct(x => x.Status == IssueStatus.Started, x => x.Folder);
        var buttons = BotButtons.GetFolderButtons(folders, InlineKeyboardButton.WithCallbackData("Назад", $"fromfolder#{folder}"));

        await _messenger.EditMessage(
            botStatus.ChatId, 
            update.CallbackQuery!.Message!.MessageId,
            $"Всего директорий: {folders.Count}", buttons);
    }
}