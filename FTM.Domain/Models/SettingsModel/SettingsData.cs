using FTM.Domain.Models.Base;

namespace FTM.Domain.Models.SettingsModel;

public record SettingsData(
    int Id,
    bool IsDailyScheduleEnabled, 
    int? DailyScheduleHour, 
    int? DefaultRemindHours, 
    int? DefaultRemindMinutes) : BaseData(Id)
{
    
}