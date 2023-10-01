using FTM.Domain.Models.SettingsModel;
using Telegram.Bot.Types.ReplyMarkups;

namespace FTM.Infrastructure.BotImplementation.Services.ButtonStyling;

public static class SettingsButtons
{
    public static InlineKeyboardMarkup ForDaily(Settings settings, int timezone)
    {
        var checkboxString = "Ежедневный список задач: " + 
                             (settings.IsDailyScheduleEnabled ? "✔️" : "✖️");
        var checkbox = 
            InlineKeyboardButton.WithCallbackData(checkboxString, 
                $"setdaily#{!settings.IsDailyScheduleEnabled}");

        var buttons = new List<IEnumerable<InlineKeyboardButton>>(2) { new []{checkbox} };
        
        if (settings.IsDailyScheduleEnabled)
        {
            var currentHours = settings.GetDailyScheduleHour(timezone);
            
            var timeButtons = new List<InlineKeyboardButton>(3);

            if (currentHours > 0)
            {
                timeButtons.Add(InlineKeyboardButton.WithCallbackData("<", $"setdailytime#{currentHours - 1}"));
            }
            
            timeButtons.Add(InlineKeyboardButton.WithCallbackData($"{currentHours}:00", "nocall#"));

            if (currentHours < 24)
            {
                timeButtons.Add(InlineKeyboardButton.WithCallbackData(">", $"setdailytime#{currentHours + 1}"));
            }
            
            buttons.Add(timeButtons);
        }
        
        buttons.Add(new []{InlineKeyboardButton.WithCallbackData("Назад", "backtosettings#")});

        return new InlineKeyboardMarkup(buttons);
    }
}