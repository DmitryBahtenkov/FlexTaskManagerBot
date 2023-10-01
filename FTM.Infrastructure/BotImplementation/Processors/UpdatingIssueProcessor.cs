using FTM.Domain.Exceptions;
using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.Services;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Exceptions;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.Extensions;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Processors;

public class UpdatingIssueProcessor : IBotProcessor<Update>
{
    public UpdatingIssueProcessor(IUnitOfWork<Issue> issueUnit, MessengerService messengerService, MessageAttachmentService attachmentService, FieldSetterFactory<Issue> fieldSetterFactory, ICurrentUserService currentUserService)
    {
        _messengerService = messengerService;
        _attachmentService = attachmentService;
        _fieldSetterFactory = fieldSetterFactory;
        _currentUserService = currentUserService;
        _issueRepository = issueUnit.GetRepository();
    }

    public BotState State => BotState.UpdatingTask;

    private readonly IRepository<Issue> _issueRepository;
    private readonly MessengerService _messengerService;
    private readonly MessageAttachmentService _attachmentService;
    private readonly FieldSetterFactory<Issue> _fieldSetterFactory;
    private readonly ICurrentUserService _currentUserService;

    public async Task Process(Update update, BotStatus botStatus)
    {
        if (!botStatus.EntityId.HasValue)
        {
            throw new BusinessException("Не найдена задача");
        }

        var issue = await _issueRepository.ByIdAsync(botStatus.EntityId.Value);

        if (issue is null)
        {
            throw new BusinessException("Не найдена задача");
        }

        if (string.IsNullOrEmpty(botStatus.EditingField))
        {
            var markup = BotButtons.GetForEditIssue(issue);
            await _messengerService.SendMessage(botStatus.ChatId, "Выберите поле для редактирования", markup);
            return;
        }

        var timezone = _currentUserService.Timezone ?? 3;

        var data = update.Message!.Text!;

        var setter = _fieldSetterFactory.CreateSetter(botStatus.EditingField);
        try
        {
            if (data is null)
            {

                var attachment = _attachmentService.GetAttached(update.Message);

                setter.Set(issue, botStatus.EditingField, attachment, new Dictionary<string, object>
                {
                    {nameof(ICurrentUserService.Timezone), timezone}
                });
            }
            else
            {
                setter.Set(issue, botStatus.EditingField, data, new Dictionary<string, object>
                {
                    {nameof(ICurrentUserService.Timezone), timezone}
                });
            }
        }
        catch (BusinessException e)
        {
            var message = $"Ошибка: {e.Message}";
            throw new BotBusinessException(message, e, BotButtons.ForUpdateValidation(issue));
        }

        await _issueRepository.UpdateAsync(issue);
        botStatus.State = BotState.ListenTasks;

        await _messengerService.SendMessage(botStatus.ChatId, issue.ToBotString(), BotButtons.GetForIssue(issue));
    }
}