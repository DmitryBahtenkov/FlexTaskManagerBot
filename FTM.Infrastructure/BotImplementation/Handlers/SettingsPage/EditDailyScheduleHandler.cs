using FTM.Domain.Exceptions;
using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.SettingsModel;
using FTM.Domain.Services;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.BotImplementation.Services.ButtonStyling;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Handlers.SettingsPage;

public class EditDailyScheduleHandler : TelegramHandlerBase
{
    private readonly MessengerService _messengerService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IRepository<Settings> _settingsRepository;

    public EditDailyScheduleHandler(
        MessengerService messengerService,
        ICurrentUserService currentUserService, IRepository<Settings> settingsRepository)
    {
        _messengerService = messengerService;
        _currentUserService = currentUserService;
        _settingsRepository = settingsRepository;
    }

    public override string Command => "setdailytime";


    public override Task Handle(Update update, BotStatus botStatus)
    {
        var data = int.Parse(ExactData(update.CallbackQuery!.Data!));
        var settings = _currentUserService.GetSettings();
        
        if (settings is null)
        {
            throw new BusinessException("Настройки пользователя не найдены");
        }

        settings.DailyScheduleHour = data - (_currentUserService.Timezone ?? 3);
        
        return _messengerService.EditMessage(
            botStatus.ChatId,
            update.CallbackQuery!.Message!.MessageId,
            "Настройки ежедневных напоминаний", SettingsButtons.ForDaily(settings, _currentUserService.Timezone ?? 3));
    }
}