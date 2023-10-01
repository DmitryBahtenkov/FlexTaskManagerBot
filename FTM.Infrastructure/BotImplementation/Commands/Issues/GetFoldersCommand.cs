using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Commands.Issues;

public class GetFoldersCommand : TelegramCommandBase
{
    private readonly IRepository<Issue> _issueRepository;
    private readonly MessengerService _messenger;
    
    public GetFoldersCommand(IRepository<Issue> issueRepository, MessengerService messenger)
    {
        _issueRepository = issueRepository;
        _messenger = messenger;
    }

    public override string Command => "folders";
    
    public override async Task Handle(Update update, BotStatus botStatus)
    {
        botStatus.State = BotState.ListenTasks;
        var folders = await _issueRepository.SelectDistinct(
            x => x.Status == IssueStatus.Started,
            x => x.Folder
        );

        await _messenger.SendMessage(botStatus.ChatId, $"Всего директорий: {folders.Count}",
            BotButtons.GetFolderButtons(folders));
    }

    public override string? Description => "Просмотр папок";
}