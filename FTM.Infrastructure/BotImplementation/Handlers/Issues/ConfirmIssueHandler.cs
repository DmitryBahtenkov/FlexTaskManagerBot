using FTM.Domain.Exceptions;
using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.IssueFileModel;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.Models.IssueModel.DTO;
using FTM.Domain.Services;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.Extensions;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Handlers.Issues;

public class ConfirmIssueHandler : TelegramHandlerBase
{
    private readonly IRepository<Issue> _issueRepository;
    private readonly IUnitOfWork<Issue> _issueUnit;
    private readonly ICacheService _cacheService;
    private readonly ICurrentUserService _currentUserService;
    private readonly MessengerService _messengerService;

    public ConfirmIssueHandler(
        ICacheService cacheService,
        ICurrentUserService currentUserService,
        MessengerService messengerService,
        IUnitOfWork<Issue> issueUnit)
    {
        _issueRepository = issueUnit.GetRepository();
        _cacheService = cacheService;
        _currentUserService = currentUserService;
        _messengerService = messengerService;
        _issueUnit = issueUnit;
    }

    public override string Command => "confirm";
    public override async Task Handle(Update update, BotStatus botStatus)
    {
        var tempId = ExactData(update.GetCallbackData(), 1);
        var variantIndex = int.Parse(ExactData(update.GetCallbackData(), 2) ?? "");

        var value = await _cacheService.GetEntry<TempIssueDto>(tempId ?? "");

        if (value is null)
        {
            throw new BusinessException("Что-то пошло не так");
        }

        if (variantIndex != -1)
        {
            var variant = value.Variants[variantIndex];

            value.IssueForSaving.Text = variant.Text;
            if (variant.HasTime)
            {
                value.IssueForSaving.RemindTime = variant.Date;
            }
            else
            {
                value.IssueForSaving.RemindTime = variant.Date.AddHours(7);
            }
        }

        var issue = await _issueRepository.AddAsync(value.IssueForSaving);
        await _issueUnit.SaveChangesAsync();

        await _messengerService.EditMessage(
            botStatus.ChatId,
            update.GetMessageId(),
            issue.ToBotString(),
            BotButtons.GetForIssue(issue));
    }
}