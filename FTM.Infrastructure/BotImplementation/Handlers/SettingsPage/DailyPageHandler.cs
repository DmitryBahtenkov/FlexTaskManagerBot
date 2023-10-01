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

public class DailyPageHandler : TelegramHandlerBase
{
    private readonly MessengerService _messengerService;
    private readonly ICurrentUserService _currentUserService;

    public DailyPageHandler(MessengerService messengerService, ICurrentUserService currentUserService)
    {
        _messengerService = messengerService;
        _currentUserService = currentUserService;
    }

    public override string Command => "dailypage";
    public override async Task Handle(Update update, BotStatus botStatus)
    {
        var settings = _currentUserService.GetSettings();

        if (settings is null)
        {
            throw new BusinessException("Настройки пользователя не найдены");
        }

        var buttons = SettingsButtons.ForDaily(settings, _currentUserService.Timezone ?? 3);

        await _messengerService.EditMessage(
            botStatus.ChatId,
            update.CallbackQuery!.Message!.MessageId,
            "Настройки ежедневных напоминаний", buttons);
    }
}