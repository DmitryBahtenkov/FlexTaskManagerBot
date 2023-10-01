using FTM.Domain.Exceptions;
using FTM.Domain.Models.BotStatusModel;
using FTM.Domain.Models.SettingsModel;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Base;
using FTM.Infrastructure.BotImplementation.Services;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Handlers.SettingsPage;

public class EditTimezoneHandler : TelegramHandlerBase
{
    private readonly IRepository<Settings> _settingsRepository;
    private readonly MessengerService _messengerService;

    public EditTimezoneHandler(IRepository<Settings> settingsRepository, MessengerService messengerService)
    {
        _settingsRepository = settingsRepository;
        _messengerService = messengerService;
    }

    public override string Command => "settimezone";
    public override async Task Handle(Update update, BotStatus botStatus)
    {
        var timezone = int.Parse(ExactData(update.CallbackQuery!.Data!));
        var settings = await GetUserSettings(botStatus);

        if (settings.Timezone == timezone)
        {
            return;
        }
        
        settings.Timezone = timezone;

        var buttons = BotButtons.ForTimezone(settings);

        await _messengerService.EditMessage(botStatus.ChatId, 
            update.CallbackQuery!.Message!.MessageId,
            "Изменить таймзону", buttons);
    }

    private async Task<Settings> GetUserSettings(BotStatus botStatus)
    {
        Settings? settings = default;
        if (botStatus.User is not null)
        {
            settings = botStatus.User.Settings.FirstOrDefault();
        }
        else
        {
            settings = await _settingsRepository.GetAsync(x => x.UserId == botStatus.UserId);
        }

        return settings ?? throw new BusinessException("Не найдены настройки пользователя");
    }
}