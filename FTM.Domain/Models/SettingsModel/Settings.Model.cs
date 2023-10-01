using System.ComponentModel.DataAnnotations.Schema;
using FTM.Domain.Models.Base;
using FTM.Domain.Models.UserModel;

namespace FTM.Domain.Models.SettingsModel;

public partial class Settings : BaseEntity
{
    public bool IsDailyScheduleEnabled { get; set; }
    public int? DailyScheduleHour { get; set; }
    public int? DefaultRemindHours { get; set; }
    public int? DefaultRemindMinutes { get; set; }
    public int? Timezone { get; set; }
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }
    
    public override BaseData ToData()
    {
        return new SettingsData(
            Id, 
            IsDailyScheduleEnabled, 
            DailyScheduleHour, 
            DefaultRemindHours,
            DefaultRemindMinutes);
    }
}