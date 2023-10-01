using FTM.Domain.Exceptions;
using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.SettingsModel;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Handlers.SettingsPage;

public class TimezonePageHandler : TelegramHandlerBase
{
    public TimezonePageHandler(IRepository<Settings> settingsRepository, MessengerService messengerService)
    {
        _settingsRepository = settingsRepository;
        _messengerService = messengerService;
    }

    public override string Command => "timezonepage";

    private readonly IRepository<Settings> _settingsRepository;
    private readonly MessengerService _messengerService;
    
    public override async Task Handle(Update update, BotStatus botStatus)
    {
        var settings = await _settingsRepository.GetAsync(x => x.UserId == botStatus.UserId);

        if (settings is null)
        {
            throw new BusinessException("Настройки пользователя не найдены");
        }

        var buttons = BotButtons.ForTimezone(settings);

        await _messengerService.EditMessage(
            botStatus.ChatId,
            update.CallbackQuery!.Message!.MessageId,
            "Изменить таймзону", buttons);
    }
}