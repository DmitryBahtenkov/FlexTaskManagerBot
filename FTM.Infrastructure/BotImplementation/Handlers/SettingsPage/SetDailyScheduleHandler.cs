using FTM.Domain.Exceptions;
using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Services;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.BotImplementation.Services.ButtonStyling;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Handlers.SettingsPage;

public class SetDailyScheduleHandler : TelegramHandlerBase
{
    private readonly ICurrentUserService _currentUserService;
    private readonly MessengerService _messengerService;
    
    public SetDailyScheduleHandler(
        ICurrentUserService currentUserService,
        MessengerService messengerService)
    {
        _currentUserService = currentUserService;
        _messengerService = messengerService;
    }

    public override string Command => "setdaily";
    public override Task Handle(Update update, BotStatus botStatus)
    {
        var data = bool.Parse(ExactData(update.CallbackQuery!.Data!));
        var settings = _currentUserService.GetSettings();
        
        if (settings is null)
        {
            throw new BusinessException("Настройки пользователя не найдены");
        }

        settings.IsDailyScheduleEnabled = data;
        
        return _messengerService.EditMessage(
            botStatus.ChatId,
            update.CallbackQuery!.Message!.MessageId,
            "Настройки ежедневных напоминаний", SettingsButtons.ForDaily(settings, _currentUserService.Timezone ?? 3));
    }
}