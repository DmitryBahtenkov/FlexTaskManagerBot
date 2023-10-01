using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Handlers.Issues;

public class DeleteIssueHandler : TelegramHandlerBase
{
    private readonly IRepository<Issue> _issueRepository;
    private readonly IUnitOfWork<Issue> _issueUnit;
    private readonly MessengerService _messengerService;

    public DeleteIssueHandler(IUnitOfWork<Issue> issueUnit, MessengerService messengerService)
    {
        _issueUnit = issueUnit;
        _messengerService = messengerService;
        _issueRepository = issueUnit.GetRepository();
    }

    public override string Command => "delete";
    public override async Task Handle(Update update, BotStatus botStatus)
    {
        var id = int.Parse(ExactData(update.CallbackQuery!.Data!));
        var entity = await _issueRepository.ByIdAsync(id);

        if (entity is null)
        {
            return;
        }

        await _issueRepository.DeleteAsync(entity);
        await _issueUnit.SaveChangesAsync();

        await _messengerService.EditMessage(botStatus.ChatId, update.CallbackQuery!.Message!.MessageId,
            "|Задача удалена без возможности восстановления|", BotButtons.ForDelete());
    }
}