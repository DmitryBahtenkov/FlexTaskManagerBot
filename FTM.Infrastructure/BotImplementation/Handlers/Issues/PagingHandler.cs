using System.Linq.Expressions;
using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.BotImplementation.Services.ButtonStyling;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Handlers.Issues;

public class PagingHandler : TelegramHandlerBase
{
    public PagingHandler(IRepository<Issue> issueRepository, MessengerService messenger)
    {
        _issueRepository = issueRepository;
        _messenger = messenger;
    }

    public override string Command => "taskpaging";

    private readonly IRepository<Issue> _issueRepository;
    private readonly MessengerService _messenger;

    public override async Task Handle(Update update, BotStatus botStatus)
    {
        var page = int.Parse(ExactData(update.CallbackQuery!.Data!));
        var filter = botStatus.Filter?.GetExpressions() ?? new List<Expression<Func<Issue, bool>>>(0);
        var result = await _issueRepository.Page(filter, page);
        
        var markup = BotPagingButtons.GetForPaging(result);
        await _messenger.EditMessage(botStatus.ChatId, update.CallbackQuery!.Message!.MessageId,$"Всего элементов: {result.TotalItems}", markup);
    }
}