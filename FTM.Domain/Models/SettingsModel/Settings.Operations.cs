namespace FTM.Domain.Models.SettingsModel;

public partial class Settings
{
    public static Settings Default => new Settings
    {
        IsDailyScheduleEnabled = true,
        DailyScheduleHour = 10,
        DefaultRemindHours = 10,
        DefaultRemindMinutes = 30,
        Timezone = 3
    };

    public int GetDailyScheduleHour(int timezone)
    {
        var utcTime = DailyScheduleHour ?? 7;

        return utcTime + timezone;
    }
}